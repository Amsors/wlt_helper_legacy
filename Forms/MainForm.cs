using System;
using System.Diagnostics;
using System.Drawing;
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
            InitializeConfig();
            MainFormInitialize();
            NotifyIconInitialize(); 
            TbxLogger.Initialize(this.txt_StatusBox);
            LaunchCronJobs();
        }

        private void InitializeConfig()
        {
            DataStorage.InitializeConfigFile();
            ckb_LaunchOnBoot.Checked = UserConfig.LaunchOnBoot;
            ckb_HideOnLaunch.Checked = UserConfig.HideOnLaunch;
            ckb_RepeatedCheck.Checked = UserConfig.RepeatedCheck;
            txt_CheckInterval.Text = UserConfig.CheckInterval.ToString();
            txt_InitialCheckMaxCnt.Text = UserConfig.InitialCheckMaxCnt.ToString();
            AppSettings.SetAutoStart();
        }

        private void MainFormInitialize()
        {
            byte[] iconBytes = AppSettings.GetIconBytes();
            Icon myIcon = AppSettings.BytesToIcon(iconBytes);
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
            tsmi_Exit.Text = "退出软件";
            btn_Uninstall.Text = "卸载软件";
            lbl_CheckInterval.Text = "检查间隔";
            lbl_InitialCheckMaxCnt.Text = "最大检查次数";
            ckb_RepeatedCheck.Text = "重复检查网络连通性";
            btn_SubmitSettings.Text = "保存设置";
            btn_Restart.Text = "重启软件";

            this.FormClosing += MainForm_FormClosing;
            this.Load += MainForm_Load;
        }

        private void NotifyIconInitialize()
        {
            byte[] iconBytes = AppSettings.GetIconBytes();
            Icon myIcon = AppSettings.BytesToIcon(iconBytes);
            this.notifyIcon.Icon = myIcon;
            notifyIcon.Text = "wlt_helper";
            notifyIcon.Visible = true;
        }

        private void LaunchCronJobs()
        {
            //CronJob job1 = new CronJob();
            //TimerTask Task1 = new TimerTask(job1.ConnectToWlt, UserConfig.CheckInterval);

            var timerManager2 = new TimerTaskManager(UserConfig.CheckInterval);
            timerManager2.AddTask(CronJob.ConnectToWlt);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TbxLogger.LogWrite("应用启动");
            if (UserConfig.HideOnLaunch)
            {
                TbxLogger.LogWrite("窗口隐藏");
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.isMainFormVisible = false;
            }
            string user_pwd = DataStorage.LoadSavedCredentials();
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
                bool success = DataStorage.SaveCredentials(userInput, pwdInput);
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
            using (var webFunction = new WltWebFunction())
            {
                string testUrl = AppConfig.url_DefaultConnectTest;
                bool isAccessible = await webFunction.TestWebsiteAccessAsync(testUrl);
                string content = $"网站 {testUrl} {(isAccessible ? "可访问" : "不可访问")}";
                TbxLogger.LogWrite(content);
            }
        }

        private async void btn_Login_Click(object sender, EventArgs e)
        {
            using (var webFunction = new WltWebFunction())
            {
                await webFunction.LoginToWlt();
            }
        }

        private void ckb_LaunchOnBoot_CheckedChanged(object sender, EventArgs e)
        {
            UserConfig.LaunchOnBoot = ckb_LaunchOnBoot.Checked;
            AppSettings.SetAutoStart();
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
                UserConfig.HideOnLaunch = true;
            }
            else
            {
                UserConfig.HideOnLaunch = false;
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
                $"本地保存的登录凭据 {AppConfig.path_Credential}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"窗口关闭后请耐心等待应用删除",
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

        private void txt_SSID_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbl_SSID_Click(object sender, EventArgs e)
        {

        }

        private void txt_InitialCheckMaxCnt_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_CheckInterval_TextChanged(object sender, EventArgs e)
        {

        }

        private void ckb_RepeatedCheck_CheckedChanged(object sender, EventArgs e)
        {
            TbxLogger.LogWrite($"设定重复检查为{ckb_RepeatedCheck.Checked}");
            if (ckb_RepeatedCheck.Checked)
            {
                UserConfig.RepeatedCheck = true;
            }
            else
            {
                UserConfig.RepeatedCheck = false;
            }
        }

        private void btn_SubmitSettings_Click(object sender, EventArgs e)
        {
            if (!uint.TryParse(txt_InitialCheckMaxCnt.Text, out uint number1) ||
                !uint.TryParse(txt_CheckInterval.Text, out uint number2) ||
                number1 == 0 || number2 == 0)
            {
                TbxLogger.LogWrite($"提交设置更改 输入不合法");
                MessageBox.Show("设置输入不合法");
            }
            else
            {
                if (number2 < 3000)
                {
                    TbxLogger.LogWrite($"提交设置更改 输入不合理");
                    MessageBox.Show("检查间隔设置不合理" +
                        Environment.NewLine +
                        "应不小于3000ms");
                }
                else
                {
                    TbxLogger.LogWrite($"提交设置更改 成功");
                    MessageBox.Show($"更改成功" +
                        $"{Environment.NewLine}重启软件后生效");
                    UserConfig.InitialCheckMaxCnt = number1;
                    UserConfig.CheckInterval = number2;
                }
            }
        }

        private void btn_Restart_Click(object sender, EventArgs e)
        {
            Process restartProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c timeout /t {AppConfig.time_Restart} && start \"\" \"{AppConfig.path_Exe}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            restartProcess.Start();

            ExitApp();
        }
    }
}
