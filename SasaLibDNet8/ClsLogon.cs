using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SasaLibDNet8
{
    /// <summary>
    /// Windows認証ユーザを偽装してログオンを行う
    /// from https://qiita.com/h-ymmr/items/48aa308b8219f35f7255
    /// SasaLib.ImpersonatedUser も類似
    /// </summary>
    //public class ClsLogon : IDisposable
    //{
    //    bool thisUse = false;

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="lpszUserName"></param>
    //    /// <param name="lpszDomain"></param>
    //    /// <param name="lpszPassword"></param>
    //    /// <param name="dwLogonType"></param>
    //    /// <param name="dwLogonProvider"></param>
    //    /// <param name="phToken"></param>
    //    /// <returns></returns>
    //    [DllImport("advapi32.dll", SetLastError = true)]
    //    public static extern bool LogonUser(
    //        String lpszUserName,
    //        String lpszDomain,
    //        String lpszPassword,
    //        LogonSessionType dwLogonType,
    //        LogonProvider dwLogonProvider,
    //        out IntPtr phToken);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="existingTokenHandle"></param>
    //    /// <param name="SECURITY_IMPERSONATION_LEVEL"></param>
    //    /// <param name="duplicateTokenHandle"></param>
    //    /// <returns></returns>
    //    [DllImport("advapi32.dll", SetLastError = true)]
    //    public static extern bool DuplicateToken(
    //        IntPtr existingTokenHandle,
    //        int SECURITY_IMPERSONATION_LEVEL,
    //        ref IntPtr duplicateTokenHandle);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="handle"></param>
    //    /// <returns></returns>
    //    [DllImport("kernel32.dll", SetLastError = true)]
    //    public extern static bool CloseHandle(IntPtr handle);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public enum LogonSessionType : uint
    //    {
    //        Interactive = 2,
    //        Network,
    //        Batch,
    //        Service,
    //        NetworkCleartext = 8,
    //        NewCredentials
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public enum LogonProvider : uint
    //    {
    //        Default = 0,
    //        WinNT35,
    //        WinNT40,
    //        WinNT50
    //    }

       
    //    private WindowsImpersonationContext _objContext = null;
    //    private String _strUserName = null;
    //    private String _strPassword = null;
    //    private String _strDomainName = null;
    //    private String _strMessage = "";
    //    private IntPtr _objUserHandle = IntPtr.Zero;
    //    private IntPtr _objDupUserHandle = IntPtr.Zero;

    //    private string callData;


    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public ClsLogon()
    //    {
    //    }

    //    /// <summary>
    //    /// Windows認証でユーザーを偽装してログオン
    //    /// </summary>
    //    /// <param name="Domain">ドメイン</param>
    //    /// <param name="UserName">ユーザー名</param>
    //    /// <param name="Password">パスワード</param>
    //    /// <param name="UsingClsLogon">trueならDomainとUsername,Passwrdを使い 偽装してログオン</param>
    //    public ClsLogon(string Domain, string UserName, string Password, bool UsingClsLogon = false, bool debugConsoleMsg = false, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
    //    {
    //        strDomainName = Domain;
    //        strUserName = UserName;
    //        strPassword = Password;

    //        if (debugConsoleMsg)
    //        {
    //            Console.WriteLine($"□ClsLogon()呼ばれました。UsingClsLogon = {UsingClsLogon} {DateTime.Now} {this.callData}");
    //        }

    //        this.callData = $"呼び出し元:ソースファイル:{sourceFilePath},{sourceLineNumber}行,メンバー名：{memberName}";

    //        if (UsingClsLogon)
    //        {
    //            bool result = Logon(debugConsoleMsg);
    //            thisUse = true;
    //        }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public String strUserName { get { return _strUserName; } set { _strUserName = value; } }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public String strPassword { get { return _strPassword; } set { _strPassword = value; } }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public String strDomainName { get { return _strDomainName; } set { _strDomainName = value; } }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public String strMessage { get { return _strMessage; } set { _strMessage = value; } }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="debugConsoleMsg"></param>
    //    /// <returns></returns>
    //    public Boolean Logon(bool debugConsoleMsg = false)
    //    {
    //        ClsWinError objWinError = new ClsWinError();
    //        Boolean blnIsOk = false;
    //        int intWin32Error = 0;
    //        String strErrDesc = null;
    //        _strMessage = "NG";
    //        try
    //        {
    //            blnIsOk = LogonUser(_strUserName, _strDomainName, _strPassword, LogonSessionType.NewCredentials, LogonProvider.Default, out _objUserHandle);
    //            if (blnIsOk)
    //            {
    //                blnIsOk = DuplicateToken(_objUserHandle, 2, ref _objDupUserHandle);
    //                if (blnIsOk)
    //                {
    //                    WindowsIdentity identity = new WindowsIdentity(_objDupUserHandle);
    //                    WindowsImpersonationContext _objContext = identity.Impersonate();
    //                    _strMessage = "OK";
    //                }
    //                else
    //                {
    //                    intWin32Error = Marshal.GetLastWin32Error();
    //                }
    //            }
    //            else
    //            {
    //                intWin32Error = Marshal.GetLastWin32Error();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            _strMessage = "NG : SU(EXCEPTION) : " + e.Message;
    //            Console.WriteLine($"■SasaLib.Winlogon.ClsLogon.Logon(...) 例外発生 {_strMessage}");
    //        }

    //        if (0 != intWin32Error)
    //        {
    //            strErrDesc = objWinError.GetWinErrMessage(intWin32Error);

    //            if (debugConsoleMsg)
    //            {
    //                Console.WriteLine($"■SasaLib.Winlogon.ClsLogon.Logon(...) intWin32Error発生: {strErrDesc}");
    //            }
    //        }

    //        _strMessage += " : SU(" + intWin32Error + ")";

    //        if (!String.IsNullOrEmpty(strErrDesc))
    //        {
    //            _strMessage += " : " + strErrDesc;

    //            if (debugConsoleMsg)
    //            {
    //                Console.WriteLine($"■SasaLib.Winlogon.ClsLogon.Logon(...) 例外発生とintWin32Error発生: {_strMessage}");
    //            }
    //        }
    //        return blnIsOk;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public Boolean Logoff()
    //    {
    //        Boolean blnIsOk = true;
    //        try
    //        {
    //            if (null != _objContext) _objContext.Undo();
    //            if (IntPtr.Zero != _objDupUserHandle) CloseHandle(_objDupUserHandle);
    //            if (IntPtr.Zero != _objUserHandle) CloseHandle(_objUserHandle);
    //            _strMessage = "OK : EXIT SU(0)";
    //        }
    //        catch (Exception e)
    //        {
    //            blnIsOk = false;
    //            _strMessage = "NG : EXIT SU(EXCEPTION) : " + e.Message;
    //        }
    //        return blnIsOk;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        if (thisUse)
    //        {
    //            bool result = Logoff();
    //        }
    //    }
    //}

    public class ClsLogon
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        public ClsLogon(string domainName, string userName, string password, bool UsingClsLogon = false, bool debugConsoleMsg = false, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (UsingClsLogon == false)
                return;

            const int LOGON32_PROVIDER_DEFAULT = 0;
            //This parameter causes LogonUser to create a primary token.   
            const int LOGON32_LOGON_INTERACTIVE = 2;


            // Call LogonUser to obtain a handle to an access token.   
            SafeAccessTokenHandle safeAccessTokenHandle;
            bool returnValue = LogonUser(userName, domainName, password,
                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                out safeAccessTokenHandle);

            // Impersonate the specified user and execute the action
            WindowsIdentity.RunImpersonated(
               safeAccessTokenHandle,
                () =>
                {
                    // ここに実行したいアクションを記述
                    Console.WriteLine($"I'm running as {WindowsIdentity.GetCurrent().Name}");
                }
            );
        }
    }
}
