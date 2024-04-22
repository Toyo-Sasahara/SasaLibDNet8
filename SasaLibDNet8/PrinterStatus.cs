using System;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

namespace SasaLib
{
    [SupportedOSPlatform("windows")]
    public static class PrinterStatus
    {
        public static int GetNumberofPrintQues(string PrinterName)
        {

            var printQueue = GetPrintQueue(PrinterName);

            if (printQueue == null)
                return -1;

            int numberOfJobs = 0;
            if (printQueue != null)
                numberOfJobs = printQueue.NumberOfJobs;

            return numberOfJobs;
        }


        /// <summary>
        /// 指定したプリンタドライバのキュー情報を得る
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="isDebug"></param>
        /// <returns></returns>
        public static System.Printing.PrintQueue GetPrintQueue(string printerName, EventsSummary evt = null, bool isDebug = false)

        {
            System.Printing.LocalPrintServer server = new System.Printing.LocalPrintServer();
            System.Printing.PrintQueueCollection queueCollection = server.GetPrintQueues();

            var que = queueCollection.FirstOrDefault(x => x.FullName == printerName);

            if (que == null)
            {
                DebugConsole.WriteLine($"PrinterStatus.GetPrintQueue(..) プリンタ名 {printerName} は存在しません");
                return null;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"PrinterStatus.GetPrintQueue(..)  対象プリンタ {printerName}");

            if (isDebug)
            {
                try
                {
                    sb.AppendLine($"Name : \"{que.Name}\" 印刷キューの名前を取得または設定します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"Comment : \"{que.Comment}\" t\r\nプリンターに関するコメントを取得または設定します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"Location : \"{que.Location}\" プリンターの物理的な場所を取得または設定します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"NeedUserIntervention : \"{que.NeedUserIntervention}\" プリンターが人の介入を必要とするかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"NumberOfJobs : \"{que.NumberOfJobs}\" 印刷キューに並んでいるジョブの合計数を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"Description : \"{que.Description}\" 印刷キューの説明を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"FullName : \"{que.FullName}\"キューの完全な名前を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"HasPaperProblem : \"{que.HasPaperProblem}\" 特定できない用紙の問題がプリンターで発生しているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"HasToner : \"{que.HasToner}\" プリンターにトナーがあるかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"AveragePagesPerMinute : \"{que.AveragePagesPerMinute}\" ページ/分で測定されたプリンターの速度を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"ClientPrintSchemaVersion : \"{que.ClientPrintSchemaVersion}\" 印刷スキーマのバージョンを取得します。");
                }
                catch { }
                //sb.AppendLine($"CurrentJobSettings : \"{que.CurrentJobSettings}\"");
                //sb.AppendLine($"DefaultPrintTicket : \"{que.DefaultPrintTicket}\"");
                try
                {
                    sb.AppendLine($"DefaultPriority : \"{que.DefaultPriority}\"");
                }
                catch { }
                try
                {
                    sb.AppendLine($"HostingPrintServer : \"{que.HostingPrintServer}\" 印刷キューを制御するプリント サーバーを取得または設定 (protected) します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"InPartialTrust : \"{que.InPartialTrust}\" キューが部分信頼モード (より高いレベルの信頼) で動作しているかどうかを示す値を取得または設定します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsBidiEnabled : \"{que.IsBidiEnabled}\" プリンターとの双方向通信が有効かどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsBusy : \"{que.IsBusy}\" 印刷デバイスがビジーかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsDevQueryEnabled : \"{que.IsDevQueryEnabled}\" ドキュメントとプリンターの構成が一致しない場合にキューでドキュメントを保持するかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsDirect : \"{que.IsDirect}\" キューから直接プリンターに印刷するかドキュメントをスプールしてから印刷するかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsDoorOpened : \"{que.IsDoorOpened}\" プリンターのドアが開いているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsHidden : \"{que.IsHidden}\" アプリケーションのユーザー インターフェイスで印刷キューが非表示になっているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsInError : \"{que.IsInError}\" プリンターやデバイスがエラー状態になっているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsInitializing : \"{que.IsInitializing}\"プリンターが初期化中かどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsIOActive : \"{que.IsIOActive}\" プリンターがデータやシグナルを送受信しているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsManualFeedRequired : \"{que.IsManualFeedRequired}\" 現在の印刷ジョブで、プリンターに手差しで給紙する必要があるかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsNotAvailable : \"{que.IsNotAvailable}\" プリンターが使用可能かどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsOffline : \"{que.IsOffline}\" プリンターがオフラインであるかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsOutOfMemory : \"{que.IsOutOfMemory}\" プリンターのメモリが不足しているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsOutOfPaper : \"{que.IsOutOfPaper}\" 現在のジョブに必要なサイズの用紙をプリンターに補充する必要があるかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsOutputBinFull : \"{que.IsOutputBinFull}\" プリンターの出力領域がいっぱいになっているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsPaperJammed : \"{que.IsPaperJammed}\" プリンターで紙詰まりが発生しているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsPendingDeletion : \"{que.IsPendingDeletion}\" 印刷キューが一時停止されているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsPowerSaveOn : \"{que.IsPowerSaveOn}\" プリンターが印刷ジョブの削除中かどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsPrinting : \"{que.IsPrinting}\" プリンターが省電力モードかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsProcessing : \"{que.IsProcessing}\" ジョブが印刷中かどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsPublished : \"{que.IsPublished}\" プリンターが印刷ジョブを処理しているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsQueued : \"{que.IsQueued}\" 一度に複数の印刷ジョブを含むキューがプリンターでサポートされているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsRawOnlyEnabled : \"{que.IsRawOnlyEnabled}\" 印刷元のアプリケーションから Windows スプーラーへのデータ フローを高速化できる EMF (拡張メタファイル) を印刷キューで使用できるかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsServerUnknown : \"{que.IsServerUnknown}\" プリンターがエラー状態になっているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsShared : \"{que.IsShared}\" ネットワーク上の他のコンピューターがこのプリンターを使用できるかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsTonerLow : \"{que.IsTonerLow}\" プリンターのトナーが不足しているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsWaiting : \"{que.IsWaiting}\" キューがジョブの追加を待機しているかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsWarmingUp : \"{que.IsWarmingUp}\" プリンターがウォームアップ中かどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"IsXpsDevice : \"{que.IsXpsDevice}\" プリンターのドライバーが、XPSDrv モデルに基づいて構築されている (したがって、ページ記述言語として XML Paper Specification (XPS) が使用されている) かどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"KeepPrintedJobs : \"{que.KeepPrintedJobs}\" キューでプリンター言語ファイルを印刷後に削除せずに保存するかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"PagePunt : \"{que.PagePunt}\" プリンターで現在のページを印刷できないかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"PrintingIsCancelled : \"{que.PrintingIsCancelled}\" 現在の印刷ジョブをキャンセルするかどうかを示す値を取得または設定します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"Priority : \"{que.Priority}\" 同じプリント サーバーでホストされており、同じ物理プリンターを使用する他の印刷キューと比較した場合の、この印刷キューの優先順位を取得または設定します。");
                }
                catch { }
                //sb.AppendLine($"PropertiesCollection : \"{que.PropertiesCollection}\" 属性と値のペアのコレクションを取得します。");
                try
                {
                    sb.AppendLine($"QueueAttributes : \"{que.QueueAttributes}\" 印刷キューのプロパティを取得します。");
                }
                catch { }
                //sb.AppendLine($"QueueDriver : \"{que.QueueDriver}\" キューのプリンター ドライバーを取得または設定します。");
                //sb.AppendLine($"QueuePort : \"{que.QueuePort}\" キューが使用するポートを取得または設定します。");
                //sb.AppendLine($"QueuePrintProcessor : \"{que.QueuePrintProcessor}\" キューが使用するプリント プロセッサを取得または設定します。");
                try
                {
                    sb.AppendLine($"QueueStatus : \"{que.QueueStatus}\" プリンターのステータスを表す値を取得します (\"ウォームアップ中\"、\"初期化中\"、\"印刷中\" など)。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"ScheduleCompletedJobsFirst \"{que.ScheduleCompletedJobsFirst}\" プリンターで、キューに入った順序や優先順位に関係なく、スプール処理が完了したジョブから先に印刷するかどうかを示す値を取得します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"SeparatorFile : \"{que.SeparatorFile}\" 各印刷ジョブの先頭に挿入されるファイルのパスとファイル名を取得または設定します。");
                }
                catch { }
                try
                {
                    sb.AppendLine($"プリンターが共有されている場合にネットワーク上のユーザーに表示されるプリンターの名前を取得または設定します。ShareName : \"{que.ShareName}\"");
                }
                catch { }
                //sb.AppendLine($"StartTimeOfDay : \"{que.StartTimeOfDay}\" プリンターがジョブを印刷する最も早い時刻を取得または設定します。この時刻は、協定世界時刻 (UTC) (グリニッジ標準時 [GMT] とも呼ばれます) の午前 0 時からの分数で表現されます。");
                //sb.AppendLine($"UntilTimeOfDay : \"{que.UntilTimeOfDay}\" プリンターがジョブを印刷する最も遅い時刻を取得または設定します。この時刻は、協定世界時刻 (UTC) (グリニッジ標準時 [GMT] とも呼ばれます) の午前 0 時からの分数で表現されます。");
                //sb.AppendLine($"UserPrintTicket : \"{que.UserPrintTicket}\" 印刷ジョブに関する詳細情報を含む、現在のユーザーの既定 PrintTicket のオブジェクトを取得または設定します。");
            }
            string debugmsg = sb.ToString();

            DebugConsole.WriteLine(debugmsg);

            if (evt != null)
            {
                evt.Add(debugmsg);
            }

            return que;
        }
    }
}
