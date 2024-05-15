using System.Runtime.Versioning;
using System.Text;

namespace SasaLib
{
    [SupportedOSPlatform("windows")]

    /// <summary>
    /// BASE64に関するConvertクラスのラッパー
    /// </summary>
    public class Base64
    {
        private Encoding enc;
        /// <summary>
        /// ■コンストラクタ
        /// </summary>
        /// <param name="encStr">エンコーディング指定</param>
        public Base64(string encStr = "UTF-8")
        {
            enc = Encoding.GetEncoding(encStr);
        }
        /// <summary>
        /// ■文字列をBASE64エンコード
        /// </summary>
        /// <param name="str">エンコードする文字列</param>
        /// <returns>エンコードされた文字列</returns>
        public string Encode(string str)
        {
            return Convert.ToBase64String(enc.GetBytes(str));
        }
        /// <summary>
        /// ■BASE64文字列をでコード
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Decode(string str)
        {
            return enc.GetString(Convert.FromBase64String(str));
        }
    }
}

