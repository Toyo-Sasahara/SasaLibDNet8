//using System;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using System.Runtime.Versioning;

//namespace SasaLib.Eventlog
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    static public class Log
//    {
//        /// <summary>
//        /// イベントログに出力
//        /// </summary>
//        /// <param name="source">出力ソース名</param>
//        /// <param name="format">出力文字列フォーマット</param>
//        /// <param name="args">変数</param>
//        static public void WriteEntry(string source, string format, params object[] args)
//        {
//            var stringFormated = string.Format(format, args);
//            Console.WriteLine("Log:" + source + ":" + format, args);
//            EventLog.WriteEntry(source, stringFormated);
//        }

//        /// <summary>
//        /// イベントログに出力
//        /// </summary>
//        /// <param name="source">ソース名</param>
//        /// <param name="eventType">イベント種別</param>
//        /// <param name="eventID">イベントID</param>
//        /// <param name="format">メッセージフォーマット</param>
//        /// <param name="CallerMemmberName">true:呼び出し先表示, false:表示しない</param>
//        /// <param name="OutConsole"></param>
//        /// <param name="memberName">指定しないこと</param>
//        /// <param name="sourceFilePath">指定しないこと</param>
//        /// <param name="sourceLineNumber">指定しないこと</param>
//        [DebuggerHidden()]
//        static public void WriteEntry(string source, EventLogEntryType eventType, int eventID, string format, bool CallerMemmberName = true, bool OutConsole = true, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
//        {
//            var stringFormated = default(string);
//            try
//            {
//                if (CallerMemmberName)
//                {
//                    sourceFilePath = FileFolder.GetFileName(sourceFilePath);
//                    stringFormated = format + $"\n　呼び出し元:ソースファイル:{sourceFilePath},{sourceLineNumber}行,メンバー名：{memberName}";
//                }
//                else
//                {
//                    stringFormated = format;
//                }
//            }
//            catch (System.Security.SecurityException sex)
//            {
//                Console.WriteLine($"{sex.Message} {sex.InnerException}");
//            }
//            catch (Exception ex)
//            {
//                EventLog.WriteEntry(source, $"string.Format(format)のエラー{ex.Message} 呼び出し元:ソースファイル:{sourceFilePath},{sourceLineNumber}行,メンバー名：{memberName}", eventType, eventID);
//            }
//            try
//            {
//                //ソースが存在していない時は、作成する
//                if (!System.Diagnostics.EventLog.SourceExists(source))
//                {
//                    //ログ名を空白にすると、"Application"となる
//                    System.Diagnostics.EventLog.CreateEventSource(source, "");
//                }

//                EventLog.WriteEntry(source, stringFormated, eventType, eventID);
//            }
//            catch (System.Security.SecurityException sex)
//            {
//                Console.WriteLine($"{sex.Message} {sex.InnerException}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"{ex.Message} {ex.InnerException}");
//            }

//            string datetime = System.DateTime.Now.ToString("HH:mm:ss.fff");

//            if (OutConsole)
//            {
//                switch (eventType)
//                {
//                    case EventLogEntryType.Information:
//                        DebugConsole.WriteLine($"□SasaLib.Eventlog.Log.WriteEntry(..)情報({datetime}):" + source + ":" + stringFormated);
//                        Console.ResetColor();
//                        break;
//                    case EventLogEntryType.Warning:
//                        Console.ForegroundColor = ConsoleColor.Yellow;
//                        Console.WriteLine($"△SasaLib.Eventlog.Log.WriteEntry(..)警告({datetime}):" + source + ":" + stringFormated);
//                        Console.ResetColor();
//                        break;
//                    case EventLogEntryType.Error:
//                        Console.ForegroundColor = ConsoleColor.Red;
//                        Console.WriteLine($"▲SasaLib.Eventlog.Log.WriteEntry(..)エラー({datetime}):" + source + ":" + stringFormated);
//                        Console.ResetColor();
//                        break;
//                    case EventLogEntryType.SuccessAudit:
//                        Console.WriteLine($"□SasaLib.Eventlog.Log.WriteEntry(..)成功({datetime}):" + source + ":" + stringFormated);
//                        Console.ResetColor();
//                        break;
//                    case EventLogEntryType.FailureAudit:
//                        Console.ForegroundColor = ConsoleColor.DarkRed;
//                        Console.WriteLine($"▲SasaLib.Eventlog.Log.WriteEntry(..)失敗({datetime}):" + source + ":" + stringFormated);
//                        Console.ResetColor();
//                        break;
//                    default:
//                        Console.WriteLine("▲不明:" + source + ":" + stringFormated);
//                        Console.ResetColor();
//                        break;
//                }
//            }
//        }


//    }
//}
