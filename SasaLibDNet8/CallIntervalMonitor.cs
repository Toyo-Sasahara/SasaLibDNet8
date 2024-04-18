using SasaLib;
using System;

namespace SasaLib
{
    public class CallIntervalMonitor
    {
        static DateTime recentTime;
        static string path;
        public bool IsNearTime;

        /// <summary>
        /// コンストラクタ呼び出しが前回から1秒以内ならtrueを返す
        /// </summary>
        /// <param name="flag"></param>
        public CallIntervalMonitor(string Path , int spanSec = 1)
        {
            if (path == Path && DateTime.Now.Subtract(recentTime) < new TimeSpan(0, 0, 0, spanSec))
            {
                IsNearTime = true;
                //DebugConsole.WriteLine($"☆彡前回の呼び出しから{spanSec} 秒以内です");
            }
            else
            {
                recentTime = DateTime.Now;
                path = Path;
                IsNearTime = false;
                DebugConsole.WriteLine($"☆彡前回の呼び出しから{spanSec} 秒以上経過していました{path}");
            }
        }
    }
}
