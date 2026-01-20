using Google.Protobuf.WellKnownTypes;
using SasaLib;
using StreamCommandBridge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ToyoStageService;

//namespace StreamCommandExecutorServer
//{
//#if NETCOREAPP
//    [SupportedOSPlatform("windows")]
//#endif
//    internal static class CCMD_SeValue_Server
//    {
//        internal static async Task<bool> ExecuteAsync(ICommandContext context, object targetObj, Action<string> WriteLine)
//        {
//            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

//            var _CmdLogWrite = context.CreateCmdLogger(WriteLine); // コマンド実行ログ共通化拡張目祖度d
//            var _timing = context.BeginTiming(_CmdLogWrite); // メソッド実行時間調査用拡張メソッド

//            _CmdLogWrite($"実行開始");

//            try
//            {
//                string remoteHost = context.RemoteHost;
//                string remoteUser = context.RemoteUser;

//                var wseq1 = await context.WriteAcceptedMsgWithLogAsync("①アクセプト送信", log: _CmdLogWrite); // コマンド受付・スタートメッセージを送信

//                var rseq2 = await context.ReceiveResultWithLogAsync<string>("②", "変数名", BinaryConvertTYPE.JsonSerializer, log: _CmdLogWrite);
//                var VariableName = rseq2.Result;

//                System.Type targetObjSystemType = targetObj.GetType();
//                WriteLine($"オブジェクト {targetObj.ToString()} から {VariableName} を取得します");
//                object value = null;
//                bool result = GetMemberValue(targetObj, VariableName, out value);


//                var options = new JsonSerializerOptions { Converters = { new ObjectWithTypeJsonConverter(WriteLine) }, WriteIndented = true };

//                ObjectWithType objectWithType = new ObjectWithType { TypeName = targetObj.GetType().AssemblyQualifiedName, Data = value }; // 指定したフィールドをカプセル化

//                var wseq3 = await context.SendResultWithLogAsync("③", "変更前データ(ObjectWithType)", objectWithType, BinaryConvertTYPE.JsonSerializer, options, _CmdLogWrite);

//                var rseq4 = await context.ReceiveResultWithLogAsync<ObjectWithType>("③", "データ受信(ObjectWithType)", BinaryConvertTYPE.JsonSerializer, options: options, log: _CmdLogWrite);
//                var oType = rseq4.Result;

//                SetMemberValue(targetObj, VariableName, oType.Data); // 変数設定

//                if (string.IsNullOrWhiteSpace(VariableName) == false)
//                {


//                }
//                else
//                {

//                    return false;
//                }

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


//            /// <summary>
//            /// オブジェクトから指定した名前を持つパブリック プロパティまたはフィールドの値をリフレクションで取得します。
//            /// </summary>
//            /// <param name="target"></param>
//            /// <param name="memberName"></param>
//            /// <param name="value"></param>
//            /// <returns></returns>
//            /// <exception cref="ArgumentNullException"></exception>
//            /// <exception cref="ArgumentException"></exception>

//        }

//        private static  bool GetMemberValue(object target, string memberName, out object value)
//        {
//            value = null;

//            if (target == null)
//                throw new ArgumentNullException(nameof(target), "対象のオブジェクトが null です。");
//            if (string.IsNullOrWhiteSpace(memberName))
//                throw new ArgumentException("メンバー名を指定してください。", nameof(memberName));

//            // ターゲットの型を取得
//            var type = target.GetType();

//            // プロパティを検索
//            var property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
//            if (property != null)
//            {
//                // プロパティから値を取得
//                value = property.GetValue(target);
//                return true;
//            }

//            // フィールドを検索
//            var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.Instance);
//            if (field != null)
//            {
//                // フィールドから値を取得
//                value = field.GetValue(target);
//                return true;
//            }


//            return false;
//        }

//        /// <summary>
//        /// オブジェクトの指定されたパブリック プロパティまたはフィールドの値をリフレクションで設定します。
//        /// </summary>
//        /// <param name="target">対象のオブジェクト。</param>
//        /// <param name="memberName">書き換えるプロパティまたはフィールドの名前。</param>
//        /// <param name="value">設定する値。</param>
//        /// <exception cref="ArgumentException">プロパティやフィールドが見つからない場合にスローされます。</exception>
//        private static void SetMemberValue(object target, string memberName, object value)
//        {
//            if (target == null)
//                throw new ArgumentNullException(nameof(target), "対象のオブジェクトが null です。");
//            if (string.IsNullOrWhiteSpace(memberName))
//                throw new ArgumentException("メンバー名を指定してください。", nameof(memberName));


//            // ターゲットの型を取得
//            var type = target.GetType();

//            // プロパティを検索
//            var property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
//            if (property != null)
//            {
//                // プロパティが読み取り専用の場合は例外をスロー
//                if (!property.CanWrite)
//                {
//                    throw new ArgumentException($"プロパティ '{memberName}' は書き込み不可です。");
//                }

//                // プロパティに値を設定
//                property.SetValue(target, Convert.ChangeType(value, property.PropertyType));
//                return;
//            }

//            // フィールドを検索
//            var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.Instance);
//            if (field != null)
//            {
//                // フィールドに値を設定
//                field.SetValue(target, Convert.ChangeType(value, field.FieldType));
//                return;
//            }

//            // プロパティまたはフィールドが見つからなかった場合
//            throw new ArgumentException($"'{memberName}' という名前のプロパティまたはフィールドは存在しません。", nameof(memberName));
//        }

//    }
//}
