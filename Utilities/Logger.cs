using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wlt_helper.Utilities
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Windows.Forms;

    public static class TbxLogger
    {
        private static ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        private static ManualResetEvent _hasNewLog = new ManualResetEvent(false);
        private static Thread _loggingThread;
        private static TextBox _targetTextBox;
        private static bool _isRunning;

        public static void Initialize(TextBox textBox)
        {
            if (textBox == null) throw new ArgumentNullException(nameof(textBox));
            _targetTextBox = textBox;

            _isRunning = true;
            _loggingThread = new Thread(ProcessLogQueue)
            {
                Name = "LoggingThread",
                IsBackground = true
            };
            _loggingThread.Start();
        }

        public static void LogWrite(string message)
        {
            if (!_isRunning) return;

            var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            var formattedMessage = $"[{timestamp}] {message}";

            _logQueue.Enqueue(formattedMessage);
            _hasNewLog.Set();
        }

        public static void Shutdown()
        {
            _isRunning = false;
            _hasNewLog.Set();
            _loggingThread?.Join(1000);
        }

        private static void ProcessLogQueue()
        {
            while (_isRunning)
            {
                _hasNewLog.WaitOne(100);
                _hasNewLog.Reset();

                while (_isRunning && _logQueue.TryDequeue(out string message))
                {
                    if (_targetTextBox.IsDisposed) return;

                    _targetTextBox.Invoke(new Action(() =>
                    {
                        _targetTextBox.AppendText(message + Environment.NewLine);
                    }));
                }
            }
        }
    }

    public static class DebugLogger
    {
        private static readonly bool enable = true;
        public static void LogWrite(string message)
        {
            if (enable)
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }
    }
}
