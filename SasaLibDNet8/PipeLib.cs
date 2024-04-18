using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace SasaLib.PIPE
{
    /// <summary>
    /// 名前付きパイプサーバーのストリーム作成
    /// </summary>
    public static class NamedPipeServerStreamFactory
    {
        /// <summary>
        /// パイプストリーム本命 コマンドラインAdministratorで同時起動可能2019-04-07時点
        /// </summary>
        /// <param name="piepName"></param>
        /// <param name="maxInstances"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static NamedPipeServerStream Create(string piepName, int maxInstances = NamedPipeServerStream.MaxAllowedServerInstances)
        {
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);

            PipeSecurity ps = new PipeSecurity();

            ps.AddAccessRule(new PipeAccessRule("Users", PipeAccessRights.ReadWrite | PipeAccessRights.CreateNewInstance, AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule("CREATOR OWNER", PipeAccessRights.FullControl, AccessControlType.Allow));
            ps.AddAccessRule(new PipeAccessRule("SYSTEM", PipeAccessRights.FullControl, AccessControlType.Allow));

            ps.AddAccessRule(par);

            //return new NamedPipeServerStream(PipeName, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 2048, 2048, ps);
            var stream = new NamedPipeServerStream(piepName, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 2048, 2048);
            stream.SetAccessControl(ps);
        
            return stream;
        }

        /// <summary>
        /// ストリーム作成 本命 　 Administrator権限で動作させること　WindowsService で LocalSystemアカウント にて起動可能
        /// 但し同時起動はうまくいかない2018/11/20
        /// </summary>
        /// <param name="name"></param>
        /// <param name="maxInstances"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static NamedPipeServerStream Create2(string pipeName, int maxInstances = NamedPipeServerStream.MaxAllowedServerInstances)
        {
            #region ネットワークパイプ接続のためのセキュリティ指定
            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            PipeSecurity ps = new PipeSecurity();
            ps.AddAccessRule(par);
            #endregion

            //return new NamedPipeServerStream(name, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 1024, 1024, ps);

            var stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 2048, 2048);
            stream.SetAccessControl(ps);

            return stream;

        }
        #region testcode
        //test2
        public static NamedPipeServerStream Create3(string pipeName, int maxInstances = NamedPipeServerStream.MaxAllowedServerInstances)
        {
            PipeSecurity ps = new PipeSecurity();

            var a = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 1024, 1024);
            ps = a.GetAccessControl();

            SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
            PipeAccessRule par = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            ps.AddAccessRule(par);

            // return new NamedPipeServerStream(name, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 1024, 1024, ps);

            var stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 2048, 2048);
            stream.SetAccessControl(ps);

            return stream;

        }

        //test3
        public static NamedPipeServerStream Create4(string pineName, int maxInstances = NamedPipeServerStream.MaxAllowedServerInstances)
        {
            PipeSecurity ps = new PipeSecurity();

            ps.AddAccessRule(new PipeAccessRule($"{Environment.UserDomainName}\\{Environment.UserName}", PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));

            //return new NamedPipeServerStream(name, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 1024, 1024, ps);

            var stream = new NamedPipeServerStream(pineName, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 2048, 2048);
            stream.SetAccessControl(ps);

            return stream;

        }

        //test4
        public static NamedPipeServerStream Create5(string pipeName, int maxInstances = NamedPipeServerStream.MaxAllowedServerInstances)
        {
            return new NamedPipeServerStream(pipeName, PipeDirection.InOut, maxInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 1024, 1024);
        }
        #endregion
    }

    public static class NamedPipeClientInfo
    {
        /// <summary>
        /// 名前付きパイプのクライアント プロセス識別子を取得
        /// </summary>
        /// <param name="Pipe"></param>
        /// <param name="ClientProcessId"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetNamedPipeClientProcessId(IntPtr Pipe, out uint ClientProcessId);

        /// <summary>
        /// NamedPipeServerStreamに接続してきたクライアントのプロセス識別子を返す
        /// </summary>
        /// <param name="pipeServer"></param>
        /// <returns></returns>
        public static uint GetClientProcessID(NamedPipeServerStream pipeServer)
        {
            uint processId;
            IntPtr pipeHandle = pipeServer.SafePipeHandle.DangerousGetHandle();
            if (GetNamedPipeClientProcessId(pipeHandle, out processId))
            {
                return processId;
            }
            return 0;
        }

        /// <summary>
        /// 名前付きパイプのクライアント コンピューター名を取得
        /// </summary>
        /// <param name="Pipe"></param>
        /// <param name="ClientComputerName"></param>
        /// <param name="ClientComputerNameLength"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]       
        private static extern bool GetNamedPipeClientComputerName(IntPtr Pipe, StringBuilder ClientComputerName, uint ClientComputerNameLength);

        /// <summary>
        /// NamedPipeServerStreamに接続してきたクライアントのPC名返す(Dnsを引かない)
        /// </summary>
        /// <param name="pipeServer"></param>
        /// <returns></returns>
        public static string GetClientComputerName(NamedPipeServerStream pipeServer)
        {
            uint buffer = 32768;
            IntPtr pipeHandle = pipeServer.SafePipeHandle.DangerousGetHandle();

            var computerName = new StringBuilder();

            if (GetNamedPipeClientComputerName(pipeHandle, computerName, buffer))
            {
                return computerName.ToString();
            }
            else throw new Win32Exception();
        }

        /// <summary>
        /// NamedPipeServerStreamに接続してきたクライアントのPC名をDnsで調べて返す
        /// </summary>
        /// <param name="pipeServer"></param>
        /// <returns></returns>
        public static string GetClientDnsComputerName(NamedPipeServerStream pipeServer)
        {
            string DnsClienntComputerName;
            try
            {
                DnsClienntComputerName = Net.DnsGetHostName(GetClientComputerName(pipeServer)); // IPアドレスの場合ホスト名を返す

            }
            catch
            {
                DnsClienntComputerName = GetClientComputerName(pipeServer);
            }
            return DnsClienntComputerName;
        }

        /// <summary>
        /// NamedPipeServerStreamに接続してきたクライアントのユーザ名を返す
        /// </summary>
        /// <param name="pipeServer"></param>
        /// <returns></returns>
        public static string GetClientUserName(NamedPipeServerStream pipeServer)
        {
            // 接続してきたクライアントのユーザ名
            var ans = pipeServer.GetImpersonationUserName();
            return ans;
        }

        /// <summary>
        /// NamedPipeServerStreamに接続してきたクライアントの接続情報を文字列で得る
        /// </summary>
        /// <param name="serverId">任意のしきべつばんごう</param>
        /// <param name="pipeSrvStream"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static string GetClientHostAndUser(NamedPipeServerStream pipeSrvStream, int serverId = 0)
        {

            // パイプ終端のクライアントユーザー名
            string ImpersonationUserName = "";
            // 接続元PC
            string namedPipeClientComputerName = "";

            if (pipeSrvStream.IsConnected == false)
            {
                return "PIPE未接続";
            }

            try
            {
                // 接続してきたクライアントのユーザ名
                ImpersonationUserName = pipeSrvStream.GetImpersonationUserName();

                // 接続してきたクライアントのホスト名
                namedPipeClientComputerName = NamedPipeClientInfo.GetClientComputerName(pipeSrvStream);

                string DnsClienntComputerName = Net.DnsGetHostNameOrIP(namedPipeClientComputerName); // IPアドレスの場合ホスト名を返す

                return $"[接続元:\"{namedPipeClientComputerName}\"-\"{ImpersonationUserName}\"]";
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib.PIPE", EventLogEntryType.Error, 9700, $" ConectedPipeInformationMsg(...) [ServerID:{serverId}] 接続PC名:\"{namedPipeClientComputerName}\" , 接続ユーザー名:\"{ImpersonationUserName}\"にて 例外発生  {ex.Message}");

                return null;
            }
        }

    }

}

