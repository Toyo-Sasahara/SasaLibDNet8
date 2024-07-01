//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Runtime.Versioning;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Xml.Serialization;

//namespace SasaLib.NumberingSupport
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public static class PurchasingManufacturerCodeConfigWork
//    {
//        /// <summary>
//        /// 材質コード設定ﾌｧｲﾙの読込とオブジェクト生成
//        /// </summary>
//        /// <param name="purchasingManufacturerCodeConfigFullPath"></param>
//        /// <param name="remake"></param>
//        /// <param name="version"></param>
//        /// <param name="LogWrite"></param>
//        /// <returns></returns>
//        public static bool PreparationConfigData(string purchasingManufacturerCodeConfigFullPath, bool remake = false, double version = 1.4d, SasaLibDelegateWriteLine LogWrite = null)
//        {
//            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

//            PurchasingManufacturerCodeConfig.Config = new PurchasingManufacturerCodeConfig();

//            bool removeresult;
//            if (remake)
//            {
//                removeresult = FileFolder.RemoveFile(purchasingManufacturerCodeConfigFullPath);
//            }

//            System.IO.StreamReader sr = null;

//            try
//            {
//                // 既に設定ファイルがある場合はあらかじめデシリアライズする
//                if (FileFolder.FileExists(purchasingManufacturerCodeConfigFullPath) == true)
//                {
//                    // 保存した設定ファイル内容を復元する
//                    XmlSerializer serializer = new XmlSerializer(typeof(PurchasingManufacturerCodeConfig));
//                    sr = new System.IO.StreamReader(purchasingManufacturerCodeConfigFullPath, new System.Text.UTF8Encoding(false));
//                    PurchasingManufacturerCodeConfig.Config = (PurchasingManufacturerCodeConfig)serializer.Deserialize(sr);
//                    sr.Close();

//                }
//                else
//                {
//                    LogWrite($"■ﾌｧｲﾙ {purchasingManufacturerCodeConfigFullPath} は見つかりませんでした。");


//                    Create(purchasingManufacturerCodeConfigFullPath, version, LogWrite);
//                }

//            }
//            catch (Exception ex)
//            {
//                LogWrite($"※PurchasingManufacturerCodeConfigWork.PreparationConfigData(...)で例外発生 {ex.Message} {ex.InnerException} ");
//                Eventlog.Log.WriteEntry("SasaLibNumberingSupport", EventLogEntryType.Error, 0, $"※PurchasingManufacturerCodeConfigWork.PreparationConfigData(...)で例外発生 設定ﾌｧｲﾙ{purchasingManufacturerCodeConfigFullPath}, 例外情報:{ex.Message} {ex.InnerException}");
//                sr.Close();

//                PurchasingManufacturerCodeConfig.Config = new PurchasingManufacturerCodeConfig();

//                return false;
//            }
//            return true;
//        }

//        static void Create(string purchasingManufacturerCodeConfigFullPath, double version, SasaLibDelegateWriteLine LogWrite = null)
//        {
//            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

//            // 足りない設定があれば追加する                
//            if (PurchasingManufacturerCodeConfig.Config.VersionNumber <= 1.0d)
//                PurchasingManufacturerCodeConfig.Config.VersionNumber = 1.0d;

//            // 足りない設定があれば追加する                
//            if (string.IsNullOrWhiteSpace(PurchasingManufacturerCodeConfig.Config.VersionComment))
//            {
//                PurchasingManufacturerCodeConfig.Config.VersionComment = "Ver 1.0d 2021-12-13 プログラム内蔵初期バージョン";
//            }



//            PurchasingManufacturerCodeConfig.Config.PurchasingManufacturerDatas = new List<PurchasingManufacturerCodeConfig.PurchasingManufacturer> {

//                    new PurchasingManufacturerCodeConfig.PurchasingManufacturer {
//                        CODE = "11" ,
//                        NAME = "FC***" ,
//                        Pattern = @"^FC\d{3}"
//                    },

//                };

//            //
//            XmlSerializer serializer = new XmlSerializer(typeof(PurchasingManufacturerCodeConfig));
//            using (StreamWriter sw = new StreamWriter(purchasingManufacturerCodeConfigFullPath, false, Encoding.UTF8))
//            {
//                serializer.Serialize(sw, PurchasingManufacturerCodeConfig.Config);
//                if (LogWrite != null) LogWrite($"設定ファイル {purchasingManufacturerCodeConfigFullPath} を更新しました");
//            }
//        }

//        public static bool SaveConfig(string purchasingManufacturerCodeConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
//        {
//            bool result = false;
//            XmlSerializer serializer = new XmlSerializer(typeof(PurchasingManufacturerCodeConfig));
//            using (StreamWriter sw = new StreamWriter(purchasingManufacturerCodeConfigFullPath, false, Encoding.UTF8))
//            {
//                try
//                {
//                    serializer.Serialize(sw, PurchasingManufacturerCodeConfig.Config);
//                    if (LogWrite != null) LogWrite($"設定ファイル {purchasingManufacturerCodeConfigFullPath} を更新しました");
//                    result = true;
//                }
//                catch (IOException ioe)
//                {
//                    if (LogWrite != null) LogWrite($"設定ファイル {purchasingManufacturerCodeConfigFullPath} の更新で例外発生{ioe.Message}");
//                    result = false;
//                }
//            }
//            return result;
//        }

//        /// <summary>
//        ///  正規表現にて検索。最初にヒットした答えを返す。みつからない場合null
//        /// </summary>
//        /// <param name="NAME"></param>
//        /// <param name="WriteLine"></param>
//        /// <returns></returns>
//        public static string GetMakerCode(string NAME, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            if (WriteLine == null) WriteLine = Console.WriteLine;

//            NAME = StringUtil.Zen2Han(NAME);

//            string CODE = null;
//            foreach (var PurchasingManufacturer in PurchasingManufacturerCodeConfig.Config.PurchasingManufacturerDatas)
//            {
//                string pattern = StringUtil.Zen2Han(PurchasingManufacturer.Pattern);

//                bool result = Regex.IsMatch(NAME, pattern, RegexOptions.IgnoreCase);
//                if (result == true)
//                {
//                    WriteLine($"購入品会社名 {NAME} がパターンに合致.{PurchasingManufacturer.Pattern} -> {PurchasingManufacturer.NAME} , {PurchasingManufacturer.CODE}");
//                    CODE = PurchasingManufacturer.CODE;
//                    break;
//                }
//            }
//            return CODE;
//        }
//    }
//}
