///// SasahaSoftware 内部クラス
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Runtime.Versioning;

///// <summary>
///// 図面種類を分類する
///// </summary>
//namespace SasaLib.NumberingSupport
//{
//    /// <summary>
//    /// 図面種類を分類をXMLで記述するためのシリアライズ前提クラス
//    /// ｽﾃｰｼﾞｻｰﾊﾞｰと CADｱﾄﾞｲﾝ、承認クライアントにて 利用される
//    /// 変更時の影響に要注意
//    /// </summary>
//    [Serializable] // パイプでオブジェクトを送受信するため、シリアル化のマークが必要
//    [SupportedOSPlatform("windows")]
//    public class StageServerDatabaseConfig
//    {
//        public static StageServerDatabaseConfig Config { set; get; }

//        /// <summary>
//        /// この設置ファイルの書式バージョン
//        /// </summary>
//        //public string VERSION;

//        public double VersionNumber;
//        public string VersionComment;

//        /// <summary>
//        /// プリフィクス別に図面種類を設定
//        /// </summary>
//        public List<DrawingType> DrawingTypes { get; set; }


//        /// <summary>
//        /// ■図面番号先頭から図面種類を判別
//        /// </summary>
//        /// <param name="FirstGroup">ハイフンで区切った最初のグループ</param>
//        /// <returns></returns>
//        public static string GetDrawingTypeTypeName(string FirstGroup)
//        {
//            var DrawingTypes = StageServerDatabaseConfig.Config.DrawingTypes;
//            foreach (var drawingType in DrawingTypes)
//            {
//                if (drawingType.Prefix == FirstGroup)
//                {
//                    return drawingType.TypeName;
//                }
//            }

//            return "分類不能";
//        }

//        /// <summary>
//        /// ■プリフィックスを元に図面種類を分類
//        /// </summary>
//        /// <param name="Prefix">"M" とか "SF" とかの文字列。大文字小文字に注意</param>
//        /// <returns>図面のおおざっぱな分類を表す SasaLib.NumberingSupport.DrawingClassEnum</returns>
//        public static DrawingClassEnum GetDrawingTypeClass(string Prefix)
//        {
//            try
//            {
//                var DrawingTypes = StageServerDatabaseConfig.Config.DrawingTypes;
//                foreach (var drawingType in DrawingTypes)
//                {
//                    if (drawingType.Prefix == Prefix)
//                    {
//                        return drawingType.Class;
//                    }
//                }
//                return DrawingClassEnum.Unknown;

//            }
//            catch(Exception ex)
//            {
//                Eventlog.Log.WriteEntry("StageServerDatabaseConfig.DrawingClassEnum", EventLogEntryType.Error, 0, $"※StageServerDatabaseConfig.DrawingClassEnum(...) 。例外発生{ex.Message}");

//                return DrawingClassEnum.Unknown;

//            }
//        }

//        //XMLシリアライズのためには(なぜか)コンストラクタが必要
//        public StageServerDatabaseConfig() { }
//    }

//    /// <summary>
//    /// 図面種類を分類するためのクラス
//    /// </summary>
//    [Serializable]
//    public struct DrawingType
//    {
//        /// <summary>
//        /// 図面番号のプレフィックス (正規表現で "^.*-" にマッチする箇所)
//        /// </summary>
//        public string Prefix;
//        /// <summary>
//        /// Prefix にて分類される分類名つまりArcSuiteでの"図面種類"
//        /// </summary>
//        public string TypeName;
//        /// <summary>
//        /// この Prefix の図面が、部品図または組立図、技術図書、不明 のどちらに該当するか
//        /// </summary>
//        public DrawingClassEnum Class;
//    }

//    /// <summary>
//    /// 図面のおおざっぱな分類を表す イーナム
//    /// </summary>
//    [Serializable]
//    public enum DrawingClassEnum
//    {
//        /// <summary>
//        /// 不明
//        /// </summary>
//        Unknown,
//        /// <summary>
//        /// 部品図か組立図そのどちらかが含まれるプレフィックス
//        /// </summary>
//        PartsOrAssy,
//        /// <summary>
//        /// 技術図書のみが含まれるプレフィックス
//        /// </summary>
//        Technical,
//        /// <summary>
//        /// 部品図のみが含まれるプレフィックス
//        /// </summary>
//        Parts,
//        /// <summary>
//        /// 組立図のみが含まれるプレフィックス
//        /// </summary>
//        Assy,
//    }
//}
