using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.SysConfigurator
{
    public delegate bool Delegate_RemoteServerGeFileList(string ServerSourceFolder, string seachPattern, out List<string> files, Action<string> WriteLine);

    [SupportedOSPlatform("windows")]
    public class PrepareSysConfigurator
    {
        /// <summary>
        /// 
        ///  = @"C:\Users\Public\Documents\TOYOCOMMON\InventorTOYOaddin";
        /// </summary>
        public string configWorkFolder { get; private set; }

        /// <summary>
        /// 
        ///  = @"*.conf";
        /// </summary>
        public string SysConfiguratorFilePattern { get; private set; }

        /// <summary>
        /// 
        ///  = "2022";
        /// </summary>
        public byte[] AESkey { get; private set; }

        /// <summary>
        /// 
        /// = "2022"
        /// </summary>
        public string CadVersonString { get; private set; }

        /// <summary>
        /// 
        /// 
        /// </summary>
        public Delegate_RemoteServerGeFileList delegate_RemoteServerGeFileList { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Delegate_FileCopy delegate_FileCopy { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Action<string> delegate_WriteLine { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerSideInstructionsToToyoAddinFolder { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ServerControlFileFolder">ｺﾝﾄﾛｰﾙﾌｧｲﾙﾀﾞｳﾝﾛｰﾄﾞ先ｻｰﾊﾞｰ起点とする、コントロールファイルがあるフォルダ 例:C:\ProgramData\TOYOCOMMON\Inventor</param>
        /// <param name="LocalConfigWorkFolder">ローカルＰＣの 作業フォルダ 例:C:\Users\Public\Documents\TOYOCOMMON\InventorTOYOaddin</param>
        /// <param name="SysConfiguratorFilePattern">コントローファイルの検索パターン 例：ローカルＰＣの検索</param>
        /// <param name="AESkey"></param>
        /// <param name="VersonString"></param>
        /// <param name="delegate_RemoteServerGeFileList">デリゲート先</param>
        /// <param name="delegate_FieCopy">デリゲート先</param>
        /// <param name="WriteLine">デリゲート先</param>
        public PrepareSysConfigurator(
            string ServerControlFileFolder,
            string LocalConfigWorkFolder,
            string SysConfiguratorFilePattern,
            byte[] AESkey,
            string VersonString,
            Delegate_RemoteServerGeFileList delegate_RemoteServerGeFileList,
            Delegate_FileCopy delegate_FieCopy,
            Action<string> WriteLine
            )
        {
            this.ServerSideInstructionsToToyoAddinFolder = ServerControlFileFolder;
            this.configWorkFolder = LocalConfigWorkFolder;
            this.SysConfiguratorFilePattern = SysConfiguratorFilePattern;
            this.AESkey = AESkey;
            this.CadVersonString = VersonString;
            this.delegate_RemoteServerGeFileList = delegate_RemoteServerGeFileList;
            this.delegate_FileCopy = delegate_FieCopy;
            this.delegate_WriteLine = WriteLine;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="forceExecute"></param>
        /// <returns></returns>
        public bool Execute(out TimeSpan ts, bool forceExecute = false, Action<string> WriteLine = null)
        {
            if (WriteLine != null)
                this.delegate_WriteLine = WriteLine;

            // 作業ベースフォルダ,ログ出力先ﾍﾞｰｽﾌｫﾙﾀﾞ
            var BaseDir = System.IO.Path.Combine(configWorkFolder, "[" + CadVersonString + "]");
            FileFolder.MakeDirectory(BaseDir.TrimEnd('\\'));

            // サンプル作成
            new SysConfiguration().CreateSmapleConfig(BaseDir, delegate_WriteLine);

            // サーバー側設定ファイルﾌｫﾙﾀﾞ
            var ServerSourceFolder = System.IO.Path.Combine(ServerSideInstructionsToToyoAddinFolder, "[" + CadVersonString + "]");

            // ファイル一覧を得ます
            delegate_WriteLine($"ファイル一覧を得ます。パス:\"{ServerSourceFolder}\" 検索条件:\"{SysConfiguratorFilePattern}\"");
            List<string> SysConfiguratorFiles = new List<string>();
            bool resultGetFiles = delegate_RemoteServerGeFileList(ServerSourceFolder, SysConfiguratorFilePattern, out SysConfiguratorFiles, delegate_WriteLine);

            // ■■AdditionalToyoAddinInstruction.Execute()により追加指示を実行
            // Stopwatchクラス生成・計測開始
            var sw = new System.Diagnostics.Stopwatch(); sw.Start();

            if (resultGetFiles == true)
            {

                delegate_WriteLine($"■PrepareSysConfigurator 該当件数は{SysConfiguratorFiles.Count} 件あります。");

                int i = 0;
                foreach (var aa in SysConfiguratorFiles) delegate_WriteLine($"■{++i}. \"{System.IO.Path.GetFileName(aa)}\"");

                SysConfigurator.XML_Control xML_ControlObj = new SysConfigurator.XML_Control(AESkey, BaseDir);

                /// ﾃﾞﾘｹﾞｰﾄメソッド定義・・リモートサーバーよりFileRecv使用。
                xML_ControlObj.DelegateFileCopyFunc = delegate_FileCopy;


                foreach (var SysConf in SysConfiguratorFiles)
                {
                    delegate_WriteLine($"■SasaLib.SysConfigurator.PrepareSysConfigurator(..) サーバー側よりダウンロード実行: \"{System.IO.Path.GetFileName(SysConf)}\" ");

                    // サーバーからDLしてきた実行ﾌｧｲﾙが入る
                    string DownloadedSysConfiguratorFullFileName;

                    // ジョブファイルをロード
                    bool addwork = RemoteServerControlFileLoad(SysConf, configWorkFolder, CadVersonString, out DownloadedSysConfiguratorFullFileName, delegate_WriteLine);


                    var executeResult = xML_ControlObj.JobLoadAndExecute(DownloadedSysConfiguratorFullFileName, BaseDir, delegate_WriteLine, forceExecute, IsRemoveControlFile: true);

                    if (executeResult)
                    {
                        delegate_WriteLine($"■SasaLib.SysConfigurator.PrepareSysConfigurator(..) 追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ \"{System.IO.Path.GetFileName(SysConf)}\" の 処理の結果: {executeResult} でした");
                    }
                    else
                    {
                        delegate_WriteLine($"※SasaLib.SysConfigurator.PrepareSysConfigurator(..)r 追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ \"{System.IO.Path.GetFileName(SysConf)}\" の 処理の結果: {executeResult} でした");
                    }
                    delegate_WriteLine($"-- --");
                }


                sw.Stop(); ts = sw.Elapsed; // 計測終了
                delegate_WriteLine($"■SasaLib.SysConfigurator.Execute(..) <<処理(A)経過時間:{ts.Hours} 時間{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>>");
                delegate_WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                return true;
            }
            else
            {
                ts = new TimeSpan();
                delegate_WriteLine($"※アクセスできませんでした。パス:\"{BaseDir}\" 検索条件:\"{SysConfiguratorFilePattern}\"");
                return false;
            }
        }

        private bool RemoteServerControlFileLoad(string ServerSourceFullfilename, string InstructionsAddinLocalFileSotreFolder, string vernumber, out string InstructionsVersionToyoAddinFullFileName, Action<string> WriteLine)
        {
            // Stopwatchクラス生成・計測開始
            var initilaizeSw = new System.Diagnostics.Stopwatch(); initilaizeSw.Start();
            WriteLine($"■PrepareSysConfigurator.RemoteServerControlFileLoad(..)開始 <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> サーバーから 追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ \"{ServerSourceFullfilename}\" の読出しを開始");

            // バージョン番号フォルダを埋め込んだ サーバー側フォルダ名
            string InstructionsVersionToyoAddinFullFileNameSeverSource = ServerSourceFullfilename;

            // バージョン番号フォルダを埋め込んだ ローカル側フルファイル名
            InstructionsVersionToyoAddinFullFileName = System.IO.Path.Combine(InstructionsAddinLocalFileSotreFolder, "[" + vernumber + "]", System.IO.Path.GetFileName(ServerSourceFullfilename));

            try
            {

                string folder = System.IO.Path.Combine(InstructionsAddinLocalFileSotreFolder, "[" + vernumber + "]");
                var result1 = FileFolder.MakeDirectory(folder);
                if (result1)
                    WriteLine($"■PrepareSysConfigurator.RemoteServerControlFileLoad(..) フォルダ作成成功 \"{folder}\" <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>>");
                else
                    WriteLine($"※PrepareSysConfigurator.RemoteServerControlFileLoad(..) フォルダ作成失敗 \"{folder}\" <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>>");

                bool getans = delegate_FileCopy(InstructionsVersionToyoAddinFullFileNameSeverSource, InstructionsVersionToyoAddinFullFileName, WriteLine);


                if (getans == false)
                {
                    WriteLine(
                        $"※Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙの取得に失敗 <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> (ｻｰﾊﾞｰ側:{InstructionsVersionToyoAddinFullFileNameSeverSource}," +
                        $" ﾛｰｶﾙPC保存先:{InstructionsVersionToyoAddinFullFileName})");

                    if (System.IO.File.Exists(InstructionsVersionToyoAddinFullFileName))
                    {
                        var ans = FileFolder.RemoveFile(InstructionsVersionToyoAddinFullFileName);
                        if (ans)
                            WriteLine($"※既存のAddin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ \"{InstructionsVersionToyoAddinFullFileName}\" を削除しました");
                        else
                        {
                            WriteLine($"※既存のAddin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ \"{InstructionsVersionToyoAddinFullFileName}\" の削除に失敗しました");
                        }
                    }

                    WriteLine($"※PrepareSysConfigurator.RemoteServerControlFileLoad(..)終了 <<経過時間:{initilaizeSw.Elapsed.Hours}時間 {initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> ｻｰﾊﾞｰからAddin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙの取得に失敗しています ");

                    return false;
                }
                else
                {
                    WriteLine($"■PrepareSysConfigurator.RemoteServerControlFileLoad(..)終了 <<経過時間:{initilaizeSw.Elapsed.Hours}時間 {initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> ｻｰﾊﾞｰからAddin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙを　\"{InstructionsVersionToyoAddinFullFileName}\" へ保存完了しました");

                    return true;
                }
            }
            catch (System.Exception ex)
            {
                WriteLine($"※PrepareSysConfigurator.RemoteServerControlFileLoad(..)終了 <<経過時間:{initilaizeSw.Elapsed.Hours}時間 {initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>>  Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ のｻｰﾊﾞｰからのロードにて例外検知 {ex.Message}");
                return false;
            }

        }

    }
}
