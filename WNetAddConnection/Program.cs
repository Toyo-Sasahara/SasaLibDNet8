using System;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    [StructLayout(LayoutKind.Sequential)]
    public class NETRESOURCE
    {
        public int dwScope = 0;
        public int dwType = 1; // RESOURCETYPE_DISK
        public int dwDisplayType = 0;
        public int dwUsage = 0;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpLocalName = null;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpRemoteName = null;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpComment = null;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpProvider = null;
    }

    [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
    private static extern int WNetAddConnection2(
        NETRESOURCE lpNetResource,
        string lpPassword,
        string lpUserName,
        int dwFlags);

    [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
    private static extern int WNetCancelConnection2(string lpName, int dwFlags, bool fForce);

    static void Main(string[] args)
    {
        bool retryUntilSuccess = Array.Exists(args, arg => arg.Equals("-r", StringComparison.OrdinalIgnoreCase));
        bool checkWrite = Array.Exists(args, arg => arg.Equals("-rw", StringComparison.OrdinalIgnoreCase));

        Console.Write("共有フォルダのパスを入力してください（例：\\\\SERVER\\SHARE）：");
        string remotePath = Console.ReadLine();

        NETRESOURCE netResource = new NETRESOURCE
        {
            dwScope = 0, // RESOURCESCOPETYPE_GLOBAL
            dwType = 1, // RESOURCETYPE_DISK
            dwDisplayType = 0, // RESOURCEDISPLAYTYPE_GENERIC
            dwUsage = 0, // RESOURCEUSAGE_CONNECTABLE
            lpLocalName = null, // ローカル名は不要           
            lpRemoteName = remotePath,
            lpProvider = null // プロバイダは不要   
        };

        bool success = false;


        do
        {
            Console.Write("ユーザー名（例：DOMAIN\\user）：");
            string UserID = Console.ReadLine();

            Console.Write("パスワード：");
            string password = ReadPassword();


            // 既に接続済みの場合がある為、一旦接続を解除する
            WNetCancelConnection2(remotePath, 0, true);
            
            Console.WriteLine("\n接続を試行中...");

            int result = WNetAddConnection2(netResource, password, UserID, 0);

            if (result == 0)
            {
                Console.WriteLine("✅ 接続に成功しました。");
                success = true;

                if (checkWrite)
                {
                    try
                    {
                        string dummyFilePath = System.IO.Path.Combine(remotePath, "dummy_write_test.tmp");
                        Console.WriteLine($"書き込みテスト: {dummyFilePath}");

                        // ファイル書き込み
                        System.IO.File.WriteAllText(dummyFilePath, "write test");

                        // 所有者取得
                        var fileInfo = new System.IO.FileInfo(dummyFilePath);
                        var fileSecurity = fileInfo.GetAccessControl();
                        var owner = fileSecurity.GetOwner(typeof(System.Security.Principal.NTAccount));
                        Console.WriteLine($"✅ 書き込みに成功しました。所有者: {owner.Value}");

                        // 削除
                        System.IO.File.Delete(dummyFilePath);
                        Console.WriteLine("ファイルを削除しました。");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ 書き込みに失敗しました：{ex.Message}");
                    }
                }


                // クリーンアップ
                Console.WriteLine("切断中...");
                WNetCancelConnection2(remotePath, 0, true);
            }
            else
            {
                Console.WriteLine($"❌ 接続に失敗しました（{GetErrorMessage(result)})");

                if (!retryUntilSuccess)
                    break;
                else
                    Console.WriteLine("もう一度入力してください。\n");
            }

        } while (!success);

        Console.WriteLine("終了するには Enter を押してください。");
        Console.ReadLine();
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern int FormatMessage(
    int flags, IntPtr source, int messageId, int languageId,
    [Out] System.Text.StringBuilder buffer, int size, IntPtr arguments);

    private const int FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
    private const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;

    public static string GetErrorMessage(int errorCode)
    {
        var buffer = new System.Text.StringBuilder(512);
        int result = FormatMessage(
            FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
            IntPtr.Zero,
            errorCode,
            0, // 自動言語選択（ロケールID）
            buffer,
            buffer.Capacity,
            IntPtr.Zero);

        if (result > 0)
        {
            return buffer.ToString().Trim(); // 改行や末尾スペースを除去
        }
        else
        {
            return $"不明なエラーコード: {errorCode}";
        }
    }

    // パスワード非表示入力用
    static string ReadPassword()
    {
        var password = new StringBuilder();
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Length--;
                Console.Write("\b \b");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
        }
        return password.ToString();
    }
}
