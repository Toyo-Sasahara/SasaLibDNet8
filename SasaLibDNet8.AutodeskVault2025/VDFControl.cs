// Autodesk Vaultシステム クライアント側制 プログラム 

// 参照アセンブリ
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.Connectivity.WebServices.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.Connectivity.WebServices.WCF.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.DataManagement.Client.Framework.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.DataManagement.Client.Framework.Forms.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.DataManagement.Client.Framework.Vault.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.DataManagement.Client.Framework.Vault.Forms.dll

// C:\Program Files\Autodesk\Inventor 2020\Bin\Public Assemblies\Autodesk.Inventor.Interop.dll

// C:\Program Files\Autodesk\Inventor 20XX\Bin\Connectivity.InventorAddin.EdmAddin.dll

// AdskLicensingSDK_5.dll をアセンブリと同じフォルダにコピーすること！！ (Ver 2022)
// I tested this morning, the AdskLicensingSDK_2.dll must be present in my output directory. Like this, the export works perfectly.
// https://forums.autodesk.com/t5/vault-customization/read-only-license-exception-to-get-user-and-group-from-api-in/td-p/8803658
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\AdskLicensingSDK_2.dll

// https://justonesandzeros.typepad.com/blog/vault/page/8/

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
using Autodesk.DataManagement.Client.Framework.Vault.Services.Connection;
using System.Windows;
//using Connectivity.InventorAddin.EdmAddin;

//using DEXX = DevExpress.XtraTreeList;
//using DEXUE = DevExpress.Utils.Extensions;
//using DEXXN = DevExpress.XtraPrinting.Native;
//using DEXU = DevExpress.Utils;
//using DEXXM = DevExpress.XtraRichEdit.Model;
//using DEXSI = DevExpress.Services.Internal;
//using DEXXR = DevExpress.XtraEditors.Repository;
//using DEXIWWUN = DevExpress.Internal.WinApi.Windows.UI.Notifications;
//using DevExpress.XtraRichEdit.Model;

namespace SasaLib.AutodeskVault
{
    /// <summary>
    /// ■Autodesk Vaultシステム クライアント側制御メインクラス Vault Development Framework (VDF)
    /// Autodesk.DataManagement.DLL で構成される。  VDF は一般的なVault アルゴリズムのための再利用可能なビジネス ロジックと、
    /// 一般的なワークフローのための再利用可能な GUI コントロールを提供する高レベルのフレームワークであり、アドインやクライアントの開発を簡素化します
    /// </summary>
    public partial class VDFControl
    {
        private SasaLibDelegateWriteLine WriteLine = DebugConsole.WriteLine;

        /// <summary>
        /// ■ﾛｸﾞｲﾝしているかを示す
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _IsConnected;
            }
            set
            {
                _IsConnected = value;

                //if (value == false)
                //{
                //    _IsEdmConnected = value;
                //}
            }
        }
        private bool _IsConnected;

        /// <summary>
        /// ■ｱﾄﾞｲﾝがﾛｸﾞｲﾝしているかを示す
        /// </summary>
        public bool IsEdmConnected
        {
            get
            {
                edmSecurity = Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance;

                if (edmSecurity == null)
                    return false;
                _IsEdmConnected = edmSecurity.IsSignedIn();
                return _IsEdmConnected;
            }
        }
        private bool _IsEdmConnected;

        /// <summary>
        /// ■コネクションオブジェクト
        /// </summary>
        public VDFVC.Connections.Connection VaultConnection
        {
            get
            {
                return _vaultConnection;
            }
            private set
            {
                _vaultConnection = value;
                if (_vaultConnection != null)
                {
                    AcwDocumentService = _vaultConnection.WebServiceManager.DocumentService;
                    AcwtWM = _vaultConnection.WebServiceManager;
                }
                else
                {
                    _vaultConnection = value;
                }
            }
        }
        private VDFVC.Connections.Connection _vaultConnection;

        /// <summary>
        /// Vault Serverとの通信に使用される一連の機能を提供します。サーバー機能は、機能に基づいてサービスに編成されています。
        /// </summary>
        private ACW.DocumentService AcwDocumentService { get; set; }

        private ACWTools.WebServiceManager AcwtWM { get; set; }

        /// <summary>
        /// ■ｱﾄﾞｲﾝでログインするために必要
        /// </summary>
        private Connectivity.InventorAddin.EdmAddin.EdmSecurity edmSecurity;

        /// <summary>
        /// ■コンストラクタ
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="vaultName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="methodWriteLine"></param>
        public VDFControl(string serverName, string vaultName, string userName, string password, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) this.WriteLine = DebugConsole.WriteLine;

            // Vaultログインウィンドウ
            VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                serverName, vaultName, userName, password, VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, null
                );
            if (!results.Success)
                return;

            VaultConnection = results.Connection;

            IsConnected = VaultConnection.IsConnected;


        }

        /// <summary>
        /// ■コンストラクタ
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="parent"></param>
        /// <param name="methodWriteLine"></param>
        public VDFControl(string caption, IntPtr parent, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) this.WriteLine = DebugConsole.WriteLine;

            LogIn(caption, parent);

            if (VaultConnection != null)
                IsConnected = VaultConnection.IsConnected;
        }

        /// <summary>
        /// ■コンストラクタ(InventorTOYOaddInにて使用)
        /// </summary>
        /// <param name="ShowEdmLoginWindow"></param>
        /// <param name="methodWriteLine"></param>
        public VDFControl(bool ShowEdmLoginWindow = true, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) this.WriteLine = DebugConsole.WriteLine;

            edmSecurity = Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance;
            if (edmSecurity.IsSignedIn())
            {
                this.WriteLine("■VDFControl(..) すでに EdmSecurity.IsSignedIn でした。");
            }
            else
            {
                if (ShowEdmLoginWindow)
                {
                    this.WriteLine("■VDFControl(..) VaultAddinﾛｸﾞｲﾝ ｳｨﾄﾞｳ 表示");
                    //this.o_InventorApp.CommandManager.ControlDefinitions["LoginCmdIntName"].Execute();
                    edmSecurity.OnLoginButtonExecute(true);
                }
            }
        }

        /// <summary>
        /// ■デストラクタ
        /// </summary>
        ~VDFControl()
        {
            if (VaultConnection != null)
            {
                VDF.Vault.Library.ConnectionManager.LogOut(VaultConnection);
            }
        }

        /// <summary>
        /// ■VDFVC.Connections.Connection を 外部から設定
        /// </summary>
        /// <param name="vaultConnection"></param>
        public void SetVaultConnection(VDFVC.Connections.Connection vaultConnection)
        {
            this.VaultConnection = vaultConnection;
        }

        /// <summary>
        /// ■Vaultｱﾄﾞｲﾝのログイン情報を使用せずにﾛｸﾞｲﾝ
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        public bool LogIn(string caption = null)
        {
            if (IsConnected == false)
            {
                VDF.Vault.Forms.Settings.LoginSettings settings = new VDF.Vault.Forms.Settings.LoginSettings();

                if (caption != null)
                {
                    settings.Caption = caption;
                }

                VaultConnection = VDF.Vault.Forms.Library.Login(settings);

                if (VaultConnection != null)
                {
                    IsConnected = VaultConnection.IsConnected;
                }
                else
                    IsConnected = false;
            }
            else
            {

            }
            return IsConnected;
        }

        /// <summary>
        /// ■Vaultにログインを実行する
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        private bool LogIn(string caption, IntPtr parent)
        {
            if (IsConnected == false)
            {
                VDF.Vault.Forms.Settings.LoginSettings settings = new VDF.Vault.Forms.Settings.LoginSettings();

                if (caption != null)
                {
                    settings.Caption = caption;
                }

                // 
                settings.OptionsWindow.SetParent(VDF.Forms.Settings.WindowSettings.ParentType.UseSpecificWindow, parent);

                VaultConnection = VDF.Vault.Forms.Library.Login(settings);

                if (VaultConnection != null)
                {
                    IsConnected = VaultConnection.IsConnected;
                }
                else
                    IsConnected = false;
            }

            return IsConnected;
        }

        /// <summary>
        /// ■Vaultから切断を実行する。
        /// </summary>
        /// <returns></returns>
        public bool LogOut()
        {
            var ans = VDF.Vault.Library.ConnectionManager.LogOut(VaultConnection);

            return ans;
        }

        /// <summary>
        /// ■ Vaultｱﾄﾞｲﾝからのﾛｸﾞｲﾝを実行します
        /// </summary>
        /// <returns></returns>
        public bool EdmLogIn(string serverName = null, string vaultName = null, string userName = null, string planePassword = null)
        {
            edmSecurity = Connectivity.InventorAddin.EdmAddin.EdmSecurity.Instance;

            VDFV.Currency.Connections.Connection EdmSecurityVaultConnection = null;

            if (edmSecurity.IsSignedIn())
            {
                this.WriteLine("■VDFControl.EdmLogIn() すでに EdmSecurity.IsSignedIn 済みです。");
            }
            else
            {
                this.WriteLine("■VDFControl.EdmLogIn() 警告 EdmSecurity.IsSignedIn 済みでないため Autodesk.DataManagement.Client.Framework.Vault.Library.ConnectionManager.LogIn(..)を試行します。 ");

                #region テストコード アドイン側にログイン結果が反映されません
                if (serverName != null && vaultName != null && userName != null && planePassword != null)
                {
                    VDFVC.Connections.AuthenticationFlags authFlags = VDFVC.Connections.AuthenticationFlags.Standard;
                    VDFVR.LogInResult logInResult = Autodesk.DataManagement.Client.Framework.Vault.Library.ConnectionManager.LogIn(
                       serverName: serverName, vaultName: vaultName, userName: userName, password: planePassword,
                       authFlags, null);

                    if (logInResult.Success == false)
                    {
                        if (logInResult.ErrorMessages.Count() > 0)
                        {
                            string errMsg = String.Join(", ", logInResult.ErrorMessages.Values.ToList());
                            this.WriteLine($"※VDFControl.EdmLogIn() Autodesk.DataManagement.Client.Framework.Vault.Library.ConnectionManager.LogIn() ErrorMessages {errMsg}");
                        }

                        this.WriteLine($"※VDFControl.EdmLogIn() Autodesk.DataManagement.Client.Framework.Vault.Library.ConnectionManager.LogIn() 失敗しました");

                        //o_InventorApp.CommandManager.ControlDefinitions["LoginCmdIntName"].Execute();
                        edmSecurity.OnLoginButtonExecute(true);
                    }
                    else
                    {
                        //EdmLoginPreferences a = edmSecurity.GetEdmLoginPreferences();
                        //EdmLoginPreferences b = edmSecurity.GetContentLoginPreferences();

                        EdmSecurityVaultConnection = logInResult.Connection;
                    } // 強制ログインが成功の場合、Connectionオブジェクトを EdmSecurityVaultConnection へ接続
                }
                else
                {
                    //o_InventorApp.CommandManager.ControlDefinitions["LoginCmdIntName"].Execute();
                    edmSecurity.OnLoginButtonExecute(true);
                }
                #endregion
            } // EdmSecurity.IsSignedIn() がfalseの場合、アカウント情報を使用してﾛｸﾞｲﾝを試みる

            VDF.Vault.Currency.Connections.Connection connection;

            // TODO; 参照失敗
            //if (edmSecurity.VaultConnection != null)
            //{
            //    EdmSecurityVaultConnection = edmSecurity.VaultConnection;
            //} // VaultAddinﾀﾞｲｱﾛｸﾞでのﾛｸﾞｲﾝが完了済みならそちらを優先

            if (EdmSecurityVaultConnection != null)
            {
                VaultConnection = EdmSecurityVaultConnection;

                this.WriteLine($"■VDFControl.EdmLogIn() Connection.IsConnected:{VaultConnection.IsConnected}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.IsReadOnly:{VaultConnection.IsReadOnly}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.Server:{VaultConnection.Server}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.UserName:{VaultConnection.UserName}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.Ticket:{VaultConnection.Ticket}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.IdentityKey:{VaultConnection.IdentityKey}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.IsAnonymousConnection:{VaultConnection.IsAnonymousConnection}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.IsAutodeskAuthenticatedConnection:{VaultConnection.IsAutodeskAuthenticatedConnection}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.IsWindowsAuthenticatedConnection:{VaultConnection.IsWindowsAuthenticatedConnection}");
                this.WriteLine($"■VDFControl.EdmLogIn() Connection.IsServerOnlyConnection:{VaultConnection.IsServerOnlyConnection}");
                return true;
            }
            else
            {
                return false;
            }
        }

        // -- //

        /// <summary>
        /// ■ボールトフォルダからワーキングフォルダを生成します。結果ではフォルダ名の最後に\が付きます
        /// 例 "$/abc" -> "D:\SasaVault\abc\"
        /// </summary>
        /// <param name="vaultFolder">例 "$/abc"</param>
        /// <returns>結果 "D:\SasaVault\abc\"</returns>
        public string GetVaultWorkFolder(string vaultFolder = "$")
        {
            try
            {

                VDF.Currency.FolderPathAbsolute workingFolder = VaultConnection.WorkingFoldersManager.GetWorkingFolder(vaultFolder);
                if (workingFolder != null)
                {
                    return workingFolder.FullPath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.WriteLine($"VDFControl.GetVaultWorkFolder({vaultFolder})にて例外{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ■FileIteration(ｼｽﾃﾑﾌﾟﾛﾊﾟﾃｨ ClientFileNameと FolderPath) と現在ログイン中のワーキングフォルダからﾛｰｶﾙﾌｧｲﾙ名を組立てます
        /// </summary>
        /// <param name="fileInteration"></param>
        /// <returns>null は例外発生時</returns>
        public string GetLocalFullFileName(VDFVCE.FileIteration fileInteration)
        {
            try
            {
                var VaultfolderPath = GetPropertyFromSysName(fileInteration, "FolderPath") as String;
                var VaultClientFileName = GetPropertyFromSysName(fileInteration, "ClientFileName").ToString();
                string localFolderPath = GetVaultWorkFolder(VaultfolderPath);
                string localFullFileName = System.IO.Path.Combine(localFolderPath, VaultClientFileName);

                return localFullFileName;

            }
            catch (Exception ex)
            {
                this.WriteLine($"VDFControl.GetLocalFullFileName(..)にて例外検知 {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ■現在のコネクションが認識しているVaultローカルワークフォルダ
        /// </summary>
        /// <returns></returns>
        public string GetWorkingFolderLocation()
        {
            ACW.DocumentService documentService = VaultConnection.WebServiceManager.DocumentService;
            string wrokingfolder = documentService.GetRequiredWorkingFolderLocation();
            return wrokingfolder;
        }

        /// <summary>
        /// ■ローカルファイルをVaultﾌｧｲﾙﾊﾟｽに変更 VDF.Currency.PathConverter クラスの機能を使用
        /// </summary>
        /// <param name="workSpacelocalFile"></param>
        /// <param name="vrootStr"></param>
        /// <param name="lrootStr"></param>
        /// <returns></returns>
        public VDF.Currency.VaultFilePathAbsolute ConvertToVaultFilePath(string workSpacelocalFile, string vrootStr = "$", string lrootStr = null)
        {
            if (lrootStr == null)
            {
                lrootStr = GetWorkingFolderLocation();
                this.WriteLine($"VDFControl.ConvertToVaultFilePath() 現在のワークフォルダ {lrootStr}");
            }

            VDF.Currency.VaultFolderPathAbsolute vroot = new VDF.Currency.VaultFolderPathAbsolute(vrootStr);
            VDF.Currency.FolderPathAbsolute lroot = new VDF.Currency.FolderPathAbsolute(lrootStr);
            VDF.Currency.FilePathAbsolute filePathAbsolute = new VDF.Currency.FilePathAbsolute(workSpacelocalFile);
            Autodesk.DataManagement.Client.Framework.Currency.PathConverter pathConverter = new VDF.Currency.PathConverter(vroot, lroot);

            VDF.Currency.VaultFilePathAbsolute vaultFilepath = pathConverter.ToVaultFile(filePathAbsolute);
            return vaultFilepath;
        }

        /// <summary>
        /// ■Vaultﾌｧｲﾙﾊﾟｽをローカルファイルパスに変更 VDF.Currency.PathConverter クラスの機能を使用
        /// </summary>
        /// <param name="VaultFilePath"></param>
        /// <param name="vrootStr"></param>
        /// <param name="lrootStr"></param>
        /// <returns></returns>
        public VDF.Currency.FilePathAbsolute ConvertToLocalFileFullPath(string VaultFilePath, string vrootStr = "$", string lrootStr = @"D:\MainVault")
        {
            VDF.Currency.VaultFolderPathAbsolute vroot = new VDF.Currency.VaultFolderPathAbsolute(vrootStr);
            VDF.Currency.FolderPathAbsolute lroot = new VDF.Currency.FolderPathAbsolute(lrootStr);
            VDF.Currency.VaultFilePathAbsolute vaultfilePathAbsolute = new VDF.Currency.VaultFilePathAbsolute(VaultFilePath);

            Autodesk.DataManagement.Client.Framework.Currency.PathConverter pathConverter = new VDF.Currency.PathConverter(vroot, lroot);

            VDF.Currency.FilePathAbsolute localFilefullpath = pathConverter.ToLocalFile(vaultfilePathAbsolute);
            return localFilefullpath;
        }

        /// <summary>
        /// ■ファイルのイテレーション ID を取得
        /// </summary>
        /// <param name="oAFI"></param>
        /// <returns></returns>
        public long GetFile_EntityIterationId(VDFVCE.FileIteration oAFI)
        {
            var EntityIterationId = oAFI.EntityIterationId;

            return EntityIterationId;

        }

        /// <summary>
        /// ■ファイルのマスターIDを検索
        /// </summary>
        /// <param name="oAFI"></param>
        /// <returns></returns>
        public long GetFile_EntityMasterId(VDFVCE.FileIteration oAFI)
        {
            var FileMasterId = oAFI.EntityMasterId;

            return FileMasterId;

        }

        /// <summary>
        /// ■ファイルがチェックアウトされたフォルダの ID を取得します。このプロパティは、IsCheckedOut が真の場合にのみ有効
        /// </summary>
        /// <param name="oAFI"></param>
        /// <returns></returns>
        public long GetCheckdOutFile_FolderId(VDFVCE.FileIteration oAFI)
        {
            var FolderId = oAFI.FolderId;

            return FolderId;

        }

        public string GetVaultFolderName(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("入力文字列が有効ではありません。");
            }

            // 最後の "/" のインデックスを検索
            int lastSlashIndex = input.LastIndexOf('/');

            if (lastSlashIndex != -1)
            {
                // "/" が見つかった場合、その位置までの部分文字列を返す
                return input.Substring(0, lastSlashIndex);
            }
            else
            {
                // "/" が見つからない場合、元の文字列をそのまま返す
                return input;
            }
        }
        // -- //

        /// <summary>
        /// ■FileIteration オブジェクトから DispNameのUDPを取得する
        /// </summary>
        /// <param name="fileInteration"></param>
        /// <param name="DispName"></param>
        /// <returns></returns>
        public object GetProperty(VDFVCE.FileIteration fileInteration, string DispName)
        {
            string SysName = default;

            // DispNameからSysNameを得ます
            ACW.PropDef[] filePropDefsTEST = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            try
            {
                SysName = filePropDefsTEST.Single(n => n.DispName == DispName).SysName;
            }
            catch (Exception ex)
            {
                WriteLine($"GetProperty(..) 例外検知 Vaultﾌﾟﾛﾊﾟﾃｨ名 SysName:{SysName} は存在しません {ex.Message}");
                return null;
            }

            VDFVCP.PropertyDefinitionDictionary propDefs;
            propDefs = VaultConnection.PropertyManager.GetPropertyDefinitions(VDF.Vault.Currency.Entities.EntityClassIds.Files, null, VDFVCP.PropertyDefinitionFilter.IncludeAll);

            object propValue;
            try
            {
                var propdef = propDefs[SysName];
                propValue = VaultConnection.PropertyManager.GetPropertyValue(fileInteration, propdef, null);
            }
            catch (Exception ex)
            {
                this.WriteLine($"GetProperty(..) 例外検知 Vaultﾌﾟﾛﾊﾟﾃｨ名 SysName:{SysName} は存在しません {ex.Message}");
                return null;
            }

            this.WriteLine($"{DispName.ToString()} = {propValue}");

            return propValue;
        }

        /// <summary>
        /// ■FileIteration オブジェクトから SysNameのプロパティを取得する
        /// </summary>
        /// <param name="fileInteration"></param>
        /// <param name="SysName"></param>
        /// <returns></returns>
        public object GetPropertyFromSysName(VDFVCE.FileIteration fileInteration, string SysName)
        {
            VDFVCP.PropertyDefinitionDictionary propDefs;
            propDefs = VaultConnection.PropertyManager.GetPropertyDefinitions(VDF.Vault.Currency.Entities.EntityClassIds.Files, null, VDF.Vault.Currency.Properties.PropertyDefinitionFilter.IncludeAll);

            object propValue;
            try
            {
                var propdef = propDefs[SysName];
                propValue = VaultConnection.PropertyManager.GetPropertyValue(fileInteration, propdef, null);
            }
            catch (Exception ex)
            {
                this.WriteLine($"GetProperty(..) 例外検知 Vaultﾌﾟﾛﾊﾟﾃｨ名 SysName:{SysName} は存在しません {ex.Message}");
                return null;
            }

            this.WriteLine($"{SysName.ToString()} = {propValue}");

            return propValue;
        }

        /// <summary>
        /// ■指定した EntityIterationId ＩＤをもつファイルの参照を取得する
        /// </summary>
        /// <param name="entityIterationId"></param>
        /// <returns></returns>
        public List<ACW.FileAssocParam> GetAssocParamsFromEntityIterationId(long entityIterationId)
        {
            try
            {
                ACW.File mFile = VaultConnection.WebServiceManager.DocumentService.GetFileById(entityIterationId);

                List<ACW.FileAssocParam> mFileAssocParams = new List<ACW.FileAssocParam>();

                ACW.FileAssocArray mFileAssocArray;

                /// マスターIDによってファイルのリストの最新のファイル関連付けを取得する。
                mFileAssocArray = AcwtWM.DocumentService.GetLatestFileAssociationsByMasterIds(new long[] { mFile.MasterId },
                                        ACW.FileAssociationTypeEnum.None, false,
                                        ACW.FileAssociationTypeEnum.All, false, false, false, true).FirstOrDefault();

                if (mFileAssocArray.FileAssocs != null)
                {
                    foreach (ACW.FileAssoc item in mFileAssocArray.FileAssocs)
                    {
                        ACW.FileAssocParam mFileAssocParam = new ACW.FileAssocParam();
                        mFileAssocParam.CldFileId = item.CldFile.Id;
                        mFileAssocParam.ExpectedVaultPath = item.ExpectedVaultPath;
                        mFileAssocParam.RefId = item.RefId;
                        mFileAssocParam.Source = item.Source;
                        mFileAssocParam.Typ = item.Typ;
                        mFileAssocParams.Add(mFileAssocParam);
                    }
                }

                return mFileAssocParams;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// ■このVaultが認識しているInventorプロジェクトファイル名を取得します。
        /// </summary>
        /// <returns></returns>
        public string GetInventorProjectFileLocation()
        {
            if (VaultConnection != null)
            {
                string s = VaultConnection.WebServiceManager.DocumentService.GetInventorProjectFileLocation();
                return s;
            }
            else
                return null;
        }

        // -- //

        #region ACW.SrchCond[]組み立て

        /// <summary>
        /// ■２つの検索条件を組立てる
        /// </summary>
        /// <param name="DispName1"></param>
        /// <param name="SearchValue1"></param>
        /// <param name="DispName2"></param>
        /// <param name="SearchValue2"></param>
        /// <returns></returns>
        public ACW.SrchCond[] MakeSrchConds(string DispName1, string SearchValue1, string DispName2, string SearchValue2)
        {
            if (VaultConnection == null || VaultConnection.IsConnected == false)
            {
                this.WriteLine($"Vaultにﾛｸﾞｲﾝしていません");
                return null;
            }

            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");

            ACW.PropDef NamePropDef1 = filePropDefs.Single(n => n.DispName == DispName1);

            this.WriteLine($"NamePropDef1.DispName={NamePropDef1.DispName},SysName={NamePropDef1.SysName},ID={NamePropDef1.Id},UsageCount={NamePropDef1.UsageCount}");

            ACW.SrchCond srchCond1 = new ACW.SrchCond()
            {
                PropDefId = NamePropDef1.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = SearchValue1
            };

            ACW.PropDef NamePropDef2 = null;
            try
            {
                NamePropDef2 = filePropDefs.Single(n => n.DispName == DispName2);
            }
            catch (Exception ex)
            {
                this.WriteLine($" NamePropDef2 = filePropDefs.Single(n => n.DispName == DispName2); で例外検知 DispName1:{DispName1},DispName2:{DispName2},{ex.Message}");
                { }
            }

            if (NamePropDef2 != null)
            {
                this.WriteLine($"NamePropDef2.DispName={NamePropDef2.DispName},SysName={NamePropDef2.SysName},ID={NamePropDef2.Id},UsageCount={NamePropDef2.UsageCount}");

                ACW.SrchCond srchCond2 = new ACW.SrchCond()
                {
                    PropDefId = NamePropDef2.Id,
                    PropTyp = ACW.PropertySearchType.SingleProperty,
                    SrchOper = 3, // is equal
                    SrchRule = ACW.SearchRuleType.Must,
                    SrchTxt = SearchValue2
                };

                ACW.SrchCond[] results;

                results = new ACW.SrchCond[] { srchCond1, srchCond2 };


                return results;
            }
            else
            {
                ACW.SrchCond srchCond2 = new ACW.SrchCond();

                ACW.SrchCond[] results;
                results = new ACW.SrchCond[] { srchCond1, srchCond2 };

                return results;
            }

        }

        /// <summary>
        /// ■２つの検索条件を組立てる(DispNameではなくSysNameとして指定)
        /// </summary>
        /// <param name="SysName1"></param>
        /// <param name="SearchValue1"></param>
        /// <param name="SysName2"></param>
        /// <param name="SearchValue2"></param>
        /// <returns></returns>
        public ACW.SrchCond[] MakeSrchCondsSysName(string SysName1, string SearchValue1, string SysName2, string SearchValue2)
        {
            if (VaultConnection.IsConnected)
            {

                ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");

                ACW.PropDef NamePropDef1 = filePropDefs.Single(n => n.SysName == SysName1);

                this.WriteLine($"NamePropDef1.SysName={NamePropDef1.SysName},SysName={NamePropDef1.SysName},ID={NamePropDef1.Id},UsageCount={NamePropDef1.UsageCount}");

                ACW.SrchCond srchCond1 = new ACW.SrchCond()
                {
                    PropDefId = NamePropDef1.Id,
                    PropTyp = ACW.PropertySearchType.SingleProperty,
                    SrchOper = 3, // is equal
                    SrchRule = ACW.SearchRuleType.Must,
                    SrchTxt = SearchValue1
                };

                ACW.PropDef NamePropDef2 = null;
                try
                {
                    NamePropDef2 = filePropDefs.Single(n => n.SysName == SysName2);
                }
                catch (Exception ex)
                {
                    this.WriteLine($" NamePropDef2 = filePropDefs.Single(n => n.SysName == SysName2); で例外検知 SysName1:{SysName1},SysName2:{SysName2},{ex.Message}");
                    { }
                }

                if (NamePropDef2 != null)
                {
                    this.WriteLine($"NamePropDef2.SysName={NamePropDef2.SysName},SysName={NamePropDef2.SysName},ID={NamePropDef2.Id},UsageCount={NamePropDef2.UsageCount}");

                    ACW.SrchCond srchCond2 = new ACW.SrchCond()
                    {
                        PropDefId = NamePropDef2.Id,
                        PropTyp = ACW.PropertySearchType.SingleProperty,
                        SrchOper = 3, // is equal
                        SrchRule = ACW.SearchRuleType.Must,
                        SrchTxt = SearchValue2
                    };

                    ACW.SrchCond[] results;

                    results = new ACW.SrchCond[] { srchCond1, srchCond2 };


                    return results;
                }
                else
                {
                    ACW.SrchCond srchCond2 = new ACW.SrchCond();

                    ACW.SrchCond[] results;
                    results = new ACW.SrchCond[] { srchCond1, srchCond2 };

                    return results;
                }

            }
            else
            {
                this.WriteLine($"MakeSrchCondsSysName() Vaultにログインしていません.これは本来発生させるべきでないエラーです");
                return null;
            }
        }

        /// <summary>
        /// ■名前が "PartNumber+.DWG" , ファイル拡張子が ".DWG" , DWGファイルのタイプがInventor以外 を検索
        /// </summary>
        /// <param name="PartNumber"></param>
        /// <returns></returns>
        public ACW.SrchCond[] MakeSrchCondsNotInventorDWGFile(string PartNumber)
        {
            if (VaultConnection.IsConnected == false)
            {
                this.WriteLine($"Vaultにﾛｸﾞｲﾝしていません");
                return null;
            }
            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");

            ////

            ACW.PropDef NamePropDef1 = filePropDefs.Single(n => n.DispName == "名前");

            this.WriteLine($"NamePropDef1.DispName={NamePropDef1.DispName},SysName={NamePropDef1.SysName},ID={NamePropDef1.Id},UsageCount={NamePropDef1.UsageCount}");


            ACW.SrchCond srchCond1 = new ACW.SrchCond()
            {
                PropDefId = NamePropDef1.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = PartNumber + ".DWG"
            };

            ////

            ACW.PropDef NamePropDef2 = filePropDefs.Single(n => n.DispName == "ファイル拡張子");

            this.WriteLine($"NamePropDef1.DispName={NamePropDef2.DispName},SysName={NamePropDef2.SysName},ID={NamePropDef2.Id},UsageCount={NamePropDef2.UsageCount}");

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

            this.WriteLine($"NamePropDef2.DispName={NamePropDef3.DispName},SysName={NamePropDef3.SysName},ID={NamePropDef3.Id},UsageCount={NamePropDef3.UsageCount}");

            ACW.SrchCond srchCond3 = new ACW.SrchCond()
            {
                PropDefId = NamePropDef3.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 2, // Does not contain
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = "Inventor"
            };

            ////

            ACW.SrchCond[] results = new ACW.SrchCond[] { srchCond1, srchCond2, srchCond3 };


            return results;
        }

        #endregion ACW.SrchCond[]組み立て

        #region Vault内検索

        /// <summary>
        /// ■■複数条件から１つの検索結果を得る(基本)
        /// </summary>
        /// <param name="SrchDonds"></param>
        /// <returns></returns>
        public VDFVCE.FileIteration FindFileFirst(ACW.SrchCond[] SrchDonds)
        {
            if (SrchDonds == null)
            {
                this.WriteLine($"※FindFileFirst(..) 検索条件が null です");
                return null;
            }

            string bookmark = string.Empty;
            ACW.SrchStatus status = null;

            ACW.File[] results = null;
            try
            {
                // 結果がACW.FILE[]で返される
                results = VaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
                                SrchDonds,                // conditions 検索条件の配列を指定します。 空の配列を渡すこともでき、その場合はすべてのFilesが返されます。 Vaultにあるすべてのファイルをスキャンするには、実はこの方法が最も効率的です。
                                null,             // sortConditions 順番を気にしないのであれば、NULLを渡すことができます。 並べ替えが必要な場合は、SrchSortクラスが非常に簡単です。 ソートしたいVaultのプロパティを指定し、昇順または降順にすることができます。 入力は配列で、複数のプロパティでソートできるようになっています。 配列の最初のオブジェクトが一次ソート、2 番目のオブジェクトが二次ソート、というようになります。
                                null,             // FolderIds FolderIds は、特定のフォルダのセットで検索したい場合に使用します。 Vault 全体を検索したい場合は、null を渡します。 ルートフォルダを指定すると、パフォーマンスが低下することがあります。
                                false,            // recurseFolders falseの場合は、指定したフォルダー内のファイルのみを検索します。trueの場合は、指定したフォルダーとそのサブフォルダーが検索されます
                                true,             // lateset only trueの場合は、ファイルの最新バージョンのみが返されます。それ以外の場合は、一致するすべてのバージョンが返されます。
                                ref bookmark,    // bookmark 複数のページを返す必要がある場合に使用します。 最初にFindを呼び出したとき、ブックマークには空の文字列を渡します。 最初の呼び出しですべての結果が得られなかった場合、ブックマークが更新されます。 次の呼び出しの際には、そのブックマークを使用することができ、前回の検索が終了した場所から検索が継続されます。
                                out status        // 検索結果の総数を知ることができます。 また、再インデックスが行われているかどうかも知ることができます。 再インデックスが行われている場合、検索結果が100%正確ではない可能性があります。 この場合、ユーザーへの警告以外にできることはあまりありません。
                            );

            }
            catch (Exception ex)
            {
                this.WriteLine($"※VDFVCE.FileIteration FindFileFirst(ACW.SrchCond[] SrchDonds) 例外発生 {ex.Message}");
            }

            if (results != null)
            {
                ACW.File webServiceFile = results[0];

                var fileIteraition = new VDFVCE.FileIteration(VaultConnection, webServiceFile);

                return fileIteraition;
            }
            else
                return null;
        }

        /// <summary>
        /// ■検索 最初に見つかった1項目のみを返す
        /// </summary>
        /// <param name="SearchValue"></param>
        /// <param name="SysName"></param>
        /// <returns></returns>
        public VDFVCE.FileIteration FindFileFirst(string SearchValue, string SysName = "Name")
        {
            if (string.IsNullOrEmpty(SearchValue) == true)
                return null;

            if (VaultConnection.WebServiceManager != null)
            {
                ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
                ACW.PropDef NamePropDef = filePropDefs.Single(n => n.SysName == SysName);

                #region 検索条件の準備

                ACW.SrchCond srchCond = new ACW.SrchCond()
                {
                    PropDefId = NamePropDef.Id,
                    PropTyp = ACW.PropertySearchType.SingleProperty,
                    SrchOper = 3, // is equal
                    SrchRule = ACW.SearchRuleType.Must,
                    SrchTxt = SearchValue
                };
                #endregion

                string bookmark = string.Empty;
                ACW.SrchStatus status = null;

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
                                ref bookmark,    // bookmark
                                out status        // searchstatusは
                            ); ;
                if (results != null)
                {
                    /// これはWebサービスファイルの周りのラッパーで、作業するオブジェクトのリッチ化など、さまざまな機能強化を提供しています。ファイルがチェックアウトされると、新しいバージョンがプレースホルダとして作成されます。チェックアウト状態では、このオブジェ
                    List<VDFVCE.FileIteration> fileIterationList = new List<VDFVCE.FileIteration>();

                    ACW.File webServiceFile = results[0];

                    var fileIteraition = new VDFVCE.FileIteration(VaultConnection, webServiceFile);

                    return fileIteraition;
                }
                else
                    return null;

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("vaultConnection.WebServiceManager が nullです", "エラー");
                Eventlog.Log.WriteEntry("SasaLib.AutodeskVault", System.Diagnostics.EventLogEntryType.Error, 0, $"※エラー。vaultConnection.WebServiceManagerがnullです");
                return null;
            }
        }

        /// <summary>
        /// ■Vaultシステムプロパティの "名前" "Name" にて１件検索
        /// </summary>
        /// <param name="SearchValue"></param>
        /// <param name="DispName"></param>
        /// <returns></returns>
        public VDFVCE.FileIteration FindFileFirstFromDispName(string SearchValue, string DispName = "名前", SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            if (string.IsNullOrEmpty(SearchValue) == true)
                return null;
            if (VaultConnection.IsConnected == false)
                return null;

            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef NamePropDef = filePropDefs.Single(n => n.DispName == DispName);

            #region 検索条件の準備

            ACW.SrchCond srchCond = new ACW.SrchCond()
            {
                PropDefId = NamePropDef.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = SearchValue
            };
            #endregion

            string bookmark = string.Empty;
            ACW.SrchStatus status = null;

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
                            ref bookmark,    // bookmark
                            out status        // searchstatusは
                        ); ;
            if (results != null)
            {
                ACW.File webServiceFile = results[0];

                var fileIteraition = new VDFVCE.FileIteration(VaultConnection, webServiceFile);

                return fileIteraition;
            }
            else
                return null;
        }

        /// <summary>
        /// ■Vaultシステムプロパティ Name と FolderPath ２つの条件にて検索する
        /// </summary>
        /// <param name="Name">Prop.Def.SysName == "Name" に該当するプロパティ値</param>
        /// <param name="FolderPath">>Prop.Def.SysName == "FolderPath" に該当するプロパティ値</param>
        /// <returns></returns>
        public VDFVCE.FileIteration FindFileAndFolderFirst(string Name, string FolderPath)
        {
            if (string.IsNullOrEmpty(Name) == true)
                return null;

            ACW.PropDef[] filePropDefs = VaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            ACW.PropDef NamePropDef = filePropDefs.Single(n => n.SysName == "Name");
            ACW.PropDef FolderPathPropDef = filePropDefs.Single(n => n.SysName == "FolderPath");

            #region 検索条件の準備

            ACW.SrchCond srchCond_Name = new ACW.SrchCond()
            {
                PropDefId = NamePropDef.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = Name
            };

            ACW.SrchCond srchCond_FolderPath = new ACW.SrchCond()
            {
                PropDefId = FolderPathPropDef.Id,
                PropTyp = ACW.PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = ACW.SearchRuleType.Must,
                SrchTxt = FolderPath
            };

            #endregion


            string bookmark = string.Empty;
            ACW.SrchStatus status = null;

            // 結果がACW.FILE[]で返される
            ACW.File[] results = VaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(
                            new ACW.SrchCond[] {
                                    srchCond_Name,
                                    srchCond_FolderPath,
                                //padlockProject
                            },                // conditions
                            null,             // sortConditions
                            null,             // FolderIds
                            false,            // recurseFolders
                            true,             // lateset only
                            ref bookmark,    // bookmark
                            out status        // searchstatusは
                        ); ;
            if (results != null)
            {
                /// これはWebサービスファイルの周りのラッパーで、作業するオブジェクトのリッチ化など、さまざまな機能強化を提供しています。ファイルがチェックアウトされると、新しいバージョンがプレースホルダとして作成されます。チェックアウト状態では、このオブジェ
                List<VDFVCE.FileIteration> fileIterationList = new List<VDFVCE.FileIteration>();

                ACW.File webServiceFile = results[0];

                var fileIteraition = new VDFVCE.FileIteration(VaultConnection, webServiceFile);

                return fileIteraition;
            }
            else
                return null;
        }

        /// <summary>
        /// ■部品番号からVaultファイルを検索(メソッド名がよくない)
        /// </summary>
        /// <param name="PartNumber"></param>
        /// <returns>VDFVCE.FileIteration 型のデータとして返す</returns>
        public VDFVCE.FileIteration SearchTOYOcomponentFile(string PartNumber, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            VDFVCE.FileIteration oAFI = null;

            if (oAFI == null)
            {
                ACW.SrchCond[] _srchCond1 = MakeSrchConds("ファイル拡張子", "IAM", "名前", PartNumber + ".IAM");
                oAFI = FindFileFirst(_srchCond1);
            }
            if (oAFI == null)
            {
                ACW.SrchCond[] _srchCond2 = MakeSrchConds("ファイル拡張子", "IAM", "部品番号", PartNumber);
                oAFI = FindFileFirst(_srchCond2);
            }
            if (oAFI == null)
            {
                ACW.SrchCond[] _srchCond3 = MakeSrchConds("ファイル拡張子", "IPT", "名前", PartNumber + ".IPT");
                oAFI = FindFileFirst(_srchCond3);
            }
            if (oAFI == null)
            {
                ACW.SrchCond[] _srchCond4 = MakeSrchConds("ファイル拡張子", "IPT", "部品番号", PartNumber);
                oAFI = FindFileFirst(_srchCond4);
            }

            if (oAFI != null) // 検索ヒット
            {
                methodWriteLine($"●VDFControl.SearchTOYOcomponentPartNumber(..) PartNumber:\"{PartNumber}\"がみつかりました。oAFI.EntityName:\"{oAFI.EntityName}\"");
                return oAFI;
            }
            else
            {
                methodWriteLine($"〇VDFControl.SearchTOYOcomponentPartNumber(..) PartNumber:\"{PartNumber}\"はみつかりませんでした");
                return null;
            }
        }

        /// <summary>
        /// ■Vaultから図面番号DWGファイルを検索する
        /// </summary>
        /// <param name="PartNumber"></param>
        /// <returns></returns>
        public VDFVCE.FileIteration SearchTOYOdwgFile(string PartNumber, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            VDFVCE.FileIteration oAFI = null;

            if (oAFI == null)
            {
                ACW.SrchCond[] _srchCond1 = MakeSrchCondsNotInventorDWGFile(PartNumber);
                oAFI = FindFileFirst(_srchCond1);
            }

            if (oAFI != null) // 検索ヒット
            {
                this.WriteLine($"●VDFControl.SearchTOYOdwgFile(..) PartNumber:\"{PartNumber}\"がみつかりました。oAFI.EntityName:\"{oAFI.EntityName}\"");
                return oAFI;
            }
            else
            {
                this.WriteLine($"〇VDFControl.SearchTOYOdwgFile(..) PartNumber:\"{PartNumber}\"はみつかりませんでした");
                return null;
            }
        }

        #endregion Vault内検索

        #region チェックアウトまたはダウンロード

        /// <summary>
        /// ■チェックアウト取消
        /// </summary>
        /// <param name="fileIter"></param>
        /// <returns></returns>
        public VDFVCE.FileIteration UndoCheckoutFile(VDFVCE.FileIteration fileIter, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            try
            {
                string filepath = GetLocalFullFileName(fileIter);
                this.WriteLine($"VDFVCE.FileIteration UndoCheckoutFile(..) ﾛｰｶﾙﾌｧｲﾙは {filepath}と判断しています");
                VDF.Currency.FilePathAbsolute filePathAbsolute = new VDF.Currency.FilePathAbsolute(filepath);
                VDFVCE.FileIteration result;
                if (fileIter.IsCheckedOut == true)
                {
                    result = VaultConnection.FileManager.UndoCheckoutFile(fileIter, filePathAbsolute);
                }
                else
                {
                    this.WriteLine($"VDFVCE.FileIteration UndoCheckoutFile(..)  {fileIter.EntityName} チェックアウトされていないファイルが指定されました ");
                    result = fileIter;
                }
                return result;
            }
            catch (Exception ex)
            {
                this.WriteLine($"VDFVCE.FileIteration UndoCheckoutFile(VDFVCE.FileIteration fileIter) 例外発生。{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ■チェックアウト（AcquireFiles(setting) 呼び出し。チェックアウトのみ。ダウンロードされません）
        /// </summary>
        /// <param name="fileIter"></param>
        /// <returns></returns>
        public VDF.Vault.Results.AcquireFilesResults CheckOutFile(VDFVCE.FileIteration fileIter, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection);

            settings.AddFileToAcquire(fileIter, VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Checkout);

            VDF.Vault.Results.AcquireFilesResults results = VaultConnection.FileManager.AcquireFiles(settings);

            return results;

        }

        /// <summary>
        /// ■ダウンロード（AcquireFiles(setting) 呼び出し。指定した VDFVCE.FileIteration オブジェクト１つのみをダウンロード）
        /// </summary>
        /// <param name="fileIter">対象ファイルを指定する</param>
        /// <param name="localPathToWriteFileTo">nullのときVaultで指定された作業フォルダにダウンロードされる</param>
        /// <returns></returns>
        public VDF.Vault.Results.AcquireFilesResults DownlodeFile(VDFVCE.FileIteration fileIter, string localPathToWriteFileTo = null, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection);

            if (localPathToWriteFileTo != null)
            {
                /// public void AddFileToAcquire(FileIteration file, AcquisitionOption acquisitionOption, FolderPathAbsolute downloadPath)
                settings.AddFileToAcquire(
                    fileIter,
                        VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download, // AcquireFilesSettings.AcquisitionOption Checkout, Download, NoAction
                        new VDF.Currency.FolderPathAbsolute(localPathToWriteFileTo)         // FilePathAbsolute downloadPath
                    );
            }
            else
            {
                settings.AddFileToAcquire(fileIter, VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download);
            }

            VDF.Vault.Results.AcquireFilesResults results = VaultConnection.FileManager.AcquireFiles(settings);


            return results;
        }

        /// <summary>
        /// ■ダウンロード（AcquireFiles(setting) 呼び出し。指定した ACW.File オブジェクト１つのみをダウンロード）
        /// </summary>
        /// <param name="vaultConnection"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public VDF.Vault.Results.AcquireFilesResults DownlodeFile(ACW.File file, string ChecoutComment, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            var fileIteration = new VDFVCE.FileIteration(VaultConnection, file);
            var settings = new VDFV.Settings.AcquireFilesSettings(VaultConnection);
            settings.CheckoutComment = ChecoutComment;
            settings.OptionsResolution.OverwriteOption = VDFV.Settings.AcquireFilesSettings.AcquireFileResolutionOptions.OverwriteOptions.ForceOverwriteAll;
            settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeChildren = true;
            settings.OptionsRelationshipGathering.FileRelationshipSettings.RecurseChildren = true;
            settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeLibraryContents = true;
            settings.OptionsRelationshipGathering.FileRelationshipSettings.VersionGatheringOption =
                Autodesk.DataManagement.Client.Framework.Vault.Currency.VersionGatheringOption.Latest;
            settings.DefaultAcquisitionOption = VDFV.Settings.AcquireFilesSettings.AcquisitionOption.Download;
            settings.AddFileToAcquire(fileIteration, VDFV.Settings.AcquireFilesSettings.AcquisitionOption.Download);

            var downloadedFiles = VaultConnection.FileManager.AcquireFiles(settings);
            return downloadedFiles;
        }

        /// <summary>
        /// ■ダウンロード （AcquireFiles()呼び出し。参照先も含めてダウンロード。）
        /// </summary>
        /// <param name="fileIter">VDFVCE.FileIteration オブジェクト</param>
        /// <param name="localPathToWriteFileTo">ダウンロード先のパス null の場合は元の場所</param>
        /// <param name="acquisitionOption"></param>
        /// <returns></returns>
        public VDF.Vault.Results.AcquireFilesResults DownlodFileIncludeChildren(VDFVCE.FileIteration fileIter, string localPathToWriteFileTo = null, VDFV.Settings.AcquireFilesSettings.AcquisitionOption acquisitionOption = VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            VDFV.Settings.AcquireFilesSettings settings = new VDFV.Settings.AcquireFilesSettings(VaultConnection);

            if (localPathToWriteFileTo != null)
            {
                settings.AddFileToAcquire(
                    fileIter,
                        acquisitionOption, // AcquireFilesSettings.AcquisitionOption Checkout, Download, NoAction

                        new VDF.Currency.FolderPathAbsolute(localPathToWriteFileTo)         // FilePathAbsolute downloadPath
                    );
            }
            else
            {
                settings.AddFileToAcquire(fileIter, acquisitionOption);
            }

            // Gets whether children of a source file should be consumed. For example, an assembly depends on it's parts. 
            // ソースファイルの子ファイルが消費されるかどうかを取得します。例えば、アセンブリはその部品に依存します。
            settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeChildren = true;

            // Gets or sets whether children should be included recursively, or if just the first level of children should be included 
            // 子供を再帰的に含めるか、または子供の最初のレベルだけを含めるかを取得または設定します
            settings.OptionsRelationshipGathering.FileRelationshipSettings.RecurseChildren = true;

            // Gets or sets whether parents should be included recursively, or if just the first level parent should be included 
            // 親を再帰的に含めるか、第一階層の親だけを含めるかを取得または設定する。
            settings.OptionsRelationshipGathering.FileRelationshipSettings.RecurseParents = true;

            // Gets or sets whether parents of the source entity will be gathered. For example, the Assembly associated with a part. 
            // ソース・エンティティの親が収集されるかどうかを取得または設定します。たとえば、部品に関連付けられたアセンブリ。
            settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeParents = false;

            // Gets or sets whether gathered entities that are a part of a Vault Library should be consumed. 
            // Vault Libraryの一部である収集されたエンティティを消費するかどうかを取得または設定します。
            settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeLibraryContents = true;

            // Gets or sets whether related documentation that is associated with the entity will be gathered. For examplee, a DWG might be considered related documentation for an assembly. The DWG is not considered an attachment or a dependent. 
            // エンティティに関連する関連ドキュメントを収集するかどうかを取得または設定します。例えば、DWG はアセンブリの関連文書とみなされるかもしれません。DWGは、添付ファイルや従属物とはみなされません。
            settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeRelatedDocumentation = true;

            // Gets or sets an option which determines which versions of related files should be gathered. 
            // 関連するファイルのどのバージョンを収集するかを決定するオプションを取得または設定します。
            settings.OptionsRelationshipGathering.FileRelationshipSettings.VersionGatheringOption = VDF.Vault.Currency.VersionGatheringOption.Latest;

            VDF.Vault.Results.AcquireFilesResults results = VaultConnection.FileManager.AcquireFiles(settings);

            return results;
        }

        /// <summary>
        /// ■ダウンロード（AcquireFiles(setting) 呼び出し。参照先も含めてダウンロード） 
        /// </summary>
        /// <param name="topLevelFileIter"></param>
        public bool DownloadAssembly(VDFVCE.FileIteration topLevelFileIter, string NewTopFolder, out VDFVR.AcquireFilesResults acquireFilesResultsOut, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            acquireFilesResultsOut = null;

            try
            {
                VDF.Currency.FolderPathAbsolute workingFolder = VaultConnection.WorkingFoldersManager.GetWorkingFolder("$");
                string _worikingRootFolder = workingFolder.FullPath; //"G:\\SasaVault\\"

                // 最初にﾄｯﾌﾟｱｾﾝﾌﾞﾘのみダウンロードします
                ///　fileIteration から本来のVaultFullPathを得ます
                string _topLevelfileVaultFullPath = VaultConnection.WorkingFoldersManager.GetPathOfFileInWorkingFolder(topLevelFileIter).FullPath;

                this.WriteLine($"{_topLevelfileVaultFullPath}をﾀﾞｳﾝﾛｰﾄﾞします 保存先は 現在のVault作業ﾌｫﾙﾀ{_worikingRootFolder}を{NewTopFolder}に置換した場所になります");
                this.DownloadOneFile(topLevelFileIter, NewTopFolder);
                this.WriteLine($"{_topLevelfileVaultFullPath}をﾀﾞｳﾝﾛｰﾄﾞ完了");

                // ﾌｧｲﾙダウンロードのための設定を生成します
                VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection);
                //
                settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeChildren = true;
                //
                settings.OptionsRelationshipGathering.FileRelationshipSettings.RecurseChildren = true;
                //
                settings.OptionsRelationshipGathering.FileRelationshipSettings.RecurseParents = true;
                //
                settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeParents = false;
                //
                settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeLibraryContents = true;
                // 
                settings.OptionsRelationshipGathering.FileRelationshipSettings.IncludeRelatedDocumentation = true;
                // ﾌｧｲﾙの最新版を指定
                settings.OptionsRelationshipGathering.FileRelationshipSettings.VersionGatheringOption = VDF.Vault.Currency.VersionGatheringOption.Latest;

                // 指定ﾌｧｲﾙから参照ﾌｧｲﾙを検索します。
                settings.AddFileToAcquire(
                    topLevelFileIter,
                    VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.NoAction // AcquireFilesSettings.AcquisitionOption Checkout, Download, NoAction
                    );
                string tempforlder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName());
                FileFolder.MakeDirectory(tempforlder.TrimEnd('\\'));
                settings.LocalPath = new VDF.Currency.FolderPathAbsolute(tempforlder);

                this.WriteLine($"{topLevelFileIter.EntityName} の参照を検索しています・・・");

                acquireFilesResultsOut = VaultConnection.FileManager.AcquireFiles(settings); // ﾌｧｲﾙのダウンロードを実行します。検索結果はVaultローカルフォルダにダウンロード

                this.WriteLine($"検索終了。{topLevelFileIter.EntityName}を除いた{acquireFilesResultsOut.FileResults.Count()}件のﾀﾞｳﾝﾛｰﾄﾞを開始します");

                int count = 0;
                // 検索結果から1ﾌｧｲﾙ取り出して処理
                foreach (VDF.Vault.Results.FileAcquisitionResult _FileAcquisitionResult in acquireFilesResultsOut.FileResults)
                {
                    {
                        string LocalPathFolderPath = _FileAcquisitionResult.LocalPath.FolderPath;
                    }/// テスト用コード

                    VDFVCE.FileIteration oAFi = _FileAcquisitionResult.File;
                    this.DownloadOneFile(oAFi, NewTopFolder);
                    count++;
                    {
                        string _oAFiVaultFullPath = VaultConnection.WorkingFoldersManager.GetPathOfFileInWorkingFolder(oAFi).FullPath;
                        this.WriteLine($"{_oAFiVaultFullPath}をﾀﾞｳﾝﾛｰﾄしました {count}/{acquireFilesResultsOut.FileResults.Count()}件目");
                    }
                }

            }
            catch (Exception ex)
            {
                this.WriteLine($"VDFControl.DownloadAssembly(...)にて例外発生{ex.Message}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// ■ダウンロード（AcquireFiles(setting) 呼び出し。指定したVDFVCE.FileIteration のコレクションオブジェクトにより複数をダウンロード）
        /// </summary>
        /// <param name="fileIters"></param>
        /// <param name="localPathToWriteFileTo"></param>
        /// <returns>nullのときVaultで指定された作業フォルダにダウンロードされる</returns>
        public VDF.Vault.Results.AcquireFilesResults DownlodFiles(ICollection<VDFVCE.FileIteration> fileIters, string localPathToWriteFileTo, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            // テンポラリにファイルをダウンロードする
            VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection);

            settings.LocalPath = new VDF.Currency.FolderPathAbsolute(localPathToWriteFileTo);

            foreach (VDFVCE.FileIteration fileIter in fileIters)
            {
                settings.AddFileToAcquire(fileIter, VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download);
            }
            VDF.Vault.Results.AcquireFilesResults results = VaultConnection.FileManager.AcquireFiles(settings);

            return results;
        }

        /// <summary>
        /// ■ダウンロード(Vaultファイルパスを指定してダウンロードする)
        /// </summary>
        /// <param name="VaultFullFileName"></param>
        /// <param name="result"></param>
        /// <param name="methodWriteLine"></param>
        /// <returns></returns>
        public bool DownlodeFile(string VaultFullFileName, ref VDF.Vault.Results.AcquireFilesResults result, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            // https://justonesandzeros.typepad.com/blog/2013/05/how-to-acquire-files.html
            int pos1 = VaultFullFileName.LastIndexOf("/");
            string VaultFileName = VaultFullFileName.Substring(pos1 + 1, VaultFullFileName.Length - pos1 - 1);

            int pos2 = VaultFullFileName.LastIndexOf(VaultFileName);
            string VaultFolderPath = VaultFullFileName.Substring(0, pos2 - 1);


            VDFVCE.FileIteration fileIter = FindFileAndFolderFirst(VaultFileName, VaultFolderPath);
            if (fileIter != null)
            {
                result = DownlodeFile(fileIter);
                if (result != null)
                {
                    this.WriteLine($"■ボルトファイル名：【{VaultFileName}】 フォルダのパス【{VaultFolderPath}】ダウンロード成功");
                    return true;
                }
                else
                {
                    this.WriteLine($"※ボルトファイル名：【{VaultFileName}】 フォルダのパス【{VaultFolderPath}】ダウンロード失敗");
                    return false;
                }
            }
            else
            {
                this.WriteLine($"※見つかりません ボルトに検索するファイル名：【{VaultFileName}】 フォルダのパス【{VaultFolderPath}】");
                return false;
            }
        }

        /// <summary>
        /// ■ダウンロード（Vaultエンティティパスを指定）
        /// </summary>
        /// <param name="VaultFullFileName">Vaultファイルパス 例：</param>
        /// <param name="localPathToWriteFileTo"></param>
        /// <param name="this.WriteLine"></param>
        /// <returns></returns>
        public VDF.Vault.Results.AcquireFilesResults DownlodeFile(string VaultFullFileName, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            // https://justonesandzeros.typepad.com/blog/2013/05/how-to-acquire-files.html
            int pos1 = VaultFullFileName.LastIndexOf("/");
            string VaultFileName = VaultFullFileName.Substring(pos1 + 1, VaultFullFileName.Length - pos1 - 1);

            int pos2 = VaultFullFileName.LastIndexOf(VaultFileName);
            string VaultFolderPath = VaultFullFileName.Substring(0, pos2 - 1);

            this.WriteLine($"ボルトに検索するファイル名：【{VaultFileName}】 フォルダのパス【{VaultFolderPath}】");

            VDFVCE.FileIteration fileIter = FindFileAndFolderFirst(VaultFileName, VaultFolderPath);
            if (fileIter != null)
            {
                var result = DownlodeFile(fileIter);

                return result;

            }
            else
            {
                VDF.Vault.Results.AcquireFilesResults acquireFilesResults = default;
                return acquireFilesResults;
            }
        }

        /// <summary>
        /// ■ダウンロード（指定した VDFVCE.FileIteration オブジェクト の最新をダウンロード.）
        /// </summary>
        /// <param name="fileIter"></param>
        /// <param name="vaultlocalFullpath"></param>
        /// <param name="this.WriteLine"></param>
        /// <returns></returns>
        public bool DownloadFileFromFilename(VDFVCE.FileIteration fileIter, ref string vaultlocalFullpath, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            /// 検索結果oAFIを使い実体ファイル(最新)をダウンロード（Vaultローカルフォルダ）
            /// ADCFVR.AcquireFilesResults oAFR　にダウンロードした各ファイルの情報が得られる
            VDFVR.AcquireFilesResults oAFR = DownlodeFile(fileIter);
            if (oAFR != null)
            {
                /// 結果oAFR
                IEnumerable<VDFVR.FileAcquisitionResult> oFARenum = oAFR.FileResults;

                if (oFARenum.Count() == 1)
                {
                    VDFVR.FileAcquisitionResult ofileAcquistionResult = oFARenum.First();
                    vaultlocalFullpath = ofileAcquistionResult.LocalPath.FullPath;
                    this.WriteLine($"VDFControl.DownloadFileFromFilename(..) ボルトに１つ見つかりました：\"{vaultlocalFullpath}\"");

                    return true;
                }
                else // 結果が複数見つかったのでエラー
                {
                    this.WriteLine($"結果が複数見つかってしまいました");
                    foreach (Autodesk.DataManagement.Client.Framework.Vault.Results.FileAcquisitionResult ofileAcquistionResult in oFARenum)
                    {
                        this.WriteLine($"  \"{ofileAcquistionResult.LocalPath.FullPath}\"");
                        this.WriteLine($"　総数{oFARenum.Count()}");
                    }
                    vaultlocalFullpath = null;

                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ■トップフォルダを置換した場所に指定したﾌｧｲﾙをダウンロードする
        /// </summary>
        /// <param name="fileIteration"></param>
        /// <param name="NewTopFolder"></param>
        /// <returns></returns>
        private VDF.Vault.Results.FileAcquisitionResult DownloadOneFile(VDFVCE.FileIteration fileIteration, string NewTopFolder, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            ///　fileIteration から本来のVaultFullPathを得ます
            VDF.Currency.FilePathAbsolute _flePathAbsolute = VaultConnection.WorkingFoldersManager.GetPathOfFileInWorkingFolder(fileIteration);
            string _VaultLocalFullPath = _flePathAbsolute.FullPath;

            VDF.Currency.FolderPathAbsolute workingFolder = VaultConnection.WorkingFoldersManager.GetWorkingFolder("$");

            string _worikingRootFolder = workingFolder.FullPath; //"G:\\SasaVault\\"

            string _currentWorkingFolderRegexPatern = _worikingRootFolder.TrimEnd('\\').Replace(@"\", @"\\");  // "G:\\SasaVault\\" -> "G:\\SasaVault"
            string _newTopFolder = NewTopFolder.TrimEnd('\\'); // 
            string _newFullpath = Regex.Replace(_VaultLocalFullPath, _currentWorkingFolderRegexPatern, _newTopFolder); // ダウンロード先トップフォルダが変更されたフルパス
            string _newFolder = System.IO.Path.GetDirectoryName(_newFullpath); // フォルダのみ抽出。ダウンロードされたファイルはここに入ります。

            VDF.Vault.Settings.AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(VaultConnection);
            settings.AddFileToAcquire(
                fileIteration,
                    VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download, // AcquireFilesSettings.AcquisitionOption Checkout, Download, NoAction
                    new VDF.Currency.FolderPathAbsolute(_newFolder)         // FilePathAbsolute downloadPath
                );
            VDF.Vault.Results.AcquireFilesResults results = VaultConnection.FileManager.AcquireFiles(settings);

            VDF.Vault.Results.FileAcquisitionResult result = results.FileResults.First();
            return result;
        }

        /// <summary>
        /// ■VaultダウンロードダイアログにてDL
        /// </summary>
        /// <param name="fileIter"></param>
        /// <param name="parentWindowHandle"></param>
        public VDF.Vault.Results.AcquireFilesResults DownloadDialog(List<VDFVCE.FileIteration> fileIterationList, IntPtr parentWindowHandle, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            // Downloads and/or checks out files form the vault 
            // pop up the Get/Checkout dialog
            VDF.Vault.Forms.Settings.InteractiveAcquireFileSettings settings
                = new VDF.Vault.Forms.Settings.InteractiveAcquireFileSettings(VaultConnection, parentWindowHandle, "Download files");

            foreach (var fileIteration in fileIterationList)
            {
                if (fileIteration == null)
                {
                    return null;
                }
                settings.AddEntityToAcquire(fileIteration);
            }

            var result = VDF.Vault.Forms.Library.AcquireFiles(settings);
            return result;
        }

        #endregion チェックアウトまたはダウンロード

        #region チェックインまたはアップロード

        /// <summary>
        /// ■ファイルのチェックイン
        /// </summary>
        /// <param name="fileIter"></param>
        /// <returns></returns>
        public VDFVCE.FileIteration CheckInFile(VDFVCE.FileIteration fileIter, string comment = "", SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            //  https://forums.autodesk.com/t5/vault-customization/check-in-files-by-api/td-p/3134336
            try
            {
                VDFVCE.FileIteration resultFileIteration;
                if (fileIter.IsCheckedOut == true)
                {
                    ACW.FileAssocParam[] resutlassoc = GetAssocParamsFromEntityIterationId(fileIter.EntityIterationId).ToArray();

                    string newFilename = null;
                    VDF.Currency.FilePathAbsolute filePath = null;

                    resultFileIteration = VaultConnection.FileManager.CheckinFile(file: fileIter, comment: comment, keepCheckedOut: false, associations: resutlassoc, bom: null, copyBom: true, newFileName: newFilename, classification: fileIter.FileClassification, hidden: false, filePath: filePath);

                    return resultFileIteration;
                }
                else
                {
                    methodWriteLine($"VDFVCE.FileIteration CheckInFile(..)  {fileIter.EntityName} チェックアウトされていないファイルが指定されました ");

                    return null;

                }
            }
            catch (Exception ex)
            {
                methodWriteLine($"VDFVCE.FileIteration CheckInFile(VDFVCE.FileIteration fileIter) 例外発生。{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ■ﾌｧｲﾙリソースをVaultへアップロード
        /// </summary>
        /// <param name="svcmgr"></param>
        /// <param name="filename"></param>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public ACW.ByteArray UploadFileResource(ACWTools.WebServiceManager svcmgr, string filename, byte[] fileContents, int MAX_FILE_TRANSFER_SIZE = 2147483647, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            try
            {

                svcmgr.FilestoreService.FileTransferHeaderValue = new ACW.FileTransferHeader();
                svcmgr.FilestoreService.FileTransferHeaderValue.Identity = Guid.NewGuid();
                svcmgr.FilestoreService.FileTransferHeaderValue.Extension = System.IO.Path.GetExtension(filename);
                svcmgr.FilestoreService.FileTransferHeaderValue.Vault = svcmgr.WebServiceCredentials.VaultName;

                ACW.ByteArray uploadTicket = new ACW.ByteArray();
                int bytesTotal = (fileContents != null ? fileContents.Length : 0);
                int bytesTransferred = 0;
                do
                {
                    int bufferSize = (bytesTotal - bytesTransferred) % MAX_FILE_TRANSFER_SIZE;
                    byte[] buffer = null;
                    if (bufferSize == bytesTotal)
                    {
                        buffer = fileContents;
                    }
                    else
                    {
                        buffer = new byte[bufferSize];
                        Array.Copy(fileContents, (long)bytesTransferred, buffer, 0, (long)bufferSize);
                    }

                    svcmgr.FilestoreService.FileTransferHeaderValue.Compression = ACW.Compression.None;
                    svcmgr.FilestoreService.FileTransferHeaderValue.IsComplete = (bytesTransferred + bufferSize) == bytesTotal ? true : false;
                    svcmgr.FilestoreService.FileTransferHeaderValue.UncompressedSize = bufferSize;

                    using (var fileContentsStream = new MemoryStream(fileContents))
                        uploadTicket.Bytes = svcmgr.FilestoreService.UploadFilePart(fileContentsStream);
                    bytesTransferred += bufferSize;

                } while (bytesTransferred < bytesTotal);

                return uploadTicket;

            }
            catch (Exception ex)
            {
                this.WriteLine($"※VDFControl.UploadFileResource(..)にて例外 {ex.Message}");

                return null;
            }

        }

        /// <summary>
        /// ■新規ファイルをチェックイン
        /// </summary>
        /// <param name="vaultFolder"></param>
        /// <param name="CheckIn_fullFileName"></param>
        /// <param name="errcode"></param>
        /// <returns></returns>
        public ACW.File AddUploadFile(string vaultFolder, string CheckIn_fullFileName, out int errcode, string versionComment = null, string vaultFileName = null, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            errcode = 0;
            var oDocumentService = VaultConnection.WebServiceManager.DocumentService;

            ACW.Folder oFolder = oDocumentService.GetFolderByPath(vaultFolder);
            var resutl = AddUploadFile(oFolder, CheckIn_fullFileName, out errcode, versionComment, vaultFileName);
            return resutl;
        }

        /// <summary>
        /// ■新規ファイルをチェックイン
        /// </summary>
        /// <param name="oFolder"></param>
        /// <param name="CheckIn_fullFileName"></param>
        /// <param name="errcode"></param>
        /// <param name="versionComment"></param>
        /// <param name="vaultFileName"></param>
        /// <returns></returns>
        private ACW.File AddUploadFile(ACW.Folder oFolder, string CheckIn_fullFileName, out int errcode, string versionComment = null, string vaultFileName = null, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            try
            {
                //ファイルを開く
                System.IO.FileStream fs = new System.IO.FileStream(CheckIn_fullFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //ファイルを読み込むバイト型配列を作成する
                byte[] bs = new byte[fs.Length];
                //ファイルの内容をすべて読み込む
                fs.Read(bs, 0, bs.Length);
                //閉じる
                fs.Close();

                ACWTools.WebServiceManager svcmgr = VaultConnection.WebServiceManager;
                ACW.ByteArray byteArray = UploadFileResource(svcmgr, CheckIn_fullFileName, bs);

                var oDocumentService = VaultConnection.WebServiceManager.DocumentService;

                //FileAssocParam[] fileAssocParams = new FileAssocParam[1];
                //ACW.BOM bom = new ACW.BOM();

                /*
                 * public override File AddUploadedFile(
                        System.long folderId, System.string fileName, System.string comment,
                        System.DateTime lastWrite, FileAssocParam[] associations, BOM bom, FileClassification fileClass,
                        System.bool hidden,   ByteArray uploadticket)
                パラメータ
                    folderId 
                        新しいファイルが追加されるフォルダーです。
                    fileName 
                        Vaultに表示されるファイル名です。
                    comment 
                        Text data to be associated with version 1 of the file.
                    lastWrite 
                        ファイルのバージョン1と関連付けるテキストデータ。
                    associations 
                        ファイルに関するアソシエーションです。
                    bom 
                       このファイルに関連付ける部品表。部品表がない場合は NULL を渡す必要があります。.
                    fileClass 
                         ファイルの分類を示す。e.
                    hidden 
                        trueの場合、ファイルはクライアントによって隠される必要があります。
                    uploadticket 
                         アップロードされたバイナリデータの識別子。.
                
                戻り値
                    そのファイルに対するVaultのビューを表すFileオブジェクトです。

                必要な権限
                    FileCreate

                備考
                    この関数は、バイナリデータがFilestoreServiceを通じてアップロードされた後に使用されます。 この関数は、バイナリデータとVaultのメタデータを関連付け、ファイルのVault表現を作成します。                 */

                string _fileName;
                if (vaultFileName == null)
                    _fileName = System.IO.Path.GetFileName(CheckIn_fullFileName);
                else
                    _fileName = vaultFileName;

                ACW.File file = oDocumentService.AddUploadedFile(oFolder.Id, _fileName, versionComment, DateTime.Now, null, null, ACW.FileClassification.DesignRepresentation, false, byteArray);
                errcode = 0;
                return file;
            }
            catch (Exception ex)
            {

                // int型に変換 エラーコードとして扱えるなら
                int i;
                if (int.TryParse(ex.Message, out i))
                {
                    errcode = i;
                    return null;
                }
                else
                {
                    this.WriteLine($"※VDFControl.AddUploadFile(..)にて例外 {ex.Message}");
                    errcode = 0;
                }
                return null;
            }
        }

        /// <summary>
        /// ■既存のチェックアウトずみファイルに対して新しいファイルをアップロードしチェックイン（ﾌｧｲﾙ参照は考慮しません）
        /// </summary>
        /// <param name="fileIter"></param>
        /// <param name="CheckIn_fullFileName"></param>
        /// <param name="errcode"></param>
        /// <param name="versionComment"></param>
        /// <param name="keepChekedOut"></param>
        /// <param name="vaultNewFileName"></param>
        /// <returns></returns>
        public ACW.File CheckinUploadfile(VDFVCE.FileIteration fileIter, string CheckIn_fullFileName, out int errcode, string versionComment = null, bool keepChekedOut = false, string vaultNewFileName = null, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            try
            {
                long fileMasterId = fileIter.EntityMasterId;

                //ファイルを開く
                System.IO.FileStream fs = new System.IO.FileStream(CheckIn_fullFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //ファイルを読み込むバイト型配列を作成する
                byte[] bs = new byte[fs.Length];
                //ファイルの内容をすべて読み込む
                fs.Read(bs, 0, bs.Length);
                //閉じる
                fs.Close();

                ACWTools.WebServiceManager svcmgr = VaultConnection.WebServiceManager;
                ACW.ByteArray byteArray = UploadFileResource(svcmgr, CheckIn_fullFileName, bs);

                var oDocumentService = VaultConnection.WebServiceManager.DocumentService;

                //FileAssocParam[] fileAssocParams = new FileAssocParam[1];
                //ACW.BOM bom = new ACW.BOM();

                string _newFileName;
                if (vaultNewFileName == null)
                    _newFileName = System.IO.Path.GetFileName(CheckIn_fullFileName);
                else
                    _newFileName = vaultNewFileName;


                ACW.File file = oDocumentService.CheckinUploadedFile(fileMasterId, versionComment, keepChekedOut, DateTime.Now, null, null, false, _newFileName, ACW.FileClassification.DesignRepresentation, false, byteArray);
                errcode = 0;
                return file;
            }
            catch (Exception ex)
            {

                // int型に変換 エラーコードとして扱えるなら
                int i;
                if (int.TryParse(ex.Message, out i))
                {
                    errcode = i;
                    return null;
                }
                else
                {
                    this.WriteLine($"※VDFControl.CheckinUploadfile(..)にて例外 {ex.Message}");
                    errcode = 0;
                }
                return null;
            }

        }

        /// <summary>
        /// ■Vaultファイル名変更。ファイル参照を考慮します 変更対象はダウンロード済みかつチェックアウトしていること
        /// </summary>
        /// <param name="fileIter">名前変更対象の FileIteration ｵﾌﾞｼﾞｪｸﾄ</param>
        /// <param name="newFilePath">名前変更後のローカルファイルパス</param>
        /// <param name="newFileName">nullの場合は newFilePath から 拡張子含むファイル名が使用されます</param>
        /// <param name="comment">チェックイン時のコメント</param>
        /// <returns>成功した場合は入力した FileIteration を返す。失敗した場合は null</returns>
        public VDFVCE.FileIteration RenameCheckInFile(VDFVCE.FileIteration fileIter, string newFilePath, string newFileName = null, string comment = null, SasaLibDelegateWriteLine methodWriteLine = null)
        {
            if (methodWriteLine == null) methodWriteLine = this.WriteLine;

            //  https://forums.autodesk.com/t5/vault-customization/check-in-files-by-api/td-p/3134336
            try
            {
                string orgLocalFullFilename;

                if (fileIter == null)
                    throw new Exception("fileIter が null");

                if (newFilePath == null)
                    throw new Exception("newFilePath が null");

                if (fileIter.IsCheckedOut == false)
                    throw new Exception("チェックアウトされていません");

                if (string.IsNullOrWhiteSpace(fileIter.CheckedOutSpec))
                    throw new Exception("CheckedOutSpecが空文字かnullです");

                if (string.IsNullOrWhiteSpace(newFileName))
                    newFileName = System.IO.Path.GetFileName(newFilePath);

                orgLocalFullFilename = fileIter.CheckedOutSpec;

                if (System.IO.File.Exists(orgLocalFullFilename) == false)
                {
                    VDFV.Settings.AcquireFilesSettings settings = new VDFV.Settings.AcquireFilesSettings(VaultConnection);
                    settings.AddFileToAcquire(fileIter, VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download);

                    VDFVR.AcquireFilesResults results = VaultConnection.FileManager.AcquireFiles(settings);

                }

                if (System.IO.File.Exists(orgLocalFullFilename) == false)
                    throw new Exception($"\"{orgLocalFullFilename}\" が存在しません");


                bool copyResult = FileFolder.CopyFile(orgLocalFullFilename, newFilePath);
                if (copyResult == false)
                    throw new Exception($"\"{orgLocalFullFilename}\" を \"{newFilePath}\" へのコピーに失敗しました");

                VDFVCE.FileIteration resultFileIteration;
                if (fileIter.IsCheckedOut == true)
                {
                    /// アセンブリファイルの場合 参照先情報を取得します。チェックイン時にそのまま書き戻します。
                    ACW.FileAssocParam[] resutlassoc = GetAssocParamsFromEntityIterationId(fileIter.EntityIterationId).ToArray();

                    VDF.Currency.FilePathAbsolute filePath;
                    if (string.IsNullOrWhiteSpace(newFilePath))
                    {
                        filePath = null;
                    }
                    else
                    {
                        filePath = new VDF.Currency.FilePathAbsolute(newFilePath);
                    }

                    resultFileIteration = VaultConnection.FileManager.CheckinFile(file: fileIter, comment: comment, keepCheckedOut: false, associations: resutlassoc, bom: null, copyBom: true, newFileName: newFileName, classification: fileIter.FileClassification, hidden: false, filePath: filePath);

                    if (resultFileIteration.IsCheckedOut == true)
                        throw new Exception($"\"{fileIter.EntityName}\" の チェックインに失敗しました");

                    this.WriteLine($"チェックイン成功: resultFileIteration.VersionNumbe:{resultFileIteration.VersionNumber}");
                    this.WriteLine($"チェックイン成功: resultFileIteration.CreateUserName:{resultFileIteration.CreateUserName}");
                    this.WriteLine($"チェックイン成功: resultFileIteration.CheckInDate:{resultFileIteration.CheckInDate}");


                }
                else
                {
                    this.WriteLine($"VDFVCE.FileIteration RenameCheckInFile(..)  {fileIter.EntityName} チェックアウトされていないファイルが指定されました ");
                    resultFileIteration = null;
                }

                return resultFileIteration;
            }
            catch (Exception ex)
            {
                this.WriteLine($"VDFVCE.FileIteration RenameCheckInFile(..) 例外発生。{ex.Message}");
                return null;
            }
        }

        #endregion チェックインまたはアップロード

        // -- //

        /// <summary>
        /// ■Vaultフォルダー間でファイルを移動
        /// このメソッドで移動された被参照ファイルはVaultで参照先が追跡されます。
        /// </summary>
        /// <param name="vaultFolderPath"></param>
        /// <param name="vaultFileName"></param>
        /// <param name="DistVaultFolderPath"></param>
        public bool MoveFile(string vaultFolderPath, string vaultFileName, string DistVaultFolderPath)
        {
            try
            {
                ACW.SrchCond[] _srchCond1 = MakeSrchCondsSysName("FolderPath", vaultFolderPath, "Name", vaultFileName);
                VDFVCE.FileIteration oSource = FindFileFirst(_srchCond1);

                if (oSource == null)
                    return false;

                ACW.DocumentService documentService = VaultConnection.WebServiceManager.DocumentService;

                ACW.Folder oFolder = documentService.GetFolderByPath(DistVaultFolderPath);


                if (oSource != null && oFolder != null)
                {
                    try
                    {
                        documentService.MoveFile(oSource.EntityMasterId, oSource.FolderId, oFolder.Id);

                        return true;
                    }
                    catch (Exception ex)
                    {
                        this.WriteLine($"※MoveFile(..)で例外 {ex.Message}");
                        return false;
                    }
                }
                else
                {
                    if (oSource == null)
                        this.WriteLine($"※MoveFile(..) 移動元 {vaultFolderPath}/{vaultFileName} が見つかりません");
                    if (oFolder == null)
                        this.WriteLine($"※MoveFile(..) 移動先フォルダ  {DistVaultFolderPath} が見つかりません");

                    return false;
                }

            }
            catch (Exception ex)
            {
                this.WriteLine($"※MoveFile(..) 例外発生  {ex.Message}");
                return false;
            }
        }

    }
}