using System;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace SasaLibDNet8
{
    public static class StreamExtensions
    {
        /// <summary>
        /// ストリームからデータを読み込み、バイト配列に格納
        /// バッファあり
        /// </summary>
        /// <param name="st">読み込む対象のストリーム</param>
        /// <param name="bufSize">1回に読み出すバイトサイズ</param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream st, int bufSize)
        {
            byte[] buf = new byte[bufSize]; // 一時バッファ

            using (MemoryStream msTmp = new MemoryStream())
            {

                while (true)
                {
                    // ストリームから一時バッファに読み込む
                    int read = st.Read(buf, 0, buf.Length);

                    if (read > 0)
                    {
                        // 一時バッファの内容をメモリ・ストリームに書き込む
                        msTmp.Write(buf, 0, read);
                    }
                    else
                    {
                        break;
                    }
                }
                // メモリ・ストリームの内容をバイト配列に格納
                return msTmp.ToArray();
            }
        }

        /// <summary>
        /// ストリームからデータを読み込み、バイト配列に格納
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream st)
        {
            using (MemoryStream msTmp = new MemoryStream())
            {
                st.Position = 0;
                st.CopyTo(msTmp);
                // メモリ・ストリームの内容をバイト配列に格納
                return msTmp.ToArray();
            }
        }

        /// <summary>
        /// ストリームの指定ポジションからからデータを読み込み、バイト配列に格納
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream st, long start)
        {
            using (MemoryStream msTmp = new MemoryStream())
            {
                st.Position = start;
                st.CopyTo(msTmp);
                // メモリ・ストリームの内容をバイト配列に格納
                return msTmp.ToArray();
            }
        }

        /// <summary>
        /// Stream の 指定したポジションから　終了ポジションまでを byte[] として取り出す
        /// （試験中）
        /// </summary>
        /// <param name="st"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static byte[] StreamFromGetBytes(Stream st, long start, long end)
        {
            using (Stream distStream = new MemoryStream())
            {
                long saveSize = end - start;
                byte[] dataBytes = new byte[saveSize];
                st.Position = start;
                st.Read(dataBytes, 0, (int)saveSize);
                distStream.Write(dataBytes, 0, dataBytes.Length);

                return dataBytes;
            }
        }

        /// <summary>
        /// Byte配列からStreamを作成
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] by)
        {
            using (MemoryStream msTmp = new MemoryStream(by))
            {
                return msTmp;
            }
        }

        /// <summary>
        /// ストリームをファイルに保存.Position0から始める
        /// </summary>
        /// <param name="input">保存するストリーム</param>
        /// <param name="savepath">ファイルパス</param>
        public static void StreamToFile(Stream input, string savepath)
        {
            input.Position = 0;

            FileStream fs;
            // ファイルストリームを準備

            try
            {
                fs = new System.IO.FileStream(savepath, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                long bufSize = 1024;
                byte[] buffer = new byte[bufSize];

                int len;
                input.Position = 0;
                while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, len);
                }

                fs.Close();

            }
            catch (IOException ioe)
            {
                Eventlog.Log.WriteEntry("SaSaLib", EventLogEntryType.Information, 9700, $"SaSaLib.StreamExtensions.StreamToFile(..)にて例外検知 {ioe.Message}");
            }
        }

        /// <summary>
        /// Stream の 指定したポジションから　終了ポジションまでを ファイルに書き込む
        /// </summary>
        /// <param name="st">読出し対象ストリーム</param>
        /// <param name="start">開始ポジション</param>
        /// <param name="end">終了ポジション</param>
        /// <param name="savepath">書き出しファイルパス</param>
        public static void StreamToFile(Stream st, long start, long end, string savepath)
        {
            using (Stream fs = new FileStream(savepath, FileMode.Create, FileAccess.Write))
            {
                long saveCount = end - start;
                byte[] dataBytes = new byte[saveCount];

                st.Position = start;
                st.Read(dataBytes, 0, (int)saveCount);
                fs.Write(dataBytes, 0, dataBytes.Length);
            }
        }

        /// <summary>
        /// ファイルからメモリーストリームへ読み込み
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns></returns>
        public static MemoryStream StreamFromFile(string path)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);
            return ms;
        }

        /// <summary>
        /// ファイルから準備したメモリーストリームへ読み込み
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ms"></param>
        public static void StreamFromFile(string path, MemoryStream ms)
        {
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);

        }

        /// <summary>
        /// Stream の 指定したポジションから　終了ポジションまでを別のストリームに書き込む
        /// </summary>
        /// <param name="source">読出し対象ストリーム</param>
        /// <param name="start">開始ポジション</param>
        /// <param name="end">終了ポジション</param>
        /// <param name="dist"></param>
        public static void CopyOtherStream(Stream source, long start, long end, Stream dist)
        {
            long saveSize = end - start;
            byte[] dataBytes = new byte[saveSize];
            source.Position = start;
            source.Read(dataBytes, 0, (int)saveSize);
            dist.Write(dataBytes, 0, dataBytes.Length);
        }
        /// <summary>
        /// Stream の 指定したポジションから　終了ポジションまでを別のストリームに書き込む(その２)
        /// </summary>
        /// <param name="st"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Stream FetchPartialFromStream(Stream st, long start, long end)
        {
            using (Stream distStream = new MemoryStream())
            {
                long saveSize = end - start;
                byte[] dataBytes = new byte[saveSize];
                st.Position = start;
                st.Read(dataBytes, 0, (int)saveSize);
                distStream.Write(dataBytes, 0, dataBytes.Length);
                return distStream;
            }
        }

        /// <summary>
        /// ストリームの指定位置から指定したバイト配列を探し、最初に始まった位置を返す
        /// </summary>
        /// <param name="source">検索元</param>
        /// <param name="checkBytes">検索するバイト配列</param>
        /// <param name="start">先頭からのオフセット</param>
        /// <returns></returns>
        public static long SearchStreamByteArrayOrg(MemoryStream source, byte[] checkBytes, long start)
        {
            Console.WriteLine("A source.Length={0}", source.Length);
            Console.WriteLine("A start={0}", start);

            long backupPosition = source.Position;

            using (MemoryStream distnation = new MemoryStream())
            {
                source.Position = start;
                //  ストリームを複製
                source.CopyTo(distnation);
                Console.WriteLine("A distnation.Length={0}", distnation.Length);
                // 読出しバッファの準備
                byte[] bufBytes = new byte[checkBytes.Length];

                for (int curPos = 0; curPos < distnation.Length; curPos++)
                {
                    distnation.Position = curPos;
                    // 複製したストリームから用意したバッファ分を読出し。ポジションは移動する
                    int read = distnation.Read(bufBytes, 0, bufBytes.Length);
                    // チェック対象と比較。同じならtrue
                    bool isEqual1 = ((IStructuralEquatable)checkBytes).Equals(bufBytes, StructuralComparisons.StructuralEqualityComparer);
                    if (isEqual1)
                    {
                        long ans = curPos + start;
                        source.Position = backupPosition;
                        Console.WriteLine("A ans={0}", ans);
                        return ans;
                    }
                }
                source.Position = backupPosition;
                return -1;
            }
        }
        /// <summary>
        /// ストリームの指定位置から指定したバイト配列を探し、最初に始まった位置を返す.Stream.Positionは保護されません
        /// </summary>
        /// <param name="source">検索元</param>
        /// <param name="checkBytes">検索するバイト配列</param>
        /// <param name="start">先頭からのオフセット</param>
        /// <returns></returns>
        public static long SearchStreamByteArray(MemoryStream source, byte[] checkBytes, long start)
        {

            // 読出しバッファを探す配列分準備する
            byte[] bufBytes = new byte[checkBytes.Length];

            // curPos に開始位置をセット、検索を開始curPosの最大値はsourceストリームのLength
            for (long curPos = start; curPos < source.Length; curPos++)
            {
                //ソースストリームのポジションを移動
                source.Position = curPos;

                // 複製したストリームから用意したバッファ分を読出し。ポジションは移動する
                int read = source.Read(bufBytes, 0, bufBytes.Length);
                // チェック対象と比較。同じならtrue
                bool isEqual1 = ((IStructuralEquatable)checkBytes).Equals(bufBytes, StructuralComparisons.StructuralEqualityComparer);
                if (isEqual1)
                {
                    long ans = curPos;
                    return ans;
                }
            }

            return -1;

        }

        /// <summary>
        /// ストリームの指定位置から指定したバイト配列を探し、最初に始まった位置を返す
        /// </summary>
        /// <param name="source"></param>
        /// <param name="checkBytes"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static long SearchStreamByteArrayTest(MemoryStream source, byte[] checkBytes, long start)
        {
            byte[] sourceBytes = StreamToBytes(source, start);
            return (SearchBytePattern(sourceBytes, checkBytes));
        }

        /// <summary>
        /// バイト配列からバイト配列を検索
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static long SearchBytePattern(byte[] bytes, byte[] pattern)
        {
            return -1;
        }

        /// <summary>
        /// char[] の配列を byte[]の配列へコピー
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] CopyCharsToBytes(char[] source)
        {
            byte[] ans = new byte[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                ans[i] = (byte)source[i];
            }

            return ans;
        }

        /* 未使用
        /// <summary>
        /// 指定されたByte配列が見つかるまで、ストリームを進める。
        /// 
        /// </summary>
        /// <param name="stream">The stream to search in</param>
        /// <param name="searchBytes">The byte sequence to search for</param>
        /// <returns></returns>
        public static int ScanUntilFound(this Stream stream, byte[] searchBytes)
        {
            // 検索バイト配列のサイズの新しいバイト配列を生成
            byte[] streamBuffer = new byte[searchBytes.Length];

            int nextRead = searchBytes.Length;

            int totalScannedBytes = -1;

            while (true)
            {
                // 検索バイト列のサイズ分ストリームから読み出す
                FillBuffer(stream, streamBuffer, nextRead);
                totalScannedBytes += nextRead; //this is only used for final reporting of where it was found in the stream

                //読み出したバイト配列と、検索バイト配列とを比較
                if (ArraysMatch(searchBytes, streamBuffer, 0))
                    // 一致するまで読み込んだバイト数を返す
                    return totalScannedBytes; //found it

                nextRead = FindPartialMatch(searchBytes, streamBuffer);
            }
        }

        /// <summary>
        /// Check all offsets, for partial match. 
        /// 部分一致の場合は、すべてのオフセットを確認します。
        /// </summary>
        /// <param name="searchBytes"></param>
        /// <param name="streamBufferBytes"></param>
        /// <returns>The amount of bytes which need to be read in, next round</returns>
        static int FindPartialMatch(byte[] searchBytes, byte[] streamBufferBytes)
        {
            // 1234 = 0 - found it. this special case is already catered directly in ScanUntilFound            
            // #123 = 1 - partially matched, only missing 1 value
            // ##12 = 2 - partially matched, only missing 2 values
            // ###1 = 3 - partially matched, only missing 3 values
            // #### = 4 - not matched at all

            for (int i = 1; i < searchBytes.Length; i++)
            {
                if (ArraysMatch(searchBytes, streamBufferBytes, i))
                {
                    // EG. Searching for 1234, have #123 in the streamBuffer, and [i] is 1
                    // Output: 123#, where # will be read using FillBuffer next. 
                    // 例えば。 1234を探し、streamBufferに＃123、[i]は1 Output：123＃です。ここで＃は次にFillBufferを使
                    // って読み込まれます。
                    Array.Copy(streamBufferBytes, i, streamBufferBytes, 0, searchBytes.Length - i);
                    return i; //if an offset of [i], makes a match then only [i] bytes need to be read from the stream to check if there's a match
                }
            }

            return 4;
        }

        /// <summary>
        /// Reads bytes from the stream, making sure the requested amount of bytes are read (streams don't always fulfill the full request first time)
        /// // ストリームからバイトを読み込み、要求されたバイト数が読み込まれていることを確認します（ストリームが常に完全な要求を最初に満たしているとは限りません）。
        /// /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="streamBufferBytes">The buffer to read into</param>
        /// <param name="bytesNeeded">How many bytes are needed. If less than the full size of the buffer, it fills the tail end of the streamBuffer</param>
        static void FillBuffer(Stream stream, byte[] streamBufferBytes, int bytesNeeded)
        {
            // EG1. [123#] - bytesNeeded is 1, when the streamBuffer contains first three matching values, but now we need to read in the next value at the end 
            //                                  streamBufferに最初の3つの一致する値が含まれている場合は、最後に次の値を読み込む必要があります
            // EG2. [####] - bytesNeeded is 4

            var bytesAlreadyRead = streamBufferBytes.Length - bytesNeeded; //invert
            while (bytesAlreadyRead < streamBufferBytes.Length)
            {
                bytesAlreadyRead += stream.Read(streamBufferBytes, bytesAlreadyRead, streamBufferBytes.Length - bytesAlreadyRead);
            }
        }

        /// <summary>
        /// Checks if arrays match exactly, or with offset. 
        /// 
        /// 配列が正確に一致するか、またはオフセットと一致するかをチェックします。
        /// </summary>
        /// <param name="searchBytes">Bytes to search for. Eg. [1234]</param>
        /// <param name="streamBufferBytes">Buffer to match in. Eg. [#123] </param>
        /// <param name="startAt">When this is zero, all bytes are checked. Eg. If this value 1, and it matches, this means the next byte in the stream to read may mean a match
        /// これがゼロの場合、すべてのバイトがチェックされます。例えば。この値が1で一致した場合、これはストリームの次のバイトが読み込み対象であることを意味します</param>
        /// <returns></returns>
        static bool ArraysMatch(byte[] searchBytes, byte[] streamBufferBytes, int startAt)
        {
            for (int i = 0; i < searchBytes.Length - startAt; i++)
            {
                if (searchBytes[i] != streamBufferBytes[i + startAt])
                    return false;
            }
            return true;
        }

        */
    }
}