using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Runtime.Versioning;

namespace SasaLib
{


    [SupportedOSPlatform("windows")]
    public class WithFakeAccount : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpszUsername"></param>
        /// <param name="lpszDomain"></param>
        /// <param name="lpszPassword"></param>
        /// <param name="dwLogonType"></param>
        /// <param name="dwLogonProvider"></param>
        /// <param name="phToken"></param>
        /// <returns></returns>
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="UsingClsLogon"></param>
        /// <param name="acton"></param>
        /// <param name="debugConsoleMsg"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public WithFakeAccount(string domainName, string userName, string password, bool UsingClsLogon, Action acton, bool debugConsoleMsg = false, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (UsingClsLogon == true)
            {
                if (string.IsNullOrWhiteSpace(domainName)==false && string.IsNullOrWhiteSpace(userName)==false && string.IsNullOrWhiteSpace(password) == false)
                {
                    const int LOGON32_PROVIDER_DEFAULT = 0;
                    //This parameter causes LogonUser to create a primary token.   
                    const int LOGON32_LOGON_INTERACTIVE = 2;

                    // Call LogonUser to obtain a handle to an access token.   
                    SafeAccessTokenHandle safeAccessTokenHandle;
                    bool returnValue = LogonUser(userName, domainName, password,
                        LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                        out safeAccessTokenHandle);

                    // Impersonate the specified user and execute the action
                    WindowsIdentity.RunImpersonated(safeAccessTokenHandle, acton);
                }
                else
                {
                    acton();
                }
            }
            else
            {
                acton();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static void WithFakeAccounttest()
        {
            new WithFakeAccount("AD", "Sekkei-User", "sk", true, () =>
            {
                Console.WriteLine("アカウント偽装中: " + WindowsIdentity.GetCurrent().Name);
            });

            // Check the identity again.  
            Console.WriteLine("After impersonation: " + WindowsIdentity.GetCurrent().Name);

        }
    }

}
