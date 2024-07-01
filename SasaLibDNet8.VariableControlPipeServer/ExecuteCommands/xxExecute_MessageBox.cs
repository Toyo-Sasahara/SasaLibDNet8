//using SasaLib;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.IO.Pipes;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.DirectoryServices;
//using System.Threading;
//using SasaLib.PIPE;
//using System.Runtime.Versioning;

//namespace SasaLib.VariableControlPipeServer.ExecuteCommands
//{
//    [SupportedOSPlatform("windows")]
//    internal class Execute_MessageBox
//    {

//        NamedPipeServerStream PipeSrvStream { get; }
//        int ServerId { get; }
//        SasaLibDelegateWriteLine WriteLine { get; }

//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        /// <param name="serverId"></param>
//        /// <param name="pipeSrvStream"></param>
//        /// <param name="writeLine"></param>
//        internal Execute_MessageBox(int serverId, NamedPipeServerStream pipeSrvStream, SasaLibDelegateWriteLine writeLine)
//        {
//            PipeSrvStream = pipeSrvStream;
//            ServerId = serverId;
//            WriteLine = writeLine;
//        }



//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="hwnd"></param>
//        /// <returns></returns>
//        internal bool HandShakeProcess_WinFormMessageBox(int hwnd)
//        {
//            try
//            {
//                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
//                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(PipeSrvStream, ServerId);

//                // -------------------------------------------------------------

//                StreamString stst = new StreamString(PipeSrvStream);

//                stst.WriteString("OK. Send Window Title.");

//                int timeout = 5000;

//                string title = stst.ReadString(timeout, WriteLine);

//                WriteLine($"{clientInfo}よりﾀｲﾄﾙを受信しました。{title} ");

//                // -------------------------------------------------------------

//                stst.WriteString("OK. Send Message.");

//                string message = stst.ReadString(timeout, WriteLine);

//                WriteLine($"{clientInfo}よりﾒｯｾｰｼﾞを受信しました。{message} ");

//                // -------------------------------------------------------------

//                stst.WriteString("OK. Send MessageBoxButtons");

//                object receveObj;
//                using (BinaryReader reader = new BinaryReader(PipeSrvStream, Encoding.UTF8, true))
//                {
//                    receveObj = reader.ReadObject<Object>();
//                }
//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] receveObj = \"{receveObj}\"");

//                MessageBoxButtons messageBoxButtons = (MessageBoxButtons)receveObj;
//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] {clientInfo}よりオブジェクトを受信しました。{messageBoxButtons} ");

//                // -------------------------------------------------------------

//                stst.WriteString("OK. Send messageBoxIcon");

//                object receveObj2;
//                using (BinaryReader reader = new BinaryReader(PipeSrvStream, Encoding.UTF8, true))
//                {
//                    receveObj2 = reader.ReadObject<Object>();
//                }
//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] receveObj2 = \"{receveObj2}\"");

//                MessageBoxIcon messageBoxIcon = (MessageBoxIcon)receveObj2;
//                WriteLine($"{clientInfo}よりメッセージボックスアイコン オブジェクトを受信しました。{messageBoxIcon} ");

//                // -------------------------------------------------------------

//                stst.WriteString("OK. Send Auto close time (msec)");

//                int receveObj3;
//                using (BinaryReader reader = new BinaryReader(PipeSrvStream, Encoding.UTF8, true))
//                {
//                    receveObj3 = reader.ReadObject<int>();
//                }
//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] receveObj3 = \"{receveObj3}\"");
//                int closeTimeMsec = receveObj3;
//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] {clientInfo}より自動クローズ時間を受信しました。{closeTimeMsec} msec");

//                // -------------------------------------------------------------
//                WindowControl windowControl = new WindowControl();
//                IWin32Window owner = windowControl.GetWindowFromHost(hwnd);

//                AutoCloseMessageBoxForm autoCloseMessageBoxForm = new AutoCloseMessageBoxForm(message, title, messageBoxButtons, messageBoxIcon);
//                autoCloseMessageBoxForm.timeRemaining = closeTimeMsec;

//                if (hwnd == 0)
//                    autoCloseMessageBoxForm.TopMost = true;
//                else
//                    autoCloseMessageBoxForm.TopMost = false;


//                DialogResult dialogRes = autoCloseMessageBoxForm.ShowDialog(owner);

//                // -------------------------------------------------------------

//                WriteLine($"DialogResult値を返します：{dialogRes} ");

//                using (var writer = new BinaryWriter(PipeSrvStream, Encoding.UTF8, true))
//                {
//                    writer.WriteObject(dialogRes);
//                }

//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        /// <summary>
//        /// CADクライアントへメッセージボックスを表示
//        /// </summary>
//        /// <param name="delegate_MessageBoxShow">CADアドイン側で用意するメッセージボックス表示メソッド</param>
//        /// <returns></returns>
//        internal bool HandShakeProcess_CAD_MessageBox(Delegate_MessageBoxShow delegate_MessageBoxShow)
//        {
//            if (delegate_MessageBoxShow == null)
//            {
//                delegate_MessageBoxShow = InternalUserMsgBoxShow;
//            } // CAD側からメッセージボックス表示メソッドが提供されなかった場合

//            try
//            {
//                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
//                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(PipeSrvStream, ServerId);


//                StreamString stst = new StreamString(PipeSrvStream);

//                stst.WriteString("OK. Send Window Title.");

//                int timeout = 5000;

//                string title = stst.ReadString(timeout, WriteLine);

//                WriteLine($"{clientInfo}よりﾀｲﾄﾙを受信しました。{title} ");

//                stst.WriteString("OK. Send Message.");

//                string message = stst.ReadString();

//                WriteLine($"{clientInfo}よりﾒｯｾｰｼﾞを受信しました。{message} ");

//                stst.WriteString("OK. Send MessageBoxButtons");

//                object receveObj;
//                using (BinaryReader reader = new BinaryReader(PipeSrvStream, Encoding.UTF8, true))
//                {
//                    receveObj = reader.ReadObject<Object>();
//                }
//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] receveObj = \"{receveObj}\"");

//                MessageBoxButtons messageBoxButtons = (MessageBoxButtons)receveObj;
//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] {clientInfo}よりオブジェクトを受信しました。{messageBoxButtons} ");

//                stst.WriteString("OK. Send messageBoxIcon");

//                object receveObj2;
//                using (BinaryReader reader = new BinaryReader(PipeSrvStream, Encoding.UTF8, true))
//                {
//                    receveObj2 = reader.ReadObject<Object>();
//                }
//                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{ServerId}] receveObj2 = \"{receveObj2}\"");

//                MessageBoxIcon messageBoxIcon = (MessageBoxIcon)receveObj2;
//                WriteLine($"{clientInfo}よりオブジェクトを受信しました。{messageBoxIcon} ");


//                DialogResult dialogRes = delegate_MessageBoxShow(message, title, messageBoxButtons, messageBoxIcon);

//                WriteLine($"DialogResult値を返します：{dialogRes} ");

//                using (var writer = new BinaryWriter(PipeSrvStream, Encoding.UTF8, true))
//                {
//                    writer.WriteObject(dialogRes);
//                }

//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        DialogResult InternalUserMsgBoxShow(string Message, string Title, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon)
//        {
//            var result = System.Windows.Forms.MessageBox.Show(Message, Title, messageBoxButtons, messageBoxIcon);
//            return result;
//        }


//        /// ------------------------------------------------------------- ///

//        /// <summary>
//        /// ログオンユーザー情報を取得するデリゲート関数
//        /// </summary>
//        /// <returns></returns>
//        internal delegate string GetUserInformaiton();

//        /// <summary>
//        /// ログオンユーザー情報を取得 
//        /// </summary>
//        /// <param name="method"></param>
//        /// <param name="errMsg"></param>
//        /// <returns></returns>
//        [SupportedOSPlatform("windows")]
//        internal bool HandShakeProcess_GetCurrentUserInformation(GetUserInformaiton method, out string errMsg)
//        {
//            try
//            {
//                string userinformation = method();

//                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
//                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(PipeSrvStream, ServerId);

//                // -------------------------------------------------------------

//                StreamString stst = new StreamString(PipeSrvStream);

//                stst.WriteString(userinformation);

//                errMsg = null;
//                return true;
//            }
//            catch (Exception ex)
//            {
//                errMsg = $"HandShakeProcess_GetApplicationUser(..) 例外検知 {ex.Message}";
//                return false;
//            }
//        }


//        /// <summary>
//        /// ログオンユーザー情報を取得 Environment.UserDomainName
//        /// </summary>
//        /// <returns></returns>
//        internal string GetCurrentUserDomainName()
//        {
//            string result = Environment.UserDomainName;
//            return result;
//        }

//        /// <summary>
//        /// ログオンユーザー情報を取得 Environment.UserName
//        /// </summary>
//        /// <returns></returns>
//        internal string GetCurrentUserName()
//        {
//            string result = Environment.UserName;
//            return result;
//        }

//        /// <summary>
//        /// ログオンユーザー情報を取得 DirectoryEntry()を使って ドメイン上のフルネームを取得
//        /// </summary>
//        /// <returns></returns>
//        internal string GetCurrentDomainUserFullName()
//        {
//            string uName = Environment.UserName;
//            string domain = Environment.UserDomainName;

//            string domain_path = $"WinNT://{domain}/{uName}";

//            DirectoryEntry dirEnt = new DirectoryEntry(domain_path);

//            string result  = dirEnt.Properties["FullName"].Value.ToString();

//            return result;
//        }

//        /// <summary>
//        /// ログオンユーザーの 各種情報を１行で出力
//        /// </summary>
//        /// <returns></returns>
//        internal string GetCurrentUserInformation()
//        {
//            string uName = Environment.UserName;
//            string domain = Environment.UserDomainName;

//            string domain_path = $"WinNT://{domain}/{uName}";

//            DirectoryEntry dirEnt = new DirectoryEntry(domain_path);
//            string domain_UserFullName = dirEnt.Properties["FullName"].Value.ToString();

//            string result = $"ログオンユーザー:{uName} , 所属ドメイン:{domain} , ドメイン上フルネーム:{domain_UserFullName}";

//            return result;
//        }
//    }
//}
