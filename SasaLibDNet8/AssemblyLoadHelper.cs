using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib;

/// <summary>
/// 
/// </summary>
public static class AssemblyLoadHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logFilePath"></param>
    public static void LoggerSetup(string logFilePath = "assembly_load_log.log")
    {

        // イベントハンドラ登録
        AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
        {
            System.IO.File.AppendAllText(logFilePath, $"[成功] アセンブリ: {args.LoadedAssembly.FullName}\n");
        };

        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            System.IO.File.AppendAllText(logFilePath, $"[失敗] アセンブリ: {args.Name}\n");
            return null; // 解決できない場合
        };

        // 例: アセンブリをロード
        try
        {
            Assembly.Load("NonExistentAssembly");
        }
        catch (Exception ex)
        {
            System.IO.File.AppendAllText(logFilePath, $"[例外] {ex.Message}\n");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="MethodName"></param>
    public static void CheckMethod(string MethodName)
    {
        // 任意のクラスとそのメソッドを取得する例
        var method = typeof(Console).GetMethod(MethodName, new[] { typeof(string) });

        if (method != null)
        {
            // 提供元アセンブリを取得
            Assembly assembly = method.DeclaringType.Assembly;

            // アセンブリ情報を出力
            DebugConsole.WriteLine($"メソッド: {method.Name}");
            DebugConsole.WriteLine($"所属クラス: {method.DeclaringType.FullName}");
            DebugConsole.WriteLine($"提供元アセンブリ: {assembly.FullName}");
            DebugConsole.WriteLine($"アセンブリの場所: {assembly.Location}");
        }
    }
}

