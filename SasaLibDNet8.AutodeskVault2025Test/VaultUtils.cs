using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Inventor;
using ACW = Autodesk.Connectivity.WebServices;
using VDF = Autodesk.DataManagement.Client.Framework;
using VDFV = Autodesk.DataManagement.Client.Framework.Vault;
using VDFVCP = Autodesk.DataManagement.Client.Framework.Vault.Currency.Properties;
using VDFVCE = Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;
using System.Text.RegularExpressions;
//using Connectivity.InventorAddin.EdmAddin;
using System.Windows.Forms;
using Autodesk.DataManagement.Client.Framework.Vault.Currency.Entities;

namespace SasaLib.AutodeskVault
{

    public static class VaultUtils
    {
        public struct Folder
        {
            public int depth;
            public string FolderPath;
        }

        public static bool SweepLocalFolder(string RootFolder,out string[] filefolders)
        {
            filefolders = System.IO.Directory.GetDirectories(@"D:\MainVault", "*", System.IO.SearchOption.AllDirectories);

            return true;
        }

        /// <summary>
        /// ■ローカル作業フォルダの内容を再帰調査
        /// </summary>
        /// <param name="RootFolder"></param>
        /// <param name="OrderByFolders"></param>
        /// <returns></returns>
        public static bool SweepLocalFolder(string RootFolder, out List<Folder> OrderByFolders)
        {

            var filefolders = System.IO.Directory.GetDirectories(@"D:\MainVault", "*", System.IO.SearchOption.AllDirectories);

            List<Folder>  Folders = new List<Folder>();

            foreach (var a in filefolders)
            {
                string[] b = a.Split(System.IO.Path.DirectorySeparatorChar);
                int FolderDepth = b.Length;

                Folder f = new Folder { depth = FolderDepth, FolderPath = a };

                Folders.Add(f);
            }

            OrderByFolders = new List<Folder>();

            IOrderedEnumerable<Folder> OrderbyDesc = Folders.OrderByDescending(x => x.depth);

            OrderByFolders = OrderbyDesc.ToList();

            return true;
        }
    }
}
