using SasaLib;
//using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaSaLibDNet8TestAPP
{
    [SupportedOSPlatform("windows")]
    public partial class Tab14_FileSystemWathcer_UserControl : UserControl
    {
        Form1 mainForm;

        bool flag = false;


        public Tab14_FileSystemWathcer_UserControl(Form1 form)
        {
            this.mainForm = form;
            InitializeComponent();

        }

        private void Tab14_FileSystemWathcer_Load(object sender, EventArgs e)
        {
            InitFileSystemWather();

            SetToControls();

        }


        private void Tab14_FileSystemWathcer_VisibleChanged(object sender, EventArgs e)
        {
            LogWindowWriteLine($"Tab14_FileSystemWathcer_VisibleChanged(..)実行・・・sender.GetType() = \"{sender.GetType()}\" e.GetType() = \"{e.GetType()}\"");

            SetToControls();
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
            this.LogWindow_textBox.AppendText($"{DateTime.Now.ToShortTimeString()} {msg}\r\n");
        }


        //FileSystemWatcherのオブジェクト名を設定
        System.IO.FileSystemWatcher watcher;

        //public void InitFileSystemWatherOrg()
        //{
        //    //デスクトップ上にあるテキストファイルが更新（上書き保存）されたら、そのファイルを強制終了させて”sample write”という文字列を末尾に書くサンプルを作成します。

        //    this.Load += (s, e) =>
        //    {
        //        //監視するディレクトリの指定
        //        var directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        //        //監視するディレクトリとファイルの種類を指定して初期化
        //        watcher = new System.IO.FileSystemWatcher(directory, "*.*");

        //        //監視するフィールドの設定
        //        watcher.NotifyFilter =
        //            (NotifyFilters.Attributes
        //            | NotifyFilters.LastAccess
        //            | NotifyFilters.LastWrite
        //            | NotifyFilters.CreationTime
        //            | NotifyFilters.FileName
        //            | NotifyFilters.DirectoryName);

        //        //サブディレクトリは監視する
        //        watcher.IncludeSubdirectories = true;

        //        //イベント設定
        //        watcher.Created += new System.IO.FileSystemEventHandler(watcher_Created);
        //        watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
        //        watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Deleted);
        //        watcher.Renamed += new System.IO.RenamedEventHandler(watcher_Renamed);
        //    };
        //}

        public void InitFileSystemWather(string directory = @"D:\MainVault")
        {

            //監視するディレクトリとファイルの種類を指定して初期化
            watcher = new System.IO.FileSystemWatcher(directory, "*.*");

            //監視するフィールドの設定
            watcher.NotifyFilter =
                    (NotifyFilters.Attributes
                    | NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.CreationTime
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName);

            //サブディレクトリは監視する
            watcher.IncludeSubdirectories = true;

            //イベント設定
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Created);
            watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Deleted);
            watcher.Renamed += new System.IO.RenamedEventHandler(watcher_Renamed);
        }

        public void InitFileSystemWather(FileSystemEventHandler watcher_Created, FileSystemEventHandler watcher_Changed, FileSystemEventHandler watcher_Deleted, RenamedEventHandler watcher_Renamed, string directory = @"D:\MainVault")
        {

            //監視するディレクトリとファイルの種類を指定して初期化
            watcher = new System.IO.FileSystemWatcher(directory, "*.*");

            //監視するフィールドの設定
            watcher.NotifyFilter =
                    (NotifyFilters.Attributes
                    | NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.CreationTime
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName);

            //サブディレクトリは監視する
            watcher.IncludeSubdirectories = true;

            //イベント設定
            watcher.Created += new System.IO.FileSystemEventHandler(watcher_Created);
            watcher.Changed += new System.IO.FileSystemEventHandler(watcher_Changed);
            watcher.Deleted += new System.IO.FileSystemEventHandler(watcher_Deleted);
            watcher.Renamed += new System.IO.RenamedEventHandler(watcher_Renamed);
        }

        public void StartFileSystemWatcher(string path = null)
        {
            //監視を開始する
            if (string.IsNullOrWhiteSpace(path) == false)
                watcher.Path = path;
            watcher.EnableRaisingEvents = true;
            LogWindowWriteLine("Start!");

        }

        public void StopFileSystemWatcher()
        {
            //監視を停止する
            watcher.EnableRaisingEvents = false;
            LogWindowWriteLine("Sopt!");


        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            LogWindowWriteLine($"Created:\"{e.FullPath}\"");
        }

        private void watcher_Created2(object sender, FileSystemEventArgs e)
        {
            string souceFolder = SourceFolder_textBox.Text;
            string distFolder = DistFolder_textBox.Text;

            LogWindowWriteLine($"Created:\"{e.FullPath}\"");


            string createPath_soruceFolder = System.IO.Path.GetDirectoryName(e.FullPath);
            string createPath_sourceFile = System.IO.Path.GetFileName(e.FullPath);

            string a = hitosikunaikasho(souceFolder, createPath_soruceFolder);

            LogWindowWriteLine($"a:\"{a}\"");



            SasaLib.FileFolder.CopyFile(e.FullPath, distFolder);

        }

        private DateTime lastWriteTimeSave = DateTime.Now;
        //private void watcher_Org_Changed(object sender, FileSystemEventArgs e)
        //{
        //    LogWindowWriteLine("Changed");

        //    //初回のみイベントを実行
        //    var file = new FileInfo(e.FullPath);
        //    if (file.LastWriteTime.Subtract(lastWriteTimeSave) < new TimeSpan(0, 0, 0, 1)) return;

        //    //メモ帳のプロセスをプロセスを強制的に終了させる
        //    Process[] ps = Process.GetProcessesByName("notepad");
        //    foreach (Process p in ps) p.Kill();

        //    //イベントを無効にする
        //    watcher.Changed -= watcher_Changed;

        //    //イベント発生元のファイルパスを取得しファイルを開く
        //    using (StreamWriter writer = new StreamWriter(e.FullPath, true, Encoding.GetEncoding("Shift_JIS")))
        //    {
        //        //文字を書き込む
        //        writer.WriteLine("sample write");
        //    };

        //    //更新日時を更新
        //    lastWriteTimeSave = file.LastWriteTime;

        //    //イベントを無効にする
        //    watcher.Changed += watcher_Changed;
        //}

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {

            //初回のみイベントを実行 同じ時間帯に発生したイベントは無視
            var file = new FileInfo(e.FullPath);
            if (file.LastWriteTime.Subtract(lastWriteTimeSave) < new TimeSpan(0, 0, 0, 1)) return;

            try
            {
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" 理由:{e.ChangeType}");
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" GetCreationTime : {System.IO.File.GetCreationTime(e.FullPath)}");
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" GetLastAccessTime : {System.IO.File.GetLastAccessTime(e.FullPath)}");
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" GetLastWriteTime : {System.IO.File.GetLastWriteTime(e.FullPath)}");
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" GetAttributes : {System.IO.File.GetAttributes(e.FullPath)}");
            }
            catch (IOException ioe)
            {
                LogWindowWriteLine($"例外：{ioe.Message}");
            }

            ////イベントを無効にする
            //watcher.Changed -= watcher_Changed;

            ////イベントを有効にする
            //watcher.Changed += watcher_Changed;
        }
        private void watcher_Changed2(object sender, FileSystemEventArgs e)
        {

            //初回のみイベントを実行 同じ時間帯に発生したイベントは無視
            var file = new FileInfo(e.FullPath);
            if (file.LastWriteTime.Subtract(lastWriteTimeSave) < new TimeSpan(0, 0, 0, 1)) return;

            try
            {
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" 理由:{e.ChangeType}");
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" GetCreationTime : {System.IO.File.GetCreationTime(e.FullPath)}");
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" GetLastAccessTime : {System.IO.File.GetLastAccessTime(e.FullPath)}");
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" GetLastWriteTime : {System.IO.File.GetLastWriteTime(e.FullPath)}");
                LogWindowWriteLine($"初回のみChanged:\"{e.Name}\" GetAttributes : {System.IO.File.GetAttributes(e.FullPath)}");
            }
            catch (IOException ioe)
            {
                LogWindowWriteLine($"例外：{ioe.Message}");
            }

            ////イベントを無効にする
            //watcher.Changed -= watcher_Changed;

            ////イベントを有効にする
            //watcher.Changed += watcher_Changed;
        }

        private void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                LogWindowWriteLine($"Deleted:\"{e.FullPath}\"");

            }
            catch (IOException ioe)
            {
                LogWindowWriteLine($"例外：{ioe.Message}");
            }

        }
        private void watcher_Deleted2(object sender, FileSystemEventArgs e)
        {
            try
            {
                LogWindowWriteLine($"Deleted:\"{e.FullPath}\"");
            }
            catch (IOException ioe)
            {
                LogWindowWriteLine($"例外：{ioe.Message}");
            }

        }

        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            try
            {
                LogWindowWriteLine($"Renamed:\"{e.FullPath}\"");
            }
            catch (IOException ioe)
            {
                LogWindowWriteLine($"例外：{ioe.Message}");
            }

        }
        private void watcher_Renamed2(object sender, RenamedEventArgs e)
        {
            try
            {
                LogWindowWriteLine($"Renamed:\"{e.FullPath}\"");
            }
            catch (IOException ioe)
            {
                LogWindowWriteLine($"例外：{ioe.Message}");
            }

        }

        private void Start_button_Click(object sender, EventArgs e)
        {
            try
            {
                StartFileSystemWatcher(Folder_textBox.Text);
            }
            catch (IOException ioe)
            {
                LogWindowWriteLine($"例外：{ioe.Message}");
            }

        }

        private void Stop_button_Click(object sender, EventArgs e)
        {
            StopFileSystemWatcher();
        }

        private void ClearLog_button_Click(object sender, EventArgs e)
        {
            this.LogWindow_textBox.Clear();
        }

        private void Start_Sync_button_Click(object sender, EventArgs e)
        {
            string sourceDirName = SourceFolder_textBox.Text;
            string destDirName = DistFolder_textBox.Text;
            CopyDirectory(sourceDirName, destDirName, true, true);

            InitFileSystemWather(watcher_Created2, watcher_Changed2, watcher_Deleted2, watcher_Renamed2, sourceDirName);

            StartFileSystemWatcher(sourceDirName);

        }

        private void Stop_Sync_button_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// ディレクトリをコピーする
        /// </summary>
        /// <param name="sourceDirName">コピーするディレクトリ</param>
        /// <param name="destDirName">コピー先のディレクトリ</param>
        /// <param name="newerOnly">新しいファイルのみコピーする</param>
        /// <param name="sync">sourceDirNameにないファイルを削除する</param>
        public static void CopyDirectory(
            string sourceDirName,
            string destDirName,
            bool newerOnly,
            bool sync)
        {
            //コピー先のディレクトリがないときは作る
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                //属性もコピー
                File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));
            }

            //コピー先のディレクトリ名の末尾に"\"をつける
            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
            {
                destDirName = destDirName + Path.DirectorySeparatorChar;
            }

            //コピー元のディレクトリにあるファイルをコピー
            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string f in files)
            {
                string destFileName = destDirName + Path.GetFileName(f);
                //コピー先にファイルが存在し、
                //コピー元より更新日時が古い時はコピーする
                if (!newerOnly ||
                    !File.Exists(destFileName) ||
                    File.GetLastWriteTime(destFileName) < File.GetLastWriteTime(f))
                {
                    File.Copy(f, destFileName, true);
                }
            }

            //コピー先にあってコピー元にないファイルを削除
            if (sync)
            {
                DeleteNotExistFiles(sourceDirName, destDirName);
            }

            //コピー元のディレクトリにあるディレクトリについて、再帰的に呼び出す
            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, destDirName + Path.GetFileName(dir), newerOnly, sync);
            }
        }

        /// <summary>
        /// destDirNameにありsourceDirNameにないファイルを削除する
        /// </summary>
        /// <param name="sourceDirName">比較先のフォルダ</param>
        /// <param name="destDirName">比較もとのフォルダ</param>
        private static void DeleteNotExistFiles(
            string sourceDirName,
            string destDirName)
        {
            //sourceDirNameの末尾に"\"をつける
            if (sourceDirName[sourceDirName.Length - 1] != Path.DirectorySeparatorChar)
            {
                sourceDirName = sourceDirName + Path.DirectorySeparatorChar;
            }

            //destDirNameにありsourceDirNameにないファイルを削除する
            string[] files = Directory.GetFiles(destDirName);
            foreach (string f in files)
            {
                if (!File.Exists(sourceDirName + Path.GetFileName(f)))
                {
                    File.Delete(f);
                }
            }

            //destDirNameにありsourceDirNameにないフォルダを削除する
            string[] folders = Directory.GetDirectories(destDirName);
            foreach (string folder in folders)
            {
                if (!Directory.Exists(sourceDirName + Path.GetFileName(folder)))
                {
                    Directory.Delete(folder, true);
                }
            }
        }


        private static string hitosikunaikasho(string pathA = "C:\\Users\\User\\Documents\\FolderA", string pathB = "C:\\Users\\User\\Desktop\\FolderB")
        {


            string[] pathAComponents = pathA.Split('\\');
            string[] pathBComponents = pathB.Split('\\');

            int minLength = Math.Min(pathAComponents.Length, pathBComponents.Length);
            int diffIndex = 0;

            for (int i = 0; i < minLength; i++)
            {
                if (pathAComponents[i] != pathBComponents[i])
                {
                    diffIndex = i;
                    break;
                }
            }

            string uncommonPartA = string.Join("\\", pathAComponents, diffIndex, pathAComponents.Length - diffIndex);
            string uncommonPartB = string.Join("\\", pathBComponents, diffIndex, pathBComponents.Length - diffIndex);

            DebugConsole.WriteLine("共通でない部分A: " + uncommonPartA);
            DebugConsole.WriteLine("共通でない部分B: " + uncommonPartB);

            return uncommonPartA;
        }

    }
}
