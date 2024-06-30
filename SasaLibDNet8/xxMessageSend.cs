//using System.Diagnostics;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public static class MessageSend
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="account"></param>
//        /// <param name="computer"></param>
//        /// <param name="message"></param>
//        public static void MsgExe(string account, string computer, string message)
//        {
//            var startInfo = new ProcessStartInfo
//            {
//                FileName = @"C:\windows\sysnative\msg.exe",
//                Arguments = string.Format($"{account} /SERVER:{computer} \"{message}\""),
//                RedirectStandardError = true,
//                CreateNoWindow = true,
//                UseShellExecute = false,
//            };
//            var p = Process.Start(startInfo);
//            var errorMessage = p.StandardError.ReadToEnd();
//            p.WaitForExit();
//            p.Close();

//            if (errorMessage.Length > 0)
//            {
//                // エラー発生時の処理
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="computer"></param>
//        /// <param name="message"></param>
//        public static void MsgExe(string computer, string message)
//        {
//            var startInfo = new ProcessStartInfo
//            {
//                FileName = @"C:\windows\system32\msg.exe",
//                Arguments = string.Format($"* /SERVER:{computer} \"{message}\""),
//                RedirectStandardError = true,
//                CreateNoWindow = true,
//                UseShellExecute = false,
//            };
//            var p = Process.Start(startInfo);
//            var errorMessage = p.StandardError.ReadToEnd();
//            p.WaitForExit();
//            p.Close();

//            if (errorMessage.Length > 0)
//            {
//                // エラー発生時の処理
//            }
//        }

//    }
//}
