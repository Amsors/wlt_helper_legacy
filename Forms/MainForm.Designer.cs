using System.Drawing;
using System.Windows.Forms;

namespace wlt_helper_legacy
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txt_UserName = new System.Windows.Forms.TextBox();
            this.btn_Submit = new System.Windows.Forms.Button();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.btn_TogglePassword = new System.Windows.Forms.Button();
            this.lbl_UserName = new System.Windows.Forms.Label();
            this.lbl_Password = new System.Windows.Forms.Label();
            this.btn_TestURL = new System.Windows.Forms.Button();
            this.lbl_SSID = new System.Windows.Forms.Label();
            this.txt_SSID = new System.Windows.Forms.TextBox();
            this.lbl_Title = new System.Windows.Forms.Label();
            this.txt_StatusBox = new System.Windows.Forms.TextBox();
            this.ckb_LaunchOnBoot = new System.Windows.Forms.CheckBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btn_ExitApp = new System.Windows.Forms.Button();
            this.ckb_HideOnLaunch = new System.Windows.Forms.CheckBox();
            this.cms_notifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_Uninstall = new System.Windows.Forms.Button();
            this.cms_notifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_UserName
            // 
            this.txt_UserName.Location = new System.Drawing.Point(236, 139);
            this.txt_UserName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_UserName.MaxLength = 128;
            this.txt_UserName.Name = "txt_UserName";
            this.txt_UserName.Size = new System.Drawing.Size(267, 25);
            this.txt_UserName.TabIndex = 0;
            // 
            // btn_Submit
            // 
            this.btn_Submit.Location = new System.Drawing.Point(545, 136);
            this.btn_Submit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(133, 29);
            this.btn_Submit.TabIndex = 1;
            this.btn_Submit.Text = "button1";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(236, 197);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_Password.MaxLength = 128;
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.Size = new System.Drawing.Size(267, 25);
            this.txt_Password.TabIndex = 2;
            // 
            // btn_TogglePassword
            // 
            this.btn_TogglePassword.Location = new System.Drawing.Point(545, 196);
            this.btn_TogglePassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_TogglePassword.Name = "btn_TogglePassword";
            this.btn_TogglePassword.Size = new System.Drawing.Size(133, 29);
            this.btn_TogglePassword.TabIndex = 3;
            this.btn_TogglePassword.Text = "button2";
            this.btn_TogglePassword.UseVisualStyleBackColor = true;
            this.btn_TogglePassword.Click += new System.EventHandler(this.btn_TogglePassword_Click);
            // 
            // lbl_UserName
            // 
            this.lbl_UserName.AutoSize = true;
            this.lbl_UserName.Location = new System.Drawing.Point(236, 121);
            this.lbl_UserName.Name = "lbl_UserName";
            this.lbl_UserName.Size = new System.Drawing.Size(55, 15);
            this.lbl_UserName.TabIndex = 4;
            this.lbl_UserName.Text = "label1";
            // 
            // lbl_Password
            // 
            this.lbl_Password.AutoSize = true;
            this.lbl_Password.Location = new System.Drawing.Point(236, 180);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new System.Drawing.Size(55, 15);
            this.lbl_Password.TabIndex = 5;
            this.lbl_Password.Text = "label2";
            // 
            // btn_TestURL
            // 
            this.btn_TestURL.Location = new System.Drawing.Point(415, 269);
            this.btn_TestURL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_TestURL.Name = "btn_TestURL";
            this.btn_TestURL.Size = new System.Drawing.Size(89, 29);
            this.btn_TestURL.TabIndex = 6;
            this.btn_TestURL.Text = "button1";
            this.btn_TestURL.UseVisualStyleBackColor = true;
            this.btn_TestURL.Click += new System.EventHandler(this.btn_TestURL_Click);
            // 
            // lbl_SSID
            // 
            this.lbl_SSID.AutoSize = true;
            this.lbl_SSID.Location = new System.Drawing.Point(234, 293);
            this.lbl_SSID.Name = "lbl_SSID";
            this.lbl_SSID.Size = new System.Drawing.Size(55, 15);
            this.lbl_SSID.TabIndex = 7;
            this.lbl_SSID.Text = "label1";
            // 
            // txt_SSID
            // 
            this.txt_SSID.Location = new System.Drawing.Point(234, 318);
            this.txt_SSID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_SSID.Name = "txt_SSID";
            this.txt_SSID.ReadOnly = true;
            this.txt_SSID.Size = new System.Drawing.Size(149, 25);
            this.txt_SSID.TabIndex = 8;
            // 
            // lbl_Title
            // 
            this.lbl_Title.Font = new System.Drawing.Font("楷体", 50F);
            this.lbl_Title.Location = new System.Drawing.Point(187, 25);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new System.Drawing.Size(540, 86);
            this.lbl_Title.TabIndex = 9;
            this.lbl_Title.Text = "label1";
            this.lbl_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_StatusBox
            // 
            this.txt_StatusBox.Location = new System.Drawing.Point(150, 375);
            this.txt_StatusBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_StatusBox.Multiline = true;
            this.txt_StatusBox.Name = "txt_StatusBox";
            this.txt_StatusBox.ReadOnly = true;
            this.txt_StatusBox.Size = new System.Drawing.Size(615, 103);
            this.txt_StatusBox.TabIndex = 10;
            // 
            // ckb_LaunchOnBoot
            // 
            this.ckb_LaunchOnBoot.AutoSize = true;
            this.ckb_LaunchOnBoot.Location = new System.Drawing.Point(581, 276);
            this.ckb_LaunchOnBoot.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckb_LaunchOnBoot.Name = "ckb_LaunchOnBoot";
            this.ckb_LaunchOnBoot.Size = new System.Drawing.Size(101, 19);
            this.ckb_LaunchOnBoot.TabIndex = 11;
            this.ckb_LaunchOnBoot.Text = "checkBox1";
            this.ckb_LaunchOnBoot.UseVisualStyleBackColor = true;
            this.ckb_LaunchOnBoot.CheckedChanged += new System.EventHandler(this.ckb_LaunchOnBoot_CheckedChanged);
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(415, 318);
            this.btn_Login.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(89, 29);
            this.btn_Login.TabIndex = 12;
            this.btn_Login.Text = "button1";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cms_notifyIcon;
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // btn_ExitApp
            // 
            this.btn_ExitApp.Location = new System.Drawing.Point(801, 25);
            this.btn_ExitApp.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.btn_ExitApp.Name = "btn_ExitApp";
            this.btn_ExitApp.Size = new System.Drawing.Size(89, 29);
            this.btn_ExitApp.TabIndex = 13;
            this.btn_ExitApp.Text = "button1";
            this.btn_ExitApp.UseVisualStyleBackColor = true;
            this.btn_ExitApp.Click += new System.EventHandler(this.btn_ExitApp_Click);
            // 
            // ckb_HideOnLaunch
            // 
            this.ckb_HideOnLaunch.AutoSize = true;
            this.ckb_HideOnLaunch.Location = new System.Drawing.Point(581, 324);
            this.ckb_HideOnLaunch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckb_HideOnLaunch.Name = "ckb_HideOnLaunch";
            this.ckb_HideOnLaunch.Size = new System.Drawing.Size(101, 19);
            this.ckb_HideOnLaunch.TabIndex = 14;
            this.ckb_HideOnLaunch.Text = "checkBox1";
            this.ckb_HideOnLaunch.UseVisualStyleBackColor = true;
            this.ckb_HideOnLaunch.CheckedChanged += new System.EventHandler(this.ckb_HideOnLaunch_CheckedChanged);
            // 
            // cms_notifyIcon
            // 
            this.cms_notifyIcon.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cms_notifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Exit});
            this.cms_notifyIcon.Name = "contextMenuStrip1";
            this.cms_notifyIcon.Size = new System.Drawing.Size(228, 28);
            // 
            // tsmi_Exit
            // 
            this.tsmi_Exit.Name = "tsmi_Exit";
            this.tsmi_Exit.Size = new System.Drawing.Size(227, 24);
            this.tsmi_Exit.Text = "toolStripMenuItem1";
            this.tsmi_Exit.Click += new System.EventHandler(this.tsmi_Exit_Click);
            // 
            // btn_Uninstall
            // 
            this.btn_Uninstall.Location = new System.Drawing.Point(801, 75);
            this.btn_Uninstall.Name = "btn_Uninstall";
            this.btn_Uninstall.Size = new System.Drawing.Size(88, 29);
            this.btn_Uninstall.TabIndex = 16;
            this.btn_Uninstall.Text = "button1";
            this.btn_Uninstall.UseVisualStyleBackColor = true;
            this.btn_Uninstall.Click += new System.EventHandler(this.btn_Uninstall_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 503);
            this.Controls.Add(this.btn_Uninstall);
            this.Controls.Add(this.ckb_HideOnLaunch);
            this.Controls.Add(this.btn_ExitApp);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.ckb_LaunchOnBoot);
            this.Controls.Add(this.txt_StatusBox);
            this.Controls.Add(this.lbl_Title);
            this.Controls.Add(this.txt_SSID);
            this.Controls.Add(this.lbl_SSID);
            this.Controls.Add(this.btn_TestURL);
            this.Controls.Add(this.lbl_Password);
            this.Controls.Add(this.lbl_UserName);
            this.Controls.Add(this.btn_TogglePassword);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.btn_Submit);
            this.Controls.Add(this.txt_UserName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.cms_notifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txt_UserName;
        private Button btn_Submit;
        private TextBox txt_Password;
        private Button btn_TogglePassword;
        private Label lbl_UserName;
        private Label lbl_Password;
        private Button btn_TestURL;
        private Label lbl_SSID;
        private TextBox txt_SSID;
        private Label lbl_Title;
        private TextBox txt_StatusBox;
        private CheckBox ckb_LaunchOnBoot;
        private Button btn_Login;
        private NotifyIcon notifyIcon;
        private Button btn_ExitApp;
        private CheckBox ckb_HideOnLaunch;
        private ContextMenuStrip cms_notifyIcon;
        private ToolStripMenuItem tsmi_Exit;
        private Button btn_Uninstall;
    }
}

