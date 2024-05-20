using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Versioning;

namespace SasaLib
{
    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class Compress
    {
        /// <summary>
        /// ZIP圧縮
        /// </summary>
        /// <param name="sourceFileFolder"></param>
        /// <param name="distZipFileFullPath"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public static bool CreateZipArchiveFile(string sourceFileFolder, string distZipFileFullPath, SasaLibDelegateWriteLine WriteLine)
        {
            try
            {
                System.IO.Compression.ZipFile.CreateFromDirectory(
                    sourceFileFolder,
                    distZipFileFullPath,
                    System.IO.Compression.CompressionLevel.Optimal,
                    true,
                    System.Text.Encoding.GetEncoding("shift_jis"));　// TODO: Encoding.GetEncoding(932)は .NET Core にて例外が出てしまう

                while (!File.Exists(distZipFileFullPath))
                {
                    System.Threading.Thread.Sleep(100);
                }

                if (File.Exists(distZipFileFullPath))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteLine($"※SasaLib.CreateZipArchiveFile() 例外検知 sourceFileFolder:{sourceFileFolder} -> distZipFileFullPath:{distZipFileFullPath} {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 解凍先ﾌｫﾙﾀﾞ指定に問題あり
        /// </summary>
        /// <param name="sourceZipFileFullPath"></param>
        /// <param name="distFolder"></param>
        /// <param name="isDistFolderRemove"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public static bool UnZipArchiveFile(string sourceZipFileFullPath, string distFolder, bool isDistFolderRemove, SasaLibDelegateWriteLine WriteLine)
        {
            try
            {
                if (isDistFolderRemove)
                {
                    FileFolder.RemoveFolder(distFolder, true);
                }

                System.IO.Compression.ZipFile.ExtractToDirectory(
                    sourceZipFileFullPath,
                    distFolder,
                    System.Text.Encoding.GetEncoding("shift_jis")　// TODO: Encoding.GetEncoding(932)は .NET Core にて例外が出てしまう
                    );
                return true;
            }
            catch (Exception ex)
            {
                WriteLine($"※SasaLib.UnZipArchiveFile() 例外検知 sourceZipFileFullPath:{sourceZipFileFullPath} -> distFolder:{distFolder} {ex.Message}");
                return false;
            }
        }

    }

    /// <summary>
    /// ZIP拡張クラス。
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class MyZipFileExtensions
    {
        /// <summary>
        /// エントリーがディレクトリかどうか取得する。
        /// </summary>
        /// <param name="entry">ZIPアーカイブエントリー</param>
        /// <returns></returns>
        public static bool IsDirectory(this ZipArchiveEntry entry)
        {
            return string.IsNullOrEmpty(entry.Name);
   
        }

        /// <summary>
        /// ZIPアーカイブ内のすべてのファイルを特定のフォルダに解凍する
        /// https://notshown.hatenablog.jp/entry/2017/02/15/090908
        /// </summary>
        /// <param name="source">ZIPアーカイブ</param>
        /// <param name="destinationDirectoryName">解凍先ディレクトリ。</param>
        /// <param name="overwrite">上書きフラグ。ファイルの上書きを行う場合はtrue。</param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public static bool ExtractToDirectory(this ZipArchive source, string destinationDirectoryName, bool overwrite, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = Console.WriteLine;

            bool ans = true;
            try
            {
                foreach (var entry in source.Entries)
                {
                    var sanitizedFullName = entry.FullName.Replace("/", "\\");
                    var fullPath = Path.Combine(destinationDirectoryName, sanitizedFullName);
                    string directory=null;

                    try
                    {
                        directory = System.IO.Path.GetDirectoryName(fullPath);
                        if (System.IO.Directory.Exists(directory) == false)
                        {
                            System.IO.Directory.CreateDirectory(directory);
                            WriteLine($"■SasaLib.MyZipFileExtensions.ExtractToDirectory(..) directory={directory} がありません。作成しました");
                        }
                    }
                    catch (Exception ioe)
                    {
                        WriteLine($"※SasaLib.MyZipFileExtensions.ExtractToDirectory(..) directory={directory} がありません。作成しようとしましたが失敗しました。スキップします  {ioe.Message}");
                        continue;
                    }

                    WriteLine($"■SasaLib.MyZipFileExtensions.ExtractToDirectory(..) overwriteﾓｰﾄﾞです fullPath={fullPath} ");

                    if (entry.IsDirectory())
                    {
                        if (!Directory.Exists(fullPath))
                        {
                            Directory.CreateDirectory(fullPath);
                        }
                    }
                    else
                    {
                        if (overwrite)
                        {
                            if (System.IO.File.Exists(fullPath) == true)
                            {
                                WriteLine($"■SasaLib.MyZipFileExtensions.ExtractToDirectory(..) overwriteﾓｰﾄﾞです。 {fullPath} が既に存在します。読取り属性の解除を試みます");

                                try
                                {
                                    // ファイルやディレクトリの属性（群）を取得
                                    FileAttributes fas = System.IO.File.GetAttributes(fullPath);

                                    // ファイル属性に読み取り専用を解除
                                    fas = fas & ~FileAttributes.ReadOnly;
                                    System.IO.File.SetAttributes(fullPath, fas);

                                }
                                catch (Exception ex)
                                {
                                    WriteLine($"※SasaLib.MyZipFileExtensions.ExtractToDirectory(..) overwriteﾓｰﾄﾞです。 {fullPath} を書き込み可能への設定変更に失敗 {ex.Message}");
                                }
                            }
                            //
                            try
                            {
                                entry.ExtractToFile(fullPath, true);
                                WriteLine($"■SasaLib.MyZipFileExtensions.ExtractToDirectory(..) overwriteﾓｰﾄﾞです。{fullPath} を解凍しました。");

                            }
                            catch (Exception ex)
                            {
                                WriteLine($"※SasaLib.MyZipFileExtensions.ExtractToDirectory(..) overwriteﾓｰﾄﾞです。 {fullPath} を解凍に失敗 {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                entry.ExtractToFile(fullPath, true);
                                WriteLine($"■SasaLib.MyZipFileExtensions.ExtractToDirectory(..) 非overwriteﾓｰﾄﾞです。 {fullPath} を解凍しました");
                            }
                            catch (Exception ex)
                            {
                                WriteLine($"※SasaLib.MyZipFileExtensions.ExtractToDirectory(..) 非overwriteﾓｰﾄﾞです。 {fullPath} を解凍に失敗 {ex.Message}");
                            }
                        }
                    }
                }
                source.Dispose();

                return ans;
            }
            catch (Exception ex)
            {
                WriteLine($"※SasaLib.MyZipFileExtensions.ExtractToDirectory(..)  一部もしくは全部のﾌｧｲﾙを解凍に失敗 {ex.Message}");
                return false;
            }

        }
    }
}
