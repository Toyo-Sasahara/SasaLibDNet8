//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics.Eventing.Reader;
//using System.Runtime.Versioning;

//namespace SasaLib
//{

//    [SupportedOSPlatform("windows")]
//    public class SasaLibMySqlConnect
//    {
//        string Server;
//        int Port;
//        string Database;

//        /// <summary>
//        /// MySql コネクションストリングの Charset='value' の値です
//        /// </summary>
//        string CharSet = "sjis";
//        string Uid;
//        string Pwd;

//        string ConnectionString;

//        MySqlConnection connection;
//        MySqlCommand command;

//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        /// <param name="Server">ホスト名</param>
//        /// <param name="Port"ポート番号></param>
//        /// <param name="Database">データベース名 exp:"sampledb050"</param>
//        /// <param name="Charset">キャラクタセットexp:"sjis"</param>
//        /// <param name="Uid">exp:"toyo_readonly"</param>
//        /// <param name="Pwd">exp:""</param>
//        public SasaLibMySqlConnect(string Server, int Port, string Database, string Charset, string Uid, string Pwd)
//        {
//            this.Server = Server;
//            this.Port = Port;
//            this.Database = Database;
//            this.CharSet = Charset;
//            this.Uid = Uid;
//            this.Pwd = Pwd;

//            ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}; Pwd={Pwd}; Charset='{Charset}'";

//        }

//        /// <summary>
//        /// 部品図採番実績を検索
//        /// </summary>
//        /// <param name="TableName"></param>
//        /// <param name="PREF_NAME"></param>
//        /// <param name="PREF_RL"></param>
//        /// <param name="NumberingRecordAvaliable">採番実績があればtrue</param>
//        /// <param name="anserstring"></param>
//        /// <returns>データベース問合せが正常ならtrue<</returns>
//        public bool DrawingPartNumberFindFirst(string TableName, string PREF_NAME, string PREF_RL, out bool NumberingRecordAvaliable, out string anserstring)
//        {
//            NumberingRecordAvaliable = false;
//            anserstring = null;

//            try
//            {
//                using (connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString))
//                using (command = new MySql.Data.MySqlClient.MySqlCommand())
//                {
//                    command.Connection = connection;

//                    connection.Open();

//                    // SQLを実行します。
//                    command.CommandText = $"SET NAMES binary;SELECT * FROM {TableName}  WHERE (PREF_NAME = '{PREF_NAME}' AND PREF_RL = '{PREF_RL}') ORDER BY PREF_DATE DESC;";

//                    //var line = command.ExecuteNonQuery();

//                    MySqlDataReader results = command.ExecuteReader();
//                    string resultOne = null;
//                    while (results.Read())
//                    {
//                        if (resultOne == null)
//                        {
//                            NumberingRecordAvaliable = true;
//                            resultOne = $"発見！！ {STR(results, "PREF_NAME")}{STR(results, "PREF_RL")} , 採番日;{((DateTime)results["PREF_DATE"]).ToShortDateString()} ,  {STR(results, "PREF_YOTEI")}";
//                        }
//                        else
//                            break;
//                    }
//                    connection.Close();
//                    anserstring = resultOne;
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Eventlog.Log.WriteEntry("SasaLibMySqlConnect", EventLogEntryType.Error, 0, $"SasaLibMySqlConnectエラー：例外発生 {ex.Message}");
//                return false;
//            }
//        }

//        /// <summary>
//        /// 組立図採番実績を検索
//        /// </summary>
//        /// <param name="TableName"></param>
//        /// <param name="PREF_NAME"></param>
//        /// <param name="NumberingRecordAvaliable">採番実績があればtrue</param>
//        /// <param name="anserstring"></param>
//        /// <returns>データベース問合せが正常ならtrue<</returns>
//        public bool DrawingAssyNumberFindFirst(string TableName, string PREF_NAME, out bool NumberingRecordAvaliable, out string anserstring)
//        {
//            NumberingRecordAvaliable = false;
//            anserstring = null;

//            try
//            {
//                using (connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString))
//                using (command = new MySql.Data.MySqlClient.MySqlCommand())
//                {
//                    command.Connection = connection;

//                    connection.Open();

//                    // SQLを実行します。
//                    command.CommandText = $"SET NAMES binary;SELECT * FROM {TableName}  WHERE PREF_NAME = '{PREF_NAME}' ORDER BY PREF_DATE DESC;";

//                    //var line = command.ExecuteNonQuery();

//                    MySqlDataReader results = command.ExecuteReader();
//                    string resultOne = null;
//                    while (results.Read())
//                    {
//                        if (resultOne == null)
//                        {
//                            NumberingRecordAvaliable = true;
//                            resultOne = $"発見！！ {STR(results, "PREF_NAME")} ,{STR(results, "PREF_CUSTOMER")},{STR(results, "PREF_MATHINE")} , 採番日;{((DateTime)results["PREF_DATE"]).ToShortDateString()} ,  {STR(results, "PREF_YOTEI")}";
//                        }
//                        else
//                            break;
//                    }
//                    connection.Close();
//                    anserstring = resultOne;
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Eventlog.Log.WriteEntry("SasaLibMySqlConnect", EventLogEntryType.Error, 0, $"SasaLibMySqlConnectエラー：例外発生 {ex.Message}");
//                return false;
//            }
//        }

//        /// <summary>
//        /// レイアウト図採番実績を検索
//        /// </summary>
//        /// <param name="TableName"></param>
//        /// <param name="PREF_NAME"></param>
//        /// <param name="NumberingRecordAvaliable">採番実績があればtrue</param>
//        /// <param name="anserstring"></param>
//        /// <returns>データベース問合せが正常ならtrue<</returns>
//        public bool DrawingLayoutNumberFindFirst(string TableName, string PREF_NAME, out bool NumberingRecordAvaliable, out string anserstring)
//        {
//            NumberingRecordAvaliable = false;
//            anserstring = null;

//            try
//            {
//                using (connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString))
//                using (command = new MySql.Data.MySqlClient.MySqlCommand())
//                {
//                    command.Connection = connection;

//                    connection.Open();

//                    // SQLを実行します。
//                    command.CommandText = $"SET NAMES binary;SELECT * FROM {TableName}  WHERE PREF_NAME = '{PREF_NAME}' ORDER BY PREF_DATE DESC;";

//                    //var line = command.ExecuteNonQuery();

//                    MySqlDataReader results = command.ExecuteReader();
//                    string resultOne = null;
//                    while (results.Read())
//                    {
//                        if (resultOne == null)
//                        {
//                            NumberingRecordAvaliable = true;
//                            resultOne = $"発見！！ {STR(results, "PREF_NAME")} ,{STR(results, "PREF_CUSTOMER")},{STR(results, "PREF_MATHINE")} , 採番日;{((DateTime)results["PREF_DATE"]).ToShortDateString()} ,  {STR(results, "PREF_YOTEI")}";
//                        }
//                        else
//                            break;
//                    }
//                    connection.Close();
//                    anserstring = resultOne;
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Eventlog.Log.WriteEntry("SasaLibMySqlConnect", EventLogEntryType.Error, 0, $"SasaLibMySqlConnectエラー：例外発生 {ex.Message}");
//                return false;
//            }
//        }

//        public List<string> SQLWhere(string SQLCOMMANDTEXT)
//        {
//            List<string> anserlines = new List<string>();
//            try
//            {
//                using (connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString))
//                using (command = new MySql.Data.MySqlClient.MySqlCommand())
//                {
//                    command.Connection = connection;

//                    connection.Open();

//                    // SQLを実行します。
//                    command.CommandText = $"SET NAMES binary;{SQLCOMMANDTEXT}";


//                    MySqlDataReader results = command.ExecuteReader();

//                    while (results.Read())
//                    {

//                        DateTime dt = DateTime.MinValue;
//                        try
//                        {
//                            dt = (DateTime)results["PREF_DATE"];
//                        }
//                        catch
//                        { }

//                        string res = $"PREF_NAME:{STR(results, "PREF_NAME"),-20}  ,  PREF_RL:{STR(results, "PREF_RL"),-2}  ,  PREF_CUSTOMER:{STR(results, "PREF_CUSTOMER"),-20}  ,  PREF_YOTEI:{STR(results, "PREF_YOTEI"),-20} ,PREF_DATE:{dt.ToShortDateString(),-20}";
//                        anserlines.Add(res);
//                    }
//                    connection.Close();
//                    return anserlines;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                Eventlog.Log.WriteEntry("SasaLibMySqlConnect", EventLogEntryType.Error, 0, $"SasaLibMySqlConnectエラー：例外発生 {ex.Message}");
//                return anserlines;
//            }
//        }

//        /// <summary>
//        /// MySqlDataReaderのデータから指定した属性名の値（漢字はSJISであること）を string値で取り出す
//        /// </summary>
//        /// <param name="dr">MySqlDataReaderのｵﾌﾞｼﾞｪｸﾄ</param>
//        /// <param name="KEY">属性名</param>
//        /// <returns></returns>
//        private string STR(MySqlDataReader dr, string KEY)
//        {
//            string result = null;
//            try
//            {
//                if (string.IsNullOrWhiteSpace(KEY))
//                {
//                    return "";
//                }
//                else
//                {
//                    if (CharSet == "sjis")
//                        result = StringUtil.SystemByteArraySJIStoString(dr[KEY]);
//                    else if (CharSet == "utf8")
//                        result = StringUtil.SystemByteArrayUTF8toString(dr[KEY]);
//                    else
//                        throw new Exception("CharSetの値は sjis または utf8 のどちらかです");

//                    return result;
//                }

//            }
//            catch (Exception ex)
//            {
//                Eventlog.Log.WriteEntry("SasaLibMySqlConnect", EventLogEntryType.Error, 0, $"SasaLibMySqlConnect.STR(MysqlDataReader dr,string KEY={KEY}) にて例外発生 {ex.Message}");

//                return null;
//            }
//        }

//    }
//}
