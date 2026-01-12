using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.SysConfigurator
{
    [SupportedOSPlatform("windows")]
    public static class TestRun
    {
        public static void Execute(string ControlFile, string baseFolder = null, string loggingFolder = null)
        {
            byte[] AESkey
                = {
                      0x6F, 0xDF, 0x98, 0x00, 0x67, 0x36, 0x7D, 0x3B,
                      0xFF, 0xC9, 0x3B, 0x79, 0x4D, 0xD4, 0x81, 0x72
                 };
            string Base64AES_key = EncryptionAES.CovertFromByteArrayToBase64String(AESkey);



            string executedDirectory = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            Console.WriteLine($"実行中ﾌｫﾙﾀﾞ {executedDirectory}");
            DebugConsole.WriteLine($"コントロールファイル {ControlFile}");

            DebugConsole.WriteLine($"ログファイルは {System.IO.Path.GetDirectoryName(ControlFile)}");

            string baseDir;
            if (baseFolder == null)
                baseDir = System.IO.Path.GetDirectoryName(ControlFile);
            else
                baseDir = baseFolder;

            if (string.IsNullOrWhiteSpace(loggingFolder))
                loggingFolder = System.IO.Path.GetDirectoryName(ControlFile);

            // loggingFolder の存在確認と作成
            if (!string.IsNullOrWhiteSpace(loggingFolder))
            {
                try
                {
                    if (!System.IO.Directory.Exists(loggingFolder))
                    {
                        System.IO.Directory.CreateDirectory(loggingFolder);
                        DebugConsole.WriteLine($"ログフォルダを作成しました {loggingFolder}");
                    }
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"ログフォルダの作成に失敗しました: {loggingFolder} - {ex.Message}");
                    throw;
                }
            }

            // loggingFolder の存在確認と作成
            if (!string.IsNullOrWhiteSpace(loggingFolder))
            {
                try
                {
                    if (!System.IO.Directory.Exists(loggingFolder))
                    {
                        System.IO.Directory.CreateDirectory(loggingFolder);
                        DebugConsole.WriteLine($"ログフォルダを作成しました {loggingFolder}");
                    }
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"ログフォルダの作成に失敗しました: {loggingFolder} - {ex.Message}");
                    throw;
                }

                // ログフォルダへの書き込み可否チェック（不可なら例外送出）
                string testFilePath = System.IO.Path.Combine(loggingFolder, $"._write_test_{Guid.NewGuid():N}.tmp");
                try
                {
                    System.IO.File.WriteAllText(testFilePath, "permission test");
                    System.IO.File.Delete(testFilePath);
                    DebugConsole.WriteLine($"ログフォルダへの書き込みを確認しました {loggingFolder}");
                }
                catch (UnauthorizedAccessException uaex)
                {
                    DebugConsole.WriteLine($"ログフォルダへの書き込み権限がありません: {loggingFolder} - {uaex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"ログフォルダへの書き込みチェックに失敗しました: {loggingFolder} - {ex.Message}");
                    throw;
                }
            }

            XML_Control xML_ConfigxFile = new XML_Control(Base64AES_key, loggingFolder, ControlFile);

            xML_ConfigxFile.JobLoadAndExecute(ControlFile, baseDir, DebugConsole.WriteLine, foreceExecute: true, IsRemoveControlFile: false, loggingFolder: loggingFolder);

        }

    }
}
