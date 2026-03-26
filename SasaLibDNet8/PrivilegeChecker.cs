using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Principal;

namespace SasaLib
{
    /// <summary>
    /// 
    /// </summary>
#if NETCOREAPP
    [SupportedOSPlatform("windows")]
#endif
    public static class PrivilegeChecker
    {
        /// <summary>
        /// 現在のユーザーが Administrators グループに所属しているかを判定
        /// </summary>
        public static bool IsInLocalAdministratorsGroup()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// 現在のプロセスがUAC昇格済みかを判定
        /// 管理者として実行されているかを見たい場合はこちらが実用的
        /// </summary>
        public static bool IsProcessElevated()
        {
            IntPtr tokenHandle = IntPtr.Zero;
            try
            {
                if (!OpenProcessToken(GetCurrentProcess(), TOKEN_QUERY, out tokenHandle))
                {
                    return false;
                }

                TOKEN_ELEVATION elevation;
                int size;
                int elevationSize = Marshal.SizeOf(typeof(TOKEN_ELEVATION));

                IntPtr elevationPtr = Marshal.AllocHGlobal(elevationSize);
                try
                {
                    if (!GetTokenInformation(
                        tokenHandle,
                        TOKEN_INFORMATION_CLASS.TokenElevation,
                        elevationPtr,
                        elevationSize,
                        out size))
                    {
                        return false;
                    }

                    elevation = Marshal.PtrToStructure<TOKEN_ELEVATION>(elevationPtr);
                    return elevation.TokenIsElevated != 0;
                }
                finally
                {
                    Marshal.FreeHGlobal(elevationPtr);
                }
            }
            finally
            {
                if (tokenHandle != IntPtr.Zero)
                {
                    CloseHandle(tokenHandle);
                }
            }
        }

        /// <summary>
        /// 実質的に「現在このプロセスが管理者権限で動いているか」
        /// を判定したいなら、通常はこちらを使う
        /// </summary>
        public static bool HasUsableAdministratorPrivileges()
        {
            return IsProcessElevated();
        }

        #region Win32

        private const uint TOKEN_QUERY = 0x0008;

        private enum TOKEN_INFORMATION_CLASS
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType = 18,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TOKEN_ELEVATION
        {
            public int TokenIsElevated;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(
            IntPtr processHandle,
            uint desiredAccess,
            out IntPtr tokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool GetTokenInformation(
            IntPtr tokenHandle,
            TOKEN_INFORMATION_CLASS tokenInformationClass,
            IntPtr tokenInformation,
            int tokenInformationLength,
            out int returnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        #endregion
    }
}
