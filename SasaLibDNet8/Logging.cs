using NeoSmart.AsyncLock;
using SasaLibDummy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SasaLib
{
    public class Logging
    {
        AsyncLock _asyncLock = new AsyncLock();

        //private static System.IO.StreamWriter _sw = null;
        //private static string pathfile;

        private System.IO.StreamWriter _sw = null;
        public string CurrentLogFullFileName { get; private set; }

        private DateTime currentDate;

        IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());

        public string CurrentMethod
        {
            get
            {
                var sf = new System.Diagnostics.StackFrame(1);
                Console.WriteLine(sf);
                return sf.GetMethod().Name;
            }
        }


        /// <summary>
        /// コンストラクタ
        /// ファイル書き出しを開始 現在実行しているコードを格納しているアセンブリのフォルダに書出し
        /// </summary>
        /// <param name="filename">ログファイルネーム(ファイル名のみ)</param>
        public Logging(string filename)
        {
            string destinationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location);
            CurrentLogFullFileName = destinationPath + System.IO.Path.DirectorySeparatorChar + hostInfo.HostName + "-" + filename;

            if (System.IO.File.Exists(CurrentLogFullFileName))
                currentDate = System.IO.File.GetCreationTime(CurrentLogFullFileName);
            else
                currentDate = DateTime.Now;

            try
            {

                DirectoryInfo destinationDirectory = new DirectoryInfo(destinationPath);


                //コピー先のディレクトリがなければ作成する
                if (destinationDirectory.Exists == false)
                    destinationDirectory.Create();
                Console.WriteLine("Logファイル:{0}", CurrentLogFullFileName);
                _sw = new System.IO.StreamWriter(CurrentLogFullFileName, true);
            }
            catch (Exception ex)
            {

                Console.WriteLine("ログファイルに書き込めません\r[" + CurrentLogFullFileName + "]" + ex.Message);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// ファイル書き出しを開始　フォルダとファイル名を指定  $"{hostInfo.HostName}-{suffixLogfilename}"がファイル名として作成されます。
        /// </summary>
        /// <param name="folder">ログを書き出すフォルダ名</param>
        /// <param name="suffixLogfilename">ログファイル名の後半部と拡張子 例： sasahara.log （左記の先頭にメソッド内にて$"{hostInfo.HostName}-"が追加されます）</param>
        public Logging(string folder, string suffixLogfilename, bool append = true)
        {
            CurrentLogFullFileName = System.IO.Path.Combine(folder, hostInfo.HostName + "-" + suffixLogfilename);

            if (System.IO.File.Exists(CurrentLogFullFileName))
                currentDate = System.IO.File.GetCreationTime(CurrentLogFullFileName);
            else
                currentDate = DateTime.Now;

            try
            {
                if (System.IO.Directory.Exists(folder) == false)
                {
                    Debug.WriteLine($"{folder}がありません。");
                    try
                    {
                        System.IO.Directory.CreateDirectory(folder);
                        Debug.WriteLine($"{folder}を作成しました");
                    }
                    catch (Exception ex) { Debug.WriteLine($"{folder}を作成できません{ex.Message}"); }
                }
                _sw = new System.IO.StreamWriter(CurrentLogFullFileName, append);
            }
            catch
            {
                string newpathfile = System.IO.Path.Combine(Path.GetTempPath(), hostInfo.HostName + "-" + Path.GetRandomFileName() + suffixLogfilename);
                Debug.WriteLine($"ファイル[{CurrentLogFullFileName}]に書き込めません[{newpathfile}]を使用します");
                _sw = new System.IO.StreamWriter(newpathfile, append);
            }
        }

        ~Logging()
        {
            Close();
        }

        /// <summary>
        /// ファイル書き出しを終了
        /// </summary>
        public void Close()
        {
            if (_sw != null)
            {
                // CloseをDisposeに変えてみた

                try
                {
                    _sw.Dispose();
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"SasaLib.Logging.Close()にて例外発生。{ex.Message}");
                }
            }
        }

        /// <summary>
        /// 書き込み(WriteLine)
        /// </summary>
        /// <param name="value">書き込む値</param>
        /// <param name="DebugWriteLineSwitch">出力を Debug.WriteLine(..) によっても実行する</param>
        /// <param name="ConsoleWriteLineSwitch">出力を Console.WriteLine(..) によっても実行する</param>
        public async void WriteLine(object value, bool DebugWriteLineSwitch = false, bool ConsoleWriteLineSwitch = false, bool FlashSync = false)
        {
            using (await _asyncLock.LockAsync())
            {

                try
                {
                    if (_sw.BaseStream != null)
                    {
                        string datetime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        var writeString = @"[" + datetime + @"] " + value;

                        if (DebugWriteLineSwitch)
                            Debug.WriteLine(writeString);
                        if (ConsoleWriteLineSwitch)
                            Console.WriteLine(writeString);

                        await _sw.WriteLineAsync(writeString);

                        if (FlashSync)
                        {
                            Flash();
                        }
                    }
                }
                catch (IOException ioe)
                {
                    System.Console.WriteLine($"Logging.WriteLine() 例外 {ioe.Message}");
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"Logging.WriteLine() 例外 {e.Message}");
                    //エラーのためクローズはしない
                    //_sw.Close();
                }
            }
        }

        public async Task<string> ResultWriteLine(object value, bool DebugWriteLineSwitch = false, bool ConsoleWriteLineSwitch = false, bool FlashSync = false)
        {
            using (await _asyncLock.LockAsync())
            {

                try
                {
                    if (_sw.BaseStream != null)
                    {
                        string datetime = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        var writeString = @"[" + datetime + @"] " + value;

                        if (DebugWriteLineSwitch)
                            Debug.WriteLine(writeString);
                        if (ConsoleWriteLineSwitch)
                            Console.WriteLine(writeString);

                        await _sw.WriteLineAsync(writeString);

                        if (FlashSync)
                        {
                            Flash();
                        }

                        return null;
                    }
                    else
                    {
                        return $"StreamWriter.BaseStream が null でした";
                    }
                }
                catch (IOException ioe)
                {
                    System.Console.WriteLine($"Logging.WriteLine() 例外 {ioe.Message}");
                    return $"\"{ioe.Message}\",\"{ioe.InnerException}\"";
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"Logging.WriteLine() 例外 {e.Message}");
                    return $"\"{e.Message}\",\"{e.InnerException}\"";
                }
            }
        }

        /// <summary>
        /// 書き込み(自動ログローテート)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="switchTime"></param>
        /// <param name="DebugWriteLineSwitch"></param>
        /// <param name="ConsoleWriteLineSwitch"></param>
        [SupportedOSPlatform("windows")]
        public async void LogRotateWriteLine(object value, string switchTime = "Day", bool DebugWriteLineSwitch = false, bool ConsoleWriteLineSwitch = false, bool FlashSync = false)
        {
            using (await _asyncLock.LockAsync())
            {

                string writeString = null;

                try
                {
                    int start = currentDate.Day;
                    int now = DateTime.Now.Day;


                    if (switchTime.ToUpper() == "Month".ToUpper())
                    {
                        start = currentDate.Month;
                        now = DateTime.Now.Month;
                    }
                    else if (switchTime.ToUpper() == "Day".ToUpper())
                    {
                        start = currentDate.Day;
                        now = DateTime.Now.Day;
                    }
                    else if (switchTime.ToUpper() == "Hour".ToUpper())
                    {
                        start = currentDate.Hour;
                        now = DateTime.Now.Hour;
                    }
                    else if (switchTime.ToUpper() == "Minute".ToUpper())
                    {
                        start = currentDate.Minute;
                        now = DateTime.Now.Minute;
                    }
                    else
                    {
                        WriteLine($"ログローテート指定パラメータに問題  switchTime:\"{switchTime}\" は正しくない。\"Day\" に変更しました");
                        start = currentDate.Day;
                        now = DateTime.Now.Day;
                    }

                    if (start != now)
                    {
                        var filenameWithioutExtension = System.IO.Path.GetFileNameWithoutExtension(CurrentLogFullFileName); // 現在のログファイル名を得る（拡張子無し）
                        var Extension = System.IO.Path.GetExtension(CurrentLogFullFileName); // 現在のログファイル名の拡張子を得る
                        var directory = System.IO.Path.GetDirectoryName(CurrentLogFullFileName); // 現在のログファイル名のフォルダ名を得る
                        var oldString = currentDate.ToString("yyyy年MM月dd(ddd),HH時mm分から"); // ログローティーション後のファイル名に付与するﾀｲﾑｽﾀﾝﾌﾟ文字列をを組立てる
                        var copyFileName = $"{filenameWithioutExtension}_{oldString}{Extension}"; // ログローティーション後のファイル名を組立てる
                        var copyFullPath = System.IO.Path.Combine(directory, copyFileName); // ログローティーション後のﾌﾙﾊﾟｽ名を組立てる

                        await _sw.FlushAsync();
                        try
                        {
                            _sw.Close(); // 現在書き出し中のログを閉じる
                        }
                        catch { }


                        System.IO.File.Copy(CurrentLogFullFileName, copyFullPath, true); // 閉じた直後のログをログローティーション後のフルパス名にて複製する
                        System.IO.File.SetCreationTime(copyFullPath, currentDate); // コピーしたアーカイブログファイルの作成日をコピー元に変更しておきます
                        System.IO.File.Delete(CurrentLogFullFileName); // 不要になったので一旦削除します
                        _sw = new System.IO.StreamWriter(CurrentLogFullFileName, append: false); // 新しいログファイルをオープン（新規作成）する

                        currentDate = DateTime.Now; // 新しいログファイルをオープンした直後の日時を退避する
                    } // ログファイルを追加する直税に、現在のログファイルをオープンした日時と比較し違いが生じていたらログローティーションを実施

                    if (_sw.BaseStream != null)
                    {
                        writeString = @"[" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + @"] " + value;

                        if (DebugWriteLineSwitch)
                            Debug.WriteLine(writeString);
                        if (ConsoleWriteLineSwitch)
                            Console.WriteLine(writeString);

                        await _sw.WriteLineAsync(writeString);

                        if (FlashSync)
                        {
                            Flash();
                        }
                    }

                }
                catch (IOException ioe)
                {
                    if (value != null)
                        Eventlog.Log.WriteEntry("SasaLib.Logging", EventLogEntryType.Error, 4004, $"LogRotateWriteLine(..) value = {value} , 例外検知(IOException) {ioe.Message} {ioe.StackTrace}", false);
                    else
                        Eventlog.Log.WriteEntry("SasaLib.Logging", EventLogEntryType.Error, 4004, $"LogRotateWriteLine(..) value = nullでした , 例外検知(IOException) {ioe.Message} {ioe.StackTrace}", false);
                }
                catch (Exception e)
                {
                    if (value != null)
                    {
                        Eventlog.Log.WriteEntry("SasaLib.Logging", EventLogEntryType.Error, 4004, $"LogRotateWriteLine(..) value = {value} ,  例外検知(Exception) {e.Message} {e.StackTrace}", false);
                    }
                    else
                        Eventlog.Log.WriteEntry("SasaLib.Logging", EventLogEntryType.Error, 4004, $"LogRotateWriteLine(..) value = nullでした , 例外検知(IOException) {e.Message} {e.StackTrace}", false);

                }
            }
        }

        [SupportedOSPlatform("windows")]
        public async Task<string> ResultLogRotateWriteLine(object value, string switchTime = "Day", bool DebugWriteLineSwitch = false, bool ConsoleWriteLineSwitch = false, bool FlashSync = false)
        {
            using (await _asyncLock.LockAsync())
            {

                string writeString = null;

                try
                {
                    int start = currentDate.Day;
                    int now = DateTime.Now.Day;


                    if (switchTime.ToUpper() == "Month".ToUpper())
                    {
                        start = currentDate.Month;
                        now = DateTime.Now.Month;
                    }
                    else if (switchTime.ToUpper() == "Day".ToUpper())
                    {
                        start = currentDate.Day;
                        now = DateTime.Now.Day;
                    }
                    else if (switchTime.ToUpper() == "Hour".ToUpper())
                    {
                        start = currentDate.Hour;
                        now = DateTime.Now.Hour;
                    }
                    else if (switchTime.ToUpper() == "Minute".ToUpper())
                    {
                        start = currentDate.Minute;
                        now = DateTime.Now.Minute;
                    }
                    else
                    {
                        WriteLine($"ログローテート指定パラメータに問題  switchTime:\"{switchTime}\" は正しくない。\"Day\" に変更しました");
                        start = currentDate.Day;
                        now = DateTime.Now.Day;
                    }

                    if (start != now)
                    {
                        var filenameWithioutExtension = System.IO.Path.GetFileNameWithoutExtension(CurrentLogFullFileName); // 現在のログファイル名を得る（拡張子無し）
                        var Extension = System.IO.Path.GetExtension(CurrentLogFullFileName); // 現在のログファイル名の拡張子を得る
                        var directory = System.IO.Path.GetDirectoryName(CurrentLogFullFileName); // 現在のログファイル名のフォルダ名を得る
                        var oldString = currentDate.ToString("yyyy年MM月dd(ddd),HH時mm分から"); // ログローティーション後のファイル名に付与するﾀｲﾑｽﾀﾝﾌﾟ文字列をを組立てる
                        var copyFileName = $"{filenameWithioutExtension}_{oldString}{Extension}"; // ログローティーション後のファイル名を組立てる
                        var copyFullPath = System.IO.Path.Combine(directory, copyFileName); // ログローティーション後のﾌﾙﾊﾟｽ名を組立てる

                        await _sw.FlushAsync();
                        try
                        {
                            _sw.Close(); // 現在書き出し中のログを閉じる
                        }
                        catch { }


                        System.IO.File.Copy(CurrentLogFullFileName, copyFullPath, true); // 閉じた直後のログをログローティーション後のフルパス名にて複製する
                        System.IO.File.SetCreationTime(copyFullPath, currentDate); // コピーしたアーカイブログファイルの作成日をコピー元に変更しておきます
                        System.IO.File.Delete(CurrentLogFullFileName); // 不要になったので一旦削除します
                        _sw = new System.IO.StreamWriter(CurrentLogFullFileName, append: false); // 新しいログファイルをオープン（新規作成）する

                        currentDate = DateTime.Now; // 新しいログファイルをオープンした直後の日時を退避する
                    } // ログファイルを追加する直税に、現在のログファイルをオープンした日時と比較し違いが生じていたらログローティーションを実施

                    if (_sw.BaseStream != null)
                    {
                        writeString = @"[" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + @"] " + value;

                        if (DebugWriteLineSwitch)
                            Debug.WriteLine(writeString);
                        if (ConsoleWriteLineSwitch)
                            Console.WriteLine(writeString);

                        await _sw.WriteLineAsync(writeString);

                        if (FlashSync)
                        {
                            Flash();
                        }
                    }
                    else
                    {
                        return $"StreamWriter.BaseStream が null でした";
                    }

                    return null;

                }
                catch (IOException ioe)
                {
                    if (value != null)
                        Eventlog.Log.WriteEntry("SasaLib.Logging", EventLogEntryType.Error, 4004, $"LogRotateWriteLine(..) value = {value} , 例外検知(IOException) {ioe.Message} {ioe.StackTrace}", false);
                    else
                        Eventlog.Log.WriteEntry("SasaLib.Logging", EventLogEntryType.Error, 4004, $"LogRotateWriteLine(..) value = nullでした , 例外検知(IOException) {ioe.Message} {ioe.StackTrace}", false);

                    return $"\"{ioe.Message}\",\"{ioe.InnerException}\"";
                }
                catch (Exception e)
                {
                    if (value != null)
                    {
                        Eventlog.Log.WriteEntry("SasaLib.Logging", EventLogEntryType.Error, 4004, $"LogRotateWriteLine(..) value = {value} ,  例外検知(Exception) {e.Message} {e.StackTrace}", false);
                    }
                    else
                        Eventlog.Log.WriteEntry("SasaLib.Logging", EventLogEntryType.Error, 4004, $"LogRotateWriteLine(..) value = nullでした , 例外検知(IOException) {e.Message} {e.StackTrace}", false);

                    return $"\"{e.Message}\",\"{e.InnerException}\"";
                }
            }
        }

        public async void Flash()
        {
            using (await _asyncLock.LockAsync())
            {

                if (_sw.BaseStream != null)
                {
                    try
                    {
                        await _sw.FlushAsync();
                    }
                    catch { }
                }
            }
        }

        public string GetLogFilePath()
        {
            if (CurrentLogFullFileName != null)
                return CurrentLogFullFileName;
            else
                return null;
        }
    }
}
