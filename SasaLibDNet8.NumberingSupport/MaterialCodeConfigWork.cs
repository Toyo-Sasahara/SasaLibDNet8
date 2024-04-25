using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using static SasaLib.NumberingSupport.MaterialCodeConfig;

namespace SasaLib.NumberingSupport
{
    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class MaterialCodeConfigWork
    {
        /// <summary>
        /// 材質コード設定ﾌｧｲﾙの読込とオブジェクト生成
        /// </summary>
        /// <param name="materialCodeConfigFullPath"></param>
        /// <param name="remake"></param>
        /// <param name="version"></param>
        /// <param name="LogWrite"></param>
        /// <returns></returns>
        public static bool PreparationConfigData(string materialCodeConfigFullPath, bool remake = false, double version = 1.4d, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            bool removeresult;
            if (remake)
            {
                removeresult = FileFolder.RemoveFile(materialCodeConfigFullPath);
            }

            System.IO.StreamReader sr = null;

            try
            {
                // 既に設定ファイルがある場合はあらかじめデシリアライズする
                if (FileFolder.FileExists(materialCodeConfigFullPath) == true)
                {
                    // 保存した設定ファイル内容を復元する
                    XmlSerializer serializer = new XmlSerializer(typeof(MaterialCodeConfig));
                    sr = new System.IO.StreamReader(materialCodeConfigFullPath, new System.Text.UTF8Encoding(false));
                    MaterialCodeConfig.Config = (MaterialCodeConfig)serializer.Deserialize(sr);
                    sr.Close();

                    if (MaterialCodeConfig.Config.VERSION < version)
                    {
                        LogWrite($"■ﾌｧｲﾙ {materialCodeConfigFullPath} のVERSIONは {MaterialCodeConfig.Config.VERSION} であり指定ﾊﾞｰｼﾞｮﾝ{version}より古いです。再構築します");

                        bool sucess = PreparationConfigData(materialCodeConfigFullPath, remake = true, version, LogWrite);

                        if (sucess == false)
                        {
                            LogWrite($"■ﾌｧｲﾙ {materialCodeConfigFullPath}を正常に読み込んでいません。");

                            return false;
                        }
                        else
                            return true;
                    }
                }
                else
                {
                    LogWrite($"■ﾌｧｲﾙ {materialCodeConfigFullPath} は見つかりませんでした。");
                    MaterialCodeConfig.Config = new MaterialCodeConfig();
                    Create(materialCodeConfigFullPath, version, LogWrite);
                }

            }
            catch (Exception ex)
            {
                LogWrite($"※MaterialCodeConfigWork.PreparationConfigData(...)で例外発生 {ex.Message} {ex.InnerException} ");
                Eventlog.Log.WriteEntry("SasaLibNumberingSupport", EventLogEntryType.Error, 0, $"※MaterialCodeConfigWork.PreparationConfigData(...)で例外発生 設定ﾌｧｲﾙ{materialCodeConfigFullPath}, 例外情報:{ex.Message} {ex.InnerException}");
                sr.Close();

                return false;
            }
            return true;
        }

        static void Create(string materialCodeConfigFullPath, double version, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            // 足りない設定があれば追加する                
            if (MaterialCodeConfig.Config.VersionNumber <= 1.0d)
                MaterialCodeConfig.Config.VersionNumber = 1.0d;

            // 足りない設定があれば追加する                
            if (string.IsNullOrWhiteSpace(MaterialCodeConfig.Config.VersionComment))
            {
                MaterialCodeConfig.Config.VersionComment = "Ver 1.0d 2021-12-13 プログラム内蔵初期バージョン";
            }



            MaterialCodeConfig.Config.MaterialCodeDatas = new List<MaterialCodeConfig.MaterialCodeData> {

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "11" ,MATERIAL = "FC***" ,Pattern = @"^FC\d{3}" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "12" ,MATERIAL = "FCD***" ,Pattern = @"^FCD\d{3}" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "21" ,MATERIAL = "SPCC" ,Pattern = @"^SSPC." },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "22" ,MATERIAL = "SS400" ,Pattern = @"^SS\d{3}" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "23" ,MATERIAL = "S**C" ,Pattern = @"^SPCC" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "24" ,MATERIAL = "STK**" ,Pattern = @"^STK\d{3}" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "25" ,MATERIAL = "SK-*" ,Pattern = @"^SK-.*" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "26" ,MATERIAL = "SKS-*" ,Pattern = @"^SKS-.*" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "27" ,MATERIAL = "SKD-*" ,Pattern = @"^SKD-.*" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "28" ,MATERIAL = "SKH-*" ,Pattern = @"^SKH-.*" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "29" ,MATERIAL = "SUJ2" ,Pattern = @"^SUJ2" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "2A" ,MATERIAL = "SCM***" ,Pattern = @"^SCM.*" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "31" ,MATERIAL = "SUS303" ,Pattern = @"^SUS303" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "32" ,MATERIAL = "SUS304" ,Pattern = @"^SUS304" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "33" ,MATERIAL = "SUS316" ,Pattern = @"^SUS316" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "34" ,MATERIAL = "SUS430" ,Pattern = @"^SUS430" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "35" ,MATERIAL = "SUS440C" ,Pattern = @"^SUS440C" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "36" ,MATERIAL = "SUS420J2" ,Pattern = @"^SUS420J2" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "37" ,MATERIAL = "SAF2205" ,Pattern = @"^SAF2205" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "41" ,MATERIAL = "AC*A" ,Pattern = @"^AC.A" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "42" ,MATERIAL = "A2017" ,Pattern = @"^A2017" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "43" ,MATERIAL = "A5052" ,Pattern = @"^A5052" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "44" ,MATERIAL = "A6061" ,Pattern = @"^A6061" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "45" ,MATERIAL = "A6063" ,Pattern = @"^A6063" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "46" ,MATERIAL = "A7075" ,Pattern = @"^A7075" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "47" ,MATERIAL = "A2219" ,Pattern = @"^A2219" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "48" ,MATERIAL = "A5083" ,Pattern = @"^A5083" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "49" ,MATERIAL = "A2024" ,Pattern = @"^A2024" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "51" ,MATERIAL = "SWP-*" ,Pattern = @"^SWP-.*" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "52" ,MATERIAL = "SUP*" ,Pattern = @"^SUP.*" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "53" ,MATERIAL = "SWO" ,Pattern = @"^SWO" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "54" ,MATERIAL = "SK-5" ,Pattern = @"^SK-5" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "55" ,MATERIAL = "リボン鋼" ,Pattern = @"^リボン鋼" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "56" ,MATERIAL = "ベイナイト鋼" ,Pattern = @"^ベイナイト鋼" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "5A" ,MATERIAL = "SUS304-WPB" ,Pattern = @"^SUS304" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "5B" ,MATERIAL = "SUS304-CSP" ,Pattern = @"^SUS304-CSP" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "5C" ,MATERIAL = "SUS301-CSP" ,Pattern = @"^SUS301-CSP" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "61" ,MATERIAL = "CAC406" ,Pattern = @"^CAC406" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "62" ,MATERIAL = "C2801" ,Pattern = @"^C2801" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "63" ,MATERIAL = "C3604" ,Pattern = @"^C3604" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "64" ,MATERIAL = "C5191" ,Pattern = @"^C5191" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "71" ,MATERIAL = "ポリアセタール" ,Pattern = @"^ポリアセタール" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "72" ,MATERIAL = "ポリアミド" ,Pattern = @"^ポリアミド" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "73" ,MATERIAL = "PTFE" ,Pattern = @"^PTFE" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "74" ,MATERIAL = "UHMWPE" ,Pattern = @"^フェノール樹脂" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "76" ,MATERIAL = "断熱材" ,Pattern = @"^断熱材" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "77" ,MATERIAL = "エチレンビニルアルコール" ,Pattern = @"^エチレンビニルアルコール" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "78" ,MATERIAL = "PEEK" ,Pattern = @"^PEEK" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "81" ,MATERIAL = "塩化ビニル" ,Pattern = @"^塩化ビニル" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "82" ,MATERIAL = "アクリル" ,Pattern = @"^アクリル" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "83" ,MATERIAL = "ポリカーボネート" ,Pattern = @"^ポリカーボネート" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "84" ,MATERIAL = "高衝撃塩化ビニル" ,Pattern = @"^高衝撃塩化ビニル" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "85" ,MATERIAL = "PET" ,Pattern = @"^PET" },

                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "91" ,MATERIAL = "スポンジ" ,Pattern = @"^スポンジ" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "92" ,MATERIAL = "天然ゴム" ,Pattern = @"^天然ゴム" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "94" ,MATERIAL = "シリコンゴム" ,Pattern = @"^シリコンゴム" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "95" ,MATERIAL = "ウレタンゴム" ,Pattern = @"^ウレタンゴム" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "96" ,MATERIAL = "ニトリルゴム" ,Pattern = @"^ニトリルゴム" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "97" ,MATERIAL = "ネオプレン" ,Pattern = @"^ネオプレン" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "98" ,MATERIAL = "モルトプレン" ,Pattern = @"^モルトプレン" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "99" ,MATERIAL = "アメゴム" ,Pattern = @"^アメゴム" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "Z1" ,MATERIAL = "フェルト" ,Pattern = @"^フェルト" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "Z2" ,MATERIAL = "レザー" ,Pattern = @"^レザー" },
                    new MaterialCodeConfig.MaterialCodeData { MATERIALCODE = "A1" ,MATERIAL = "チタン" ,Pattern = @"^チタン" },
                };

            //
            XmlSerializer serializer = new XmlSerializer(typeof(MaterialCodeConfig));
            using (StreamWriter sw = new StreamWriter(materialCodeConfigFullPath, false, Encoding.UTF8))
            {
                serializer.Serialize(sw, MaterialCodeConfig.Config);
                if (LogWrite != null) LogWrite($"設定ファイル {materialCodeConfigFullPath} を更新しました");
            }
        }

        public static bool SaveConfig(string materialCodeConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
        {
            bool result = false;
            XmlSerializer serializer = new XmlSerializer(typeof(MaterialCodeConfig));
            using (StreamWriter sw = new StreamWriter(materialCodeConfigFullPath, false, Encoding.UTF8))
            {
                try
                {
                    serializer.Serialize(sw, MaterialCodeConfig.Config);
                    if (LogWrite != null) LogWrite($"設定ファイル {materialCodeConfigFullPath} を更新しました");
                    result = true;
                }
                catch (IOException ioe)
                {
                    if (LogWrite != null) LogWrite($"設定ファイル {materialCodeConfigFullPath} の更新で例外発生{ioe.Message}");
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// AMATERIALNAMEから材質コードを調査。MaterialCodeConfig.Config.MaterialCodeDatas から 正規表現にて検索。最初にヒットした答えを返す。みつからない場合null
        /// </summary>
        /// <param name="MATERIALNAME"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        public static string GetMaterialCode(string MATERIALNAME, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            List<MaterialCodeData> matchMaterialCodeDatas = new List<MaterialCodeData>();

            MATERIALNAME = StringUtil.Zen2Han(MATERIALNAME);

            string MATERIALCODE = null;

            foreach (var MaterialCodeData in MaterialCodeConfig.Config.MaterialCodeDatas)
            {
                string pattern = StringUtil.Zen2Han(MaterialCodeData.Pattern);

                bool result = Regex.IsMatch(MATERIALNAME, pattern, RegexOptions.IgnoreCase);
                if (result == true)
                {
                    delegateWriteLine($"材料名{MATERIALNAME}がパターンに合致.{MaterialCodeData.Pattern} -> {MaterialCodeData.MATERIAL} , {MaterialCodeData.MATERIALCODE}");

                    matchMaterialCodeDatas.Add(MaterialCodeData);

                    MATERIALCODE = MaterialCodeData.MATERIALCODE;
                }
            }
            if (matchMaterialCodeDatas.Count == 1)
            {
                return matchMaterialCodeDatas[0].MATERIALCODE;

            }
            else if (matchMaterialCodeDatas.Count > 1)
            {
                var recoveryTarget = string.Join(" , ", matchMaterialCodeDatas.ConvertAll(item => item.Pattern).ToList());

                delegateWriteLine($"材料名{MATERIALNAME}は複数のパターンにマッチしています。データファイルの修正が必要です。修正対象{recoveryTarget}");

                return null;
            }
            else
            {
                return null;
            }

        }
    }
}
