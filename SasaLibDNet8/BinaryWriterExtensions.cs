using System.Diagnostics;
using System.IO;

namespace SasaLib.PIPE
{
    // BinaryWriterの拡張メソッドを定義
    // https://gist.github.com/ichiroku11/80faa8675c5354245001759733df1348
    public static class BinaryWriterExtensions
    {
        // 2024年1月22日 不用と判断したためコメント・様子見
        //public static void WriteObject<TObject>(this BinaryWriter writer, TObject obj, ref int WriteBtyeLength, int writebufsize = 1024 * 20)
        //{
        //    var converter = new ObjectConverter<TObject>();
        //    var bytes = converter.ToByteArray(obj);

        //    //全体の長さを書き込む
        //    WriteBtyeLength = bytes.Length;
        //    WriteObject(writer, obj, 1024 * 20);
        //}


        /// <summary>
        // オブジェクトの書き込み
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="writer">BinaryWriter</param>
        /// <param name="obj">送信するオブジェクト</param>
        /// <param name="writebufsize">バッファーサイズ</param>
        /// <param name="Verbose">コンソールにデバッグ情報表示</param>
        /// <param name="debugMsg">例外発生時にイベントビューアに記録する追加メッセージ・ﾃﾞﾊﾞｯｸﾞ目的</param>
        /// <returns>送信したバイト数</returns>
        //public static void WriteObject<TObject>(this BinaryWriter writer, TObject obj, int writebufsize = 1024 * 20, bool Verbose = false, string debugMsg = null)
        public static int WriteObject<TObject>(this BinaryWriter writer, TObject obj,int writebufsize = 1024*20,bool Verbose = false, string debugMsg = null)
        {
            int length=0;

            try
            {
                var converter = new ObjectConverter<TObject>();
                var bytes = converter.ToByteArray(obj);
#if DEBUG
                if (Verbose)
                    Trace.Write($"WriteObject()送信開始   送信サイズ = {bytes.Length}");
#endif

                //全体の長さを書き込む
                length = bytes.Length;
                writer.Write(length);

                #region 送出バッファリング
                // 送出するbyte[]の長さが writebufsizeより小さいならそのまま送出
                if (length < writebufsize)
                {
                    writer.Write(bytes, 0, length);
                }
                // byte[]の長さが writebufsize 以上ならwritebufsizeで分割して送る
                else
                {
                    Trace.Write($",バッファサイズ = {writebufsize}・・・");

                    for (int i = 0; i < length; i = i + writebufsize)
                    {
                        int v = length - i;
                        if (v < writebufsize)
                        {
                            writebufsize = v;
                        }
                        ///Console.WriteLine($"writer.Write({i},{writebufsize})");
                        writer.Write(bytes, i, writebufsize);
                    }
                }
                #endregion
#if DEBUG
                if (Verbose)
                    Trace.WriteLine($"送信完了");
#endif
            }
            catch (IOException ioe)
            {
                Trace.WriteLine($"void WriteObject<TObject>(...)にて例外 {ioe.Message}");
                Eventlog.Log.WriteEntry("SasaLib", EventLogEntryType.Error, 0, $"▲WriteObject(...),失敗,Exception={ioe.Message} 長さ={length} debugMsg=\"{debugMsg}\"");
            }

            return length;
        }
        
        //分割せずに送出する。OSによって送られるデータの最大値がちがうっぽい？
        public static void WriteObjectNotSplit<TObject>(this BinaryWriter writer, TObject obj, int writebufsize = 2048, bool Verbose = false)
        {
            int length = 0;

            try
            {
                var converter = new ObjectConverter<TObject>();
                var bytes = converter.ToByteArray(obj);
                if (Verbose)
                    Debug.Write($"WriteObject()送信開始   bytes.length = {bytes.Length}・・");

                // 長さを書き込んでから
                length = bytes.Length;
                writer.Write(length);
                // バイト配列を書き込む
                writer.Write(bytes);

                if (Verbose)
                    Debug.WriteLine($"送信完了");
            }
            catch (IOException ioe)
            {
                Eventlog.Log.WriteEntry("SasaLib", EventLogEntryType.Error, 0, $"▲WriteObject(...),失敗,Exception={ioe.Message} 長さ={length}");
            }
        }
    }
}