using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CommandLine;

namespace SasaLib.SysConfigurator
{
    [SupportedOSPlatform("windows")]
    public static class Program
    {
        public static void Main(string[] args)
        {
            // パーサーで引数を解析
            //Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
            //    {
            //        // 引数が正しく解析された場合の処理
            //        Console.WriteLine($"Hello, {options.Name}!");
            //        if (options.Age.HasValue)
            //        {
            //            Console.WriteLine($"You are {options.Age} years old.");
            //        }
            //    }).WithNotParsed(errors =>
            //    {
            //        // 引数解析に失敗した場合の処理
            //        Console.WriteLine("Failed to parse arguments.");
            //    });

            if (args.Length < 2)
            {
                Console.WriteLine("実行ファイル コンフィギュファイル ベースディレクトリ");
            }
            else
            {
                Console.WriteLine($"args[0] (コンフィギュファイル)= {args[0]}");
                Console.WriteLine($"args[1] （ベースディレクトリ）= {args[1]}");

                TestRun.Execute(args[0], args[1]);
            }
        }
    }
}
