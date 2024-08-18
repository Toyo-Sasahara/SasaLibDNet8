using Inventor;
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
        /// Ribbon/Command
        /// ■ファイルメニューにコマンドを追加
        /// 使用先プロジェクト InventorTOYOaddinCommit
        /// 使用先プロジェクト InventorTOYOaddin01
        /// 使用先プロジェクト InventorTOYOaddin02
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="buttonDefinition"></param>
        /// <param name="UseLargeIcon"></param>
        /// <param name="ShowText"></param>
        /// <param name="TargetControlInternalName"></param>
        /// <param name="InsertBeforeTargetControl"></param>
        public static void AddNewFileMenu(Inventor.Application oApp, Inventor.ButtonDefinition buttonDefinition, bool UseLargeIcon = false, bool ShowText = true, string TargetControlInternalName = "", bool InsertBeforeTargetControl = false)
        {
            //// File Controls (Application Menu)に ボタンを追加
            try
            {
                Inventor.CommandControls fileControls = oApp.UserInterfaceManager.FileBrowserControls;
                fileControls.AddButton(buttonDefinition, UseLargeIcon, ShowText, TargetControlInternalName, InsertBeforeTargetControl);
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"CreateToyoCOMMANDatFileMenu()で例外\n{ex.Message}");
            }

        }

        /// <summary>
        /// ■ファイルメニューにコマンドを追加
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="InternalName">ButtonDefinition を得るための内部名</param>
        /// <param name="UseLargeIcon"></param>
        /// <param name="ShowText"></param>
        /// <param name="TargetControlInternalName">新しいコントロールを隣に配置する既存のコントロールの内部名を指定</param>
        /// <param name="InsertBeforeTargetControl">この内部名を持つターゲットの前に挿入する</param>
        public static void AddNewFileMenu(Inventor.Application oApp, string InternalName, bool UseLargeIcon = false, bool ShowText = true, string TargetControlInternalName = "", bool InsertBeforeTargetControl = false)
        {
            //// File Controls (Application Menu)に ボタンを追加
            try
            {
                Inventor.CommandControls fileControls = oApp.UserInterfaceManager.FileBrowserControls;

                //create button definition
                ButtonDefinition buttonDefinition = oApp.CommandManager.ControlDefinitions[InternalName] as ButtonDefinition;

                fileControls.AddButton(buttonDefinition, UseLargeIcon, ShowText, TargetControlInternalName, InsertBeforeTargetControl);
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"CreateToyoCOMMANDatFileMenu()で例外\n{ex.Message}");
            }

        }

        /// <summary>
        /// Ribbon/Command
        /// ■コマンドカテゴリを作成
        /// 使用先プロジェクト InventorTOYOaddinCommit
        /// 使用先プロジェクト InventorTOYOaddin01
        /// 使用先プロジェクト InventorTOYOaddin02
        /// </summary>
        /// <param name="m_inventorApplication"></param>
        /// <param name="DisplayName"></param>
        /// <param name="InternalName"></param>
        /// <param name="addInCLSIDString"></param>
        /// <returns></returns>
        public static Inventor.CommandCategory AddNewCommandCategories(Inventor.Application m_inventorApplication, string DisplayName, string InternalName, object addInCLSIDString)
        {
            Inventor.CommandCategory cmdCategory;
            try
            {

                foreach(Inventor.CommandCategory category in  m_inventorApplication.CommandManager.CommandCategories)
                {
                    if (
                        (category.InternalName.ToLower() == InternalName.ToLower())
                        )
                    {
                        Debug.WriteLine($"Inventor.CommandCategory AddNewCommandCategories(...) カテゴリー登録済み DisplayName={category.DisplayName}, InternalName={category.InternalName}, ClientId={category.ClientId}");
                        cmdCategory = m_inventorApplication.CommandManager.CommandCategories[category.InternalName];
                        return cmdCategory;
                    }
                }

                cmdCategory = m_inventorApplication.CommandManager.CommandCategories.Add(DisplayName, InternalName, addInCLSIDString);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddNewCommandCategorie()で例外発生 \nDisplayName={DisplayName},\nInternalName={InternalName} \naddInCLSIDString={addInCLSIDString}\n ex.Message ={ex.Message}");
                cmdCategory = m_inventorApplication.CommandManager.CommandCategories[InternalName];
            }
            return cmdCategory;
        }
    }
}
