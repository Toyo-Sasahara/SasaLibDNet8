using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SasaLib.NumberingSupport
{
    public static class StageServerDatabaseConfigWork
    {
        static string AssemblyInternalName = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).InternalName;


        /// <summary>
        /// 
        /// </summary>
        public static DateTime ReadTime;

        /// <summary>
        /// StageServerDatabaseConfig.XML ファイルを読み込む。
        /// </summary>
        static public bool PreparationConfigData(string stageServerDatabaseConfigFullPath, bool remake = false, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            bool removeresult;
            if (remake)
            {
                removeresult = SasaLib.FileFolder.RemoveFile(stageServerDatabaseConfigFullPath);
            }

            System.IO.StreamReader sr = null;
            try
            {
                // 既に設定ファイルがある場合はあらかじめデシリアライズする
                if (FileFolder.FileExists(stageServerDatabaseConfigFullPath) == true)
                {

                    // 保存した設定ファイル内容を復元する
                    XmlSerializer serializer = new XmlSerializer(typeof(StageServerDatabaseConfig));
                    sr = new System.IO.StreamReader(stageServerDatabaseConfigFullPath, new System.Text.UTF8Encoding(false));
                    StageServerDatabaseConfig.Config = (StageServerDatabaseConfig)serializer.Deserialize(sr);

                    sr.Close();
                }
                else
                {
                    StageServerDatabaseConfig.Config = new StageServerDatabaseConfig();
                    LogWrite($"※ﾌｧｲﾙ {stageServerDatabaseConfigFullPath} は見つかりませんでした。");

                    // 現状の設定から足りない情報を追加してシリアライズ
                    Remake(stageServerDatabaseConfigFullPath, LogWrite);
                }
            }
            catch (Exception ex)
            {
                LogWrite($"※StageServerDatabaseConfigWork.PreparationConfigData(...)で例外発生 {ex.Message} {ex.InnerException} ");
                SasaLib.Eventlog.Log.WriteEntry("SasaLibNumberingSupport", EventLogEntryType.Error, 0, $"※StageServerDatabaseConfigWork.PreparationConfigData(...)で例外発生 設定ﾌｧｲﾙ{stageServerDatabaseConfigFullPath}, 例外情報:{ex.Message} {ex.InnerException}");
                sr.Close();

                return false;
            }
            return true;
        }

        /// <summary>
        /// 足りない設定はデフォルト値で埋めて設定ファイルを更新する
        /// </summary>
        /// <param name="stageServerDatabaseConfigFullPath"></param>
        public static void Remake(string stageServerDatabaseConfigFullPath, SasaLibDelegateWriteLine LogWrite = null)
        {
            bool updated = false;

            // 足りない設定があれば追加する                
            if (StageServerDatabaseConfig.Config.VersionNumber <= 1.0d)
                StageServerDatabaseConfig.Config.VersionNumber = 1.0d;

            // 足りない設定があれば追加する                
            if (string.IsNullOrWhiteSpace(StageServerDatabaseConfig.Config.VersionComment))
            {
                StageServerDatabaseConfig.Config.VersionComment = "Ver 1.0d 2021-12-13 プログラム内蔵初期バージョン";
                updated = true;
            }

            // 足りない設定があれば追加する                
            if (StageServerDatabaseConfig.Config.DrawingTypes == null || StageServerDatabaseConfig.Config.DrawingTypes.Count == 0)
            {
                StageServerDatabaseConfig.Config.DrawingTypes = new List<DrawingType> {
                            new DrawingType{Prefix="01", Class=DrawingClassEnum.Technical, TypeName="レイアウト図"},
                            new DrawingType{Prefix="02", Class=DrawingClassEnum.Technical, TypeName="動作フローチャート図"},
                            new DrawingType{Prefix="03", Class=DrawingClassEnum.Technical, TypeName="据付検査用レイアウト図"},
                            new DrawingType{Prefix="04", Class=DrawingClassEnum.Technical, TypeName="電装用レイアウト図"},
                            new DrawingType{Prefix="05", Class=DrawingClassEnum.PartsOrAssy, TypeName="部品図"},
                            new DrawingType{Prefix="10", Class=DrawingClassEnum.Technical, TypeName="印刷ピッチ図"},
                            new DrawingType{Prefix="11", Class=DrawingClassEnum.Technical, TypeName="パックレイアウト図"},
                            new DrawingType{Prefix="12", Class=DrawingClassEnum.Technical, TypeName="展開図"},
                            new DrawingType{Prefix="13", Class=DrawingClassEnum.Technical, TypeName="駆動系統図"},
                            new DrawingType{Prefix="14", Class=DrawingClassEnum.Technical, TypeName="タイミングチャート図"},
                            new DrawingType{Prefix="15", Class=DrawingClassEnum.Technical, TypeName="エアー配管図"},
                            new DrawingType{Prefix="16", Class=DrawingClassEnum.Technical, TypeName="機器配置図"},
                            new DrawingType{Prefix="17", Class=DrawingClassEnum.Technical, TypeName="電気設計データ表"},
                            new DrawingType{Prefix="18", Class=DrawingClassEnum.Technical, TypeName="グリース配管図"},
                            new DrawingType{Prefix="19", Class=DrawingClassEnum.Technical, TypeName="テンションロール配置図"},
                            new DrawingType{Prefix="20", Class=DrawingClassEnum.Technical, TypeName="残留リスクマップ"},
                            new DrawingType{Prefix="21", Class=DrawingClassEnum.Technical, TypeName="組立検査図面"},

                            new DrawingType{Prefix="BRTS", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="BRTM", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="BRP", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="BRE", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="BM", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="BG", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="CT", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="CB", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="DU", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="E", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EA", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EB", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EC", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ED", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EE", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EG", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EH", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EJ", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EK", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EL", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EM", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EP", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EQ", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ER", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ES", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ET", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="EY", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="F", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="G", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="GS", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="GN", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="H", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="HP", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="IG", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="J", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="JYT", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="JTM", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="JZB", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="K", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="KJ", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="KH", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="KG", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="KB", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="KA", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="LG", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="M", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="MA", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="MB", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="MG", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="MW", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="MNP", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="N", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="NG", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="P", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="PG", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="PT", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="R", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="RC", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="RG", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="RF", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="RS", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="S", Class=DrawingClassEnum.Parts,TypeName="" },
                            new DrawingType{Prefix="SB", Class=DrawingClassEnum.Parts,TypeName="" },
                            new DrawingType{Prefix="SC", Class=DrawingClassEnum.Parts,TypeName="" },
                            new DrawingType{Prefix="SCP", Class=DrawingClassEnum.Parts,TypeName="" },
                            new DrawingType{Prefix="SV", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="SFCR", Class=DrawingClassEnum.Parts,TypeName="" },
                            new DrawingType{Prefix="SF", Class=DrawingClassEnum.Parts,TypeName="" },
                            new DrawingType{Prefix="SH", Class=DrawingClassEnum.Parts,TypeName="" },
                            new DrawingType{Prefix="SK", Class=DrawingClassEnum.Parts,TypeName="" },
                            new DrawingType{Prefix="SL", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="SP", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ST", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="STD", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="SA3G", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="TEST", Class=DrawingClassEnum.Technical, TypeName="試作図"},

                            new DrawingType{Prefix="WG", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="X", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="XX", Class=DrawingClassEnum.PartsOrAssy},

                            new DrawingType{Prefix="ZA", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ZB", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ZC", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ZD", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ZE", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ZF", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ZG", Class=DrawingClassEnum.PartsOrAssy},
                            new DrawingType{Prefix="ZV", Class=DrawingClassEnum.PartsOrAssy},
                };
                updated = true;
            }


            //
            if (updated)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StageServerDatabaseConfig));
                using (StreamWriter sw = new StreamWriter(stageServerDatabaseConfigFullPath, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, StageServerDatabaseConfig.Config);
                    if (LogWrite != null) LogWrite($"設定ファイル {stageServerDatabaseConfigFullPath} を更新しました");
                }
            }

        }

        /// <summary>
        /// 現在の変数で保存
        /// </summary>
        public static void Save(string stageServerDatabaseConfigFullPath=null)
        {
            if (string.IsNullOrWhiteSpace(stageServerDatabaseConfigFullPath) == false)
            {
                stageServerDatabaseConfigFullPath = Path.Combine(FileFolder.GetCommonApplicationData(), @"C:\ProgramData\TOYOCOMMON", @"StageServerDatabaseConfig.XML");

                XmlSerializer serializer = new XmlSerializer(typeof(StageServerDatabaseConfig));
                using (StreamWriter sw = new StreamWriter(stageServerDatabaseConfigFullPath, false, Encoding.UTF8))
                {
                    serializer.Serialize(sw, StageServerDatabaseConfig.Config);
                    SasaLib.Eventlog.Log.WriteEntry("ToyoSTAGESERVICEDATABASEconfig", EventLogEntryType.Information, 9100,
                        $"図面承認・登録システム {AssemblyInternalName}\nSave() 設定ファイル {stageServerDatabaseConfigFullPath} を現在の変数で保存しました");
                }
            }

        }
    }
}

