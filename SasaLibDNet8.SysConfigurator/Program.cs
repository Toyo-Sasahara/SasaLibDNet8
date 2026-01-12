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
        public static bool VerboseMode { get; set; }

        public static void Main(string[] args)
        {
            Parser parser = new Parser(c =>
            {
                c.CaseSensitive = false;             // 小文字対象
                c.HelpWriter = Console.Error;
            });


            // コマンドライン引数を解析
            var parseResult = parser.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptions(opts)) // 成功時
                .WithNotParsed(errs => HandleParseError(errs));
        }

        private static void RunOptions(Options opts)
        {
            Console.WriteLine($"必須オプションを確認。スクリプトを実行します。");

            Console.WriteLine($"-c, --control-file = \"{opts.ControlFullFileName}\"");
            Console.WriteLine($"-b, --base-folder = \"{opts.RootFolder}\"");
            if (string.IsNullOrWhiteSpace(opts.LoggingFolder) == false)
                Console.WriteLine($"-l, --logging-folder = \"{opts.LoggingFolder}\"");
            else
                Console.WriteLine($"-l, --logging-folder 指定なし。--control-file のフォルダが指定されます");

            Console.WriteLine($"-v, --verbose = \"{opts.Verbose}\"");

            if (opts.Verbose)
                VerboseMode = true;
            else
                VerboseMode = false;

            TestRun.Execute(opts.ControlFullFileName, opts.RootFolder, opts.LoggingFolder);


        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Error parsing command-line arguments:");
            foreach (var err in errs)
            {
                Console.WriteLine(err.ToString());
            }
        }
    }
}
