//using System;
//using System.Runtime.Versioning;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class StopWatch
//    {
//        //Stopwatchオブジェクトを作成する
//        System.Diagnostics.Stopwatch sw;
//        private readonly string ID;


//        bool eventLogOut;

//        EventsSummary summary;

//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        /// <param name="IDname"></param>
//        /// <param name="consoleShow"></param>
//        /// <param name="eventLogOut"></param>
//        public StopWatch(string IDname="", bool consoleShow = false, bool eventLogOut = false)
//        {
//            ID = IDname;
//            this.eventLogOut = eventLogOut;

//            sw = new System.Diagnostics.Stopwatch();
            
//            if (consoleShow)
//                DebugConsole.WriteLine($"□計測スタート:{ID}");
//            if (this.eventLogOut) { 
//                this.summary = new EventsSummary("SasaLib",0);
//                summary.Add($"SasaLibStopWatchクラス 計測スタート {IDname}");
//            }
//            sw.Start();
//        }
        
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="Comment"></param>
//        /// <param name="consoleShow"></param>
//        public void LapTime(string Comment="", bool consoleShow = false)
//        {
//            if (consoleShow)
//                DebugConsole.WriteLine($"　→ラップタイム:{ID}:経過時間{sw.Elapsed}:{Comment}");

//            if (this.eventLogOut)
//            {
//                summary.Add($"　→ラップタイム:{ID}:経過時間{sw.Elapsed}:{Comment}");
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <returns></returns>
//        public TimeSpan GetLapTime()
//        {
//            return sw.Elapsed;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="Comment"></param>
//        /// <param name="consoleShow"></param>
//        /// <returns></returns>
//        public TimeSpan Stop(string Comment = "", bool consoleShow = false)
//        {
//            if (consoleShow)
//                DebugConsole.WriteLine($"□計測終了:{ID}:経過時間{sw.Elapsed}:{Comment}");

//            if (this.eventLogOut)
//            {
//                summary.Add($"計測終了:{ID}:経過時間{sw.Elapsed}:{Comment}");
//                summary.SendEntry(System.Diagnostics.EventLogEntryType.Information, header: $"{this.ID} 経過時間", footer: "");
//                summary.clear();
//            }

//            sw.Stop();
//            return sw.Elapsed;
//        }
//    }
//}
