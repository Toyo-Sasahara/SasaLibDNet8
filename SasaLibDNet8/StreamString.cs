using NeoSmart.AsyncLock;
using SasaLibDNet8;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SasaLibDNet8
{
    /// <summary>
    /// ストリームを使って文字列をread/writeするクラス
    /// </summary>
    public class StreamString
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        AsyncLock _asyncLockReadString = new AsyncLock();
        public bool LockAsyncMode = true;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ioStream"></param>
        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        /// <summary>
        /// 文字列をストリームから読みだす。
        /// </summary>
        /// <returns></returns>
        //public string ReadString()
        //{
        //    var taskresult = _readString();

        //    return taskresult.Result;
        //}

        /// <summary>
        /// ﾃｽﾄ
        /// </summary>
        /// <returns></returns>
        private async Task<string> _readString()
        {
            using (await _asyncLockReadString.LockAsync())
            {

                Task<string> taskResult = Task.Factory.StartNew(() =>
                {
                    int len;
                    len = ioStream.ReadByte() * 256;
                    len += ioStream.ReadByte();
                    if (len < 0)
                    {
                        len = 0;
                    }
                    byte[] inBuffer = new byte[len];
                    ioStream.Read(inBuffer, 0, len);

                    return streamEncoding.GetString(inBuffer);
                });

                return await taskResult;
            }
        }

        /// <summary>
        /// 文字列をストリームから読みだす。タイムアウト機能付き
        /// </summary>
        /// <param name="timeoutmsec">-1 の時、永久に待つみたい</param>
        /// <returns></returns>
        public string ReadString(int timeoutmsec = 10000, SasaLibDelegateWriteLine WriteLine = null)
        {
            bool OperationCanceledException;
            bool AggregateException;

            return ReadString(timeoutmsec, out OperationCanceledException, out AggregateException, WriteLine);
        }

        /// <summary>
        /// 文字列をストリームから読みだす。タイムアウト機能付き (OperationCanceledException , AggregateException 例外の発生有無をメソッド終了後に 参照できる)
        /// </summary>
        /// <param name="timeoutmsec"></param>
        /// <param name="OperationCanceledException"></param>
        /// <param name="AggregateException"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public string ReadString(int timeoutmsec, out bool OperationCanceledException, out bool AggregateException, SasaLibDelegateWriteLine WriteLine = null)
        {
            OperationCanceledException = false;
            AggregateException = false;

            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var source = new CancellationTokenSource();
            source.CancelAfter(timeoutmsec);


            Task<string> taskResult = Task.Run(() =>
            {
                int len;
                len = ioStream.ReadByte() * 256;
                len += ioStream.ReadByte();
                if (len < 0)
                {
                    len = 0;
                }
                byte[] inBuffer = new byte[len];
                ioStream.Read(inBuffer, 0, len);

                return streamEncoding.GetString(inBuffer);
            });


            try
            {
                taskResult.Wait(source.Token);//OperationCanceledException　（cancellationToken は取り消されました）
                                              //AggregateException　（タスクが取り消されました。 InnerExceptions コレクションに TaskCanceledException オブジェクトが含まれています。
                                              //またはタスクの実行時に例外がスローされました。 InnerExceptions コレクションには、例外に関する情報が含まれています。）
            }
            catch (OperationCanceledException)
            {
                WriteLine($"※SasaLib.StreamString.ReadString(..) OperationCanceledExceptionが発生しました。タイムアウト {timeoutmsec} 。 nullを返します");
                OperationCanceledException = true;
                return null;
            }
            catch (AggregateException exc)
            {
                WriteLine($"※SasaLib.StreamString.ReadString(..) 集約した例外 AggregateExceptionが発生");
                foreach (var e in exc.Flatten().InnerExceptions)
                {
                    WriteLine($"{e.Message} {e.InnerException}");
                }

                AggregateException = true;
                return null;
            }

            return taskResult.Result;
        }



        /// <summary>
        /// 文字列をストリームに書き込む
        /// </summary>
        /// <param name="outString"></param>
        /// <returns></returns>
        public int WriteString(string outString)
        {
            try
            {
                if (outString == null)
                {
                    outString = "";

                    DebugConsole.WriteLine($"SasaLib.StreamString.WriteString(string outString)にnull値が渡されました。\n 空行に変換します");

                    SasaLibDNet8.Eventlog.Log.WriteEntry("SasaLib", EventLogEntryType.Information, 9300, $"SasaLib.StreamString.WriteString(string outString)にnull値が渡されました。\n 空行に変換します");
                }

                byte[] outBuffer = streamEncoding.GetBytes(outString);

                int len = outBuffer.Length;

                if (len > UInt16.MaxValue)
                {
                    len = (int)UInt16.MaxValue;
                }
                ioStream.WriteByte((byte)(len / 256));
                ioStream.WriteByte((byte)(len & 255));
                ioStream.Write(outBuffer, 0, len);
                ioStream.Flush();

                return outBuffer.Length + 2;
            }
            catch (Exception ex)
            {
                SasaLibDNet8.Eventlog.Log.WriteEntry("SasaLib", EventLogEntryType.Error, 9300, $"SasaLib.StreamString.WriteString(string outString) 送信しようとした文字列 \"{outString}\" ,例外発生。 -1 を返します ({ex.Message})");
                return -1;
            }
        }


        /// <summary>
        /// 文字列をストリームに書き込む（タイムアウト機能付き）
        /// </summary>
        /// <param name="outString"></param>
        /// <returns></returns>
        public int WriteString(string outString, int timeoutmsec = 10000, SasaLibDelegateWriteLine WriteLine = null)
        {
            bool OperationCanceledException;
            bool AggregateException;
            return WriteString(outString, timeoutmsec, out OperationCanceledException, out AggregateException, WriteLine);
        }

        public int WriteString(string outString, int timeoutmsec, out bool OperationCanceledException, out bool AggregateException, SasaLibDelegateWriteLine WriteLine = null)
        {
            OperationCanceledException = false;
            AggregateException = false;

            try
            {
                if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

                var source = new CancellationTokenSource();
                source.CancelAfter(timeoutmsec);

                Task<int> taskResult = Task.Factory.StartNew(() =>
                {
                    if (outString == null)
                    {
                        outString = "";

                        SasaLibDNet8.Eventlog.Log.WriteEntry("SasaLib", EventLogEntryType.Error, 9300, $"SasaLib.StreamString.WriteString(string outString)にnull値が渡されました。\n 空行に変換します");
                    }

                    byte[] outBuffer = streamEncoding.GetBytes(outString);

                    int len = outBuffer.Length;

                    if (len > UInt16.MaxValue)
                    {
                        len = (int)UInt16.MaxValue;
                    }

                    try
                    {
                        ioStream.WriteByte((byte)(len / 256));
                        ioStream.WriteByte((byte)(len & 255));
                        ioStream.Write(outBuffer, 0, len);
                        ioStream.Flush();
                    }
                    catch (Exception ex)
                    {
                        WriteLine($"※SasaLib.StreamString.WriteString(..)  例外が発生しました。{ex.Message} {ex.InnerException}");
                    }

                    return outBuffer.Length + 2;
                }, source.Token);

                try
                {
                    taskResult.Wait(source.Token);//OperationCanceledException　（cancellationToken は取り消されました）
                                                  //AggregateException　（タスクが取り消されました。 InnerExceptions コレクションに TaskCanceledException オブジェクトが含まれています。
                                                  //またはタスクの実行時に例外がスローされました。 InnerExceptions コレクションには、例外に関する情報が含まれています。）
                }
                catch (OperationCanceledException)
                {
                    WriteLine($"※SasaLib.StreamString.WriteString(..) OperationCanceledExceptionが発生しました。タイムアウト {timeoutmsec} 。 nullを返します");
                    OperationCanceledException = true;
                    return -1;
                }
                catch (AggregateException exc)
                {

                    WriteLine($"※SasaLib.StreamString.WriteString(..) 集約した例外 AggregateExceptionが発生");
                    foreach (var e in exc.Flatten().InnerExceptions)
                    {
                        WriteLine($"{e.Message} {e.InnerException}");
                    }

                    AggregateException = true;
                    return -1;
                }

                return taskResult.Result;

            }
            catch (Exception ex)
            {
                WriteLine($"※SasaLib.StreamString.WriteString(..)  例外が発生しました。{ex.Message} {ex.InnerException}　nullを返します");

                return -1;
            }

        }

        public bool IoStreamFlush()
        {
            try
            {
                ioStream.Flush();
                return true;
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"※StreamString.IoStreamFlush(..)にて例外検知  {ex.Message} {ex.InnerException}");
                return false;
            }
        }
    }
}
