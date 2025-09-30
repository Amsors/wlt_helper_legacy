using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace wlt_helper.Services
{
    internal class DataStorage
    {
        private static readonly byte[] s_additionalEntropy = Encoding.UTF8.GetBytes("asdasd");
        public static void SaveCredentials(string username, string password)
        {
            try
            {
                // 1. 将用户名和密码组合为一个字符串（或使用更结构化的方式，如JSON）
                string credentials = $"{username}|{password}";
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(credentials);

                // 2. 使用DPAPI加密数据。DataProtectionScope.CurrentUser 确保只有当前用户能解密。
                byte[] encryptedData = ProtectedData.Protect(plaintextBytes, s_additionalEntropy, DataProtectionScope.CurrentUser);

                // 3. 将加密后的数据保存到文件（也可存到注册表）
                string filePath = "credential";
                System.IO.File.WriteAllBytes(filePath, encryptedData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"保存凭据时出错：{ex.Message}");
            }
        }
        public static string LoadSavedCredentials()
        {
            string filePath = "credential";
            if (!System.IO.File.Exists(filePath)) return null;

            try
            {
                byte[] encryptedData = System.IO.File.ReadAllBytes(filePath);
                byte[] decryptedData = ProtectedData.Unprotect(encryptedData, s_additionalEntropy, DataProtectionScope.CurrentUser);

                string credentials = Encoding.UTF8.GetString(decryptedData);
                return credentials;
            }
            catch (CryptographicException)
            {
                System.IO.File.Delete(filePath);
                Debug.WriteLine("保存的登录信息已失效，请重新输入。");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"加载凭据时出错：{ex.Message}");
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
