///// ToyoSTAGINGSYSTEMwatch service用 PIPEconnectionLoop
//using SasaLib.PIPE;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.IO;
//using System.IO.Pipes;
//using System.Reflection;
//using System.Runtime.Versioning;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace SasaLib.VariableControlPipeServer
//{

//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    internal class Execute_ValueControl
//    {

//        SasaLibDelegateWriteLine WriteLine;


//        object targetObj;

//        public int ServerId { get; private set; }

//        NamedPipeServerStream pipeSrvStream;


//        /// <summary>
//        /// ■GlovalValues.ServerVersionを返す
//        /// </summary>
//        /// <param name="serverId"></param>
//        /// <param name="pipeSrvStream"></param>
//        internal Execute_ValueControl(int serverId, NamedPipeServerStream pipeSrvStream, Object targetObj, SasaLibDelegateWriteLine WriteLine)
//        {
//            this.ServerId = serverId;
//            this.pipeSrvStream = pipeSrvStream;
//            this.targetObj = targetObj;
//            this.WriteLine = WriteLine;
//        }

//        /// <summary>
//        /// パブリッククラス・パブリックフィールドを外部から読み出し
//        /// </summary>
//        /// <returns></returns>
//        internal bool HandShakeProcess_GetVaule()
//        {
//            try
//            {
//                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
//                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(pipeSrvStream, ServerId);


//                StreamString stst = new StreamString(pipeSrvStream);

//                stst.WriteString("OK. Send CommitConfig Variable name.");

//                int timeout = 5000;

//                string VariableName = stst.ReadString(timeout, WriteLine);

//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] CommandSubAnalyze_CommitConfigVaule(..) {clientInfo} よりVariableNameを受信しました。\"{VariableName}\"");

//                Type targetObjSystemType = targetObj.GetType();

//                FieldInfo field = targetObjSystemType.GetField(VariableName);
//                if (field != null)
//                {
//                    using (var writer = new BinaryWriter(pipeSrvStream, Encoding.UTF8, true))
//                    {
//                        writer.WriteObject(field.FieldType);
//                    }
//                    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] 変数 \"{VariableName}\" の 型:{field.FieldType} を  \"{clientInfo}\"  へ送信しました");

//                    var valule = field.GetValue(targetObj);
//                    using (var writer = new BinaryWriter(pipeSrvStream, Encoding.UTF8, true))
//                    {
//                        writer.WriteObject(valule, timeout);
//                    }
//                    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] 変数 \"{VariableName}\" の 値:{valule}　を  \"{clientInfo}\"  へ送信しました");


//                    return true;
//                }
//                else
//                {
//                    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] 変数 \"{VariableName}\" は定義されていません (FieldInfo null)");
//                    return false;
//                }
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// パブリッククラス・パブリックフィールドを外部から設定
//        /// </summary>
//        /// <returns></returns>
//        internal bool HandShakeProcess_SetVaule()
//        {
//            try
//            {
//                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
//                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(pipeSrvStream, ServerId);


//                StreamString stst = new StreamString(pipeSrvStream);

//                stst.WriteString("OK. Send CommitConfig Variable name.");
//                int timeout = 5000;
//                string VariableName = stst.ReadString(timeout, WriteLine);

//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] HandShakeProcess_SetVaule() {clientInfo}から受信。VariableName = \"{VariableName}\"");


//                Type typeOfListString = targetObj.GetType();

//                FieldInfo field = typeOfListString.GetField(VariableName);

//                if (field != null)
//                {
//                    using (var writer = new BinaryWriter(pipeSrvStream, Encoding.UTF8, true))
//                    {
//                        writer.WriteObject(field.FieldType);
//                    }
//                    stst.WriteString("OK. Send Object Data.");

//                    object receveObj;
//                    using (BinaryReader reader = new BinaryReader(pipeSrvStream, Encoding.UTF8, true))
//                    {
//                        receveObj = reader.ReadObject<Object>();
//                    }
//                    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] receveObj = \"{receveObj}\"");

//                    field.SetValue(targetObj, receveObj);

//                    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] HandShakeProcess_SetVaule() スタティックオブジェクト \"{targetObj}\" の \"{VariableName}\" に \"{receveObj}\" （{field.FieldType} 型）を代入しました");

//                    return true;

//                }
//                else
//                {
//                    WriteLine($"※[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] HandShakeProcess_SetVaule() スタティック変数 {targetObj} オブジェクトに {VariableName} は見つかりませんでした");

//                    return false;
//                }

//            }
//            catch
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// パブリッククラスのオブジェクトをすべて呼出し
//        /// </summary>
//        /// <returns></returns>
//        internal bool HandShakeProcess_ListAllValue()
//        {
//            try
//            {
//                List<string> datas = ListValueSrings(targetObj);

//                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
//                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(pipeSrvStream, ServerId);

//                using (var writer = new BinaryWriter(pipeSrvStream, Encoding.UTF8, true))
//                {
//                    writer.WriteObject(datas);
//                }


//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] List<string>型 を  {clientInfo}  へ送信しました");



//                return true;

//            }
//            catch
//            {
//                return false;
//            }

//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="targetObj"></param>
//        /// <returns></returns>
//        List<string> ListValueSrings(Object targetObj)
//        {
//            Type targetObjSystemType = targetObj.GetType();

//            List<string> result = new List<string>();

//            FieldInfo[] myFieldInfos;
//            Type myType = targetObjSystemType;

//            // Get the type and fields of FieldInfoClass.
//            myFieldInfos = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

//            foreach (FieldInfo myFieldInfo in myFieldInfos)
//            {
//                string valuestr;
//                try
//                {

//                    Type type = myFieldInfo.FieldType;

//                    var valule = myFieldInfo.GetValue(targetObj);

//                    valuestr = (string)Convert.ChangeType(valule, TypeCode.String);

//                }
//                catch (Exception ex)
//                {
//                    WriteLine($"例外検知 {ex.Message} {myFieldInfo.Name} = {myFieldInfo.GetValue(targetObj)}");
//                    valuestr = "例外発生";
//                }

//                result.Add($"変数名: {myFieldInfo.Name}\r\n" +
//                    $"\t 値: {valuestr}\r\n" +
//                    $"\t 型: {myFieldInfo.FieldType}\r\n" +
//                    $"\tメンバータイプ: {myFieldInfo.MemberType}\r\n" +
//                    $"\tIsPublic: {myFieldInfo.IsPublic} IsFamily: {myFieldInfo.IsFamily}\r\n");

//            }
//            return result;
//        }

//        /// <summary>
//        /// XMLの指定した要素名を別の要素名に置き換え、値も変更。新しいファイルにて保存
//        /// </summary>
//        /// <param name="errMsg"></param>
//        /// <returns></returns>
//        /// <exception cref="NotImplementedException"></exception>
//        internal bool HandShakeProcess_XmlFileTagUpdate(out string errMsg)
//        {
//            errMsg = null;

//            int timeout = 5000;
//            try
//            {
//                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
//                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(pipeSrvStream, ServerId);


//                StreamString stst = new StreamString(pipeSrvStream);

//                stst.WriteString("OK. Send XmlConfigFile FullPath.");

//                string XmlConfigFileFullPath = stst.ReadString(timeout, WriteLine); // ①XmlConfigFileFullPath

//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] CommandSubAnalyze_CommitConfigVaule(..) {clientInfo} より XmlConfigFileFullPath を受信しました。{XmlConfigFileFullPath} ");

//                stst.WriteString("OK. Send CurrentElement.");

//                string CurrentElement = stst.ReadString(timeout, WriteLine); // ②CurrentElement

//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] CommandSubAnalyze_CommitConfigVaule(..) {clientInfo} より CurrentElement を受信しました。{CurrentElement} ");

//                stst.WriteString("OK. Send NewElement.");

//                string NewElement = stst.ReadString(timeout, WriteLine); // ③NewElement

//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] CommandSubAnalyze_CommitConfigVaule(..) {clientInfo} より NewElement を受信しました。{NewElement} ");

//                stst.WriteString("OK. Send Value.");

//                string Value = stst.ReadString(timeout, WriteLine); // ④Value

//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] CommandSubAnalyze_CommitConfigVaule(..) {clientInfo} より Value を受信しました。{Value} ");

//                if (System.IO.File.Exists(XmlConfigFileFullPath) == false)
//                {
//                    errMsg = $"{XmlConfigFileFullPath} がみつかりません";
//                    return false;
//                }

//                bool xmlUpdateResult = XmlModify.XmlTagUpdate(XmlConfigFileFullPath, CurrentElement, NewElement, Value);

//                if (xmlUpdateResult == true)
//                {
//                    using (var writer = new BinaryWriter(pipeSrvStream, Encoding.UTF8, true))
//                    {
//                        writer.WriteObject(true);
//                    }

//                    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] true を  {clientInfo}  へ送信しました");

//                    return true;
//                }
//                else
//                {
//                    using (var writer = new BinaryWriter(pipeSrvStream, Encoding.UTF8, true))
//                    {
//                        writer.WriteObject(false);
//                    }

//                    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] false を  {clientInfo}  へ送信しました");

//                    errMsg = $"SasaLib.XmlModify.XmlTagUpdate(..) の戻り値が false でした";
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
//                errMsg = $"HandShakeProcess_XmlFileTagUpdate(..) にて例外検知 {ex.Message}";
//                return false;
//            }
//        }
//    }
//}
