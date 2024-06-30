//using System.Text;

//namespace SasaLib
//{
//    public static partial class TextFile
//    {
//        /// <summary>
//        /// BOMを調べて、文字コードを判別する。
//        /// </summary>
//        /// <param name="bytes">文字コードを調べるデータ。</param>
//        /// <returns>BOMが見つかった時は、対応するEncodingオブジェクト。
//        /// 見つからなかった時は、Encoding.Default。</returns>
//        [System.Diagnostics.DebuggerStepThrough]
//        public static System.Text.Encoding DetectEncodingFromBOM(byte[] bytes)
//        {
//            if (bytes.Length < 2)
//            {
//                return null;
//            }
//            if ((bytes[0] == 0xfe) && (bytes[1] == 0xff))
//            {
//                //UTF-16 BE
//                return new System.Text.UnicodeEncoding(true, true);
//            }
//            if ((bytes[0] == 0xff) && (bytes[1] == 0xfe))
//            {
//                if ((4 <= bytes.Length) &&
//                    (bytes[2] == 0x00) && (bytes[3] == 0x00))
//                {
//                    //UTF-32 LE
//                    return new System.Text.UTF32Encoding(false, true);
//                }
//                //UTF-16 LE
//                return new System.Text.UnicodeEncoding(false, true);
//            }
//            if (bytes.Length < 3)
//            {
//                return null;
//            }
//            if ((bytes[0] == 0xef) && (bytes[1] == 0xbb) && (bytes[2] == 0xbf))
//            {
//                //UTF-8
//                return new System.Text.UTF8Encoding(true, true);
//            }
//            if (bytes.Length < 4)
//            {
//                return null;
//            }
//            if ((bytes[0] == 0x00) && (bytes[1] == 0x00) &&
//                (bytes[2] == 0xfe) && (bytes[3] == 0xff))
//            {
//                //UTF-32 BE
//                return new System.Text.UTF32Encoding(true, true);
//            }

//            return Encoding.Default;
//        }

//        /// <summary>
//        /// BOMを調べて、文字コードを判別する。
//        /// </summary>
//        /// <param name="FilePath">判別するテキストファイル名</param>
//        /// <returns>BOMが見つかった時は、対応するEncodingオブジェクト。見つからなかった時は、null。</returns>
//        [System.Diagnostics.DebuggerStepThrough]
//        public static System.Text.Encoding DetectEncodingFromBOM(string FilePath)
//        {
//            //テキストファイルを開く
//            byte[] bs = System.IO.File.ReadAllBytes(FilePath);

//            //文字コードを判別する
//            System.Text.Encoding enc = DetectEncodingFromBOM(bs);

//            return enc;
//        }
//    }


//}
