// コマンドライン引数のオプションを定義
using CommandLine;

public class Options
{
    [Option('c', "control-file", Required = true, HelpText = "コントロールファイルのパスを指定")]
    public string ControlFullFileName { get; set; }

    [Option('b', "base-folder", Required = true, HelpText = "コントロールファイルが認識するベースフォルダを指定")]
    public string RootFolder { get; set; }

    [Option('l', "logging-folder", Required = false, HelpText = "ログ出力先フォルダ")]
    public string LoggingFolder { get; set; }

    [Option('v', "verbose", Default = false, HelpText = "詳細表示")]
    public bool Verbose { get; set; }
}
