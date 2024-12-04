// コマンドライン引数のオプションを定義
using CommandLine;

public class Options
{
    [Option('c', "control-file", Required = true, HelpText = "コントロールファイル")]
    public string ControlFullFileName { get; set; }

    [Option('b', "base-folder", Required = true, HelpText = "コピー元ベースフォルダ")]
    public string RootFolder { get; set; }

    [Option('v', "verbose", Default = false, HelpText = "詳細表示")]
    public bool Verbose { get; set; }
}
