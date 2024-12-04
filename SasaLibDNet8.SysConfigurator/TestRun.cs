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
        public static void Execute(string ControlFile, string baseFolder = null)
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

            XML_Control xML_ConfigxFile = new XML_Control(Base64AES_key, System.IO.Path.GetDirectoryName(ControlFile));
                       

            xML_ConfigxFile.JobLoadAndExecute(ControlFile, baseDir, DebugConsole.WriteLine,foreceExecute:true,IsRemoveControlFile:false);

        }

    }
}
