using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net.Http;

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
                Debug.WriteLine($"访问 {url} 超时（3秒内未响应）");
                return false;
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"访问 {url} 时发生网络错误：{ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"访问 {url} 时发生未知错误：{ex.Message}");
                return false;
            }
        }
        public async Task<string> PostFormAsync(string url, Dictionary<string, string> formData, string charSet = "utf-8")
        {
            try
            {
                // 将字典数据转换为application/x-www-form-urlencoded格式
                var formContent = new FormUrlEncodedContent(formData);
                // 设置请求头
                formContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                // 发送POST请求
                HttpResponseMessage response = await _httpClient.PostAsync(url, formContent);
                // 确保响应成功
                response.EnsureSuccessStatusCode();

                var customEncoding = System.Text.CodePagesEncodingProvider.Instance.GetEncoding(charSet);
                byte[] responseBytes = await response.Content.ReadAsByteArrayAsync();
                if(customEncoding == null)
                {
                    Debug.WriteLine($"{charSet} Encode is unavailable");
                    return "N/A";
                }
                string responseString = customEncoding.GetString(responseBytes);

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
                Debug.WriteLine($"Ping操作发生异常: {ex.Message}");
                return false;
            }
        }

        public async Task LoginToWlt()
        {
            string url = AppConfig.url_WltLogin;
            (string user, string pwd) = DataStorage.GetUserPwd();
            if(user == null || pwd == null)
            {
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
                Debug.WriteLine($"POST请求响应：{response}");
                if (response.Contains("网络设置成功"))
                {
                    Debug.WriteLine("登录成功");
                }
                else
                {
                    //mainform.OutputToStatusBox(" 失败");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"POST请求失败：{ex.Message}");
                Debug.WriteLine("登录失败");
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
                    Debug.WriteLine("托管资源（HttpClient）已释放。");
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
