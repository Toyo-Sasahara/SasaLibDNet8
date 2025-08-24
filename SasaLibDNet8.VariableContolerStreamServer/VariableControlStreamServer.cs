using SasaLib;
using SasaLib.VariableControlPipeServer;
using StreamCommandBridge;
using System.Windows.Forms;
using ToyoStageService;
using ToyoStageService.StreamBasedServer;

public class VariableControlStreamServer
{
    public VariableControlStreamServer(int baseTcpPort, string pipeName, string handShareStr, Action<string> WriteLine)
    {
        /// ■抽象化サーバータスク生成
        Task.Run(async () =>
        {

            try
            {
                // 抽象化サーバータスクを生成
                await StreamBasedServerLauncher.StartMultipleServersAsync(
                    serverCount: 3,
                    baseTcpPort: baseTcpPort,
                    pipeName: pipeName,
                    maxTcpConnections: 5,
                    maxPipeInstances: 5,
                    serverHandshakeStartMSG: handShareStr,
                    startHandShakeReadWriteTimeOut: 5000,
                    commandHandler: ExecuteAsync,
                    WriteLine: WriteLine
                );


                WriteLine($"■StreamBasedServerLauncher.StartMultipleServersAsync() 実行完了");
            }
            catch (Exception ex)
            {
                WriteLine($"※await StreamBasedServerLauncher.StartMultipleServersAsync()の実行で例外発生\n{ex.Message}");
            }
        });

    }

    public async Task<bool> ExecuteAsync(ICommandContext context, string command, Action<string> WriteLine)
    {
        if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

        bool success = false;

        switch (command)
        {
            // ■
            case CMDNAME.GetValue:// 公開されているパブリックフィールド／パブリックプロパティを指定し値を表示
                                  //commandResult = oExecute_ValueContro.HandShakeProcess_GetVaule();
                break;
            case CMDNAME.GetValue_V2:// 公開されているパブリックフィールド／パブリックプロパティを指定し値を表示
                                     //commandResult = oExecute_ValueContro.HandShakeProcess_GetVaule_V2();
                break;
            case CMDNAME.SetValue: // 公開されているパブリックフィールド／パブリックプロパティを指定し値を設定
                                   //commandResult = oExecute_ValueContro.HandShakeProcess_SetVaule();
                break;
            case CMDNAME.SetValue_V2: // 公開されているパブリックフィールド／パブリックプロパティを指定し値を設定
                break;
            case CMDNAME.ListAllValue: // 公開されているパブリックフィールド／パブリックプロパティ すべての名前と値を調査
                break;
            case CMDNAME.ListAllValue_V2: // 公開されているパブリックフィールド／パブリックプロパティ すべての名前と値を調査
                break;
            case CMDNAME.WinFormMessageBox: // このライブラリに用意され自動クローズ機能付きダイアログメッセージ表示機能を使ってメッセージを表示させる
                break;
            case CMDNAME.WinFormMessageBox_V2: // このライブラリに用意され自動クローズ機能付きダイアログメッセージ表示機能を使ってメッセージを表示させる
                break;
            case CMDNAME.CAD_MessageBox: // CADアプリケーション側に準備されていればそれらのダイアログメッセージ表示機能を使ってメッセージを表示させる
                break;
            case CMDNAME.CAD_MessageBox_V2: // CADアプリケーション側に準備されていればそれらのダイアログメッセージ表示機能を使ってメッセージを表示させる
                break;
            case CMDNAME.CheckFileHash: // ファイルを指定し、ハッシュ情報を調査
                break;
            case CMDNAME.GetVersionInfo: // ファイルを指定し.Netバージョン情報を調査
                break;
            case CMDNAME.GetCurrentUserInformation: // 
                break;
            case CMDNAME.GetCurrentUserName: // 
                break;
            case CMDNAME.GetCurrentUserDomainName: // 
                break;
            case CMDNAME.GetCurrentUserDomainFullName: // 
                break;
            case CMDNAME.GetFileTimeStamp: // 
                break;
            case CMDNAME.XmlFileTagUpdate: // 
                break;
            case CMDNAME.XmlFileTagUpdate_V2: // 
                break;
            case CMDNAME.GetInstalledSoftwareVersion: // 
                break;
        }

        await Task.Delay(0);
        return true;
    }

}