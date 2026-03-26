using SasaLib;
//using SasaLib.Winlogon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.SysConfigurator
{
    //  SomeDelegate という名前のデリゲート型を定義
    public delegate void LogMsgDelegate(string logtext);

    [SupportedOSPlatform("windows")]
    public class SysConfiguratorLog
    {
        /// <summary>
        /// デリゲート
        /// </summary>
        public LogMsgDelegate DelegateSubWriteLine { get; set; }

        /// <summary>
        /// 現在のロギング先フォルダ
        /// </summary>
        public string CurrnetLogFolder { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string LogFileName { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string RecentLogFullFileName { get; private set; }

        /// <summary>
        /// ローカル保存先
        /// CurrnetLogFolderに書き込めないときに使用するフォルダ
        /// </summary>
        string LocalLogFolder = System.IO.Path.GetTempPath();

        /// <summary>
        /// ログ識別名 
        /// </summary>
        string Suffix = "";

        string identifier = null;

        // あとから指定する表示ﾊﾞｰｼﾞｮﾝ
        //public string LogVersionName { get; set; }

        //public string LogPrefixName { get; set; } = "未設定";

        //private readonly string pipename;

        /// <summary>
        ///
        /// </summary>
        private string DomainName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private string UserPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private bool ClsLogon { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="LogFileFolder">ログ作成フォルダ</param>
        /// <param name="identifier">識別用文字列</param>
        public SysConfiguratorLog(string LogFileFolder, string identifier)
        {
            this.identifier = identifier;
            this.CurrnetLogFolder = LogFileFolder;
            this.LogFileName = $"SysConfigurator_[{Environment.UserName}]{Suffix}.log";

        }

        ~SysConfiguratorLog()
        {

        }

        public void SetClsLogon(string DomainName, string UserName, string UserPassword, bool ClsLogon)
        {
            this.DomainName = DomainName;
            this.UserName = UserName;
            this.UserPassword = UserPassword;
            this.ClsLogon = ClsLogon;

            WriteLine($"■Log.SetClsLogon(..)呼び出されました。DomainName:{DomainName} UserName:{UserName} UserPassword:{UserPassword} ClsLogon:{ClsLogon}");
        }


        /// <summary>
        /// ログファイルのサフィックスを設定
        /// ホスト名-{ユーザー名}{サフィックス}.log
        /// </summary>
        /// <param name="suffix"></param>
        public void SetSuffix(string suffix)
        {
            this.Suffix = suffix;
        }

        /// <summary>
        /// ロギングフォルダ変更
        /// </summary>
        /// <param name="newLoggingFolder"></param>
        /// <returns></returns>
        public bool ChangeLoggingFolder(string newLoggingFolder)
        {
            if (string.IsNullOrWhiteSpace(newLoggingFolder))
            {
                WriteLine($"※Log.ChangeLoggingFolder(..) エラー 引数がまちがっています。");
                return false;
            }

            // 現在の保存先を退避
            string oldLoggingFolder = CurrnetLogFolder;

            bool result = false;

            var dummy = new WithFakeAccount(DomainName, UserName, UserPassword, ClsLogon, () =>
            {
                WriteLine($"■現在のロギング先 {CurrnetLogFolder} です。");
                Uri u = new Uri(newLoggingFolder);

                if (u.IsUnc)
                {
                    WriteLine($"■ログファイル切替先{newLoggingFolder} はUNCファイルです");

                    if (FileFolder.IsNetworkFolderExists(newLoggingFolder) == false)
                    {
                        WriteLine($"※ロギングフォルダ {newLoggingFolder}にアクセスできませんでした");
                        WriteLine($"※ロギングフォルダを {oldLoggingFolder} から {newLoggingFolder} へ変更できませんでした。{CurrnetLogFolder} を使います。");
                        result =  false;
                    }
                    else
                    {
                        WriteLine($"■ロギングフォルダ {newLoggingFolder}はアクセス可能です");
                    }
                }

                if (FileCreateTest(newLoggingFolder))
                {
                    WriteLine($"■{newLoggingFolder}は書き込み可能でした。以降書き込み先を切り替えます。\r\n");

                    CurrnetLogFolder = newLoggingFolder; // プロパティ CurrentLogFolder を変更します。

                    WriteLine($"■ロギングフォルダを {oldLoggingFolder} から {newLoggingFolder} へ変更しました。");
                    result = true;
                }
                else
                {
                    WriteLine($"※ロギングフォルダを {oldLoggingFolder} から {newLoggingFolder} へ変更できませんでした。{CurrnetLogFolder} を使います。");

                    result = false;
                }
            });

            return result;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="Msg"></param>
        public void WriteLine(string Msg)
        {
            // DelegateSubWriteLineがnull出ない場合はデリゲート先を呼び出します。
            if (DelegateSubWriteLine != null)
            {
                DelegateSubWriteLine(Msg);
            }

            if (CurrentLogFolderFileCreatetest() == false)
            {
                ChangeLoggingFolder(LocalLogFolder);
                WriteLine($"{CurrnetLogFolder}へのﾛｸﾞﾒｯｾｰｼﾞのﾃｽﾄ書込みができないため{LocalLogFolder}へ変更しました");
            }

            Logging Mylog;
            Mylog = new Logging(CurrnetLogFolder, LogFileName);

            if (string.IsNullOrWhiteSpace(identifier))
                Mylog.WriteLine(Msg);
            else
                Mylog.WriteLine(identifier + " " + Msg);

            RecentLogFullFileName = Mylog.GetLogFilePath();
            Mylog.Close();
        }


        /// <summary>
        /// ファイル作成が可能かを調査する
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        bool FileCreateTest(string folder)
        {
            string testpath = System.IO.Path.Combine(folder, Path.GetRandomFileName());
            try
            {
                bool ans = FileFolder.MakeDirectory(folder);

                if (ans == true)
                {
                    if (!File.Exists(testpath))
                    {
                        File.Create(testpath).Close();

                        File.Delete(testpath);

                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    WriteLine($"FileCreateTest(). ディレクトリ {folder} 作成テストに失敗");
                    return false;
                }

            }
            catch (Exception ex)
            {
                WriteLine($"FileCreateTest()にて例外 {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool CurrentLogFolderFileCreatetest()
        {
            string testpath = System.IO.Path.Combine(CurrnetLogFolder, Path.GetRandomFileName());
            try
            {
                FileFolder.MakeDirectory(CurrnetLogFolder);

                if (!File.Exists(testpath))
                {
                    File.Create(testpath).Close();

                    File.Delete(testpath);

                    return true;
                }
                else
                    return false;

            }
            catch (Exception ex)
            {
                WriteLine($"CurrentLogFolderFileCreatetest()にて例外 {ex.Message}");
                Eventlog.Log.WriteEntry("AutocadTOYOaddin", EventLogEntryType.Error, 0, $"※Log.CurrentLogFolderFileCreatetest()にて例外 {ex.Message}");

                return false;
            }

        }
    }
}
