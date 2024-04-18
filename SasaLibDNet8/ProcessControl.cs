using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLibDNet8
{
    public static class ProcessControl
    {
        /// <summary>
        /// 指定した実行ファイルが実行中かを調べる テスト中
        /// </summary>
        /// <param name="exePath"></param>
        /// <returns></returns>
        public static bool IsProcessRunningForExecutable(string exePath)
        {
            string exeFileName = System.IO.Path.GetFileNameWithoutExtension(exePath);

            // 実行中のプロセスを取得
            Process[] processes = Process.GetProcesses();

            // 実行中のプロセスの中から、指定した実行ファイルを持つプロセスがあるかどうかをチェック
            foreach (Process process in processes)
            {
                try
                {
                    if (process.MainModule != null && process.MainModule.FileName.Equals(exePath, StringComparison.OrdinalIgnoreCase))
                    {
                        // 実行中のプロセスの実行ファイルパスが指定したパスと一致する場合、実行中と判定
                        return true;
                    }
                }
                catch (Exception)
                {
                    // MainModule がアクセスできない場合があるため、例外処理で回避
                }
            }

            return false;
        }
    }
}
