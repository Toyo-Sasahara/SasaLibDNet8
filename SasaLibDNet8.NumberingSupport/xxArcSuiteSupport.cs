//using SasaLib.NumberingSupport;
//using SasaLibDummy;
//using StageServerRemote;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Versioning;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace SasaLib.NumberingSupport
//{

//    /// <summary>
//    /// ArcSuiteSupportクラス
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public static class ArcSuiteSupport
//    {
//        /// <summary>
//        /// ■ConversionFormulaNumberConfig.XMLの記述に従い 部品番号を変更する
//        /// </summary>
//        /// <param name="inputNUMBER"></param>
//        /// <param name="WriteLine"></param>
//        /// <returns></returns>
//        public static string GetArcSuiteSpecealCovertedPARTNUMBER(string inputNUMBER, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            string ConvertedNumber;
//            NumberTypeConfig.DrawingType drawingType;
//            string TypeName;
//            string result = GetArcSuiteSpecealCovertedPARTNUMBER(inputNUMBER, out ConvertedNumber, out drawingType, out TypeName, WriteLine);
//            return result;
//        }

//        /// <summary>
//        /// ■ConversionFormulaNumberConfig.XMLの記述に従い 部品番号を変更する
//        /// </summary>
//        /// <param name="inputNUMBER"></param>
//        /// <param name="ConvertedNumber"></param>
//        /// <param name="drawingType"></param>
//        /// <param name="TypeName"></param>
//        /// <param name="WriteLine"></param>
//        /// <returns></returns>
//        public static string GetArcSuiteSpecealCovertedPARTNUMBER(string inputNUMBER, out string ConvertedNumber, out NumberTypeConfig.DrawingType drawingType, out string TypeName, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            if (WriteLine == null) WriteLine = Console.WriteLine;

//            ConvertedNumber = inputNUMBER;
//            string sanitizeNumber = null;

//            bool ConvertedFormulaNumber = ToyoDrawingTypeClassify.ArcSuiteSpecealConversionFormulaNumber(inputNUMBER, out ConvertedNumber,WriteLine);
//            if (ConvertedFormulaNumber)
//            {
//                WriteLine($"■{inputNUMBER}をArcSuiteSpecealConversionFormulaNumber(..)を使い変換 →  sanitizeNumber={ConvertedNumber}になりました。");
//                sanitizeNumber = ConvertedNumber;

//            }
//            else
//            {
//                WriteLine($"■{inputNUMBER}をArcSuiteSpecealConversionFormulaNumber(..)を使い変換 → テーブルにヒットしません。  sanitizeNumber={inputNUMBER}になります。");
//                sanitizeNumber = inputNUMBER;
//            }

//            bool isVariant = true;
//            string suffixMIN = null;
//            string suffixMAX = null;
//            var numberIsToyo = ToyoDrawingTypeClassify.CheckNumber(inputNUMBER, out drawingType, ref isVariant, ref suffixMIN, ref suffixMAX, out TypeName, WriteLine);
//            WriteLine($"■入力値inputNUMBERは inputNUMBER={inputNUMBER} drawingType={drawingType} TypeName={TypeName}と判定しました");

            
//            if (numberIsToyo)
//            {
//                switch (drawingType)
//                {
//                    case NumberTypeConfig.DrawingType.TOYO_ASSY_Drawing:
//                    case NumberTypeConfig.DrawingType.TOYO_PART_Drawing:
//                        if (ConvertedFormulaNumber)
//                        {
//                            sanitizeNumber = ConvertedNumber;
//                        }
//                        else
//                        {
//                            sanitizeNumber = ArcSuiteSupport.ConvertSanitaizedPartnumber(ConvertedNumber);
//                        }

//                        break;
//                    case NumberTypeConfig.DrawingType.TOYO_TECHNICAL_Drawing:
//                        sanitizeNumber = inputNUMBER;
//                        break;
//                    case NumberTypeConfig.DrawingType.Unknown:
//                        sanitizeNumber = inputNUMBER;
//                        break;
//                }
//                WriteLine($"■入力値[{inputNUMBER}]は東陽図面形式・種類 [{drawingType}]と判断しています");
//            }
//            else
//            {
//                sanitizeNumber = inputNUMBER;
//                WriteLine($"※入力値[{inputNUMBER}は非東陽形式と判断");
//            }
//            return sanitizeNumber;
//        }

//        /// <summary>
//        /// ■ArcSuiteへ図面番号を検索して表示
//        /// </summary>
//        /// <param name="PARTNUMBER">アークスイートから検索する図面番号</param>
//        public static void DrawingSearchAndWebOpen(string SANITIZEDPARTNUMBER, string ArcSuiteSearchURL)
//        {
//            if (string.IsNullOrWhiteSpace(SANITIZEDPARTNUMBER) != true)
//            {

//                //string ArcSuiteURL = $"http://ass1/ArcSuite/docspace/sdk/contentView.do" +
//                //                    $"?enc=UTF-8&service=cn%3Ddrep_service%40ass1%2Cou%3Dcomponents%2Cdc%3D" +
//                //                    $"ArcSuite&workspace=TECHS&condition=TECHS&item=0%3Aoperator%3Ainclude&item=0%3Avalue%3A" +
//                //                    $"{SANITIZEDPARTNUMBER}";
//                string ArcSuiteURL = ArcSuiteSearchURL.Replace("{SANITIZEDPARTNUMBER}", SANITIZEDPARTNUMBER);
//                System.Diagnostics.Process.Start(ArcSuiteURL);
//            }
//        }

//        /// <summary>
//        /// 図面番号を想定したﾌｧｲﾙ名からアークスイート検索用文字列を生成する（変換結果は参照返し）
//        /// RL-R,RL-L は RLに変換されます
//        /// </summary>
//        /// <param name="PARTNUMBER">入力文字列</param>
//        /// <returns>変換結果を返還後文字列で返す.変換失敗した場合は入力値をそのまま返す</returns>
//        public static string ConvertSanitaizedPartnumber(string PARTNUMBER, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            if (WriteLine == null) WriteLine = Console.WriteLine;

//            string snitaizedPartNumber;
//            bool ans = ConvertSanitaizedPartnumber(PARTNUMBER, out snitaizedPartNumber, WriteLine);

//            if (ans)
//                return snitaizedPartNumber;
//            else
//                return PARTNUMBER;
//        }

//        /// <summary>
//        /// 大文字小文字を無視し半角の RL-R,RL-L は RLに変換されます。それ以外は（変換結果は参照返し）
//        /// </summary>
//        /// <param name="PARTNUMBER">元の図面番号文字列。</param>
//        /// <param name="ans">変更された文字列　Inventor拡張子がある場合のみ拡張子を削除される</param>
//        /// <returns>変更された場合true</returns>
//        public static bool ConvertSanitaizedPartnumber(string PARTNUMBER, out string outPartNumber, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            WriteLine($"■ArcSuiteSupport.ConvertSanitaizedPartnumber(...) 実行");


//            if (WriteLine == null) WriteLine = Console.WriteLine;

//            outPartNumber = PARTNUMBER;

//            if (string.IsNullOrWhiteSpace(PARTNUMBER)) return false;

//            RegexOptions options = RegexOptions.IgnoreCase;

//            bool result = false;

//            //文字列最後尾が拡張子の様なら削除
//            string result0 = System.Text.RegularExpressions.Regex.Replace(PARTNUMBER, "(.IPT$|.IAM$|.IDW$|.DWG$|.DXF$)", "", options);
//            if (PARTNUMBER != result0)
//            {
//                WriteLine($"■ArcSuiteSupport.ConvertSanitaizedPartnumber(...) 置換しました{PARTNUMBER} -> {result0}");

//                outPartNumber = result0;
//                result = true;
//            }


//            // ArcSuiteの検索用ゼロ補完番号を生成
//            string result1 = ArcSuiteSupport.SanitaizingSimplificationStringForToyoDRAWINGNumber(result0, WriteLine);
//            if (result1 != result0)
//            {
//                outPartNumber = result1;
//                WriteLine($"■ArcSuiteSupport.ConvertSanitaizedPartnumber(...) 置換しました{result0} -> {outPartNumber}");
//                result = true;
//            }
//            else
//                outPartNumber = result0;


//            // 大文字化
//            result0 = outPartNumber.ToUpper();


//            // M-10201-001RL-R -> M-10201-001RL
//            string result2 = System.Text.RegularExpressions.Regex.Replace(result0, "(RL-R$|RL-L$)", "RL", options);
//            if (result0 != result2)
//            {
//                outPartNumber = result2;
//                WriteLine($"■ArcSuiteSupport.ConvertSanitaizedPartnumber(...) 置換しました{result0} -> {outPartNumber}");
//                result = true;
//            }
//            else
//                outPartNumber = result0;


//            // M-10201-001_1 -> 
//            string result3 = System.Text.RegularExpressions.Regex.Replace(result2, @"_\d$", "", options);
//            if (result2 != result3)
//            {
//                WriteLine($"■ArcSuiteSupport.ConvertSanitaizedPartnumber(...) 置換しました{result2} -> {outPartNumber}");
//                outPartNumber = result3;
//                result = true;
//            }
//            else
//            {
//                outPartNumber = result2;
//                WriteLine($"■ArcSuiteSupport.ConvertSanitaizedPartnumber(...) {result2} -> {outPartNumber}");
//            }

//            return result;
//        }

//        /// <summary>
//        /// Inventor向け
//        /// 文字列末尾がRLで終わっている場合-Rを追加します。最後に拡張子として.IPTを追加します。
//        /// 図面番号から想定されるパーツファイル名を組立てるために呼ばれます。
//        /// </summary>
//        /// <param name="inputPARTNUMBER"></param>
//        /// <returns></returns>
//        public static string ConvertToyoPARTNUMBERtoToyoPARTNUMBERcomponentName(string inputPARTNUMBER, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            if (WriteLine == null) WriteLine = Console.WriteLine;

//            NumberTypeConfig.DrawingType drawingType;
//            bool isVariant = true;
//            string suffixMIN = null;
//            string suffixMAX = null;
//            string TypeName;
//            if (ToyoDrawingTypeClassify.CheckNumber(inputPARTNUMBER, out drawingType, ref isVariant, ref suffixMIN, ref suffixMAX, out TypeName, WriteLine))
//            {
//                WriteLine($"■ArcSuiteSupport.ConvertToyoPARTNUMBERtoToyoPARTNUMBERcomponentName(...) 図面番号 {inputPARTNUMBER} は 東陽図面形式で {drawingType} と判断されました");


//                string zeroPaddingedPARTNUMBER;
//                bool result = ZeroPaddingPartNumber(inputPARTNUMBER, out zeroPaddingedPARTNUMBER, WriteLine);
//                if (result) WriteLine($"■ArcSuiteSupport.ConvertToyoPARTNUMBERtoToyoPARTNUMBERcomponentName(...) 変換されました {inputPARTNUMBER} -> {zeroPaddingedPARTNUMBER}"); else WriteLine($"変換されなかった {inputPARTNUMBER} -> {zeroPaddingedPARTNUMBER}");

//                // M-10201-001RL -> M-10201-001RL-R
//                string ToyoPARTNUMBERcomponetName = System.Text.RegularExpressions.Regex.Replace(zeroPaddingedPARTNUMBER, "(RL$)", "RL-R", RegexOptions.IgnoreCase);

//                return ToyoPARTNUMBERcomponetName;
//            }
//            else
//                return inputPARTNUMBER;
//        }

//        /// <summary>
//        /// ■入力文字列を大文字化した後、東陽の図番形式と想定される文字列のみを空白を削除したのち、全角を半角にする。（半角ｶﾅは使用しない）最後に、ハイフンで区切られた3項目目をゼロ保管処理）
//        /// </summary>
//        /// <param name="orgValue"></param>
//        /// <returns></returns>
//        public static string SanitaizingSimplificationStringForToyoDRAWINGNumber(string orgValue, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            if (WriteLine == null) WriteLine = Console.WriteLine;

//            orgValue = orgValue.ToUpper();

//            NumberTypeConfig.DrawingType drawingType;
//            bool isVariant = true;
//            string suffixMIN = null;
//            string suffixMAX = null;
//            string TypeName;
//            if (ToyoDrawingTypeClassify.CheckNumber(orgValue, out drawingType, ref isVariant, ref suffixMIN, ref suffixMAX, out TypeName))
//            {
//                WriteLine($"■ArcSuiteSupport.SanitaizingSimplificationStringForToyoDRAWINGNumber(...)図面番号【{orgValue}】は 東陽図面【{drawingType}】 と判断されました");

//                // 文字列中の半角スペース、全角スペースを削除、全角文字を半角に変更
//                orgValue.Replace(" ", "").Replace("　", "").Zen2HanANK_ZenSpace2HanSpace();

//                string sanitaizedValue;
//                bool result = ZeroPaddingPartNumber(orgValue, out sanitaizedValue, WriteLine:null);
//                if (result) WriteLine($"■ArcSuiteSupport.SanitaizingSimplificationStringForToyoDRAWINGNumber(...)変換されました {orgValue} -> {sanitaizedValue}"); else WriteLine($"変換されなかった {orgValue} -> {sanitaizedValue}");

//                return sanitaizedValue;
//            }
//            else
//            {
//                return orgValue;
//            }
//        }

//        /// <summary>
//        /// 東陽図番形式とされる入力文字列について、部品図・組図のみゼロ補完して返却
//        /// </summary>
//        /// <param name="inputString">@"^[A-Z][A-Z0-9]*-\d{5,6}-\d{1,3}[A-Z]?[A-Z]?$" "^[A-Z][A-Z0-9]*-\d{5,6}$" これらに該当する文字列のこと </param>
//        /// <param name="zeroPaddingToyoPARTNUMBER">ゼロ補完後の文字列</param>
//        /// <returns>ゼロ補完されたらtrue</returns>
//        public static bool ZeroPaddingPartNumber(string inputString, out string zeroPaddingToyoPARTNUMBER, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            if (WriteLine == null) WriteLine = Console.WriteLine;

//            // 文字列を'-'で分解
//            string[] splitWord = inputString.Split('-');
//            // いくつの単語に分かれたか調査
//            switch (splitWord.Length)
//            {
//                case 0:
//                case 1:
//                    zeroPaddingToyoPARTNUMBER = String.Join("-", splitWord);
//                    return false;
//                case 2:
//                    // M-10201,M-123446,
//                    // M-11123456R 等、ハイフン一つで構成されている文字列の処理
//                    WriteLine($"■ArcSuiteSupport.ZeroPaddingPartNumber(string)配列要素2コ={inputString}");
//                    if (splitWord[1] == "")
//                    {
//                        zeroPaddingToyoPARTNUMBER = String.Join("-", splitWord);
//                        return true;
//                    }
//                    // 1つ目のハイフンの次のトークンの最後が英数字の場合、-000を付加。
//                    // M-10201  -> M-10201-000
//                    // M-ABCDEFG3   ->  M-ABCDEFG3-000
//                    else if (char.IsNumber(splitWord[1], splitWord[1].Length - 1))
//                    {
//                        zeroPaddingToyoPARTNUMBER = inputString + "-000";
//                        return true;
//                    }
//                    // 1つ目のハイフンの次のトークンの最後がRLの場合、-000RLを付加
//                    // M-10201RL    ->  M-10201-000RL
//                    else if (Regex.IsMatch(inputString, @"[Rr][Ll]$"))
//                    {
//                        WriteLine($"■ArcSuiteSupport.ZeroPaddingPartNumber(string)1つめのハイフンの次のトークンの最後がRLかrlかRlかrLです={inputString}");
//                        zeroPaddingToyoPARTNUMBER = Regex.Replace(inputString, @"[Rr][Ll]$", "-000RL");
//                        return true;
//                    }
//                    // 1つ目のハイフンの次のトークンの最後がRの場合、-000Rを付加
//                    // M-10201R M-10201-000R
//                    else if (Regex.IsMatch(inputString, @"[Rr]$"))
//                    {
//                        WriteLine($"■ArcSuiteSupport.ZeroPaddingPartNumber(string)1つめのハイフンの次のトークンの最後がRかrです={inputString}");
//                        zeroPaddingToyoPARTNUMBER = Regex.Replace(inputString, @"[Rr]$", "-000R");
//                        return true;
//                    }
//                    // 1つ目のハイフンの次のトークンの最後がLの場合、-000Lを付加
//                    // M-10201L M-10201-000L
//                    else if (Regex.IsMatch(inputString, @"[Ll]$"))
//                    {
//                        WriteLine($"■ArcSuiteSupport.ZeroPaddingPartNumber(string)1つめのハイフンの次のトークンの最後がLかlです={inputString}");
//                        zeroPaddingToyoPARTNUMBER = Regex.Replace(inputString, @"[Ll]$", "-000L");
//                        return true;
//                    }
//                    // 該当なしの場合、分割した文字列を再結合
//                    else
//                    {
//                        WriteLine($"■ArcSuiteSupport.ZeroPaddingPartNumber(string)該当なし。そのまま再結合します={inputString}");
//                        zeroPaddingToyoPARTNUMBER = String.Join("-", splitWord);
//                        return true;
//                    }

//                case 3:
//                case 4:
//                    // 文字列中のハイフンの数が2個と3個はここで処理
//                    // M-10201-R    ->  M-10201-000R
//                    // M-10201-RL    ->  M-10201-000RL
//                    // M-10201-12RL    ->  M-10201-012RL
//                    // M-10201-1    ->  M-10201-001
//                    // M-10201-3R    ->  M-10201-003RL
//                    // M-10201-3RL-R    ->  M-10201-003RL-R
//                    WriteLine($"■ArcSuiteSupport.ZeroPaddingPartNumber({inputString})配列要素３こと４こ{inputString}");

//                    if (Regex.IsMatch(splitWord[2], @"^\d{1,2}(L|R|RL)?$"))
//                    {
//                        WriteLine($"3か４に分割された 2番目のデータで数字部が1～2桁R,L,RLの記号があるか、ない場合【{splitWord[2]}】");

//                        splitWord[2] = StringUtil.GetStringZeroPadding(splitWord[2], 3);
//                        zeroPaddingToyoPARTNUMBER = String.Join("-", splitWord);
//                    }
//                    else
//                    {
//                        zeroPaddingToyoPARTNUMBER = String.Join("-", splitWord);
//                    }

//                    return true;
//                default:
//                    zeroPaddingToyoPARTNUMBER = inputString;
//                    return false;
//            }
//        }

//        /// <summary>
//        /// 図面種類を問い合わせます
//        /// </summary>
//        /// <param name="inputNumberstr"></param>
//        /// <param name="drawingType"></param>
//        /// <returns></returns>
//        public static string GetDrawingTypeName(string inputNumberstr, out NumberTypeConfig.DrawingType drawingType)
//        {
//            string TypeName = null;

//            drawingType = NumberTypeConfig.DrawingType.Unknown;

//            if (string.IsNullOrWhiteSpace(inputNumberstr) == false)
//            {

//                // 図面番号が妥当かを確認する
//                bool isVariant = true;
//                string suffixMIN = null;
//                string suffixMAX = null;
//                DebugConsole.WriteLine($"■図面種類を問い合わせます・・・");

//                bool ans = ToyoDrawingTypeClassify.CheckNumber(inputNumberstr, out drawingType, ref isVariant, ref suffixMIN, ref suffixMAX, out TypeName);

//                if (ans == false)
//                {
//                    Eventlog.Log.WriteEntry("SasaLibNumberingSupport", System.Diagnostics.EventLogEntryType.Warning, 0, $"ArcSuiteSupport.GetDrawingTypeName()の戻り値がfalseでした inputNumberstr={inputNumberstr}, drawingType={drawingType}, isVariant={isVariant},suffixMIN={suffixMIN},suffixMAX={suffixMAX},TypeName={TypeName}");
//                }
//            }
//            return TypeName;
//        }

//        /// <summary>
//        /// ArcSuiteのsystem:status 属性値を整形する
//        /// </summary>
//        /// <param name="SystemStatus"></param>
//        /// <returns></returns>
//        public static string GetArcSuiteSystemStatusDisplayName(string SystemStatus)
//        {
//            string result;
//            switch (SystemStatus)
//            {
//                case "system:editable":
//                    result = "使用可能";
//                    break;
//                case "system:editing":
//                    result = "取換え中";
//                    break;
//                case "system:fixed":
//                    result = "固定";
//                    break;
//                case "system:obsolete":
//                    result = "廃棄";
//                    break;
//                case "user:disabled":
//                    result = "使用禁止";
//                    break;
//                case "user:deprecated":
//                    result = "非推奨";
//                    break;
//                case "user:default":
//                    result = "ﾃﾞﾌｫﾙﾄ";
//                    break;
//                default:
//                    result = "定義なし";
//                    break;
//            }

//            return result;
//        }

//        /// <summary>
//        /// アークスイートのsystem:createdOn 属性値を整形する
//        /// </summary>
//        /// <param name="system_createdon"></param>
//        /// <returns></returns>
//        public static DateTime GetSyssmteCreatedOnTime(string system_createdon)
//        {
//            if (string.IsNullOrWhiteSpace(system_createdon))
//                return DateTime.MinValue;

//            string strTime=null;
//            string format = null;
//            try
//            {
//                strTime = system_createdon.Replace("JST", "").Replace("T", " ");
//                format = "yyyy-MM-dd HH:mm:ss";
//                return DateTime.ParseExact(strTime, format, null);
//            }
//            catch (Exception ex)
//            {
//                Eventlog.Log.WriteEntry("SasaLibNumberingSupport", System.Diagnostics.EventLogEntryType.Error, 0, $"ArcSuiteSupport.GetSyssmteCreatedOnTime(\"{system_createdon}\") にて例外発生:{ex.Message} strTime = \"{strTime}\"  format = \"{format}\"");
//                return DateTime.MinValue;
//            }
//        }

//        //public static string GetCadTypeString(RemoteClientCADtype.CadType CadType)
//        //{
//        //    StringBuilder sb = new StringBuilder("");

//        //    string bitstring = Convert.ToString((int)CadType, 2).PadLeft(8, '0');

//        //    DebugConsole.WriteLine($"CadType = {bitstring}");

//        //    if (CadType.HasFlag(RemoteClientCADtype.CadType.AutoCAD2D))
//        //    {
//        //        sb.Append("AutoCAD2D ");
//        //    }
//        //    if (CadType.HasFlag(RemoteClientCADtype.CadType.SolidWorksModel))
//        //    {
//        //        sb.Append("SolidWorksModel ");
//        //    }
//        //    if (CadType.HasFlag(RemoteClientCADtype.CadType.SolidWorksDraw))
//        //    {
//        //        sb.Append("SolidWorksDraw ");
//        //    }
//        //    if (CadType.HasFlag(RemoteClientCADtype.CadType.InventorModel))
//        //    {
//        //        sb.Append("InventorModel ");
//        //    }
//        //    if (CadType.HasFlag(RemoteClientCADtype.CadType.InventorDraw))
//        //    {
//        //        sb.Append("InventorDraw ");
//        //    }
//        //    return sb.ToString();
//        //}


//        /// <summary>
//        /// RemoteClientCADtype.CadType CadType を 文字列に変換。スペースで区切る
//        /// </summary>
//        /// <param name="CadType"></param>
//        /// <returns></returns>
//        public static string GetCadTypeString(RemoteClientCADtype.CadType CadType)
//        {
//            string bitstring = Convert.ToString((int)CadType, 2).PadLeft(8, '0');

//            List<string> cadTypes = new List<string>();

//            DebugConsole.WriteLine($"CadType = {bitstring}");

//            if (CadType.HasFlag(RemoteClientCADtype.CadType.AutoCAD2D))
//            {
//                cadTypes.Add(RemoteClientCADtype.CadType.AutoCAD2D.ToString());
//            }
//            if (CadType.HasFlag(RemoteClientCADtype.CadType.SolidWorksModel))
//            {
//                cadTypes.Add(RemoteClientCADtype.CadType.SolidWorksModel.ToString());
//            }
//            if (CadType.HasFlag(RemoteClientCADtype.CadType.SolidWorksDraw))
//            {
//                cadTypes.Add(RemoteClientCADtype.CadType.SolidWorksDraw.ToString());
//            }
//            if (CadType.HasFlag(RemoteClientCADtype.CadType.InventorModel))
//            {
//                cadTypes.Add(RemoteClientCADtype.CadType.InventorModel.ToString());
//            }
//            if (CadType.HasFlag(RemoteClientCADtype.CadType.InventorDraw))
//            {
//                cadTypes.Add(RemoteClientCADtype.CadType.InventorDraw.ToString());
//            }
//            if (CadType.HasFlag(RemoteClientCADtype.CadType.undefined_1))
//            {
//                cadTypes.Add(RemoteClientCADtype.CadType.undefined_1.ToString());
//            }
//            if (CadType.HasFlag(RemoteClientCADtype.CadType.undefined_2))
//            {
//                cadTypes.Add(RemoteClientCADtype.CadType.undefined_2.ToString());
//            }
//            if (CadType.HasFlag(RemoteClientCADtype.CadType.undefined_3))
//            {
//                cadTypes.Add(RemoteClientCADtype.CadType.undefined_3.ToString());
//            }

//            return string.Join(" ",cadTypes);
//        }

//        /// <summary>
//        /// アークスイートのuser:modelcreationonorder 属性値 を成型する
//        /// </summary>
//        /// <param name="user_modelcreationonorder"></param>
//        /// <returns></returns>
//        public static string GetUserMoodelcreationonorderDiplayName(string user_modelcreationonorder)
//        {
//            if (string.IsNullOrWhiteSpace(user_modelcreationonorder) == true)
//                return $"3D作成発注中: 値はありません";
//            else
//                return $"3D作成発注中:『 {user_modelcreationonorder}』";
//        }

//    }
//}
