using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net.Http;
using wlt_helper.Utilities;

namespace wlt_helper.Services
{
    public class WltWebFunction:IDisposable
    {
        public static bool isLogined = false;

        private readonly HttpClient _httpClient;
        private bool _disposed = false; // 资源是否已被释放

        public WltWebFunction()
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(3) // 设置默认超时时间为3秒
            };
        }

        public async Task<bool> TestWebsiteAccessAsync(string url)
        {
            try
            {
                using (var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(3)))
                {
                    HttpResponseMessage response = await _httpClient.GetAsync(url, cts.Token);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (TaskCanceledException)
            {
                TbxLogger.LogWrite($"访问 {url} 超时（3秒内未响应）");
                return false;
            }
            catch (HttpRequestException ex)
            {
                TbxLogger.LogWrite($"访问 {url} 时发生网络错误：{ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"访问 {url} 时发生未知错误：{ex.Message}");
                return false;
            }
        }
        public async Task<string> PostFormAsync(string url, Dictionary<string, string> formData, string charSet = "utf-8")
        {
            try
            {
                var formContent = new FormUrlEncodedContent(formData);
                formContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage response = await _httpClient.PostAsync(url, formContent);
                response.EnsureSuccessStatusCode();

                var customEncoding = System.Text.CodePagesEncodingProvider.Instance.GetEncoding(charSet);
                byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();
                if(customEncoding == null)
                {
                    TbxLogger.LogWrite($"所选字符集{charSet}不可用");
                    return "N/A";
                }
                string responseString = customEncoding.GetString(responseBytes);
                if (responseString == null)
                {
                    TbxLogger.LogWrite($"返回响应字符串为空");
                    return "N/A";
                }

                return responseString;
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException($"向 {url} 发送POST请求超时");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"向 {url} 发送POST请求时发生网络错误：{ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"向 {url} 发送POST请求时发生未知错误：{ex.Message}");
            }
        }

        public static async Task<bool> PingWebsiteAsync(string url)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = await ping.SendPingAsync(url, 1000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"Ping操作发生异常: {ex.Message}");
                return false;
            }
        }

        public async Task LoginToWlt()
        {
            string url = AppConfig.url_WltLogin;
            (string user, string pwd) = DataStorage.GetUserPwd();
            if(user == null || pwd == null)
            {
                TbxLogger.LogWrite("用户名或密码为空，网络通登录失败");
                return;
            }
            var formData = new Dictionary<string, string>
            {
                { "name", user },
                { "password", pwd },
                {"cmd", "login" },
                {"url", "URL" },
                {"set", "%D2%BB%BC%FC%C9%CF%CD%F8" } // 一键登录
            };

            try
            {
                string response = await PostFormAsync(url, formData, "GB2312");
                if (response.Contains("网络设置成功"))
                {
                    TbxLogger.LogWrite("登录成功");
                }
                else
                {
                    TbxLogger.LogWrite("登录失败 网页返回内容无法解析");
                }

            }
            catch (Exception ex)
            {
                TbxLogger.LogWrite($"登录失败 {ex.Message}");
            }
        }

        public static bool NeedToLogin(string ssid)
        {
            if (ssid == AppConfig.SSID_Target)
            {
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                    
                    DebugLogger.LogWrite("HttpClient 已释放");
                }
                _disposed = true;
            }
        }
        ~WltWebFunction()
        {
            Dispose(false);
        }
    }
}
