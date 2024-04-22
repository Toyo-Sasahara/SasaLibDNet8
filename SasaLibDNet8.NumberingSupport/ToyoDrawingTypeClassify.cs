using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using SasaLib;

namespace SasaLib.NumberingSupport
{
    [SupportedOSPlatform("windows")]
    public static class ToyoDrawingTypeClassify
    {
        /// <summary>
        /// ■入力された図面番号が東陽図番か調査する
        /// </summary>
        /// <param name="PARTNUMBER"></param>
        /// <param name="drawingType"></param>
        /// <param name="TypeName"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        public static bool CheckNumber_Obsoluete(string PARTNUMBER, out NumberTypeConfig.DrawingType drawingType, out string TypeName, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            bool isVariant = true;
            string suffixMIN = null;
            string suffixMAX = null;
            PARTNUMBER = StringUtil.Zen2HanANK_ZenSpace2HanSpace(PARTNUMBER).Trim().Trim('\t', '"', '\\').ToUpper();
            bool result = CheckNumber(PARTNUMBER, out drawingType, ref isVariant, ref suffixMIN, ref suffixMAX, out TypeName, delegateWriteLine);

            return result;
        }

        /// <summary>
        /// ■■■（重要）入力された図面番号が東陽図番か調査し、プリフィックスから図面種類を特定する（表形式図の場合suffixMIN, suffixMAXを返す）
        /// </summary>
        /// <param name="DrawingNumber">図面番号</param>
        /// <param name="drawingType">図面種類解析結果</param>
        /// <param name="isVariant"></param>
        /// <param name="suffixMIN"></param>
        /// <param name="suffixMAX"></param>
        /// <param name="TypeName"></param>
        /// <param name="WriteLine"></param>
        /// <returns> 東陽の一般的な図番の場合はtrue,妥当でない図番の場合はfalseを返す。妥当でなくてもArcSuiteには登録済みであることに注意 </returns>
        public static bool CheckNumber(string DrawingNumber, out NumberTypeConfig.DrawingType drawingType, ref bool isVariant, ref string suffixMIN, ref string suffixMAX, out string TypeName, SasaLibDelegateWriteLine WriteLine = null, bool verbose = true)
        {
            if (WriteLine == null) WriteLine = Console.WriteLine;

            if (verbose)
                WriteLine($"■ToyoDrawingTypeClassify.CheckNumber(..):  \"{DrawingNumber}\" について東陽正規図面番号かの判定を開始します");

            // 強制的に大文字化します
            DrawingNumber = DrawingNumber.ToUpper();

            TypeName = "分類不能"; // 分類不可の時のデフォルト。

            // 図面番号はハイフンで分割できること
            // 文字列を'-'で分解
            string[] delimiter = { "-" };
            string[] splitWord = DrawingNumber.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            int splitCount = splitWord.Count();

            if (splitCount > 1)
            {
                string FirstGroupStrings = splitWord[0];
                string LastGropuStrings = splitWord[splitWord.Count() - 1];
                if (verbose)
                    WriteLine($"■ToyoDrawingTypeClassify.CheckNumber(..)　\"{DrawingNumber}\" を\"-\"で分離。分離結果 {splitCount} トークンです");

                // ■■StagetServerDatabaseConfig.XMLより プレフィックスを使い DrawingClassを決定する

                DrawingClassEnum drawingClass = StageServerDatabaseConfig.GetDrawingTypeClass(FirstGroupStrings);
                if (verbose) 
                    WriteLine($"■ToyoDrawingTypeClassify.CheckNumber(..)　StageServerDatabaseConfig.GetDrawingClass(..) により  先頭トークン \"{FirstGroupStrings}\" は 図面クラス：{drawingClass} と判定されました");

                switch (drawingClass)
                {
                    case DrawingClassEnum.Parts: // ■パーツしか存在しないプレフィックスの場合
                        drawingType = NumberTypeConfig.DrawingType.TOYO_PART_Drawing;
                        TypeName = "部品図";
                        if (verbose)
                            WriteLine($"\t【{DrawingNumber}】  第一トークン \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。 \"{drawingType}（{TypeName}）\" と認識されました");

                        isVariant = isVariantCheckFrom_NumberTypeConfigXML(DrawingNumber, out suffixMIN, out suffixMAX, WriteLine, verbose);
                        if (isVariant)
                            WriteLine($"\t【{DrawingNumber}】 は isVariantCheckFrom_NumberTypeConfigXML(..)により{TypeName} の表図面と認識されました. suffixMIN:{suffixMIN},suffixMAX:{suffixMAX}");
                        break;

                    case DrawingClassEnum.Assy: // アセンブリしか存在しないプレフィックスの場合
                        drawingType = NumberTypeConfig.DrawingType.TOYO_ASSY_Drawing;
                        TypeName = "組立図";
                        if (verbose)
                            WriteLine($"\t【{DrawingNumber}】  第一トークン \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。 \"{drawingType}（{TypeName}）\" と認識されました");

                        isVariant = isVariantCheckFrom_NumberTypeConfigXML(DrawingNumber, out suffixMIN, out suffixMAX, WriteLine, verbose);

                        if (isVariant)
                            WriteLine($"\t【{DrawingNumber}】 は isVariantCheckFrom_NumberTypeConfigXML(..)により{TypeName}の表図面と認識されました. suffixMIN:{suffixMIN},suffixMAX:{suffixMAX}");

                        break;

                    case DrawingClassEnum.PartsOrAssy: // どちらも存在するプレフィックスの場合,  // NumberTypeConfig.XMLを使って掘り下げる
                        if (isASSYCheckFrom_NumberTypeConfigXML(DrawingNumber, WriteLine, verbose))
                        {
                            drawingType = NumberTypeConfig.DrawingType.TOYO_ASSY_Drawing;
                            TypeName = "組立図";
                            if (verbose)
                                WriteLine($"\t【{DrawingNumber}】  第一トークン \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。NumberTypeConfigXMLによる精査で \"{drawingType}（{TypeName}）\" と認識されました");

                            isVariant = isVariantCheckFrom_NumberTypeConfigXML(DrawingNumber, out suffixMIN, out suffixMAX, WriteLine, verbose);
                            if (isVariant)
                                WriteLine($"\t【{DrawingNumber}】 は isVariantCheckFrom_NumberTypeConfigXML(..)により{TypeName}の表図面と認識されました. suffixMIN:{suffixMIN},suffixMAX:{suffixMAX}");
                        }
                        else if (isPARTCheckFrom_NumberTypeConfigXML(DrawingNumber, WriteLine, verbose))
                        {
                            drawingType = NumberTypeConfig.DrawingType.TOYO_PART_Drawing;
                            TypeName = "部品図";
                            if (verbose)
                                WriteLine($"\t【{DrawingNumber}】  第一トークン \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。NumberTypeConfigXMLによる精査で \"{drawingType}（{TypeName}）\" と認識されました");
                            isVariant = isVariantCheckFrom_NumberTypeConfigXML(DrawingNumber, out suffixMIN, out suffixMAX, WriteLine, verbose);
                            if (isVariant)
                                WriteLine($"\t【{DrawingNumber}】 は isVariantCheckFrom_NumberTypeConfigXML(..)により{TypeName}の表図面と認識されました. suffixMIN:{suffixMIN},suffixMAX:{suffixMAX}");
                        }
                        else
                        {
                            drawingType = NumberTypeConfig.DrawingType.Unknown;
                            TypeName = "分類不能";
                            if (verbose)
                                WriteLine($"\t【{DrawingNumber}】  第一トークン \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。NumberTypeConfigXMLによる精査で \"{drawingType}（{TypeName}）\" と認識されました");
                            if (TypeName == "分類不能")
                                return false;
                        }
                        break;

                    case DrawingClassEnum.Technical: // 技術図書のプレフィックスの場合
                        drawingType = NumberTypeConfig.DrawingType.TOYO_TECHNICAL_Drawing;
                        TypeName = StageServerDatabaseConfig.GetDrawingTypeTypeName(FirstGroupStrings);
                        if (verbose)
                            WriteLine($"\t【{DrawingNumber}】  第一トークン  \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。NumberTypeConfigXMLによる精査で \"{drawingType}（{TypeName}）\" と認識されました");
                        break;

                    case DrawingClassEnum.Unknown: // DrawingClassEnum.Unknownの場合 ﾊﾟｰﾂかｱｾﾝﾌﾞﾘかでﾋｯﾄするか調査する。
                        if (isASSYCheckFrom_NumberTypeConfigXML(DrawingNumber, WriteLine, verbose))
                        {
                            drawingType = NumberTypeConfig.DrawingType.TOYO_ASSY_Drawing;
                            TypeName = "組立図";
                            if (verbose)
                                WriteLine($"\t【{DrawingNumber}】  第一トークン  \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。 NumberTypeConfigXMLによる精査で\"{drawingType}（{TypeName}）\" と認識されました");
                            isVariant = isVariantCheckFrom_NumberTypeConfigXML(DrawingNumber, out suffixMIN, out suffixMAX, WriteLine, verbose);
                        }
                        else if (isPARTCheckFrom_NumberTypeConfigXML(DrawingNumber, WriteLine, verbose))
                        {
                            drawingType = NumberTypeConfig.DrawingType.TOYO_PART_Drawing;
                            TypeName = "部品図";
                            if (verbose)
                                WriteLine($"\t【{DrawingNumber}】  第一トークン  \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。NumberTypeConfigXMLによる精査で \"{drawingType}（{TypeName}）\" と認識されました");
                            isVariant = isVariantCheckFrom_NumberTypeConfigXML(DrawingNumber, out suffixMIN, out suffixMAX, WriteLine, verbose);
                        }
                        else // ﾊﾟｰﾂ・ｱｾﾝﾌﾞﾘどちらもヒットしない場合は NumberTypeConfig.DrawingType.TOYO_TECHNICAL_Drawing とし TypeNameを検索
                        {
                            drawingType = NumberTypeConfig.DrawingType.TOYO_TECHNICAL_Drawing;
                            TypeName = StageServerDatabaseConfig.GetDrawingTypeTypeName(FirstGroupStrings);
                            if (verbose)
                                WriteLine($"\t【{DrawingNumber}】  第一トークン  \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。NumberTypeConfigXMLによる精査で \"{drawingType}（{TypeName}）\" と認識されました");
                            if (TypeName == "分類不能")
                                return false;
                        }
                        break;

                    default:
                        drawingType = NumberTypeConfig.DrawingType.Unknown;
                        TypeName = "分類不能";
                        if (verbose)
                            WriteLine($"\t【{DrawingNumber}】  第一トークン \"{FirstGroupStrings}\" を \"{drawingClass}\" と判定。 \"{drawingType}（{TypeName}）\" と認識されました");
                        Eventlog.Log.WriteEntry("ToyoDATABASE", EventLogEntryType.Error, 9700, $"GetToyoDrawingType(..)にて【{DrawingNumber}】を '分類不能' と認識しました");
                        return false;
                }
                return true;
            }
            else
            {
                if (verbose)
                    WriteLine($"※ToyoDrawingTypeClassify.CheckNumber(..): 【{DrawingNumber}】を\"-\"で分割できません");
                drawingType = NumberTypeConfig.DrawingType.Unknown;
                TypeName = "分類不能";
                return false;
            }
        }

        /// <summary>
        /// ArcSuite検索のみに通用する 変換テーブルを上から順に試し 入力文字列と置換結果とに違いが最初に発生したテーブルのReplacement文字列を返す
        /// </summary>
        /// <param name="PARTNUMBER"></param>
        /// <param name="sanitizedPartnumber">変換結果。テーブルにヒットしない場合はそのまま返る</param>
        /// <returns></returns>
        internal static bool ArcSuiteSpecealConversionFormulaNumber(string PARTNUMBER, out string sanitizedPartnumber, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            sanitizedPartnumber = PARTNUMBER;

            if (ConversionFormulaNumberConfig.Config != null)
            {
                RegexOptions options = RegexOptions.IgnoreCase;

                if (ConversionFormulaNumberConfig.Config.ConversionFormulas == null)
                {
                    delegateWriteLine($"※ConversionFormulaNumberConfig.Config.ConversionFormulas がnullです");
                    return false;
                }

                foreach (ConversionFormula ConversionFormula in ConversionFormulaNumberConfig.Config.ConversionFormulas)
                {
                    var Pattern = ConversionFormula.Pattern;
                    var Replacement = ConversionFormula.Replacement;
                    string replacementedPARTNUMBER = Regex.Replace(PARTNUMBER, Pattern, Replacement, options);
                    if (replacementedPARTNUMBER != PARTNUMBER)
                    {
                        sanitizedPartnumber = replacementedPARTNUMBER;
                        delegateWriteLine($"■ConversionFormulaNumberConfig.Config.ConversionFormulas にヒットしました  Pattern\"{Pattern}\" PARTNUMBER \"{PARTNUMBER}\" -> \"{replacementedPARTNUMBER}\"");
                        return true;
                    }
                }

                return false;

            }
            else
                return false;
        }

        /// <summary>
        /// パーツ番号を NumberTypeConfig.XML使い 【組立図面番号】ならtrueを返す
        /// </summary>
        /// <param name="input"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        private static bool isASSYCheckFrom_NumberTypeConfigXML(string input, SasaLibDelegateWriteLine delegateWriteLine = null, bool verbose = true)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            if (NumberTypeConfig.Config.RegexPatern_TOYO_ASSY_Drawing == null)
            {
                delegateWriteLine($"※NumberTypeConfig.Config.RegexPatern_TOYO_ASSY_Drawing がnullです");
                return false;
            }

            foreach (var pattern in NumberTypeConfig.Config.RegexPatern_TOYO_ASSY_Drawing)
            {
                bool result = Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
                if (result == true)
                {
                    if (verbose)  delegateWriteLine($"\t【{input}】は【組立図面番号】 である。　　正規表現【{pattern}】の結果より判定");

                    return true;
                }
                else
                {
                    if (verbose) delegateWriteLine($"\t【{input}】は【組立図面番号】 ではない。　　正規表現【{pattern}】の結果より判定");                  
                }
            }
            return false;
        }

        /// <summary>
        ///  【部品図面番号】ならtrueを返す
        /// </summary>
        /// <param name="input"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        private static bool isPARTCheckFrom_NumberTypeConfigXML(string input, SasaLibDelegateWriteLine delegateWriteLine = null, bool verbose = true)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            if (NumberTypeConfig.Config.RegexPatern_TOYO_PART_Drawing == null)
            {
                delegateWriteLine($"※NumberTypeConfig.Config.RegexPatern_TOYO_PART_Drawing がnullです");
                return false;
            }

            foreach (var pattern in NumberTypeConfig.Config.RegexPatern_TOYO_PART_Drawing)
            {
                //SasaLib.DoEvents.Run();

                bool result = Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
                if (result == true)
                {
                    if (verbose)  delegateWriteLine($"\t【{input}】は【部品面番号】 である。　正規表現【{pattern}】の結果より判定");
                    
                    return true;
                }
                else
                {
                    if (verbose) delegateWriteLine($"\t【{input}】は【部品面番号】 ではない。　正規表現【{pattern}】の結果より判定");
                }
            }
            return false;
        }

        /// <summary>
        /// 表図面かを調査
        /// </summary>
        /// <param name="input"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        private static bool isVariantCheckFrom_NumberTypeConfigXML(string input, out string suffixMIN, out string suffixMAX, SasaLibDelegateWriteLine delegateWriteLine = null, bool verbose = true)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;
            suffixMIN = null;
            suffixMAX = null;

            if (NumberTypeConfig.Config.RegexPatern_TOYO_Variant_Suffix == null)
            {
                delegateWriteLine($"※NumberTypeConfig.Config.RegexPatern_TOYO_Variant_Suffix がnullです");
                return false;
            }


            foreach (var pattern in NumberTypeConfig.Config.RegexPatern_TOYO_Variant_Suffix)
            {
                bool result = AnalyzeVariantSuffixNumber(input, pattern, out suffixMIN, out suffixMAX, delegateWriteLine);
                if (result == true)
                {
                    if (verbose) delegateWriteLine($"\t【{input}】は【表形式図面】 である。　サフィックスの値 MIN:{suffixMIN} MAX:{suffixMAX}　正規表現【{pattern}】の結果より判定");

                    return true;
                }
                else
                {
                    if (verbose) delegateWriteLine($"\t【{input}】は【表形式図面 】ではない。正規表現【{pattern}】の結果より判定");
                }
            }
            return false;
        }

        /// <summary>
        /// 表形式図面番号の、サフィックス文字列 xxx～xxx を取り出します。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="suffixMIN"></param>
        /// <param name="suffixMAX"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        private static bool AnalyzeVariantSuffixNumber(string input, string pattern, out string suffixMIN, out string suffixMAX, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            try
            {
                MatchCollection result = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);

                bool ans = false;
                suffixMIN = null;
                suffixMAX = null;

                if (result.Count == 1)
                {
                    int suffixMinInt; int suffixMaxInt;
                    if (int.TryParse(result[0].Groups[1].Captures[0].Value, out suffixMinInt))
                    {
                        suffixMIN = result[0].Groups[1].Captures[0].Value;
                        ans = true;
                    }
                    else
                        ans = false;
                    if (int.TryParse(result[0].Groups[2].Captures[0].Value, out suffixMaxInt))
                    {
                        suffixMAX = result[0].Groups[2].Captures[0].Value;
                        ans = true;
                    }
                    else
                        ans = false;
                }
                return ans;
            }
            catch (Exception ex)
            {
                delegateWriteLine($"※ToyoDrawingTypeClassify.AnalyzeVariantSuffixNumber(..) にて例外発生 {ex.Message} {ex.InnerException}");

                suffixMIN = null;
                suffixMAX = null;
                return false;
            }

        }
    }
}
