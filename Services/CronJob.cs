using System.Diagnostics;
using wlt_helper.Utilities;

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
        public async void ConnectToWlt(object state)
        {
            bool pingAvailable = await WltWebFunction.PingWebsiteAsync(AppConfig.url_WltHost);
            if (pingAvailable == false)
            {
                TbxLogger.LogWrite("检测到非校园网 不进行登录");
                return;
            }
            bool isAccessible;
            using (var webFunction = new WltWebFunction())
            {
                string testUrl = AppConfig.url_DefaultConnectTest;
                isAccessible = await webFunction.TestWebsiteAccessAsync(testUrl);
                string content = $"网站 {testUrl} {(isAccessible ? "可访问" : "不可访问")}";
                TbxLogger.LogWrite(content);
            }
            if (isAccessible == false)
            {
                TbxLogger.LogWrite($"判定为网络连接不通，需登录");
                using (var webFunction = new WltWebFunction())
                {
                    await webFunction.LoginToWlt();
                }
            }
            else
            {
                TbxLogger.LogWrite($"判定为网络可以连接通，不需登录");
                WltWebFunction.isLogined = true;
            }
        }
    }
}
