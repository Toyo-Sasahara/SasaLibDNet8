
using SasaLib;
using SasaLib.PIPE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/// https://qiita.com/takutoy/items/c384fcb439d345a9a0d3#%E9%9D%9E%E5%90%8C%E6%9C%9Ftask%E3%82%92%E4%B8%A6%E8%A1%8C%E5%AE%9F%E8%A1%8C%E3%81%99%E3%82%8B


namespace SasaLib.VariableControlPipeServer
{
    public delegate DialogResult Delegate_MessageBoxShow(string Message, string Title, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon);

    /// <summary>
    /// ToyoSTAGINGSYSTEMwatch 側 PIPEconnectionLoop
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class VariableControlPipeServer
    {
        public SasaLibDelegateWriteLine WriteLine;

        object targetObj;
        public int ParentWindowHandle { get; set; }
        Delegate_MessageBoxShow delegate_MessageBoxShow;

        public string pipeName { get; private set; }
        public string HandShakeKeyword { get; private set; }

        /// <summary>
        /// ■CADクライアント 汎用ネットワークパイプサーバー
        /// </summary>
        /// <param name="pipeName">パイプ名</param>
        /// <param name="handShakeKeyword">ハンドシェーク文字列</param>
        /// <param name="targetObj">public属性のフィールドを備えるオブジェクト（CommitConfig.Config 等）</param>
        /// <param name="writeLine">メッセージボックス</param>
        /// <param name="hwnd">親ｳｨﾝﾄﾞｳハンドル</param>
        /// <param name="delegate_MessageBoxShow">CADクライアント側が用意するMessageBox.Show(..)型メソッドへのﾃﾞﾘｹﾞｰﾄ</param>
        public VariableControlPipeServer(string pipeName, string handShakeKeyword, object targetObj, SasaLibDelegateWriteLine writeLine, int hwnd = 0, Delegate_MessageBoxShow delegate_MessageBoxShow = null, int numberOfTasks = 3)
        {
            this.targetObj = targetObj;
            this.ParentWindowHandle = hwnd;
            this.delegate_MessageBoxShow = delegate_MessageBoxShow;

            if (writeLine == null) this.WriteLine = DebugConsole.Write; else this.WriteLine = writeLine;

            this.pipeName = pipeName;
            HandShakeKeyword = handShakeKeyword;

            _VariableControlPipeServer(numberOfTasks);
        }

        private void _VariableControlPipeServer(int numberOfTasks)
        {
            try
            {
                var tasks = new List<Task<int>>();

                // タスクをデリゲートとして定義します
                Func<object, int> action = (object obj) =>
                {
                    int i = (int)obj;

                    Task task = PIPEserverGeneration(i);

                    int tickCount = Environment.TickCount;

                    return tickCount;
                };

                // 開始したタスクの構築
                for (int i = 0; i < numberOfTasks; i++)
                {
                    int index = i;
                    tasks.Add(Task<int>.Factory.StartNew(action, index));
                }

                try
                {
                    // すべてのタスクが終了するのを待ちます。
                    Task.WaitAll(tasks.ToArray());
                }
                catch (AggregateException e)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int j = 0; j < e.InnerExceptions.Count; j++)
                    {
                        sb.Append($"{e.InnerExceptions[j]}");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] _VariableControlPipeServer(...)で例外キャッチ： {ex.Message}");
            }
        }


        /// <summary>
        /// ■パイプサーバー生成 
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        private async Task PIPEserverGeneration(int serverId)
        {
            bool BUSYFLAG = false;
            string serverHostName = System.Net.Dns.GetHostName();
            WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]  [ID:{serverId}] , VariableControlPipeServer.PIPEserverGeneration(..) \"\\\\{serverHostName}\\pipe\\{pipeName}\"開始");
            // ループスタート
            while (true)
            {
                // 名前付きパイプサーバーPIPE名 StageServerConfig.Config.PipeNameDW の生成 
                using (var namedpipeSrvStreamFactory = NamedPipeServerStreamFactory.Create(pipeName))
                {
                    // ｸﾗｲｱﾝﾄから送られてきたコマンド文字列
                    string command;
                    // パイプ終端のクライアントユーザー名
                    string ImpersonationUserName = "";
                    // 接続元PC
                    string namedPipeClient = "";
                    // 接続してきたクライアントのid
                    int clientProcessID;

                    try
                    {
                        // クライアントからの接続を待つ
                        await namedpipeSrvStreamFactory.WaitForConnectionAsync();

                        try
                        {
                            // 接続してきたクライアントのユーザ名
                            ImpersonationUserName = namedpipeSrvStreamFactory.GetImpersonationUserName();
                        }
                        catch (Exception ex)
                        {
                            WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] GetImpersonationUserName()で例外発生  {ex.Message} {ex.InnerException} , ImpersonationUserName : {ImpersonationUserName}");
                        }

                        try
                        {
                            // 接続してきたクライアントのホスト名
                            namedPipeClient = NamedPipeClientInfo.GetClientComputerName(namedpipeSrvStreamFactory);
                        }
                        catch (Exception ex)
                        {
                            WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] NamedPipeClientInfo.GetClientComputerNam()で例外発生  {ex.Message} {ex.InnerException}, ImpersonationUserName : {ImpersonationUserName}");
                        }

                        // 接続してきたクライアントのid
                        clientProcessID = (int)NamedPipeClientInfo.GetClientProcessID(namedpipeSrvStreamFactory);

                        // 接続元のｸﾗｲｱﾝﾄ情報を文字列化                     
                        string clientInfo = ConectedPipeInformationMsg(serverId, namedPipeClient, ImpersonationUserName, clientProcessID);

                        WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] 名前付きパイプの接続が確立されました。\"\\\\{serverHostName}\\{pipeName}\"  , clientInfo ; {clientInfo}");

                        StreamString stStr = new StreamString(namedpipeSrvStreamFactory);

                        if (BUSYFLAG == true)
                        {
                            WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] \"\\\\{serverHostName}\\{pipeName}\" {clientInfo} , BUSYフラグtrueのため、処理中です受け付けません , clientInfo ; {clientInfo}");
                            // コネクトキーワード送出
                            stStr.WriteString(@"BUSY");
                            continue;
                        }
                        else
                        {
                            WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] \"\\\\{serverHostName}\\{pipeName}\" PIPEserverGeneration(..) BUSYフラグ trueにセット , clientInfo ; {clientInfo}");
                            BUSYFLAG = true;
                        }

                        // コネクトキーワード送出
                        int timeout = 20000;
                        int result = stStr.WriteString(HandShakeKeyword, timeout, WriteLine);
                        if (result == 0)
                        {
                            WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] \"\\\\{serverHostName}\\{pipeName}\" 最初のハンドシェイク失敗 WriteString({HandShakeKeyword} タイムアウト{timeout} msec発生 ) , clientInfo ; {clientInfo}");
                            return;
                        }
                        // クライアントから最初に送られてきた文字列をコマンド名とする。
                        command = stStr.ReadString(timeout, WriteLine);
                        if (command == null)
                        {
                            WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] \"\\\\{serverHostName}\\{pipeName}\" コマンド名受信失敗 timeout : {timeout} sec , clientInfo ; {clientInfo}");
                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] \"\\\\{serverHostName}\\{pipeName}\" 最初のハンドシェイクで例外発生  {ex.Message}");
                        Task.WaitAll(new[] { PIPEserverGeneration(serverId++) });
                        return;
                    }

                    //■コマンドワード解析
                    CommandAnalyze pipeCommands = new CommandAnalyze(command, targetObj, namedpipeSrvStreamFactory, serverId, namedPipeClient, ImpersonationUserName, clientProcessID, ref BUSYFLAG, WriteLine, ParentWindowHandle, delegate_MessageBoxShow);

                    BUSYFLAG = false;

                    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}]  コマンド:{pipeCommands} 終了しました。 BUSYフラグ falseにセット");
                } //  using (var namedpipeSrvStreamFactory = NamedPipeServerStreamFactory.Create(pipeName)

                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}]  PIPEserverGeneration(..) ループの最後です");
            }
        }

        /// <summary>
        /// 接続してきたPIPEクライアントの情報を文字列化
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="namedPipeClient"></param>
        /// <param name="ImpersonationUserName"></param>
        /// <param name="clientProcessID"></param>
        /// <returns></returns>
        private string ConectedPipeInformationMsg(int serverId, string namedPipeClient, string ImpersonationUserName, int clientProcessID)
        {
            try
            {
                string DnsClienntComputerName = Net.DnsGetHostName(namedPipeClient); // IPアドレスの場合ホスト名を返す

                return $"ｸﾗｲｱﾝﾄPC:[{DnsClienntComputerName}]({namedPipeClient}) , ﾕｰｻﾞｰ:{ImpersonationUserName} , ｸﾗｲｱﾝﾄﾌﾟﾛｾｽID:{clientProcessID} ";
            }
            catch (Exception ex)
            {
                //WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]  [ID:{serverId}] :接続元：{ImpersonationUserName}: ConectedPipeInformationMsg(...)にて 例外発生  {ex.Message}");

                return $"ConectedPipeInformationMsg(..) 例外 [ID:{serverId}] , 接続元:[Dns名取得失敗]({namedPipeClient}) , ユーザー名: {ImpersonationUserName} , ｸﾗｲｱﾝﾄﾌﾟﾛｾｽID:{clientProcessID} {ex.Message}";
            }
        }
    }
}
