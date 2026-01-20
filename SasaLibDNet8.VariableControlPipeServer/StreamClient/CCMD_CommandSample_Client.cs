using Microsoft.VisualStudio.TextManager.Interop;
using SasaLib;
using STAGINGSYSTEM_COMMANDS;
using StreamCommandBridge;
using StreamCommandBridge.StreamBasedClient;
using StreamCommandExecutorServer;
using System;
using System.Collections.Generic;


//using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

//namespace StreamCommandExecutorClient
//{
//    public static class CCMD_CommandSample_Client
//    {
//        public class Param
//        {
//            public Action<string> WriteLine = null;
//            public bool debugMode = false;
//        }

//        /// <summary>
//        /// （同期版）
//        /// </summary>
//        /// <param name="server"></param>
//        /// <param name="WriteLine"></param>
//        /// <returns></returns>
//        public static (bool Sucess, string Message) ExecuteSync(ServerInfo server,  Action<string> WriteLine)
//        {
//           var result= Task.Run(async () =>
//               await CCMD_CommandSample_Client.ExecuteAsync(server,  WriteLine)
//                   .ConfigureAwait(false)
//           ).GetAwaiter().GetResult();

//            if (result.Sucess)
//            {
//                return (true, result.Message);

//            }
//            else
//            {
//                return (false, result.Message);
//            }

//        }

//        /// <summary>
//        /// （非同期版）
//        /// </summary>
//        /// <param name="serverInfo"></param>
//        /// <param name="WriteLine"></param>
//        /// <returns></returns>
//        public static async Task<(bool Sucess, string Message)> ExecuteAsync(ServerInfo serverInfo, Action<string> WriteLine = null)
//        {
//            WriteLine($"CMD_CommitRecepitonState_Client");

//            StreamBasedClient streamConnection;
//            if (serverInfo.connectionType == ConnectionType.Tcp)
//            {
//                 streamConnection = new StreamBasedClient(
//                    new StreamProvider_Tcp(serverInfo.serverName, serverInfo.tcpPort, WriteLine),
//                    WriteLine);
//            }
//            else
//            {
//                streamConnection = new StreamBasedClient(
//                    new StreamProvider_Pipe(serverInfo.serverName, serverInfo.pipeName, WriteLine),
//                    WriteLine);
//            }


//            CCMD_CommandSample_Client.Param param = new CCMD_CommandSample_Client.Param()
//            {
//                WriteLine = Console.WriteLine
//            };

//            var result = await streamConnection.SendCommandAsync(
//                commandName: CMDS.DC_CommitRecepitonState, subCommandName: null,
//                ExecuteCommandByNameAsync: CCMD_CommandSample_Client.ExecuteAsync, commandParam: param,
//                userName: Environment.UserName
//            );

//            if (result.isError == false)
//            {
//                foreach (var a in result.ResultStrs)
//                {
//                    WriteLine($"{a}");
//                }
//                return (true, result.errorMsg);
//            }
//            else
//            {
//                return (false, result.errorMsg);
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="context"></param>
//        /// <param name="param"></param>
//        /// <returns></returns>
//        private static async Task<(bool isError, string errorMsg, List<string> ResultStrs)> ExecuteAsync(ICommandContext context, Param param)
//        {
//            Action<string> WriteLine = DebugConsole.WriteLine;
//            if (param.WriteLine != null) { WriteLine = param.WriteLine; }

//            bool isError = default;
//            string errorMessage = default;
//            List<string> result = default;

//            var _CmdLogWrite = context.CreateCmdLogger(WriteLine); // コマンド実行ログ共通化拡張目祖度d
//            var _timing = context.BeginTiming(_CmdLogWrite); // メソッド実行時間調査用拡張メソッド

//            try
//            {
//                _CmdLogWrite($"実行開始");

//                // コマンド受付・スタートメッセージを受信
//                var rseq1 = await context.ReadAcceptMsgWithLogAsync("①アクセプト受信", _CmdLogWrite);

//                var wseq2 = await context.WriteAcceptedMsgWithLogAsync("②アクセプト送信", _CmdLogWrite); // コマンド受付・スタートメッセージを送信

//                var rseq2 = await context.ReceiveResultWithLogAsync<List<string>>("③", "データタイプ(List<string>)受信", BinaryConvertTYPE.JsonSerializer, log:_CmdLogWrite);

//                isError = rseq2.IsError;
//                errorMessage = rseq2.ErrorMessage;
//                result = rseq2.Result;
//            }
//            catch (Exception ex)
//            {
//                isError = true;
//                errorMessage = ex.Message;
//            }
//            finally
//            {
//                _CmdLogWrite($"実行完了");
//                _timing.Dispose();
//            }

//            return (isError, errorMessage, result);
//        }
//    }
//}
