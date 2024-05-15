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
using System.Runtime.Versioning;

//using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Inventor コントロール用共有ソース
/// </summary>
namespace SasaLib.InventorAPI
{
    [SupportedOSPlatform("windows")]

    public static partial class InventorControl
    {
        /// <summary>
        /// ■プロジェクトファイルフルパスの一覧を返す
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static string[] GetProjectFullFileNames(Inventor.Application oApp)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            List<string> projects = new List<string>();

            foreach (DesignProject dp in oProjectMgr.DesignProjects)
            {
                projects.Add(dp.FullFileName);
            }
            string[] array;
            array = projects.ToArray();

            return array;
        }

        /// <summary>
        /// ■Inventorプロジェクトをファイルから追加し、可能ならアクティブプロジェクトに変更します。
        /// </summary>
        /// <param name="oApp">Inventorアプリケーションオブジェクト</param>
        /// <param name="IPJfullfileName">*.IPJファイルフルパス</param>
        public static DesignProject SetActiveProject(Inventor.Application oApp, string IPJfullfileName)
        {
            if (oApp.Documents.LoadedCount == 0)
            {
                Inventor.DesignProjectManager oDesignProjectMgr;
                oDesignProjectMgr = oApp.DesignProjectManager;

                Console.WriteLine($"現在のプロジェクト名：{oDesignProjectMgr.ActiveDesignProject.Name}");
                Console.WriteLine($"現在のプロジェクトファイル：{oDesignProjectMgr.ActiveDesignProject.FullFileName}");

                // DesignProjects.AddExisting Method
                // プロジェクト ファイルのリストに既存のプロジェクト ファイルを追加するメソッドです。
                // これは、ディスク上の特定の ipj ファイルを参照し、[プロジェクト エディタ]ダイアログ ボックスで選択することと同じです。
                // デザイン プロジェクトがコレクション内に既に存在している場合、対応する DesignProject オブジェクトが返されます。
                Inventor.DesignProject anser = oDesignProjectMgr.DesignProjects.AddExisting(IPJfullfileName);

                try
                {
                    if (oApp.Documents.Count == 0)
                    {
                        anser.Activate();

                        DebugConsole.WriteLine($"新しいプロジェクト名：{anser.Name}");
                        DebugConsole.WriteLine($"新しいプロジェクトファイル：{anser.FullFileName}");
                        return anser;
                    }
                    else
                    {
                        DebugConsole.WriteLine($"ファイルが {oApp.Documents.Count} コ開かれているのでアクティブプロジェクトを変更できません");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"プロジェクトを {anser.Name} 変更できませんでした.{ex.Message}");
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        #region GetProjectName

        /// <summary>
        /// ■指定したIPJファイルのプロジェクト名を取得する
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="ProjectFileFullPath"></param>
        /// <returns></returns>
        public static string GetProjectName(Inventor.Application oApp, string ProjectFileFullPath)
        {
            Inventor.DesignProjectManager oDesignProjectMgr;
            oDesignProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oDesignProjectMgr.DesignProjects.ItemByName[ProjectFileFullPath];

            string ProjectName = oProject.Name;

            return ProjectName;
        }

        /// <summary>
        /// ■アクティブプロジェクト名のみを取得
        /// </summary>
        /// <returns></returns>
        public static string GetActiveProjectName(Inventor.Application oApp, out string ProjectFullFileName)
        {
            Inventor.DesignProjectManager oDesignProjectMgr;
            oDesignProjectMgr = oApp.DesignProjectManager;

            var anser = oDesignProjectMgr.ActiveDesignProject.Name;
            ProjectFullFileName = oDesignProjectMgr.ActiveDesignProject.FullFileName;
            return anser;
        }

        /// <summary>
        /// ■アクティブプロジェクト名を取得
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static string GetActiveProjectFullFileName(Inventor.Application oApp)
        {
            string ProjectFullFileName;
            GetActiveProjectName(oApp, out ProjectFullFileName);
            return ProjectFullFileName;
        }

        #endregion GetProjectName

        #region GetProjectFolder

        /// <summary>
        /// ■アクティブプロジェクトファイルが存在するフォルダを返す
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static string GetActiveProjectFolder(Inventor.Application oApp)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oProjectMgr.ActiveDesignProject;

            string ProjectFullFileName = oProject.FullFileName;

            return System.IO.Path.GetDirectoryName(ProjectFullFileName);

        }

        /// <summary>
        /// ■プロジェクトファイルの保存フォルダを返す(本体・シール予定)
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="ProjectFileFullPath"></param>
        /// <param name="ProjectFolder"></param>
        /// <returns></returns>
        static void GetProjectFolder(Inventor.Application oApp, string ProjectFileFullPath, out string ProjectFolder)
        {
            Inventor.DesignProjectManager oDesignProjectMgr;
            oDesignProjectMgr = oApp.DesignProjectManager;
            try
            {
                Inventor.DesignProject oProject;
                oProject = oDesignProjectMgr.DesignProjects.ItemByName[ProjectFileFullPath];

                string ProjectFullFileName = oProject.FullFileName;

                ProjectFolder = System.IO.Path.GetDirectoryName(ProjectFullFileName);

            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"GetProjectFolder(...)にて例外 {ex.Message}");

                ProjectFolder = null;
            }

        }

        /// <summary>
        ///  ■プロジェクトファイルの保存フォルダを返す
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="ProjectFileFullPath"></param>
        /// <returns></returns>
        public static string GetProjectFolder(Inventor.Application oApp, string ProjectFileFullPath)
        {
            string ProjectFolder;
            GetProjectFolder(oApp, ProjectFileFullPath, out ProjectFolder);
            return ProjectFolder;
        }

        #endregion GetProjectFolder

        #region GetProjectWorkSpaceFolder

        /// <summary>
        /// ■指定したプロジェクトファイル名に定義されている作業スペースへのフルパスを得る(本体・シール予定)
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="ProjectFileFullPath"></param>
        /// <param name="WorkspacePath"></param>
        /// <returns></returns>
        public static void GetProjectWorkSpaceFolder(Inventor.Application oApp, string ProjectFileFullPath, out string WorkspacePath)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            try
            {
                Inventor.DesignProject oProject;
                oProject = oProjectMgr.DesignProjects.ItemByName[ProjectFileFullPath];
                if (oProject != null)
                {
                    WorkspacePath = oProject.WorkspacePath;
                }
                else
                {
                    WorkspacePath = null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"InventorControl.GetProjectWorkSpaceFolder()にて例外 {ex.Message}");
                WorkspacePath = null;
                return;
            }
        }

        /// <summary>
        /// ■指定したプロジェクトファイル名に定義されている作業スペースへのフルパスを得る
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="ProjectFileFullPath"></param>
        /// <returns></returns>
        public static string GetProjectWorkSpaceFolder(Inventor.Application oApp, string ProjectFileFullPath)
        {
            string WorkspacePath;
            GetProjectWorkSpaceFolder(oApp, ProjectFileFullPath, out WorkspacePath);
            return WorkspacePath;
        }

        /// <summary>
        /// ■アクティブプロジェクトの業スペースへのフルパスを取得する(本体・シール予定)
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static string GetActiveProjectWorkSpaceFolder(Inventor.Application oApp, out string WorkspacePath)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oProjectMgr.ActiveDesignProject;

            WorkspacePath = oProject.WorkspacePath;
            return WorkspacePath;
        }
        /// <summary>
        /// ■アクティブプロジェクトの業スペースへのフルパスを取得する。
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static string GetActiveProjectWorkSpaceFolder(Inventor.Application oApp)
        {
            string WorkspacePath;
            GetActiveProjectWorkSpaceFolder(oApp, out WorkspacePath);
            return WorkspacePath;
        }

        #endregion GetProjectWorkSpaceFolder

        /// <summary>
        /// ■アクティブプロジェクトのテンプレートファイルパスを取得する
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static string GetActiveProjectTemplateFolder(Inventor.Application oApp)
        {
            Inventor.DesignProjectManager oDesignProjectMgr;
            oDesignProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oDesignProjectMgr.ActiveDesignProject;

            string TemplatesPath = oProject.TemplatesPath;

            return TemplatesPath;

        }

        /// <summary>
        /// ■アクティブプロジェクトのライブラリ情報を取得
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static string[] GetActiveProjectLibraryPaths(Inventor.Application oApp)
        {
            string[] Paths;
            _GetActiveProjectLibraryName(oApp, out Paths);
            return Paths;
        }

        /// <summary>
        /// ■現在のﾌﾟﾛｼﾞｪｸﾄで定義されたコンテンツセンタ―パスを取得
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static string GetActiveProjectContentCenterPath(Inventor.Application oApp)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oProjectMgr.ActiveDesignProject;

            return oProject.ContentCenterPath;
        }

        /// <summary>
        /// ■アクティブプロジェクトのライブラリ情報を取得（本体・シール予定）
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="Paths">ライブラリパスの配列</param>
        /// <returns>ライブラリ名の配列</returns>
        private static string[] _GetActiveProjectLibraryName(Inventor.Application oApp, out string[] Paths)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oProjectMgr.ActiveDesignProject;

            string ProjectFolder = System.IO.Path.GetDirectoryName(oProject.FullFileName);


            Inventor.ProjectPaths oLibraryPaths;
            oLibraryPaths = oProject.LibraryPaths;

            List<string> LibraryNames = new List<string>();
            List<string> LibraryPaths = new List<string>();


            foreach (Inventor.ProjectPath oLibraryPath in oLibraryPaths)
            {
                LibraryNames.Add(oLibraryPath.Name);

                string LibraryPath = oLibraryPath.Path.Replace(@".\", "");

                LibraryPaths.Add(System.IO.Path.Combine(ProjectFolder, LibraryPath));
            }

            Paths = LibraryPaths.ToArray();

            return LibraryNames.ToArray();
        }


        /// <summary>
        /// ■ライブラリ名の実際のパスを返す
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="ProjectFileFullPath">プロジェクトファイル</param>
        /// <param name="LibrayName">ライブラリ名</param>
        /// <returns></returns>
        public static string GetProjectLibraryPath(Inventor.Application oApp, string ProjectFileFullPath, string LibrayName)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;

            try
            {

                oProject = oProjectMgr.DesignProjects.ItemByName[ProjectFileFullPath];

                string ProjectFolder = System.IO.Path.GetDirectoryName(oProject.FullFileName);


                Inventor.ProjectPaths oLibraryPaths;
                oLibraryPaths = oProject.LibraryPaths;

                try
                {

                    ProjectPath projecPath = oLibraryPaths[LibrayName];

                    string LibraryPath = projecPath.Path.Replace(@".\", "");

                    string LibPath = (System.IO.Path.Combine(ProjectFolder, LibraryPath));

                    return LibPath;
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"GetProjectLibraryPath(...)にて例外 {ex.Message}");

                    return null;
                }

            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"GetProjectLibraryPath(...)にて例外 {ex.Message}");

                return null;
            }

        }

        public static string GetActiveProjectLibraryPath(Inventor.Application oApp, string LibrayName)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oProjectMgr.ActiveDesignProject;
            string ProjectFolder = System.IO.Path.GetDirectoryName(oProject.FullFileName);


            Inventor.ProjectPaths oLibraryPaths;
            oLibraryPaths = oProject.LibraryPaths;

            ProjectPath projecPath = oLibraryPaths[LibrayName];

            string LibraryPath = projecPath.Path.Replace(@".\", "");

            string LibPath = (System.IO.Path.Combine(ProjectFolder, LibraryPath));


            return LibPath;
        }

        /// <summary>
        /// ■アクティブプロジェクトがVaULTか？
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static bool IsActiveProjectTypeVault(Inventor.Application oApp)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oProjectMgr.ActiveDesignProject;

            if (oProject.ProjectType == MultiUserModeEnum.kVaultMode)
                return true;
            else
                return false;
        }

        /// <summary>
        /// ■アクティブプロジェクトがシングルユーザーの場合true
        /// </summary>
        /// <param name="oApp"></param>
        /// <returns></returns>
        public static bool IsActiveProjectTypeSingleUser(Inventor.Application oApp)
        {
            Inventor.DesignProjectManager oProjectMgr;
            oProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oProject;
            oProject = oProjectMgr.ActiveDesignProject;

            if (oProject.ProjectType == MultiUserModeEnum.kSingleUserMode)
                return true;
            else
                return false;
        }

        /// <summary>
        ///  ■アクティブプロジェクトのライブラリフォルダが無い場合に、作成する
        /// </summary>
        /// <param name="oApp"></param>
        public static void RemakeLibraryFolder(Inventor.Application oApp, out string[] Paths, bool automode = false)
        {
            string ProjectName;
            InventorControl.GetActiveProjectName(oApp, out ProjectName);

            string[] LibaryPaths;
            InventorControl._GetActiveProjectLibraryName(oApp, out LibaryPaths);
            Paths = LibaryPaths;
            foreach (var path in LibaryPaths)
            {
                if (FileFolder.DirExists(path))
                {
                }
                else
                {
                    FileFolder.MakeDirectory(path);
                    if (automode == false)
                    {
                        MessageBox.Show($"ﾌﾟﾛｼﾞｪｸﾄﾌｧｲﾙ{ProjectName}に設定されているﾗｲﾌﾞﾗﾘﾊﾟｽ{path}が見つかりません。再作成しました", "■東陽ﾂｰﾙ 情報", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }

            string WorkSpacePath;
            InventorControl.GetActiveProjectWorkSpaceFolder(oApp, out WorkSpacePath);
            if (FileFolder.DirExists(WorkSpacePath))
            {
            }
            else
            {
                FileFolder.MakeDirectory(WorkSpacePath);
                if (automode == false)
                {
                    MessageBox.Show($"ﾌﾟﾛｼﾞｪｸﾄﾌｧｲﾙ{ProjectName}に設定されている作業ｽﾍﾟｰｽ{WorkSpacePath}が見つかりません。再作成しました", "■東陽ﾂｰﾙ 情報", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }



        }

        /// <summary>
        /// ■アクティブプロジェクトのワークスペースとライブラリフォルダの有無をチェック、無い場合作成
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="Paths"></param>
        /// <param name="automode"></param>
        public static void RemakeWorkSpaceAndLibraryFolder(Inventor.Application oApp, bool automode = false)
        {
            string ProjectFullFileName;
            InventorControl.GetActiveProjectName(oApp, out ProjectFullFileName);

            string WorkSpacePath = InventorControl.GetActiveProjectWorkSpaceFolder(oApp);
            if (string.IsNullOrWhiteSpace(WorkSpacePath) == false)
            {
                if (FileFolder.DirExists(WorkSpacePath) == false)
                {
                    FileFolder.MakeDirectory(WorkSpacePath);
                    if (automode == false)
                    {
                        MessageBox.Show($"ﾌﾟﾛｼﾞｪｸﾄﾌｧｲﾙ{ProjectFullFileName}に設定されている作業ｽﾍﾟｰｽ{WorkSpacePath}が見つかりません。再作成しました\n" +
                        $"この動作は【東陽Inventorｱﾄﾞｲﾝ】 によって実行されました",
                        "■東陽ﾂｰﾙ 情報", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }

            string[] LibaryPaths = InventorControl.GetActiveProjectLibraryPaths(oApp);

            foreach (var path in LibaryPaths)
            {
                if (FileFolder.DirExists(path))
                {
                }
                else
                {
                    FileFolder.MakeDirectory(path);
                    if (automode == false)
                    {
                        MessageBox.Show($"ﾌﾟﾛｼﾞｪｸﾄﾌｧｲﾙ{ProjectFullFileName}に設定されているﾗｲﾌﾞﾗﾘﾊﾟｽ{path}が見つかりません。再作成しました\n" +
                        $"この動作は【東陽Inventorｱﾄﾞｲﾝ】 によって実行されました",
                        "■東陽ﾂｰﾙ 情報", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }



        }

        /// <summary>
        /// ■アクティブプロジェクトの種類によってコンテンツセンタを切り替える
        /// </summary>
        /// <param name="oApp"></param>
        public static bool ChangeActiveProjectContentCenterAcessmode(Inventor.Application oApp, out MultiUserModeEnum currnetProjectMode, out ContentCenterAccessOptionEnum contentCenterAccessOptionEnum, out string DesktopModeLibraryLocation)
        {
            Inventor.DesignProjectManager oDesignProjectMgr;
            oDesignProjectMgr = oApp.DesignProjectManager;

            Inventor.DesignProject oDesignProject;
            oDesignProject = oDesignProjectMgr.ActiveDesignProject;



            oApp.ContentCenterOptions.GetAccessOption(out contentCenterAccessOptionEnum, out DesktopModeLibraryLocation);


            if (oDesignProject.ProjectType == MultiUserModeEnum.kVaultMode)  // ﾌﾟﾛｼﾞｪｸﾄﾌｧｲﾙが Vaultの時
            {
                currnetProjectMode = oDesignProject.ProjectType;

                if (contentCenterAccessOptionEnum == ContentCenterAccessOptionEnum.kInventorDesktopAccess)　 // ｱﾌﾟﾘｹｰｼｮﾝの設定がﾃﾞｽｸﾄｯﾌﾟの場合はVaultへ変更する
                {
                    contentCenterAccessOptionEnum = ContentCenterAccessOptionEnum.kVaultOrProductstreamServerAccess;
                    try
                    {
                        oApp.ContentCenterOptions.SetAccessOption(contentCenterAccessOptionEnum);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        DebugConsole.WriteLine($"ChangeActiveProjectContentCenterAcessmode(...)にて例外 {ex.Message}");

                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            else // ﾌﾟﾛｼﾞｪｸﾄﾌｧｲﾙが Vault以外の時
            {
                currnetProjectMode = oDesignProject.ProjectType;

                if (contentCenterAccessOptionEnum == ContentCenterAccessOptionEnum.kVaultOrProductstreamServerAccess)　 // ｱﾌﾟﾘｹｰｼｮﾝの設定がVaultの場合はﾃﾞｽｸﾄｯﾌﾟへ変更する
                {
                    contentCenterAccessOptionEnum = ContentCenterAccessOptionEnum.kInventorDesktopAccess;
                    oApp.ContentCenterOptions.SetAccessOption(contentCenterAccessOptionEnum, DesktopModeLibraryLocation);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// □未テスト ﾗｲﾌﾞﾗﾘパスを追加
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="LibraryName"></param>
        /// <param name="LibraryPath"></param>
        public static void AddLibraryPath(Inventor.Application oApp, string LibraryName, string LibraryPath)
        {
            DesignProjectManager oProjectMgr = oApp.DesignProjectManager;

            // Get the active project
            DesignProject dProject = oProjectMgr.ActiveDesignProject;

            ProjectPaths oLibraryPaths = dProject.LibraryPaths;

            oLibraryPaths.Add(LibraryName, LibraryPath);
        }


    }
}
