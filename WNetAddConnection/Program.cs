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
        public string lpLocalName = null;
        public string lpRemoteName = null;
        public string lpComment = null;
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

        Console.Write("共有フォルダのパスを入力してください（例：\\\\SERVER\\SHARE）：");
        string remotePath = Console.ReadLine();

        var netResource = new NETRESOURCE
        {
            lpRemoteName = remotePath
        };

        bool success = false;

        do
        {
            Console.Write("ユーザー名（例：DOMAIN\\user）：");
            string username = Console.ReadLine();

            Console.Write("パスワード：");
            string password = ReadPassword();

            Console.WriteLine("\n接続を試行中...");

            int result = WNetAddConnection2(netResource, password, username, 0);

            if (result == 0)
            {
                Console.WriteLine("✅ 接続に成功しました。");
                success = true;

                // クリーンアップ
                Console.WriteLine("切断中...");
                WNetCancelConnection2(remotePath, 0, true);
            }
            else
            {
                Console.WriteLine($"❌ 接続に失敗しました（エラーコード: {result}）");

                if (!retryUntilSuccess)
                    break;
                else
                    Console.WriteLine("もう一度入力してください。\n");
            }

        } while (!success);

        Console.WriteLine("終了するには Enter を押してください。");
        Console.ReadLine();
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
