
// 参照アセンブリ
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.Connectivity.WebServices.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.Connectivity.WebServices.WCF.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.DataManagement.Client.Framework.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.DataManagement.Client.Framework.Forms.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.DataManagement.Client.Framework.Vault.dll
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\Autodesk.DataManagement.Client.Framework.Vault.Forms.dll

// C:\Program Files\Autodesk\Inventor 2020\Bin\Public Assemblies\Autodesk.Inventor.Interop.dll

// C:\Program Files\Autodesk\Inventor 20XX\Bin\Connectivity.InventorAddin.EdmAddin.dll

// AdskLicensingSDK_2.dll をアセンブリと同じフォルダにコピーすること！！
// I tested this morning, the AdskLicensingSDK_2.dll must be present in my output directory. Like this, the export works perfectly.
// https://forums.autodesk.com/t5/vault-customization/read-only-license-exception-to-get-user-and-group-from-api-in/td-p/8803658
// C:\Program Files\Autodesk\Autodesk Vault 2020 SDK\bin\x64\AdskLicensingSDK_2.dll

// https://justonesandzeros.typepad.com/blog/vault/page/8/

using System;
using ACW = Autodesk.Connectivity.WebServices;

using VDF = Autodesk.DataManagement.Client.Framework;
using VDFV = Autodesk.DataManagement.Client.Framework.Vault;
using VDFVCE = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using VDFVFS = Autodesk.DataManagement.Client.Framework.Vault.Forms.Settings;

namespace SasaLib.AutodeskVault
{
    /// <summary>
    public class VDFVaultForm
    {
        Action<string> WriteLine = Console.WriteLine;

        public VDF.Vault.Currency.Connections.Connection vaultConnection { get; private set; }

        public ACW.DocumentService acwDocumentService {  get; private set; }

        private static string previusSelectVaultFolder;
        private static string previusSelectFullName;

        public VDFVaultForm(VDF.Vault.Currency.Connections.Connection connection)
        {
            vaultConnection = connection;
            acwDocumentService = vaultConnection.WebServiceManager.DocumentService;
        }

        /// <summary>
        /// ■Vault ファイル選択フォームの表示
        /// </summary>
        /// <param name="vaultFolder"></param>
        /// <param name="Caption"></param>
        /// <param name="InitialSelectionText"></param>
        /// <param name="actionButtonLabel"></param>
        /// <returns></returns>
        public VDFV.Forms.Results.SelectEntityResults SelectEntityDialog(string vaultFolder = null , string Caption = "Vaultから選択", string InitialSelectionText = "", string ActionButtonLabel = "選択")
        {
            if (vaultConnection != null && vaultConnection.IsConnected == true)
            {
                try
                {
                    if (vaultFolder == null)
                    {
                        if (previusSelectVaultFolder == null)
                            vaultFolder = "$";
                        else
                            vaultFolder = previusSelectVaultFolder;
                    }

                    ACW.Folder acwFolder = acwDocumentService.GetFolderByPath(vaultFolder);
                    VDFVCE.Folder folder = new VDFVCE.Folder(vaultConnection, acwFolder);

                    VDFVFS.SelectEntitySettings entitysettings  = new VDFVFS.SelectEntitySettings();

                    entitysettings.ConfigureActionButtons(ActionButtonLabel, options:null, defaultOption:null, persistChosenOption: false);
                    entitysettings.ShowFolderView = true;
                    entitysettings.DialogCaption = Caption;
                    entitysettings.InitialBrowseLocation = folder;
                    entitysettings.ActionButtonNavigatesContainers = true;
                    entitysettings.InitialSelectionText = InitialSelectionText;

                    VDFV.Forms.Results.SelectEntityResults result = VDFV.Forms.Library.SelectEntity(vaultConnection, entitysettings);

                    previusSelectVaultFolder = ((Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities.Folder)result.ParentEntity).FolderPath;

                    previusSelectFullName = ((Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities.Folder)result.ParentEntity).FullName;
                    return result;

                }
                catch (Exception ex)
                {
                    return null;
                }
                

            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// ■Vault フォルダ選択フォームの表示
        /// </summary>
        /// <param name="vaultFolder"></param>
        /// <returns></returns>
        public VDFV.Forms.Results.SelectVaultFolderResults FolderSelect(string vaultFolder = "$/")
        {
            if (vaultConnection !=null && vaultConnection.IsConnected == true)
            {
                VDFV.Forms.Settings.SelectVaultFolderSettings settings
                    = new VDF.Vault.Forms.Settings.SelectVaultFolderSettings(vaultConnection);
                settings.InitialSelectedFolderPath = vaultFolder;

                VDFV.Forms.Results.SelectVaultFolderResults result = VDFV.Forms.Library.SelectVaultFolder(settings);
                return result;

            }
            else
            {
                return null;
            }
        }

    }
}