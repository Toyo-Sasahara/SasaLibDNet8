using System;
using System.Diagnostics;

namespace SasaLibDNet8
{
    /// <summary>
    /// Explorerを制御するクラス
    /// </summary>
    public static class WindowsExplorer
    {
        /// <summary>
        /// 指定ファイルを選択した状態でファイルの所属するフォルダを開く
        /// </summary>
        /// <param name="filepath">ファイルパスを指定</param>
        public static void OpenSelectFolder(string filepath)
        {
            string cmdline = @"explorer.exe /e, /root, /select," + "\"" + filepath + "\"";
            Console.WriteLine($"{cmdline}");
            Process.Start("EXPLORER.EXE", cmdline);
        }
    }
}
