using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wlt_helper.Services
{
    public class TimerTask
    {
        private System.Threading.Timer timer;
        public TimerTask(System.Threading.TimerCallback timercallback, int period = 1000)
        {
            timer = new System.Threading.Timer(timercallback, null, 0, period);
        }
    }
    internal class CronJob
    {
        public  async void ConnectToWlt(object state)
        {
            //string ssid = WltWebFunction.GetCurrentConnection();
            //if (WltWebFunction.NeedToLogin(ssid) == false)
            //{
            //    Debug.WriteLine("无需登录，SSID不匹配");
            //    return;
            //}
            bool pingAvailable = await WltWebFunction.PingWebsiteAsync(AppConfig.url_WltHost);
            if (pingAvailable == false)
            {
                Debug.WriteLine("非校园网");
                return;
            }
            bool isAccessible;
            using (var webFunction = new WltWebFunction())
            {
                string testUrl = AppConfig.url_MSConnectTest;
                isAccessible = await webFunction.TestWebsiteAccessAsync(testUrl);
                string content = $"网站 {testUrl} 可访问性：{(isAccessible ? "可访问" : "不可访问")}";
            }
            if (isAccessible == false)
            {
                Debug.WriteLine("连接不通，需登录");
                using (var webFunction = new WltWebFunction())
                {
                    await webFunction.LoginToWlt();
                }
            }
            else
            {
                Debug.WriteLine("连接通，不需登录");
                WltWebFunction.isLogined = true;
            }
        }
    }
}
