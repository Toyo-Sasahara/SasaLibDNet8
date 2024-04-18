using SasaLib.ArcSuitePreview;
using StageServerRemote;
using System;
using System.Collections;
using System.Collections.Generic;
using static StageServerRemote.RemoteClientCADtype;
using SasaLib;
using SasaLib.NumberingSupport;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net.NetworkInformation;
using System.Linq;
using SasaLibDNet8;

namespace SasaLib.ArcSuitePreview
{
    public class CheckArcSuiteData
    {
        RemoteClientDRAWREGIST remoteClientDR;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="remoteClientDR"></param>
        public CheckArcSuiteData(RemoteClientDRAWREGIST remoteClientDR)
        {
            this.remoteClientDR = remoteClientDR;
        }

        /// <summary>
        /// ■ｱｰｸｽｲｰﾄへ図面を検索する.検索結果が１件以外は検索失敗とみなす(OLD Name CommitDialogCheckArcSuiteDuplicateConfirm)
        /// </summary>
        /// <param name="SANITIZEDPARTNUMBER"></param>
        /// <param name="ct"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        public ArcSuitePreview.ArcsuitePreview QueryStart(string SANITIZEDPARTNUMBER, CancellationToken ct, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            // Stopwatchクラス生成・計測開始
            var sw = new System.Diagnostics.Stopwatch(); sw.Start();
            delegateWriteLine($"■CheckArcSuiteData.QueryStart(..)開始。 ArcSuiteへ {SANITIZEDPARTNUMBER}を問い合わせ <<処理(A) 時間記録スタート>>");

            /// アークスイート検索をかけるパーツナンバーのリスト
            List<string> CheckPARTNUMBERlist = new List<string>();

            CheckPARTNUMBERlist.Add(SANITIZEDPARTNUMBER);

            ///delegateWriteLine($"■CheckArcSuiteData.QueryStart(..) List<string> CheckPARTNUMBERlist へ {SANITIZEDPARTNUMBER} を 追加しました");

            // ArcSuiteへ図面番号リストを使い検索.CSV形式がArrayListで返ります。
            ArrayList attributeArrayResults = null;
            string arcSuiteGetAttrValue
                = "user:zuban|system:editionNumber|system:createdOn|system:status|user:torokubi|user:drawingrevision|" +
                "user:guidticketcode|user:partname|user:description|user:modelcreationonorder|user:cadtype|system:latesteditionflag|system:contentType|system:fileName|system:contentSizeBytes";

            ArcSuitePreview.ArcsuitePreview stArcSuitePreview = new ArcSuitePreview.ArcsuitePreview();
            try
            {
                delegateWriteLine($"■CheckArcSuiteData.QueryStart(..) ArrayList RemoteClientDRAWREGIST.GetArcSuiteAttribute3(..) にて図面の検索を開始します...");

                attributeArrayResults = remoteClientDR.GetArcSuiteAttribute3("3e6f0b6f002b", CheckPARTNUMBERlist, arcSuiteGetAttrValue, ct, Console.WriteLine).Result;

                if (ct.IsCancellationRequested == false)
                {
                    if (attributeArrayResults != null)
                    {
                        delegateWriteLine($"■CheckArcSuiteData.QueryStart(..) ArrayList RemoteClientDRAWREGIST.GetArcSuiteAttribute3(..) 検索完了 返り値 attributeArrayResults.Count ={attributeArrayResults.Count}");

                        if (attributeArrayResults.Count == 1)
                        {
                            delegateWriteLine($"■CheckArcSuiteData.QueryStart(..) ArrayList RemoteClientDRAWREGIST.GetArcSuiteAttribute3(\"3e6f0b6f002b\",..) 返り値 attributeArrayResults.Count ={attributeArrayResults.Count} ZUMENキャビネットに見つからないため PURCHASED_ITEM_SPEC キャビネットに対して再検索を行います");

                            attributeArrayResults = remoteClientDR.GetArcSuiteAttribute3("PURCHASED_ITEM_SPEC", CheckPARTNUMBERlist, arcSuiteGetAttrValue, ct, Console.WriteLine).Result;

                            delegateWriteLine($"■CheckArcSuiteData.QueryStart(..) ArrayList RemoteClientDRAWREGIST.GetArcSuiteAttribute3(\"PURCHASED_ITEM_SPEC\",..) 返り値 attributeArrayResults.Count ={attributeArrayResults.Count} ");
                        }

                        /// CSVファイルを解析（検索結果は１件の場合のみ有効）
                        stArcSuitePreview = analyzeResultCSV(attributeArrayResults, delegateWriteLine);

                        sw.Stop(); TimeSpan ts = sw.Elapsed; // 計測終了
                        delegateWriteLine($"■CheckArcSuiteData.QueryStart(..)終了 stArcSuitePreview.Normality = {stArcSuitePreview.Normality} <<処理(A)経過時間:{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>>");

                        return stArcSuitePreview;
                    } // GetArcSuiteAttribute3 stArcSuitePreview nul以外
                    else
                    {
                        stArcSuitePreview = new ArcSuitePreview.ArcsuitePreview();
                        stArcSuitePreview.Found = false;
                        stArcSuitePreview.Normality = false;

                        sw.Stop(); TimeSpan ts = sw.Elapsed; // 計測終了
                        delegateWriteLine($"※CheckArcSuiteData.QueryStart(..)終了 エラー：GetArcSuiteAttribute3A stArcSuitePreview がnullでした。stArcSuitePreview.Normality = {stArcSuitePreview.Normality} <<処理(A)経過時間:{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>");

                        return stArcSuitePreview;
                    } // GetArcSuiteAttribute3 stArcSuitePreview が null
                }
                else
                {
                    delegateWriteLine($"●CheckArcSuiteData.QueryStart(..) キャンセル実行●");
                    ArcsuitePreview notresult = new ArcSuitePreview.ArcsuitePreview();
                    return notresult;
                }
            }
            catch (Exception ex)
            {
                delegateWriteLine($"※CheckArcSuiteData.QueryStart(..)終了 例外検知 {ex.Message}");
                return stArcSuitePreview;
            }
        }

        /// <summary>
        /// ■ｱｰｸｽｲｰﾄへ図面(stringのListコレクション)を検索する.検索結果を ArcsuitePreviewのコレクションにて返す
        /// </summary>
        /// <param name="listZUBAN"></param>
        /// <param name="ct"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        public List<ArcSuitePreview.ArcsuitePreview> QueryStart(List<string> listZUBAN, CancellationToken ct, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            // Stopwatchクラス生成・計測開始
            var sw = new System.Diagnostics.Stopwatch(); sw.Start();
            ///delegateWriteLine($"■CheckArcSuiteData.QueryStart(..) List<string> CheckPARTNUMBERlist へ {SANITIZEDPARTNUMBER} を 追加しました");

            // ArcSuiteへ図面番号リストを使い検索.CSV形式がArrayListで返ります。
            ArrayList resultsCsvs = null;
            string arcSuiteGetAttrValue
                = "user:zuban|system:editionNumber|system:createdOn|system:status|user:torokubi|user:drawingrevision|" +
                "user:guidticketcode|user:partname|user:description|user:modelcreationonorder|user:cadtype|system:latesteditionflag|system:contentType|system:fileName|system:contentSizeBytes";

            List<ArcSuitePreview.ArcsuitePreview> stArcSuitePreviews = new List<ArcSuitePreview.ArcsuitePreview>();
            try
            {
                if (listZUBAN != null && listZUBAN.Count > 0)
                {
                    var listZUBANstr = string.Join(", ", listZUBAN.Select(item => $"\"{item}\""));

                    delegateWriteLine($"■CheckArcSuiteData.QueryStart(..)：ArcSuiteへ問い合わせを開始します {listZUBAN.Count} 件 【{listZUBANstr}】");

                    resultsCsvs = remoteClientDR.GetArcSuiteAttribute3("3e6f0b6f002b", listZUBAN, arcSuiteGetAttrValue, ct, delegateWriteLine).Result;

                    if (ct.IsCancellationRequested == false)
                    {
                        if (resultsCsvs != null)
                        {

                            /// CSVファイルを解析
                            stArcSuitePreviews = analyzeResultsCSV(resultsCsvs, delegateWriteLine);

                            string  anserZUbansStr = "----";
                            try
                            {
                               anserZUbansStr = string.Join(", ", stArcSuitePreviews.Select(item => item.user_zuban).Select(item => $"\"{item}\""));
                            }
                            catch { }                          

                            sw.Stop(); TimeSpan ts = sw.Elapsed; // 計測終了
                            delegateWriteLine($"■CheckArcSuiteData.QueryStart(..)：完了・返り値 resultsCsv.Count = {resultsCsvs.Count - 1} 件, {anserZUbansStr} <<経過時間:{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>>");

                            return stArcSuitePreviews;
                        } //
                        else
                        {
                            sw.Stop(); TimeSpan ts = sw.Elapsed; // 計測終了
                            delegateWriteLine($"※CheckArcSuiteData.QueryStart(..)：エラー：nullでした。<<経過時間:{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>");

                            return null;
                        } // 
                    }
                    else
                    {
                        delegateWriteLine($"■CheckArcSuiteData.QueryStart(..)：キャンセル実行");
                        return null;
                    }
                }
                else
                {
                    delegateWriteLine($"■CheckArcSuiteData.QueryStart(..)：検索対象が指定されていません ...");
                    return null;
                }
            }
            catch (Exception ex)
            {
                delegateWriteLine($"※CheckArcSuiteData.QueryStart(..)：CheckArcSuiteData.QueryStart(..)終了 例外検知 {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ArcSuite属性検索結果のCSVファイルを解析 。回答結果が１件以下
        /// </summary>
        /// <param name="CsvData"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        private ArcSuitePreview.ArcsuitePreview analyzeResultCSV(ArrayList CsvData, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            ArcSuitePreview.ArcsuitePreview stArcSuitePreview = new ArcSuitePreview.ArcsuitePreview();

            if (CsvData.Count == 0)
            {
                stArcSuitePreview.Normality = false;
                stArcSuitePreview.Found = false;
                delegateWriteLine($"※CheckArcSuiteData.analyzeResultCSV(..)終了 エラー：CsvData.Countの値が{CsvData.Count}");
                return stArcSuitePreview;
            }
            else if (CsvData.Count == 1)
            {
                stArcSuitePreview.Normality = true;
                stArcSuitePreview.Found = false;
                delegateWriteLine($"■CheckArcSuiteData.analyzeResultCSV(..)終了 検索内容にヒットしませんでした");
                return stArcSuitePreview;
            }
            else if (CsvData.Count > 2)
            {
                stArcSuitePreview.Normality = false;
                stArcSuitePreview.Found = false;
                delegateWriteLine($"※CheckArcSuiteData.analyzeResultCSV(..)終了 エラー：CsvData.Countの値が{CsvData.Count}");
                return stArcSuitePreview;
            }

            try
            {
                string headder = Csv.CsvArrayListToString((ArrayList)CsvData[0]);
                string data = Csv.CsvArrayListToString((ArrayList)CsvData[1]);

                delegateWriteLine($"■CheckArcSuiteData.analyzeResultCSV(..)   ﾍｯﾀﾞ部 CsvData[0] = {headder}");
                delegateWriteLine($"■CheckArcSuiteData.analyzeResultCSV(..)   ﾃﾞｰﾀ部 CsvData[1] = {data}");
            }
            catch (Exception ex)
            {
                delegateWriteLine(ex.ToString());
            }

            bool isSucess;
            
            var result = ArcSuitePreviewDataCreate(CsvData, 1,out isSucess);

            return result;

        }

        /// <summary
        /// ArcSuite属性検索結果のCSVファイルを解析 。回答結果が複数
        /// </summary>
        /// <param name="CsvData"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns>
        /// 検索結果が 1件以上存在する場合：List<ArcSuitePreview.ArcsuitePreview>を返す（.Count() > 0）
        /// 検索結果が 0 の場合 List<ArcSuitePreview.ArcsuitePreview>を返す（.Count() == 0）
        /// 検索失敗の場合 null を返す
        /// </returns>
        private List<ArcSuitePreview.ArcsuitePreview> analyzeResultsCSV(ArrayList CsvData, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            List<ArcSuitePreview.ArcsuitePreview> arcsuitePreviews = new List<ArcsuitePreview>();

            if (CsvData.Count <= 0)
            {
                return null;
            }
            else  if (CsvData.Count == 1)
            {
                return arcsuitePreviews;
            }
            else
            {
                for (int ii = 1; ii < CsvData.Count; ii++)
                {
                    bool isSucess;
                    arcsuitePreviews.Add(ArcSuitePreviewDataCreate(CsvData, ii, out isSucess));
                } //

                return arcsuitePreviews;

            }

        } // 

        /// <summary>
        /// DRGETATTRの取得ＣＳＶ結果ファイルを解析、ArcsuitePreview 構造体にて返します
        /// </summary>
        /// <param name="CsvData"></param>
        /// <param name="ii"></param>
        /// <returns></returns>
        private ArcsuitePreview ArcSuitePreviewDataCreate(ArrayList CsvData, int ii, out bool sucess, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            try
            {
                string user_zuban = Csv.GetFromCsvArrayList(CsvData, "user:zuban", ii, true);

                if (String.IsNullOrEmpty(user_zuban) == false)
                {
                    string _serviceId = Csv.GetFromCsvArrayList(CsvData, "serviceId", ii, true);
                    string _cabinetId = Csv.GetFromCsvArrayList(CsvData, "cabinetId", ii, true);
                    string _drepId = Csv.GetFromCsvArrayList(CsvData, "drepId", ii, true);

                    string _system_filename = Csv.GetFromCsvArrayList(CsvData, "system:filename", ii, true);
                    string _system_contentsizebytes = Csv.GetFromCsvArrayList(CsvData, "system:contentsizebytes", ii, true);
                    string _system_contentType = Csv.GetFromCsvArrayList(CsvData, "system:contentType", ii, true);

                    string _system_editionnumber = Csv.GetFromCsvArrayList(CsvData, "system:editionnumber", ii, true);
                    string _system_createdon = Csv.GetFromCsvArrayList(CsvData, "system:createdon", ii, true);
                    string _system_status = Csv.GetFromCsvArrayList(CsvData, "system:status", ii, true);

                    string _system_latestEditionFlag = Csv.GetFromCsvArrayList(CsvData, "system:latesteditionflag", ii, true);

                    string _user_torokubi = Csv.GetFromCsvArrayList(CsvData, "user:torokubi", ii, true);
                    string _user_drawingrevision = Csv.GetFromCsvArrayList(CsvData, "user:drawingrevision", ii, true);
                    string _user_guidticketcode = Csv.GetFromCsvArrayList(CsvData, "user:guidticketcode", ii, true);
                    string _user_partname = Csv.GetFromCsvArrayList(CsvData, "user:partname", ii, true);
                    string _user_description = Csv.GetFromCsvArrayList(CsvData, "user:description", ii, true);
                    string _user_modelcreationonorder = Csv.GetFromCsvArrayList(CsvData, "user:modelcreationonorder", ii, true);
                    CadType user_cadtype = GetCadtypes(Csv.GetFromCsvArrayList(CsvData, "user:cadtype", ii, true));

                    string arcSuitePrevewCreateString;

                    if (string.IsNullOrWhiteSpace(_user_drawingrevision) == false)
                    {
                        arcSuitePrevewCreateString = "状態:" + _system_status + " 登録日時:" + _system_createdon + ", 図面Rev:" + _user_drawingrevision;
                    }
                    else
                    {
                        arcSuitePrevewCreateString = "状態:" + _system_status + " 登録日時:" + _system_createdon;
                    }

                    ArcSuitePreview.ArcsuitePreview stArcSuitePreview = new ArcSuitePreview.ArcsuitePreview()
                    {
                        ID = $"{_serviceId}:{_cabinetId}:{_drepId}",
                        SERVICEID_CABINETID = $"{_serviceId},{_cabinetId}",
                        user_zuban = user_zuban,
                        createdOnMessage = arcSuitePrevewCreateString,
                        system_editionNumber = _system_editionnumber,
                        system_createdon = _system_createdon,
                        sysmte_status = _system_status,
                        user_torokubi = _user_torokubi,
                        user_drawingrevision = _user_drawingrevision,
                        user_guidticketcode = _user_guidticketcode,
                        user_partname = _user_partname,
                        user_description = _user_description,
                        user_modelcreationonorder = _user_modelcreationonorder,
                        //stArcSuitePreview.user_cadtype_string = ArcSuiteSupport.GetCadTypeString(user_cadtype),
                        user_cadtype = user_cadtype,
                        system_latestEditionFlag = _system_latestEditionFlag,
                        system_contentType = _system_contentType,
                        system_filename = _system_filename,
                        system_contentsizebytes = _system_contentsizebytes,
                        Found = true,
                        Normality = true,
                    };

                    sucess = true;
                    return stArcSuitePreview;
                }
                else
                {
                    sucess = true;
                    return new ArcSuitePreview.ArcsuitePreview();
                }
            }
            catch (Exception ex)
            {
                delegateWriteLine($"");
                sucess = false;
                return new ArcSuitePreview.ArcsuitePreview();
            }
        } //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_user_strCadtype"></param>
        /// <returns></returns>
        public static CadType GetCadtypes(string _user_strCadtype)
        {
            int previous_cadtype;
            if (String.IsNullOrEmpty(_user_strCadtype))
                previous_cadtype = 0;
            else
            {
                if (int.TryParse(_user_strCadtype, out previous_cadtype) == false)
                    return CadType.NotSet;
            }
            return (CadType)previous_cadtype;
        }

        /// <summary>
        /// ｱｰｸｽｲｰﾄからイメージをSystem.Drawing.Imageオブジェクトとしてデータ受信を行う
        /// </summary>
        /// <param name="remoteClientDR"></param>
        /// <param name="ZUBAN"></param>
        /// <param name="FullFileName">ランダムに決定されたファイル名を返す</param>
        public static bool GetArcSuiteImagePipe(RemoteClientDRAWREGIST remoteClientDR, string ZUBAN, out string FullFileName, SasaLibDelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            // Stopwatchクラス生成・計測開始
            var sw = new System.Diagnostics.Stopwatch(); sw.Start();
            delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteImagePipe(..) スタート <<処理(B) 時間記録スタート>>");

            try
            {
                // ランダムフォルダ名を生成
                string PreviewImageFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "InventorTOYOaddin", System.IO.Path.GetRandomFileName());
                delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteImagePipe(..) プレビューイメージ一時保存ﾌｫﾙﾀﾞが決定されました {PreviewImageFolder} ");


                System.IO.Directory.CreateDirectory(PreviewImageFolder);
                delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteImagePipe(..) 一時保存ﾌｫﾙﾀﾞ {PreviewImageFolder} を作成しました");

                System.Drawing.Image img = remoteClientDR.GetArcSuiteLatestDrawing(ZUBAN);
                delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteImagePipe(..) System.Drawing.iamge remoteClientDR.GetArcSuiteLatestDrawing(..) を実行しました.結果:img = {img}");
                if (img != null)
                {
                    FullFileName = System.IO.Path.Combine(PreviewImageFolder, System.IO.Path.ChangeExtension(ZUBAN, "tif"));
                    delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteImagePipe(..) イメージのファイル名を組立ました :{FullFileName}");

                    bool result =   FileFolder.RemoveFile(FullFileName);
                    delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteImagePipe(..) 念のためﾞ {FullFileName} を削除しました 結果:{result}");

                    img.Save(FullFileName);

                    sw.Stop(); TimeSpan ts = sw.Elapsed; // 計測終了
                    delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteImagePipe(..) 終了。  {FullFileName}として {ZUBAN} を保存しました  <<処理(B)経過時間:{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>>");

                    return true;
                }
                else
                {
                    FullFileName = null;

                    sw.Stop(); TimeSpan ts = sw.Elapsed;　// 計測終了
                    delegateWriteLine($"※CheckArcSuiteData.GetArcSuiteImagePipe(..)終了 remoteClientDR.GetArcSuiteLatestDrawing(..) の結果がnullでした 図面ﾀﾞｳﾝﾛｰﾄﾞ失敗 {ZUBAN}　<<処理(B)経過時間:{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>>");

                    return false;
                }

            }
            catch (Exception ex)
            {
                FullFileName = null;
                sw.Stop(); TimeSpan ts = sw.Elapsed;　// 計測終了
                delegateWriteLine($"※CheckArcSuiteData.GetArcSuiteImagePipe(..)終了 例外発生 {ex.Message} <<処理(B)経過時間:{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>>");
                return false;
            }

        }

        /// <summary>
        /// ｱｰｸｽｲｰﾄからイメージをダウンロードする(実体ファイルをダウンロード)
        /// </summary>
        /// <param name="remoteClientDR"></param>
        /// <param name="ZUBAN"></param>
        /// <param name="PreviewImageCacheFolder"></param>
        /// <param name="FullFileName"></param>
        /// <param name="delegateWriteLine"></param>
        /// <returns></returns>
        public static bool GetArcSuiteContent(RemoteClientDRAWREGIST remoteClientDR, string target_ServiceID_CabinetID, string ZUBAN, string extension, string PreviewImageCacheFolder, out string FullFileName, CancellationToken ct, DelegateWriteLine delegateWriteLine = null)
        {
            if (delegateWriteLine == null) delegateWriteLine = Console.WriteLine;

            if (target_ServiceID_CabinetID == null) target_ServiceID_CabinetID = "ass1,3e6f0b6f002b";

            // Stopwatchクラス生成・計測開始
            var sw = new System.Diagnostics.Stopwatch(); sw.Start();
            delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteContent(..) スタート <<処理(C) 時間記録スタート>>");

            try
            {
                System.IO.Directory.CreateDirectory(PreviewImageCacheFolder);
                delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteContent(..) 一時保存先フォルダは {PreviewImageCacheFolder} を作成しました");

                FullFileName = System.IO.Path.Combine(PreviewImageCacheFolder, System.IO.Path.ChangeExtension(ZUBAN, extension));
                delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteContent(..) 保存ファイルﾊﾟｽは {FullFileName} を強制します");

                bool removeOk = FileFolder.RemoveFile(FullFileName);
                delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteContent(..) 念のため{FullFileName} を削除します。削除結果は{removeOk}でした");

                string ResultMsg;

                delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteContent(..) remoteClientDR.GetArcSuiteLatestDrawingFile(..) を開始します。");
                bool result = remoteClientDR.GetArcSuiteLatestDrawingFile(target_ServiceID_CabinetID: target_ServiceID_CabinetID, ZUBAN, FullFileName, out ResultMsg, Console.WriteLine);

                sw.Stop(); TimeSpan ts = sw.Elapsed; // 計測終了
                delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteContent(..) remoteClientDR.GetArcSuiteLatestDrawingFile(..) から取得が完了しました {ResultMsg} <<処理(C)経過時間:{ts.Minutes}分 {ts.Seconds}秒 , ({sw.ElapsedMilliseconds}msec)>>");

                if (result == true)
                {
                    delegateWriteLine($"■CheckArcSuiteData.GetArcSuiteContent(..) ArcSuiteから図面ﾀﾞｳﾝﾛｰﾄﾞ成功 {ZUBAN} {FullFileName} ");
                    return true;
                }
                else
                {
                    FullFileName = null;
                    delegateWriteLine($"※CheckArcSuiteData.GetArcSuiteContent(..) ArcSuite既に登録ずみの図面ﾀﾞｳﾝﾛｰﾄﾞ失敗 {ZUBAN} ");
                    Eventlog.Log.WriteEntry("SasaLibArcSuitePreview", EventLogEntryType.Error, 0, $"※CheckArcSuiteData.GetArcSuiteContent(..) ArcSuite既に登録ずみの図面ﾀﾞｳﾝﾛｰﾄﾞ失敗 {ZUBAN} ");
                    return false;
                }

            }
            catch (Exception ex)
            {
                delegateWriteLine($"※CheckArcSuiteData.GetArcSuiteContent({ZUBAN}, out string TemporalyFullFileName)で例外発生 {ex.Message}");
                Eventlog.Log.WriteEntry("SasaLibArcSuitePreview", EventLogEntryType.Error, 0, $"※CheckArcSuiteData.GetArcSuiteContent({ZUBAN}, out string TemporalyFullFileName)で例外発生 {ex.Message}");
                FullFileName = null;
                return false;
            }

        }
    }
}
