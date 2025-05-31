using Autodesk.Connectivity.WebServices;
//using Autodesk.DataManagement.Client.Framework.Vault.Settings;
//using DevExpress.Services.Internal;
//using DevExpress.XtraEditors;
//using DevExpress.XtraLayout.Customization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACW = Autodesk.Connectivity.WebServices;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace SasaLib.AutodeskVault
{
    class sampleCode
    {
        //検索のサンプル

        public static void search(VDF.Vault.Currency.Connections.Connection mVaultConnection)
        {

            // C#
            PropDef[] filePropDefs = mVaultConnection.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            PropDef checkedOutPropDef = filePropDefs.Single(n => n.SysName == "CheckoutUserName");
            PropDef projectPropDef = filePropDefs.Single(n => n.SysName == "Project");



            SrchCond isCheckedOut = new SrchCond()
            {
                PropDefId = checkedOutPropDef.Id,
                PropTyp = PropertySearchType.SingleProperty,
                SrchOper = 5, // is not empty
                SrchRule = SearchRuleType.Must,
            };



            SrchCond padlockProject = new SrchCond()
            {
                PropDefId = projectPropDef.Id,
                PropTyp = PropertySearchType.SingleProperty,
                SrchOper = 3, // is equal
                SrchRule = SearchRuleType.Must,
                SrchTxt = "Padlock"
            };



            string bookmark = string.Empty;
            SrchStatus status = null;
            List<Autodesk.Connectivity.WebServices.File> totalResults = new List<Autodesk.Connectivity.WebServices.File>();

            while (status == null || totalResults.Count < status.TotalHits)

            {

                Autodesk.Connectivity.WebServices.File[] results = mVaultConnection.WebServiceManager.DocumentService.FindFilesBySearchConditions(

                    new SrchCond[] { isCheckedOut, padlockProject },

                    null, null, false, true, ref bookmark, out status);



                if (results != null)

                    totalResults.AddRange(results);

                else

                    break;

            }

            // total results now has the results
        }



    }


    //public class vaultsample`
    //{
    //    VDF.Vault.Currency.Connections.Connection _connection;

    //    public vaultsample()
    //    {
    //        Folder[]

    //        VDF.Vault.Results.AcquireFilesResults result = null;
    //        ACW.File checkedOutNewFile = CheckOut(folders[0], selectedFile, ref result);

    //        if (checkedOutNewFile != null)
    //        {
    //            CheckIn(folders[0], checkedOutNewFile, newname, Coment, result);
    //        }

    //    }

    //    public ACW.File CheckOut(Folder folder, ACW.File file, ref VDF.Vault.Results.AcquireFilesResults result)
    //    {
    //        if (folder == null)
    //            folder = _connection.WebServiceManager.DocumentService.GetFolderById(file.FolderId);
    //        string localPath = _connection.WorkingFoldersManager.GetWorkingFolder(folder.FullName).FullPath;
    //        if (!System.IO.Directory.Exists(localPath))
    //            System.IO.Directory.CreateDirectory(localPath);

    //        AcquireFilesSettings settings = new VDF.Vault.Settings.AcquireFilesSettings(_connection);
    //        settings.OptionsRelationshipGathering.FileRelationshipSettings.VersionGatheringOption = Autodesk.DataManagement.Client.Framework.Vault.Currency.VersionGatheringOption.Latest;
    //        settings.LocalPath = new VDF.Currency.FolderPathAbsolute(localPath);

    //        settings.AddFileToAcquire(
    //            new VDF.Vault.Currency.Entities.FileIteration(_connection, file),
    //            VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Download | VDF.Vault.Settings.AcquireFilesSettings.AcquisitionOption.Checkout);
    //        result = _connection.FileManager.AcquireFiles(settings);

    //        foreach (var r in result.FileResults)
    //        {

    //            return m_serviceManagervaultConnection.WebServiceManager.DocumentService.GetFileById(fileIter.EntityIterationId);.DocumentService.GetFileById(r.File.EntityIterationId);

    //        }
    //        return null;
    //    }
    //    public ACW.File CheckIn(Folder folder, ACW.File file, string newname, string comm, VDF.Vault.Results.AcquireFilesResults result)
    //    {

    //        if (folder == null)
    //            folder = _connection.WebServiceManager.DocumentService.GetFolderById(file.FolderId);

    //        string localPath = _connection.WorkingFoldersManager.GetWorkingFolder(folder.FullName).FullPath;
    //        string filePath = System.IO.Path.Combine(localPath, file.Name);

    //        FileAssocParam[] assparamsarr = null;
    //        if (result != null)
    //        {
    //            FileRelationshipGatheringSettings settings = new FileRelationshipGatheringSettings();
    //            settings.IncludeChildren = true;
    //            var fassoc = _connection.FileManager.GetFileAssociationLites(new long[] { result.FileResults.First().NewFileIteration.EntityIterationId }, settings);

    //            List<FileAssocParam> assparams = new List<FileAssocParam>();
    //            foreach (var * *** in fassoc)
    //            {
    //                FileAssocParam param = new FileAssocParam();
    //                param.CldFileId = ****.CldFileId;
    //                param.RefId = ****.RefId;
    //                param.Source = ****.Source;
    //                param.Typ = ****.Typ;
    //                param.ExpectedVaultPath = ****.ExpectedVaultPath;
    //                assparams.Add(param);
    //            }
    //            assparamsarr = assparams.ToArray();
    //        }
    //        VDF.Vault.Currency.Entities.FileIteration ff = _connection.FileManager.CheckinFile(
    //           new VDF.Vault.Currency.Entities.FileIteration(_connection, file),
    //           comm,
    //           false,
    //           assparamsarr,
    //           null,
    //           true,
    //           newname,
    //           file.FileClass,
    //        false,
    //           new VDF.Currency.FilePathAbsolute(filePath));


    //        return m_serviceManager.DocumentService.GetFileById(ff.EntityIterationId);

    //    }

    //}
}
