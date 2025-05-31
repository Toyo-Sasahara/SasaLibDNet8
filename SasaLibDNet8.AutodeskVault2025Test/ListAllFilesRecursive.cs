using Autodesk.Connectivity.WebServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ACW = Autodesk.Connectivity.WebServices;
using VDF = Autodesk.DataManagement.Client.Framework;

namespace SasaLib.AutodeskVault
{
    public class ListAllFilesRecursive
    {
        /// <summary>
        /// 
        /// </summary>
        static List<string> vaultAllFileList = new List<string>();

        VDF.Vault.Currency.Connections.Connection vaultConnection;

        /// <summary>
        /// 
        /// </summary>
        public  ListAllFilesRecursive(VDF.Vault.Currency.Connections.Connection connection)
        {
            vaultConnection = connection;

            try
            {
                // ルートフォルダからスタートします。
                VDF.Vault.Currency.Entities.Folder root = vaultConnection.FolderManager.RootFolder;

                vaultAllFileList.Clear();

                // フォルダとサブフォルダ内のすべてのファイルを印刷する関数を呼び出します。
                PrintFilesInFolder(root);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外発生 {ex.Message}");
            }
        }

        public List<string> GetFiles()
        {
            return vaultAllFileList;
        }

        /// <summary>
        /// 現在のフォルダ内のすべてのファイルとサブフォルダを印刷します。自身で再呼び出しされます
        /// File[] GetLatestFilesByFolderId(long folderId, bool includeHidden);
        /// </summary>
        /// <param name="parentFolder">印刷したいフォルダです。</param>
        /// <param name="connection">Vault Server の呼び出しを行うためのマネージャ オブジェクト。</param>
        private void PrintFilesInFolder(VDF.Vault.Currency.Entities.Folder parentFolder)
        {
            // 現在のフォルダ内のすべてのファイルを取得します。
            ACW.File[] childACWFiles = vaultConnection.WebServiceManager.DocumentService.GetLatestFilesByFolderId(parentFolder.Id, false);

            // 見つけたファイルをプリントアウトします。
            if (childACWFiles != null && childACWFiles.Any())
            {
                foreach (ACW.File file in childACWFiles)
                {
                    // フォルダ名＋ファイル名のフルパスを表示します。
                    vaultAllFileList.Add(parentFolder.FullName + "/" + file.Name);
                    Console.WriteLine($"{parentFolder.FullName + "/" + file.Name}");
                }
            }

            // サブフォルダがないかチェックします。
            IEnumerable<VDF.Vault.Currency.Entities.Folder> folders = vaultConnection.FolderManager.GetChildFolders(parentFolder, false, false);
            if (folders != null && folders.Any())
            {
                foreach (VDF.Vault.Currency.Entities.Folder folder in folders)
                {
                    // 各サブフォルダ内のファイルを再帰的に印刷します。
                    PrintFilesInFolder(folder);
                }
            }
        }

    }
}
