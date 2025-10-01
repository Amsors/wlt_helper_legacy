using wlt_helper.Utilities;
using System.Diagnostics;
using System.Collections.Generic;
using System.Timers;
using System;

namespace wlt_helper.Services
{
    //public class TimerTask
    //{
    //    private System.Threading.Timer timer;
    //    public TimerTask(System.Threading.TimerCallback timercallback, uint period = 1000)
    //    {
    //        timer = new System.Threading.Timer(timercallback, null, 0, period);
    //    }
    //}
    internal static class CronJob
    {
        static bool ConnectToWlt_hasLogined = false;
        static int ConnectToWlt_tryConnectCnt = 0;
        static bool ConnectToWlt_Lock = false;

        public static async void ConnectToWlt()
        {
            if (ConnectToWlt_Lock) return;
            ConnectToWlt_Lock = true;

            Debug.WriteLine("检查网络通");
            if (UserConfig.RepeatedCheck == false)
            {
                if (ConnectToWlt_hasLogined)
                {
                    return;
                }
                if (ConnectToWlt_tryConnectCnt > UserConfig.InitialCheckMaxCnt)
                {
                    return;
                }
            }

            ConnectToWlt_tryConnectCnt++;

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
                ConnectToWlt_hasLogined = true;
                WltWebFunction.isLogined = true;
            }
            ConnectToWlt_Lock = false;
        }
    }
    public class TimerTaskManager
    {
        private Timer _timer;
        private readonly object _tasksLock = new object();
        private List<Action> _tasks;

        public TimerTaskManager(double intervalMilliseconds)
        {
            _tasks = new List<Action>();
            _timer = new Timer(intervalMilliseconds);
            _timer.Elapsed += ExecuteAllTasks;
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void AddTask(Action task)
        {
            lock (_tasksLock)
            {
                _tasks.Add(task);
            }
        }

        public void RemoveTask(Action task)
        {
            lock (_tasksLock)
            {
                _tasks.Remove(task);
            }
        }

        private void ExecuteAllTasks(object sender, ElapsedEventArgs e)
        {
            lock (_tasksLock)
            {
                foreach (var task in _tasks)
                {
                    try
                    {
                        task.Invoke();
                        Console.WriteLine($"Task executed at {DateTime.Now}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error executing task: {ex.Message}");
                    }
                }
            }
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
