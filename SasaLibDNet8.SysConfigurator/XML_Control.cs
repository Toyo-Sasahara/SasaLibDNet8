using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.SqlServer.Server;
using SasaLib;
using SasaLib.Eventlog;
using static System.Net.WebRequestMethods;

namespace SasaLib.SysConfigurator
{
    public enum WriteMode
    {
        NoOverwrite, ForceOverwrite,
    }

    public enum Attribute
    {
        ReadOnly, ReadWrite
    }


    public struct GetFulFileName
    {
        public string Comment;
        public string ServerSideFullFileName;
        public string LocalSideFullFileName;
        /// <summary>
        /// 既存ﾌｧｲﾙが存在した場合の対象包　
        /// </summary>
        public WriteMode LocalSideFileWriteMode;
        public bool UnZip;
        public string UnZipLocalFolder;
        public bool DeleteZipFileAfterUnzipped;
    }


    public struct SetAttributeFile
    {
        public string Comment;
        public string LocalSideFullFileName;
        public Attribute Attribute;
    }

    public struct RemoveFile
    {
        public string Comment;
        public string LocalSideFullFileName;
    }

    public struct SetRegistry
    {
        public string Comment;
        public string KeyName;
        public string ValueName;
        public Microsoft.Win32.RegistryValueKind RegistryValueKind;
        public string Value;
    }

    public struct RemoveRegistry
    {
        public string Comment;
        public string OpenKeyName;
        public string SubKeyName;
        public string ValueName;
    }

    public struct ExecuteProcess
    {
        public string Comment;
        public string FileName;
        public string Arguments;
        public string UserName;
        public string EncryptedUserName;
        public string Password;
        public string EncryptedPassword;
        public string Domain;
        public bool StdOut;
        public bool StdErr;
    }

    public delegate bool Delegate_FileCopy(string Source, string Dist, Action<string> WriteLine);

    [SupportedOSPlatform("windows")]
    public class XML_Control
    {
        private string LogBaseFolder { get; set; }

        private byte[] AESkey { get; set; }

        private byte[] AES_iv { get; set; }

        SysConfiguration configClass = new SysConfiguration();

        SysConfiguratorLog SysConfigLogSystem;

        /// <summary>
        /// 最初のログファイル名を保持する変数（）
        /// </summary>
        public static string FirstLogFilename;

        /// <summary>
        /// このｱﾄﾞｲﾝのｲﾝｽﾀﾝｽの区別のための識別子（複数同時起動時にログファイル上で区別するため）
        /// </summary>


        public XML_Control(string Base64AESkey, string LogBaseFolder = null, string LogFileName = null)
        {
            this.AESkey = EncryptionAES.ConvertFromBase64StringToByteArray(Base64AESkey);
            this.LogBaseFolder = LogBaseFolder;

            if (LogBaseFolder == null)
            {
                var newFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), @"TOYOCOMMON");
                this.LogBaseFolder = newFolder;
            }
            else
            {
                this.LogBaseFolder = LogBaseFolder;
            }

            FileFolder.MakeDirectory(this.LogBaseFolder);
            SysConfigLogSystem = new SysConfiguratorLog(this.LogBaseFolder, DateTime.Now.ToString("HHmmss"));

            if (string.IsNullOrWhiteSpace(LogFileName) == false)
            {
                SysConfigLogSystem.LogFileName = LogFileName;
            }
        }
        public XML_Control(byte[] AESkey, string LogBaseFolder = null, string LogFileName = null)
        {
            this.AESkey = AESkey;
            this.LogBaseFolder = LogBaseFolder;

            if (LogBaseFolder == null)
            {
                var newFolder = System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), @"TOYOCOMMON");
                this.LogBaseFolder = newFolder;
            } // LogBaseFolderに指定がなかった場合
            else
            {
                this.LogBaseFolder = LogBaseFolder;
            } // LogBaseFolderに指定があった場合。
            FileFolder.MakeDirectory(this.LogBaseFolder);
            SysConfigLogSystem = new SysConfiguratorLog(this.LogBaseFolder, DateTime.Now.ToString("HHmmss"));

            if (string.IsNullOrWhiteSpace(LogFileName) == false)
            {
                SysConfigLogSystem.LogFileName = LogFileName;
            }

        }

        // ファイルコピーメソッドを外部から与える場合に使用。
        public Delegate_FileCopy DelegateFileCopyFunc = null;


        /// <summary>
        /// 設定適用済みの場合にリネームするファイル名を生成
        /// </summary>
        /// <param name="InstructionsVersionToyoAddinFullFileName"></param>
        /// <returns></returns>
        public string Applied_GetInstructions_fullfilename(string InstructionsVersionToyoAddinFullFileName, DateTime ConfigDateTime, Action<string> LogWrite)
        {
            string applied_Instructions_fullfilename;
            string timestamp;


            // 適用後のフルファイル名を組立
            if (ConfigDateTime != null)
            {
                timestamp = ConfigDateTime.ToString("yyyy-MM-dd_HHmmss");
                LogWrite($"■Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ \"{System.IO.Path.GetFileName(InstructionsVersionToyoAddinFullFileName)}\" DATETIMEタグの値 {timestamp}");
            }
            else
            {
                timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
                LogWrite($"■Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ \"{System.IO.Path.GetFileName(InstructionsVersionToyoAddinFullFileName)}\" DATETIMEタグは見つかりませんでした。現在時刻を使用します {timestamp}");
            }

            // 適用後のファイル名を組立て
            applied_Instructions_fullfilename = InstructionsVersionToyoAddinFullFileName + "_" + timestamp + ".Applied";

            return applied_Instructions_fullfilename;
        }


        /// <summary>
        /// ■SysConfiguration.Conffファイルの内容に従ってジョブを実行します。
        /// </summary>
        /// <param name="ConfigFullFileName"></param>
        /// <param name="LogWrite"></param>
        /// <param name="foreceExecute"></param>
        /// <returns></returns>
        public bool JobLoadAndExecute(string ConfigFullFileName, string baseDir, Action<string> LogWrite, bool foreceExecute = false, bool IsRemoveControlFile = false)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            // Stopwatchクラス生成・計測開始
            var initilaizeSw = new System.Diagnostics.Stopwatch(); initilaizeSw.Start();

            LogWrite($"■XML_Control.JobLoadAndExecute(..) <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> 開始");
            SysConfiguration baseConfig;

            bool loadresult = configClass.ReadloadConfigData(ConfigFullFileName, out baseConfig, LogWrite);

            if (loadresult == false)
            {
                LogWrite($"■XML_Control.JobLoadAndExecute(..) <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> \"{System.IO.Path.GetFileName(ConfigFullFileName)}\" は処理対象外です。");

                // 用が済んだファイルを削除
                if (IsRemoveControlFile)
                    if (FileFolder.RemoveFile(ConfigFullFileName))
                        LogWrite($"■XML_Control.JobLoadAndExecute(..) <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> \"{System.IO.Path.GetFileName(ConfigFullFileName)}\" を削除しました");
                return false;
            }

            string configFileNameShort = System.IO.Path.GetFileName(ConfigFullFileName);

            AES_iv = EncryptionAES.ConvertFromBase64StringToByteArray(baseConfig.AES_iv_Base64);
            DateTime ConfigDateTime = baseConfig.DATETIME;
            
            //LogWrite($"■ｺﾝﾄﾛｰﾙﾌｧｲﾙ {configFileNameShort} 処理開始. DATETIMEタグは {ConfigDateTime}です");

            string applied_Instructions_fullfilename = Applied_GetInstructions_fullfilename(ConfigFullFileName, ConfigDateTime, LogWrite);

            if (foreceExecute)
            {
                if (FileFolder.RemoveFile(applied_Instructions_fullfilename))
                    LogWrite($"■XML_Control.JobLoadAndExecute(..) <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> メソッドパラメータ foreExecuteがtrueで呼び出された為 追加設定履歴ﾌｧｲﾙ \"{applied_Instructions_fullfilename}\" を削除しました");
            }
            else
            {
                LogWrite($"■XML_Control.JobLoadAndExecute(..) <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> メソッドパラメータ foreExecuteがfalseでした。 強制再実行フラグは設定されていません");

            }

            if (System.IO.File.Exists(applied_Instructions_fullfilename) == false)
            {
                ///
                LogWrite($"■ｺﾝﾄﾛｰﾙﾌｧｲﾙ \"{System.IO.Path.GetFileName(ConfigFullFileName)}\" の履歴  \"{applied_Instructions_fullfilename}\"が存在しません 未実行です。指令を実行します");

                bool result = false;

                // コントロールにログ出力先設定があった場合
                if (baseConfig.LogFolderName != null)
                {
                    string newLogFolderName = System.Environment.ExpandEnvironmentVariables(baseConfig.LogFolderName);
                    bool checkans = FileFolder.ContainInvalidFileNmaeChars(newLogFolderName);
                    if (checkans == false)
                    {
                        bool ans = SysConfigLogSystem.ChangeLoggingFolder(System.Environment.ExpandEnvironmentVariables(baseConfig.LogFolderName));
                        if (ans)
                        {
                            LogWrite($"■ｺﾝﾄﾛｰﾙﾌｧｲﾙのﾛｸﾞ出力先ﾌｫﾙﾀﾞの変更が指示されました \"{SysConfigLogSystem.CurrnetLogFolder}\"");
                        }
                        else
                        {
                            LogWrite($"※ｺﾝﾄﾛｰﾙﾌｧｲﾙのﾛｸﾞ出力先ﾌｫﾙﾀﾞの変更に失敗しました \"{SysConfigLogSystem.CurrnetLogFolder}\" -> \"{baseConfig.LogFolderName}\" ");
                        }

                    }
                    else
                    {
                        LogWrite($"※ｺﾝﾄﾛｰﾙﾌｧｲﾙのﾛｸﾞ出力先ﾌｫﾙﾀﾞ  \"{newLogFolderName}\" への変更は失敗しました。（使用不可の文字があります）");
                    }

                }
                // コントロールに ログファイル名の設定があった場合
                if (baseConfig.LogFileName != null)
                {
                    string newLogFileName = System.Environment.ExpandEnvironmentVariables(baseConfig.LogFileName);
                    bool checkans = FileFolder.ContainInvalidFileNmaeChars(newLogFileName);
                    if (checkans == false)
                    {
                        SysConfigLogSystem.LogFileName = newLogFileName;
                        LogWrite($"■ｺﾝﾄﾛｰﾙﾌｧｲﾙのﾛｸﾞ出力先ﾌｫﾙﾀﾞ をｺﾝﾄﾛｰﾙﾌｧｲﾙの指示により変更  \"{SysConfigLogSystem.CurrnetLogFolder}\" , \"{SysConfigLogSystem.LogFileName}\"");
                        LogWrite = SysConfigLogSystem.WriteLine;
                    }
                    else
                    {
                        LogWrite($"※ｺﾝﾄﾛｰﾙﾌｧｲﾙのﾛｸﾞ出力先ﾌｫﾙﾀﾞ の  \"{newLogFileName}\" への変更は失敗しました。（使用不可の文字があります）");
                    }
                }


                // ﾌｧｲﾙ取得指示があれば実行。zip解凍可
                if ((baseConfig.GetFileLists != null) && (baseConfig.GetFileLists.Count > 0))
                {
                    LogWrite("");
                    result = Process_GetFiles(baseConfig.GetFileLists, baseDir, LogWrite);
                    if (result)
                    {
                        LogWrite($"■ファイル取得指示の結果は{result}でした。");
                    }
                    else
                    {
                        LogWrite($"※ファイル取得指示の結果は{result}でした。");
                    }
                }


                // ﾌｧｲﾙ単独アトリビュート変更指示があれば実行
                if ((baseConfig.SetAttributeFiles != null) && (baseConfig.SetAttributeFiles.Count > 0))
                {
                    LogWrite("");
                    result = Process_ChangeAttribute(baseConfig.SetAttributeFiles, LogWrite);
                    if (result)
                    {
                        LogWrite($"■ファイルアトリビュート変更指示の結果は{result}でした。");
                    }
                    else
                    {
                        LogWrite($"※ファイルアトリビュート変更指示の結果は{result}でした。");
                    }
                }



                // ファイル削除指示があれば実行
                if ((baseConfig.RemoveFiles != null) && (baseConfig.RemoveFiles.Count > 0))
                {
                    LogWrite("");
                    result = Process_Delete(baseConfig.RemoveFiles, LogWrite);
                    if (result)
                    {
                        LogWrite($"■ファイル削除指示の結果は{result}でした。");
                    }
                    else
                    {
                        LogWrite($"※ファイル削除指示の結果は{result}でした。");
                    }
                }


                // レジストリ設定指示があれば実行
                if ((baseConfig.SetRegistries != null) && (baseConfig.SetRegistries.Count > 0))
                {
                    LogWrite("");
                    result = Process_SetRegistry(baseConfig.SetRegistries, LogWrite);
                    if (result)
                    {
                        LogWrite($"■レジストリ設定指示の結果は{result}でした。");
                    }
                    else
                    {
                        LogWrite($"※レジストリ設定指示の結果は{result}でした。");
                    }

                }


                // レジストリ削除指示があれば実行
                if ((baseConfig.RemoveRegistries != null) && (baseConfig.RemoveRegistries.Count > 0))
                {
                    LogWrite("");
                    result = Process_RemoveRegistry(baseConfig.RemoveRegistries, LogWrite);
                    if (result)
                    {
                        LogWrite($"■レジストリ削除指示の結果は{result}でした。");
                    }
                    else
                    {
                        LogWrite($"※レジストリ削除指示の結果は{result}でした。");
                    }

                }


                // プロセス実行処理があれば実行
                if ((baseConfig.ExecuteProcesses != null) && (baseConfig.ExecuteProcesses.Count > 0))
                {
                    LogWrite("");
                    result = Process_ProcessExecute(baseConfig.ExecuteProcesses, LogWrite);
                    if (result)
                    {
                        LogWrite($"■外部コマンド実行指示の結果は{result}でした。");
                    }
                    else
                    {
                        LogWrite($"※外部コマンド実行指示の結果は{result}でした。");
                    }
                }

                // 用が済んだファイルを削除
                if (IsRemoveControlFile)
                    if (FileFolder.RemoveFile(ConfigFullFileName) == false)
                        LogWrite($"※コントロールファイル \"{System.IO.Path.GetFileName(ConfigFullFileName)}\" を削除に失敗しました");

                if (result)
                {
                    baseConfig.SaveConfig(applied_Instructions_fullfilename, baseConfig, LogWrite);

                    LogWrite($"■Addin追加指令を実行しました。元のファイルは 履歴ﾌｧｲﾙ \"{applied_Instructions_fullfilename}\" として保存しました");
                }

                LogWrite($"■Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ 処理を終了します");

                return result;
            }
            else
            {
                // 用が済んだファイルを削除
                if (IsRemoveControlFile)
                    if (FileFolder.RemoveFile(ConfigFullFileName) == false)
                        LogWrite($"※XML_Control.JobLoadAndExecute(..) <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>> コントロールファイル \"{System.IO.Path.GetFileName(ConfigFullFileName)}\" を削除に失敗しました");

                LogWrite($"■XML_Control.JobLoadAndExecute(..) <<経過時間:{initilaizeSw.Elapsed.Hours} 時間{initilaizeSw.Elapsed.Minutes}分 {initilaizeSw.Elapsed.Seconds}秒>>  Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ実行履歴ﾌｧｲﾙ \"{System.IO.Path.GetFileName(applied_Instructions_fullfilename)}\" は既に存在しています。追加指令をスキップします");
                return true;
            }
        }

        private bool Process_GetFiles(List<GetFulFileName> GetFileLists, string baseDir, Action<string> LogWrite)
        {
            bool result = true;

            if (GetFileLists.Count > 0)
            {

                LogWrite($"■XML_Control.Process_GetFiles(..) ファイル取得指示が {GetFileLists.Count} 件あります。");


                foreach (var getFileList in GetFileLists)
                {
                    result = GetFiles(getFileList, baseDir, LogWrite);
                }
            }
            else
            {
                //LogWrite($"■ファイル取得指示はありません。");
            }
            return result;

        }

        private bool Process_ChangeAttribute(List<SetAttributeFile> SetAttributeFiles, Action<string> LogWrite)
        {
            bool result = true;

            if (SetAttributeFiles.Count > 0)
            {

                LogWrite($"■XML_Control.Process_ChangeAttribute(..) ファイルアトリビュート変更指示が {SetAttributeFiles.Count} 件あります。");
                foreach (var SetAttributeFile in SetAttributeFiles)
                {
                    result = ChangeAttribute(SetAttributeFile, LogWrite);
                }
            }
            else
            {
                //LogWrite($"■ファイルアトリビュート変更指示はありません。");
            }
            return result;
        }

        private bool Process_Delete(List<RemoveFile> RemoveFiles, Action<string> LogWrite)
        {
            bool result = true;

            if (RemoveFiles.Count > 0)
            {

                LogWrite($"■XML_Control.Process_Delete(..) ファイル削除指示が {RemoveFiles.Count} 件あります。");
                foreach (var file in RemoveFiles)
                {
                    result = Delete(file, LogWrite);
                }
            }
            else
            {
                //LogWrite($"■ファイル削除指示はありません。");
            }

            return result;
        }

        private bool Process_SetRegistry(List<SetRegistry> SetRegistries, Action<string> LogWrite)
        {
            bool result = true;

            if (SetRegistries.Count > 0)
            {

                LogWrite($"■XML_Control.Process_Delete.Process_SetRegistry(..) レジストリ設定指示が {SetRegistries.Count} 件あります。");

                foreach (var setRegistry in SetRegistries)
                {
                    result = SetRegistry(setRegistry, LogWrite);
                }
            }
            else
            {
                //LogWrite($"■レジストリ設定処理はありません。");
            }
            return result;
        }

        private bool Process_RemoveRegistry(List<RemoveRegistry> RemoveRegistries, Action<string> LogWrite)
        {
            bool result = true;

            if (RemoveRegistries.Count > 0)
            {

                LogWrite($"■XML_Control.Process_RemoveRegistry(..) レジストリ削除指示が {RemoveRegistries.Count} 件あります。");

                foreach (var removeRegistry in RemoveRegistries)
                {
                    result = RemoveRegistry(removeRegistry, LogWrite);
                }
            }
            else
            {
                //LogWrite($"■レジストリ削除処理はありません。");
            }
            return result;
        }

        private bool Process_ProcessExecute(List<ExecuteProcess> ExecuteProcesses, Action<string> LogWrite)
        {
            bool result = true;

            if (ExecuteProcesses.Count > 0)
            {

                LogWrite($"■XML_Control.Process_ProcessExecute(..) 外部コマンド実行指示が {ExecuteProcesses.Count} 件あります。");
                foreach (var executeProcess in ExecuteProcesses)
                {
                    result = ProcessExecute(executeProcess, AESkey, AES_iv, LogWrite);
                }
            }
            else
            {
                //LogWrite($"■外部コマンド実行指示はありません。");
            }

            return result;
        }

        /// <summary>
        /// ■ファイル取得
        /// </summary>
        /// <param name="file"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        private bool GetFiles(GetFulFileName file, string baseDirectory, Action<string> WriteLine)
        {
            bool resultRemoteLode = false;

            WriteLine($"■XML_Control.GetFiles(..) コメント \"{file.Comment}\"");

            string ServerSideFullFileName = null;
            if (string.IsNullOrWhiteSpace(file.LocalSideFullFileName) == false)
            {
                ServerSideFullFileName = file.ServerSideFullFileName;
            }
            else
            {
                WriteLine($"※XML_Control.GetFiles(..) ｻｰﾊﾞｰｻｲﾄﾞの読み出しﾌｧｲﾙﾌﾙﾊﾟｽの指示がありません");
                return false;
            }


            string LocalSideFullFileName = null;
            if (string.IsNullOrWhiteSpace(file.LocalSideFullFileName) == false)
            {
                LocalSideFullFileName = System.Environment.ExpandEnvironmentVariables(file.LocalSideFullFileName);
                WriteLine($"■XML_Control.GetFiles(..) ﾛｰｶﾙｻｲﾄﾞ:\"{file.LocalSideFullFileName}\"");
                WriteLine($"■XML_Control.GetFiles(..) -> 環境変数展開後:\"{LocalSideFullFileName}\"");
            }
            else
            {
                WriteLine($"※XML_Control.GetFiles(..) ﾛｰｶﾙｻｲﾄﾞ側のﾌｧｲﾙﾌﾙﾊﾟｽの指示がありません");
                return false;
            }

            WriteMode LocalSideFileWriteMode = file.LocalSideFileWriteMode;

            bool unzip = file.UnZip;


            string UnzipExtractFolder = null;
            if (unzip == true)
            {
                if (string.IsNullOrWhiteSpace(file.UnZipLocalFolder) == false)
                {
                    UnzipExtractFolder = System.Environment.ExpandEnvironmentVariables(file.UnZipLocalFolder);
                    WriteLine($"■XML_Control.GetFiles(..) UnZipﾓｰﾄﾞです。ZIP解凍先ﾛｰｶﾙﾌｫﾙﾀﾞ:{file.UnZipLocalFolder} -> 環境変数展開後{UnzipExtractFolder}");
                }
                else
                {
                    WriteLine($"※XML_Control.GetFiles(..) UnZip==trueですがZIP解凍先ﾛｰｶﾙﾌｫﾙﾀﾞの指示がありません");
                    return false;
                }
            }

            bool DeleteZipFileAfterUnzipped = file.DeleteZipFileAfterUnzipped;

            string tempfolder = System.IO.Path.GetTempPath();

            using (var LoadTempFile = new ManagedTemporaryFile(tempfolder, "XML_Control.GetFiles_"))
            {
                // ファイルコピー用メソッドがデリゲードで指定されている場合、そうでない場合で呼び出しメソッドを変える
                // ﾛｰｶﾙﾌｧｲﾙｼｽﾃﾑのみでの動作の場合は ソース側ﾌｧｲﾙを検索する機能を設けたメソッドGetSourceFileToTemp()を使う
                if (DelegateFileCopyFunc == null)
                    resultRemoteLode = GetSourceFileToTemp(ServerSideFullFileName, baseDirectory, LoadTempFile.FullTempFileName);
                else
                    resultRemoteLode = DelegateFileCopyFunc(ServerSideFullFileName, LoadTempFile.FullTempFileName, WriteLine);

                if (resultRemoteLode)
                {
                    WriteLine($"■XML_Control.GetFiles(..) ｻｰﾊﾞｰｻｲﾄﾞ:{ServerSideFullFileName}\n -> テンポラリファイル:{LoadTempFile.FullTempFileName}として取得成功");

                    /// 既存のﾛｰｶﾙﾌｧｲﾙの属性を保持する変数（デフォルトでテンポラリ作成されたファイルの方の属性を保持する）
                    FileAttributes LocalSideFullFileNameFileAttr = System.IO.File.GetAttributes(LoadTempFile.FullTempFileName);

                    /// ディレクトリの存在を確認・ないなら作成する。作成に失敗したらエラーとする

                    try
                    {
                        if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(LocalSideFullFileName)) == false)
                        {

                            if (FileFolder.MakeDirectory(System.IO.Path.GetDirectoryName(LocalSideFullFileName)))
                                WriteLine($"■XML_Control.GetFiles(..) ディレクトリ \"{System.IO.Path.GetDirectoryName(LocalSideFullFileName)}\" が無いため作成しました。処理を続けます");
                            else
                            {
                                WriteLine($"※XML_Control.GetFiles(..) ディレクトリ \"{System.IO.Path.GetDirectoryName(LocalSideFullFileName)}\" の作成に失敗しました。スキップします");
                                return false;
                            }
                        }
                        else
                        {
                            WriteLine($"■XML_Control.GetFiles(..) ディレクトリ \"{System.IO.Path.GetDirectoryName(LocalSideFullFileName)}\" は既に存在しています。処理を続けます");
                        }
                    }
                    catch (Exception ex)
                    {
                        Eventlog.Log.WriteEntry("AutocadTOYOaddin", EventLogEntryType.Error, 0, $"※SasaLib.SysConfigurator.XML_Control.GetFiles(..)にて例外検知 {ex.Message}");
                        WriteLine($"※XML_Control.GetFiles(..) SasaLib.SysConfigurator.XML_Control.GetFiles(..)にて例外検知 {ex.Message}。スキップします");
                    }

                    /// ﾛｰｶﾙﾌｧｲﾙが存在しているなら削除する
                    if (System.IO.File.Exists(LocalSideFullFileName) == true)
                    {

                        if (LocalSideFileWriteMode == WriteMode.ForceOverwrite)
                        {
                            try
                            {
                                // ファイルやディレクトリの属性（群）を取得（元のファイルの属性を保持）
                                LocalSideFullFileNameFileAttr = System.IO.File.GetAttributes(LocalSideFullFileName);
                                ;
                                // ファイル属性に読み取り専用を解除
                                FileAttributes fas = LocalSideFullFileNameFileAttr & ~FileAttributes.ReadOnly;
                                System.IO.File.SetAttributes(LocalSideFullFileName, fas);
                            }
                            catch (System.Exception ex)
                            {
                                WriteLine($"■XML_Control.GetFiles(..) ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" を  上書きするモードです.書き込み可能への設定変更に失敗しました {ex.Message}");
                            }

                            if (FileFolder.RemoveFile(LocalSideFullFileName) == true)
                                WriteLine($"■XML_Control.GetFiles(..) ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" を 上書きするモードです.既存ﾌｧｲﾙを削除しました");
                            else
                            {
                                WriteLine($"※XML_Control.GetFiles(..) ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" を 上書きするモードです.既存ﾌｧｲﾙを削除に失敗しました。このファイルはスキップします");
                                return false;
                            }
                        }
                    }

                    /// ﾛｰｶﾙﾌｧｲﾙが存在していないことを確認し、テンポラリから移動する
                    if (System.IO.File.Exists(LocalSideFullFileName) == false)
                    {

                        bool ans = FileFolder.MoveFile(LoadTempFile.FullTempFileName, LocalSideFullFileName);

                        if (ans)
                        {
                            WriteLine($"■XML_Control.GetFiles(..) テンポラリ:\"{LoadTempFile.FullTempFileName}\" -> ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" 移動成功");

                            try
                            {
                                // ファイル属性を適用（テンポラリファイルの属性値か、元のファイルがあった場合はそのファイルの属性値を適用）
                                System.IO.File.SetAttributes(LocalSideFullFileName, LocalSideFullFileNameFileAttr);
                            }
                            catch (System.Exception ex)
                            {
                                WriteLine($"※XML_Control.GetFiles(..) ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" 属性値書き戻しに失敗しました {ex.Message}");
                            }

                            if (unzip)
                            {
                                bool unzipans = ZipFile.OpenRead(LocalSideFullFileName).ExtractToDirectory(UnzipExtractFolder, true, WriteLine);

                                if (unzipans)
                                {
                                    WriteLine($"■XML_Control.GetFiles(..) unzip指示があります。ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" 解凍先:\"{UnzipExtractFolder}\" 解凍成功");

                                    if (DeleteZipFileAfterUnzipped == true)
                                    {
                                        try
                                        {
                                            System.IO.File.Delete(LocalSideFullFileName);
                                            WriteLine($"■XML_Control.GetFiles(..) 圧縮ﾌｧｲﾙ展開後削除指示 があります:\"{LocalSideFullFileName}\" の削除に成功しました");
                                        }
                                        catch (IOException e1)
                                        {
                                            WriteLine($"※XML_Control.GetFiles(..) 圧縮ﾌｧｲﾙ展開後削除指示 がありましたが、:{LocalSideFullFileName} の削除に失敗しました 。処理を続けられません continueします {e1.Message}");
                                            return false;
                                        }
                                    }
                                }
                                else
                                {
                                    WriteLine($"※XML_Control.GetFiles(..) unzip指示があります。ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" 解凍先:\"{UnzipExtractFolder}\" 解凍失敗。\"{LocalSideFullFileName}\"の削除を試みます");

                                    try
                                    {
                                        System.IO.File.Delete(LocalSideFullFileName);
                                        WriteLine($"※XML_Control.GetFiles(..) 圧縮ファイルを削除しました:{LocalSideFullFileName} の削除に成功しました");
                                    }
                                    catch (IOException e1)
                                    {
                                        WriteLine($"※XML_Control.GetFiles(..) 圧縮ﾌｧｲﾙ {LocalSideFullFileName} の削除に失敗しました 。 {e1.Message}");
                                    }
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            WriteLine($"※XML_Control.GetFiles(..) テンポラリ:\"{LoadTempFile.FullTempFileName}\" -> ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" 移動失敗。ﾛｰｶﾙｻｲﾄﾞ:\"{LocalSideFullFileName}\" が既に存在しています(2)。処理を続けられません continueします");
                            return false;
                        }
                    }
                    else
                    {
                        WriteLine($"※XML_Control.GetFiles(..) ﾛｰｶﾙｻｲﾄﾞ:{LocalSideFullFileName}は既に存在しています。WriteMode.ForceOverwrite が {WriteMode.ForceOverwrite} です。 スキップします");
                        return false;
                    }

                }
                else
                {
                    WriteLine($"※XML_Control.GetFiles(..) ｻｰﾊﾞｰｻｲﾄﾞ:{ServerSideFullFileName} -> テンポラリ:{LoadTempFile.FullTempFileName} 取得失敗(2)。処理を続けられませんスキップします");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// ■ファイル属性変更
        /// </summary>
        /// <param name="file"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        private bool ChangeAttribute(SetAttributeFile file, Action<string> WriteLine)
        {
            string LocalSideFullFileName = null;

            WriteLine($"■コメント \"{file.Comment}\"");

            if (string.IsNullOrWhiteSpace(file.LocalSideFullFileName) == false)
            {
                LocalSideFullFileName = System.Environment.ExpandEnvironmentVariables(file.LocalSideFullFileName);
                WriteLine($"■ﾛｰｶﾙｻｲﾄﾞ:\"{file.LocalSideFullFileName}\" -> 環境変数展開後:\"{LocalSideFullFileName}\"");
            }
            else
            {
                WriteLine($"※ﾛｰｶﾙｻｲﾄﾞの作成ﾌｧｲﾙﾌﾙﾊﾟｽ指示がありません <LocalSideFullFileName></LocalSideFullFileName> ");
                return false;
            }


            var Attribute = file.Attribute;

            bool setAttributeResult = false;

            if (string.IsNullOrWhiteSpace(LocalSideFullFileName) == false)
            {
                switch (Attribute)
                {
                    case Attribute.ReadOnly:
                        try
                        {
                            // ファイルやディレクトリの属性（群）を取得
                            FileAttributes attributes = System.IO.File.GetAttributes(LocalSideFullFileName);

                            // ファイル属性に読み取り専用をセット
                            attributes = attributes | FileAttributes.ReadOnly;
                            System.IO.File.SetAttributes(LocalSideFullFileName, attributes);

                            setAttributeResult = true;

                        }
                        catch (System.Exception ex)
                        {
                            WriteLine($"※ {LocalSideFullFileName} を 読み取り専用 への設定変更に失敗 {ex.Message}");
                            setAttributeResult = false;
                        }

                        break;
                    case Attribute.ReadWrite:
                        try
                        {
                            // ファイルやディレクトリの属性（群）を取得
                            FileAttributes attributes = System.IO.File.GetAttributes(LocalSideFullFileName);

                            // 読み取り専用属性を解除
                            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                attributes = attributes & ~FileAttributes.ReadOnly;  // NOT演算子を使って読み取り専用属性を除去
                                System.IO.File.SetAttributes(LocalSideFullFileName, attributes);
                                //Console.WriteLine("ファイルから読み取り専用属性を解除しました。");
                            }
                            else
                            {
                               WriteLine($"■{LocalSideFullFileName}には読み取り専用属性が設定されていません。");
                            }

                            setAttributeResult = true;
                        }
                        catch (System.Exception ex)
                        {
                            WriteLine($"※ {LocalSideFullFileName} を 書き込み可能 への設定変更に失敗 {ex.Message}");
                            setAttributeResult = false;
                        }

                        break;
                    default:
                        break;
                }

                if (setAttributeResult)
                    WriteLine($"■ファイルアトリビュート変更対象:\"{LocalSideFullFileName}\" に {file.Attribute} をセットに成功しました");
                else
                    WriteLine($"※ファイルアトリビュート変更対象:\"{LocalSideFullFileName}\"  に {file.Attribute} をセットを失敗しました");

                return setAttributeResult;
            }
            else
            {
                WriteLine($"※ﾛｰｶﾙｻｲﾄﾞの作成ﾌｧｲﾙﾌﾙﾊﾟｽ指示がありません <LocalSideFullFileName></LocalSideFullFileName> ");
                return false;
            }

        }

        /// <summary>
        /// ■ファイル削除
        /// </summary>
        /// <param name="file"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        private bool Delete(RemoveFile file, Action<string> WriteLine)
        {
            WriteLine($"■コメント \"{file.Comment}\"");

            string orderPath = null;
            if (string.IsNullOrWhiteSpace(file.LocalSideFullFileName) == false)
            {
                orderPath = file.LocalSideFullFileName;
            }
            else
            {
                WriteLine($"※ﾛｰｶﾙｻｲﾄﾞの作成ﾌｧｲﾙﾌﾙﾊﾟｽ指示がありません <LocalSideFullFileName></LocalSideFullFileName> ");
                return false;
            }

            if (string.IsNullOrWhiteSpace(orderPath) == false)
            {

                try
                {
                    // ファイル属性に読み取り専用を解除
                    FileAttributes fattr = System.IO.File.GetAttributes(orderPath) & ~FileAttributes.ReadOnly;
                    System.IO.File.SetAttributes(orderPath, fattr);
                }
                catch (System.Exception ex)
                {
                    WriteLine($"※ﾛｰｶﾙｻｲﾄﾞ:\"{orderPath}\" 属性変更（書き込み可）失敗しました {ex.Message}");
                    return false;
                }


                if (System.IO.File.GetAttributes(orderPath).HasFlag(FileAttributes.Directory))
                {
                    try
                    {
                        System.IO.Directory.Delete(orderPath, true);
                        WriteLine($"■削除対象 :\"{orderPath}\" フォルダとサブフォルダ及びファイルの削除に成功しました");
                        return true;
                    }
                    catch (IOException e1)
                    {
                        WriteLine($"※削除対象 :\"{orderPath}\" フォルダとサブフォルダ及びファイルの削除に失敗しました {e1.Message}");
                        return false;
                    }


                } // ディレクトリの場合
                else
                {
                    try
                    {
                        System.IO.File.Delete(orderPath);
                        WriteLine($"■削除対象:\"{orderPath}\" ファイルの削除に成功しました");
                        return true;

                    }
                    catch (IOException e1)
                    {
                        WriteLine($"※削除対象:\"{orderPath}\" ファイルの削除に失敗しました {e1.Message}");
                        return false;
                    }

                } // ファイルの場合
            }
            else
            {
                WriteLine($"※削除対象ﾌｧｲﾙの指定がありません");
                return false;
            }
        }

        /// <summary>
        /// ■レジストリ設定
        /// </summary>
        /// <param name="setRegistry"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        private bool SetRegistry(SetRegistry setRegistry, Action<string> WriteLine)
        {
            WriteLine($"■コメント \"{setRegistry.Comment}\"");

            string KeyName = null;
            if (string.IsNullOrWhiteSpace(setRegistry.KeyName) == false)
            {
                KeyName = setRegistry.KeyName;
                WriteLine($"■ｷｰ名 KeyName:\"{KeyName}\"");
            }
            else
            {
                WriteLine($"※レジストリ設定のKeyName指示がありません");
                return false;
            }


            string ValueName = null;
            if (string.IsNullOrWhiteSpace(setRegistry.ValueName) == false)
            {
                ValueName = setRegistry.ValueName;
                WriteLine($"■値の名前 ValueName:\"{ValueName}\"");
            }
            else
            {
                WriteLine($"※レジストリ設定のValueName指示がありません");
                return false;
            }

            Microsoft.Win32.RegistryValueKind RegistryValueKind;
            RegistryValueKind = setRegistry.RegistryValueKind;


            string Value;
            if (setRegistry.Value != null)
            {
                Value = System.Environment.ExpandEnvironmentVariables(setRegistry.Value);
                WriteLine($"■値のﾃﾞｰﾀ Value:\"{setRegistry.Value}\" -> 環境変数展開後 Value:\"{Value}\"");
            }
            else
            {
                WriteLine($"※レジストリ設定のValue指示がありません");
                return false;
            }
            Microsoft.Win32.RegistryValueKind registryValueKind = Microsoft.Win32.RegistryValueKind.Unknown;


            // レジストリの設定
            try
            {
                object regValue = null;

                Microsoft.Win32.RegistryKey rKey = null;
                if (System.Text.RegularExpressions.Regex.IsMatch(KeyName, @"HKEY_CURRENT_USER\\", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    KeyName = System.Text.RegularExpressions.Regex.Replace(KeyName, @"HKEY_CURRENT_USER\\", "");
                    //// レジストリ・キーを新規作成して開く
                    rKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(KeyName, true);
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(KeyName, @"HKEY_CLASSES_ROOT\\", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    KeyName = System.Text.RegularExpressions.Regex.Replace(KeyName, @"HKEY_CLASSES_ROOT\\", "");
                    rKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(KeyName, true);
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(KeyName, @"HKEY_LOCAL_MACHINE\\", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    KeyName = System.Text.RegularExpressions.Regex.Replace(KeyName, @"HKEY_LOCAL_MACHINE\\", "");
                    rKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(KeyName, true);
                }
                else
                {
                    WriteLine($"※レジストリ設定 キー名:\"{KeyName}\" の オープンに失敗しました");
                    return false;
                }

                switch (RegistryValueKind)
                {
                    case Microsoft.Win32.RegistryValueKind.String:
                        regValue = Value;
                        registryValueKind = Microsoft.Win32.RegistryValueKind.String;
                        break;
                    case Microsoft.Win32.RegistryValueKind.Binary:
                        regValue = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(Value);
                        registryValueKind = Microsoft.Win32.RegistryValueKind.Binary;
                        break;
                    case Microsoft.Win32.RegistryValueKind.DWord:
                        if (UInt32.TryParse(Value, out var numDWord))
                        {
                            regValue = unchecked((Int32)numDWord);
                            registryValueKind = Microsoft.Win32.RegistryValueKind.DWord;
                        }
                        else
                        {
                            throw new System.Exception($"{Value}はDWordに変換できません");
                        }
                        break;
                    case Microsoft.Win32.RegistryValueKind.QWord:
                        if (UInt64.TryParse(Value, out var numQword))
                        {
                            regValue = unchecked((Int64)numQword);
                            registryValueKind = Microsoft.Win32.RegistryValueKind.DWord;
                        }
                        else
                        {
                            throw new System.Exception($"{Value}はDWordに変換できません");
                        }
                        break;
                    default:
                        regValue = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(Value);
                        registryValueKind = Microsoft.Win32.RegistryValueKind.Binary;
                        break;
                }


                //// レジストリの値を設定
                rKey.SetValue(ValueName, regValue, registryValueKind);

                //// 開いたレジストリを閉じる
                rKey.Close();

                //SasaLib.REGutils.SetValue(KeyName,ValueName,Value);

                //設定したレジストリの値をコンソールに表示
                WriteLine($"■レジストリ設定 キー名:\"{KeyName}\" , 名前:\"{ValueName}\"  , データタイプ:\"{registryValueKind}\" , データ:\"{Value}\" 設定に成功しました");

                return true;
            }
            catch (System.Exception ex)
            {
                // レジストリ・キーが存在しない
                WriteLine($"※レジストリ設定 キー名:\"{KeyName}\" , 名前:\"{ValueName}\" , データタイプ:\"{registryValueKind}\" , データ:\"{Value}\" 設定に失敗しました {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// ■レジストリ削除
        /// </summary>
        /// <param name="reegistry"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        private bool RemoveRegistry(RemoveRegistry reegistry, Action<string> WriteLine)
        {
            WriteLine($"■コメント \"{reegistry.Comment}\"");

            string OpenKeyName = null;
            if (string.IsNullOrWhiteSpace(reegistry.OpenKeyName) == false)
            {
                OpenKeyName = reegistry.OpenKeyName;
            }
            else
            {
                WriteLine($"※レジストリ削除のOpenKeyName指示がありません");
                return false;
            }


            // レジストリのｵｰﾌﾟﾝ
            try
            {

                Microsoft.Win32.RegistryKey rKey = null;
                if (System.Text.RegularExpressions.Regex.IsMatch(OpenKeyName, @"HKEY_CURRENT_USER\\", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    OpenKeyName = System.Text.RegularExpressions.Regex.Replace(OpenKeyName, @"HKEY_CURRENT_USER\\", "");
                    //// レジストリ・キーを開く
                    rKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(OpenKeyName, true);
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(OpenKeyName, @"HKEY_CLASSES_ROOT\\", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    OpenKeyName = System.Text.RegularExpressions.Regex.Replace(OpenKeyName, @"HKEY_CLASSES_ROOT\\", "");
                    rKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(OpenKeyName, true);
                }
                else if (System.Text.RegularExpressions.Regex.IsMatch(OpenKeyName, @"HKEY_LOCAL_MACHINE\\", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    OpenKeyName = System.Text.RegularExpressions.Regex.Replace(OpenKeyName, @"HKEY_LOCAL_MACHINE\\", "");
                    rKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(OpenKeyName, true);
                }
                else
                {
                    WriteLine($"※レジストリ設定 キー名:\"{OpenKeyName}\" の オープンに失敗しました");
                    return false;
                }


                string ValueName = null;
                string SubKeyName = null;
                if ((string.IsNullOrWhiteSpace(reegistry.ValueName) == false) && (string.IsNullOrWhiteSpace(reegistry.SubKeyName) == false))
                {
                    WriteLine($"※削除対象 値名 と サブキー 同時に指定できません");

                    return false;
                }
                else if (string.IsNullOrWhiteSpace(reegistry.ValueName) == false)
                {
                    ValueName = reegistry.ValueName;
                    WriteLine($"削除対象は キー:\"{OpenKeyName}\" , 値名:\"{ValueName}\"");
                    //// キー内の値を削除
                    try
                    {
                        rKey.DeleteValue(ValueName, true);
                        WriteLine($"■レジストリ設定 キー:\"{OpenKeyName}\" , 値名:\"{ValueName}\" 削除に成功しました");
                    }
                    catch
                    {
                        throw new System.Exception($"※レジストリ設定 キー:\"{OpenKeyName}\" , 値名:\"{ValueName}\" 削除に失敗しました");
                    }
                }
                else if (string.IsNullOrWhiteSpace(reegistry.SubKeyName) == false)
                {
                    SubKeyName = reegistry.SubKeyName;
                    WriteLine($"削除対象は キー:\"{OpenKeyName}\" , サブキー:\"{SubKeyName}\"");
                    //// キー内の値を削除
                    try
                    {
                        rKey.DeleteSubKey(SubKeyName, true);
                        //設定したレジストリの値をコンソールに表示
                        WriteLine($"■レジストリ設定 キー:\"{OpenKeyName}\" , サブキー:\"{SubKeyName}\" 削除に成功しました");
                    }
                    catch
                    {

                        throw new System.Exception($"※レジストリ設定 キー:\"{OpenKeyName}\" , サブキー:\"{SubKeyName}\" 削除に失敗しました");
                    }

                }
                else
                {
                    WriteLine($"※削除対象 値名 または サブキー が指定されていません");

                    return false;
                }

                //// 開いたレジストリを閉じる
                rKey.Close();

                return true;
            }
            catch (System.Exception ex)
            {
                // レジストリ・キーが存在しない
                WriteLine($"※レジストリ設定 キー名:\"{OpenKeyName}\" 内の 削除に失敗しました {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// ■プロセス実行
        /// </summary>
        /// <param name="executeProcess"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        private bool ProcessExecute(ExecuteProcess executeProcess, byte[] AES_key, byte[] AES_iv, Action<string> WriteLine)
        {
            WriteLine($"■コメント \"{executeProcess.Comment}\"");

            try
            {
                string FileName = null;
                if (string.IsNullOrWhiteSpace(executeProcess.FileName) == false)
                {
                    FileName = executeProcess.FileName;
                }
                else
                {
                    WriteLine($"※プロセス実行指示の FileName 指示がありません");
                    return false;
                }

                string Arguments = null;
                if (string.IsNullOrWhiteSpace(executeProcess.Arguments) == false)
                {
                    Arguments = executeProcess.Arguments;
                }

                string UserName = null;
                if (string.IsNullOrWhiteSpace(executeProcess.UserName) == false)
                {
                    UserName = executeProcess.UserName;
                }

                string PlanePassword = null;
                if (string.IsNullOrWhiteSpace(executeProcess.Password) == false)
                {
                    PlanePassword = executeProcess.Password;
                }

                string DecryptedPassword = null;
                if (string.IsNullOrWhiteSpace(executeProcess.EncryptedPassword) == false)
                {
                    try
                    {
                        DecryptedPassword = EncryptionAES.DecryptFromBase64NoAsync(executeProcess.EncryptedPassword, AES_key, AES_iv);
                        //AdditionalMsgWriteLine($"■パスワード複合化後；{EncryptedPassword}");
                    }
                    catch (System.Exception ex)
                    {
                        WriteLine($"※Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ パスワード複合化で失敗  EncryptedPassword:\"{executeProcess.EncryptedPassword}\"  {ex.Message}");
                        return false;
                    }
                }


                string Domain = null;
                if (string.IsNullOrWhiteSpace(executeProcess.Domain) == false)
                {
                    Domain = executeProcess.Domain;
                }

                ProcessStartInfo psInfo = new ProcessStartInfo();
                psInfo.FileName = FileName; //
                psInfo.Arguments = Arguments; //引数

                psInfo.UserName = UserName; //
                psInfo.Domain = Domain; // ドメイン

                if ((string.IsNullOrEmpty(PlanePassword) == false) && (string.IsNullOrEmpty(DecryptedPassword) == false))
                {
                    WriteLine($"※Password と EncryptedPassword を同時に指定できません。スキップします");
                    return false;
                }
                else if (string.IsNullOrEmpty(PlanePassword) == false)
                {
                    psInfo.Password = GetSecureString(PlanePassword);
                }
                else if (string.IsNullOrEmpty(DecryptedPassword) == false)
                {
                    psInfo.Password = GetSecureString(DecryptedPassword);
                }
                else
                {
                    psInfo.Password = null;
                }



                if (executeProcess.StdOut == true)
                {
                    psInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト
                }

                if (executeProcess.StdErr == true)
                {
                    psInfo.RedirectStandardError = true;
                }

                psInfo.WorkingDirectory = System.IO.Path.GetTempPath();

                psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
                psInfo.UseShellExecute = false; // シェル機能を使用しない

                WriteLine($"■Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ 設定値 Filename:\"{executeProcess.FileName}\" Arguments:\"{executeProcess.Arguments}\" UserName:\"{executeProcess.UserName}\" Domain:\"{executeProcess.Domain}\" Password:\"{executeProcess.Password}\"  EncryptedPassword:\"{executeProcess.EncryptedPassword}\" StdOut:\"{executeProcess.StdOut}\" StdErr:\"{executeProcess.StdErr}\"");

                // https://resanaplaza.com/%E3%80%90%E3%82%B3%E3%83%94%E3%83%9A%E3%81%A7%E5%AE%8C%E4%BA%86%E3%80%91c%E3%81%8B%E3%82%89process%E3%82%92%E4%BD%BF%E3%81%A3%E3%81%A6%E5%A4%96%E9%83%A8%E3%83%97%E3%83%AD%E3%82%B0%E3%83%A9%E3%83%A0/

                using (var process = Process.Start(psInfo))
                {
                    WriteLine($"■Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ プロセスを実行開始・・・\"{executeProcess.FileName}\"");
                    string so = null;
                    string se = null;
                    if (executeProcess.StdOut == true)
                        so = process.StandardOutput.ReadToEnd();
                    if (executeProcess.StdErr == true)
                        se = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    if (executeProcess.StdOut == true)
                    {
                        WriteLine($"■標準出力結果 Filename:\"{executeProcess.FileName}\" Arguments:\"{executeProcess.Arguments}\" UserName:\"{executeProcess.UserName}\" Domain:\"{executeProcess.Domain}\"");
                        WriteLine($"{so}");
                        WriteLine($"■標準出力結果ここまで");
                    }
                    if (executeProcess.StdErr == true)
                    {
                        WriteLine($"■標準エラー出力結果 Filename:\"{executeProcess.FileName}\" Arguments:\"{executeProcess.Arguments}\" UserName:\"{executeProcess.UserName}\" Domain:\"{executeProcess.Domain}\"");
                        WriteLine($"{se}");
                        WriteLine($"■標準エラー出力結果ここまで");
                    }
                    WriteLine($"■Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ プロセスを実行終了・・・");

                    return true;
                }
            }
            catch (System.Exception ex)
            {
                WriteLine($"※Addin追加ｺﾝﾄﾛｰﾙﾌｧｲﾙ プロセス実行指示で例外発生 Filename:\"{executeProcess.FileName}\" Arguments:\"{executeProcess.Arguments}\" UserName:\"{executeProcess.UserName}\" Domain:\"{executeProcess.Domain}\" Password:\"{executeProcess.Password}\"  EncryptedPassword:\"{executeProcess.EncryptedPassword}\" StdOut:\"{executeProcess.StdOut}\" StdErr:\"{executeProcess.StdErr}\" {ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// パスワードセキュアストリングに変換
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private System.Security.SecureString GetSecureString(string text)
        {
            //SecureStringオブジェクトを作成
            System.Security.SecureString secStr = new System.Security.SecureString();

            //AppendChareで一文字づつ追加
            foreach (char c in text)
                secStr.AppendChar(c);
            return secStr;
        }

        /// <summary>
        /// Sourceﾊﾟｽ名からDistﾊﾟｽ名へファイルを取得します。
        /// 
        /// </summary>
        /// <param name="Source">ソース先パス名</param>
        /// <param name="Dist">コピー先パス名</param>
        /// <returns>成功したらtrue</returns>
        private bool GetSourceFileToTemp(string Source, string basePath, string Dist)
        {
            string newsource = FileFolder.FileExistsLoopSearch(Source, basePath, true);
            if (newsource == null)
                return false;

            // bool result = FileFolder.CopyFileWithIncrementedFileName(newsource, Dist);
            Console.WriteLine($"{newsource} を {Dist} へ複製を試みます");
            //bool result = FileFolder.CopyWithFileRotation(newsource, Dist);            bool result = FileFolder.CopyWithFileRotation(newsource, Dist);
            bool result = FileFolder.CopyFile(newsource, Dist, true);
            if (result)
                Console.WriteLine($"{newsource} を {Dist} へ複製 成功");
            else
                Console.WriteLine($"{newsource} を {Dist} へ複製 失敗");
            return result;
        }

    }

}
