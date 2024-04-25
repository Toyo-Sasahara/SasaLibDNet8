using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml.Serialization;

namespace SasaLib.NumberingSupport
{
    [SupportedOSPlatform("windows")]
    public class ConversionFormulaNumberConfigWork
    {
        /// <summary>
        /// 設定を準備する。デシアライズ可能な場合は最初に実行。次に足りない設定をデフォルト値で埋める。
        /// 設定ファイルが存在しない場合はデフォルト値で新規作成する。
        /// </summary>
        /// <returns></returns>
        public static bool PreparationConfigData(string SuffixZeroPadVariantNumberTypeConfigFullPath , bool remake = false, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            bool removeresult;
            if (remake)
            {
                removeresult = FileFolder.RemoveFile(SuffixZeroPadVariantNumberTypeConfigFullPath);
            }

            System.IO.StreamReader sr = null;

            try
            {
                // 既に設定ファイルがある場合はあらかじめデシリアライズする
                if (FileFolder.FileExists(SuffixZeroPadVariantNumberTypeConfigFullPath) == true)
                {
                    // 保存した設定ファイル内容を復元する
                    XmlSerializer serializer = new XmlSerializer(typeof(ConversionFormulaNumberConfig));
                    sr = new System.IO.StreamReader(SuffixZeroPadVariantNumberTypeConfigFullPath, new System.Text.UTF8Encoding(false));
                    ConversionFormulaNumberConfig.Config = (ConversionFormulaNumberConfig)serializer.Deserialize(sr);
                    sr.Close();

                    SaveConfig(SuffixZeroPadVariantNumberTypeConfigFullPath);
                }
                else
                {
                    ConversionFormulaNumberConfig.Config = new ConversionFormulaNumberConfig();
                    LogWrite($"※ﾌｧｲﾙ {SuffixZeroPadVariantNumberTypeConfigFullPath} は見つかりませんでした。");

                    // 現状の設定から足りない情報を追加してシリアライズ
                    Remake(SuffixZeroPadVariantNumberTypeConfigFullPath, LogWrite);
                }

            }
            catch (Exception ex)
            {
                LogWrite($"※ConversionFormulaNumberConfigWork.PreparationConfigData(...)で例外発生 {ex.Message} {ex.InnerException} ");
                Eventlog.Log.WriteEntry("SasaLibNumberingSupport", EventLogEntryType.Error, 0, $"※ConversionFormulaNumberConfigWork.PreparationConfigData(...)で例外発生 {ex.Message} {ex.InnerException}  設定ﾌｧｲﾙ{SuffixZeroPadVariantNumberTypeConfigFullPath}, 例外情報:{ex.Message} {ex.InnerException}");
                sr.Close();

                return false;
            }
            return true;
        }

        /// <summary>
        /// 足りない設定はデフォルト値で埋めて設定ファイルを更新する
        /// </summary>
        /// <param name="SuffixZeroPadVariantNumberTypeConfigFullPath"></param>
        public static void Remake(string SuffixZeroPadVariantNumberTypeConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
        {
            bool updated = false;

            //// 足りない設定があれば追加する                
            //ConversionFormulaNumberConfig.Config.VERSION = 1.0d;

            // 足りない設定があれば追加する                
            if (ConversionFormulaNumberConfig.Config.VersionNumber <= 1.0d)
                ConversionFormulaNumberConfig.Config.VersionNumber = 1.0d;

            // 足りない設定があれば追加する                
            if (string.IsNullOrWhiteSpace(ConversionFormulaNumberConfig.Config.VersionComment))
            {
                ConversionFormulaNumberConfig.Config.VersionComment = "Ver 1.0d 2021-12-13 プログラム内蔵初期バージョン";
                updated = true;
            }


            ConversionFormulaNumberConfig.Config.COMMNENT01 = "Pattern は正規表現のパターン値、Replacement は置換文字列";
            ConversionFormulaNumberConfig.Config.COMMNENT02 = "";
            ConversionFormulaNumberConfig.Config.COMMNENT03 = "";

            if (ConversionFormulaNumberConfig.Config.ConversionFormulas == null || (ConversionFormulaNumberConfig.Config.ConversionFormulas.Count() == 0))
            {
                ConversionFormulaNumberConfig.Config.ConversionFormulas = new List<ConversionFormula>
                {
                    new ConversionFormula {Comment = "ガイドロール" , Pattern = @"^TS-12802-\d{1,3}$",   Replacement = "TS-12802-000" },
                    new ConversionFormula {Comment = "ガイドロール" , Pattern = @"^TS-12801-\d{1,3}$",   Replacement = "TS-12801-000" },
                    new ConversionFormula {Comment = "スクリュウ" , Pattern = @"^TS-12720-\d{1,3}$",   Replacement = "TS-12720-000" },
                    new ConversionFormula {Comment = "スクリュウ" , Pattern = @"^TS-12712-\d{1,3}$",   Replacement = "TS-12712-000" },
                    new ConversionFormula {Comment = "スクリュウ" , Pattern = @"^TS-12710-\d{1,3}$",   Replacement = "TS-12710-000" },
                    new ConversionFormula {Comment = "スクリュウ" , Pattern = @"^TS-12708-\d{1,3}$" ,   Replacement = "TS-12708-000" },
                    new ConversionFormula {Comment = "スクリュウ" , Pattern = @"^TS-12706-\d{1,3}$" ,   Replacement = "TS-12706-000" },
                    new ConversionFormula {Comment = "スクリュウ" , Pattern = @"^TS-12705-\d{1,3}$" ,   Replacement = "TS-12705-000" },
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12416-\d{1,3}U$",   Replacement = "TS-12416-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12416-\d{1,3}S$",   Replacement = "TS-12416-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12412-\d{1,3}U$",   Replacement = "TS-12412-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12412-\d{1,3}S$",   Replacement = "TS-12412-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12410-\d{1,3}U$",   Replacement = "TS-12410-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12410-\d{1,3}S$",   Replacement = "TS-12410-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12408-\d{1,3}U$",   Replacement = "TS-12408-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12408-\d{1,3}S$",   Replacement = "TS-12408-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12406-\d{1,3}U$",   Replacement = "TS-12406-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-12406-\d{1,3}S$",   Replacement = "TS-12406-000S"},
                    new ConversionFormula {Comment = "スタット" , Pattern = @"^TS-12301-\d{1,3}$" ,   Replacement = "TS-12301-000" },
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12250-\d{1,3}U$",   Replacement = "TS-12250-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12250-\d{1,3}S$",   Replacement = "TS-12250-000S"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12245-\d{1,3}U$",   Replacement = "TS-12245-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12245-\d{1,3}S$",   Replacement = "TS-12245-000S"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12240-\d{1,3}U$",   Replacement = "TS-12240-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12240-\d{1,3}S$",   Replacement = "TS-12240-000S"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12240-\d{1,3}B$",   Replacement = "TS-12240-000B"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12235-\d{1,3}U$",   Replacement = "TS-12235-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12235-\d{1,3}S$",   Replacement = "TS-12235-000S"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12235-\d{1,3}B$",   Replacement = "TS-12235-000B"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12230-\d{1,3}U$",   Replacement = "TS-12230-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12230-\d{1,3}S$",   Replacement = "TS-12230-000S"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12230-\d{1,3}B$",   Replacement = "TS-12230-000B"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12225-\d{1,3}U$",   Replacement = "TS-12225-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12225-\d{1,3}S$",   Replacement = "TS-12225-000S"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12225-\d{1,3}B$",   Replacement = "TS-12225-000B"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12220-\d{1,3}U$",   Replacement = "TS-12220-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12220-\d{1,3}S$",   Replacement = "TS-12220-000S"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12220-\d{1,3}B$",   Replacement = "TS-12220-000B"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12217-\d{1,3}U$",   Replacement = "TS-12217-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12216-\d{1,3}U$",   Replacement = "TS-12216-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12215-\d{1,3}U$",   Replacement = "TS-12215-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12215-\d{1,3}B$",   Replacement = "TS-12215-000B"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12214-\d{1,3}U$",   Replacement = "TS-12214-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12212-\d{1,3}U$",   Replacement = "TS-12212-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12210-\d{1,3}U$",   Replacement = "TS-12210-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12209-\d{1,3}U$",   Replacement = "TS-12209-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12208-\d{1,3}U$",   Replacement = "TS-12208-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12206-\d{1,3}U$",   Replacement = "TS-12206-000U"},
                    new ConversionFormula {Comment = "スペーサー" , Pattern = @"^TS-12205-\d{1,3}U$",   Replacement = "TS-12205-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10747-\d{1,3}U$",   Replacement = "TS-10747-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10747-\d{1,3}S$",   Replacement = "TS-10747-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10746-\d{1,3}U$",   Replacement = "TS-10746-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10746-\d{1,3}S$",   Replacement = "TS-10746-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10745-\d{1,3}U$",   Replacement = "TS-10745-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10745-\d{1,3}S$",   Replacement = "TS-10745-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10744-\d{1,3}U$",   Replacement = "TS-10744-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10744-\d{1,3}S$",   Replacement = "TS-10744-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10743-\d{1,3}U$",   Replacement = "TS-10743-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10743-\d{1,3}S$",   Replacement = "TS-10743-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10742-\d{1,3}U$",   Replacement = "TS-10742-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10742-\d{1,3}S$",   Replacement = "TS-10742-000S"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10741-\d{1,3}U$",   Replacement = "TS-10741-000U"},
                    new ConversionFormula {Comment = "ロッドスクリュウ" , Pattern = @"^TS-10741-\d{1,3}S$",   Replacement = "TS-10741-000S"},
                    new ConversionFormula {Comment = "クランクロッド" , Pattern = @"^TS-10740-\d{1,3}$" ,   Replacement = "TS-10740-000" },
                    new ConversionFormula {Comment = "受け皿" , Pattern = @"^PT-\d{6}-\d{2}$" ,   Replacement = "PT-" },
                    new ConversionFormula {Comment = "樹脂カバー" , Pattern = @"^RC-\d{8}-\d.$" ,   Replacement = "RC-" },
                    new ConversionFormula {Comment = "フィルムストッパー" , Pattern = @"^RS-\d{7}-.$" ,   Replacement = "RS-" },
                    new ConversionFormula {Comment = "側板" , Pattern = @"^SB-\d{6}$" ,   Replacement = "SB-" },
                    new ConversionFormula {Comment = "側板" , Pattern = @"^SC-\d{6}-J$" ,   Replacement = "SC-" },
                    new ConversionFormula {Comment = "側板" , Pattern = @"^SC-\d{6}-U$" ,   Replacement = "SC-U" },
                    new ConversionFormula {Comment = "シャフト" , Pattern = @"^SF-\d{6}-(S|U|C|CF|J|JF)$" ,   Replacement = "SF-" },
                    new ConversionFormula {Comment = "シャフト" , Pattern = @"^SL-\d{6}-S$" ,   Replacement = "SL-" },
                    new ConversionFormula {Comment = "両端タップ付きシャフト" , Pattern = @"^ST-\d{6}-\d{1,3}(S|U|C|CF|J|JF)$" ,   Replacement = "ST-" },
                    new ConversionFormula {Comment = "テンションロールブラケット" , Pattern = @"^TB-\d{5}-(S|U)$" ,   Replacement = "TB-" },
                };                                                  
                updated = true;                                     
            }                                                       
                                                                    
            //                                                      
            if (updated)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConversionFormulaNumberConfig));
                using (StreamWriter sw = new StreamWriter(SuffixZeroPadVariantNumberTypeConfigFullPath, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, ConversionFormulaNumberConfig.Config);
                    if (LogWrite != null) LogWrite($"■設定ファイル {SuffixZeroPadVariantNumberTypeConfigFullPath} を更新しました");
                }
            }

        }

        /// <summary>
        /// NumberTypeConfig.XMLへ設定変数を描き戻す
        /// </summary>
        public static bool SaveConfig(string SuffixZeroPadVariantNumberTypeConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
        {
            bool result = false;
            XmlSerializer serializer = new XmlSerializer(typeof(ConversionFormulaNumberConfig));
            using (StreamWriter sw = new StreamWriter(SuffixZeroPadVariantNumberTypeConfigFullPath, false, Encoding.UTF8))
            {
                try
                {
                    serializer.Serialize(sw, ConversionFormulaNumberConfig.Config);
                    if (LogWrite != null) LogWrite($"■設定ファイル {SuffixZeroPadVariantNumberTypeConfigFullPath} を更新しました");
                    result = true;
                }
                catch (IOException ioe)
                {
                    if (LogWrite != null) LogWrite($"※設定ファイル {SuffixZeroPadVariantNumberTypeConfigFullPath} の更新で例外発生{ioe.Message}");
                    result = false;
                }
            }
            return result;
        }
    }
}
