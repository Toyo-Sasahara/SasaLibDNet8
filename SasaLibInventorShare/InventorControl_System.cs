using SasaLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Inventor コントロール用共有ソース
/// </summary>
namespace SasaLib.InventorAPI
{
    public static partial class InventorControl
    {
        /// <summary>
        /// FileDialog
        /// ■ファイル選択ダイアログを表示(読込用)
        /// 使用先プロジェクト InventorTOYOaddinCommit
        /// 使用先プロジェクト InventorTOYOaddin01
        /// 使用先プロジェクト InventorTOYOaddin02
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="Title"></param>
        /// <param name="InitialDirectory"></param>
        /// <param name="Filter"></param>
        /// <returns></returns>
        public static string FileSelectForRead(Inventor.Application oApp, string Title, string InitialDirectory, string Filter)
        {
            Inventor.FileDialog oFileDlg;

            oApp.CreateFileDialog(out oFileDlg);
            oFileDlg.Filter = Filter;
            oFileDlg.FilterIndex = 1;
            oFileDlg.DialogTitle = Title;

            oFileDlg.InitialDirectory = InitialDirectory;

            oFileDlg.CancelError = true;
            try
            {
                oFileDlg.ShowOpen();

                return oFileDlg.FileName;
            }
            catch (Exception err)
            {
                DebugConsole.WriteLine($"※FileSelectForRead(..)にて例外検知 {err.Message} {err.InnerException}");
                return null;
            }


        }


        /// <summary>
        /// ControlDefinition
        /// ■Inventorコマンドを実行。フォームから実行不可
        /// </summary>
        /// <param name="oInventorApp"></param>
        /// <param name="CommandName"></param>
        public static void ExecuteInventorCommand(Inventor.Application oInventorApp, string CommandName, Action<string> LogWrite = null)
        {
            if (LogWrite != null) LogWrite($"■ｺﾏﾝﾄﾞ:{CommandName}を実行します");
            try
            {
                Inventor.ControlDefinition cmdCategory = oInventorApp.CommandManager.ControlDefinitions[CommandName];
                cmdCategory.Execute();
                if (LogWrite != null) LogWrite($"■{CommandName}を実行完了しました");
            }
            catch (Exception ex)
            {
                if (LogWrite != null) LogWrite($"※ｺﾏﾝﾄﾞ:{CommandName} の実行に失敗しました {ex.Message}");
            }
        }

        /// <summary>
        /// ■"AppLocalUpdateCmd" (ローカル更新を実行)
        /// </summary>
        /// <param name="oInventorApp"></param>
        public static void ExecuteAppLocalUpdateCmd(Inventor.Application oInventorApp)
        {
            ExecuteInventorCommand(oInventorApp, "AppLocalUpdateCmd");
        }



        /// <summary>
        /// ■"AppIsometricViewCmd" (ホームビューを実行)
        /// </summary>
        /// <param name="oInventorApp"></param>
        public static void ExecutAppIsometricViewCmd(Inventor.Application oInventorApp)
        {
            var ans = oInventorApp.CommandManager.ControlDefinitions["AppIsometricViewCmd"];
            ans.Execute();

        }

    }
}
