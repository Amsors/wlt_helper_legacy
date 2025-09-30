using System;
using System.Diagnostics;
using System.Drawing;
using System.Text.Json;
using IWshRuntimeLibrary;

namespace wlt_helper.Services
{
    internal class AppSettings
    {
        //public static bool isAutostart;
        //public static bool isHideOnLaunch;

        public static bool SetAutoStart()
        {
            if (UserConfig.LaunchOnBoot)
            {
                EnableAutoStart();
            }
            else
            {
                DisableAutoStart();
            }
            return true;
        }

        private static bool EnableAutoStart(string shortcutName = "MyApp", string description = "Default")
        {
            try
            {
                string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                Debug.WriteLine(startupPath);
                string shortcutPath = System.IO.Path.Combine(startupPath, $"{shortcutName}.lnk");
                //string appPath = Process.GetCurrentProcess().MainModule.FileName;
                //string appPath = Environment.ProcessPath;
                string appPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                if (appPath == null)
                {
                    Debug.WriteLine("Error");
                    return false;
                }

                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                shortcut.TargetPath = appPath;
                shortcut.WorkingDirectory = System.IO.Path.GetDirectoryName(appPath);
                shortcut.Description = description;
                //shortcut.IconLocation = "icon.ico, 0";

                shortcut.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool DisableAutoStart(string shortcutName = "MyApp", string description = "Default")
        {
            try
            {
                string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                Debug.WriteLine(startupPath);
                string shortcutPath = System.IO.Path.Combine(startupPath, $"{shortcutName}.lnk");
                //string appPath = Process.GetCurrentProcess().MainModule.FileName;
                //string appPath = Environment.ProcessPath;
                string appPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                if (appPath == null)
                {
                    Debug.WriteLine("Error");
                    return false;
                }

                if (System.IO.File.Exists(shortcutPath))
                {
                    System.IO.File.Delete(shortcutPath);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] GetIconBytes()
        {
            return wlt_helper_legacy.Properties.Resources.icon_1_16x16;
        }

        public static Icon BytesToIcon(byte[] iconBytes)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(iconBytes))
            {
                return new Icon(ms);
            }
        }

        public static bool ExistConfigFile()
        {
            return System.IO.File.Exists(AppConfig.path_Config);
        }

        public static void SetConfigFile()
        {
            string directory = AppConfig.path_Config;
            string configJson = UserConfig.ToJsonString();
            try
            {
                System.IO.File.WriteAllText(directory, configJson);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"发生错误：{ex.Message}");
            }
        }

        public static string ReadConfigFile()
        {
            string confJson = System.IO.File.ReadAllText(AppConfig.path_Config);
            return confJson;
        }
    }

    public static class AppConfig
    {
        public static readonly string path_Config = @"wlt_helper_config.json";
        public static readonly string url_MSConnectTest = @"http://www.msftconnecttest.com/connecttest.txt";
        public static readonly string url_BaiduConnectTest = @"http://baidu.com";
        public static readonly string url_WltLogin = @"http://wlt.ustc.edu.cn/cgi-bin/ip";
        public static readonly string url_WltHost = @"wlt.ustc.edu.cn";
        public static readonly string SSID_Target = "ustcnet";
        public static readonly int time_ScanNetworkAvaidability = 3000;
    }

    internal static class UserConfig
    {
        private static bool _hideOnLaunch = false;
        public static bool HideOnLaunch
        {
            get
            {
                return _hideOnLaunch;
            }
            set
            {
                _hideOnLaunch = value;
                AppSettings.SetConfigFile();
            }
        }
        private static bool _launchOnBoot = false;
        public static bool LaunchOnBoot
        {
            get
            {
                return _launchOnBoot;
            }
            set
            {

                _launchOnBoot = value;
                AppSettings.SetConfigFile();
            }
        }
        public static string ToJsonString()
        {
            var data = new
            {
                _hideOnLaunch,
                _launchOnBoot
            };
            string plainJson = JsonSerializer.Serialize(data);
            string formattedJson = JsonSerializer.Serialize(
            JsonSerializer.Deserialize<object>(plainJson),
            new JsonSerializerOptions { WriteIndented = true }
        );
            return formattedJson;
        }
    }
}
