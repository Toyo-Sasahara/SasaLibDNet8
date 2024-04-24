using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SasaLib
{
    /// <summary>
    /// メソッドの返り値をパックして返す
    /// </summary>
    public struct ResultAndMsg
    {
        // 返り値　true, false
        public bool result;
        // メッセージ
        public string msg;
    }

    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class FileFolder
    {
        public static bool IsDirectory(string path)
        {
            var fileInfo = new FileInfo(path);

            if (fileInfo.Attributes.HasFlag(FileAttributes.Directory))
                return true;
            else
                return false;
        }

        /// ---------------------------------------------------------------------------------------
        /// <summary>
        ///     指定した検索パターンに一致するファイルを最下層まで検索しすべて返します。</summary>
        /// <param name="stRootPath">
        ///     検索を開始する最上層のディレクトリへのパス。</param>
        /// <param name="stPattern">
        ///     パス内のファイル名と対応させる検索文字列。</param>
        /// <returns>
        ///     検索パターンに一致したすべてのファイルパス。</returns>
        /// ---------------------------------------------------------------------------------------
        [SupportedOSPlatform("windows")]
        public static string[] GetFilesMostDeep(string stRootPath, string stPattern)
        {
            System.Collections.Specialized.StringCollection hStringCollection = (
                new System.Collections.Specialized.StringCollection()
            );

            // このディレクトリ内のすべてのファイルを検索する
            try
            {
                foreach (string stFilePath in System.IO.Directory.GetFiles(stRootPath, stPattern))
                {
                    hStringCollection.Add(stFilePath);
                    MainWindow.DoEvents();
                }
                // このディレクトリ内のすべてのサブディレクトリを検索する (再帰)
                foreach (string stDirPath in System.IO.Directory.GetDirectories(stRootPath))
                {
                    MainWindow.DoEvents();

                    string[] stFilePathes = GetFilesMostDeep(stDirPath, stPattern);

                    // 条件に合致したファイルがあった場合は、ArrayList に加える
                    if (stFilePathes != null)
                    {
                        hStringCollection.AddRange(stFilePathes);
                    }
                }
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class.", EventLogEntryType.Error, 0, $"FileFolder.GetFilesMostDeep(..)にて 例外検知 {ex.Message}");
                return null;
            }

            // StringCollection を 1 次元の String 配列にして返す
            string[] stReturns = new string[hStringCollection.Count];
            hStringCollection.CopyTo(stReturns, 0);

            return stReturns;
        }

        /// <summary>
        /// 指定した検索パターンに一致するファイルを最下層まで検索しすべて返します。
        /// メソッド内でnewを使いません
        /// </summary>
        /// <param name="stRootPath"></param>
        /// <param name="stPattern"></param>
        /// <returns></returns>
        public static string[] FindFiles(string stRootPath, string stPattern = "*.*")
        {
            var fullPaths = new List<string>();
            // このディレクトリ内のすべてのファイルを検索する
            try
            {
                foreach (string stFilePath in System.IO.Directory.GetFiles(stRootPath, stPattern))
                {
                    fullPaths.Add(stFilePath);
                    MainWindow.DoEvents();
                }
                // このディレクトリ内のすべてのサブディレクトリを検索する (再帰)
                foreach (string stDirPath in System.IO.Directory.GetDirectories(stRootPath))
                {
                    MainWindow.DoEvents();

                    string[] stFilePathes = GetFilesMostDeep(stDirPath, stPattern);

                    // 条件に合致したファイルがあった場合は、ArrayList に加える
                    if (stFilePathes != null)
                    {
                        fullPaths.AddRange(stFilePathes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲FindFiles({stRootPath},{stPattern}),失敗,IOException={ex.Message}");

            }

            return fullPaths.ToArray();
        }

        /// <summary>
        /// System.IO.File.Exists()のラッパー 指定したファイルがあればtureを返す
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static bool FileExists(string fullpath)
        {
            try { return System.IO.File.Exists(fullpath); }
            catch (Exception ex) { Console.WriteLine(ex); return false; }
        }

        /// <summary>
        /// 未実装
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static bool FileExists(object fullpath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ディレクトリ存在確認
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static bool DirExists(string fullpath)
        {
            try { return System.IO.Directory.Exists(fullpath); }
            catch (Exception ex) { Console.WriteLine(ex); return false; }
        }

        /// <summary>
        /// 入力したフルパス名が存在しない場合、指定したbasePathディレクトリ フルパス名の先頭からディレクトリを１つづ消した文字列 をSystem.Path.Combine()で
        ///　構築した新しいフルパス名にてファイルの存在確認を繰り返す。見つかった場合はそのパスを返す。見つからない場合はnullを返す
        ///　C:\ProgramData\TOYOCOMMON\Autocad\Template\am2022_JIS-TOYO.dwt
        ///　{basePath}\ProgramData\TOYOCOMMON\Autocad\Template\am2022_JIS-TOYO.dwt
        ///　{basePath}\TOYOCOMMON\Autocad\Template\am2022_JIS-TOYO.dwt
        ///　{basePath}\Autocad\Template\am2022_JIS-TOYO.dwt
        ///　{basePath}\Template\am2022_JIS-TOYO.dwt
        ///　{basePath}\am2022_JIS-TOYO.dwt
        ///　
        /// </summary>
        /// <param name="searchFullFileName">調査するﾌﾙﾊﾟｽ名</param>
        /// <param name="basePath"></param>
        /// <param name="ExpandEnvironment">trueのとき文字列中の環境変数定義を展開します</param>
        /// <returns></returns>
        public static string FileExistsLoopSearch(string searchFullFileName, string basePath, bool ExpandEnvironment = true)
        {
            Console.WriteLine($"FileExistsLoopSearch(..)スタート");

            string Fullfilename;
            string BasaePathname;

            // 最初に指定したフルパス名で発見したらそのまま返す
            if (System.IO.File.Exists(searchFullFileName))
            {
                Console.WriteLine($"指定フルパス名にてすぐ見つかりました \"{searchFullFileName}\"");

                return searchFullFileName;
            }


            Console.WriteLine($"searchFullFileName = {searchFullFileName}");
            Console.WriteLine($"basePath = {basePath}");

            if (ExpandEnvironment)
            {
                Fullfilename = System.Environment.ExpandEnvironmentVariables(searchFullFileName);
                BasaePathname = System.Environment.ExpandEnvironmentVariables(basePath);
            }
            else
            {
                Fullfilename = searchFullFileName;
                BasaePathname = basePath;
            }

            Console.WriteLine($"Fullfilename = {Fullfilename}");

            Console.WriteLine($"BasaePathname = {BasaePathname}");

            int p;

            do
            {
                // 文字列の最初の\マークの位置を得ます。
                p = Fullfilename.IndexOf(@"\");
                // 
                if (p < 0)
                {
                    Console.WriteLine($"見つかりませんでした");
                    return null;
                }


                // 見つけた位置から最後までの文字列を取得し、自身を上書きします
                Fullfilename = Fullfilename.Substring(p + 1);

                // 文字列からフォルダ名の取得を試みます。
                var curDirectory = System.IO.Path.GetDirectoryName(Fullfilename);

                // ベースフォルダを 文字列先頭に接続し、ファイルの存在を確認します
                var newSearchFullPath = System.IO.Path.Combine(BasaePathname, Fullfilename);
                Console.WriteLine($"存在を調査する新しいフルパス = {newSearchFullPath}");



                if (System.IO.File.Exists(newSearchFullPath))
                {
                    // ファイルが存在したら、その新しいフルパス名を返します。
                    Console.WriteLine($"見つかりました \"{newSearchFullPath}\"");
                    return newSearchFullPath;
                }

                // \マークが見つからなかった場合は 検索失敗としてnullを返して終了します
            } while (p > 0);

            Console.WriteLine($"見つかりませんでした");

            return null;

        }


        /// <summary>
        /// フォルダ存在判定【タイムアウト付き】
        /// https://apuridasuo.hatenablog.com/entry/2020/05/19/160907
        /// </summary>
        /// <param name="path">検索されるフォルダパス</param>
        /// <param name="i_Wait">【ms】だけ処理待ち</param>
        /// <returns>検索結果（ True = 存在する ）</returns>
        public static bool IsNetworkFolderExists(string path, int i_Wait = 150)
        {
            bool exists = true;
            System.Threading.Thread t = new System.Threading.Thread
            (
                new System.Threading.ThreadStart(delegate ()
                {
                    exists = Directory.Exists(path);
                })
            );
            t.Start();
            bool completed = t.Join(i_Wait);
            if (!completed)
            {
                exists = false;
#pragma warning disable SYSLIB0006 // 型またはメンバーが旧型式です
                t.Abort();
#pragma warning restore SYSLIB0006 // 型またはメンバーが旧型式です
            }
            return exists;
        }

        /// <summary>
        /// C:\ProgramDataを返す
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string GetCommonApplicationData()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        }

        /// <summary>
        /// 現在ユーザーのAppplication Dataフォルダ (%APPDATA%)を返す
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string GetRoamingUserApplicationData()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        /// <summary>
        /// ユーザーフォルダを返す
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string GetUserProfile()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }

        /// <summary>
        /// ユーザーのマイドキュメントフォルダを返す
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string GetMyDocuments()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        /// <summary>
        /// ユーザーのマイピクチャフォルダを返す
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string GetMyPictures()
        {
            return System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        }

        /// <summary>
        /// パスを指定し、無ければディレクトリを作成。ディレクトリパスの末尾にディレクトリセパレーターは不要
        /// </summary>
        /// <param name="path">作成するディレクトリパス</param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool MakeDirectory(string path)
        {
            // 文字列の最後はディレクトリせぱーれたでない場合は追加
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
                path += Path.DirectorySeparatorChar;

            //ディレクトリの存在確認.なければ作成
            try
            {
                if (Directory.Exists(path) != true)
                {

                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                }
                return true;
            }
            catch (IOException e1)
            {
                Console.WriteLine($"▲ディレクトリ作成失敗：IOException source: {0}", e1);
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲MakeDirectory({path}),失敗,IOException={e1.Message}");
                return false;
            }
        }

        /// <summary>
        /// ファイルの削除 System.IO.File.delete()のラッパー
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool RemoveFile(string filepath)
        {
            try
            {
                System.IO.File.Delete(filepath);
                return true;
            }
            catch (IOException e1)
            {
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲RemoveFile({filepath}),失敗,IOException={e1.Message}");
                return false;
            }
        }

        /// <summary>
        /// フォルダの削除
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool RemoveFolder(string filepath, bool recursive = false)
        {
            try
            {
                System.IO.Directory.Delete(filepath, recursive);
                return true;
            }
            catch (IOException e1)
            {
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲RemoveFolder({filepath}),失敗,IOException={e1.Message}");
                return false;
            }
        }

        /// <summary>
        /// ファイルを移動
        /// System.IO.File.Move()のラッパー
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool MoveFile(string sourceFileName, string destFileName)
        {
            try
            {
                System.IO.File.Move(sourceFileName, destFileName);
                return true;
            }
            catch (IOException e1)
            {
                    Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲MoveFile({sourceFileName}, {destFileName}),失敗,IOException={e1.Message}");
                return false;
            }

        }

        /// <summary>
        /// 指定したファイル（フルパス）を指定したフォルダにコピーする。
        /// 保存先フォルダが無い場合は作成する。保存先ファイルが存在した場合は上書き。
        /// </summary>
        /// <param name="sourceFilePath">ソースファイル・フルパス</param>
        /// <param name="dist">保存先フォルダ.末尾に\はなし</param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool CopyWithRotatedBackup(string sourceFilePath, string dist)
        {
            try
            {
                if (IsFolder(dist))
                {
                    //保存先ディレクトリが存在しない場合、作成を試みる
                    if (Directory.Exists(dist) != true)
                    {
                        var ans = Directory.CreateDirectory(dist);
                    }
                    string fileName = Path.GetFileName(sourceFilePath);
                    System.IO.File.Copy(sourceFilePath, dist + System.IO.Path.DirectorySeparatorChar + fileName, true);
                    return true;
                }
                else // distfolderがファイルを指している場合
                {
                    //保存先ディレクトリが存在しない場合、作成を試みる
                    if (Directory.Exists(System.IO.Path.GetDirectoryName(dist)) != true)
                    {
                        var ans = Directory.CreateDirectory(System.IO.Path.GetDirectoryName(dist));
                    }
                    System.IO.File.Copy(sourceFilePath, dist, true);
                    return true;
                }
            }
            catch (IOException e1)
            {
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲CopyFile({sourceFilePath},{dist}),失敗,IOException={e1.Message}");
                return false;
            }
        }

        /// <summary>
        /// File.Copy(source,dist,flag)のラッパー
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="distFilePath"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool CopyFile(string sourceFilePath, string distFilePath, bool overwrite = true)
        {
            try
            {
                System.IO.File.Copy(sourceFilePath, distFilePath, overwrite);

                return true;
            }
            catch (IOException e1)
            {
                Console.WriteLine("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲CopyFile({sourceFilePath},{distFilePath},{overwrite}),失敗,IOException={e1.Message}");
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲CopyFile({sourceFilePath},{distFilePath},{overwrite}),失敗,IOException={e1.Message}");
                return false;
            }
        }

        /// <summary>
        /// 指定したファイル（フルパス）を指定したフォルダにコピーする。コピー先に同名ファイルがある場合は
        /// ファイル名末尾に_数値 をインクリメントしてコピーを実行する
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="targetFolderPath"></param>
        [SupportedOSPlatform("windows")]
        public static bool CopyFileWithIncrementedFileName(string sourceFilePath, string targetFolderPath)
        {
            try
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string targetFilePath = Path.Combine(targetFolderPath, fileName);

                //保存先ディレクトリが存在しない場合、作成を試みる
                if (Directory.Exists(targetFolderPath) != true)
                {
                    var ans = Directory.CreateDirectory(targetFolderPath);
                }

                int fileCount = 0;
                while (System.IO.File.Exists(targetFilePath))
                {
                    fileCount++;
                    string incrementedFileName = Path.GetFileNameWithoutExtension(fileName) + "_" + fileCount.ToString() + Path.GetExtension(fileName);
                    targetFilePath = Path.Combine(targetFolderPath, incrementedFileName);
                }

                System.IO.File.Copy(sourceFilePath, targetFilePath);

                return true;
            }
            catch (IOException e1)
            {
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲CopyFileWithIncrementedFileName({sourceFilePath},{targetFolderPath}),失敗,IOException={e1.Message}");
                return false;
            }
        }

        /// <summary>
        /// コピー先のファイルが存在する場合には新しいファイル名を作成してコピーを行う
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        public static bool CopyWithIncrementedFileName(string sourcePath, string destinationPath)
        {
            try
            {
                // ファイルが存在する場合は、新しい名前を作成して再帰的にコピーを試行
                if (System.IO.File.Exists(destinationPath))
                {
                    string directory = Path.GetDirectoryName(destinationPath);
                    string fileName = Path.GetFileNameWithoutExtension(destinationPath);
                    string extension = Path.GetExtension(destinationPath);

                    // カウントをインクリメントしながら、新しいファイル名を検索
                    int count = 1;
                    string newFileName = $"{directory}/{fileName}({count}){extension}";

                    while (System.IO.File.Exists(newFileName))
                    {
                        count++;
                        newFileName = $"{directory}/{fileName}({count}){extension}";
                    }

                    // 新しい名前で再帰的にコピーを試行
                    CopyWithIncrementedFileName(sourcePath, newFileName);
                }
                else
                {
                    // ファイルをコピー
                    System.IO.File.Copy(sourcePath, destinationPath);
                    DebugConsole.WriteLine($"ファイルをコピーしました: {sourcePath} -> {destinationPath}");
                }

                return true;

            }
            catch (Exception ioex)
            {
                DebugConsole.WriteLine($"ファイルをコピーしました: {sourcePath} -> {destinationPath} {ioex.Message} {ioex.InnerException}");

                return false;
            }
        }

        /// <summary>
        /// コピー先のファイルが存在する場合には既存のファイルをローテートしてからコピーを行う
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        public static bool CopyWithFileRotation(string sourcePath, string destinationPath)
        {
            try
            {
                // ファイルが存在する場合は、既存のファイルをローテートしてからコピーを試行
                if (System.IO.File.Exists(destinationPath))
                {
                    string directory = Path.GetDirectoryName(destinationPath);
                    string fileName = Path.GetFileNameWithoutExtension(destinationPath);
                    string extension = Path.GetExtension(destinationPath);

                    // ローテート対象のファイル名を決定
                    string rotateFileName = $"{directory}/{fileName}(1){extension}";
                    int count = 1;
                    while (System.IO.File.Exists(rotateFileName))
                    {
                        count++;
                        rotateFileName = $"{directory}/{fileName}({count}){extension}";
                    }

                    // ローテート処理
                    for (int i = count - 1; i >= 1; i--)
                    {
                        string source = $"{directory}/{fileName}({i}){extension}";
                        string destination = $"{directory}/{fileName}({i + 1}){extension}";
                        System.IO.File.Move(source, destination);
                    }

                    // (1)をsourcePath からコピーして作成
                    System.IO.File.Copy(sourcePath, $"{directory}/{fileName}(1){extension}");
                    Console.WriteLine($"ファイルをコピーしました: {sourcePath} -> {directory}/{fileName}(1){extension}");

                    System.IO.File.Copy(sourcePath, destinationPath, overwrite: true);

                    Console.WriteLine($"バックアップファイルを更新しました: {sourcePath} -> {directory}/{fileName}{extension}");
                }
                else
                {
                    // ファイルをコピー
                    System.IO.File.Copy(sourcePath, destinationPath);
                    Console.WriteLine($"バックアップファイルを新規作成しました: {sourcePath} -> {destinationPath}");
                }

                return true;
            }
            catch (Exception ioex)
            {
                DebugConsole.WriteLine($"ファイルをコピーしました: {sourcePath} -> {destinationPath} {ioex.Message} {ioex.InnerException}");

                return false;
            }

        }

        /// <summary>
        /// フォルダーなら True。存在しない場合flase
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFolder(string path)
        {
            if (System.IO.Directory.Exists(path) | System.IO.File.Exists(path))
            {
                var isDirectory = System.IO.File
                    .GetAttributes(path)
                    .HasFlag(FileAttributes.Directory);
                return isDirectory;
            }
            else
                return false;
        }
        /// <summary>
        /// 更新日時を取得する
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string StrFileUpdateTime(string filepath)
        {
            // 更新日時を取得する
            DateTime dtUpdate = System.IO.File.GetLastWriteTime(filepath);
            return dtUpdate.ToString();
        }

        public static DateTime FileUpdateTime(string filepath)
        {
            // 更新日時を取得する
            DateTime dtUpdate = System.IO.File.GetLastWriteTime(filepath);
            return dtUpdate;
        }

        /// <summary>
        ///ディレクトリの再帰コピー。ディレクトリが無ければ作成される。ファイルは上書きされる。
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinationPath"></param>
        [SupportedOSPlatform("windows")]
        public static void DirectoryCopy(string sourcePath, string destinationPath)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);
            DirectoryInfo destinationDirectory = new DirectoryInfo(destinationPath);

            try
            {
                //コピー先のディレクトリがなければ作成する
                if (destinationDirectory.Exists == false)
                {
                    destinationDirectory.Create();
                    destinationDirectory.Attributes = sourceDirectory.Attributes;
                }

                //ファイルのコピー
                foreach (FileInfo fileInfo in sourceDirectory.GetFiles())
                {
                    string distFullFileName = System.IO.Path.Combine(destinationDirectory.FullName, destinationDirectory.FullName);
                    //同じファイルが存在していたら、常に上書きする
                    fileInfo.CopyTo(distFullFileName, true);
                }

                //ディレクトリのコピー（再帰を使用）
                foreach (System.IO.DirectoryInfo directoryInfo in sourceDirectory.GetDirectories())
                {
                    string disFullFileFolder = destinationDirectory.FullName + @"\" + directoryInfo.Name;

                    DirectoryCopy(directoryInfo.FullName, disFullFileFolder + directoryInfo.Name);
                }
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲FileFolder.DirectoryCopy(..) sourcePath:{sourcePath} 例外検知：{ex.Message}");
            }
        }

        //public static void DirectoryCopy(string sourcePath, string destinationPath)
        //{
        //    DirectoryInfo sourceDirectory = new DirectoryInfo(sourcePath);
        //    DirectoryInfo destinationDirectory = new DirectoryInfo(destinationPath);

        //    //コピー先のディレクトリがなければ作成する
        //    if (destinationDirectory.Exists == false)
        //    {
        //        destinationDirectory.Create();
        //        destinationDirectory.Attributes = sourceDirectory.Attributes;
        //    }

        //    //ファイルのコピー
        //    foreach (FileInfo fileInfo in sourceDirectory.GetFiles())
        //    {
        //        //同じファイルが存在していたら、常に上書きする
        //        fileInfo.CopyTo(destinationDirectory.FullName + @"\" + fileInfo.Name, true);
        //    }

        //    //ディレクトリのコピー（再帰を使用）
        //    foreach (System.IO.DirectoryInfo directoryInfo in sourceDirectory.GetDirectories())
        //    {
        //        DirectoryCopy(directoryInfo.FullName, destinationDirectory.FullName + @"\" + directoryInfo.Name);
        //    }
        //}

        /// <summary>
        /// Path.GetExtension()のラッパー
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static string GetExtention(string fullpath)
        {
            return Path.GetExtension(fullpath);
        }


        /// <summary>
        /// Path.ChangeExtensionのラッパー
        /// パス文字列のみを変えるので実際に変更されない
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static string ChangeExtension(string fullpath, string ext)
        {
            return Path.ChangeExtension(fullpath, ext);
        }


        /// <summary>
        /// 拡張子がないファイル名をフルパスから返す
        /// Path.GetFileNameWithoutExtension()のラッパー
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static string GetFileNameWithoutExtension(string fullpath)
        {
            try { return System.IO.Path.GetFileNameWithoutExtension(fullpath); }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲GetFileNameWithoutExtension({fullpath}),失敗,IOException={ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ファイルのフルパスからベースファイル名のみとりだし
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static string GetFileName(string fullpath)
        {
            try
            {
                return System.IO.Path.GetFileName(fullpath);
            }
            catch (Exception ex) { Console.WriteLine(ex); return null; }
        }

        /// <summary>
        /// Path.GetDirectoryName()のラッパー
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static string GetFolderName(string fullpath)
        {
            try { return System.IO.Path.GetDirectoryName(fullpath); }
            catch (Exception ex) { Console.WriteLine(ex); return null; }

        }


        /// <summary>
        /// ファイル・フォルダパスからルート部分(ドライブ、または共有名)を削除して返す
        /// \\HOST\SHARE\Directory\a.txt -> Directory\a.txt
        /// D:\Directory\a.txt -> Directory\a.txt
        /// </summary>
        /// <param name="uncPath"></param>
        /// <returns></returns>
        public static string RemoveHostAndShare(string uncPath)
        {
            var root = Path.GetPathRoot(uncPath);

            var result = uncPath.Replace(root, "").TrimStart(System.IO.Path.DirectorySeparatorChar);

            return result;
        }


        /// <summary>
        /// 二つのパスで示したファイルの中身が一致するかをチェック
        /// </summary>
        /// <param name="filepath1"></param>
        /// <param name="filepath2"></param>
        /// <returns></returns>
        public static bool FileCompare(string filepath1, string filepath2)
        {
            if (filepath1 == filepath2)
                return true;

            int byte1;
            int byte2;
            bool ret = false;


            try
            {
                // ReadOnlyで読み込み
                using (FileStream fs1 = new FileStream(filepath1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (FileStream fs2 = new FileStream(filepath2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs1.Length == fs2.Length)
                    {
                        do
                        {
                            byte1 = fs1.ReadByte();
                            byte2 = fs2.ReadByte();
                        }
                        while ((byte1 == byte2) && (byte1 != -1));

                        if (byte1 == byte2)
                            ret = true;
                    }
                    fs1.Close();
                    fs2.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// ReadCSVtoDictionary
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static ResultAndMsg ReadCSVtoDictionary(string filename, Dictionary<string, string> dict)
        {
            Logging log = new Logging(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"SasaLib.log");
            log.LogRotateWriteLine("ReadCSVtoList Start [" + filename + "]");
            //
            string filepath = "";
            filepath += filename;
            try
            {
                // ReadOnlyで読み込み
                using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.GetEncoding("Shift_JIS")))　// TODO: Encoding.GetEncoding(932)は .NET Core にて例外が出てしまう
                {
                    //sr.ReadLine(); 最初の一行分(表のヘッダ部分)を飛ばしたい場合
                    while (!sr.EndOfStream)
                    {
                        List<string> addData = new List<string>();

                        string line = sr.ReadLine();//一行ずつ読み込む
                                                    //先頭コメントは無視
                        if (line[0] != '#' && line[0] != ';')
                        {
                            // カンマがあるか
                            if (line.Contains(","))
                            {
                                string[] splitData = line.Split(',');//タブ区切りで分割したものを配列に追加
                                try
                                {
                                    dict.Add(splitData[0].Zen2HanUpperAndKANA(), splitData[1].Zen2HanUpperAndKANA());

                                }
                                catch (Exception ex)
                                {
                                    log.LogRotateWriteLine("ReadCSVtoDictionary() ファイルが正しくない [" + filepath + "]" + dict.Count + "行\r1" + "項目【" + line + "】:" + ex.Message);
                                    log.LogRotateWriteLine("ReadCSVtoDictionary() End");
                                    log.Close();
                                    return new ResultAndMsg()
                                    {
                                        result = false,
                                        msg = "\r[" + filepath + "]" + dict.Count + "行 " + "項目【" + line + "】:" + ex.Message
                                    };
                                }
                            }
                            else
                            {
                                log.LogRotateWriteLine("ReadCSVtoDictionary() カンマが無い[" + filepath + "]" + dict.ToString());
                                log.LogRotateWriteLine("ReadCSVtoDictionary End");
                                log.Close();
                                return new ResultAndMsg()
                                {
                                    result = false,
                                    msg = "(カンマが無い)\r[" + filepath + "]" + dict.Count
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                log.LogRotateWriteLine("コード変換テーブルファイルの読み込みエラー[" + filepath + "]" + e.Message);
                log.LogRotateWriteLine("ReadCSVtoList End");
                log.Close();
                return new ResultAndMsg()
                {
                    result = false,
                    msg = "コード変換テーブルファイルが正しくない [" + e.Message + @"]"
                };
            }

            log.LogRotateWriteLine("ReadCSVtoList End");
            log.Close();
            return new ResultAndMsg()
            {
                result = true,
                msg = null
            };
        }

        /// <summary>
        /// CSVファイルをあらかじめ確保したLISTへよみこみ
        /// List<List<string>> LoadFileData = new List<List<string>>();」 オブジェクトに読み込む
        /// データーベースファイルはそのままリストに入れます
        /// Shift_JISです
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="LoadFileData"></param>
        /// <returns>ResultAndMsg</returns>
        [SupportedOSPlatform("windows")]
        public static ResultAndMsg ReadCSVtoList(string filename, List<List<string>> LoadFileData)
        {
            Logging log = new Logging(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"SasaLib.log");
            log.LogRotateWriteLine("ReadCSVtoList Start [" + filename + "]");
            //
            string filepath = "";
            filepath += filename;
            try
            {
                // ReadOnlyモードでファイルを開く
                using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.GetEncoding("Shift_JIS")))　// TODO: Encoding.GetEncoding(932)は .NET Core にて例外が出てしまう
                {
                    //sr.ReadLine(); 最初の一行分(表のヘッダ部分)を飛ばしたい場合
                    while (!sr.EndOfStream)
                    {
                        List<string> addData = new List<string>();
                        string line = sr.ReadLine();//一行ずつ読み込む
                                                    //先頭コメントは無視
                        if (line[0] != '#' && line[0] != ';')
                        {
                            string[] splitData = line.Split(',');//タブ区切りで分割したものを配列に追加
                            for (int i = 0; i < splitData.Length; i++)
                            {
                                addData.Add(splitData[i]);//追加用のList<string>の作成
                            }
                            LoadFileData.Add(addData);//List<List<string>>のList<string>部分の追加
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                log.LogRotateWriteLine("コード変換テーブルファイルの読み込みエラー[" + filepath + "]" + e.Message);
                log.LogRotateWriteLine("ReadCSVtoList End");
                log.Close();
                return new ResultAndMsg()
                {
                    result = false,
                    msg = "コード変換テーブルファイルの読み込みエラー [" + e.Message + @"]"
                };
            }

            log.LogRotateWriteLine("ReadCSVtoList End");
            log.Close();
            return new ResultAndMsg()
            {
                result = true,
                msg = null
            };
        }

        /// <summary>
        /// 文字列からファイル名やパスに適さない文字を全角に変換する。
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        public static string ConvertInvalidFileNameChars(this string fname)
        {
            char[] cs = Path.GetInvalidFileNameChars();
            string output = fname
                .Replace("\"", "”")
                .Replace(@"<", "＜").Replace(@">", "＞")
                .Replace(@"|", "｜").Replace(@":", "：")
                .Replace(@"*", "＊").Replace(@"?", "？")
                .Replace(@"\", "￥").Replace(@"/", "／");
            foreach (char c in cs)
            {
                output = output.Replace(c, '_');
            }
            return output;
        }

        /// <summary>
        /// ファイル名に使用できない文字が含まれている場合はtrue
        /// </summary>
        /// <param name="filename">チェックするファイル名</param>
        /// <returns>true:使用不能な文字が含まれている|false:含まれていない</returns>
        public static bool ContainInvalidFileNmaeChars(string filename)
        {
            //ファイル名に使用できない文字を取得
            char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();

            if (filename.IndexOfAny(invalidChars) < 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// サイズがゼロのファイル作成
        /// </summary>
        /// <param name="filePath">作成するfileのフルパス</param>
        /// <returns>filePathが存在すればfalse,存在しない場合は作成後trueを返す</returns>
        public static bool Touch(string filePath)
        {
            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    System.IO.File.Create(filePath).Close();
                    return true;
                }
                else
                    return false;
            }
            catch (IOException ioe)
            {
                DebugConsole.WriteLine($"FileFolder.Touch({filePath})にて例外発生 {ioe.Message}");
                return false;
            }
        }

        /// <summary>
        /// 実際に拡張子を変える
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool ChangeExtensionExcute(string oldPath, string extension)
        {
            bool ans = false;
            try
            {
                // 拡張子を変更する
                string newPath = Path.ChangeExtension(oldPath, extension);
                //実際にファイル名を変更する
                //fileNameがない場合や、newFileNameが存在する場合は例外がスローされる
                System.IO.File.Move(oldPath, newPath);
                ans = true;
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲ChangeExtensionExcute({oldPath}),失敗,Exception={ex.Message}");
                ans = false;
            }
            return ans;
        }

        /// <summary>
        /// 選択したフォルダを返す
        /// </summary>
        /// <param name="SelectedPath">ユーザーが選択したパスを指定</param>
        /// <returns>フォルダーパスを返す</returns>
        [SupportedOSPlatform("windows")]
        public static string FolderSelect(string SelectedPath = "")
        {
            //FolderBrowserDialogクラスのインスタンスを作成
            FolderBrowserDialog fbd = new FolderBrowserDialog
            {

                //上部に表示する説明テキストを指定する
                Description = "フォルダを指定してください。",
                //ルートフォルダを指定する
                //デフォルトでDesktop
                RootFolder = Environment.SpecialFolder.Desktop,
                //最初に選択するフォルダを指定する
                //RootFolder以下にあるフォルダである必要がある
                SelectedPath = SelectedPath,
                //ユーザーが新しいフォルダを作成できるようにする
                //デフォルトでTrue
                ShowNewFolderButton = true
            };

            //ダイアログを表示する
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return (fbd.SelectedPath);
                ;
            }
            return null;
        }

        /// <summary>
        /// 指定したファイルパスがショートカットファイルならリンク先を返す。ショートカットファイル以外はそのまま返す
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static string GetTargetPath(string FilePath)
        {
            try
            {
                // ファイルの拡張子を取得
                string extension = Path.GetExtension(FilePath);
                // ファイルへのショートカットは拡張子".lnk"
                if (".lnk" == extension.ToLower())
                {
                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                    // ショートカットオブジェクトの取得
                    IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(FilePath);

                    // ショートカットのリンク先の取得
                    string targetPath = shortcut.TargetPath.ToString();

                    return targetPath;
                }
                else
                    return FilePath;

            }
            catch (IOException ioex)
            {
                Console.WriteLine($"GetTargetPathで例外発生：{ioex.Message}");
                Eventlog.Log.WriteEntry("SasaLib FileFolder Class", EventLogEntryType.Error, 0, $"▲GetTargetPath({FilePath}),失敗,IOException={ioex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="targetPath"></param>
        /// <param name="workingfolder"></param>
        /// <param name="description"></param>
        /// <param name="iconLocation"></param>
        /// <returns></returns>
        public static string ReadShortcutFile(string fullPath, ref string targetPath, ref string workingfolder, ref string description, ref string iconLocation)
        {
            IWshShell_Class wsc = new IWshShell_Class();
            WshShortcut ws;
            ws = (WshShortcut)wsc.CreateShortcut(fullPath);

            if (string.IsNullOrEmpty(ws.TargetPath))
                return null;

            var result = new
            {
                ws.FullName,
                ws.TargetPath,
                ws.WorkingDirectory,
                ws.Description,
                ws.IconLocation,
                ws.Arguments,
                ws.Hotkey,
                ws.WindowStyle,
            };

            targetPath = ws.TargetPath;
            workingfolder = ws.WorkingDirectory;
            description = ws.Description;
            iconLocation = ws.IconLocation;

            return result.ToString();

        }

        /// <summary>
        /// ショートカットを作成
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="targetPath"></param>
        /// <param name="workingFolder"></param>
        /// <param name="description"></param>
        /// <param name="iconLocation"></param>
        public static bool CreateShortcutFile(string fullPath, string targetPath, string workingFolder = null, string description = "新しいｼｮｰﾄｶｯﾄ", string iconLocation = "notepad.exe, 0")
        {
            try
            {

                IWshShell_Class wsc = new IWshShell_Class();
                WshShortcut ws;

                ws = (WshShortcut)wsc.CreateShortcut(fullPath);
                ws.TargetPath = targetPath;
                ws.IconLocation = iconLocation;
                ws.Description = description;

                if (workingFolder != null)
                    ws.WorkingDirectory = workingFolder;
                else
                    ws.WorkingDirectory = System.IO.Path.GetDirectoryName(targetPath);

                ws.Save();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 自分自身のプロセスを取得する
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentProcessePath()
        {
            //自分自身のプロセスを取得する
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            return p.MainModule.FileName;

        }

        // ファイル名のサフィックスに時刻を追加
        [SupportedOSPlatform("windows")]
        public static string AppendTimeStampToFilename(string source, DateTime dt)
        {
            string basename = FileFolder.GetFileNameWithoutExtension(source);
            string extent = FileFolder.GetExtention(source);

            string timestr = dt.ToString("yyyy-MM-dd_HHmmss");
            return $"{basename}_{timestr}{extent}";
        }


        /// <summary>
        /// 指定されたファイルがロックされているかどうかを返します。
        /// </summary>
        /// <param name="path">検証したいファイルへのフルパス</param>
        /// <returns>ロックされているかどうか</returns>
        public static bool IsFileLocked(string path)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return false;
        }

        public static bool CheckReadOnly(string fullfilename, out bool IsReadOnly)
        {
            FileInfo fi = new FileInfo(fullfilename);
            try
            {
                IsReadOnly = fi.IsReadOnly;
                return true;
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"FileFolder.IsReadOnly にて例外発生.out bool IsReadOnly は falseを返します。例外理由 {ex.Message}");
                IsReadOnly = false;
                return false;
            }

        }

        /// <summary>
        /// 指定ファイルの読み取り専用属性をセット・アンセット
        /// </summary>
        /// <param name="fullfilename"></param>
        /// <param name="flag">trueのとき読み取り専用、falseの時書き込み可能に設定</param>
        /// <returns>例外発生なく期待通りならtrue</returns>
        public static bool SetReadOnly(string fullfilename, bool flag)
        {
            FileInfo fi = new FileInfo(fullfilename);
            try
            {
                fi.IsReadOnly = flag;
                return true;
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"FileFolder.SetReadOnlyにて例外発生{ex.Message}");
                return false;
            }
        }

        /// <summary>
        ///  指定ファイルの読み取り専用属性をセット・アンセット(フォルダを指定・再帰)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="flag">true: ReadOnly をセット / false: ReadOnlyをアンセット</param>
        /// <returns></returns>
        public static bool RemoveReadOnlyAttributeRecursively(string path, bool flag)
        {
            try
            {
                // フォルダの読み取り禁止属性を解除
                Directory.GetDirectories(path).ToList().ForEach(d =>
                {
                    RemoveReadOnlyAttributeRecursively(d, flag);
                });

                // ファイルの読み取り禁止属性を解除
                Directory.GetFiles(path).ToList().ForEach(filePath =>
                {
                    FileInfo strDirPathInfo = new FileInfo(filePath);
                    try
                    {
                        strDirPathInfo.IsReadOnly = flag;
                    }
                    catch (Exception ex)
                    {
                        DebugConsole.WriteLine($"FileFolder.SetReadOnlyにて例外発生{ex.Message}");
                    }
                });

                // フォルダの読み取り禁止属性を解除

                FileInfo dirPathInfo = new FileInfo(path);
                try
                {
                    dirPathInfo.IsReadOnly = flag;
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"FileFolder.SetReadOnlyにて例外発生{ex.Message}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"エラーが発生しました: {e.Message}");
                return false;
            }

            return true;
        }


        public static string UNCtoLocalPath(string uncPath = @"\\fs1.ad.local\C$\ThingsToDo.txt")
        {
            // ローカルコンピュータで利用可能なドライブを取得し、一文字の式に変換します。
            string[] drives = Environment.GetLogicalDrives();

            string driveNames = String.Empty;

            foreach (string drive in drives)
                driveNames += drive.Substring(0, 1);

            //  ローカルマシンの情報を元に、動的に正規表現パターンを作成します。
            string pattern = @"\\\\" + Environment.MachineName + @"(?:\.\w+)*\\([" + driveNames + @"])\$";

            string replacement = "$1:";

            Console.WriteLine("入力文字列。" + uncPath);

            string localPath = null;
            try
            {
                localPath = Regex.Replace(uncPath, pattern, replacement,
                                          RegexOptions.IgnoreCase,
                                          TimeSpan.FromSeconds(0.5));
                return localPath;
            }
            catch (RegexMatchTimeoutException)
            {
                Console.WriteLine("置換操作はタイムアウトしました。");
                Console.WriteLine("返された文字列。" + localPath);
                if (uncPath.Equals(localPath))
                    Console.WriteLine("元のパスと同じです。");
                else
                    Console.WriteLine("オリジナルの文字列。" + uncPath);
                return null;
            }
        }

        /// <summary>
        /// 指定した2つのファイルの内容が同じかを確認します。https://takap-tech.com/entry/2021/10/08/001246 , https://stackoverflow.com/questions/1358510/how-to-compare-2-files-fast-using-net
        /// FileStream私用
        /// </summary>
        public static bool IsSameFile(string path1, string path2)
        {
            if (path1 == path2)
            {
                return true;
            }

            FileStream fs1 = null;
            FileStream fs2 = null;
            try
            {
                fs1 = new FileStream(path1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                fs2 = new FileStream(path2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                if (fs1.Length != fs2.Length)
                {
                    return false;
                }

                int file1byte;
                int file2byte;
                do
                {
                    file1byte = fs1.ReadByte();
                    file2byte = fs2.ReadByte();
                }
                while ((file1byte == file2byte) && (file1byte != -1));
                return (file1byte - file2byte) == 0;
            }
            finally
            {
                using (fs1)
                { }
                using (fs2)
                { }
            }
        }

        /// <summary>
        /// MD5ファイルハッシュ値を得ます
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static string GetMD5FileHash(string filePath)
        {
#pragma warning disable SYSLIB0021 // 型またはメンバーが旧型式です
            System.Security.Cryptography.HashAlgorithm hashProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 型またはメンバーが旧型式です
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                byte[] bs = hashProvider.ComputeHash(fs);
                return BitConverter.ToString(bs).ToLower().Replace("-", "");

            }
        }

    }
}
