using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SasaLib.SysConfigurator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("実行ファイル コンフィギュファイル ベースディレクトリ");
            }
            else
            {
                Console.WriteLine($"args[0] (コンフィギュファイル)= {args[0]}");
                Console.WriteLine($"args[1] （ベースディレクトリ）= {args[1]}");

                TestRun.Execute(args[0] , args[1]);
            }
        }
    }
}
