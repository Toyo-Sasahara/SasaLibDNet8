using SasaLib;
using SasaLib.PIPE;
using SasaLib.VariableControlPipeServer;
//using SasaLib.Winlogon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.IO.Pipes;
using System.Net.NetworkInformation;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SasaLib.VariableControlPipeClient
{
    /// <summary>
    /// DRAWCAPTUEserviceとのリモート接続クラス
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class VariableControlPipeClient : RMCsupport
    {
        private readonly string PipeName;

        public delegate bool delegate_MainMethod(NamedPipeClientStream pipeCltStream);

        bool pingOK;

        #region ●プロパティ
        /// <summary>
        ///
        /// </summary>
        internal string DomainName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal string UserPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal bool ClsLogon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal string ServerHostname { get; set; }

        /// <summary>
        /// 接続タイムアウト
        /// </summary>
        public int pileCltStremConnectTimeOut { get; set; } = 500;

        /// <summary>
        /// PIPE接続のステータス
        /// </summary>
        public bool PipeConnectionStatus { get; private set; }

        #endregion

        #region ●コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="DomainName"></param>
        /// <param name="UserName"></param>
        /// <param name="UserPassword"></param>
        /// <param name="ClsLogon"></param>
        /// <param name="ServerHostname"></param>
        /// <param name="PipeName"></param>
        public VariableControlPipeClient(string DomainName, string UserName, string UserPassword, bool ClsLogon, string ServerHostname, string PipeName)
        {
            this.DomainName = DomainName;
            this.UserName = UserName;
            this.UserPassword = UserPassword;
            this.ClsLogon = ClsLogon;
            this.ServerHostname = ServerHostname;
            this.PipeName = PipeName;


            if (CheckPing(ServerHostname) == false)
            {
                DebugConsole.Write($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] Ping応答なし:{ServerHostname}\n");
                pingOK = false;
            }
            else
            {
                pingOK = true;
            }
        }
        #endregion      

        /// <summary>
        /// コマンド名のみで実行できるタイプのメソッド(返ってくるデータは1つ:文字列)
        /// </summary>
        /// <param name="CommandName"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public async Task<string> GetZeroValue_DataCommandAsync(string CommandName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            string taskresult = await Task.Run(() =>
            {
                string resultString = null;

                var result = Command_ConnnectStartAsync(CommandName, _MainMethod, WriteLine);

                if (result.Result)
                {
                    return resultString;
                }
                else
                {
                    return null;
                }

                bool _MainMethod(NamedPipeClientStream pipeCltStream)
                {
                    WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] CommandName:{CommandName} （引数なし）メイン処理スタート");

                    StreamString stst = new StreamString(pipeCltStream);

                    int timeout = 20000;

                    string ServerResPon1 = stst.ReadString(timeout, WriteLine);

                    WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] ServerResPon1 = {ServerResPon1}");

                    resultString = ServerResPon1;

                    WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] 接続先 \"\\\\{ServerHostname}\\PIPE\\{PipeName}\" CommandName:{CommandName}, Result:{resultString}");

                    return true;
                }

            });

            return taskresult;
        }

        /// <summary>
        /// コマンド名と1つの引数で実行できるタイプのメソッド(返ってくるデータは1つ:文字列)
        /// </summary>
        /// <param name="CommandName"></param>
        /// <param name="stringValue1"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public async Task<string> GetOneValue_DataCommandAsync(string CommandName, string stringValue1, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            string taskresult = await Task.Run(() =>
            {
                string resultString = null;

                var result = Command_ConnnectStartAsync(CommandName, _MainMethod, WriteLine);

                if (result.Result)
                {
                    return resultString;
                }
                else
                {
                    return null;
                }

                bool _MainMethod(NamedPipeClientStream pipeCltStream)
                {
                    WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] CommandName:{CommandName} （引数１コ）メイン処理スタート");

                    StreamString stst = new StreamString(pipeCltStream);

                    int timeout = 20000;

                    string ServerResPon1 = stst.ReadString(timeout, WriteLine);

                    WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] ServerResPon1 = {ServerResPon1}");

                    stst.WriteString(stringValue1);

                    string ServerResPon2 = stst.ReadString(timeout, WriteLine);

                    WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] ServerResPon2 = {ServerResPon2}");

                    resultString = ServerResPon2;

                    WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] 接続先 \"\\\\{ServerHostname}\\PIPE\\{PipeName}\" CommandName:{CommandName}  stringValue1:{stringValue1} resultString:{resultString}");

                    return true;
                }

            });

            return taskresult;
        }

        /// <summary>
        /// コマンド名と2つの引数で実行できるタイプのメソッド(返ってくるデータは1つ:文字列) 
        /// </summary>
        /// <param name="CommandName"></param>
        /// <param name="stringValue1"></param>
        /// <param name="stringValue2"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public async Task<string> GetTwoValue_DataCommandAsync(string CommandName, string stringValue1, string stringValue2, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            string taskresult = await Task.Run(() =>
            {
                string resultString = null;

                var result = Command_ConnnectStartAsync(CommandName, _MainMethod, WriteLine);

                if (result.Result)
                {
                    return resultString;
                }
                else
                {
                    return null;
                }

                bool _MainMethod(NamedPipeClientStream pipeCltStream)
                {
                    WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] CommandName:{CommandName} （引数２コ）メイン処理スタート");

                    StreamString stst = new StreamString(pipeCltStream);

                    int timeout = 20000;

                    string ServerResPon1 = stst.ReadString(timeout, WriteLine);

                    WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] ServerResPon1 = {ServerResPon1}");

                    stst.WriteString(stringValue1);

                    string ServerResPon2 = stst.ReadString(timeout, WriteLine);

                    WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] ServerResPon2 = {ServerResPon2}");


                    stst.WriteString(stringValue2);

                    string ServerResPon3 = stst.ReadString(timeout, WriteLine);

                    WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] ServerResPon3 = {ServerResPon3}");


                    resultString = ServerResPon3;

                    WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先 \"\\\\{ServerHostname}\\PIPE\\{PipeName}\" CommandName:{CommandName}  stringValue1:{stringValue1},stringValue2:{stringValue2}, result:{resultString}");

                    return true;
                }

            });

            return taskresult;
        }

        /// <summary>
        /// CommitConfig.Config オブジェクトの値を調査  (Task.Resut を  object 値 で 返す)
        /// </summary>
        /// <param name="CommitConfigParameterName"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public async Task<object> GetValueAndValueType_DataCommandAsync(string CommitConfigParameterName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;
            string CommandName = CMDNAME.GetValue;

            object taskresult = await Task.Run(() =>
            {
                object resultObject = null;
                Task<bool> result = null;
                try
                {
                    result = Command_ConnnectStartAsync(CommandName, _MainMethod, WriteLine);
                }
                catch (Exception ex)
                {
                    WriteLine($"※コマンド実行ｴﾗｰ CommandName : \"{CommandName}\" , CommitConfigParameterName : \"{CommitConfigParameterName}\" , 例外メッセージ : \"{ex.Message}\"");
                }

                if (result.Result)
                {
                    return resultObject;
                }
                else
                {
                    return null;
                }

                bool _MainMethod(NamedPipeClientStream pipeCltStream)
                {
                    DebugConsole.WriteLine("GetCommitConfigValue(..) スタート");

                    StreamString stst = new StreamString(pipeCltStream);

                    string ServerResPon1 = stst.ReadString();

                    DebugConsole.WriteLine($"ServerResPon1 = {ServerResPon1}");

                    stst.WriteString(CommitConfigParameterName); // 変数名送信

                    DebugConsole.WriteLine($"問い合わせる変数名 = 【{CommitConfigParameterName}】");

                    object receveFieldType;
                    using (BinaryReader reader = new BinaryReader(pipeCltStream, Encoding.UTF8, true))
                    {
                        receveFieldType = reader.ReadObject<System.Type>(); // 変数型情報受信
                    }
                    DebugConsole.WriteLine($"ｻｰﾊﾞｰから型情報受信 = 【{receveFieldType}】");

                    object receveObj;
                    using (BinaryReader reader = new BinaryReader(pipeCltStream, Encoding.UTF8, true))
                    {
                        receveObj = reader.ReadObject<Object>(); // ｻｰﾊﾞｰからオブジェクト受信
                    }

                    object value = Convert.ChangeType(receveObj, receveFieldType as System.Type); // 受信オブジェクトを変換
                    string valuletype = receveFieldType.ToString();

                    WriteLine($"\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 公開変数【{CommitConfigParameterName}】＝【{receveFieldType}】型：【{value}】");

                    resultObject = value;
                    return true;
                }

            });

            return taskresult;
        }

        /// <summary>
        /// CommitConfig.Config オブジェクトの値を調査 (Task.Resut を KeyValuePare<string オブジェクトタイプ, object 値> で 返す)
        /// </summary>
        /// <param name="CommitConfigParameterName"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public async Task<object> GetSetValueAndValueType_DataCommandAsync(string CommitConfigParameterName, string CommitConfigValue, bool setmode, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            object taskresult = await Task.Run(() =>
            {
                object resultObject = null;
                Task<bool> result = null;


                string CommandName;
                if (setmode == false)
                {
                    CommandName = CMDNAME.GetValue;
                    try
                    {
                        result = Command_ConnnectStartAsync(CommandName, _GetCommitConfigValue, WriteLine);
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"※コマンド実行ｴﾗｰ {CommandName} {CommitConfigParameterName}　{ex.Message}");
                    }
                }
                else
                {
                    CommandName = CMDNAME.SetValue;
                    try
                    {
                        WriteLine($"セットﾓｰﾄﾞ {CommitConfigParameterName}　= {CommitConfigValue}");
                        Task<bool> result1 = Command_ConnnectStartAsync(CommandName, _SetCommitConfigValue, WriteLine);
                        WriteLine($"セットﾓｰﾄﾞ 結果 {result1}");
                        result = Command_ConnnectStartAsync(CMDNAME.GetValue, _GetCommitConfigValue, WriteLine);
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"※コマンド実行ｴﾗｰ {CommandName} {CommitConfigParameterName}　{ex.Message}");
                    }

                }

                if (result.Result)
                {
                    return resultObject;
                }
                else
                {
                    return null;
                }

                //////////////////////////

                bool _GetCommitConfigValue(NamedPipeClientStream pipeCltStream)
                {
                    DebugConsole.WriteLine("_GetCommitConfigValue(..) スタート");

                    StreamString stst = new StreamString(pipeCltStream);

                    string ServerResPon1 = stst.ReadString();

                    DebugConsole.WriteLine($"ServerResPon1 = {ServerResPon1}");

                    stst.WriteString(CommitConfigParameterName); // 変数名送信

                    DebugConsole.WriteLine($"問い合わせる変数名 = 【{CommitConfigParameterName}】");

                    object receveFieldType;
                    using (BinaryReader reader = new BinaryReader(pipeCltStream, Encoding.UTF8, true))
                    {
                        receveFieldType = reader.ReadObject<System.Type>(); // 変数型情報受信
                    }
                    DebugConsole.WriteLine($"ｻｰﾊﾞｰから型情報受信 = 【{receveFieldType}】");

                    object receveObj;
                    using (BinaryReader reader = new BinaryReader(pipeCltStream, Encoding.UTF8, true))
                    {
                        receveObj = reader.ReadObject<Object>(); // ｻｰﾊﾞｰからオブジェクト受信
                    }

                    object value;
                    if (receveObj != null)
                    {
                        value = Convert.ChangeType(receveObj, receveFieldType as System.Type); // 受信オブジェクトを変換
                        string valuletype = receveFieldType.ToString();

                        WriteLine($"\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 公開変数【{CommitConfigParameterName}】,型【{receveFieldType}】,値【{value}】");

                        KeyValuePair<string, object> keyValuePair = new System.Collections.Generic.KeyValuePair<string, object>(valuletype, value);
                        resultObject = keyValuePair;
                        return true;
                    }
                    else
                    {
                        WriteLine($"\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 変数【{CommitConfigParameterName}】は存在しません");

                        return false;
                    }
                }

                bool _SetCommitConfigValue(NamedPipeClientStream pipeCltStream)
                {
                    DebugConsole.WriteLine("_SetCommitConfigValue(..) スタート");

                    StreamString stst = new StreamString(pipeCltStream);

                    string ServerResPon1 = stst.ReadString();

                    DebugConsole.WriteLine($"ServerResPon1 = {ServerResPon1}");

                    stst.WriteString(CommitConfigParameterName); // 変数名送信

                    System.Type receveFieldType;
                    using (BinaryReader reader = new BinaryReader(pipeCltStream, Encoding.UTF8, true))
                    {
                        receveFieldType = reader.ReadObject<System.Type>();
                    }

                    object value;
                    if (receveFieldType != null)
                    {
                        string fieldTypeStr = receveFieldType.ToString();
                        value = Convert.ChangeType(CommitConfigValue, receveFieldType);
                    }
                    else
                    {
                        value = null;
                    }

                    using (var writer = new BinaryWriter(pipeCltStream, Encoding.UTF8, true))
                    {
                        writer.WriteObject(value);
                    }
                    return true;
                }
            });
            return taskresult;
        }

        ///---------------------------------------------------------------------------------------

        /// <summary>
        /// ハンドシェイクとコマンド開始
        /// </summary>
        /// <param name="CommandName"></param>
        /// <param name="delegate_MainMethod"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public async Task<bool> Command_ConnnectStartAsync(string CommandName, delegate_MainMethod delegate_MainMethod, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            if (pingOK == false)
                return false;
            DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 接続文字列:{RMCsupport.ConnectKeyword}");

            bool result = false;

            //using (new ClsLogon(DomainName, UserName, UserPassword, ClsLogon))
            //{
            //    NamedPipeClientStream pipeCltStream = new NamedPipeClientStream(ServerHostname, PipeName);
            //    try
            //    {
            //        // 待機中のサーバーへ接続
            //        try
            //        {
            //            pipeCltStream.Connect(pileCltStremConnectTimeOut);
            //        }
            //        catch (TimeoutException ex)
            //        {
            //            PipeConnectionStatus = false;
            //            DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 実行コマンド:{CommandName} にてタイムアウト例外検知 {ex.Message}");
            //            return false;
            //        }
            //        catch (InvalidOperationException ex)
            //        {
            //            PipeConnectionStatus = false;
            //            DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 実行コマンド:{CommandName} にてすでに別のクライアントに接続済み例外検知 {ex.Message}");
            //            return false;
            //        }
            //        catch (IOException ex)
            //        {
            //            PipeConnectionStatus = false;
            //            DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 実行コマンド:{CommandName} にてIOException例外検知 {ex.Message}");
            //            return false;
            //        }
            //        catch (Exception ex)
            //        {
            //            PipeConnectionStatus = false;
            //            DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 実行コマンド:{CommandName} にて例外検知 {ex.Message}");
            //            return false;
            //        }
            //        // サーバーからのサーバ識別文字列を受け取ります。
            //        string input0 = null;
            //        StreamString stsr = new StreamString(pipeCltStream);

            //        taskresult = await Task.Run(() =>
            //        {
            //            int timeout = 20000;

            //            input0 = stsr.ReadString(timeout, WriteLine);
            //            if (input0 == null)
            //            {
            //                WriteLine($"※Command_ConnnectStartAsync() ハンドシェイク失敗 timeout : {timeout} sec");
            //                return false;
            //            }
            //            if (CheckFirstMessage(input0))
            //            {
            //                //DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" からの接続文字列{input0}は期待値です");
            //                stsr.WriteString(CommandName);

            //                DebugConsole.WriteLine($"◇ [{DateTime.Now}] ﾊﾟｲﾌﾟｻｰﾊﾞ:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" , ｺﾏﾝﾄﾞ:{CommandName} 接続しました。回答待ち・・・");

            //                bool result = delegate_MainMethod(pipeCltStream);

            //                //DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" コマンド:{CommandName} 処理関数の結果 {result} です");

            //                return true;
            //            }
            //            else
            //            {
            //                WriteLine($"※\"\\\\{ServerHostname}\\{PipeName}\" からの接続文字列{input0}が期待と違います");
            //                pipeCltStream.Close();

            //                return false;
            //            }

            //        });

            //    }
            //    catch (Exception ex)
            //    {
            //        PipeConnectionStatus = false;

            //        if (pipeCltStream.IsConnected == false)
            //        {
            //            WriteLine($"※\"\\\\{ServerHostname}\\{PipeName}\" コマンド:{CommandName} 、サーバーから途中で切断されました");
            //        }
            //        else
            //        {
            //            Eventlog.Log.WriteEntry("TOYODATABASE", EventLogEntryType.Error, 6002, $"RemotePipeClient.Command_ConnnectStart(..) 、\"\\\\{ServerHostname}\\{PipeName}\" 例外発生 {ex.Message} ");
            //        }
            //    }
            //}

            new WithFakeAccount(DomainName, UserName, UserPassword, ClsLogon, () =>
            {
                Console.WriteLine("During impersonation: " + WindowsIdentity.GetCurrent().Name);

                var result = ccccAsync(ServerHostname, CommandName, delegate_MainMethod, WriteLine);
            });


            return result;
        }

        private async Task<bool> ccccAsync(string ServerHostname, string CommandName, delegate_MainMethod delegate_MainMethod, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;
            bool taskresult = false;

            NamedPipeClientStream pipeCltStream = new NamedPipeClientStream(ServerHostname, PipeName);
            try
            {
                // 待機中のサーバーへ接続
                try
                {
                    pipeCltStream.Connect(pileCltStremConnectTimeOut);
                }
                catch (TimeoutException ex)
                {
                    PipeConnectionStatus = false;
                    DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 実行コマンド:{CommandName} にてタイムアウト例外検知 {ex.Message}");
                    return false;
                }
                catch (InvalidOperationException ex)
                {
                    PipeConnectionStatus = false;
                    DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 実行コマンド:{CommandName} にてすでに別のクライアントに接続済み例外検知 {ex.Message}");
                    return false;
                }
                catch (IOException ex)
                {
                    PipeConnectionStatus = false;
                    DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 実行コマンド:{CommandName} にてIOException例外検知 {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    PipeConnectionStatus = false;
                    DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" 実行コマンド:{CommandName} にて例外検知 {ex.Message}");
                    return false;
                }
                // サーバーからのサーバ識別文字列を受け取ります。
                string input0 = null;
                StreamString stsr = new StreamString(pipeCltStream);

                taskresult = await Task.Run(() =>
                {
                    int timeout = 20000;

                    input0 = stsr.ReadString(timeout, WriteLine);
                    if (input0 == null)
                    {
                        WriteLine($"※Command_ConnnectStartAsync() ハンドシェイク失敗 timeout : {timeout} sec");
                        return false;
                    }
                    if (CheckFirstMessage(input0))
                    {
                        //DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" からの接続文字列{input0}は期待値です");
                        stsr.WriteString(CommandName);

                        DebugConsole.WriteLine($"◇ [{DateTime.Now}] ﾊﾟｲﾌﾟｻｰﾊﾞ:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" , ｺﾏﾝﾄﾞ:{CommandName} 接続しました。回答待ち・・・");

                        bool result = delegate_MainMethod(pipeCltStream);

                        //DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｻｰﾊﾞ] 接続先:\"\\\\{ServerHostname}\\PIPE\\{PipeName}\" コマンド:{CommandName} 処理関数の結果 {result} です");

                        return true;
                    }
                    else
                    {
                        WriteLine($"※\"\\\\{ServerHostname}\\{PipeName}\" からの接続文字列{input0}が期待と違います");
                        pipeCltStream.Close();

                        return false;
                    }

                });

                return true;
            }
            catch (Exception ex)
            {
                PipeConnectionStatus = false;

                if (pipeCltStream.IsConnected == false)
                {
                    WriteLine($"※\"\\\\{ServerHostname}\\{PipeName}\" コマンド:{CommandName} 、サーバーから途中で切断されました");
                }
                else
                {
                    Eventlog.Log.WriteEntry("TOYODATABASE", EventLogEntryType.Error, 6002, $"RemotePipeClient.Command_ConnnectStart(..) 、\"\\\\{ServerHostname}\\{PipeName}\" 例外発生 {ex.Message} ");
                }

                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        private bool CheckPing(string address, int Count = 1)
        {
            bool ans = false;
            Ping sender = new Ping();
            PingOptions options = new PingOptions
            {
                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                DontFragment = true
            };

            for (int i = 0; i < Count; i++)
            {
                try
                {
                    int timeout = 120;
                    string data = "aaaaaaaaaaaaaa";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    PingReply reply;
                    reply = sender.Send(address, timeout, buffer, options);

                    if (reply.Status == IPStatus.Success)
                    {
                        // Console.WriteLine("Reply from {0}: bytes={1} TTL={3}", reply.Address, reply.Buffer.Length,  reply.RoundtripTime);
                        ans = true;
                    }
                    else
                    {
                        ans = false;
                    }

                }
                catch (PingException pingex)
                {
                    var a = pingex;

                    //Console.WriteLine($"CheckPingで例外発生(宛先:{address}, Count変数={Count})：" + a.InnerException);
                }

                // ping送信の間隔を取る
                if (i < Count + 1)
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            return ans;
        }
    }

    /// <summary>
    /// ●サーバから返答された識別文字列が正規のものかをチェックする
    /// このアセンブリから利用されるデリゲートメソッドを定義する
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class RMCsupport
    {
        /// <summary>
        /// 作業中か？
        /// </summary>
        internal bool InSearchWorking { get; set; }

        public const string ConnectKeyword = @"Accept Ver 1.22.08";

        /// <summary>
        /// サーバーから返答された識別文字列をチェックする
        /// </summary>
        /// <param name="input0">サーバーからの文字列を指定</param>
        /// <returns></returns>
        internal bool CheckFirstMessage(string input0)
        {
            if (input0 == ConnectKeyword)
            {
                DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] ｻｰﾊﾞｰから送られたﾄｰｸﾝ \"{input0}\" は正規の値です");
                return true;
            }
            else if (input0 == @"BUSY")
            {
                DebugConsole.WriteLine("◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] ﾊﾟｲﾌﾟｻｰﾊﾞはBusyです。しばらくお待ちください");
                return false;
            }
            else
            {
                DebugConsole.WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄ] ｻｰﾊﾞｰから送られたﾄｰｸﾝ \"{input0}\" は認識できないﾄｰｸﾝです");

                return false;
            }
        }
    }
}
