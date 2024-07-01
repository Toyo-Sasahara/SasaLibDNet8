//using StageServerRemote;
//using System;
//using System.Runtime.Versioning;
//using static StageServerRemote.RemoteClientCADtype;
///// <summary>
///// 
///// </summary>
//namespace SasaLib.ArcSuitePreview
//{
//    //public delegate void DelegateWriteLine(string msg);
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public struct ArcsuitePreview
//    {
//        public string ID;

//        /// <summary>
//        /// 検索対象となる サービスとキャビネットＩＤを合わせた文字列  ""
//        /// </summary>
//        public string SERVICEID_CABINETID;

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得したユーザー属性値  zuban
//        /// user:Zuban
//        /// </summary>
//        public string user_zuban { get; set; }

//        /// <summary>
//        /// ｱｰｸｽｲｰﾄからﾀﾞｳﾝﾛｰﾄﾞした図面イメージファイルの一時保存パス
//        /// </summary>
//        public string temporalyDrawingImageFullFileName;

//        /// <summary>
//        /// ｰｸｽｲｰﾄからﾀﾞｳﾝﾛｰﾄﾞした図面イメージファイルのページ数(1以上はマルチページとする)
//        /// </summary>
//        public int numberOfPages;

//        /// <summary>
//        /// いくつかの図面情報を連結した表示メッセージ
//        /// </summary>
//        public string createdOnMessage;

//        /// <summary>
//        /// ArcSuiteｼｽﾃﾑ属性 system:editionNumber に該当する
//        /// </summary>
//        public string system_editionNumber;

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得したユーザー属性値 user:drawingrevison に該当する
//        /// </summary>
//        public string user_drawingrevision;

//        /// <summary>
//        /// ArcSuiteｼｽﾃﾑ属性 system:createdon に該当する
//        /// </summary>
//        public string system_createdon;

//        /// <summary>
//        /// ArcSuiteｼｽﾃﾑ属性 system:filename に該当する
//        /// </summary>
//        public string system_filename;

//        /// <summary>
//        /// ArcSuiteｼｽﾃﾑ属性 system:contentsizebytes に該当する
//        /// </summary>
//        public string system_contentsizebytes;

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得したユーザー属性値 user:torokubi
//        /// </summary>
//        public string user_torokubi;

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得したユーザー属性値 user:guidticketcode
//        /// </summary>
//        public string user_guidticketcode;

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得した 対象オブジェクトのｼｽﾃﾑ属性:状態　system:status
//        /// </summary>
//        public string sysmte_status;

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得したユーザー属性値 user:partname "部品名" です
//        /// </summary>
//        public string user_partname;

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得したユーザー属性値 user:description "説明" です
//        /// </summary>
//        public string user_description;

//        /// <summary>
//        ///  ｱｰｸｽｲｰから直接取得したユーザー属性値 user:cadtype を 解読した、申告されたCAD種類の文字列です スペースを区切り値として採用される
//        ///  旧変数名 ModelingCadType
//        ///  戻り値例 "" または "InventorModel AutoCAD2D"
//        /// </summary>
//        public string user_cadtype_string
//        {
//            get
//            {
//                return NumberingSupport.ArcSuiteSupport.GetCadTypeString(user_cadtype);
//            }
//        }

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得したユーザー属性値 user:cadtyp
//        /// </summary>
//        public CadType user_cadtype;

//        /// <summary>
//        /// ｱｰｸｽｲｰから直接取得したユーザー属性値 user:modelcreationonorder "3D作成発注中" です
//        /// </summary>
//        public string user_modelcreationonorder;

//        /// <summary>
//        /// ｱｰｸｽｲｰﾄから直接取得した 対象オブジェクトの system:latestEditionFlag "最新版フラグ" です (念のため取得・パイプサーバー側での処理に注意)
//        /// </summary>
//        public string system_latestEditionFlag;

//        /// <summary>
//        /// ｱｰｸｽｲｰﾄから直接取得した 対象オブジェクトのコンテントタイプ ex image:tiff
//        /// </summary>
//        public string system_contentType;

//        /// <summary>
//        /// アークスイート検索でひっかかった場合trueとなります
//        /// </summary>
//        public bool Found;

//        /// <summary>
//        /// ArcSuite検索作業が正常終了したかのフラグ
//        /// </summary>
//        public bool Normality;


//        /// <summary>
//        /// ■CADファイルがアークスイート原本より古いかをチェックする共通メソッド.このメソッドではアークスイート登録図面番号は扱っていません
//        /// </summary>
//        /// <param name="CADfileCreatedTime">比較対象CADファイルの最終更新日時(System.IO.File.GetCreationTime(..)で取得した値)</param>
//        /// <param name="CADfileLastWriteTime"></param>
//        /// <param name="cadData_cadtype">比較対象CADファイルのデータ形式を表すCadTypeを指定する</param>
//        /// <param name="arcSuiteData_cadtype">ArcSuite側データから取得したCadTypeを指定する</param>
//        /// <param name="arcSuiteData_system_createdOn">ArcSuite側データの"登録日時"(system:createdOn)を指定する</param>
//        /// <param name="msg"></param>
//        /// <returns>比較対象CADファイルのCADタイプが、アークスイートから検索した図面のCADタイプに含まれておらずかつ双方の日付の比較の結果ｱｰｸｽｲｰﾄ側が新しければ true</returns>
//        public bool IsCadDataOlderThanArcSuite(DateTime CADfileCreatedTime, DateTime CADfileLastWriteTime, RemoteClientCADtype.CadType cadData_cadtype, RemoteClientCADtype.CadType arcSuiteData_cadtype, DateTime arcSuiteData_system_createdOn, out string msg)
//        {
//            /// 注意　Vaultでは 最初のﾁｪｯｸｲﾝ日を System.IO.File.GetLastWriteTime(), ﾊﾞｰｼﾞｮﾝの作成日を System.IO.File.GetCreationTime() にて取得されるに日時
//            /// としています
//            try
//            {
//                if ((CADfileCreatedTime != null ) && (CADfileLastWriteTime != null))
//                {
//                    string CADfileCreatedTime_String = CADfileCreatedTime.ToString("yyyy/MM/dd , HH:mm");
//                    string CADfileLastWriteTime_String = CADfileLastWriteTime.ToString("yyyy/MM/dd , HH:mm");

//                    //string ArcSuiteCreatedOn_label_ToolTip = $"CADﾌｧｲﾙ【{cadData_FulllFileName}】更新時刻:{CADfileCreatedTime_String}";

//                    bool containSameCadType = arcSuiteData_cadtype.HasFlag(cadData_cadtype); //ArcSuite側データから取得したCadTypeに、CAD側の CadTypeが含まれるかをチェックする

//                    // 登録日時をテキストラベルに反映
//                    string dateTime_system_createdOn_String = arcSuiteData_system_createdOn.ToString("yyyy/MM/dd , HH:mm");

//                    // 現在開いているCADタイプが、アークスイートから検索した図面のCADタイプに含まれておらず、日付の比較の結果ｱｰｸｽｲｰﾄが新しければ警告します
//                    if ((containSameCadType == false) && (arcSuiteData_system_createdOn > CADfileCreatedTime))
//                    {
//                        msg = $"ArcSuite側データの登録日時:{dateTime_system_createdOn_String} ※CADﾌｧｲﾙの更新日付{CADfileCreatedTime_String}より新しい";
//                        return true;
//                    }
//                    else
//                    {
//                        msg = null;
//                        return false;
//                    }
//                }
//                else
//                    throw new Exception("CADfileCreatedTimeがnull");
//            }
//            catch (Exception ex)
//            {
//                msg = $"エラー 例外発生 {ex.Message}";
//                return false;
//            }
//        }

//        /// <summary>
//        /// ArcSuite側データのCADタイプに指定したＣＡＤタイプが含まれず、かつArcSuiteの方がCADデータタイプより古い場合trueを返す
//        /// </summary>
//        /// <param name="CADfileCreatedTime"></param>
//        /// <param name="cadData_cadtype">チェックされているか調べようとするcadType</param>
//        /// <param name="arcSuiteData_cadtype">ArcSuite側が保持しているCadaType</param>
//        /// <param name="arcSuiteData_system_createdOn">ArcSuite側の登録日時</param>
//        /// <returns></returns>
//        public bool CanSetThisCadTypeToArcSuite(DateTime CADfileCreatedTime, RemoteClientCADtype.CadType cadData_cadtype, RemoteClientCADtype.CadType arcSuiteData_cadtype, DateTime arcSuiteData_system_createdOn)
//        {
//            bool result = false;

//            // 比較対象ＣＡＤファイルのＣＡＤデータタイプを含まなくてCADデータの方がアークスイートのcreatedTimeより大きい場合に true を返す
//            bool containSameCadType = arcSuiteData_cadtype.HasFlag(cadData_cadtype); //ArcSuite側データから取得したarcSuiteData_cadtypeに、CAD側の CadTypeが含まれるかをチェックする
//            if (containSameCadType == false)
//            {
//                if (arcSuiteData_system_createdOn < CADfileCreatedTime)
//                {
//                    result = true;
//                }
//                else
//                {
//                    result = false;
//                }
//            }
//            else
//            {
                
//                result = false;
//            }

//            return result;
//        }

//        /// <summary>
//        /// user_descriptionとuser_partname より実際の名称を作る
//        /// </summary>
//        /// <param name="stArcSuitePreview"></param>
//        /// <returns></returns>
//        public static string SelectArcSuitePreviewDESCRITIONorPARTNAME(ArcsuitePreview stArcSuitePreview)
//        {
//            // stArcSuitePreview.user_description か stArcSuitePreview.user_partname に文字列があればそれを採用する
//            string result = null;
//            if (string.IsNullOrWhiteSpace(stArcSuitePreview.user_partname) == false)
//                result = stArcSuitePreview.user_partname;
//            ////////////////if (string.IsNullOrWhiteSpace(stArcSuitePreview.user_description) == false)
//            result = stArcSuitePreview.user_description;
//            return result;
//        }

//    }
//}
