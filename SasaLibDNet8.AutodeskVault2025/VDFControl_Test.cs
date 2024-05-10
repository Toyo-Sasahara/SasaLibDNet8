using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;
using ACW = Autodesk.Connectivity.WebServices;
//using CIE = Connectivity.InventorAddin.EdmAddin;
using ACWTools = Autodesk.Connectivity.WebServicesTools;

using VDF = Autodesk.DataManagement.Client.Framework;
using VDFV = Autodesk.DataManagement.Client.Framework.Vault;
using VDFVR = Autodesk.DataManagement.Client.Framework.Vault.Results;
using VDFVC = Autodesk.DataManagement.Client.Framework.Vault.Currency;
using VDFVCF = Autodesk.DataManagement.Client.Framework.Vault.Currency.FileSystem;
using VDFVCP = Autodesk.DataManagement.Client.Framework.Vault.Currency.Properties;
using VDFVCE = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using VDFVFS = Autodesk.DataManagement.Client.Framework.Vault.Forms.Settings;

//using DEXX = DevExpress.XtraTreeList;
//using DEXUE = DevExpress.Utils.Extensions;
//using DEXXN = DevExpress.XtraPrinting.Native;
//using DEXU = DevExpress.Utils;
//using DEXXM = DevExpress.XtraRichEdit.Model;
//using DEXSI = DevExpress.Services.Internal;
//using DEXXR = DevExpress.XtraEditors.Repository;
//using DEXIWWUN = DevExpress.Internal.WinApi.Windows.UI.Notifications;

namespace SasaLib.AutodeskVault
{
    public partial class VDFControl
    {
        /// <summary>
        /// けんしょうとちゅう IDWからInventorDWG
        /// /// </summary>
        /// <param name="PartNumber"></param>
        /// <returns></returns>
        public ACW.SrchCond[] MakeSrchCondsInventorDrawingFile(string PartNumber)
        {
            if (VaultConnection.IsConnected == false)
            {
                this.WriteLine($"Vaultにﾛｸﾞｲﾝしていません");
                return null;
            }
            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");

            ////

            ACW.PropDef NamePropDef1 = filePropDefs.Single(n => n.DispName == "名前");

            WriteLine($"NamePropDef1.DispName={NamePropDef1.DispName},SysName={NamePropDef1.SysName},ID={NamePropDef1.Id},UsageCount={NamePropDef1.UsageCount}");


            ACW.SrchCond srchCond1 = new ACW.SrchCond()
            {
                PropDefId = NamePropDef1.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 1, // is contains
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = PartNumber
            };

            ////

            ACW.PropDef NamePropDef2 = filePropDefs.Single(n => n.DispName == "ファイル拡張子");

            WriteLine($"NamePropDef1.DispName={NamePropDef2.DispName},SysName={NamePropDef2.SysName},ID={NamePropDef2.Id},UsageCount={NamePropDef2.UsageCount}");

            ACW.SrchCond srchCond2 = new ACW.SrchCond()
            {
                PropDefId = NamePropDef2.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = "DWG"
            };

            //// 

            ACW.PropDef NamePropDef3 = filePropDefs.Single(n => n.DispName == "DWG タイプ");

            WriteLine($"NamePropDef2.DispName={NamePropDef3.DispName},SysName={NamePropDef3.SysName},ID={NamePropDef3.Id},UsageCount={NamePropDef3.UsageCount}");

            ACW.SrchCond srchCond3 = new ACW.SrchCond()
            {
                PropDefId = NamePropDef3.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 1, // contain
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = "Inventor"
            };

            ////

            ACW.SrchCond[] results = new ACW.SrchCond[] { srchCond1, srchCond2, srchCond3 };


            return results;
        }




        /// <summary>
        /// 検索ﾃｽﾄ1 結果をList<ACW.File>形式
        /// ms-its:C:\Users\sasahara\Source\TEST.AUTODESK\Autodesk Vault 2020 SDK\docs\VaultSDK.chm::/topic2536.html
        /// </summary>
        /// <returns>List<Autodesk.Connectivity.WebServices.File></returns>
        public List<ACW.File> FindFileTest1()
        {
            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef checkedOutPropDef = filePropDefs.Single(n => n.SysName == "CheckoutUserName");
            //ACW.PropDef projectPropDef = filePropDefs.Single(n => n.SysName == "Project");

            #region 検索条件の準備
            ACW.SrchCond isCheckedOut = new ACW.SrchCond()
            {
                PropDefId = checkedOutPropDef.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 5, // is not empty
                SrchRule = ACW.SearchRuleType.Must,
            };

            //ACW.SrchCond padlockProject = new ACW.SrchCond()
            //{
            //    PropDefId = projectPropDef.Id,
            //    PropTyp = ACW.PropertySearchType.SingleProperty,
            //    SrchOper = 3, // is equal
            //    SrchRule = ACW.SearchRuleType.Must,
            //    SrchTxt = ""
            //};
            #endregion

            string bookmark = string.Empty;
            ACW.SrchStatus status = null;

            List<ACW.File> totalResults = new List<ACW.File>();

            while (status == null || totalResults.Count < status.TotalHits)
            {
                ACW.File[] results = VaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
                    new ACW.SrchCond[] {
                        isCheckedOut,
                        //padlockProject
                    },
                    null, null, false, true, ref bookmark, out status);
                if (results != null)
                    totalResults.AddRange(results);
                else
                    break;
            }

            // total results now has the results
            return totalResults;
        }

        /// <summary>
        /// 検索ﾃｽﾄ2
        /// ms-its:InventorSDKプログラム\Vault2020 SDK.chm::/topic64.html
        /// </summary>
        /// <returns> List<VDFVCE.FileIteration></returns>
        public List<VDFVCE.FileIteration> FindFileTest2()
        {
            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef checkedOutPropDef = filePropDefs.Single(n => n.SysName == "CheckoutUserName");
            //ACW.PropDef projectPropDef = filePropDefs.Single(n => n.SysName == "Project");

            #region 検索条件の準備
            ACW.SrchCond isCheckedOut = new ACW.SrchCond()
            {
                PropDefId = checkedOutPropDef.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 5, // is not empty
                SrchRule = ACW.SearchRuleType.Must,
            };

            //ACW.SrchCond padlockProject = new ACW.SrchCond()
            //{
            //    PropDefId = projectPropDef.Id,
            //    PropTyp = ACW.PropertySearchType.SingleProperty,
            //    SrchOper = 3, // is equal
            //    SrchRule = ACW.SearchRuleType.Must,
            //    SrchTxt = ""
            //};
            #endregion

            string bookmark = string.Empty;
            ACW.SrchStatus status = null;

            List<ACW.File> totalResults = new List<ACW.File>();

            while (status == null || totalResults.Count < status.TotalHits)
            {
                /* File[] FindFilesBySearchConditions(SrchCond[] conditions , SrchSort[] sortConditions,
                 *                                      long[] folderIds, bool recurseFolders, 
                 *                                      bool latestOnly,ref System.string bookmark,out SrchStatus searchstatus)
                 *                                      
                    1.conditions    は検索条件の配列です。 空の配列を渡すことができ、結果としてすべてのファイルが返されます。 これは、実際にはVault内のすべてのファイルをスキャンする最も効率的な方法です。
                    2.sortConditions　はソートオブジェクトの配列です。 順番を気にしない場合はnullを渡すことができます。 ソートを行いたい場合は、SrchSortクラスが非常に簡単です。 ソートしたいVaultプロパティを指定し、昇順または降順にすることができます。 入力は配列で、複数のプロパティをまたいでソートすることができます。 配列の最初のオブジェクトがプライマリソート、2 番目のオブジェクトがセカンダリソート、といった具合です。
                    3.FolderIds は、特定のフォルダのセットで検索する場合に使用します。 Vault 全体を検索する場合は、null を渡します。 ルート フォルダを渡すと、パフォーマンスが低下することがあります。
                    4.bookmarkは、複数のページを返す必要がある場合に使用します。 最初に Find を呼び出すときは、ブックマーク用の空の文字列を渡します。 最初の呼び出しですべての結果が得られなかった場合、ブックマークは更新されます。 次の呼び出しでブックマークを使えば、前回の検索が終わったところから検索を続けることができます。
                    5.searchstatusは、検索結果の総数を知ることができます。 また、再インデックスが行われているかどうかもわかります。 再インデックスが行われている場合、結果が100%正確ではない可能性があります。 この場合、ユーザーへの警告以外にできることはあまりありません。
                 * 
                 */
                ACW.File[] results = VaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
                                new ACW.SrchCond[] {
                                    isCheckedOut,
                                    //padlockProject
                                },                // conditions
                                null,             // sortConditions
                                null,             // FolderIds
                                false,            // recurseFolders
                                true,             // lateset only
                                ref bookmark,   　// bookmark
                                out status        // searchstatusは
                            ); ;
                if (results != null)
                    totalResults.AddRange(results);
                else
                    break;
            }

            /// これはWebサービスファイルの周りのラッパーで、作業するオブジェクトのリッチ化など、さまざまな機能強化を提供しています。ファイルがチェックアウトされると、新しいバージョンがプレースホルダとして作成されます。チェックアウト状態では、このオブジェ
            List<VDFVCE.FileIteration> fileIterationList = new List<VDFVCE.FileIteration>();

            foreach (var fileProxy in totalResults)
            {
                /// 保管庫からファイルをダウンロードおよび/またはチェックアウトするために使用される AcquireFiles ワークフローの設定オプション 
                VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection, false);

                VDFVCE.FileIteration fileIteraition = new VDFVCE.FileIteration(VaultConnection, fileProxy);

                fileIterationList.Add(fileIteraition);
            }

            // total results now has the results
            return fileIterationList;
        }

        /// <summary>
        /// 検索ﾃｽﾄ4
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>List<FileIteration>形式で結果が返る</returns>
        public List<VDFVCE.FileIteration> FindFileTest4(string Name)
        {
            if (string.IsNullOrEmpty(Name) == true)
                return null;

            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef NamePropDef = filePropDefs.Single(n => n.SysName == "Name");

            #region 検索条件の準備

            ACW.SrchCond srchCond = new ACW.SrchCond()
            {
                PropDefId = NamePropDef.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = Name
            };
            #endregion

            string bookmark = string.Empty;
            ACW.SrchStatus status = null;

            List<ACW.File> totalACWFiles = new List<ACW.File>();

            while (status == null || totalACWFiles.Count < status.TotalHits)
            {
                /* File[] FindFilesBySearchConditions(SrchCond[] conditions , SrchSort[] sortConditions,
                 *                                      long[] folderIds, bool recurseFolders, 
                 *                                      bool latestOnly,ref System.string bookmark,out SrchStatus searchstatus)
                 *                                      
                    1.conditions    は検索条件の配列です。 空の配列を渡すことができ、結果としてすべてのファイルが返されます。 これは、実際にはVault内のすべてのファイルをスキャンする最も効率的な方法です。
                    2.sortConditions　はソートオブジェクトの配列です。 順番を気にしない場合はnullを渡すことができます。 ソートを行いたい場合は、SrchSortクラスが非常に簡単です。 ソートしたいVaultプロパティを指定し、昇順または降順にすることができます。 入力は配列で、複数のプロパティをまたいでソートすることができます。 配列の最初のオブジェクトがプライマリソート、2 番目のオブジェクトがセカンダリソート、といった具合です。
                    3.FolderIds は、特定のフォルダのセットで検索する場合に使用します。 Vault 全体を検索する場合は、null を渡します。 ルート フォルダを渡すと、パフォーマンスが低下することがあります。
                    4.bookmarkは、複数のページを返す必要がある場合に使用します。 最初に Find を呼び出すときは、ブックマーク用の空の文字列を渡します。 最初の呼び出しですべての結果が得られなかった場合、ブックマークは更新されます。 次の呼び出しでブックマークを使えば、前回の検索が終わったところから検索を続けることができます。
                    5.searchstatusは、検索結果の総数を知ることができます。 また、再インデックスが行われているかどうかもわかります。 再インデックスが行われている場合、結果が100%正確ではない可能性があります。 この場合、ユーザーへの警告以外にできることはあまりありません。
                 * 
                 */
                // 結果がACW.FILE[]で返される
                ACW.File[] results = VaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
                                new ACW.SrchCond[] {
                                    srchCond,
                                    //padlockProject
                                },                // conditions
                                null,             // sortConditions
                                null,             // FolderIds
                                false,            // recurseFolders
                                true,             // lateset only
                                ref bookmark,   　// bookmark
                                out status        // searchstatusは
                            ); ;
                if (results != null)
                    totalACWFiles.AddRange(results);
                else
                    break;
            }

            /// これはWebサービスファイルの周りのラッパーで、作業するオブジェクトのリッチ化など、さまざまな機能強化を提供しています。ファイルがチェックアウトされると、新しいバージョンがプレースホルダとして作成されます。チェックアウト状態では、このオブジェ
            List<VDFVCE.FileIteration> fileIterationList = new List<VDFVCE.FileIteration>();

            foreach (var fileProxy in totalACWFiles)
            {
                /// 保管庫からファイルをダウンロードおよび/またはチェックアウトするために使用される AcquireFiles ワークフローの設定オプション 
                VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection, false);

                VDFVCE.FileIteration fileIteraition = new VDFVCE.FileIteration(VaultConnection, fileProxy);

                fileIterationList.Add(fileIteraition);
            }

            // 合計結果は現在結果を持っています
            return fileIterationList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentService"></param>
        /// <param name="file"></param>
        /// <param name="results"></param>
        public void UpdateRefsInLocalFile(ACW.DocumentService documentService, ACW.File file, VDFVR.AcquireFilesResults results)
        {
            foreach (VDFVR.FileAcquisitionResult acquireFilesResult in results.FileResults)
            {
                var assocs = documentService.GetFileAssociationsByIds(new[] { acquireFilesResult.File.EntityIterationId },
                                                                      ACW.FileAssociationTypeEnum.None, false,
                                                                      ACW.FileAssociationTypeEnum.Dependency, false, false, false);
                if (assocs.First().FileAssocs == null)
                    continue;
                var fileAssocs = assocs.First().FileAssocs.Where(fa => fa.ParFile.MasterId == acquireFilesResult.File.EntityMasterId);
                var refs = new List<VDFVCF.FileReference>();
                foreach (ACW.FileAssoc fileAssoc in fileAssocs)
                {
                    var fileCld = results.FileResults.FirstOrDefault(f => f.File.EntityMasterId == fileAssoc.CldFile.MasterId);
                    if (fileCld == null)
                        continue;
                    var reference = new VDFVCF.FileReference(fileAssoc.RefId, fileCld.LocalPath, fileAssoc.Source);
                    refs.Add(reference);
                }

                var updateReferenceModel = new Autodesk.DataManagement.Client.Framework.Vault.Models.UpdateFileReferencesModel();
                updateReferenceModel.SetTargetFilePath(acquireFilesResult.File, acquireFilesResult.LocalPath);
                updateReferenceModel.UpdateRefsInLocalFile(acquireFilesResult.File, acquireFilesResult.LocalPath, refs);
            }
        }

        /// <summary>
        ///  テストコード Vaultファイルダウンロード
        /// </summary>
        /// <param name="CheckOutUserName"></param>
        /// <returns></returns>
        public List<VDFVCE.FileIteration> DownloadFileTest1(string CheckOutUserName)
        {
            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef checkedOutPropDef = filePropDefs.Single(n => n.SysName == "CheckoutUserName");
            //ACW.PropDef projectPropDef = filePropDefs.Single(n => n.SysName == "Project");

            #region 検索条件の準備
            //ACW.SrchCond isCheckedOut = new ACW.SrchCond()
            //{
            //    PropDefId = checkedOutPropDef.Id,
            //    PropTyp = ACW.PropertySearchType.SingleProperty,
            //    SrchOper = 5, // is not empty
            //    SrchRule = ACW.SearchRuleType.Must,
            //};

            ACW.SrchCond isCheckedOut = new ACW.SrchCond()
            {
                PropDefId = checkedOutPropDef.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = CheckOutUserName
            };
            #endregion

            string bookmark = string.Empty;
            ACW.SrchStatus status = null;

            List<ACW.File> totalResults = new List<ACW.File>();

            while (status == null || totalResults.Count < status.TotalHits)
            {
                /* File[] FindFilesBySearchConditions(SrchCond[] conditions , SrchSort[] sortConditions,
                 *                                      long[] folderIds, bool recurseFolders, 
                 *                                      bool latestOnly,ref System.string bookmark,out SrchStatus searchstatus)
                 *                                      
                    1.conditions    は検索条件の配列です。 空の配列を渡すことができ、結果としてすべてのファイルが返されます。 これは、実際にはVault内のすべてのファイルをスキャンする最も効率的な方法です。
                    2.sortConditions　はソートオブジェクトの配列です。 順番を気にしない場合はnullを渡すことができます。 ソートを行いたい場合は、SrchSortクラスが非常に簡単です。 ソートしたいVaultプロパティを指定し、昇順または降順にすることができます。 入力は配列で、複数のプロパティをまたいでソートすることができます。 配列の最初のオブジェクトがプライマリソート、2 番目のオブジェクトがセカンダリソート、といった具合です。
                    3.FolderIds は、特定のフォルダのセットで検索する場合に使用します。 Vault 全体を検索する場合は、null を渡します。 ルート フォルダを渡すと、パフォーマンスが低下することがあります。
                    4.bookmarkは、複数のページを返す必要がある場合に使用します。 最初に Find を呼び出すときは、ブックマーク用の空の文字列を渡します。 最初の呼び出しですべての結果が得られなかった場合、ブックマークは更新されます。 次の呼び出しでブックマークを使えば、前回の検索が終わったところから検索を続けることができます。
                    5.searchstatusは、検索結果の総数を知ることができます。 また、再インデックスが行われているかどうかもわかります。 再インデックスが行われている場合、結果が100%正確ではない可能性があります。 この場合、ユーザーへの警告以外にできることはあまりありません。
                 * 
                 */
                ACW.File[] results = VaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
                                new ACW.SrchCond[] {
                                    isCheckedOut,
                                    //padlockProject
                                },                // conditions
                                null,             // sortConditions
                                null,             // FolderIds
                                false,            // recurseFolders
                                true,             // lateset only
                                ref bookmark,   　// bookmark
                                out status        // searchstatusは
                            ); ;
                if (results != null)
                    totalResults.AddRange(results);
                else
                    break;
            }

            /// これはWebサービスファイルの周りのラッパーで、作業するオブジェクトのリッチ化など、さまざまな機能強化を提供しています。ファイルがチェックアウトされると、新しいバージョンがプレースホルダとして作成されます。チェックアウト状態では、このオブジェ
            List<VDFVCE.FileIteration> fileIterationList = new List<VDFVCE.FileIteration>();

            foreach (var fileProxy in totalResults)
            {
                /// 保管庫からファイルをダウンロードおよび/またはチェックアウトするために使用される AcquireFiles ワークフローの設定オプション 
                VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection, false);

                VDFVCE.FileIteration fileIteraition = new VDFVCE.FileIteration(VaultConnection, fileProxy);

                fileIterationList.Add(fileIteraition);
            }

            // total results now has the results
            return fileIterationList;
        }

        /// <summary>
        ///  テストコード Vaultファイルダウンロード
        /// </summary>
        /// <param name="CheckedOutSpec"></param>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        public List<VDFVCE.FileIteration> DownloadFileTest2(string CheckedOutSpec, out List<ACW.File> totalResults)
        {
            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef checkedOutPropDef = filePropDefs.Single(n => n.SysName == "CheckoutLocalSpec");
            //ACW.PropDef projectPropDef = filePropDefs.Single(n => n.SysName == "Project");

            #region 検索条件の準備
            //ACW.SrchCond isCheckedOut = new ACW.SrchCond()
            //{
            //    PropDefId = checkedOutPropDef.Id,
            //    PropTyp = ACW.PropertySearchType.SingleProperty,
            //    SrchOper = 5, // is not empty
            //    SrchRule = ACW.SearchRuleType.Must,
            //};

            ACW.SrchCond isCheckedOut = new ACW.SrchCond()
            {
                PropDefId = checkedOutPropDef.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = CheckedOutSpec
            };
            #endregion

            string bookmark = string.Empty;
            ACW.SrchStatus status = null;

            totalResults = new List<ACW.File>();

            while (status == null || totalResults.Count < status.TotalHits)
            {
                /* File[] FindFilesBySearchConditions(SrchCond[] conditions , SrchSort[] sortConditions,
                 *                                      long[] folderIds, bool recurseFolders, 
                 *                                      bool latestOnly,ref System.string bookmark,out SrchStatus searchstatus)
                 *                                      
                    1.conditions    は検索条件の配列です。 空の配列を渡すことができ、結果としてすべてのファイルが返されます。 これは、実際にはVault内のすべてのファイルをスキャンする最も効率的な方法です。
                    2.sortConditions　はソートオブジェクトの配列です。 順番を気にしない場合はnullを渡すことができます。 ソートを行いたい場合は、SrchSortクラスが非常に簡単です。 ソートしたいVaultプロパティを指定し、昇順または降順にすることができます。 入力は配列で、複数のプロパティをまたいでソートすることができます。 配列の最初のオブジェクトがプライマリソート、2 番目のオブジェクトがセカンダリソート、といった具合です。
                    3.FolderIds は、特定のフォルダのセットで検索する場合に使用します。 Vault 全体を検索する場合は、null を渡します。 ルート フォルダを渡すと、パフォーマンスが低下することがあります。
                    4.bookmarkは、複数のページを返す必要がある場合に使用します。 最初に Find を呼び出すときは、ブックマーク用の空の文字列を渡します。 最初の呼び出しですべての結果が得られなかった場合、ブックマークは更新されます。 次の呼び出しでブックマークを使えば、前回の検索が終わったところから検索を続けることができます。
                    5.searchstatusは、検索結果の総数を知ることができます。 また、再インデックスが行われているかどうかもわかります。 再インデックスが行われている場合、結果が100%正確ではない可能性があります。 この場合、ユーザーへの警告以外にできることはあまりありません。
                 * 
                 */
                ACW.File[] results = VaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
                                new ACW.SrchCond[] {
                                    isCheckedOut,
                                    //padlockProject
                                },                // conditions
                                null,             // sortConditions
                                null,             // FolderIds
                                false,            // recurseFolders
                                true,             // lateset only
                                ref bookmark,   　// bookmark
                                out status        // searchstatusは
                            ); ;
                if (results != null)
                    totalResults.AddRange(results);
                else
                    break;
            }

            /// これはWebサービスファイルの周りのラッパーで、作業するオブジェクトのリッチ化など、さまざまな機能強化を提供しています。ファイルがチェックアウトされると、新しいバージョンがプレースホルダとして作成されます。チェックアウト状態では、このオブジェ
            List<VDFVCE.FileIteration> fileIterationList = new List<VDFVCE.FileIteration>();

            foreach (var fileProxy in totalResults)
            {
                /// 保管庫からファイルをダウンロードおよび/またはチェックアウトするために使用される AcquireFiles ワークフローの設定オプション 
                VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection, false);

                VDFVCE.FileIteration fileIteraition = new VDFVCE.FileIteration(VaultConnection, fileProxy);

                fileIterationList.Add(fileIteraition);
            }

            // total results now has the results
            return fileIterationList;
        }

        /// <summary>
        /// 名前変更・テスト
        /// </summary>
        /// <param name="vaultFolderPath"></param>
        /// <param name="vaultName"></param>
        /// <param name="newVaultName"></param>
        /// <returns></returns>
        public ACW.FileRenameRestricArray[] TestRenameFile(string vaultFolderPath, string vaultName, string newVaultName)
        {
            ACW.SrchCond[] _srchCond1 = MakeSrchCondsSysName("FolderPath", vaultFolderPath, "Name", vaultName);
            VDFVCE.FileIteration oSource = FindFileFirst(_srchCond1);

            if (oSource != null)
            {
                ACW.DocumentService documentService = VaultConnection.WebServiceManager.DocumentService;

                long filemasterID = oSource.EntityMasterId;
                long[] filemasterIDs = new long[1];
                filemasterIDs[0] = filemasterID;


                string filename = newVaultName;
                string[] filenames = new string[] { newVaultName };

                try
                {
                    ACW.FileRenameRestricArray[] result = documentService.GetFileRenameRestrictionsByMasterIds(filemasterIDs, filenames);

                    return result;
                }
                catch (Exception ex)
                {
                    this.WriteLine($"※VDFControl.RenameFile(..)で例外 {ex.Message}");
                }
            }
            else
            {
                if (oSource == null)
                    this.WriteLine($"※VDFControl.RenameFile(..)  {vaultFolderPath}/{vaultName} が見つかりません");
            }
            return null;
        }

        /// <summary>
        /// テストコード FileIteration オブジェクト内の Vaultプロパティ表示
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="fileInteration"></param>
        public void TestPrintProperties(VDFVCE.FileIteration fileInteration)
        {
            VDF.Vault.Currency.Properties.PropertyDefinitionDictionary propDefs;
            propDefs = VaultConnection.PropertyManager.GetPropertyDefinitions(VDF.Vault.Currency.Entities.EntityClassIds.Files, null, VDF.Vault.Currency.Properties.PropertyDefinitionFilter.IncludeUserDefined);
            foreach (var key in propDefs.Keys)
            {
                // Print the Name from the Definition and the Value from the Property
                object propValue = VaultConnection.PropertyManager.GetPropertyValue(fileInteration, propDefs[key], null);
                WriteLine($"{key.ToString()} = {propValue}");
            }
        }

        /// <summary>
        /// プロパティアップデートテストコード
        /// </summary>
        /// <param name="oAFI"></param>
        /// <param name="fromProperty"></param>
        /// <param name="toProperty"></param>
        public void TestPropertyUpdate(VDFVCE.FileIteration oAFI, string fromProperty, string toProperty)
        {

            VDF.Vault.Results.AcquireFilesResults resutl3 = CheckOutFile(oAFI);


            ACW.PropDef[] propDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef propDef = propDefs.SingleOrDefault(n => n.SysName == toProperty);

            if (propDef == null)
                return;

            WriteLine($"ターゲットプロパティ SysName:{propDef.SysName} , DisplayName:{propDef.DispName}");
            long[] ArryMasterId = new long[1];

            ArryMasterId[0] = oAFI.EntityMasterId;

            List<ACW.PropInstParam> propInstParamsUnamaped = new List<ACW.PropInstParam>();

            List<ACW.PropInstParam> propInstParams = new List<ACW.PropInstParam>();
            ACW.PropInstParam propInstPar0Unamaped = new ACW.PropInstParam();

            propInstPar0Unamaped.Val = GetPropertyFromSysName(oAFI, fromProperty);
            propInstPar0Unamaped.PropDefId = propDef.Id;

            WriteLine($"参照するプロパティ:{fromProperty} , 内容:{propInstPar0Unamaped.Val}");

            propInstParamsUnamaped.Add(propInstPar0Unamaped);

            ACW.PropInstParamArray propInstParamsArrayUnamaped = new ACW.PropInstParamArray();
            propInstParamsArrayUnamaped.Items = propInstParamsUnamaped.ToArray();

            ACW.PropInstParamArray[] propInstParamsArraysUnamaped = new ACW.PropInstParamArray[1];
            propInstParamsArraysUnamaped[0] = propInstParamsArrayUnamaped;

            VaultConnection.WebServiceManager.DocumentService.UpdateFileProperties(ArryMasterId, propInstParamsArraysUnamaped);

            //ChangeOrderService changeOrderService = vaultConnection.WebServiceManager.ChangeOrderService;
            //long changeOrderId = GetChangeOrderIdByFileId(changeOrderService, fileId);

            //var affectedFiles = new Autodesk.Connectivity.WebServices.AffectedFile[]
            //{
            //    new Autodesk.Connectivity.WebServices.AffectedFile
            //    {
            //        FileId = fileId,
            //        ChangeNumber = 0, // 0を指定して最新のバージョンに変更
            //        ChangeOrderItemId = 0 // 0を指定して新しいアイテムを作成
            //    }
            //};

            //changeOrderService.UpdateChangeOrder(ChangeOrderUtils.GetNextStates(changeOrderId), affectedFiles, null);

        }

        public void TestPropertyUpdate2(VDFVCE.FileIteration oAFI, string fromProperty, string toProperty)
        {
            long[] ArryMasterId = new long[1];
            ArryMasterId[0] = oAFI.EntityMasterId;


            VDF.Vault.Results.AcquireFilesResults resutl3 = CheckOutFile(oAFI);


            ACW.PropDef[] from_propDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef from_propDef = from_propDefs.SingleOrDefault(n => n.SysName == fromProperty);

            if (from_propDef == null)
                return;

            WriteLine($"fromプロパティ SysName:{from_propDef.SysName} , DisplayName:{from_propDef.DispName}");


            ACW.PropInstParam from_propInstPar0Unamaped = new ACW.PropInstParam();

            from_propInstPar0Unamaped.Val = GetPropertyFromSysName(oAFI, fromProperty);
            from_propInstPar0Unamaped.PropDefId = from_propDef.Id;

            WriteLine($"参照するプロパティ:{fromProperty} , 内容:{from_propInstPar0Unamaped.Val}");

            List<ACW.PropInstParam> from_propInstParamsUnamaped = new List<ACW.PropInstParam>();
            from_propInstParamsUnamaped.Add(from_propInstPar0Unamaped);

            ACW.PropInstParamArray propInstParamsArrayUnamaped = new ACW.PropInstParamArray();
            propInstParamsArrayUnamaped.Items = from_propInstParamsUnamaped.ToArray();

            ACW.PropInstParamArray[] propInstParamsArraysUnamaped = new ACW.PropInstParamArray[1];
            propInstParamsArraysUnamaped[0] = propInstParamsArrayUnamaped;

            VaultConnection.WebServiceManager.DocumentService.UpdateFileProperties(ArryMasterId, propInstParamsArraysUnamaped);

            //ChangeOrderService changeOrderService = vaultConnection.WebServiceManager.ChangeOrderService;
            //long changeOrderId = GetChangeOrderIdByFileId(changeOrderService, fileId);

            //var affectedFiles = new Autodesk.Connectivity.WebServices.AffectedFile[]
            //{
            //    new Autodesk.Connectivity.WebServices.AffectedFile
            //    {
            //        FileId = fileId,
            //        ChangeNumber = 0, // 0を指定して最新のバージョンに変更
            //        ChangeOrderItemId = 0 // 0を指定して新しいアイテムを作成
            //    }
            //};

            //changeOrderService.UpdateChangeOrder(ChangeOrderUtils.GetNextStates(changeOrderId), affectedFiles, null);

        }

        /// <summary>
        /// 用途不明Add File
        /// </summary>
        /// <param name="filePath"></param>
        public void AddFile(string vaultFolder, string filePath)
        {
            ACW.Folder webfolder = new ACW.Folder();

            webfolder.FullName = "$/";

            VDF.Vault.Currency.Entities.Folder parent = new VDF.Vault.Currency.Entities.Folder(VaultConnection, webfolder);

            AddFile(filePath, parent);
        }

        /// <summary>
        ///  FileIteration AddFile(Currency.Entities.Folder parent, string comment, FileAssocParam[] associations, BOM bom, FileClassification classification, bool hidden, FilePathAbsolute localPathWithFileName);
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="parent"></param>
        public void AddFile(string filePath, VDF.Vault.Currency.Entities.Folder parent)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                VaultConnection.FileManager.AddFile(parent, System.IO.Path.GetFileName(filePath), "Added by Vault Browser", DateTime.Now, null, null, ACW.FileClassification.None, false, stream);
            }
        }

        /// <summary>
        /// 名前変更・テスト
        /// </summary>
        /// <param name="vaultFolderPath"></param>
        /// <param name="vaultName"></param>
        /// <param name="newVaultName"></param>
        /// <returns></returns>
        public ACW.FileRenameRestric[] FileRename(string vaultFolderPath, string vaultName, string newVaultName)
        {
            ACW.SrchCond[] _srchCond1 = MakeSrchCondsSysName("FolderPath", vaultFolderPath, "Name", vaultName);
            VDFVCE.FileIteration oSource = FindFileFirst(_srchCond1);

            if (oSource != null)
            {
                ACW.DocumentService documentService = VaultConnection.WebServiceManager.DocumentService;


                long filemasterID = oSource.EntityMasterId;

                /// 影響範囲を調査するテスト?
                ACW.FileRenameRestric[] test_resultFileRenameRestrictionsByMasterId = documentService.GetFileRenameRestrictionsByMasterId(filemasterID, newVaultName);

                //ACW.File file = documentService.GetFileById(filemasterID);
                // = Autodesk.Connectivity.WebServices.File file = documentService.GetFileById(fileId);

                //file.Name = newVaultName;

                //documentService.UpdateFilePropertyDefinitions(new Autodesk.Connectivity.WebServices.File[] { file });

                try
                {
                    TestPropertyUpdate2(oSource, "ClientFileName", "Number");
                }
                catch (Exception ex)
                {
                    this.WriteLine($"※VDFControl.RenameFile(..)で例外 {ex.Message}");
                }
            }
            else
            {
                if (oSource == null)
                    this.WriteLine($"※VDFControl.RenameFile(..)  {vaultFolderPath}/{vaultName} が見つかりません");
            }
            return null;
        }

        //public void rename3(Folder[] folders stistring rng )
        //{
        //    ACW.File checkedOutNewFile = CheckOut(folders[0], selectedFile);

        //    if (checkedOutNewFile != null)

        //    {

        //        CheckIn(folders[0], checkedOutNewFile, newname, Coment);

        //    }
        //}

        //public ACW.File CheckOut(Folder folder, ACW.File file)
        //{

        //    string localPath = _connection.WorkingFoldersManager.GetWorkingFolder(folder.FullName).FullPath;

        //    if (!System.IO.Directory.Exists(localPath))

        //        System.IO.Directory.CreateDirectory(localPath);

        //    AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(vaultConnection);

        //    settings.OptionsRelationshipGathering.FileRelationshipSettings.VersionGatheringOption = Autodesk.DataManagement.Client.Framework.Vault.Currency.VersionGatheringOption.Latest;

        //    settings.OptionsResolution.OverwriteOption = AcquireFilesSettings.AcquireFileResolutionOptions.OverwriteOptions.ForceOverwriteAll;

        //    settings.LocalPath = new VDF.Currency.FolderPathAbsolute(localPath);

        //    settings.AddFileToAcquire(

        //    new VDF.Vault.Currency.Entities.FileIteration(vaultConnection, file),

        //    VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download | VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Checkout);

        //    VDF.Vault.Results.AcquireFilesResults result = vaultConnection.FileManager.AcquireFiles(settings);

        //    foreach (var r in result.FileResults)
        //    {



        //        return m_serviceManager.DocumentService.GetFileById(r.File.EntityIterationId);



        //    }

        //}
        //public ACW.File CheckIn(Folder folder, ACW.File file, string newname, string comm)
        //{
        //    string localPath = vaultConnection.WorkingFoldersManager.GetWorkingFolder(folder.FullName).FullPath;

        //    string filePath = System.IO.Path.Combine(localPath, file.Name);

        //    VDF.Vault.Currency.Entities.FileIteration ff = vaultConnection.FileManager.CheckinFile(

        //    new VDF.Vault.Currency.Entities.FileIteration(vaultConnection, file), comm, false, null, null,
        //    true, newname, file.FileClass, false,

        //    new VDF.Currency.FilePathAbsolute(filePath));

        //    return vaultConnection.WebServiceManager.DocumentService.GetFileById(ff.EntityIterationId);

        //}


        /// ------------------------------------------- ////
        /// 

        ///// <summary>
        ///// テストコード
        ///// </summary>
        ///// <param name="SrchDonds"></param>
        ///// <returns></returns>
        //public VDFVCE.FileIteration testFindFileFirst(ACW.SrchCond[] SrchDonds)
        //{
        //    string bookmark = string.Empty;
        //    ACW.SrchStatus status = null;

        //    // 結果がACW.FILE[]で返される
        //    ACW.File[] results = vaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
        //                    SrchDonds,                // conditions
        //                    null,             // sortConditions
        //                    null,             // FolderIds
        //                    false,            // recurseFolders
        //                    true,             // lateset only
        //                    ref bookmark,    // bookmark
        //                    out status        // searchstatusは
        //                ); ;
        //    if (results != null)
        //    {
        //        ACW.File webServiceFile = results[0];

        //        fileIteraition = new VDFVCE.FileIteration(vaultConnection, webServiceFile);

        //        return fileIteraition;
        //    }
        //    else
        //        return null;
        //}



        // 削除メソッド開発中
        //public DeleteFileCommandEventArgs()
        //{
        //    //
        //    ACW.File acwFile = vaultConnection.WebServiceManager.DocumentService.Get

        //    var oAFI = new VDFVCE.FileIteration(vaultConnection, acwFile);

        //    long fileMasterId = GetFileMasterId(oAFI);
        //    long folderMasterId = GetFolderId(oAFI);
        //    vaultConnection.WebServiceManager.DocumentService.DeleteFileFromFolderUnconditional(fileMasterId, folderMasterId);
        //}
    }
}
