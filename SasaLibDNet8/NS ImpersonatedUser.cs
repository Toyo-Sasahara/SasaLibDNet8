
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

//namespace SasaLibDNet8
//{
//    /// <summary>
//    /// 偽装ユーザ使用クラス（Clslogon()の別の方法）
//    /// 
//    /*
//     *  
//     using (var i = new ImpersonatedUser("login", "domain", "password"))
//     {
//            var ps = new PrinterSettings(); // default printer is not defined and etc...      // ... aspose code to send to printer
//     }
//     */
//    /// </summary>
//    public class ImpersonatedUser : IDisposable
//    {
//        IntPtr userHandle;
//        WindowsImpersonationContext impersonationContext;
//        public ImpersonatedUser( string domain, string user, string password)
//        {
//            userHandle = IntPtr.Zero;
//            bool loggedOn = LogonUser(
//                user,
//                domain,
//                password,
//                LogonType.Batch,
//                LogonProvider.Default,
//                out userHandle);
//            if (!loggedOn)
//                throw new Win32Exception(Marshal.GetLastWin32Error());
//            impersonationContext = WindowsIdentity.Impersonate(userHandle);
//        }
//        public void Dispose()
//        {
//            if (userHandle != IntPtr.Zero)
//            {
//                CloseHandle(userHandle);
//                userHandle = IntPtr.Zero;
//                impersonationContext.Undo();
//                DebugConsole.WriteLine("Finished Impersonating user");
//            }
//        }
//        [DllImport("advapi32.dll", SetLastError = true)]
//        static extern bool LogonUser(
//            string lpszUsername,
//            string lpszDomain,
//            string lpszPassword,
//            LogonType dwLogonType,
//            LogonProvider dwLogonProvider,
//            out IntPtr phToken
//        );
//        [DllImport("kernel32.dll", SetLastError = true)]
//        static extern bool CloseHandle(IntPtr hHandle);
//        enum LogonType : int
//        {
//            Interactive = 2,
//            Network = 3,
//            Batch = 4,
//            Service = 5,
//            NetworkCleartext = 8,
//            NewCredentials = 9,
//        }
//        enum LogonProvider : int
//        {
//            Default = 0,
//        }
//    }
//}
