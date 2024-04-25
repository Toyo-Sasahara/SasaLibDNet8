using System;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace SasaLib.NumberingSupport
{
    //public delegate void DelegateWriteLine(string msg);

    [SupportedOSPlatform("windows")]
    public class NumberingSupport
    {
        public string NumberingServerName { get; set; }

        public int NumberingServeMySqlPortNumber { get; set; }


        public string NumberingServerConnectUser { get; private set; }

        public string NumberingServerConnectPass { get; private set; }

        /// <summary>
        /// 図面種類エニュミレータ
        /// </summary>
        public enum DrawingTypeEnum
        {
            Part,
            Assy,
            Layout,
            Other
        }




        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="NumberingServerName"></param>
        /// <param name="MySqlPortNumber"></param>
        public NumberingSupport(string NumberingServerName, int MySqlPortNumber, string DB_Connect_User, string DB_Connect_Pass)
        {
            this.NumberingServerName = NumberingServerName;
            this.NumberingServeMySqlPortNumber = MySqlPortNumber;
            this.NumberingServerConnectUser = DB_Connect_User;
            this.NumberingServerConnectPass = DB_Connect_Pass;
        }

        /// <summary>
        /// 採番システムに採番実績を問い合わせる
        /// </summary>
        /// <param name="PARTNUMBER">問い合わせる</param>
        /// <param name="findMessage">結果が文字列として返る</param>
        /// <returns>正常動作ならtrue</returns>
        public bool CheckAcquiredNumbered(string PARTNUMBER, out bool NumberingRecordAvaliableFinulAnser, out string findMessage, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            try
            {
                if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

                /// データベース接続可否（正常接続か？）
                bool dbConnectNormal = false;

                bool _partNumberingRecordAvaliable = false;
                bool _partWithoutRLNumberingRecordAvaliable = false;
                string partAnserMessage = null;
                string partWitoutRLAnserMessage = null;

                bool _kumizuNumberingRecordAvaliable = false;
                string kumizuAnzser = null;

                bool _layoutNumberingRecordAvaliable = false;
                string layoutAnser = null;

                string PREF_NAME;
                string PREF_RL;

                NumberingRecordAvaliableFinulAnser = false;
                findMessage = null;

                if (Net.CheckPing(this.NumberingServerName) == false)
                {
                    delegateWriteLine($"※CheckAcquiredNumbered(..) 採番ｻｰﾊﾞｰ{this.NumberingServerName} はPINGに応答しませんでした。falseで抜けます");
                    return false;
                }

                SplitPARTNUMBERtoRL(PARTNUMBER, out PREF_NAME, out PREF_RL);

                SasaLibMySqlConnect MysqlDB_buhinzu_Name = new SasaLibMySqlConnect(this.NumberingServerName, this.NumberingServeMySqlPortNumber, "db_buhinzu", "utf8", NumberingServerConnectUser, NumberingServerConnectUser);
                SasaLibMySqlConnect MysqlDB_kumizu_Name = new SasaLibMySqlConnect(this.NumberingServerName, this.NumberingServeMySqlPortNumber, "db_kumizu", "utf8", NumberingServerConnectUser, NumberingServerConnectUser);
                SasaLibMySqlConnect MysqlDB_layout_Name = new SasaLibMySqlConnect(this.NumberingServerName, this.NumberingServeMySqlPortNumber, "db_layout", "utf8", NumberingServerConnectUser, NumberingServerConnectUser);


                dbConnectNormal = MysqlDB_buhinzu_Name.DrawingPartNumberFindFirst("t01prefecture", PREF_NAME, PREF_RL, out _partNumberingRecordAvaliable, out partAnserMessage);

                if (_partNumberingRecordAvaliable) { partAnserMessage = "部品図(図番完全一致)として" + partAnserMessage; }

                if (dbConnectNormal)
                    delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン① 部品図・ﾃﾞｰﾀﾍﾞｰｽ sampledb050 に対する接続成功 PREF_NAME(ゼロ補完なし)(ラストのRLを除去した文字列)=【{PREF_NAME}】,PREF_RL=【{PREF_RL}】検索結果【{partAnserMessage}】");
                else
                    delegateWriteLine($"※NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン① 部品図・ﾃﾞｰﾀﾍﾞｰｽ sampledb050 に対する接続失敗 PREF_NAME(ゼロ補完なし)(ラストのRLを除去した文字列)=【{PREF_NAME}】,PREF_RL=【{PREF_RL}】");

                if (_partNumberingRecordAvaliable == false)
                {
                    string ZeroPaddingdPREF_NAME = SanitaizingSimplificationString(PREF_NAME);

                    dbConnectNormal = MysqlDB_buhinzu_Name.DrawingPartNumberFindFirst("t01prefecture", ZeroPaddingdPREF_NAME, PREF_RL, out _partNumberingRecordAvaliable, out partAnserMessage);

                    if (_partNumberingRecordAvaliable) { partAnserMessage = "部品図(枝番ｾﾞﾛ補完後一致)として" + partAnserMessage; }
                    if (dbConnectNormal)
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン② 部品図・ﾃﾞｰﾀﾍﾞｰｽ sampledb050 に対する接続成功 ZeroPaddingdPREF_NAME(枝番3桁ゼロ補完)(ラストのRLを除去した文字列)=【{ZeroPaddingdPREF_NAME}】PREF_RL=【{PREF_RL}】検索結果【{partAnserMessage}】");
                    else
                        delegateWriteLine($"※NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン② 部品図・ﾃﾞｰﾀﾍﾞｰｽ sampledb050 に対する接続失敗 ZeroPaddingdPREF_NAME(枝番3桁ゼロ補完)(ラストのRLを除去した文字列)=【{ZeroPaddingdPREF_NAME}】PREF_RL=【{PREF_RL}】");

                } // 検索パターン①でヒットしなかった場合は検索パターン②にて調査

                if (_partNumberingRecordAvaliable == false)
                {
                    dbConnectNormal = MysqlDB_buhinzu_Name.DrawingPartNumberFindFirst("t01prefecture", PARTNUMBER, "", out _partNumberingRecordAvaliable, out partWitoutRLAnserMessage);

                    if (_partNumberingRecordAvaliable) { partAnserMessage = "部品図(枝番ｾﾞﾛ補完後一致/RL無し)として" + partAnserMessage; }
                    if (dbConnectNormal)
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン③ 部品図・ﾃﾞｰﾀﾍﾞｰｽ sampledb050 に対する接続成功 PARTNUMBER(入力値のまま)=【{PARTNUMBER}】,検索結果【{partAnserMessage}】");
                    else
                        delegateWriteLine($"※NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン③ 部品図・ﾃﾞｰﾀﾍﾞｰｽ sampledb050 に対する接続失敗 PARTNUMBER(入力値のまま)=【{PARTNUMBER}】");
                } // 検索パターン②でヒットしなかった場合は検索パターン③にて調査

                if (_partNumberingRecordAvaliable == false)
                {
                    dbConnectNormal = MysqlDB_buhinzu_Name.DrawingPartNumberFindFirst("t01prefecture", PREF_NAME, "", out _partNumberingRecordAvaliable, out partAnserMessage);

                    if (_partNumberingRecordAvaliable) { partAnserMessage = "部品図(RL無し)として" + partAnserMessage; }
                    if (dbConnectNormal)
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン④ 部品図・ﾃﾞｰﾀﾍﾞｰｽ sampledb050 に対する接続成功 PREF_NAME(ゼロ補完なし)(ラストのRLを除去した文字列)【{PREF_NAME}】,PREF_RL=【】検索結果【{partAnserMessage}】");
                    else
                        delegateWriteLine($"※NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン④ 部品図・ﾃﾞｰﾀﾍﾞｰｽ sampledb050 に対する接続失敗 PREF_NAME(ゼロ補完なし)(ラストのRLを除去した文字列)【{PREF_NAME}】");
                } // 検索パターン③でヒットしなかった場合は検索パターン④にて調査

                if (_partNumberingRecordAvaliable == false)
                {

                    dbConnectNormal = MysqlDB_kumizu_Name.DrawingAssyNumberFindFirst("t01prefecture", PARTNUMBER, out _kumizuNumberingRecordAvaliable, out kumizuAnzser);

                    if (_partNumberingRecordAvaliable) { partAnserMessage = "組立図(図番完全一致)として" + partAnserMessage; }
                    if (dbConnectNormal)
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン⑤ 組立図・ﾃﾞｰﾀﾍﾞｰｽ dbkumizu に対する接続成功 PARTNUMBER=【{PARTNUMBER}】 検索結果【{kumizuAnzser}】");
                    else
                        delegateWriteLine($"※NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン⑤ 組立図・ﾃﾞｰﾀﾍﾞｰｽ dbkumizu に対する接続失敗 PARTNUMBER=【{PARTNUMBER}】");

                    if (_kumizuNumberingRecordAvaliable == false)
                    {
                        string ZeroPaddingdPARTNUMBER = SanitaizingSimplificationString(PARTNUMBER);

                        dbConnectNormal = MysqlDB_kumizu_Name.DrawingAssyNumberFindFirst("t01prefecture", ZeroPaddingdPARTNUMBER, out _kumizuNumberingRecordAvaliable, out kumizuAnzser);

                        if (_partNumberingRecordAvaliable) { partAnserMessage = "組立図(枝番ｾﾞﾛ保管後一致)として" + partAnserMessage; }
                        if (dbConnectNormal)
                            delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン⑥ 組立図・ﾃﾞｰﾀﾍﾞｰｽ dbkumizu に対する接続成功 ZeroPaddingdPARTNUMBER=【{ZeroPaddingdPARTNUMBER}】検索結果【{kumizuAnzser}】");
                        else
                            delegateWriteLine($"※NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン⑥ 組立図・ﾃﾞｰﾀﾍﾞｰｽ dbkumizu に対する接続失敗 ZeroPaddingdPARTNUMBER=【{ZeroPaddingdPARTNUMBER}】");
                    }
                } // 部品図として見つからなかった場合組立図として検索する

                if (_partNumberingRecordAvaliable == false && _kumizuNumberingRecordAvaliable == false)
                {

                    dbConnectNormal = MysqlDB_layout_Name.DrawingLayoutNumberFindFirst("t01prefecture", PARTNUMBER, out _layoutNumberingRecordAvaliable, out layoutAnser);

                    if (_partNumberingRecordAvaliable) { partAnserMessage = "ﾚｲｱｳﾄ図(図番完全一致)として" + partAnserMessage; }
                    if (dbConnectNormal)
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン⑦ ﾚｲｱｳﾄ・ﾃﾞｰﾀﾍﾞｰｽ dblayout に対する接続成功 【{dbConnectNormal}】【{PARTNUMBER}】検索結果 【{layoutAnser}】");
                    else
                        delegateWriteLine($"※NumberingSupport.CheckAcquiredNumbered() 採番システム：検索パターン⑦ ﾚｲｱｳﾄ・ﾃﾞｰﾀﾍﾞｰｽ dblayout に対する接続失敗 【{dbConnectNormal}】【{PARTNUMBER}】");
                } //部品図としても組立図としても見つからなかった場合は技術図書図として検索する

                if (dbConnectNormal)
                {
                    if (_partNumberingRecordAvaliable == true)
                    {
                        NumberingRecordAvaliableFinulAnser = true;
                        findMessage = partAnserMessage;
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム上での図面種類判定結果：部品図です： {findMessage}");
                        return true;
                    }
                    else if (_partWithoutRLNumberingRecordAvaliable == true)
                    {
                        NumberingRecordAvaliableFinulAnser = true;
                        findMessage = partWitoutRLAnserMessage;
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム上での図面種類判定結果：部品図です(RL指定をPREF__NAME側に含めることでヒット)： {findMessage}");
                        return true;

                    }
                    else if (_kumizuNumberingRecordAvaliable == true)
                    {
                        NumberingRecordAvaliableFinulAnser = true;
                        findMessage = kumizuAnzser;
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム上での図面種類判定結果：組立図です： {findMessage}");
                        return true;
                    }
                    else if (_layoutNumberingRecordAvaliable == true)
                    {
                        NumberingRecordAvaliableFinulAnser = true;
                        findMessage = layoutAnser;
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム上での図面種類判定結果：技術図書図です： {findMessage}");
                        return true;
                    }
                    else
                    {
                        NumberingRecordAvaliableFinulAnser = false;
                        findMessage = $"{PARTNUMBER}の採番実績がありませんでした";
                        delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム上での図面種類判定結果：{findMessage}");
                        return true;
                    }
                }
                else
                {
                    NumberingRecordAvaliableFinulAnser = true;
                    findMessage = "注意：採番システムに接続できませんでした";
                    delegateWriteLine($"■NumberingSupport.CheckAcquiredNumbered() 採番システム上での図面種類判定結果：{findMessage}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                delegateWriteLine($"※NumberingSupport.CheckAcquiredNumbered() にて例外検知{ex.Message} {ex.InnerException}");
                NumberingRecordAvaliableFinulAnser = false;
                findMessage = null;
                return false;
            }
        }

        /// <summary>
        /// 部品番号からRL,R,L を分離し、PREF_NAME　PREF_RLに戻す。
        /// </summary>
        /// <param name="PARTNUMBER"></param>
        /// <param name="PREF_NAME"></param>
        /// <param name="PREF_RL"></param>
        private void SplitPARTNUMBERtoRL(string PARTNUMBER, out string PREF_NAME, out string PREF_RL)
        {
            string withoutRL = Regex.Replace(PARTNUMBER, $"(RL)$", "", RegexOptions.IgnoreCase);
            string withooutR = Regex.Replace(PARTNUMBER, $"(R)$", "", RegexOptions.IgnoreCase);
            string withouutL = Regex.Replace(PARTNUMBER, $"(L)$", "", RegexOptions.IgnoreCase);

            if (PARTNUMBER != withoutRL || PARTNUMBER != withooutR || PARTNUMBER != withouutL)
            {
                if (withoutRL != null)
                {
                    PREF_NAME = withoutRL;
                    PREF_RL = "RL";
                }
                else if (withooutR != null)
                {
                    PREF_NAME = withooutR;
                    PREF_RL = "R";
                }
                else if (withouutL != null)
                {
                    PREF_NAME = withouutL;
                    PREF_RL = "L";
                }
                else
                {
                    PREF_NAME = PARTNUMBER;
                    PREF_RL = null;
                }
            }
            else
            {
                PREF_NAME = PARTNUMBER;
                PREF_RL = null;
            }
        }

        /// <summary>
        /// サニタイズ関数（空白を削除したのち、全角を半角にする。（半角ｶﾅは使用しない）最後に、ハイフンで区切られた3項目目をゼロ保管処理）
        /// </summary>
        /// <param name="orgValue"></param>
        /// <returns></returns>
        private string SanitaizingSimplificationString(string orgValue)
        {
            string sanitaizedValue;
            bool result = ArcSuiteSupport.ZeroPaddingPartNumber(orgValue, out sanitaizedValue);
            return sanitaizedValue;
        }

        /// <summary>
        /// ■採番システムに次の図番候補を検索させるため、ﾊﾟｰﾂ番号の末尾ｻﾌｨｯｸｽを削除した文字列を返します。例：M-12345-123  -> M-12345-
        /// </summary>
        /// <param name="str"></param>
        /// <param name="drawingType"></param>
        /// <returns></returns>
        public static string RemoveSuffixNumber(string str, out DrawingTypeEnum drawingType)
        {
            string newString = null;
            // 文字列を'-'で分解
            string[] splitWord = str.Split('-');
            // いくつの単語に分かれたか調査
            switch (splitWord.Length)
            {
                case 0:
                case 1:
                case 2:
                    drawingType = DrawingTypeEnum.Other;
                    return null;

                case 3:
                case 4:
                    // 文字列中のハイフンの数が2個以上はここで処理
                    // M-10201-R    ->  M-10201-
                    // M-10201-RL    ->  M-10201-
                    // M-10201-12RL    ->  M-10201-
                    // M-10201-1    ->  M-10201-1
                    // M-10201-3R    ->  M-10201-
                    Console.WriteLine($"GetZeroPaddingPARTNUMBER(string)配列要素３こ={str}\n");
                    if (splitWord[1].Substring(splitWord[1].Length - 2) == "00")
                    {
                        drawingType = DrawingTypeEnum.Assy;
                        newString = splitWord[0] + "-" + splitWord[1] + "-";
                    }
                    else if (Regex.IsMatch(str, @"^[0-9][0-9]-\d{5}-"))
                    {
                        drawingType = DrawingTypeEnum.Layout;
                        splitWord[1] = "*";
                        newString = String.Join("-", splitWord);
                    }
                    else
                    {
                        drawingType = DrawingTypeEnum.Part;
                        newString = splitWord[0] + "-" + splitWord[1] + "-";
                    }
                    return newString;
                default:
                    drawingType = DrawingTypeEnum.Other;
                    return null;
            }
        }
    }
}
