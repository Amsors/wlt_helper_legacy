using System;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;
using wlt_helper.Services;
using wlt_helper.Utilities;

namespace wlt_helper_legacy
{
    public partial class MainForm : Form
    {
        private bool isPasswordVisible = false;
        private bool isMainFormVisible = true;

        public MainForm()
        {
            InitializeComponent();
            ConfigInitialize();
            MainFormInitialize();
            NotifyIconInitialize(); 
            LaunchCronJobs();
            TbxLogger.Initialize(this.txt_StatusBox);
        }

        private void ConfigInitialize()
        {
            if (wlt_helper.Services.AppSettings.ExistConfigFile() == false)
            {
                wlt_helper.Services.UserConfig.LaunchOnBoot = false;
                wlt_helper.Services.UserConfig.HideOnLaunch = false;

                wlt_helper.Services.AppSettings.SetConfigFile();
            }
            else
            {
                string conf = wlt_helper.Services.AppSettings.ReadConfigFile();
                using (JsonDocument document = JsonDocument.Parse(conf))
                {
                    JsonElement root = document.RootElement;

                    bool launchOnBoot = root.GetProperty("_launchOnBoot").GetBoolean();
                    bool hideOnLaunch = root.GetProperty("_hideOnLaunch").GetBoolean();

                    wlt_helper.Services.UserConfig.LaunchOnBoot = launchOnBoot;
                    wlt_helper.Services.UserConfig.HideOnLaunch = hideOnLaunch;

                    ckb_LaunchOnBoot.Checked = launchOnBoot;
                    wlt_helper.Services.AppSettings.SetAutoStart();

                    ckb_HideOnLaunch.Checked = hideOnLaunch;
                }
            }
        }

        private void MainFormInitialize()
        {
            byte[] iconBytes = wlt_helper.Services.AppSettings.GetIconBytes();
            Icon myIcon = wlt_helper.Services.AppSettings.BytesToIcon(iconBytes);
            this.Icon = myIcon;
            this.Text = "wlt_helper";
            btn_Submit.Text = "确认";
            btn_TogglePassword.Text = "显示密码";
            txt_Password.PasswordChar = '*';
            lbl_Password.Text = "密码";
            lbl_UserName.Text = "用户名";
            btn_TestURL.Text = "测试网络";
            lbl_SSID.Text = "当前WLAN的SSID";
            lbl_Title.Text = "网络通助手";
            ckb_LaunchOnBoot.Text = "开机自启动";
            btn_Login.Text = "尝试登录";
            btn_ExitApp.Text = "退出程序";
            ckb_HideOnLaunch.Text = "启动自动托盘";
            txt_SSID.Text = "N/A"; //TODO 待删除
            tsmi_Exit.Text = "退出工具";
            btn_Uninstall.Text = "卸载工具";

            this.FormClosing += MainForm_FormClosing;
            this.Load += MainForm_Load;
        }

        private void NotifyIconInitialize()
        {
            byte[] iconBytes = wlt_helper.Services.AppSettings.GetIconBytes();
            Icon myIcon = wlt_helper.Services.AppSettings.BytesToIcon(iconBytes);
            this.notifyIcon.Icon = myIcon;
            notifyIcon.Text = "wlt_helper";
            notifyIcon.Visible = true;
        }

        private void LaunchCronJobs()
        {
            wlt_helper.Services.CronJob job1 = new wlt_helper.Services.CronJob();
            wlt_helper.Services.TimerTask Task1 = new wlt_helper.Services.TimerTask(job1.ConnectToWlt, wlt_helper.Services.AppConfig.time_ScanNetworkAvaidability);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TbxLogger.LogWrite("应用启动");
            if (wlt_helper.Services.UserConfig.HideOnLaunch)
            {
                TbxLogger.LogWrite("窗口隐藏");
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.isMainFormVisible = false;
            }
            string user_pwd = wlt_helper.Services.DataStorage.LoadSavedCredentials();
            if (user_pwd != null)
            {
                string[] parts = user_pwd.Split('|');
                if (parts.Length == 2)
                {
                    txt_UserName.Text = parts[0];
                    txt_Password.Text = parts[1];
                    TbxLogger.LogWrite("读取并加载用户名与密码成功");
                    return;
                }
            }
            TbxLogger.LogWrite("读取并加载用户名与密码失败");

            //string ssid = WltWebFunction.GetCurrentConnection();
            //if (ssid != null)
            //{
            //    txt_SSID.Text = ssid;
            //}
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                TbxLogger.LogWrite("窗口关闭");
                e.Cancel = true;
                this.isMainFormVisible = false;
                this.Hide();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.isMainFormVisible) return;
            TbxLogger.LogWrite("窗口出现");
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Activate();
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            string userInput = txt_UserName.Text;
            string pwdInput = txt_Password.Text;

            if (string.IsNullOrEmpty(userInput) || string.IsNullOrEmpty(pwdInput))
            {
                TbxLogger.LogWrite("用户名和密码输入无效");
                MessageBox.Show("请检查输入内容！");
            }
            else
            {
                TbxLogger.LogWrite("用户名和密码输入成功");
                bool success = wlt_helper.Services.DataStorage.SaveCredentials(userInput, pwdInput);
                if (success)
                {
                    TbxLogger.LogWrite("用户名和密码保存成功");
                    MessageBox.Show($"已保存");
                }
                else
                {
                    TbxLogger.LogWrite("用户名和密码保存失败");
                    MessageBox.Show($"保存失败");
                }
            }
        }

        private void btn_TogglePassword_Click(object sender, EventArgs e)
        {
            if (isPasswordVisible)
            {
                txt_Password.PasswordChar = '*';
                btn_TogglePassword.Text = "显示密码";
                isPasswordVisible = false;
            }
            else
            {
                txt_Password.PasswordChar = '\0';
                btn_TogglePassword.Text = "隐藏密码";
                isPasswordVisible = true;
            }
        }

        private async void btn_TestURL_Click(object sender, EventArgs e)
        {
            using (var webFunction = new wlt_helper.Services.WltWebFunction())
            {
                string testUrl = wlt_helper.Services.AppConfig.url_DefaultConnectTest;
                bool isAccessible = await webFunction.TestWebsiteAccessAsync(testUrl);
                string content = $"网站 {testUrl} {(isAccessible ? "可访问" : "不可访问")}";
                TbxLogger.LogWrite(content);
            }
        }

        private async void btn_Login_Click(object sender, EventArgs e)
        {
            using (var webFunction = new wlt_helper.Services.WltWebFunction())
            {
                await webFunction.LoginToWlt();
            }
        }

        private void ckb_LaunchOnBoot_CheckedChanged(object sender, EventArgs e)
        {
            wlt_helper.Services.UserConfig.LaunchOnBoot = ckb_LaunchOnBoot.Checked;
            wlt_helper.Services.AppSettings.SetAutoStart();
        }

        private void btn_ExitApp_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private void ckb_HideOnLaunch_CheckedChanged(object sender, EventArgs e)
        {
            TbxLogger.LogWrite($"设定启动后自动隐藏到托盘为{ckb_HideOnLaunch.Checked}");
            if (ckb_HideOnLaunch.Checked)
            {
                wlt_helper.Services.UserConfig.HideOnLaunch = true;
            }
            else
            {
                wlt_helper.Services.UserConfig.HideOnLaunch = false;
            }
        }

        private void tsmi_Exit_Click(object sender, EventArgs e)
        {
            ExitApp();
        }

        private void btn_Uninstall_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                $"确定要卸载此工具吗？{Environment.NewLine}此操作将会删除以下三个文件：{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"程序本体 {AppConfig.path_Exe}{Environment.NewLine}" +
                $"程序配置文件 {AppConfig.path_Config}{Environment.NewLine}" +
                $"本地保存的登录凭据 {AppConfig.path_Credential}{Environment.NewLine}",
                "卸载确认",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result == DialogResult.Yes)
            {
                TbxLogger.LogWrite("确认卸载应用");
                AppSettings.UninstallApp();
                ExitApp();
            }
            else
            {
                TbxLogger.LogWrite("取消卸载应用");
            }
        }

        private void ExitApp()
        {
            this.notifyIcon.Dispose();
            this.cms_notifyIcon.Dispose();
            TbxLogger.Shutdown();
            Application.Exit();
        }
    }
}
