using SasaLib;
using System.Diagnostics;
using System.IO;
namespace SasaLibDNet8.LogSystem
{
    public class LogSystem : IDisposable
    {
        /// <summary>
        /// デリゲート
        /// </summary>
        public Action<string> DelegateSubWriteLine { get; set; }

        /// <summary>
        /// 現在のロギング先フォルダ
        /// </summary>
        public string CurrnetLogFolder { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string CurrnetLogFullFileName { get; private set; }

        /// <summary>
        /// ローカル保存先
        /// CurrnetLogFolderに書き込めないときに使用するフォルダ
        /// </summary>
        readonly string LocalLogFolder = System.IO.Path.GetTempPath();

        SasaLib.Logging logging;

        /// <summary>
        /// ログ識別名 
        /// </summary>
        string Identifier = null;


        /// <summary>
        /// あとから指定する表示ﾊﾞｰｼﾞｮﾝ
        /// </summary>
        public string CadDisplayVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ToyoAddinVersion { get; set; }


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
        /// <param name="logFileFolder">ログ作成フォルダ</param>
        /// <param name="identifier">識別用文字列</param>
        public LogSystem(string logFileFolder, string identifier)
        {
            this.Identifier = identifier;

            this.CurrnetLogFolder = logFileFolder;

            logging = CreateLogging(logFileFolder);

            if (logging != null)
            {

                WriteLine($"■SasaLib.Logging オブジェクトを 生成しました。CurrnetLogFolder={CurrnetLogFolder},identifier={Identifier}");
            }
            else
            {
                WriteLine($"※SasaLib.Logging  オブジェクトの 生成に失敗しました。CurrnetLogFolder={CurrnetLogFolder},identifier={Identifier}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            logging.Close();
        }

        /// <summary>
        /// 偽装ログオン情報をクラスフィールドに設定します
        /// </summary>
        /// <param name="DomainName">ドメイン名</param>
        /// <param name="UserName">ユーザー名</param>
        /// <param name="UserPassword">平文パスワード</param>
        /// <param name="ClsLogon">true:の時偽装ログオンを実行、false:の時は偽装ログオン情報は無視し実行中のアカウントを使います</param>
        public void SetClsLogon(string DomainName, string UserName, string UserPassword, bool ClsLogon)
        {
            this.DomainName = DomainName;
            this.UserName = UserName;
            this.UserPassword = UserPassword;
            this.ClsLogon = ClsLogon;

            WriteLine($"■Log.SetClsLogon(..)呼び出されました。DomainName:{DomainName} UserName:{UserName} UserPassword:{UserPassword} ClsLogon:{ClsLogon}");
        }

        /// <summary>

        /// <summary>
        /// ロギングフォルダ変更
        /// </summary>
        /// <param name="newLoggingFolder"></param>
        /// <returns></returns>
        public bool ChangeLoggingFolder(string newLoggingFolder, out string aaa)
        {
            aaa = null;
            if (string.IsNullOrWhiteSpace(newLoggingFolder))
            {
                WriteLine($"※Log.ChangeLoggingFolder(..) エラー 引数がまちがっています。");
                return false;
            }


            // 現在の保存先を退避
            string oldLoggingFolder = CurrnetLogFolder;

            WithFakeAccount resultFake = new WithFakeAccount(DomainName, UserName, UserPassword, ClsLogon, () =>
            {
                WriteLine($"■現在のロギング先 {CurrnetLogFolder} です。");
                if (ClsLogon)
                {
                    WriteLine($"■偽装ログオンを指定されています。ドメイン: {DomainName} , ユーザー名: {UserName} です。");
                }

                Uri u = new Uri(newLoggingFolder);

                if (u.IsUnc)
                {
                    WriteLine($"■ログファイル切替先{newLoggingFolder} はUNCファイルです");

                    if (FileFolder.IsNetworkFolderExists(newLoggingFolder) == false)
                    {
                        WriteLine($"※ロギングフォルダ {newLoggingFolder}にアクセスできませんでした");
                        WriteLine($"※ロギングフォルダを {oldLoggingFolder} から {newLoggingFolder} へ変更できませんでした。{CurrnetLogFolder} を使います。");
                        return false;
                    }
                    else
                    {
                        if (FileCreateTest(newLoggingFolder))
                        {
                            WriteLine($"■ロギングフォルダ {newLoggingFolder} はアクセス可能です");
                        }
                    }
                }
                bool result = FileCreateTest(newLoggingFolder);
                if (result)
                {
                    WriteLine($"\r\n■{oldLoggingFolder} から {newLoggingFolder} へ書き込み先を変更しました。");


                    logging = CreateLogging(newLoggingFolder);


                    if (logging != null)
                    {
                        CurrnetLogFolder = newLoggingFolder; // プロパティ CurrentLogFolder を変更します。

                        WriteLine($"■SasaLib.Logging オブジェクトを 再生成しました。logFileFolder={CurrnetLogFolder},identifier={Identifier}");
                    }
                    else
                    {
                        WriteLine($"※SasaLib.Logging  オブジェクトの 再生成に失敗しました。logFileFolder={CurrnetLogFolder},identifier={Identifier}");
                        return false;
                    }
                }
                else
                {
                    WriteLine($"※ロギングフォルダを {oldLoggingFolder} から {newLoggingFolder} へ変更できませんでした。{oldLoggingFolder} を使います。");
                    return false;

                }

                return true;

            });

            object fakeresult = resultFake.GetResult();

            return (bool)fakeresult;


        }

        private Logging CreateLogging(string baseFolder)
        {
            string _addinVer = null;
            string _cadVer = null;
            if (ToyoAddinVersion != null)
                _addinVer = $"({ToyoAddinVersion})";
            if (CadDisplayVersion != null)
                _cadVer = $"({CadDisplayVersion})";
            var logging = new SasaLib.Logging(baseFolder, $"{this.Identifier}{_addinVer}{_cadVer}[{Environment.UserName}].log");

            return logging;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLine(string Msg)
        {
            if (SasaLib.FileFolder.IsLocalPath(CurrnetLogFolder))
            {
                ClsLogon = false;
            }

            new WithFakeAccount(DomainName, UserName, UserPassword, ClsLogon, () =>
            {
                Task.Run(() =>
                {
                    if (FileCreateTest(CurrnetLogFolder) == false)
                    {
                        string aaa;
                        if (ChangeLoggingFolder(LocalLogFolder, out aaa))
                        {
                            WriteLine($"※{CurrnetLogFolder}へのﾛｸﾞﾒｯｾｰｼﾞのﾃｽﾄ書込みができないため{LocalLogFolder}へ変更しました");
                        }
                        else
                        {
                            string source = "InventorTOYOaddin";

                            try
                            {
                                //ソースが存在していない時は、作成する
                                if (!System.Diagnostics.EventLog.SourceExists(source))
                                {
                                    //ログ名を空白にすると、"Application"となる
                                    System.Diagnostics.EventLog.CreateEventSource(source, "");
                                }

                                EventLog.WriteEntry(source, $"※{CurrnetLogFolder}へのﾛｸﾞﾒｯｾｰｼﾞのﾃｽﾄ書込みができないため{LocalLogFolder}へ変更しようとしましたが失敗しました", EventLogEntryType.Error, 0);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                    }


                    if (string.IsNullOrWhiteSpace(Identifier))
                    {
                        logging.LogRotateWriteLine(Msg, FlashSync: true);
                    }
                    else
                    {
                        logging.LogRotateWriteLine(Identifier + " " + Msg, FlashSync: true);
                    }
                    CurrnetLogFullFileName = logging.GetLogFilePath();
                });
            });
        }


        /// <summary>
        /// ファイル作成が可能かを調査する
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public bool FileCreateTest(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
                return false;

            // フォルダが存在するか確認
            if (!Directory.Exists(folderPath))
                return false;

            string testFilePath = Path.Combine(folderPath, $"test_{Guid.NewGuid()}.tmp");

            try
            {
                // 書き込みテスト：一時ファイル作成
                using (FileStream fs = File.Create(testFilePath, 1, FileOptions.DeleteOnClose))
                {
                    // 書き込みテスト
                    byte[] testBytes = System.Text.Encoding.UTF8.GetBytes("Write Test");
                    fs.Write(testBytes, 0, testBytes.Length);
                    fs.Flush();

                    // 読み取りテスト
                    fs.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[testBytes.Length];
                    int bytesRead = fs.Read(buffer, 0, buffer.Length);

                    if (bytesRead != testBytes.Length || System.Text.Encoding.UTF8.GetString(buffer) != "Write Test")
                        return false; // 書き込み・読み取り失敗
                }

                // 例外なく成功すれば true
                return true;
            }
            catch
            {
                return false; // 読み書きできなければ false
            }
            finally
            {
                // テストファイルの削除（安全のため）
                try
                {
                    if (File.Exists(testFilePath))
                        File.Delete(testFilePath);
                }
                catch
                {
                    // 削除失敗時は無視
                }
            }
        }

    }
}
