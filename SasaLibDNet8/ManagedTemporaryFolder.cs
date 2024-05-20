using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;

namespace SasaLib
{
    /// <summary>
    /// 作業後に自動的に一時フォルダが消える。フォルダ内のファイルも消すことが出来る
    /// https://takap-tech.com/entry/2019/02/24/114210
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class ManagedTemporaryFolder
    {
        /// <summary>
        /// 使用後に消える一時フォルダを使用するためのコンテキストを提供します。
        /// </summary>
        /// <param name="f"></param>
        /// <param name="identifier"></param>
        public static void FolderContext(Action<string> f,string identifier = "")
        {
            string path = string.Empty;
            try
            {
                // 一時フォルダに作業用のサブフォルダを作成
                path = Path.Combine(Path.GetTempPath(), identifier + Guid.NewGuid().ToString());
                Directory.CreateDirectory(path);
                //Eventlog.Log.WriteEntry("TempResource.FolderContext(..)", EventLogEntryType.Information, 1006, $"フォルダを作成しました {path}", false);

                f(path);
            }
            finally
            {
                if (Directory.Exists(path))
                {
                    WriteModeSetRecursive(path);
                    FileFolder.RemoveFolder(path, true); // 再帰検索でフォルダ内のﾌｧｲﾙを消す
                    //Eventlog.Log.WriteEntry("TempResource.FolderContext(..)", EventLogEntryType.Information, 1006, $"フォルダを削除しました {path}", false);
                }
            }
        }

        /// <summary>
        /// 指定フォルダをスイープして読み取り専用属性を書き込み可能に変更する
        /// 再帰を使わない例
        /// </summary>
        /// <param name="root"></param>
        /// <param name="debug"></param>
        /// <param name="WriteLine"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void WriteModeSetRecursive(string root, bool debug = false ,SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            // ファイルを検査するサブフォルダの名前を保持するデータ構造。
            // サブフォルダの名前を保持する。
            Stack<string> dirs = new Stack<string>(20);

            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                // フォルダやファイルの探索権限がない場合、UnauthorizedAccessException例外がスローされます。
                // 例外がスローされます．この例外を無視して
                // 例外を無視して残りのファイルやフォルダの列挙を続けることも可能です。
                // フォルダーの列挙を続けることが許容される場合がある。また、DirectoryNotFound例外が発生する可能性もある（ただし、可能性は低い）。
                // このような例外が発生するのは、currentDir が
                // Directory.Exists を呼び出した後に 
                // 別のアプリケーションまたはスレッドによって delete された場合に発生します．この場合
                // どの例外を捕捉するかは、実行しようとする特定のタスクに完全に依存します。
                catch (UnauthorizedAccessException e)
                {
                    WriteLine($"ManagedTemporaryFolder.WriteModeSetRecursive(..) にて例外 {e.Message}");
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    WriteLine($"ManagedTemporaryFolder.WriteModeSetRecursive(..) にて例外 {e.Message}");
                    continue;
                }

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {

                    WriteLine($"ManagedTemporaryFolder.WriteModeSetRecursive(..) にて例外 {e.Message}");
                    continue;
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    WriteLine($"ManagedTemporaryFolder.WriteModeSetRecursive(..) にて例外  {e.Message}");
                    continue;
                }
                // 各ファイルに対して必要なアクションをここで実行する。
                // このブロックを修正して、必要なタスクを実行する。
                foreach (string file in files)
                {
                    try
                    {
                        // シナリオで必要なアクションを実行します。
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        if (debug)
                            WriteLine($"ManagedTemporaryFolder.WriteModeSetRecursive(..)  {fi.Name}: {fi.Length}, {fi.CreationTime}");

                        try
                        {
                            fi.IsReadOnly = false;
                        }
                        catch (Exception ex)
                        {
                            WriteLine($"ManagedTemporaryFolder.WriteModeSetRecursive(..)  SetReadOnlyにて例外発生{ex.Message}");
                        }

                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        // ファイルが別のアプリケーションによって削除された場合
                        // TraverseTree() の呼び出し以降に別のアプリケーションまたはスレッドでファイルが削除された場合
                        // を呼び出した後に削除された場合、そのまま続行します。
                        WriteLine($"ManagedTemporaryFolder.WriteModeSetRecursive(..) にて例外  {e.Message}");
                        continue;
                    }
                }
                // サブディレクトリを探索用にスタックにプッシュします。
                // これはファイルを渡す前に行うこともできる。
                foreach (string str in subDirs)
                    dirs.Push(str);
            }

        }

        /// <summary>
        /// 使い方
        /// </summary>
        public static void MethodTest()
        {
            ManagedTemporaryFolder.FolderContext(path =>
            {
                Console.WriteLine(path);

                // 作業フォルダ内にファイルを作成
                using (var file = File.CreateText(Path.Combine(path, "a.txt")))
                {
                    file.WriteLine("test");
                }
            });

            // コンテキストを抜けるとフォルダが消えている
        }

    }

}
