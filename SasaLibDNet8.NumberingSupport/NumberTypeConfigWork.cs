using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SasaLib.NumberingSupport
{
    [SupportedOSPlatform("windows")]
    public class NumberTypeConfigWork
    {

        /// <summary>
        /// 設定を準備する。デシアライズ可能な場合は最初に実行。次に足りない設定をデフォルト値で埋める。
        /// 設定ファイルが存在しない場合はデフォルト値で新規作成する。
        /// </summary>
        /// <returns></returns>
        public static bool PreparationConfigData(string numberTypeConfigFullPath , bool remake = false, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            bool removeresult;
            if (remake)
            {
                removeresult = FileFolder.RemoveFile(numberTypeConfigFullPath);
            }

            System.IO.StreamReader sr = null;

            try
            {
                // 既に設定ファイルがある場合はあらかじめデシリアライズする
                if (FileFolder.FileExists(numberTypeConfigFullPath) == true)
                {
                    // 保存した設定ファイル内容を復元する
                    XmlSerializer serializer = new XmlSerializer(typeof(NumberTypeConfig));
                    sr = new System.IO.StreamReader(numberTypeConfigFullPath, new System.Text.UTF8Encoding(false));
                    NumberTypeConfig.Config = (NumberTypeConfig)serializer.Deserialize(sr);
                    sr.Close();
                }
                else
                {
                    NumberTypeConfig.Config = new NumberTypeConfig();
                    LogWrite($"※ﾌｧｲﾙ {numberTypeConfigFullPath} は見つかりませんでした。");

                    // 現状の設定から足りない情報を追加してシリアライズ
                    Remake(numberTypeConfigFullPath, LogWrite);
                }

            }
            catch (Exception ex)
            {
                LogWrite($"※NumberTypeConfigWork.PreparationConfigData(...)で例外発生 {ex.Message} {ex.InnerException} ");
                Eventlog.Log.WriteEntry("SasaLibNumberingSupport", EventLogEntryType.Error, 0, $"※NumberTypeConfigWork.PreparationConfigData(...)で例外発生 設定ﾌｧｲﾙ{numberTypeConfigFullPath}, 例外情報:{ex.Message} {ex.InnerException}");
                sr.Close();

                return false;
            }
            return true;
        }

        /// <summary>
        /// 足りない設定はデフォルト値で埋めて設定ファイルを更新する
        /// </summary>
        /// <param name="numberTypeConfigFullPath"></param>
        public static void Remake(string numberTypeConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
        {
            bool updated = false;


            // 足りない設定があれば追加する                
            if (NumberTypeConfig.Config.VersionNumber <= 1.0d)
                NumberTypeConfig.Config.VersionNumber = 1.0d;

            // 足りない設定があれば追加する                
            if (string.IsNullOrWhiteSpace(NumberTypeConfig.Config.VersionComment))
            {
                NumberTypeConfig.Config.VersionComment = "Ver 1.0d 2021-12-13 プログラム内蔵初期バージョン";
                updated = true;
            }


            // 足りない設定があれば追加する                
            if (NumberTypeConfig.Config.RegexPatern_TOYO_ASSY_Drawing == null || NumberTypeConfig.Config.RegexPatern_TOYO_ASSY_Drawing.Count() == 0)
            {
                NumberTypeConfig.Config.RegexPatern_TOYO_ASSY_Drawing = new List<string> {
                @"^[A-Z][A-Z0-9]*-\d{3,4}00-(\d{1,3})?[RL]?[L]?(-R|-L)?$" ,
                @"^[A-Z][A-Z0-9]*-\d{3,4}00-(\d{3})[~～](\d{3})[RL]?(RL)?$" ,
                @"^[A-Z][A-Z0-9]*-\d{3,4}00[RL]?[L]?$" ,
                @"^[A-Z]{3}-.{2}-([A-Z]\d{3}00)?[RL]?[L]?(-R|-L)?$",
                @"^TEST-.{3}00-\d{3}[RL]?[L]?(-R|-L)?$",
                @"^\d{2}-\d{3}00-\d{3}[RL]?[L]?(-R|-L)?$",
                @"^XX-\d{3}00-\d{3}[RL]?[L]?(-R|-L)?$",
                };
                updated = true;
            }

            // 足りない設定があれば追加する                
            if (NumberTypeConfig.Config.RegexPatern_TOYO_PART_Drawing == null || NumberTypeConfig.Config.RegexPatern_TOYO_PART_Drawing.Count() == 0)
            {
                NumberTypeConfig.Config.RegexPatern_TOYO_PART_Drawing = new List<string> {
                @"^[A-Z][A-Z0-9]*-\d{5,6}-(\d{1,3})?[RL]?[L]?(-R|-L)?$" ,
                @"^[A-Z][A-Z0-9]*-\d{5,6}-(\d{3})[~～](\d{3})[RL]?(RL)?$" ,
                @"^[A-Z][A-Z0-9]*-\d{5,6}[RL]?[L]?$" ,
                @"^[A-Z]{3}-.{2}-([A-Z]\d{4})?[RL]?[L]?(-R|-L)?$",
                @"^TEST-.{5}-.{3}[RL]?[L]?(-R|-L)?$",
                @"^\d{2}-\d{5}-\d{3}[RL]?[L]?(-R|-L)?$",
                @"^XX-\d{5}-\d{3}[RL]?[L]?(-R|-L)?$",
                };
                updated = true;
            }

            // 足りない設定があれば追加する                
            if (NumberTypeConfig.Config.RegexPatern_TOYO_Variant_Suffix == null || NumberTypeConfig.Config.RegexPatern_TOYO_Variant_Suffix.Count() == 0)
            {
                NumberTypeConfig.Config.RegexPatern_TOYO_Variant_Suffix = new List<string> {
                @"-(\d{3})[~～](\d{3})[RL]?(RL)?$" ,
                };
                updated = true;
            }

            //
            if (updated)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(NumberTypeConfig));
                using (StreamWriter sw = new StreamWriter(numberTypeConfigFullPath, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, NumberTypeConfig.Config);
                    if (LogWrite != null) LogWrite($"■設定ファイル {numberTypeConfigFullPath} を更新しました");
                }
            }

        }

        /// <summary>
        /// NumberTypeConfig.XMLへ設定変数を描き戻す
        /// </summary>
        public static bool SaveConfig(string numberTypeConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
        {
            bool result = false;
            XmlSerializer serializer = new XmlSerializer(typeof(NumberTypeConfig));
            using (StreamWriter sw = new StreamWriter(numberTypeConfigFullPath, false, Encoding.UTF8))
            {
                try
                {
                    serializer.Serialize(sw, NumberTypeConfig.Config);
                    if (LogWrite != null) LogWrite($"■設定ファイル {numberTypeConfigFullPath} を更新しました");
                    result = true;
                }
                catch (IOException ioe)
                {
                    if (LogWrite != null) LogWrite($"※設定ファイル {numberTypeConfigFullPath} の更新で例外発生{ioe.Message}");
                    result = false;
                }
            }
            return result;
        }
    }
}
