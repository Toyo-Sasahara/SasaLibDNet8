using SasaLib;
using StreamCommandBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyoStageService;

//namespace StreamCommandExecutorServer
//{
//    internal static class CCMD_CommandSample_Server
//    {
//        internal static async Task<bool> ExecuteAsync(ICommandContext context, Action<string> WriteLine)
//        {
//            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

//            var _CmdLogWrite = context.CreateCmdLogger(WriteLine); // コマンド実行ログ共通化拡張目祖度d
//            var _timing = context.BeginTiming(_CmdLogWrite); // メソッド実行時間調査用拡張メソッド
            
//            _CmdLogWrite($"実行開始");

//            try
//            {
//                string remoteHost = context.RemoteHost;
//                string remoteUser = context.RemoteUser;

//                var wseq1 = await context.WriteAcceptedMsgWithLogAsync("①アクセプト送信", _CmdLogWrite); // コマンド受付・スタートメッセージを送信

//                var rseq2 = await context.ReadAcceptMsgWithLogAsync("②アクセプト受信", _CmdLogWrite);

//                List<string> sendData = new List<string> { "AA", "BB", "CC" };
//                var wseq3 =  await context.SendResultWithLogAsync("③", "データタイプ(List<string>)送信", sendData, BinaryConvertTYPE.JsonSerializer, log: _CmdLogWrite);


//                var rseq4 = await context.ReceiveResultWithLogAsync<List<string>>("④", "データタイプ(List<string>)受信", BinaryConvertTYPE.JsonSerializer, log: _CmdLogWrite);
//                var zubanList = rseq4.Result;
//                var wseq5 = await context.SendResultWithLogAsync("⑤", "データタイプ(List<string>)送信", zubanList, BinaryConvertTYPE.JsonSerializer, log: _CmdLogWrite);
//                return true;

//            }
//            catch (Exception ex)
//            {
//                await context.SendErrorWithLogAsync("①", $"CMD_CommitRecepitonState.ExecuteAsync {ex.Message}", _CmdLogWrite);
//                return false;
//            }
//            finally
//            {
//                _CmdLogWrite($"実行完了");
//                _timing.Dispose();
//            }
//        }
//    }
//}
