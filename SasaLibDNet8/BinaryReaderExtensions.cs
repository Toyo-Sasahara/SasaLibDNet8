using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SasaLib.PIPE
{
    // BinaryReaderの拡張メソッドを定義
    // https://gist.github.com/ichiroku11/80faa8675c5354245001759733df1348
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// 分割して読み込む
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="reader"></param>
        /// <param name="readbufsize"></param>
        /// <returns></returns>
        public static TObject ReadObject<TObject>(this BinaryReader reader, int readbufsize = 1024 * 20, SasaLibDelegateWriteLine WriteLine = null , bool Verbose = false)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            // 長さを読み込んでから、バイト配列を読み込む
            var length = 0;
            try
            {
                length = reader.ReadInt32();

                if (Verbose)
                    WriteLine($"ReadObject()受信開始   受信サイズ ={length}・・");

                // もし読み込んだサイズlengthがreadbufsizeより小さければ、内部バッファはlengthとする
                if (length < readbufsize)
                {
                    readbufsize = length;

                    if (Verbose)
                        WriteLine($"バッファサイズ設定={readbufsize}・・");
                }
                else
                {

                    if (Verbose)
                        WriteLine($"バッファサイズ設定={readbufsize}・・");
                }

            }
            catch (IOException ioe)
            {
                WriteLine($"▲BinaryReaderExtensions.ReadObject(..) 失敗. Message : \"{ioe.Message}\" StackTrace : \"{ioe.StackTrace}\"");
                //var dummy1 = new ObjectConverter<TObject>();
                //byte[] ret1 = new byte[0];
                ////var ret = dummy1.FromByteArray(ret1.ToArray()); エラーになる

                //var ret = (object)ret1.ToArray();
                //return (TObject)ret;
                return default(TObject);
            }
            catch (Exception ex)
            {
                WriteLine($"▲BinaryReaderExtensions.ReadObject(..) 失敗. Message : \"{ex.Message}\" StackTrace : \"{ex.StackTrace}\"");
                //var dummy1 = new ObjectConverter<TObject>();
                //byte[] ret1 = new byte[0];
                ////var ret = dummy1.FromByteArray(ret1.ToArray()); エラーになる

                //var ret = (object)ret1.ToArray();
                //return (TObject)ret;
                return default(TObject);
            }

            // 最終受信配列を確保
            byte[] bytesall = new byte[length];

            int readedByte = 0;
            int tempPercent = 0;
            for (int i = 0; i < length; i = i + readbufsize)
            {
                int v = length - i;
                if (v < readbufsize)
                {
                    readbufsize = v;
                }
                var bytes = reader.ReadBytes(readbufsize);
                bytes.CopyTo(bytesall, readedByte);
                readedByte = readedByte + bytes.Length;

                #region 受信割合表示
                if (Verbose)
                {
                    int percent = (int)((double)readedByte / (double)length * 100.0);
                    if (percent != tempPercent)
                    {
                        WriteLine($"{percent}%");
                    }
                    tempPercent = percent;
                }
                #endregion
            }
            if (Verbose)
                WriteLine($"ReadObject(..) 受信完了 受信サイズ bytesall.length={bytesall.Length}");

            var converter = new PIPE.ObjectConverter<TObject>();

            TObject result = converter.FromByteArray(bytesall.ToArray());

            return result;
        }


        /// <summary>
        /// 分割して読み込む.タイムアウト付き
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="reader"></param>
        /// <param name="readbufsize"></param>
        /// <param name="Verbose"></param>
        /// <param name="timeoutmsec"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public static TObject ReadObject<TObject>(this BinaryReader reader, int timeoutmsec, int readbufsize = 1024 * 20, SasaLibDelegateWriteLine WriteLine = null, bool Verbose = false)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var source = new CancellationTokenSource();
            source.CancelAfter(timeoutmsec);

            Task<TObject> taskResult = Task.Factory.StartNew(() =>
            {


                // 長さを読み込んでから、バイト配列を読み込む
                var length = 0;
                try
                {
                    length = reader.ReadInt32();

                    if (Verbose)
                        WriteLine($"ReadObject()受信開始   受信サイズ ={length}・・");

                    // もし読み込んだサイズlengthがreadbufsizeより小さければ、内部バッファはlengthとする
                    if (length < readbufsize)
                    {
                        readbufsize = length;

                        if (Verbose)
                            WriteLine($"バッファサイズ設定={readbufsize}・・");
                    }
                    else
                    {

                        if (Verbose)
                            WriteLine($"バッファサイズ設定={readbufsize}・・");
                    }

                }
                catch (IOException ioe)
                {
                    WriteLine($"▲BinaryReaderExtensions.ReadObject(..) 失敗. Message : \"{ioe.Message}\" StackTrace : \"{ioe.StackTrace}\"");
                    //var ret = new ObjectConverter<TObject>();
                    //byte[] ret1 = new byte[0];
                    ////
                    //return ret.FromByteArray(ret1.ToArray());
                    return default(TObject);

                }
                catch (Exception ex)
                {
                    WriteLine($"▲BinaryReaderExtensions.ReadObject(..) 失敗. Message : \"{ex.Message}\" StackTrace : \"{ex.StackTrace}\"");
                    //var ret = new ObjectConverter<TObject>();
                    //byte[] ret1 = new byte[0];
                    ////
                    //return ret.FromByteArray(ret1.ToArray());
                    return default(TObject);

                }

                // 最終受信配列を確保
                byte[] bytesall = new byte[length];

                int readedByte = 0;
                int tempPercent = 0;
                for (int i = 0; i < length; i = i + readbufsize)
                {
                    int v = length - i;
                    if (v < readbufsize)
                    {
                        readbufsize = v;
                    }
                    var bytes = reader.ReadBytes(readbufsize);
                    bytes.CopyTo(bytesall, readedByte);
                    readedByte = readedByte + bytes.Length;

                    #region 受信割合表示
                    if (Verbose)
                    {
                        int percent = (int)((double)readedByte / (double)length * 100.0);
                        if (percent != tempPercent)
                        {
                            WriteLine($"{percent}%");
                        }
                        tempPercent = percent;
                    }
                    #endregion
                }
                if (Verbose)
                    WriteLine($"受信完了 受信サイズ bytesall.length={bytesall.Length}");

                var converter = new PIPE.ObjectConverter<TObject>();

                TObject result = converter.FromByteArray(bytesall.ToArray());

                return result;

            }, source.Token);

            try
            {
                taskResult.Wait(source.Token);//OperationCanceledExceptionが発生します。
                                              //t.Wait();//AggregateExceptionが発生します。
            }
            catch (OperationCanceledException)
            {
                WriteLine($"※SasaLib.BinaryReaderExtensions.ReadObject(..) タイムアウト {timeoutmsec} , OperationCanceledExceptionが発生しました。default(TObject)を返します");
                return default(TObject);
            }
            catch (AggregateException)
            {
                WriteLine($"※SasaLib.BinaryReaderExtensions.ReadObject(..) タイムアウト {timeoutmsec} , AggregateExceptionが発生しました。default(TObject)を返します");
                return default(TObject);
            }

            return taskResult.Result;

        }

        /// <summary>
        ///  分割せずに読み込む。65535でエラー
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static TObject ReadObjectNotSplit<TObject>(this BinaryReader reader)
        {
            // 長さを読み込んでから、バイト配列を読み込む
            var length = reader.ReadInt32();
            var bytes = reader.ReadBytes(length);

            var converter = new ObjectConverter<TObject>();
            return converter.FromByteArray(bytes);
        }
    }
}