using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib
{
    /// <summary>
    /// GUID
    /// </summary>
    public class GUIDExtensions
    {
        /// <summary>
        /// GUIDオブジェクト
        /// </summary>
        public Guid GuidObj { get { return _guidobj; } }
        private Guid _guidobj;

        /// <summary>
        /// GUIDバイト列をBase32エンコードしたもの
        /// </summary>
        public string B32String { get { return _B32String; } }
        string _B32String;
        /// <summary>
        /// GUIDバイト列をBase64エンコードしたもの
        /// </summary>
        public string B64String { get { return _B64String; } }
        string _B64String;

        /// <summary>
        /// GUIDバイト列をBase64エンコードし.Replace("$", "/").Replace("#", "+")に変換したもの
        /// "チケットコード"として使います
        /// </summary>
        public string B64FnameString { get { return GetB64FnameStringFromB64String(_B64String); } }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sw">true:GUIDを生成,falseGUIDを生成しない</param>
        public GUIDExtensions(bool sw)
        {
            if (sw) SetNewGUID();
        }
        public GUIDExtensions()
        {
            // なにもしない
        }
        public GUIDExtensions(string s)
        {
            _guidobj = new Guid(s);
            SetGUIDstring();
        }

        /// <summary>
        /// Guid値を書き換える
        /// </summary>
        public void SetNewGUID()
        {
            _guidobj = System.Guid.NewGuid();
            SetGUIDstring();
        }

        void SetGUIDstring()
        {
            // GUIDバイト列をBase32エンコードしたもの
            _B32String = Base32.Encode(GuidObj.ToByteArray());
            // GUIDバイト列をBase64エンコードしたもの。
            // [a～z、A～Z、0～9、+、/、=]を使った24桁で構成される。=はパディング。
            //GUIDの場合は22桁で表現可能なため後部2桁が =となる。
            _B64String = Convert.ToBase64String(GuidObj.ToByteArray());
        }

        /// <summary>
        /// GUID-Base64 string をGuidオブジェクトにデコードする
        /// </summary>
        /// <param name="guidB64str"></param>
        /// <returns></returns>
        public static Guid DecodeB64GuidString(string guidB64str)
        {
            Guid guid = new Guid(Convert.FromBase64String(guidB64str));
            return guid;
        }

        /// <summary>
        /// GUID-Base32 stringをGuidオブジェクトにデコードする
        /// </summary>
        /// <param name="guidB32str"></param>
        /// <returns>Guidオブジェクト</returns>
        public static Guid DecodeB32GuidString(string guidB32str)
        {
            Guid guid = new Guid(Base32.Decode(guidB32str));
            return guid;
        }

        /// <summary>
        /// GUID-Base64 Filename string をGuidオブジェクトにデコードする
        /// </summary>
        /// <param name="B64FnameString"></param>
        /// <returns>Guidオブジェクト</returns>
        static public Guid DecodeB64FnameString(string B64FnameString)
        {
            return DecodeB64GuidString(GetB64StringFromB64FnameString(B64FnameString));
        }

        /// <summary>
        /// GUID文字列からGuidオブジェクトを得る
        /// </summary>
        /// <param name="guidstr"></param>
        /// <returns></returns>
        public static Guid GetGuidObjFromGuidString(string guidstr)
        {
            Guid guid = new Guid(guidstr);
            return guid;
        }

        /// <summary>
        /// 改変Base64文字列を 正規のBase64に戻す (TICKETCODE to GUIDBASE64)
        /// B64FnameString最大22桁でできており、#,$,と0-9,a-z,A-Zの文字で構成されている
        /// </summary>
        /// <param name="B64FnameString"></param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        static public string GetB64StringFromB64FnameString(string B64FnameString)
        {
            if (B64FnameString != null)
            {
                if (B64FnameString.Count() == 22)
                {
                    if (B64FnameString != null && B64FnameString != "")
                        return B64FnameString.Substring(0, 22).Replace("$", "/").Replace("#", "+") + "==";
                    else
                        return null;
                }
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// B64StringをUS/JPバーコードリーダーでも変換なしで使用できる形にする(GUIDBASE64 to TICKETCODE)
        /// </summary>
        /// <param name="GUIDBASE64"></param>
        /// <returns></returns>
        static public string GetB64FnameStringFromB64String(string GUIDBASE64)
        {
            if (GUIDBASE64.Count() == 24)
            {
                if (GUIDBASE64 != null)
                    return GUIDBASE64.Substring(0, 22).Replace("+", "#").Replace("/", "$");
                else
                    return null;
            }
            else
                return null;
        }

        /// <summary>
        /// GUID番号を返す
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GuidObj.ToString(); ;
        }
    }
}
