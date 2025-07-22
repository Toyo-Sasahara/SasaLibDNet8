using System;
using System.Runtime.InteropServices;

namespace SasaLib.InventorAPI
{
    /// <summary>
    /// 既存のリボンにタブ・パネル・ボタンを追加
    /// </summary>
    public class ButtonPanelTabToRibbon
    {
        /// <summary>
        /// Inventor　アプリケーションオブジェクト
        /// </summary>
        private static Inventor.Application oApp;

        /// <summary>
        /// リボンパネルオブジェクト
        /// </summary>
        private Inventor.RibbonPanel RibbonPanel;

        /// <summary>
        /// addin識別用GUID
        /// </summary>
        private static GuidAttribute addinGUID;

        /// <summary>
        /// デリゲートの定義
        /// </summary>
        private Action<string> LogWrite;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Application"></param>
        /// <param name="addInCLSID"></param>
        public ButtonPanelTabToRibbon(Inventor.Application Application, GuidAttribute addInCLSID, Action<string> LogWrite = null)
        {
            oApp = Application;

            addinGUID = addInCLSID;

            this.LogWrite = LogWrite;
        }

        /// <summary>
        /// パネルを追加します。
        /// </summary>
        /// <param name="PanelDisplayName">パネル表示名</param>
        /// <param name="PanelInternalName">パネル内部名</param>
        /// <param name="TabDisplayName">タブ表示名</param>
        /// <param name="TabInternalName">タブ内部名</param>
        /// <param name="RibbonInternalName">リボン内部名</param>
        public void AddPanel(string PanelDisplayName, string PanelInternalName, string TabDisplayName, string TabInternalName, string RibbonInternalName)
        {
            if (LogWrite == null)
                LogWrite = DebugConsole.WriteLine;

            try
            {
                // 1. リボン"RibbonInternalName"を取得
                Inventor.Ribbon ribbon = oApp.UserInterfaceManager.Ribbons[RibbonInternalName];


                // 2. リボン"RibbonInternalName"に タブ"TabInternalName"を定義もしくは取得
                Inventor.RibbonTab tab = addNewTabtoRibbon(ribbon, TabDisplayName, TabInternalName, addinGUID.Value, LogWrite);

                // 3. リボン"RibbonInternalName"の、タブ"TabInternalName"に、パネル"PanelInternalName" を定義もしくは取得
                RibbonPanel = addNewPaneltoTab(tab, PanelDisplayName, PanelInternalName, addinGUID.Value, LogWrite);

            }
            catch (Exception ex)
            {
                LogWrite($"※ButtonPanelTabToRibbon.Add(..)にて例外  PanelDisplayName={PanelDisplayName} , PanelInternalName={PanelInternalName} , TabDisplayName = {TabDisplayName}{TabInternalName} , RibbonInternalName = {RibbonInternalName}\n{ex.Message}");
            }
        }

        /// <summary>
        /// パネルを削除します
        /// </summary>
        public void DeletePanel()
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            string PanelInernalName=null;
            try
            {
                PanelInernalName = RibbonPanel.InternalName;
                RibbonPanel.Delete();
                LogWrite($"Delete(..) パネル(内部名:[{PanelInernalName}])を削除しました ");
            }
            catch (Exception ex)
            {
                if (LogWrite != null) LogWrite($"※RibbonPanel.Delete()にて例外発生.{ex.Message} パネル(内部名:[{PanelInernalName}])の削除を失敗しました");
            }
        }

        /// <summary>
        /// パネルにボタンを追加します。タイプ①
        /// </summary>
        /// <param name="ButtonDefinition"></param>
        /// <param name="UseLargeIcon"></param>
        /// <param name="ShowText"></param>
        /// <param name="TargetControlInternalName"></param>
        /// <param name="InsertBeforeTargetControl"></param>
        /// <returns></returns>
        public Inventor.CommandControl AddButon(Inventor.ButtonDefinition ButtonDefinition, bool UseLargeIcon = false, bool ShowText = true, string TargetControlInternalName = "", bool InsertBeforeTargetControl = false)
        {
            if (LogWrite == null)
                LogWrite = DebugConsole.WriteLine;

            Inventor.CommandControl ret = default(Inventor.CommandControl);
            try
            {
                    ret = RibbonPanel.CommandControls.AddButton(ButtonDefinition, UseLargeIcon, ShowText, TargetControlInternalName, InsertBeforeTargetControl);
                    //if (LogWrite != null) LogWrite($"→→◆AddButton(..)ﾘﾎﾞﾝ[{RibbonPanel.Parent.Parent.InternalName}] , ﾀﾌﾞ[{RibbonPanel.Parent.DisplayName}({RibbonPanel.Parent.InternalName})] , ﾊﾟﾈﾙ[{RibbonPanel.DisplayName} ({RibbonPanel.InternalName})] へ ﾎﾞﾀﾝ[{ButtonDefinition.DisplayName} ({ButtonDefinition.InternalName})] を追加しました");
                    return ret;
            }
            catch (Exception ex)
            {
                if (LogWrite != null) LogWrite($"→→※AddButton(..)ﾘﾎﾞﾝ[{RibbonPanel.Parent.Parent.InternalName}] , ﾀﾌﾞ[{RibbonPanel.Parent.DisplayName}({RibbonPanel.Parent.InternalName})] , ﾊﾟﾈﾙ[{RibbonPanel.DisplayName} ({RibbonPanel.InternalName})] へ ﾎﾞﾀﾝ[{ButtonDefinition.DisplayName} ({ButtonDefinition.InternalName})] {ex.Message}");
                return ret;
            }
        }

        /// <summary>
        /// パネルにボタンを追加します。タイプ②（テキスト文字列のCOMMANDNAMEを使用します）
        /// </summary>
        /// <param name="iApp">Inventor.Applicationｵﾌﾞｼﾞｪｸﾄ</param>
        /// <param name="COMMANDNAME">コマンド名</param>
        /// <param name="UseLargeIcon"></param>
        /// <param name="ShowText"></param>
        /// <param name="TargetControlInternalName"></param>
        /// <param name="InsertBeforeTargetControl"></param>
        /// <returns></returns>
        public Inventor.CommandControl AddButon(Inventor.Application iApp, string COMMANDNAME, bool UseLargeIcon = true, bool ShowText = true, string TargetControlInternalName = "", bool InsertBeforeTargetControl = false)
        {
            Inventor.CommandControl ret = default(Inventor.CommandControl);
            try
            {
                Inventor.ButtonDefinition ButtonDefinition = iApp.CommandManager.ControlDefinitions[COMMANDNAME] as Inventor.ButtonDefinition;

                ret = RibbonPanel.CommandControls.AddButton(ButtonDefinition, UseLargeIcon, ShowText, TargetControlInternalName, InsertBeforeTargetControl);
                return ret;
            }
            catch (Exception ex)
            {
                if (LogWrite != null) LogWrite($"→→※例外検知 AddButton(..)ﾘﾎﾞﾝ[{RibbonPanel.Parent.Parent.InternalName}] , ﾀﾌﾞ[{RibbonPanel.Parent.DisplayName}({RibbonPanel.Parent.InternalName})] , ﾊﾟﾈﾙ[{RibbonPanel.DisplayName} ({RibbonPanel.InternalName})] へ ﾎﾞﾀﾝ[{COMMANDNAME})を追加時点にて] {ex.Message}");
                return ret;
            }
        }

        /// <summary>
        /// ﾎﾞﾀﾝ定義を削除します
        /// </summary>
        /// <param name="ButtonDefinition"></param>
        /// <returns></returns>
        public bool DeleteButtonDefinition(Inventor.ButtonDefinition ButtonDefinition)
        {
            if (LogWrite == null)
                LogWrite = DebugConsole.WriteLine;

            try
            {
                ButtonDefinition.Delete();
                LogWrite($"ﾎﾞﾀﾝ[{ButtonDefinition.DisplayName}](内部名:{ButtonDefinition.InternalName})を削除しました");
                return true;
            }
            catch (Exception ex)
            {
                LogWrite($"※ﾎﾞﾀﾝ[{ButtonDefinition.DisplayName}](内部名:{ButtonDefinition.InternalName})を削除に失敗しました {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// パネルにセパレーターを追加します
        /// </summary>
        /// <param name="TargetControlInternalName"></param>
        /// <param name="InsertBeforeTargetControl"></param>
        public void AddSeparator(string TargetControlInternalName = "", bool InsertBeforeTargetControl = false)
        {
            if (LogWrite == null)
                LogWrite = DebugConsole.WriteLine;

            try
            {
                RibbonPanel.CommandControls.AddSeparator(TargetControlInternalName, InsertBeforeTargetControl);
            }
            catch (Exception ex)
            {
                LogWrite($"※ButtonPanelTabToRibbon.AddSeparator(..)にて例外  {ex.Message}");
            }
        }

        /// <summary>
        /// リボンにタブを追加します
        /// </summary>
        /// <param name="Ribbon">Ribbon オブジェクトは、タブのコレクションを含むユーザ インタフェースを表します</param>
        /// <param name="DisplayName">ﾀﾌﾞ表示名</param>
        /// <param name="internalName">ﾀﾌﾞ内部名</param>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        private static Inventor.RibbonTab addNewTabtoRibbon(Inventor.Ribbon Ribbon, string DisplayName, string internalName, string ClientId, Action<string> LogWrite = null)
        {
            if (LogWrite == null)
                LogWrite = DebugConsole.WriteLine;

            Inventor.RibbonTab tab;
            try
            {
                foreach (Inventor.RibbonTab ribbonTab in Ribbon.RibbonTabs)
                {
                    if (ribbonTab.InternalName.ToLower() == internalName.ToLower())
                    {
                        tab = Ribbon.RibbonTabs[ribbonTab.InternalName];

                        //LogWrite($"\n◆AddNewTabtoRibbon(...)  ﾘﾎﾞﾝ[{tab.Parent.InternalName}] , ﾀﾌﾞ[{ribbonTab.DisplayName} ({ribbonTab.InternalName})]を取得しました");

                        return tab;
                    }
                }

                tab = Ribbon.RibbonTabs.Add(DisplayName, internalName, ClientId);

                //LogWrite($"\n◆AddNewTabtoRibbon(...)  ﾘﾎﾞﾝ[{tab.Parent.InternalName}] , ﾀﾌﾞ[{tab.DisplayName} ({tab.InternalName})]を追加しました");

                return tab;
            }
            catch (Exception ex)
            {
                LogWrite($"\n※AddNewTabtoRibbon(..) 例外発生 {ex.Message}DisplayName={DisplayName},internalName={internalName},ClientId={ClientId}");

                tab = Ribbon.RibbonTabs[internalName];

                return tab;
            }
        }

        /// <summary>
        /// タブにパネルを追加します。
        /// </summary>
        /// <param name="tab">RibbonTab オブジェクトは、リボン内のタブを表します</param>
        /// <param name="DisplayName">ﾊﾟﾈﾙ表示名</param>
        /// <param name="interanalName">ﾊﾟﾈﾙ内部名</param>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        private static Inventor.RibbonPanel addNewPaneltoTab(Inventor.RibbonTab tab, string DisplayName, string interanalName, string ClientId, Action<string> LogWrite = null)
        {
            if (LogWrite == null)
                LogWrite = DebugConsole.WriteLine;

            Inventor.RibbonPanel panel;

            try
            {
                if (isContain(tab.RibbonPanels, interanalName))
                {
                    panel = tab.RibbonPanels[interanalName];

                    // LogWrite($"→◆AddNewPaneltoTab(..)  ﾀﾌﾞ[{panel.Parent.DisplayName}]の,ﾊﾟﾈﾙ[{DisplayName} ({interanalName})] を取得しました");

                    return panel;

                }
                else
                {
                    panel = tab.RibbonPanels.Add(DisplayName, interanalName, ClientId);

                    //LogWrite($"→◆AddNewPaneltoTab(..)  ﾀﾌﾞ[{panel.Parent.DisplayName}]の,ﾊﾟﾈﾙ[{DisplayName} ({interanalName})]を追加しました");

                    return panel;
                }
            }
            catch (Exception ex)
            {
                LogWrite($"→※AddNewPaneltoTab(..) 例外発生 {ex.Message} DisplayName={DisplayName} , InternalName={interanalName}");

                panel = tab.RibbonPanels.Add(DisplayName, interanalName, ClientId);

                return panel;
            }
        }

        /// <summary>
        /// 既にﾊﾟﾈﾙが存在するかをチェックsします
        /// </summary>
        /// <param name="rps"></param>
        /// <param name="InternalName"></param>
        /// <returns></returns>
        private static bool isContain(Inventor.RibbonPanels rps, string InternalName)
        {
            foreach (Inventor.RibbonPanel rp in rps)
            {
                if (rp.InternalName == InternalName)
                    return true;
            }
            return false;
        }
    }
}
