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

            var parseResult = parser.ParseArguments<Options>(args);

            // コマンドライン引数を解析
            parser.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptions(opts)) // 成功時
                .WithNotParsed(errs => HandleParseError(errs));
        }

        private static void RunOptions(Options opts)
        {


            Console.WriteLine($"必須オプションを確認。スクリプトを実行します。");

            Console.WriteLine($"--control-file = \"{opts.ControlFullFileName}\"");
            Console.WriteLine($"--base-folder = \"{opts.RootFolder}\"");

            if (opts.Verbose)
                VerboseMode = true;
            else
                VerboseMode = false;

            TestRun.Execute(opts.ControlFullFileName, opts.RootFolder);


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
