//using System;
//using System.Globalization;
//using System.Linq;
//using System.Runtime.Versioning;
//using System.Text;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class Encryption
//    {

//        /// <summary>
//        /// SasaAuth3.x 系で使用するAES鍵
//        /// </summary>
//        private byte[] SasaAuth3_AESkey = { 0x6F, 0xDF, 0x98, 0x00, 0x67, 0x36, 0x7D, 0x3B, 0xFF, 0xC9, 0x3B, 0x79, 0x4D, 0xD4, 0x81, 0x72 };

//        internal string EncryptionType { get; private set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="EncryptionType"></param>
//        public Encryption(string EncryptionType)
//        {
//            this.EncryptionType = EncryptionType;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="PlaneText"></param>
//        /// <returns></returns>
//        public string Encoding(string PlaneText)
//        {
//            if (PlaneText == null)
//            {
//                return null;
//            }

//            string ans = null;

//            switch (EncryptionType)
//            {
//                case "SasaAuth1.1":
//                    ans = new string(PlaneText.Reverse().ToArray());
//                    break;

//                case "SasaAuth2.1":
//                    ans = convertLow2UpperAndUpper2Low(new string(PlaneText.Reverse().ToArray()));
//                    break;

//                case "SasaAuth3.1":

//                    string SasaAuth3_1_AESiv_Base64 = "b9+YAGc2fTv/yTt5TdSBcg==";

//                    byte[] AES_iv = Convert.FromBase64String(SasaAuth3_1_AESiv_Base64);

//                    string text =  System.Text.Encoding.ASCII.GetString(AES_iv);
//                    ans =  EncryptionAES.EncryptToBase64NoAsync(PlaneText, SasaAuth3_AESkey, AES_iv);
//                    break;

//                default:
//                    ans = "";
//                    break;
//            }
//            return ans;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="EncodedText"></param>
//        /// <returns></returns>
//        public string Decoding(string EncodedText)
//        {
//            string ans = null;

//            if (string.IsNullOrWhiteSpace(EncodedText) == false)
//            {
//                switch (EncryptionType)
//                {
//                    case "SasaAuth1.1":
//                        ans = new string(EncodedText.Reverse().ToArray());
//                        break;

//                    case "SasaAuth2.1":
//                        ans = convertLow2UpperAndUpper2Low(new string(EncodedText.Reverse().ToArray()));
//                        break;

//                    case "SasaAuth3.1":

//                        string SasaAuth3_1_AESiv_Base64 = "b9+YAGc2fTv/yTt5TdSBcg==";

//                        byte[] AES_iv = Convert.FromBase64String(SasaAuth3_1_AESiv_Base64);
//                        ans = EncryptionAES.DecryptFromBase64NoAsync(EncodedText, SasaAuth3_AESkey, AES_iv);
//                        break;

//                    default:
//                        ans = "";
//                        break;
//                }
//            }
//            return ans;
//        }

//        string convertLow2UpperAndUpper2Low(string input)
//        {

//            string moji = input;

//            // テキスト要素を列挙するオブジェクトを取得
//            TextElementEnumerator charEnum = StringInfo.GetTextElementEnumerator(moji);

//            // 1文字ずつ解析して、文字列を加工する
//            StringBuilder output = new StringBuilder();
//            while (true)
//            {
//                // 次の1文字を取得する
//                if (charEnum.MoveNext() == false)
//                {
//                    break; // 取得する文字がない
//                }
//                var aa = charEnum.Current.ToString();
//                if (char.IsLower(aa[0]))
//                    output.Append(charEnum.Current.ToString().ToUpper());
//                else
//                    output.Append(charEnum.Current.ToString().ToLower());

//            }
//            return output.ToString();
//        }


//    }


//}
