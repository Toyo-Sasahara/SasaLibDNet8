using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SasaLib
{
    public static class UACutils
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(HandleRef hWnd,
        uint Msg, IntPtr wParam, IntPtr lParam);
        private const int BCM_FIRST = 0x1600;
        private const int BCM_SETSHIELD = BCM_FIRST + 0x000C;

        /// <summary>
        /// UACの盾アイコンをボタンコントロールに表示（あるいは、非表示）する
        /// </summary>
        /// <param name="targetButton">盾アイコンを表示するボタンコントロール</param>
        /// <param name="showShield">盾アイコンを表示する時はtrue。
        /// 非表示にする時はfalse1。</param>
        public static void SetShieldIcon(Button targetButton, bool showShield)
        {
            if (targetButton == null)
            {
                throw new ArgumentNullException("targetButton");
            }

            //Windows Vista以上か確認する
            if (Environment.OSVersion.Platform != PlatformID.Win32NT ||
                Environment.OSVersion.Version.Major < 6)
            {
                return;
            }

            //FlatStyleをSystemにする
            targetButton.FlatStyle = FlatStyle.System;

            //盾アイコンを表示（または非表示）にする
            SendMessage(new HandleRef(targetButton, targetButton.Handle),
                BCM_SETSHIELD,
                IntPtr.Zero,
                showShield ? new IntPtr(1) : IntPtr.Zero);
        }

        /// <summary>
        /// UACの盾アイコンをボタンコントロールに表示する
        /// </summary>
        /// <param name="targetButton">盾アイコンを表示するボタンコントロール</param>
        public static void SetShieldIcon(Button targetButton)
        {
            SetShieldIcon(targetButton, true);
        }

        /// <summary>
        /// 自分自身を管理者として起動する
        /// </summary>
        public static void Adminstart()
        {

            //管理者として自分自身を起動する
            System.Diagnostics.ProcessStartInfo psi =
                new System.Diagnostics.ProcessStartInfo();
            //ShellExecuteを使う。デフォルトtrueなので、必要はない。
            psi.UseShellExecute = true;
            //自分自身のパスを設定する
            psi.FileName = Application.ExecutablePath;
            //動詞に「runas」をつける
            psi.Verb = "runas";

            try
            {
                //起動する
                System.Diagnostics.Process.Start(psi);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                //「ユーザーアカウント制御」ダイアログでキャンセルされたなどによって
                //起動できなかった時
                Console.WriteLine("起動しませんでした: " + ex.Message);
            }


        }


        //using System.Runtime.InteropServices;

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr TokenHandle,
            TOKEN_INFORMATION_CLASS TokenInformationClass,
            IntPtr TokenInformation,
            uint TokenInformationLength,
            out uint ReturnLength);

        public enum TOKEN_INFORMATION_CLASS
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
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            MaxTokenInfoClass
        }

        public enum TOKEN_ELEVATION_TYPE
        {
            TokenElevationTypeDefault = 1,
            TokenElevationTypeFull,
            TokenElevationTypeLimited
        }

        /// <summary>
        /// 昇格トークンの種類を取得する
        /// </summary>
        /// <returns>昇格トークンの種類を示すTOKEN_ELEVATION_TYPE。
        /// 取得に失敗した時でもTokenElevationTypeDefaultを返す。</returns>
        public static TOKEN_ELEVATION_TYPE GetTokenElevationType()
        {
            TOKEN_ELEVATION_TYPE returnValue =
                TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;

            //Windows Vista以上か確認
            if (Environment.OSVersion.Platform != PlatformID.Win32NT ||
                Environment.OSVersion.Version.Major < 6)
            {
                return returnValue;
            }

            TOKEN_ELEVATION_TYPE tet =
                TOKEN_ELEVATION_TYPE.TokenElevationTypeDefault;
            uint returnLength = 0;
            uint tetSize = (uint)Marshal.SizeOf((int)tet);
            IntPtr tetPtr = Marshal.AllocHGlobal((int)tetSize);
            try
            {
                //アクセストークンに関する情報を取得
                if (GetTokenInformation(
                    System.Security.Principal.WindowsIdentity.GetCurrent().Token,
                    TOKEN_INFORMATION_CLASS.TokenElevationType,
                    tetPtr, tetSize, out returnLength))
                {
                    //結果を取得
                    returnValue = (TOKEN_ELEVATION_TYPE)Marshal.ReadInt32(tetPtr);
                }
            }
            finally
            {
                //解放する
                Marshal.FreeHGlobal(tetPtr);
            }

            return returnValue;
        }
    }

    public static  class AppAuthority
    {
        /// <summary>
        /// アプリケーションの権限を確認する
        /// </summary>
        /// <returns>true なら管理者権限</returns>
        public static bool IsAdministrator()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
    }
}
