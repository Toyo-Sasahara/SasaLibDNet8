using System.Runtime.InteropServices;
using System.Text;

class Program
{
    private static NETRESOURCE netResource;

    [DllImport("mpr.dll", EntryPoint = "WNetAddConnection2", CharSet = CharSet.Unicode)]
    private static extern int WNetAddConnection2(ref NETRESOURCE lpNetResource, string lpPassword, string lpUsername, Int32 dwFlags);

    [DllImport("mpr.dll", EntryPoint = "WNetCancelConnection2", CharSet = CharSet.Unicode)]
    private static extern int WNetCancelConnection2(string lpName, Int32 dwFlags, bool fForce);


    /// <summary>UNCパス</summary>
    private static string UncPath { get; set; }
    /// <summary>ユーザーID</summary>
    private static string UserID { get; set; }
    /// <summary>パスワード</summary>
    private static string Password { get; set; }


    static void Main(string[] args)
    {
        bool retryUntilSuccess = Array.Exists(args, arg => arg.Equals("-r", StringComparison.OrdinalIgnoreCase));

        Console.Write("共有フォルダのパスを入力してください（例：\\\\SERVER\\SHARE）：");
        UncPath = Console.ReadLine();

        var netResource = new NETRESOURCE
        {
            lpRemoteName = UncPath
        };

        bool success = false;

        do
        {
            Console.Write("ユーザー名（例：DOMAIN\\user）：");
            UserID = Console.ReadLine();

            Console.Write("パスワード：");
            Password = ReadPassword();

            Console.WriteLine("\n接続を試行中...");

            int result = TryConnect();

            if (result == 0)
            {
                Console.WriteLine("✅ 接続に成功しました。");
                success = true;

                // クリーンアップ
                Console.WriteLine("切断中...");
                TryDisConnect();
            }
            else
            {
                Console.WriteLine($"❌ 接続に失敗しました（{WinApiHelper.GetErrorMessage(result)}）");

                if (!retryUntilSuccess)
                    break;
                else
                    Console.WriteLine("もう一度入力してください。\n");
            }

        } while (!success);

        Console.WriteLine("終了するには Enter を押してください。");
        Console.ReadLine();
    }

    /// <summary>
    /// ネットワーク情報
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct NETRESOURCE
    {
        public int dwScope;
        public int dwType;
        public int dwDisplayType;
        public int dwUsage;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpLocalName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpRemoteName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpComment;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpProvider;
    }

    /// <summary>
    /// サーバーに接続する
    /// </summary>
    /// <returns>結果 (true : 成功、false : 失敗)</returns>
    public static int TryConnect()
    {
        try
        {
            netResource.dwScope = 0;                    // 列挙の範囲
            netResource.dwType = 1;                     // リソースタイプ
            netResource.dwDisplayType = 0;              // 表示オブジェクト
            netResource.dwUsage = 0;                    // リソースの使用方法
            netResource.lpLocalName = null;             // ローカルデバイス名
            netResource.lpRemoteName = UncPath;         // UNCパス
            netResource.lpProvider = null;              // プロバイダ名

            // 既に接続済みの場合がある為、一旦接続を解除する
            WNetCancelConnection2(UncPath, 0, true);

            // 接続
            int ret = WNetAddConnection2(ref netResource, Password, UserID, 0);

            return ret;
        }
        catch (Exception)
        {
            // エラーハンドル処理 (省略)
            return -1;
        }
    }

    /// <summary>
    /// サーバーへの接続を解除する
    /// </summary>
    /// <returns>結果 (true : 成功、false : 失敗)</returns>
    public static void TryDisConnect()
    {
        try
        {
            WNetCancelConnection2(UncPath, 0, true);
        }
        catch (Exception)
        {
            // エラーハンドル処理 (省略)
        }
    }


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

public static class WinApiHelper
{
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
}

