using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static SasaLib.NumberingSupport.PhrasesToBeAwareConfig;

namespace SasaLib.NumberingSupport
{
    [SupportedOSPlatform("windows")]
    public class PhrasesToBeAwareConfigWork
    {

        /// <summary>
        /// 設定を準備する。デシアライズ可能な場合は最初に実行。次に足りない設定をデフォルト値で埋める。
        /// 設定ファイルが存在しない場合はデフォルト値で新規作成する。
        /// </summary>
        /// <returns></returns>
        public static bool PreparationConfigData(string configFIleFullPath , bool remake = false, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            bool removeresult;
            if (remake)
            {
                removeresult = FileFolder.RemoveFile(configFIleFullPath);
            }

            System.IO.StreamReader sr = null;

            try
            {
                // 既に設定ファイルがある場合はあらかじめデシリアライズする
                if (FileFolder.FileExists(configFIleFullPath) == true)
                {
                    // 保存した設定ファイル内容を復元する
                    XmlSerializer serializer = new XmlSerializer(typeof(PhrasesToBeAwareConfig));
                    sr = new System.IO.StreamReader(configFIleFullPath, new System.Text.UTF8Encoding(false));
                    PhrasesToBeAwareConfig.Config = (PhrasesToBeAwareConfig)serializer.Deserialize(sr);
                    sr.Close();
                }
                else
                {
                    PhrasesToBeAwareConfig.Config = new PhrasesToBeAwareConfig();
                    LogWrite($"※ﾌｧｲﾙ {configFIleFullPath} は見つかりませんでした。");

                    // 現状の設定から足りない情報を追加してシリアライズ
                    Remake(configFIleFullPath, LogWrite);
                }

            }
            catch (Exception ex)
            {
                LogWrite($"※PhrasesToBeAwareConfigWork.PreparationConfigData(...)で例外発生 {ex.Message} {ex.InnerException} ");
                Eventlog.Log.WriteEntry("SasaLibNumberingSupport", EventLogEntryType.Error, 0, $"※PhrasesToBeAwareConfigWork.PreparationConfigData(...)で例外発生 設定ﾌｧｲﾙ{configFIleFullPath}, 例外情報:{ex.Message} {ex.InnerException}");
                sr.Close();

                return false;
            }
            return true;
        }

        /// <summary>
        /// 足りない設定はデフォルト値で埋めて設定ファイルを更新する
        /// </summary>
        /// <param name="PhrasesToBeAwareConfigFullPath"></param>
        public static void Remake(string PhrasesToBeAwareConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
        {
            bool updated = false;


            // 足りない設定があれば追加する                
            if (PhrasesToBeAwareConfig.Config.VersionNumber <= 1.0d)
                PhrasesToBeAwareConfig.Config.VersionNumber = 1.0d;

            // 足りない設定があれば追加する                
            if (string.IsNullOrWhiteSpace(PhrasesToBeAwareConfig.Config.VersionComment))
            {
                PhrasesToBeAwareConfig.Config.VersionComment = "Ver 1.0d 2021-12-13 プログラム内蔵初期バージョン";
                updated = true;
            }


            // 足りない設定があれば追加する                
            if (PhrasesToBeAwareConfig.Config.PhrasesToBeAwareDataSets == null)
            {
                PhrasesToBeAwareConfig.Config.PhrasesToBeAwareDataSets = new List<PhrasesToBeAwareConfig.PhrasesToBeAwareDataSet>()
                {
                    new PhrasesToBeAwareDataSet(){
                        Comment =@"平面度で指示が必要",
                        Pattern = "ひずみを取ること",
                        Evidence = @"https://tmm.cybozu.com/o/ag.cgi?page=BulletinView&bid=77259",
                        Replacement = "",
                        HowToDeal ="合否の判断ができません。必ず数値に\r\nより判定できる図面指示をお願い致します" },
                    new PhrasesToBeAwareDataSet(){
                        Comment =@"直角度で指示が必要",
                        Pattern = "直角に",
                        Evidence = @"https://tmm.cybozu.com/o/ag.cgi?page=BulletinView&bid=77259",
                        Replacement = "",
                        HowToDeal ="合否の判断ができません。必ず数値に\r\nより判定できる図面指示をお願い致します" },
                };
            }

            //
            if (updated)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PhrasesToBeAwareConfig));
                using (StreamWriter sw = new StreamWriter(PhrasesToBeAwareConfigFullPath, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, PhrasesToBeAwareConfig.Config);
                    if (LogWrite != null) LogWrite($"■設定ファイル {PhrasesToBeAwareConfigFullPath} を更新しました");
                }
            }

        }

        /// <summary>
        /// PhrasesToBeAwareConfig.XMLへ設定変数を描き戻す
        /// </summary>
        public static bool SaveConfig(string PhrasesToBeAwareConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
        {
            bool result = false;
            XmlSerializer serializer = new XmlSerializer(typeof(PhrasesToBeAwareConfig));
            using (StreamWriter sw = new StreamWriter(PhrasesToBeAwareConfigFullPath, false, Encoding.UTF8))
            {
                try
                {
                    serializer.Serialize(sw, PhrasesToBeAwareConfig.Config);
                    if (LogWrite != null) LogWrite($"■設定ファイル {PhrasesToBeAwareConfigFullPath} を更新しました");
                    result = true;
                }
                catch (IOException ioe)
                {
                    if (LogWrite != null) LogWrite($"※設定ファイル {PhrasesToBeAwareConfigFullPath} の更新で例外発生{ioe.Message}");
                    result = false;
                }
            }
            return result;
        }
    }
}
