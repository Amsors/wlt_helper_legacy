using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.Json;
using IWshRuntimeLibrary;
using wlt_helper.Utilities;

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
                TbxLogger.LogWrite("尝试启用开机自启动");
                return EnableAutoStart();
            }
            else
            {
                TbxLogger.LogWrite("尝试禁用开机自启动");
                return DisableAutoStart();
            }
        }

        private static bool EnableAutoStart(string shortcutName = "MyApp", string description = "Default")
        {
            try
            {
                string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                DebugLogger.LogWrite($"{shortcutName}: {startupPath}");
                string shortcutPath = System.IO.Path.Combine(startupPath, $"{shortcutName}.lnk");
                string appPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                if (appPath == null || shortcutPath == null)
                {
                    TbxLogger.LogWrite("启用开机自启动，错误 应用程序或快捷方式路径错误");
                    return false;
                }

                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

                shortcut.TargetPath = appPath;
                shortcut.WorkingDirectory = System.IO.Path.GetDirectoryName(appPath);
                shortcut.Description = description;
                //shortcut.IconLocation = "icon.ico, 0"; // TODO 添加shortcut图标

                shortcut.Save();
                TbxLogger.LogWrite("启用开机自启动成功");
                return true;
            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"启用开机自启动失败，错误 {ex.Message}");
                return false;
            }
        }

        private static bool DisableAutoStart(string shortcutName = "MyApp", string description = "Default")
        {
            try
            {
                string startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                DebugLogger.LogWrite($"{shortcutName}: {startupPath}");
                string shortcutPath = System.IO.Path.Combine(startupPath, $"{shortcutName}.lnk");
                string appPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                if(appPath == null || shortcutPath == null)
                {
                    TbxLogger.LogWrite("禁用开机自启动，错误 应用程序或快捷方式路径错误");
                    return false;
                }

                if (System.IO.File.Exists(shortcutPath))
                {
                    System.IO.File.Delete(shortcutPath);
                }
                else
                {
                    TbxLogger.LogWrite("禁用开机自启动，错误 应用程序快捷方式不存在");
                    return false;
                }
                TbxLogger.LogWrite("启用开机自启动成功");
                return true;
            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"禁用开机自启动失败，错误 {ex.Message}");
                return false;
            }
        }

        public static byte[] GetIconBytes()
        {
            var icon = wlt_helper_legacy.Properties.Resources.icon_1_16x16;
            if( icon == null)
            {
                TbxLogger.LogWrite("图标文件读取失败");
            }
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
                TbxLogger.LogWrite($"写入配置文件发生错误：{ex.Message}");
            }
        }

        public static string ReadConfigFile()
        {
            try
            {
                string confJson = System.IO.File.ReadAllText(AppConfig.path_Config);
                return confJson;
            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"读取配置文件发生错误：{ex.Message}");
            }
            return string.Empty;
        }
    }

    public static class AppConfig
    {
        public static readonly string path_Config = @"wlt_helper_config.json";
        public static readonly string url_DefaultConnectTest = @"http://cn.bing.com";
        public static readonly string url_MSConnectTest = @"http://www.msftconnecttest.com/connecttest.txt";
        public static readonly string url_BaiduConnectTest = @"http://baidu.com";
        public static readonly string url_BingConnectTest = @"http://cn.bing.com";
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
            try
            {
                string plainJson = JsonSerializer.Serialize(data);
                string formattedJson = JsonSerializer.Serialize(
                JsonSerializer.Deserialize<object>(plainJson), new JsonSerializerOptions { WriteIndented = true });
                return formattedJson;
            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"Json转字符串错误：{ex.Message}");
                return string.Empty;
            }
        }
    }
}
