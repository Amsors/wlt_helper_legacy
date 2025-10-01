using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using wlt_helper.Utilities;

namespace wlt_helper.Services
{
    internal class DataStorage
    {
        private static readonly byte[] s_additionalEntropy = Encoding.UTF8.GetBytes("wlt_helper_legacy_amsors_CC9F8F27-3A8A-4643-8831-861B72C7771A");
        public static bool SaveCredentials(string username, string password)
        {
            try
            {
                string credentials = $"{username}|{password}";

                byte[] plaintextBytes = Encoding.UTF8.GetBytes(credentials);
                byte[] encryptedData = ProtectedData.Protect(plaintextBytes, s_additionalEntropy, DataProtectionScope.CurrentUser);

                string filePath = AppConfig.path_Credential;
                System.IO.File.WriteAllBytes(filePath, encryptedData);

                TbxLogger.LogWrite($"保存凭据成功");
                return true;
            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"保存凭据时出错：{ex.Message}");
                return false;
            }
        }
        public static string LoadSavedCredentials()
        {
            string filePath = AppConfig.path_Credential;
            if (!System.IO.File.Exists(filePath))
            {
                TbxLogger.LogWrite($"加载credential时出错 文件不存在");
                return null;
            }

            try
            {
                byte[] encryptedData = System.IO.File.ReadAllBytes(filePath);
                byte[] decryptedData = ProtectedData.Unprotect(encryptedData, s_additionalEntropy, DataProtectionScope.CurrentUser);

                string credentials = Encoding.UTF8.GetString(decryptedData);

                TbxLogger.LogWrite($"读取本地credential成功");
                return credentials;
            }
            catch (CryptographicException)
            {
                System.IO.File.Delete(filePath);
                TbxLogger.LogWrite($"加载credential时出错 保存的登录信息已失效");
            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"加载credential时出错 {ex.Message}");
            }
            return null;
        }

        public static (string user, string pwd) GetUserPwd()
        {
            string user_pwd = LoadSavedCredentials();
            if (user_pwd != null)
            {
                string[] parts = user_pwd.Split('|');
                if (parts.Length == 2)
                {
                    return (parts[0], parts[1]);
                }
            }
            return ("NA", "NA");
        }
    }
}
