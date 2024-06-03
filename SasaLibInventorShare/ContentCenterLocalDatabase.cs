using Inventor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SasaLib.InventorAPI
{
    public class ContentCenterLocalDatabase
    {
        Inventor.Application sInventorApp;

        // データベースオブジェクト
        public DataTable ContentCenterLocalDatabaseObject { get; set; }
        // このデータベースのバージョンメッセージ
        public string TimestampString { get; private set; }
        // このデータベースのデータ行数
        public long DatabaseRowsCount { get; private set; }

        SasaLibDelegateWriteLine WriteLine;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sInventorApp">Inventorアプリケーションオブジェクト</param>
        /// <param name="LocalDataBaseFullFileName">コンテンツセンターローカルデータベースファイル（存在する場合はそのまま読みこむ。ない場合はサーバーﾌｧｲﾙを取得）</param>
        /// <param name="tablename">ContentCenterLocalDatabaseObject　の テーブル名</param>
        /// <param name="ServerSouceFullFileName">サーバーから読み出すときのフルパス名（省略時は読み込まず）</param>
        public ContentCenterLocalDatabase(Inventor.Application sInventorApp, string LocalDataBaseFullFileName, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;
            else this.WriteLine = LogWrite;

            this.sInventorApp = sInventorApp;

            if (System.IO.File.Exists(LocalDataBaseFullFileName) == true)
            {

                DateTime writeDate = System.IO.File.GetLastWriteTime(LocalDataBaseFullFileName);
                LogWrite($"ｺﾝﾃﾝﾂｾﾝﾀﾛｰｶﾙﾃﾞｰﾀﾍﾞｰｽﾌｧｲﾙ {LocalDataBaseFullFileName} 更新日時 {writeDate}");

                DataSet ds = new DataSet();

                try
                {
                    using (StreamReader sr = new StreamReader(LocalDataBaseFullFileName))
                    {
                        //最初の1行をタイムスタンプ情報行とする
                        TimestampString = sr.ReadLine();
                        //次の行の文頭文末の"を除外した文字列をDataTableオブジェクトの名前とします
                        string DataTableNameLine = sr.ReadLine().Trim('"');
                        ContentCenterLocalDatabaseObject = ds.Tables.Add(DataTableNameLine);

                        //次の行を,区切り文頭文末の"を除外した文字列をDataColumn名とします
                        string DataColumnsNameLine = sr.ReadLine();
                        string[] DataColumnNameArray = DataColumnsNameLine.Split(',');
                        foreach (string word in DataColumnNameArray)
                        {
                            string DataColumnName = word.Trim('"');
                            ContentCenterLocalDatabaseObject.Columns.Add(DataColumnName);
                        }

                        // 末尾まで繰り返す
                        while (!sr.EndOfStream)
                        {
                            // ファイルの一行を読み込む
                            string line = sr.ReadLine();
                            line = line.Trim();

                            if (string.IsNullOrWhiteSpace(line) == true)
                                continue;
                            // # はコメント行。飛ばす。
                            if (line[0] == '#')
                                continue;

                            // 読み込んだ一行を\に分けて配列に格納する
                            string[] values = line.Split('\\');

                            // データ行の構造定義 DataColumn名 に従って追加 
                            ContentCenterLocalDatabaseObject.Rows.Add(values[0], values[1], values[2]); // DataColumnNameArray[0],DataColumnNameArray[1],DataColumnNameArray[2],

                            List<string> lists = new List<string>();
                            lists.AddRange(values);
                        }
                    }
                    DatabaseRowsCount = ContentCenterLocalDatabaseObject.Rows.Count;

                }
                catch (Exception ex)
                {
                    LogWrite($"{LocalDataBaseFullFileName}からデータを読み込めませんでした。例外検知 {ex.Message} {ex.StackTrace}");
                }
            }
            else
            {
                LogWrite($"{LocalDataBaseFullFileName}からデータを読み込めませんでした。ｺﾝﾃﾝﾂｾﾝﾀの検索はできません");
            }
        }

        /// <summary>
        /// 指定した ContentIdentifier の コンテンツセンタパーツを ローカルフォルダにﾀﾞｳﾝﾛｰﾄﾞしそのパスを返す
        /// </summary>
        /// <param name="ContentTableRow_ContentIdentifier">ContentCenter オブジェクトの GetContentObject メソッドで使用可能なID</param>
        /// <param name="FailureMessage">メンバの作成に失敗した場合は、失敗の理由を説明するメッセージが含まれる出力文字列です。失敗しなかった場合は、空の文字列が返されます。</param>
        /// <param name="Refresh">メンバが既にローカルに存在している場合、使用する動作を指定するオプションの入力 です。
        /// kUseDefaultRefreshSetting は、ユーザが[アプリケーション オプション]ダイアログの[コンテンツ センター]タブを使用して設定した既定の設定をメソッドが使用されることを示します。
        /// kRefreshOutOfDateParts は、パーツが既にローカル キャッシュ内に存在するが期限が切れている場合にパーツの最新バージョンに置き換えることを示します。
        /// kDoNotRefreshOutOfDateParts は、既存のパーツを使用し、オーバーライドは使用しないことを示します。</param>
        /// <returns></returns>
        public string GetContentCenterPartFullFileName(string ContentTableRow_ContentIdentifier, out string FailureMessage, ContentMemberRefreshEnum Refresh = ContentMemberRefreshEnum.kUseDefaultRefreshSetting, SasaLibDelegateWriteLine LogWrite = null)
        {
            if (LogWrite == null) LogWrite = DebugConsole.WriteLine;

            string query = $"ContentTableRow_ContentIdentifier = '{ContentTableRow_ContentIdentifier}'";
            LogWrite($"■GetContentCenterPartFullFileName(..)開始  ContentTableRow_ContentIdentifier = '{query}' を検索開始");
            Object[] ItemArray;
            bool ans = FindFirst(query, out ItemArray);
            if (ans)
            {
                LogWrite($"■GetContentCenterPartFullFileName(..) ContentTableRow_ContentIdentifier = '{query}'　見つかりました");
                // コンテンツファミリーを特定
                ContentTableRow objectGetContent = default;
                try
                {
                    objectGetContent = (ContentTableRow)this.sInventorApp.ContentCenter.GetContentObject(ContentTableRow_ContentIdentifier);
                    LogWrite($"■GetContentCenterPartFullFileName(..) ContentTableRow_ContentIdentifier = '{query}'　見つかりました");
                }
                catch (Exception ex)
                {
                    LogWrite($"※GetContentCenterPartFullFileName(..)例外検知 {ex.Message}");
                }
                if (objectGetContent != null)
                {
                    ContentFamily contentFamily = (ContentFamily)objectGetContent.Parent;
                    if (contentFamily != null)
                    {
                        // デバッグ用途
                        string TemplateFileName = contentFamily.TemplateFileName;

                        // 作成に失敗した場合は、FailureReason 引数で失敗の理由を示します
                        MemberManagerErrorsEnum FailureReason;
                        sInventorApp.SilentOperation = true;

                        // 指定されたテーブル行のメンバ パーツを作成するメソッドです。作成されたメンバの完全なファイル名を返します。作成に失敗した場合は、FailureReason 引数で失敗の理由を示します。
                        string ContentCenterMemberFullFileName = contentFamily.CreateMember(objectGetContent, out FailureReason, out FailureMessage, Refresh);

                        //■デバッグ情報
                        ContentTableColumn FileNameColumn = contentFamily.FileNameColumn;
                        string MemberDirectory = contentFamily.MemberDirectory;

                        sInventorApp.SilentOperation = false;

                        if (FailureReason == MemberManagerErrorsEnum.kMemberManagerNoError)
                        {
                            LogWrite($"FailureMessage = {FailureMessage}");
                            LogWrite($"ContentCenterMemberFullFileName= {ContentCenterMemberFullFileName}");
                            LogWrite($"TemplateFileName= {TemplateFileName}");

                            return ContentCenterMemberFullFileName;
                        }
                        else if (FailureReason == MemberManagerErrorsEnum.kMemberManagerDifferentMember)
                        {
                            LogWrite($"FailureMessage = {FailureMessage}");
                            LogWrite($"ContentCenterMemberFullFileName= {ContentCenterMemberFullFileName}");
                            LogWrite($"TemplateFileName= {TemplateFileName}");

                            return ContentCenterMemberFullFileName;
                        }
                        else if (FailureReason == MemberManagerErrorsEnum.kMemberManagerDifferentFamily)
                        {
                            LogWrite($"FailureMessage = {FailureMessage}");
                            LogWrite($"ContentCenterMemberFullFileName= {ContentCenterMemberFullFileName}");
                            LogWrite($"TemplateFileName= {TemplateFileName}");

                            return ContentCenterMemberFullFileName;
                        }
                        else
                        {
                            LogWrite($"FailureReason = {FailureReason} {FailureMessage}");

                            return ContentCenterMemberFullFileName;
                        }
                    }
                    FailureMessage = null;

                    return null;
                }
            }
            FailureMessage = null;

            return null;
        }

        /// <summary>
        /// ContentIdentifierを Part Number から最初にヒットしたコンテントIDを返す
        /// </summary>
        /// <param name="Search_ColumnName"></param>
        /// <param name="ColumnValue">コンテンツセンターのPart_Number</param>
        /// <param name="Get_Take_ColumnName"></param>
        /// <returns>データベースから最初ヒットした ContentIdentifier を返す</returns>
        public string GetContentTableRow_ContentIdentifierFindFirst(string Search_ColumnName, string ColumnValue, string Get_Take_ColumnName)
        {
            string query = $"{Search_ColumnName} = '{ColumnValue}'";
            object[] ItemArray;

            DataRow[] dt;
            try
            {
                dt = ContentCenterLocalDatabaseObject.Select(query);
            }
            catch (Exception ex)
            {
                WriteLine($"例外検知{ex.Message}");
                return null;
            }

            if (dt.Count() > 0)
            {

                string ContentIdentifier = dt[0].Field<string>(Get_Take_ColumnName);

                return ContentIdentifier;
            }
            return null;
        }

        /// <summary>
        /// あらかじめ用意されたコンテンツセンターローカルデータベースから検索する
        /// </summary>
        /// <param name="SearchiListForVaultPartNumber"></param>
        /// <param name="ContentCenterMemberFileFullFileName"></param>
        /// <param name="FailureMessage"></param>
        /// <returns></returns>
        public bool SearchOne_FromContentCenterLocalDatabase(string SearchiListForVaultPartNumber, out string ContentCenterMemberFileFullFileName, out string FailureMessage)
        {
            ContentCenterMemberFileFullFileName = null;
            FailureMessage = null;

            // コンテントセンター（のローカルデータベースについて）検索.IDが返る
            string ContentIdentifier = this.GetContentTableRow_ContentIdentifierFindFirst("Part_Number", SearchiListForVaultPartNumber, "ContentTableRow_ContentIdentifier");

            ;
            ;
            if (ContentIdentifier != null)
            {
                ContentCenterMemberFileFullFileName = this.GetContentCenterPartFullFileName(ContentIdentifier, out FailureMessage, ContentMemberRefreshEnum.kDoNotRefreshOutOfDateParts, WriteLine);
                WriteLine($"●ｺﾝﾃﾝﾄｾﾝﾀﾛｰｶﾙﾃﾞｰﾀﾍﾞｰｽ検索結果。{SearchiListForVaultPartNumber} みつかりました {ContentCenterMemberFileFullFileName}");

                return true;
            }
            else
            {

                return false;
            }
        }

        /// <summary>
        /// コンテンツセンターローカルデータベースから前方一致の名前を検索して返す
        /// </summary>
        /// <param name="Search_ColumnName"></param>
        /// <param name="ColumnValue"></param>
        /// <param name="Get_Take_ColumnName"></param>
        /// <returns></returns>
        public IEnumerable<object> GetContentTableRow_PartNumbersFind(string Search_ColumnName, string ColumnValue, string Get_Take_ColumnName)
        {
            string query = $"{Search_ColumnName} LIKE '{ColumnValue}*'";

            System.Data.DataRow[] dt;
            try
            {
                dt = ContentCenterLocalDatabaseObject.Select(query);
            }
            catch (Exception ex)
            {
                WriteLine($"例外検知{ex.Message}");
                return null;
            }

            if (dt.Count() > 0)
            {

                int index = ContentCenterLocalDatabaseObject.Columns.IndexOf(Get_Take_ColumnName);
                var results = dt.Select(x => x.ItemArray).Select(x => x[index]);


                return results;
            }
            return null;
        }


        public DataRow[] GetContentTableDataRow_Find(string Search_ColumnName, string ColumnValue)
        {
            string query = $"{Search_ColumnName} LIKE '{ColumnValue}*'";

            DataRow[] dt;
            try
            {
                dt = ContentCenterLocalDatabaseObject.Select(query);
            }
            catch (Exception ex)
            {
                WriteLine($"例外検知{ex.Message}");
                return null;
            }

            if (dt.Count() > 0)
            {
                return dt;
            }
            return null;
        }

        /// <summary>
        /// ContentCenterLocalDatabaseObject を検索する。
        /// </summary>
        /// <param name="query">DataTable　オブジェクト に対しするクエリ</param>
        /// <param name="ItemArray">検索結果</param>
        /// <returns>みつからない場合はfalse</returns>
        private bool FindFirst(string query, out object[] ItemArray)
        {
            DataRow[] dt;
            try
            {
                dt = ContentCenterLocalDatabaseObject.Select(query);
            }
            catch (Exception ex)
            {
                WriteLine($"例外検知{ex.Message}");
                ItemArray = null;
                return false;
            }

            if (dt.Count() > 0)
            {
                ItemArray = dt[0].ItemArray;

                return true;
            }
            ItemArray = null;
            return false;
        }

        /// <summary>
        /// サポートメソッド DataTableオブジェクトのキー一覧を取得
        /// </summary>
        /// <returns></returns>
        public string[] GetDatabaseKeys()
        {
            List<string> ColumnNamesList = new List<string>();

            foreach (DataColumn dataColumn in ContentCenterLocalDatabaseObject.Columns)
            {

                ColumnNamesList.Add(dataColumn.ColumnName);
            }
            string[] ans = ColumnNamesList.ToArray();

            return ans;
        }

    }
}
