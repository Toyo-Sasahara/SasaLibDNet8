using SasaLib;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using VDF = Autodesk.DataManagement.Client.Framework;
using ACW = Autodesk.Connectivity.WebServices;

namespace SaSaLibTestApp
{
    public partial class Tab07_AutodeskVault_UserControl : UserControl
    {
        Form1 mainForm;


        public Tab07_AutodeskVault_UserControl(Form1 form)
        {
            this.mainForm = form;
            InitializeComponent();
            SetToControls();

            var eventHandler = new System.EventHandler(TabControl1Changed);


        }

        private void TabControl1_VisibleChanged(object sender, EventArgs e)
        {
            LogWindow_textBox.AppendText("TabControl1_VisibleChanged(..)実行・・・\r\n");

            SetToControls();
        }

        bool flag = false;

        /// <summary>
        /// コントロールに変化があったなら
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TabControl1Changed(object sender, EventArgs e)
        {
        }

        private void TabControl1_Load(object sender, EventArgs e)
        {
        }

        public void SetToControls()
        {
        }

        /// <summary>
        /// メインスレッド外からの呼び出しも考慮したﾛｸﾞｳｨﾝﾄﾞｳ変更メソッド
        /// </summary>
        /// <param name="msg"></param>
        public void LogWindowWriteLine(string msg)
        {
            try
            {
                if (this.InvokeRequired)
                {//https://qiita.com/taiyakisun/items/15b57df979eae7562aef
                    this.Invoke(new Action<string>(this.UpdateText), msg);
                }
                else
                {
                    UpdateText($"{msg}\n");
                }
            }
            catch (Exception ex)
            {
                this.LogWindow_textBox.AppendText($"例外検知{ex.Message}\r\n");

            }
        }
        private void UpdateText(string msg)
        {
            this.LogWindow_textBox.AppendText($"{msg}\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VDF.Vault.Forms.Settings.LoginSettings settings = new VDF.Vault.Forms.Settings.LoginSettings();


            VDF.Vault.Currency.Connections.Connection connection = VDF.Vault.Forms.Library.Login(settings);
            try
            {
                // Start at the root Folder.
                VDF.Vault.Currency.Entities.Folder root = connection.FolderManager.RootFolder;

                this.m_listBox.Items.Clear();

                // Call a function which prints all files in a Folder and sub-Folders.
                PrintFilesInFolder(root, connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                return;
            }

            VDF.Vault.Library.ConnectionManager.LogOut(connection);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ListAllFiles();

        }

        /// <summary>
        /// This function lists all the files in the Vault and displays them in the form's ListBox.
        /// </summary>
        public void ListAllFiles()
        {
            // For demonstration purposes, the information is hard-coded.
            VDF.Vault.Results.LogInResult results = VDF.Vault.Library.ConnectionManager.LogIn(
                "localhost", "Vault", "Administrator", "", VDF.Vault.Currency.Connections.AuthenticationFlags.Standard, null
                );

            if (!results.Success)
                return;

            VDF.Vault.Currency.Connections.Connection connection = results.Connection;

            try
            {
                // Start at the root Folder.
                VDF.Vault.Currency.Entities.Folder root = connection.FolderManager.RootFolder;

                this.m_listBox.Items.Clear();

                // Call a function which prints all files in a Folder and sub-Folders.
                PrintFilesInFolder(root, connection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
                return;
            }

            VDF.Vault.Library.ConnectionManager.LogOut(connection);
        }
        /// <summary>
        /// Prints all the files in the current Folder and any sub Folders.
        /// </summary>
        /// <param name="parentFolder">The folder we want to print.</param>
        /// <param name="connection">The manager object for making Vault Server calls.</param>
        private void PrintFilesInFolder(VDF.Vault.Currency.Entities.Folder parentFolder, VDF.Vault.Currency.Connections.Connection connection)
        {
            // get all the Files in the current Folder.
            ACW.File[] childFiles = connection.WebServiceManager.DocumentService.GetLatestFilesByFolderId(parentFolder.Id, false);

            // print out any Files we find.
            if (childFiles != null && childFiles.Any())
            {
                foreach (ACW.File file in childFiles)
                {
                    // print the full path, which is Folder name + File name.
                    this.m_listBox.Items.Add(parentFolder.FullName + "/" + file.Name);
                }
            }

            // check for any sub Folders.
            IEnumerable<VDF.Vault.Currency.Entities.Folder> folders = connection.FolderManager.GetChildFolders(parentFolder, false, false);
            if (folders != null && folders.Any())
            {
                foreach (VDF.Vault.Currency.Entities.Folder folder in folders)
                {
                    // recursively print the files in each sub-Folder
                    PrintFilesInFolder(folder, connection);
                }
            }
        }

    }
}
