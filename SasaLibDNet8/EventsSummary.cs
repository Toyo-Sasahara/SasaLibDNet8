using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Interop;

namespace SasaLib
{
    /// <summary>
    /// イベントをまとめて一括処理するためのクラス
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class EventsSummary : IDisposable
    {
        private readonly StringBuilder sb1;

        private readonly string source = "SasaLib.EventsSummary";
        private int eventID = 0;

        private int msgID;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventsSummary()
        {
            sb1 = new StringBuilder();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventsSummary(string source, int eventID, int msgID=0)
        {
            sb1 = new StringBuilder();
            this.source = source;
            this.eventID = eventID;
            this.msgID = msgID;
        }

        /// <summary>
        /// 
        /// </summary>
        public void clear()
        {
            msgID = 0;
            sb1.Clear();
        }

        /// <summary>
        /// イベント追加メソッド
        /// </summary>
        /// <param name="msg">イベント内容</param>
        /// <param name="OutConsole">true:Coneole.WriteLine()メソッドで内容を表示sます false:左記の逆</param>
        /// <param name="CallerMemmberName">true:呼び出し元情報を取り入れます false:左記の逆</param>
        /// <param name="memberName">使用しないこと</param>
        /// <param name="sourceFilePath">使用しないこと</param>
        /// <param name="sourceLineNumber">使用しないこと</param>
        public void Add(string msg, bool OutConsole = true, bool CallerMemmberName = true, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            try
            {
                msgID++;

                string input;
                string datetime = System.DateTime.Now.ToString("HH:mm:ss.fff");
                sourceFilePath = FileFolder.GetFileName(sourceFilePath);

                if (CallerMemmberName)
                    input = $"[TS:{datetime}] , [{sourceFilePath}] , 行:[{sourceLineNumber}] , メンバ:[{memberName}]\n" + $"【{msgID}】" + string.Format(msg) + "\n";
                else
                    input = $"[TS:{datetime}]\n" + $"【{msgID}】" + string.Format(msg);

                if (OutConsole)
                    Console.WriteLine(input);
                sb1.AppendLine(input);
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry(source, EventLogEntryType.Error, 0, $"EventsSummary.Add() 内にて例外発生 {ex.Message} {ex.InnerException}\n\"{msg}\" ", true, false);
            }
        }

        /// <summary>
        /// Entryを文字列で取得
        /// </summary>
        /// <returns></returns>
        public string GetEntry()
        {
            var result = sb1.ToString();
            return result;
        }

        /// <summary>
        /// まとめてイベントログへ送る
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="eventID"></param>
        /// <param name="header"></param>
        /// <param name="footer"></param>
        public void SendEntry(string source, EventLogEntryType eventType, int eventID, string header = null, string footer = null)
        {
            if (header==null)
                Eventlog.Log.WriteEntry(source, eventType, eventID, sb1.ToString() , true , false);
            else
                Eventlog.Log.WriteEntry(source, eventType, eventID, $"{header}\n{sb1}{footer}",true,false);
            sb1.Clear();
        }

        /// <summary>
        /// まとめてイベントログへ送る.EventLogEntryType, ヘッダーとフッターのみ指定
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="header"></param>
        /// <param name="footer"></param>
        public void SendEntry(EventLogEntryType eventType, string header = null, string footer = null)
        {
            if (header == null)
                Eventlog.Log.WriteEntry(source, eventType, eventID, sb1.ToString(), true, false);
            else
                Eventlog.Log.WriteEntry(source, eventType, eventID, $"{header}\n{sb1}{footer}", true, false);
            sb1.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            sb1.Clear();
        }

        ///// <summary>
        ///// メールにて送信
        ///// </summary>
        ///// <param name="System"></param>
        ///// <param name="Type"></param>
        //public void SendEmailFromREGISTADDR(string System, string Type)
        //{
        //    MailNotice.SendEmailFromStageServer(System, Type, sb1.ToString());
        //    sb1.Clear();
        //}

        ///// <summary>
        ///// メールにて送信
        ///// </summary>
        ///// <param name="System"></param>
        ///// <param name="Type"></param>
        //public void SendEmailFromStageServer(string System, string Type)
        //{
        //    MailNotice.SendEmailFromREGISTADDR(System, Type, sb1.ToString());
        //    sb1.Clear();
        //}
    }
}
