using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Copylight ささはそふとうぇあ C#標準ツールクラス
/// </summary>
namespace SasaLib
{
    /// <summary>
    /// 文字列操作メソッド
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 全角英数記号と半角英数記号の対応辞書
        /// 全角ｽﾍﾟｰｽと半角スペースを半角アンダーバーへ
        /// 2022-02-14 obsolete
        /// </summary>
        static Dictionary<char, char> _dictZENKAKUtoHANKAKU = new Dictionary<char, char>()
        {
            {'０','0'},{'１','1'},{'２','2'},{'３','3'},
            {'４','4'},{'５','5'},{'６','6'},{'７','7'},
            {'８','8'},{'９','9'},
            {'Ａ','A'},{'Ｂ','B'},{'Ｃ','C'},{'Ｄ','D'},{'Ｅ','E'},
            {'Ｆ','F'},{'Ｇ','G'},{'Ｈ','H'},{'Ｉ','I'},{'Ｊ','J'},
            {'Ｋ','K'},{'Ｌ','L'},{'Ｍ','M'},{'Ｎ','N'},{'Ｏ','O'},
            {'Ｐ','P'},{'Ｑ','Q'},{'Ｒ','R'},{'Ｓ','S'},{'Ｔ','T'},
            {'Ｕ','U'},{'Ｖ','V'},{'Ｗ','W'},{'Ｘ','X'},{'Ｙ','Y'},
            {'Ｚ','Z'},
            {'ａ','a'},{'ｂ','b'},{'ｃ','c'},{'ｄ','d'},{'ｅ','e'},
            {'ｆ','f'},{'ｇ','g'},{'ｈ','h'},{'ｉ','i'},{'ｊ','j'},
            {'ｋ','k'},{'ｌ','l'},{'ｍ','m'},{'ｎ','n'},{'ｏ','o'},
            {'ｐ','p'},{'ｑ','q'},{'ｒ','r'},{'ｓ','s'},{'ｔ','t'},
            {'ｕ','u'},{'ｖ','v'},{'ｗ','w'},{'ｘ','x'},{'ｙ','y'},
            {'ｚ','z'},
            {'！','!'},{'”','"'},{'＃','#'},{'＄','$'},{'％','%'},
            {'＆','&'},{'’','\''},{'（','('},{'）',')'},{'―','-'},{'ー','-'},{'‐','-'},{'－','-'},
            {'＝','='},
            {'～','~'},{'｜','|'},{'＠','@'},{'‘','`'},{'［','['},
            {'｛','{'},{'；',';'},{'＋','+'},{'：',':'},{'＊','*'},
            {'］',']'},{'｝','}'},{'，',','},{'＜','<'},{'＞','>'},{'．','.'},
            {'￥','\\'},{'＿','_'},
            {'×','x'},
            {' ','_'},{'　','_'},  //全角ｽﾍﾟｰｽと半角スペースを半角アンダーバーへ。
            {'・' ,'-'}
          };

        /// <summary>
        /// 全角英数記号と半角英数記号の対応辞書
        /// 全角ｽﾍﾟｰｽと半角スペースを半角アンダーバーへ
        /// 2022-02-14 全角長音を全角の中央点に変換していたのをとりやめた
        /// </summary>
        static Dictionary<char, char> _dictZENKAKUtoHANKAKU_KANA_CHOUON_HENKANSEZU = new Dictionary<char, char>()
        {
            {'０','0'},{'１','1'},{'２','2'},{'３','3'},
            {'４','4'},{'５','5'},{'６','6'},{'７','7'},
            {'８','8'},{'９','9'},
            {'Ａ','A'},{'Ｂ','B'},{'Ｃ','C'},{'Ｄ','D'},{'Ｅ','E'},
            {'Ｆ','F'},{'Ｇ','G'},{'Ｈ','H'},{'Ｉ','I'},{'Ｊ','J'},
            {'Ｋ','K'},{'Ｌ','L'},{'Ｍ','M'},{'Ｎ','N'},{'Ｏ','O'},
            {'Ｐ','P'},{'Ｑ','Q'},{'Ｒ','R'},{'Ｓ','S'},{'Ｔ','T'},
            {'Ｕ','U'},{'Ｖ','V'},{'Ｗ','W'},{'Ｘ','X'},{'Ｙ','Y'},
            {'Ｚ','Z'},
            {'ａ','a'},{'ｂ','b'},{'ｃ','c'},{'ｄ','d'},{'ｅ','e'},
            {'ｆ','f'},{'ｇ','g'},{'ｈ','h'},{'ｉ','i'},{'ｊ','j'},
            {'ｋ','k'},{'ｌ','l'},{'ｍ','m'},{'ｎ','n'},{'ｏ','o'},
            {'ｐ','p'},{'ｑ','q'},{'ｒ','r'},{'ｓ','s'},{'ｔ','t'},
            {'ｕ','u'},{'ｖ','v'},{'ｗ','w'},{'ｘ','x'},{'ｙ','y'},
            {'ｚ','z'},
            {'！','!'},{'”','"'},{'＃','#'},{'＄','$'},{'％','%'},
            {'＆','&'},{'’','\''},{'（','('},{'）',')'},{'―','-'},   {'‐','-'},{'－','-'},//全角のカナ長音は無視
            {'＝','='},
            {'～','~'},{'｜','|'},{'＠','@'},{'‘','`'},{'［','['},
            {'｛','{'},{'；',';'},{'＋','+'},{'：',':'},{'＊','*'},
            {'］',']'},{'｝','}'},{'，',','},{'＜','<'},{'＞','>'},{'．','.'}, 
            {'￥','\\'},{'＿','_'},
            {'×','x'},
            {' ','_'},{'　','_'}  //全角ｽﾍﾟｰｽと半角スペースを半角アンダーバーへ。           
          };

        /// <summary>
        /// 全角英数記号と半角英数記号の対応辞書
        /// //全角ｽﾍﾟｰｽは半角ｽﾍﾟｰｽへ。
        /// 2022-01-07 変換表追加
        /// 2022-02-14 obsolete
        /// </summary>
        static Dictionary<char, char> _dictZENKAKUtoHANKAKU_ZENKAKUSPACEtoHANKAKUSPACE = new Dictionary<char, char>()
        {
            {'０','0'},{'１','1'},{'２','2'},{'３','3'},
            {'４','4'},{'５','5'},{'６','6'},{'７','7'},
            {'８','8'},{'９','9'},
            {'Ａ','A'},{'Ｂ','B'},{'Ｃ','C'},{'Ｄ','D'},{'Ｅ','E'},
            {'Ｆ','F'},{'Ｇ','G'},{'Ｈ','H'},{'Ｉ','I'},{'Ｊ','J'},
            {'Ｋ','K'},{'Ｌ','L'},{'Ｍ','M'},{'Ｎ','N'},{'Ｏ','O'},
            {'Ｐ','P'},{'Ｑ','Q'},{'Ｒ','R'},{'Ｓ','S'},{'Ｔ','T'},
            {'Ｕ','U'},{'Ｖ','V'},{'Ｗ','W'},{'Ｘ','X'},{'Ｙ','Y'},
            {'Ｚ','Z'},
            {'ａ','a'},{'ｂ','b'},{'ｃ','c'},{'ｄ','d'},{'ｅ','e'},
            {'ｆ','f'},{'ｇ','g'},{'ｈ','h'},{'ｉ','i'},{'ｊ','j'},
            {'ｋ','k'},{'ｌ','l'},{'ｍ','m'},{'ｎ','n'},{'ｏ','o'},
            {'ｐ','p'},{'ｑ','q'},{'ｒ','r'},{'ｓ','s'},{'ｔ','t'},
            {'ｕ','u'},{'ｖ','v'},{'ｗ','w'},{'ｘ','x'},{'ｙ','y'},
            {'ｚ','z'},
            {'！','!'},{'”','"'},{'＃','#'},{'＄','$'},{'％','%'},
            {'＆','&'},{'’','\''},{'（','('},{'）',')'},{'―','-'},{'ー','-'},{'‐','-'},{'－','-'},
            {'＝','='},
            {'～','~'},{'｜','|'},{'＠','@'},{'‘','`'},{'［','['},
            {'｛','{'},{'；',';'},{'＋','+'},{'：',':'},{'＊','*'},
            {'］',']'},{'｝','}'},{'，',','},{'＜','<'},{'＞','>'},{'．','.'},
            {'￥','\\'},{'＿','_'},
            {'×','x'},
            {'　',' '}, //全角ｽﾍﾟｰｽは半角ｽﾍﾟｰｽへ。
            {'・' ,'-'}
          };

        /// <summary>
        /// 全角英数記号と半角英数記号の対応辞書
        /// //全角ｽﾍﾟｰｽは半角ｽﾍﾟｰｽへ。
        /// 2022-02-14 全角長音を全角の中央点を除外した
        /// </summary>
        static Dictionary<char, char> _dictZENKAKUtoHANKAKU_ZENKAKUSPACEtoHANKAKUSPACE_KANA_CHOUON_HENKANSEZU = new Dictionary<char, char>()
        {
            {'０','0'},{'１','1'},{'２','2'},{'３','3'},
            {'４','4'},{'５','5'},{'６','6'},{'７','7'},
            {'８','8'},{'９','9'},
            {'Ａ','A'},{'Ｂ','B'},{'Ｃ','C'},{'Ｄ','D'},{'Ｅ','E'},
            {'Ｆ','F'},{'Ｇ','G'},{'Ｈ','H'},{'Ｉ','I'},{'Ｊ','J'},
            {'Ｋ','K'},{'Ｌ','L'},{'Ｍ','M'},{'Ｎ','N'},{'Ｏ','O'},
            {'Ｐ','P'},{'Ｑ','Q'},{'Ｒ','R'},{'Ｓ','S'},{'Ｔ','T'},
            {'Ｕ','U'},{'Ｖ','V'},{'Ｗ','W'},{'Ｘ','X'},{'Ｙ','Y'},
            {'Ｚ','Z'},
            {'ａ','a'},{'ｂ','b'},{'ｃ','c'},{'ｄ','d'},{'ｅ','e'},
            {'ｆ','f'},{'ｇ','g'},{'ｈ','h'},{'ｉ','i'},{'ｊ','j'},
            {'ｋ','k'},{'ｌ','l'},{'ｍ','m'},{'ｎ','n'},{'ｏ','o'},
            {'ｐ','p'},{'ｑ','q'},{'ｒ','r'},{'ｓ','s'},{'ｔ','t'},
            {'ｕ','u'},{'ｖ','v'},{'ｗ','w'},{'ｘ','x'},{'ｙ','y'},
            {'ｚ','z'},
            {'！','!'},{'”','"'},{'＃','#'},{'＄','$'},{'％','%'},
            {'＆','&'},{'’','\''},{'（','('},{'）',')'},{'―','-'},   {'‐','-'},{'－','-'},//全角のカナ長音は無視
            {'＝','='},
            {'～','~'},{'｜','|'},{'＠','@'},{'‘','`'},{'［','['},
            {'｛','{'},{'；',';'},{'＋','+'},{'：',':'},{'＊','*'},
            {'］',']'},{'｝','}'},{'，',','},{'＜','<'},{'＞','>'},{'．','.'},
            {'￥','\\'},{'＿','_'},
            {'×','x'},
            {'　',' '} //全角ｽﾍﾟｰｽは半角ｽﾍﾟｰｽへ。           
          };

        /// <summary>
        /// 半角英数記号と全角英数記号の対応辞書
        /// </summary>
        static Dictionary<char, char> _dictHANKAKUtoZENKAKU = new Dictionary<char, char>()
        {
            {'0','０'},{'1','１'},{'2','２'},{'3','３'},
            {'4','４'},{'5','５'},{'6','６'},{'7','７'},
            {'8','８'},{'9','９'},
            {'A','Ａ'},{'B','Ｂ'},{'C','Ｃ'},{'D','Ｄ'},{'E','Ｅ'},
            {'F','Ｆ'},{'G','Ｇ'},{'H','Ｈ'},{'I','Ｉ'},{'J','Ｊ'},
            {'K','Ｋ'},{'L','Ｌ'},{'M','Ｍ'},{'N','Ｎ'},{'O','Ｏ'},
            {'P','Ｐ'},{'Q','Ｑ'},{'R','Ｒ'},{'S','Ｓ'},{'T','Ｔ'},
            {'U','Ｕ'},{'V','Ｖ'},{'W','Ｗ'},{'X','Ｘ'},{'Y','Ｙ'},
            {'Z','Ｚ'},
            {'a','ａ'},{'b','ｂ'},{'c','ｃ'},{'d','ｄ'},{'e','ｅ'},
            {'f','ｆ'},{'g','ｇ'},{'h','ｈ'},{'i','ｉ'},{'j','ｊ'},
            {'k','ｋ'},{'l','ｌ'},{'m','ｍ'},{'n','ｎ'},{'o','ｏ'},
            {'p','ｐ'},{'q','ｑ'},{'r','ｒ'},{'s','ｓ'},{'t','ｔ'},
            {'u','ｕ'},{'v','ｖ'},{'w','ｗ'},{'x','ｘ'},{'y','ｙ'},
            {'z','ｚ'},
            {'!','！'},{'"','”'},{'#','＃'},{'$','＄'},{'%','％'},
            {'&','＆'},{'\'','’'},{'(','（'},{')','）'},{'-','－'},
            {'=','＝'},
            {'~','～'},{'|','｜'},{'@','＠'},{'`','‘'},{'[','［'},
            {'{','｛'},{';','；'},{'+','＋'},{':','：'},{'*','＊'},
            {']','］'},{'}','｝'},{',','，'},{'<','＜'},{'>','＞'},{'.','．'},
            {'\\','￥'},{'_','＿'}
          };

        /// <summary>
        /// String文字列のバイト数を返す
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int GetStringByteCount(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return 0; // Keyの文字列が IsNullOrWhiteSpace なら 0

            System.Text.Encoding sjis = System.Text.Encoding.GetEncoding("shift_jis");
            int byteSize = sjis.GetByteCount(input);
            return byteSize;
        }

        /// <summary>
        /// String文字列の先頭からからバイト単位で指定したサイズまでの文字列を返す
        /// </summary>
        /// <param name="orgValue"></param>
        /// <param name="sizeOfByte"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static string StringReSizeByteCount(string orgValue, int sizeOfByte)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(orgValue)) return orgValue; // Keyの文字列が IsNullOrWhiteSpace なら 何もしない

                System.Text.Encoding sjis = System.Text.Encoding.GetEncoding("shift_jis");
                int orginalSize = sjis.GetByteCount(orgValue);
                if (sizeOfByte < orginalSize)
                {
                    byte[] b = sjis.GetBytes(orgValue);
                    string result = sjis.GetString(b, 0, sizeOfByte);
                    return result;
                }
                else
                {
                    return orgValue;
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("CommonTicket", $"CommonTicket.ReplaceOrCretekeyValue({orgValue}{sizeOfByte}) のエラー{ex.Message}");

                return orgValue;
            }
        }


        /// <summary>
        /// 文字列に全角ひらがな、全角カタカナ、漢字が入っていたらtrueを返す
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool CheckKanji(this string s)
        {
            bool a1 = Regex.IsMatch(s, @"\p{IsHiragana}");
            bool a2 = Regex.IsMatch(s, @"\p{IsKatakana}");
            bool a3 = Regex.IsMatch(s, @"\p{IsCJKUnifiedIdeographs}");
            return (a1 | a2 | a3);
        }


        /// <summary>
        /// 文字列に半角カナ・半角カナ記号が混入しているならTrue
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>

        public static bool CheckHalfKatakana(string target)
        {
            //文字列が半角カタカナ（句読点～半濁点）かどうかを判定します
            bool a = new Regex("[\uFF61-\uFF9F]").IsMatch(target);

            //文字列が半角カタカナ（「ｦ」～半濁点）の場合はtrue、それ以外はfalse
            bool b = new Regex("[\uFF66-\uFF9F]").IsMatch(target);

            return (a | b);
        }

        // 半角へ 変換
        /// <summary>
        /// 文字列を半角に変換する(LCMAP_HALFWIDTHにしたがう) (拡張メソッド)
        /// "ウaｂ１Ａ Ｂ　あＣエ"->"ｳab1A B あCｴ"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Zen2Han(this string str)
        {
            return SaSaLCMapString(str, NativeMethods.DwMap.LCMAP_HALFWIDTH);
        }

        /// <summary>
        /// 文字列で半角に置き換え可能な文字を置き換えて返す.スペースはアンダーバーになる（拡張メソッド）
        /// "ウＡ Ｂ　あいうＣエオ" -> "ウA_B_あいうCエオ" 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Zen2HanANK(this string source)
        {

            //Console.WriteLine(source);
            //Console.WriteLine(ZenNum2HanNum(source));

            // Regex を使わなくても以下のようにして可能
            string replaced = new String(
              source.Select(
                //n => (_dictZENKAKUtoHANKAKU.ContainsKey(n) ? _dictZENKAKUtoHANKAKU[n] : n)
                n => (_dictZENKAKUtoHANKAKU_KANA_CHOUON_HENKANSEZU.ContainsKey(n) ? _dictZENKAKUtoHANKAKU_KANA_CHOUON_HENKANSEZU[n] : n)
                ).ToArray()
              );
            //Console.WriteLine(replaced);
            return replaced;
            /* 結果は：
            01２34５6７８９Ａb１Ｃ０2ｄＥfあい ３　６
            0123456789Ａb1Ｃ02ｄＥfあい_3_6
            */
        }


        public static string Zen2HanANK_ZenSpace2HanSpace(this string source)
        {
            string replaced = new String(
              source.Select(
//                n => (_dictZENKAKUtoHANKAKU_ZENKAKUSPACEtoHANKAKUSPACE.ContainsKey(n) ? _dictZENKAKUtoHANKAKU_ZENKAKUSPACEtoHANKAKUSPACE[n] : n)
                n => (_dictZENKAKUtoHANKAKU_ZENKAKUSPACEtoHANKAKUSPACE_KANA_CHOUON_HENKANSEZU.ContainsKey(n) ? _dictZENKAKUtoHANKAKU_ZENKAKUSPACEtoHANKAKUSPACE_KANA_CHOUON_HENKANSEZU[n] : n)
                ).ToArray()
              );
            return replaced;
            /* 結果は：
            01２34５6７８９Ａb１Ｃ０2ｄＥfあい ３　６
            0123456789Ａb1Ｃ02ｄＥfあい 3 6
            */
        }

        /// <summary>
        /// 全角を半角に変換する(カナ文字は、半角カタカナにする)(拡張メソッド) 
        /// "アイウ１Ａ Ｂ　あいうＣエオ"　-> "ｱｲｳ1A B ｱｲｳCｴｵ"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Zen2HanAndKANA(this string str)
        {
            return SaSaLCMapString(str, NativeMethods.DwMap.LCMAP_HALFWIDTH | NativeMethods.DwMap.LCMAP_KATAKANA);
        }

        /// <summary>
        /// 全角を半角に変換する。小文字は大文字にする。カナ文字は、半角カタカナにする(拡張メソッド) 
        /// "アイウaｂ１Ａ Ｂ　あいうＣエオ" -> "ｱｲｳAB1A B ｱｲｳCｴｵ"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Zen2HanUpperAndKANA(this string str)
        {
            return SaSaLCMapString(str, NativeMethods.DwMap.LCMAP_HALFWIDTH | NativeMethods.DwMap.LCMAP_UPPERCASE | NativeMethods.DwMap.LCMAP_KATAKANA);
        }

        /// <summary>
        /// 文字列の数値のみを半角に変換して返す(拡張メソッド)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ZenNum2HanNum(this string source)
        {
            Regex regex = new Regex("[０-９]+");

            String Replacer(Match m)
            {
                return new String(
                  //m.Value.Select(n => _dictZENKAKUtoHANKAKU[n]).ToArray());
                m.Value.Select(n => _dictZENKAKUtoHANKAKU_KANA_CHOUON_HENKANSEZU[n]).ToArray());
        }

            return regex.Replace(source, Replacer);
        }

        // 先頭末尾削除
        /// <summary>
        /// 文字列先頭と末尾の'\f,\r,\aを削除(拡張メソッド) 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimSymbolTopAndEnd(this string str)
        {
            if (str != null)
            {
                string ans;
                ans = str.Trim('\u0001', '\f', '\r', '\a');
                return ans;

            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 文字列の数字部分をゼロ補完 する静的メソッド  (文字列,桁数)<
        /// </summary>
        /// <param name="str">文字列</param>
        /// <param name="digit">桁数</param>
        /// <returns>ゼロ補完後の文字列</returns>
        public static string GetStringZeroPadding(string str, int digit)
        {
            //Regex re1 = new Regex(@"[^0-9]");// 0～9の文字に一致しないもの
            //Regex re2 = new Regex(@"[^a-zA-Z]");// a～z、A～Zの文字に一致しないもの
            //string str2 = re1.Replace(str, "");
            //string str3 = re2.Replace(str, "");
            Regex defaultRegex1 = new Regex(@"[^0-9]");// 0～9の文字に一致しないもの
            Regex defaultRegex2 = new Regex(@"[^a-zA-Z]");// a～z、A～Zの文字に一致しないもの
            string lettersOnly = defaultRegex1.Replace(str, "");
            string numbersOnly = defaultRegex2.Replace(str, "");
            string strAns = (lettersOnly.PadLeft(digit, '0') + numbersOnly);
            return strAns;
        }

        /// <summary>
        /// 文字列をハイフンにて分割、３項目め文字列にゼロ補完施し返す。
        /// 半角文字列であること
        /// </summary>
        /// <param name="str">"-"を単独で渡すと例外</param>
        /// <returns></returns>
        public static string GetZeroPaddingPartNumber(string str)
        {
            if (str == null)
            {
                return "";
            }

            string regex = @"^[0-9]{1,}-[0-9]{5}-";
            if (Regex.IsMatch(str, regex))
            {
                Console.WriteLine("ヒット{0}", regex);
                return str;
            }

            string zeroPaddingPartNumber;

            // 文字列を'-'で分解
            string[] splitWord = str.Split('-');

            // いくつの単語に分かれたか調査
            switch (splitWord.Length)
            {
                case 0:
                    //
                    Console.WriteLine("GetZeroPaddingPARTNUMBER(string)配列内に要素なし={0}\n", str);
                    zeroPaddingPartNumber = str;
                    return zeroPaddingPartNumber;
                case 1:
                    //Console.WriteLine("GetZeroPaddingPARTNUMBER(string)配列要素1コのみ={0}\n", str);
                    zeroPaddingPartNumber = String.Join("-", splitWord);
                    return zeroPaddingPartNumber;
                case 2:
                    // M-10201,M-123446,
                    // M-11123456R 等、ハイフン一つで構成されている文字列の処理
                    //Console.WriteLine("GetZeroPaddingPARTNUMBER(string)配列要素2コ={0}\n", str);

                    // 1つ目のハイフンの次のトークンの最後が英数字の場合、-000を付加。
                    // M-10201  -> M-10201-000
                    // M-ABCDEFG3   ->  M-ABCDEFG3-000
                    if (char.IsNumber(splitWord[1], splitWord[1].Length - 1))
                    {
                        zeroPaddingPartNumber = str + "-000";
                        return zeroPaddingPartNumber;
                    }
                    // 1つ目のハイフンの次のトークンの最後がRLの場合、-000RLを付加
                    // M-10201RL    ->  M-10201-000RL
                    else if (Regex.IsMatch(str, @"[Rr][Ll]$"))
                    {
                        Console.WriteLine("GetZeroPaddingPARTNUMBER(string)1つめのハイフンの次のトークンの最後がRLかrlかRlかrLです={0}\n", str);
                        zeroPaddingPartNumber = Regex.Replace(str, @"[Rr][Ll]$", "-000RL");
                        return zeroPaddingPartNumber;
                    }
                    // 1つ目のハイフンの次のトークンの最後がRの場合、-000Rを付加
                    // M-10201R M-10201-000R
                    else if (Regex.IsMatch(str, @"[Rr]$"))
                    {
                        Console.WriteLine("GetZeroPaddingPARTNUMBER(string)1つめのハイフンの次のトークンの最後がRかrです={0}\n", str);
                        zeroPaddingPartNumber = Regex.Replace(str, @"[Rr]$", "-000R");
                        return zeroPaddingPartNumber;

                    }
                    // 1つ目のハイフンの次のトークンの最後がLの場合、-000Lを付加
                    // M-10201L M-10201-000L
                    else if (Regex.IsMatch(str, @"[Ll]$"))
                    {
                        Console.WriteLine("GetZeroPaddingPARTNUMBER(string)1つめのハイフンの次のトークンの最後がLかlです={0}\n", str);
                        zeroPaddingPartNumber = Regex.Replace(str, @"[Ll]$", "-000L");
                        return zeroPaddingPartNumber;
                    }
                    // 該当なしの場合、分割した文字列を再結合
                    else
                    {
                        //
                        Console.WriteLine("GetZeroPaddingPARTNUMBER(string)該当なし。そのまま再結合します={0}\n", str);
                        zeroPaddingPartNumber = String.Join("-", splitWord);
                        return zeroPaddingPartNumber;

                    }

                case 3:
                    // 文字列中のハイフンの数が2個以上はここで処理
                    // M-10201-R    ->  M-10201-000R
                    // M-10201-RL    ->  M-10201-000RL
                    // M-10201-12RL    ->  M-10201-012RL
                    // M-10201-1    ->  M-10201-001
                    // M-10201-3R    ->  M-10201-003RL
                    //Console.WriteLine("GetZeroPaddingPARTNUMBER(string)配列要素３こ以上={0}\n", str);
                    splitWord[2] = StringUtil.GetStringZeroPadding(splitWord[2], 3);
                    zeroPaddingPartNumber = String.Join("-", splitWord);
                    return zeroPaddingPartNumber;
                default:
                    return str;
            }
        }

        /// <summary>
        /// 文字列を全角を半角に変換、制御記号をトリム、半角や全角の空白をリプレイスする (拡張メソッド)
        /// "ウaｂ１Ａ Ｂ　あＣエ"->"ｳab1ABあCｴ"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SimplificationString(this string str)
        {
            string ans;
            ans = StringUtil.Zen2Han(str).Trim('\f', ' ', '\r', '\a', '.', '-').Replace(" ", "").Replace("　", "");
            return ans;
        }

        /// <summary>
        /// 正規表現を指定しマッチしたら　trueを返す
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool MatchStringRegex(string input, string pattern)
        {
            if (pattern != "")
            {

                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
                Match m = r.Match(input);

                if (m.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 正規表現を2パターン指定し両方ともマッチしたら　trueを返す
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern1"></param>
        /// <param name="pattern2"></param>
        /// <returns></returns>
        public static bool MatchStringRegex(string input, string pattern1, string pattern2)
        {
            if (pattern1 != "")
            {
                Regex _regx = new Regex(pattern1, RegexOptions.IgnoreCase);

                Match _m1 = _regx.Match(input);

                if (_m1.Success)
                {
                    Console.WriteLine("マッチした input:{0}, p1:{1}", input, pattern1);

                    Regex _regex = new Regex(pattern2, RegexOptions.IgnoreCase);

                    Match m2 = _regex.Match(input);

                    if (m2.Success)
                    {
                        Console.WriteLine("マッチした input:{0}, p1:{1},p2:{2}", input, pattern1, pattern2);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;

                }
            }
            return false;
        }

        /// <summary>
        /// 入力文字列と、比較用正規表現を配列で受取り、どれか一つでもヒットすればtrueを返す
        /// </summary>
        /// <param name="input"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public static bool MatchStringRegex(string input, string[] patterns)
        {
            foreach (string pt in patterns)
            {
                if (pt != "")
                {
                    Regex _regex = new Regex(pt, RegexOptions.IgnoreCase);
                    Match _match = _regex.Match(input);

                    if (_match.Success)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 入力文字列と、比較用正規表現を配列で受取り、すべてがヒットすればtrueを返す
        /// </summary>
        /// <param name="input"></param>
        /// <param name="patterns"></param>
        /// <returns></returns>
        public static bool MatchStringRegex2(string input, string[] patterns)
        {
            foreach (string pt in patterns)
            {
                if (pt != "")
                {
                    Regex _regex = new Regex(pt, RegexOptions.IgnoreCase);
                    Match _match = _regex.Match(input);

                    if (_match.Success != true)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 入力文字列が東陽図面番号の時、図面種類を文字列で返す
        /// </summary>
        /// <param name="PARTNUMBER"></param>
        /// <returns></returns>
        public static string GetToyoDrawingType(string PARTNUMBER)
        {
            string Ans = "";

            if (PARTNUMBER == "" || Regex.IsMatch(PARTNUMBER, @"^[0-9a-zA-Z]{1,}-[0-9a-zA-Z]{5,}-[0-9a-zA-Z]{1,}"))
            {
                if (Regex.IsMatch(PARTNUMBER, @"^\d{1,}-[0-9a-zA-Z]{5}-[0-9a-zA-Z]{1,}"))
                {
                    // 文字列を'-'で分解
                    string[] splitWord = PARTNUMBER.Split('-');

                    switch (splitWord[0])
                    {
                        case "01": // -レイアウト図
                            Ans = "レイアウト図";
                            break;
                        case "02": // -動作フローチャート図
                            Ans = "動作フローチャート図";
                            break;
                        case "03": // -据付検査用レイアウト図
                            Ans = "据付検査用レイアウト図";
                            break;
                        case "04": // -電装用レイアウト図
                            Ans = "電装用レイアウト図";
                            break;
                        case "05": // -残留リスクマップ用レイアウト図
                            Ans = "残留リスクマップ用レイアウト図";
                            break;
                        case "10": // -印刷ピッチ図
                            Ans = "印刷ピッチ図";
                            break;
                        case "11": // -パックレイアウト図
                            Ans = "パックレイアウト図";
                            break;
                        case "12": // -展開図
                            Ans = "展開図";
                            break;
                        case "13": // -駆動系統図
                            Ans = "駆動系統図";
                            break;
                        case "14": // -タイミングチャート図
                            Ans = "タイミングチャート図";
                            break;
                        case "15": // -配管図
                            Ans = "配管図";
                            break;
                        case "16": // -機器配置図
                            Ans = "機器配置図";
                            break;
                        case "17": // -電気設計データ表
                            Ans = "電気設計データ表";
                            break;
                        case "18": // -グリース配管図
                            Ans = "グリース配管図";
                            break;
                        case "19": // -テンションロール配置図
                            Ans = "テンションロール配置図";
                            break;
                        default:
                            Ans = "";
                            break;
                    }
                }
                else
                {
                    if (Regex.IsMatch(PARTNUMBER, @"^[A-Z][0-9A-Za-z]-[0-9]{3}00") == true)
                    {
                        Ans = "組立図";
                    }
                    else
                    {
                        Ans = "部品図";
                    }

                }
                return Ans;

            }

            return "";
        }


        /// <summary>
        /// イラスト保存用フォルダ返す M-301012-012  ->\M\301
        /// </summary>
        /// <param name="partNumber"></param>
        /// <returns></returns>
        public static string GetIllustFolder(string partNumber)
        {
            string[] splitWords = partNumber.Split('-');
            string ans;
            string No1 = "";
            string No2 = "";

            if (splitWords.Length >= 2)
            {
                No1 = splitWords[0];
                if (splitWords[1].Length >= 3)
                {
                    No2 = splitWords[1].Substring(0, 3);
                    ans = @"\" + No1 + @"\" + No2;
                }
                else
                {
                    ans = null;
                }
            }
            else
            {
                ans = null;
            }
            return ans;
        }

        /// <summary>
        /// \rで区切られた文字列
        /// "ABC\rDEF\rVFW" -> "ABC DEF VFW"
        /// "あいう\rABC"  -> "あいう\\ABC"
        /// "あいう\rABC\rDEF" ->  "あいう\\ABC\\DEF"
        /// "あいう\rうえお\rabc" =>  "あいううえお\\abc"
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SplitJpnAndEng(string s)
        {
            // ここで前後の余分なコード文字を削除
            string str = s.TrimSymbolTopAndEnd();

            System.Console.WriteLine("Original str: '{0}'", str);

            char[] delimiterChars = { '\r' };

            // 改行で配列にわける
            string[] wd = str.Split(delimiterChars);

            // 出力文字列を格納するオブジェクト
            StringBuilder _sb = new StringBuilder();

            for (int i = 0; i < wd.Count(); i++)
            {

                if (CheckKanji(wd[i]))
                {
                    //漢字・全角ひらがな・全角カタカナが混じっている場合
                    _sb.Append(wd[i]);

                    if (i < wd.Count() - 1)
                    {
                        _sb.Append(@"\");
                    }
                }
                else
                {
                    // 半角文字のみの場合 （英語名称はすべて大文字とする）
                    _sb.Append(wd[i].ToUpper());
                    if (i < wd.Count() - 1)
                    {
                        _sb.Append(@" ");
                    }

                }
            }

            return _sb.ToString();
        }

        // 特定ソフトウェア用
        /// <summary>
        /// メーカー品番に仕様や日本語が混じっている文字列からメーカー品番のみ抽出する
        /// 改行は除去する
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ExtractMakerPartNumber(this string text)
        {
            string s = text.Zen2Han();
            int length = s.Count();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                Console.WriteLine(s[i]);
                char a = s[i];
                // 改行を見つけた場合
                if (a == '\r')
                {
                    // \rの次の文字が漢字か判定
                    if (CheckKanji(s[i + 1].ToString()))
                    {
                        // 改行は削除して返す
                        return sb.ToString().Replace("\r", "");
                    }
                    // \rの次が'D=数値' 'd=数値' 'φ=数値'
                    else if (MatchStringRegex(s.Substring(i + 1), @"[DdHhLlφ]=\d"))
                    {
                        // 改行は削除して返す
                        return sb.ToString().Replace("\r", "");
                    }
                    else if (MatchStringRegex(s.Substring(i + 1), @"[DdHhLlφ]\d"))
                    {
                        // 改行は削除して返す
                        return sb.ToString().Replace("\r", "");
                    }
                    //\rの次のもじが空白
                    else if (s[i + 1] == ' ')
                    {
                        while (s[i + 1] == ' ')
                        {
                            char x = s[i];
                            i++;
                        }
                    }
                    else
                    {
                        i++;
                        ;
                        sb.Append(s[i]);
                    }
                }
                else if (CheckKanji(a.ToString()))
                {
                    // 改行は削除して返す
                    sb.Append(s[i]);
                }
                else
                {
                    sb.Append(s[i]);
                }
            }

            Console.WriteLine(sb.ToString().Replace("\r", ""));

            return sb.ToString().Replace("\r", "");
        }

        // 配列中の文字列を「'」で囲み、先頭に番号を付けて表示する
        public static void DisplayAll(this System.Collections.Generic.IEnumerable<string> words)
        {
            int count = 0;
            foreach (var s in words)
                Console.WriteLine($"{++count}：'{s}'");
        }

        // エンコード
        /// <summary>
        /// 文字列を指定した文字エンコーディングへ変換する
        /// </summary>
        /// <param name="src"></param>
        /// <param name="destEnc"></param>
        /// <returns></returns>
        public static string ConvertEncoding(string src, System.Text.Encoding destEnc)
        {
            byte[] src_temp = System.Text.Encoding.ASCII.GetBytes(src);
            byte[] dest_temp = System.Text.Encoding.Convert(System.Text.Encoding.ASCII, destEnc, src_temp);
            string ret = destEnc.GetString(dest_temp);
            return ret;
        }

        /// <summary>
        /// System.Byte[]に格納されたSJIS文字列をstring文字列に変換する
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SystemByteArraySJIStoString(object input)
        {
            string text = System.Text.Encoding.GetEncoding("shift_jis").GetString((byte[])input);
            return text;
        }

        /// <summary>
        /// System.Byte[]に格納されたUTF-8文字列をstring文字列に変換する
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SystemByteArrayUTF8toString(object input)
        {
            string text = System.Text.Encoding.GetEncoding("utf-8").GetString((byte[])input);
            return text;
        }

        public static byte[] StringToSystemByteArraySJIS(string input)
        {
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            byte[] bytes = sjisEnc.GetBytes(input);
            return bytes;
        }

        // LCMapStringW関連
        /// <summary>
        /// WIN32API:LCMapStringWを使い文字列をDwMapに従って変換する（拡張メソッド）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        static string SaSaLCMapString(this string str, NativeMethods.DwMap flags)
        {
            if (str != null)
            {
                var ci = System.Globalization.CultureInfo.CurrentCulture;
                // 強引に元の文字列×２のバッファを作る
                string result = new string(' ', str.Length * 2);
                NativeMethods.LCMapStringW(ci.LCID, (uint)flags, str, str.Length, result, result.Length);
                return result.TrimEnd();//末尾の空白を削除して返す
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// WIN32API:LCMapStringWを使い文字列を (LCMAP_HALFWIDTH | LCMAP_UPPERCASE) の設定で変換;
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SaSaLCMapStringHalFWIDHTandUPPERCASE(string str)
        {
            return (StringUtil.SaSaLCMapString(str, NativeMethods.DwMap.LCMAP_HALFWIDTH | NativeMethods.DwMap.LCMAP_UPPERCASE));
        }

        /// StringUtil Class end.
    }


    /// <summary>
    /// Win32API LCMapStringW を呼び出すクラス
    /// </summary>
    internal static class NativeMethods
    {
        public enum DwMap : uint
        {
            /// <summary>大文字と小文字を区別しません。</summary>
            NORM_IGNORECASE = 0x00000001,
            /// <summary>送りなし文字を無視します。このフラグをセットすると、日本語アクセント文字も削除されます。</summary>          
            NORM_IGNORENONSPACE = 0x00000002,
            /// <summary>記号を無視します。</summary>
            NORM_IGNORESYMBOLS = 0x00000004,
            /// <summary>小文字を使います。</summary>
            LCMAP_LOWERCASE = 0x00000100,
            /// <summary>大文字を使います。</summary>
            LCMAP_UPPERCASE = 0x00000200,
            /// <summary>正規化されたワイド文字並び替えキーを作成します。</summary>           
            LCMAP_SORTKEY = 0x00000400,
            /// <summary>Windows NT のみ : バイト順序を反転します。たとえば 0x3450 と 0x4822 を渡すと、結果は 0x5034 と 0x2248 になります。</summary>
            LCMAP_BYTEREV = 0x00000800,
            /// <summary>区切り記号を記号と同じものとして扱います。</summary>
            SORT_STRINGSORT = 0x00001000,
            /// <summary>ひらがなとカタカナを区別しません。ひらがなとカタカナを同じと見なします。</summary>
            NORM_IGNOREKANATYPE = 0x00010000,
            /// <summary>シングルバイト文字と、ダブルバイトの同じ文字とを区別しません。</summary>
            NORM_IGNOREWIDTH = 0x00020000,
            /// <summary>ひらがなにします。</summary>
            LCMAP_HIRAGANA = 0x00100000,
            /// <summary>カタカナにします。</summary>
            LCMAP_KATAKANA = 0x00200000,
            /// <summary>半角文字にします（適用される場合）。</summary>
            LCMAP_HALFWIDTH = 0x00400000,
            /// <summary>全角文字にします（適用される場合）。</summary>
            LCMAP_FULLWIDTH = 0x00800000,
            /// <summary>大文字と小文字の区別に、ファイルシステムの規則（既定値）ではなく、言語上の規則を使います。LCMAP_LOWERCASE、または LCMAP_UPPERCASE とのみ組み合わせて使えます。</summary>
            LCMAP_LINGUISTIC_CASING = 0x01000000,
            /// <summary>中国語の簡体字を繁体字にマップします。</summary>
            LCMAP_SIMPLIFIED_CHINESE = 0x02000000,
            /// <summary>中国語の繁体字を簡体字にマップします。</summary>
            LCMAP_TRADITIONAL_CHINESE = 0x04000000,
        }

        // WIN32API:LCMapStringWの宣言

        [DllImport("kernel32.dll")]
        public static extern int LCMapStringW(int Locale, uint dwMapFlags,
            [MarshalAs(UnmanagedType.LPWStr)] string lpSrcStr, int cchSrc,
            [MarshalAs(UnmanagedType.LPWStr)] string lpDestStr, int cchDest);

    }
}
