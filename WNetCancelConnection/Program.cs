using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

class Program
{
    // Windows API を使ってネットワークドライブを切断
    [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
    private static extern int WNetCancelConnection2(string lpName, int dwFlags, bool fForce);

    static void Main()
    {
        Console.WriteLine("Win32API WNetCancelConnection2 を使いすべてのネットワークドライブを切断します。");

        var networkDrives = GetNetworkDrives();

        foreach (var drive in networkDrives)
        {
            Console.Write($"切断中：{drive} ... ");
            int result = WNetCancelConnection2(drive, 0, true);
            if (result == 0)
            {
                Console.WriteLine($"WNetCancelConnection2({drive},0,true)を実行・・・成功");
            }
            else
            {
                Console.WriteLine($"WNetCancelConnection2({drive},0,true)を実行・・・失敗（エラーコード: {result}）");
            }
        }

        Console.WriteLine("処理完了。Enterキーで終了。");
        Console.ReadLine();
    }

    static List<string> GetNetworkDrives()
    {
        var drives = new List<string>();
        foreach (var drive in Environment.GetLogicalDrives())
        {
            var driveType = GetDriveType(drive);
            if (driveType == DriveType.Network)
            {
                drives.Add(drive.TrimEnd('\\')); // "Z:\" → "Z:"
            }
        }
        return drives;
    }

    static System.IO.DriveType GetDriveType(string drive)
    {
        try
        {
            return new System.IO.DriveInfo(drive).DriveType;
        }
        catch
        {
            return DriveType.Unknown;
        }
    }
}
