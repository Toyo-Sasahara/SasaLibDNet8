// SasaLib メインクラス
using System;
using System.Diagnostics;


namespace SasaLibDNet8
{



    /// <summary>
    /// OSレベルのコマンドを実行するためのクラス
    /// </summary>
    public static class OScommand
    {
        /// <summary>
        /// バッチファイルを実行する
        /// </summary>
        /// <param name="batchfilepath">バッチファイルのパス</param>
        /// <param name="param">パラメータ</param>
        /// <returns></returns>
        public static int ExcuteBatchCMD(string batchfilepath, string param = "")
        {
            //
            var startInfo = new System.Diagnostics.ProcessStartInfo()
            {
                //
                FileName = System.Environment.GetEnvironmentVariable("ComSpec"),
                //
                CreateNoWindow = true,
                //
                UseShellExecute = true,
                //
                Arguments = string.Format(@" /C """"{0}"" {1}", batchfilepath, param + @"""")
            };

            //
            Console.WriteLine(startInfo.Arguments);
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);
            //
            process.WaitForExit();
            //
            return process.ExitCode;
        }

        /// <summary>
        /// 外部コマンドを実行。終了コードを返す
        /// </summary>
        /// <param name="batchfilepath"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static int ExcuteBatchCMD2(string batchfilepath, string param = "", SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine != null)
            {
                WriteLine = Console.WriteLine;
                WriteLine($"{batchfilepath} {param}");
            }

            //
            ProcessStartInfo psInfo = new ProcessStartInfo
            {
                FileName = batchfilepath,
                Arguments = param,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process ps = Process.Start(psInfo);
            string st = ps.StandardOutput.ReadToEnd();
            ps.WaitForExit();
            int rt = ps.ExitCode;
            return rt;
        }

        /// <summary>
        /// 外部コマンドを実行。終了コードを返す。標準出力と標準出力エラーを取得する
        /// </summary>
        /// <param name="batchfilepath"></param>
        /// <param name="param"></param>
        /// <param name="stdoutStr"></param>
        /// <param name="stdErrStr"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public static int ExcuteBatchCMD2B(string batchfilepath, string param, out string stdoutStr, out string stdErrStr, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine != null)
            {
                WriteLine = Console.WriteLine;
                WriteLine($"{batchfilepath} {param}");
            }

            stdoutStr = null;
            stdErrStr = null;

            try
            {
                ProcessStartInfo psInfo = new ProcessStartInfo
                {
                    FileName = batchfilepath,
                    Arguments = param,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                Process ps = Process.Start(psInfo);

                string resultStdOut = ps.StandardOutput.ReadToEnd();

                if (string.IsNullOrEmpty(resultStdOut) == true)
                    stdoutStr = null;
                else
                    stdoutStr = resultStdOut;

                string resultStdErrOut = ps.StandardError.ReadToEnd();
                if (string.IsNullOrEmpty(resultStdErrOut) == true)
                    stdErrStr = null;
                else
                    stdErrStr = resultStdErrOut;

                ps.WaitForExit();
                int rt = ps.ExitCode;
                return rt;
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib.OScommand", EventLogEntryType.Error, 0, $"SasaLib.OScommand.ExcuteBatchCMD2B()にて例外検知。{ex.Message} {ex.StackTrace}");

                return -1;
            }
        }

        /// <summary>
        /// 外部コマンドを実行。終了コードを返す.標準出力を取りこむ
        /// </summary>
        /// <param name="batchfilepath"></param>
        /// <param name="st"></param>
        /// <param name="param"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public static int ExcuteBatchCMD3(string batchfilepath, out string st, string param = "", SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine != null)
            {
                WriteLine = Console.WriteLine;
                WriteLine($"{batchfilepath} {param}");
            }

            //
            ProcessStartInfo psInfo = new ProcessStartInfo
            {
                FileName = batchfilepath,
                Arguments = param,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process ps = Process.Start(psInfo);
            st = ps.StandardOutput.ReadToEnd();
            ps.WaitForExit();
            int rt = ps.ExitCode;
            return rt;
        }

        public static bool ExecuiteCMD(string exeFilePath, out string stdoutput, string param = "")
        {
            stdoutput = "";

            string command = "\"" + exeFilePath + "\"" + " " + param;
            if (command == string.Empty)
            {
                // コマンド文字列が空では実行できないのでなにもせずに終了
                return true;
            }
            try
            {


                Process p = new Process();
                // コマンドプロンプトと同じように実行します
                p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
                p.StartInfo.Arguments = "/c " + command; // 実行するファイル名（コマンド）

                p.StartInfo.CreateNoWindow = true;   // コンソール・ウィンドウは開かない
                p.StartInfo.UseShellExecute = false; // シェル機能を使用しない
                p.StartInfo.RedirectStandardOutput = true;   // <-- これが「標準出力」リダイレクト
                p.Start();  // コマンドを実行します

                // 標準出力に出力しようとした内容を取得
                stdoutput = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"SasaLib.OScommand.ExecuiteCMD()にて例外検知。{ex.Message}");
                return false;
            }

            return true;
        }
    }

}
