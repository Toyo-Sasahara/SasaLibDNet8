///
using SasaLib.PIPE;
using SasaLib.VariableControlPipeServer.ExecuteCommands;
using System.IO.Pipes;
using System.Runtime.Versioning;

namespace SasaLib.VariableControlPipeServer
{
    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal class CommandAnalyze
    {
        SasaLibDelegateWriteLine WriteLine;

        object targetObj;

        int hwnd;

        /// <summary>
        /// ■ファーストコマンドワード識別・分岐メソッド(ToyoDRAWCAPTUREservice)
        /// </summary>
        internal CommandAnalyze(string command, object targetOb, NamedPipeServerStream pipeSrvStream, int serverId, string namedPipeClient, string ImpersonationUserName, int clientProcessID, ref bool BUSYFLAG, SasaLibDelegateWriteLine writeLine = null, int hwnd = 0, Delegate_MessageBoxShow delegate_MessageBoxShow = null)
        {
            this.targetObj = targetOb;
            this.hwnd = hwnd;

            if (writeLine == null) this.WriteLine = DebugConsole.WriteLine; else this.WriteLine = writeLine;

            // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
            string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(pipeSrvStream, serverId);

            WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] 接続元:{ImpersonationUserName}: 【{command}】 受信");

            bool commandResult = false;

            // パブリック変数コントロール・コマンド受信と処理
            Execute_ValueControl oExecute_ValueContro = new Execute_ValueControl(serverId, pipeSrvStream, targetObj, WriteLine);

            // メッセージボックスコントロール・コマンド受信と処理
            Execute_MessageBox oExecute_MessageBox = new Execute_MessageBox(serverId, pipeSrvStream, WriteLine);

            // ファイルコントロール・コマンド受信と処理
            Execute_FileControl oExecute_FileControl = new Execute_FileControl(serverId, pipeSrvStream,  WriteLine);

            // MSI.DLL ハンドラに対するコマンド受信と処理
            Execute_MsiTool oExecute_MsiTool = new Execute_MsiTool(serverId , pipeSrvStream,  WriteLine);
            
            string errMsg=null;

            //■コマンドワード解析
            switch (command)
            {
                // ■
                case CMDNAME.GetValue:// 公開されているパブリックフィールド／パブリックプロパティを指定し値を表示
                    commandResult = oExecute_ValueContro.HandShakeProcess_GetVaule();
                    break;
                case CMDNAME.SetValue: // 公開されているパブリックフィールド／パブリックプロパティを指定し値を設定
                    commandResult = oExecute_ValueContro.HandShakeProcess_SetVaule();
                    break;
                case CMDNAME.ListAllValue: // 公開されているパブリックフィールド／パブリックプロパティ すべての名前と値を調査
                    commandResult = oExecute_ValueContro.HandShakeProcess_ListAllValue();
                    break;
                case CMDNAME.WinFormMessageBox: // このライブラリに用意され自動クローズ機能付きダイアログメッセージ表示機能を使ってメッセージを表示させる
                    commandResult = oExecute_MessageBox.HandShakeProcess_WinFormMessageBox(hwnd);
                    break;
                case CMDNAME.CAD_MessageBox: // CADアプリケーション側に準備されていればそれらのダイアログメッセージ表示機能を使ってメッセージを表示させる
                    commandResult = oExecute_MessageBox.HandShakeProcess_CAD_MessageBox(delegate_MessageBoxShow);
                    break;
                case CMDNAME.CheckFileHash: // ファイルを指定し、ハッシュ情報を調査
                    commandResult = oExecute_FileControl.HandShakeProcess_CheckFileHash(out errMsg);
                    break;
                case CMDNAME.GetVersionInfo: // ファイルを指定し.Netバージョン情報を調査
                    commandResult = oExecute_FileControl.HandShakeProcess_GetVersionInfo(out errMsg);
                    break;
                case CMDNAME.GetCurrentUserInformation: // 
                    commandResult = oExecute_MessageBox.HandShakeProcess_GetCurrentUserInformation(oExecute_MessageBox.GetCurrentUserInformation, out errMsg);
                    break;
                case CMDNAME.GetCurrentUserName: // 
                    commandResult = oExecute_MessageBox.HandShakeProcess_GetCurrentUserInformation(oExecute_MessageBox.GetCurrentUserName, out errMsg);
                    break;
                case CMDNAME.GetCurrentUserDomainName: // 
                    commandResult = oExecute_MessageBox.HandShakeProcess_GetCurrentUserInformation(oExecute_MessageBox.GetCurrentUserDomainName, out errMsg);
                    break;
                case CMDNAME.GetCurrentUserDomainFullName: // 
                    commandResult = oExecute_MessageBox.HandShakeProcess_GetCurrentUserInformation(oExecute_MessageBox.GetCurrentDomainUserFullName, out errMsg);
                    break;
                case CMDNAME.GetFileTimeStamp: // 
                    commandResult = oExecute_FileControl.HandShakeProcess_GetFileTimeStamp(out errMsg);
                    break;
                case CMDNAME.XmlFileTagUpdate: // 
                    commandResult = oExecute_ValueContro.HandShakeProcess_XmlFileTagUpdate(out errMsg);
                    break;
                case CMDNAME.GetInstalledSoftwareVersion: // 
                    commandResult = oExecute_MsiTool.HandShakeProcess_GetInstalledSoftwareVersion(out errMsg);
                    break;

                    
                // ■不明なコマンド
                default:
                    this.WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] ※[ID:{serverId}] : エラー　コマンド【{command}】は設定されていない");
                    break;
            }

            if (commandResult == false)
                this.WriteLine($"※[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] 接続元:{ImpersonationUserName}: 【{command}】は正常に終了しませんでした errMsg:{errMsg}");
            else
                this.WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ] [ID:{serverId}] 接続元:{ImpersonationUserName}: 【{command}】正常に終了しました");

        }
    }
}
