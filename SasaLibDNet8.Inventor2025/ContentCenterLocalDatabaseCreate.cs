using SasaLib;
using Inventor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Linq;

namespace SasaLib.InventorAPI
{
    /// <summary>
    /// ローカルデータベースを生成
    /// </summary>
    public class ContentCenterLocalDatabaseCreate
    {
        SasaLibDelegateWriteLine LogWrite;
        public FileStream fs;
        StreamWriter srw;
        ContentCenter oContentCenter;
        Inventor.Application sInventorApp;

        static bool loopGo = true;

        public List<string> ExcludeFullTreeViewPath { get; set; }

        public List<string> ExcludeStandardOrganization { get; set; }

        public List<string> ExcludeStandard { get; set; }

        // 一時停止用フラグ
        public static bool CreatePauseFlag { get; set; } = false;

        bool AcceptedStopFlag { get; set; } = false;

        // アボート用フラグ true なら ループを抜けさせる
        public static bool AbortLoopFlag { get; set; } = false;

        public DateTime CreatStartDatetime { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="oContentCente"></param>
        /// <param name="path"></param>
        public ContentCenterLocalDatabaseCreate(ContentCenter oContentCente, string path, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;
            this.LogWrite = LogWrite;

            sInventorApp = (Inventor.Application)oContentCente.Application;

            this.oContentCenter = oContentCente;

            try
            {
                SasaLib.FileFolder.RemoveFile(System.IO.Path.ChangeExtension(path, "bak"));
                SasaLib.FileFolder.MoveFile(path, System.IO.Path.ChangeExtension(path, "bak"));
                if (System.IO.File.Exists(path) == false)
                    LogWrite($"コンテンツセンタローカルデータベース {path} 削除しました");

                fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

            }
            catch (Exception ex)
            {
                LogWrite($"コンテンツセンタローカルデータベース作成エラー {ex.Message}");
                return;
            }
        }


        /// <summary>
        /// コンテンツセンター・ﾛｰｶﾙﾃﾞｰﾀﾍﾞｰｽﾌｧｲﾙの作成
        /// </summary>
        /// <param name="ChildNodeList">List<ContentTreeViewNode>型</param>
        public bool CreateStart(List<ContentTreeViewNode> ChildNodeList)
        {
            AbortLoopFlag = false;

            CreatStartDatetime = DateTime.Now;
            try
            {
                srw = new StreamWriter(fs);

                srw.WriteLine($"{CreatStartDatetime}");    // 1行目

                srw.WriteLine($"\"ContentTableDatabase\"");  // 2行目

                srw.WriteLine($"\"Part_Number\",\"ContentTableRow_ContentIdentifier\",\"Description\""); // 3行目

                StringBuilder sb = new StringBuilder();
                {
                    LibraryManager oContentCenterLibraryManager = oContentCenter.LibraryManager;
                    string serverLibrariesXML = oContentCenterLibraryManager.GetServerLibraries();
                    LogWrite($"■コンテンツセンタローカルデータベース作成開始 GetServerLibraries() = {serverLibrariesXML}");

                    XElement xmlTree = XElement.Parse(serverLibrariesXML);

                    foreach (XElement element in xmlTree.Elements())
                    {
                        XElement xelement = new XElement(element);
                        var xElementAttr = xelement.Attribute("AttachName");
                        sb.Append($"\"{xElementAttr.Value}\"");
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }

                var AttacheNamesString = sb.ToString();
                srw.WriteLine($"# コンテンツセンター ローカルデータベースファイル 作成ホスト:{System.Environment.MachineName} User:{System.Environment.UserName} Domain:{System.Environment.UserDomainName} : InventorのDisplayVersion:{sInventorApp.SoftwareVersion.DisplayVersion}");
                srw.WriteLine($"# 調査したデータベース:{AttacheNamesString}");

                LogWrite($"■コンテンツセンタローカルデータベース作成開始。 作成元InventorのDisplayVersion:{sInventorApp.SoftwareVersion.DisplayVersion}, 調査したデータベース:{AttacheNamesString}");

                srw.WriteLine($"# 1行目はこのデータファイルの作成タイムスタンプ");
                srw.WriteLine($"# 2行目は文頭文末の引用符を除去した文字列DataTableオブジェクトの名前");
                srw.WriteLine($"# 3行目はデータ行の構造定義。半角カンマで区切り、文頭文末の引用符を除去した文字列を DataColumn名とする。");
                srw.WriteLine($"# データ行自体はカンマではなく半角の￥で区切る");
                srw.WriteLine($"# 先頭3行より下の文頭#記号はコメント");

                var cc = ChildNodeList.Count;

                foreach (ContentTreeViewNode oNode in ChildNodeList)
                {
                    if (AbortLoopFlag)
                    {
                        LogWrite($"BreakeLoopFlag = {AbortLoopFlag} のため foreach (ContentTreeViewNode oNode in ChildNode) の途中からストリームをクローズしreturn falseで抜けます。");
                        srw.WriteLine($"#Abort {DateTime.Now}\r\n");
                        srw.Close();
                        return false;
                    }

                    if (loopGo == false)
                    {
                        LogWrite($"loopGo = {loopGo} のため foreach (ContentTreeViewNode oNode in ChildNode) の途中からストリームをクローズしreturn false抜けます。");
                        srw.WriteLine($"#Abort {DateTime.Now}\r\n");
                        srw.Close();
                        return false;
                    }
                    //this.LogWrite($"処理中 oNode.DisplayName = {oNode.DisplayName}");

                    ContentCenterLocalDBGetChild(oNode, 0);

                    // https://adndevblog.typepad.com/manufacturing/2015/12/manipulate-family-table-of-content-center.html
                }

                srw.WriteLine($"#End {DateTime.Now}\r\n");
                srw.Close();
                return true;
            }
            catch (Exception ex)
            {
                SasaLib.Eventlog.Log.WriteEntry("SasaLib.Inventor", EventLogEntryType.Error, 0, $"※ContentCenterLocalDatabaseCreate.CreateStart(..)で例外{ex.Message}");

                return false;
            }
        }

        /// <summary>
        /// コンテンツセンター・ﾛｰｶﾙﾃﾞｰﾀﾍﾞｰｽﾌｧｲﾙの作成
        /// </summary>
        /// <param name="ChildNode">ContentTreeViewNodesEnumerator型</param>
        /// <returns></returns>
        public bool CreateStart(ContentTreeViewNodesEnumerator ChildNode)
        {
            AbortLoopFlag = false;

            try
            {
                srw = new StreamWriter(fs);
                srw.WriteLine($"{DateTime.Now}");    // 1行目

                srw.WriteLine($"\"ContentTableDatabase\"");  // 2行目

                srw.WriteLine($"\"Part_Number\",\"ContentTableRow_ContentIdentifier\",\"Description\""); // 3行目

                StringBuilder sb = new StringBuilder();
                {
                    LibraryManager oContentCenterLibraryManager = oContentCenter.LibraryManager;
                    string serverLibrariesXML = oContentCenterLibraryManager.GetServerLibraries();
                    LogWrite($"■コンテンツセンタローカルデータベース作成開始 GetServerLibraries() = {serverLibrariesXML}");

                    XElement xmlTree = XElement.Parse(serverLibrariesXML);

                    foreach (XElement element in xmlTree.Elements())
                    {
                        XElement xelement = new XElement(element);
                        var xElementAttr = xelement.Attribute("AttachName");
                        sb.Append($"\"{xElementAttr.Value}\"");
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                }

                var AttacheNamesString = sb.ToString();
                srw.WriteLine($"# コンテンツセンター ローカルデータベースファイル 作成元InventorのDisplayVersion:{sInventorApp.SoftwareVersion.DisplayVersion}");
                srw.WriteLine($"# 調査したデータベース:{AttacheNamesString}");

                LogWrite($"■コンテンツセンタローカルデータベース作成開始。 作成元InventorのDisplayVersion:{sInventorApp.SoftwareVersion.DisplayVersion}, 調査したデータベース:{AttacheNamesString}");

                srw.WriteLine($"# 1行目はこのデータファイルの作成タイムスタンプ");
                srw.WriteLine($"# 2行目は文頭文末の引用符を除去した文字列DataTableオブジェクトの名前");
                srw.WriteLine($"# 3行目はデータ行の構造定義。半角カンマで区切り、文頭文末の引用符を除去した文字列を DataColumn名とする。");
                srw.WriteLine($"# データ行自体はカンマではなく半角の￥で区切る");
                srw.WriteLine($"# 先頭3行より下の文頭#記号はコメント");

                var childNodeCount = ChildNode.Count;
                LogWrite($"子ノードの個数:{childNodeCount}");

                foreach (ContentTreeViewNode oNode in ChildNode)
                {
                    if (AbortLoopFlag)
                    {
                        LogWrite($"BreakeLoopFlag = {AbortLoopFlag} のため foreach (ContentTreeViewNode oNode in ChildNode) の途中からストリームをクローズしreturn falseで抜けます。");
                        srw.WriteLine($"#Abort {DateTime.Now}\r\n");
                        srw.Close();
                        return false;
                    }

                    if (loopGo == false)
                    {
                        LogWrite($"loopGo = {loopGo} のため foreach (ContentTreeViewNode oNode in ChildNode) の途中からストリームをクローズしreturn false抜けます。");
                        srw.WriteLine($"#Abort {DateTime.Now}\r\n");
                        srw.Close();
                        return false;
                    }
                    //this.LogWrite($"処理中 oNode.DisplayName = {oNode.DisplayName}");

                    ContentCenterLocalDBGetChild(oNode, 0);

                    // https://adndevblog.typepad.com/manufacturing/2015/12/manipulate-family-table-of-content-center.html
                }

                srw.WriteLine($"#End {DateTime.Now}\r\n");
                srw.Close();
                return true;
            }
            catch (Exception ex)
            {
                SasaLib.Eventlog.Log.WriteEntry("SasaLib.Inventor", EventLogEntryType.Error, 0, $"※ContentCenterLocalDatabaseCreate.CreateStart(..)で例外{ex.Message}");

                return false;
            }
        }

        /// <summary>
        /// 再帰呼出しで処理
        /// </summary>
        /// <param name="oNode"></param>
        /// <param name="LVL"></param>
        /// <returns></returns>
        private bool ContentCenterLocalDBGetChild(ContentTreeViewNode oNode, int LVL)
        {
            string currentNode = oNode.FullTreeViewPath;

            if  (ExcludeFullTreeViewPath != null)
            {
                if (ExcludeFullTreeViewPath.Contains(currentNode))
                {
                    LogWrite($"■■処理中のノード【{currentNode}】 には 除外リスト 『{string.Join(" , ", ExcludeFullTreeViewPath)}』のいずれかが含まれます , スキップします");
                    srw.WriteLine($"# ノード（カテゴリー）【{currentNode}】は 除外リストに合致。スキップします");
                    return true;
                }
            }

            foreach (ContentTreeViewNode oSubNode in oNode.ChildNodes)
            {
                if (AbortLoopFlag)
                {
                    LogWrite($"BreakeLoopFlag = {AbortLoopFlag} のため  foreach (ContentTreeViewNode oSubNode in oNode.ChildNodes) を抜けます");

                    break;
                }

                if (loopGo == false)
                    return false;

                sInventorApp.UserInterfaceManager.DoEvents();

                ContentCenterLocalDBGetChild(oSubNode, LVL + 1);
            }

            foreach (ContentFamily oFamily in oNode.Families)
            {
                //this.LogWrite($"処理中 ContentFamily_DisplayName = {oFamily.DisplayName}");

                var FamilyDispayName = oFamily.DisplayName;
                var StandardOrganization = oFamily.StandardOrganization;
                var Standard = oFamily.Standard;


                if (ExcludeStandardOrganization != null)
                {
                    if (this.ExcludeStandardOrganization.Contains(StandardOrganization))
                    {
                        LogWrite($"■■処理中のファミリー【{FamilyDispayName}】の標準化機関【{StandardOrganization}】 は 除外リスト 『{string.Join(" , ", this.ExcludeStandardOrganization)}』のいずれかに合致します , スキップします");
                        srw.WriteLine($"# ファミリー【{FamilyDispayName}】の標準化機関【{StandardOrganization}】 は 除外リストに合致。スキップします");
                        continue;
                    }
                }

                if (ExcludeStandard != null)
                {
                    if (this.ExcludeStandard.Contains(Standard))
                    {
                        LogWrite($"■■処理中のファミリー【{FamilyDispayName}】の規格【{Standard}】 は 除外リスト 『{string.Join(" , ", this.ExcludeStandard)}』のいずれかに合致します , スキップします");
                        srw.WriteLine($"# ファミリー【{FamilyDispayName}】の規格【{Standard}】 は 除外リストに合致。スキップします");
                        continue;
                    }
                }

                if (AbortLoopFlag)
                {
                    LogWrite($"BreakeLoopFlag = {AbortLoopFlag} のため foreach (ContentFamily oFamily in oNode.Families) を抜けます");

                    break;
                }

                while (CreatePauseFlag)
                {
                    if (AcceptedStopFlag == false)
                    {
                        LogWrite("★★CreatePauseFlag = true 一時停止します"); // CreatePauseFlag = true の場合処理を個々で一時停止する
                        AcceptedStopFlag = true;
                    }

                    // 1 分待機
                    int delaytime = 1 * 60 * 1000;
                    Task.Delay(delaytime);
                }

                if (AcceptedStopFlag == true)
                {
                    LogWrite("★★CreatePauseFlag = false になりました。再開します"); // CreatePauseFlag = true の場合処理を個々で一時停止する
                    AcceptedStopFlag = false;
                }


                sInventorApp.UserInterfaceManager.DoEvents();
                if (oFamily != null)
                {

                    ContentTreeViewNode treeViewNode = oNode;
                    string ContentTreeViewNode_FullTreeViewPath = default;
                    string ContentTreeViewNode_DisplayName = default;
                    string ContentTreeViewNode_InternalName = default;

                    string ContentFamily_ContentIdentifier = default;
                    string ContentFamily_Description = default;
                    string ContentFamily_DisplayName = default;
                    string ContentFamily_InternalName = default;
                    string ContentFamily_LibraryInternalName = default;
                    string ContentFamily_LibraryName = default;
                    string ContentFamily_Manufacturer = default;
                    string ContentFamily_MemberDirectory = default;
                    string ContentFamily_RevisionId = default;
                    string ContentFamily_Standard = default;
                    string ContentFamily_StandardOrganization = default;
                    string ContentFamily_StandardRevision = default;
                    string ContentFamily_TemplateFileName = default;

                    try
                    {
                        ContentTreeViewNode_FullTreeViewPath = treeViewNode.FullTreeViewPath;
                        ContentTreeViewNode_DisplayName = treeViewNode.DisplayName;
                        ContentTreeViewNode_InternalName = treeViewNode.InternalName;

                        ContentFamily_ContentIdentifier = oFamily.ContentIdentifier;
                        ContentFamily_Description = oFamily.Description;
                        ContentFamily_DisplayName = oFamily.DisplayName;
                        ContentFamily_InternalName = oFamily.InternalName;
                        ContentFamily_LibraryInternalName = oFamily.LibraryInternalName;
                        ContentFamily_LibraryName = oFamily.LibraryName;
                        ContentFamily_Manufacturer = oFamily.Manufacturer;
                        ContentFamily_MemberDirectory = oFamily.MemberDirectory;
                        ContentFamily_RevisionId = oFamily.RevisionId;
                        ContentFamily_Standard = oFamily.Standard;
                        ContentFamily_StandardOrganization = oFamily.StandardOrganization;
                        ContentFamily_StandardRevision = oFamily.StandardRevision;
                        ContentFamily_TemplateFileName = oFamily.TemplateFileName;
                    }
                    catch (Exception ex)
                    {
                        LogWrite($"例外検知　{ex.Message}");
                    }

                    //StandardAddInServer.sLogWindowForm.WriteLine($"#{oNode.FullTreeViewPath}");
                    //sw.WriteLine($"#ContentTreeViewNode_DisplayName = {ContentTreeViewNode_DisplayName}");
                    srw.WriteLine($"#ContentFamily_DisplayName = {ContentFamily_DisplayName}");
                    //sw.WriteLine($"#ContentFamily_Description = {ContentFamily_Description}");
                    //sw.WriteLine($"#ContentFamily_TemplateFileName = {ContentFamily_TemplateFileName}");
                    bool ans = ContentCenterLocalDBGetItem(oFamily);


                    this.LogWrite($"ｺﾝﾃﾝﾂｾﾝﾀｰLocalDB {CreatStartDatetime}より作成中. {(DateTime.Now - CreatStartDatetime).Minutes} 分経過 , FullTreeViewPath = {treeViewNode.FullTreeViewPath} , ContentFamily_DisplayName = {ContentFamily_DisplayName}");

                    if (ans)
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oFamily"></param>
        /// <param name="oContentTreeViewNode"></param>
        /// <returns></returns>
        bool ContentCenterLocalDBGetItem(ContentFamily oFamily)
        {

            /// Design Tracking Properties = {32853F0F-3444-11D1-9E93-0060B03C1CA6} , 5 = Part Number
            int intPart_Number = GetColumn(oFamily, "{32853F0F-3444-11D1-9E93-0060B03C1CA6}", "5");

            ///// Inventor Summary Information = {F29F85E0-4FF9-1068-AB91-08002B27B3D9} , 2 = Title
            //int intTitle = GetColumn(oFamily, "{F29F85E0-4FF9-1068-AB91-08002B27B3D9}", "2");

            /// Design Tracking Properties = {32853F0F-3444-11D1-9E93-0060B03C1CA6} , 29 = Description
            int intDescription = GetColumn(oFamily, "{32853F0F-3444-11D1-9E93-0060B03C1CA6}", "29");

            string Part_Number;// [0]
            string ContentTableRow_ContentIdentifier; // [1]
            string Description;// [2]
            bool result = false;



            foreach (ContentTableRow oContentTableRow in oFamily.TableRows)
            {
                sInventorApp.UserInterfaceManager.DoEvents();

                try
                {
                    Part_Number = oContentTableRow.GetCellValue(intPart_Number); // [0]

                    ContentTableRow_ContentIdentifier = oContentTableRow.ContentIdentifier; // [1]

                    Description = oContentTableRow.GetCellValue(intDescription); // [2]


                    //this.LogWrite($"Part_Number = {Part_Number} | Title =  {ContentTableRow_ContentIdentifier} | Description = {Description}");

                    //  [0] - [2]
                    srw.WriteLine(
                        Part_Number +   // [0]
                        "\\" + ContentTableRow_ContentIdentifier +  // [1]
                        "\\" + Description     // [2]
                        );

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            srw.Flush();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oFamily"></param>
        /// <param name="strPropSetIDQuery"></param>
        /// <param name="strPropIDQuery"></param>
        /// <returns></returns>
        int GetColumn(ContentFamily oFamily, string strPropSetIDQuery, string strPropIDQuery)
        {
            int index = 1;

            string strPropSetID = "";

            string strPropID = "";

            foreach (ContentTableColumn oColumn in oFamily.TableColumns)
            {
                sInventorApp.UserInterfaceManager.DoEvents();

                if (oColumn.HasPropertyMap == true && oColumn.InternalName != "FILENAME")
                {
                    try
                    {
                        oColumn.GetPropertyMap(out strPropSetID, out strPropID);

                        Console.WriteLine($"strPropSetID={strPropSetID} , strPropID={strPropID}");

                        if (strPropSetID.ToUpper() == strPropSetIDQuery && strPropID.ToUpper() == strPropIDQuery)
                        {
                            return index;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"例外検知 {ex.Message}");
                    }
                }
                index += 1;
            }
            return 1;
        }
    }
}
