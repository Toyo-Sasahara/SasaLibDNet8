using System;
using System.Collections;
using System.Collections.Generic;
using CommandLine;

// CommandLineParser
// https://qiita.com/kawaidainfinity/items/0aeace9a49b9770e2545

namespace SasaLib.SysConfigurator;

internal class Options
{
    [Option('A', "Add", Required = false, HelpText = "スクリプトをターゲットファイル追加します")]
    public bool Add { get; set; }

    [Option('R', "Remove", Required = false, HelpText = "スクリプトをターゲットファイルから除去します")]
    public bool Remove { get; set; }

    
    [Option("TargetFilename", Required = true, HelpText = "必須オプション。変更するターゲットファイル名をフルパスで指定する 例： --TargetFilename \"C:\\Program Files\\Autodesk\\AutoCAD 2022\\Acadm\\acaddoc.lsp\"")]
    public string TargetFilename { get; set; }

    [Option("ScriptFilename", Required = true, HelpText = "必須オプション。追加・除去するスクリプトファイル名をフルパスで指定する。ファイル名のみ指定のは実行ファイルと同じディレクトリにあるファイルが指定される 例： --AddedFilename \"bootstap.txt\"")]
    public string ScriptFilename { get; set; }

    ////
    //[Option("Pipe", Required = false, HelpText = "オプション。ステージサーバーの接続先Pipe名を指定。無指定の場合は \"Approvalserer\" が使用される 例： --Pipe PIPETEST")]
    //public string PipeName { get; set; }

    ////
    //[Option('U', "User", Required = true, HelpText = "必須オプション。アークスイートユーザーログイン名を指定。例： -U goto")]
    //public string ArcSuiteUserID { get; set; }

    ////
    //[Option('P', "Password", Required = true, HelpText = "必須オプション。アークスイートユーザーパスワードを指定。例： -P toyokikai")]
    //public string ArcSuiteUserPass { get; set; }

    ////
    //[Option('N', "Number", Required = true, HelpText = "必須オプション。図面番号を指定。例： -P M-12345-320RL")]
    //public string PARTNUMBER { get; set; }



    //
    [Value(1, MetaName = "Others")]
    public IEnumerable<string> Others { get; set; }

}
