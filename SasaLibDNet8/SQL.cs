using System;
using System.Collections.Generic;
using System.Data.SqlClient;
// usingが必要


namespace SasaLib.SQL
{
    /// <summary>
    /// 
    /// </summary>
    public class Sql
    {
        public static SqlConnection Connect(string connectionString)
        {
            //var connectionString = Sql.GetConnectionString2
            //    (ToyoPartsListApp.OptSw.SQLDataSouce,
            //    ToyoPartsListApp.OptSw.SQLInitialCatalog,
            //    ToyoPartsListApp.Properties.Settings.Default.SQLUserID,
            //    ToyoPartsListApp.Properties.Settings.Default.SQLPassword);

            using (var connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    // データベースの接続開始
                    connection.Open();
                    return connection;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    throw;
                }
                finally
                {
                    // データベースの接続終了
                    connection.Close();
                }
            }

        }

        /// <summary>
        /// SQL Server接続用文字列を組立
        /// </summary>
        /// <param name="Host">ホスト名</param>
        /// <param name="Catalog">データベース名</param>
        /// <param name="UID">ユーザー名</param>
        /// <param name="PASS"></param>
        /// <returns></returns>
        public static string GetConnectionString(string Host, string Catalog, string UID, string PASS)
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = Host,
                InitialCatalog = Catalog,
                IntegratedSecurity = false,
                UserID = UID,
                Password = PASS
            };

            return builder.ToString();
        }


    }

    /// <summary>
    /// 
    /// </summary>
    public class SQLSV
    {
        public SqlConnection connection;

        // SQLサーバーへのコネクションを確立
        public SQLSV(string Host, string Catalog, string UID, string PASS)
        {
            try
            {
                var builder = new SqlConnectionStringBuilder()
                {
                    DataSource = Host,
                    InitialCatalog = Catalog,
                    IntegratedSecurity = false,
                    UserID = UID,
                    Password = PASS
                };

                connection = new SqlConnection(builder.ToString());

            }
            catch(Exception ex)
            {
                DebugConsole.WriteLine($"※SQLSV.SQLSV(..)にて例外検知  {ex.Message} {ex.InnerException}");
            }
        }

        /// <summary>
        /// SQLコマンド実行
        /// </summary>
        /// <param name="sqldata"></param>
        /// <returns></returns>
        public int SQLCommandExecute(string sqldata)
        {
            int lineCount;
            connection.Open();
            try
            {
                SqlCommand com = new SqlCommand(sqldata, connection);
                lineCount = com.ExecuteNonQuery();
            }
            finally
            {
                connection.Close();
            }
            return lineCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="FILENAME"></param>
        /// <param name="JOBNAME"></param>
        /// <param name="PARTNUMBER"></param>
        /// <param name="DESCRIPITION"></param>
        /// <returns></returns>
        public bool CommandTest2(string GUID,string FILENAME, string JOBNAME, string PARTNUMBER, string DESCRIPITION)
        {
            using (System.Data.SqlClient.SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    // データベースの接続開始
                    connection.Open();

                    // SQLの実行
                    command.CommandText = @"INSERT INTO FILESTORE(
                    GUID, FILENAME, JOBNAME , PARTNUMBER , DESCRIPTION)
                    VALUES(
                    @GUID, @FILENAME, @JOBNAME , @PARTNUMBER , @DESCRIPTION 
                    )";
                    command.Parameters.Add("@GUID", System.Data.SqlDbType.NVarChar);                    // 1
                    command.Parameters.Add("@FILENAME", System.Data.SqlDbType.NVarChar);                    // 1
                    command.Parameters.Add("@JOBNAME", System.Data.SqlDbType.NVarChar);                    // 1
                    command.Parameters.Add("@PARTNUMBER", System.Data.SqlDbType.NVarChar);                    // 2
                    command.Parameters.Add("@DESCRIPTION", System.Data.SqlDbType.NVarChar);                // 3

                    command.Parameters["@GUID"].Value = GUID;
                    command.Parameters["@FILENAME"].Value = FILENAME;
                    command.Parameters["@JOBNAME"].Value = JOBNAME;
                    command.Parameters["@PARTNUMBER"].Value = PARTNUMBER;
                    command.Parameters["@DESCRIPTION"].Value = DESCRIPITION;

                    command.ExecuteNonQuery();

                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    //ToyoPartsListApp.ErrorViewLog.AddLog("SQL例外【" + exception.Message + "】発生");
                    return false;
                }
                finally
                {
                    // データベースの接続終了
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="GUID"></param>
        /// <param name="FILENAME"></param>
        /// <param name="JOBNAME"></param>
        /// <param name="PARTNUMBER"></param>
        /// <param name="DESCRIPITION"></param>
        /// <param name="GUID_RAW"></param>
        /// <returns></returns>
        public bool CommandTest(string GUID, string FILENAME, string JOBNAME, string PARTNUMBER, string DESCRIPITION, Guid GUID_RAW)
        {
            using (System.Data.SqlClient.SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    // データベースの接続開始
                    connection.Open();

                    // SQLの実行
                    command.CommandText = @"INSERT INTO FILESTORE(
                    GUID, FILENAME, JOBNAME , PARTNUMBER , DESCRIPTION, GUID_RAW)
                    VALUES(
                    @GUID, @FILENAME, @JOBNAME , @PARTNUMBER , @DESCRIPTION , @GUID_RAW
                    )";


                    // SQLパラメータ設定
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    sqlParameters.AddSqlParameter(new SqlParameter("@GUID", System.Data.SqlDbType.NVarChar)).Value = GUID;
                    sqlParameters.AddSqlParameter(new SqlParameter("@FILENAME", System.Data.SqlDbType.NVarChar)).Value = FILENAME;
                    sqlParameters.AddSqlParameter(new SqlParameter("@JOBNAME", System.Data.SqlDbType.NVarChar)).Value = JOBNAME;
                    sqlParameters.AddSqlParameter(new SqlParameter("@PARTNUMBER", System.Data.SqlDbType.NVarChar)).Value = PARTNUMBER;
                    sqlParameters.AddSqlParameter(new SqlParameter("@DESCRIPTION", System.Data.SqlDbType.NVarChar)).Value = DESCRIPITION;
                    sqlParameters.AddSqlParameter(new SqlParameter("@GUID_RAW", System.Data.SqlDbType.UniqueIdentifier)).Value = GUID_RAW;

                    command.Parameters.AddParams(sqlParameters);
                    command.ExecuteNonQuery();

                    return true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    //ToyoPartsListApp.ErrorViewLog.AddLog("SQL例外【" + exception.Message + "】発生");
                    return false;
                }
                finally
                {
                    // データベースの接続終了
                    connection.Close();
                }

            }
        }
    }


    /// <summary>
    /// SQLを発行する際、SQLインジェクション対策でSqlParametaerを使用する。
    /// ①通常の書き方の場合
    /// 
    /// SqlParameter param1 = new SqlParameter("@type1", SqlDbType.VarChar);
    /// param1.Value = "S";
    /// 
    /// command.Parameters.Add(param1);
    /// SqlParameter param2 = new SqlParameter("@type2", SqlDbType.VarChar);
    /// param2.Value = "U";
    /// 
    /// command.Parameters.Add(param2);
    /// SqlParameter param3 = new SqlParameter("@type3", SqlDbType.VarChar);
    /// param3.Value = "PK";
    /// 
    /// command.Parameters.Add(param3);
    /// 
    /// 上の様に行数が多くなってしまうが、拡張メソッドを使用すると下記の様にまとめられる
    /// 
    /// usingが必要
    /// using Extensions;
    /// 省略
    /// SQLパラメータ設定
    /// List<SqlParameter> sqlParameters = new List<SqlParameter>();
    /// sqlParameters.AddSqlParameter(new SqlParameter("@type1", SqlDbType.VarChar)).Value = "S";
    /// sqlParameters.AddSqlParameter(new SqlParameter("@type2", SqlDbType.VarChar)).Value = "U";
    /// sqlParameters.AddSqlParameter(new SqlParameter("@type3", SqlDbType.VarChar)).Value = "PK";
    /// command.Parameters.AddParams(sqlParameters);
    /// </summary>
    public static class SqlParameterExtension
    {
        /// <summary>
        /// ListにSqlParameterを追加し、追加したSqlParameterを返す
        /// 使用例
        /// List<SqlParameter> sqlParameters  = new List<SqlParameter>();
        /// sqlParameters.AddSqlParameter(new SqlParameter("@type1", SqlDbType.VarChar)).Value = "S";
        /// </summary>
        public static SqlParameter AddSqlParameter(this List<SqlParameter> list, SqlParameter parameter)
        {
            list.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// SqlParameterCollectionにList<SqlParameter>を追加
        /// </summary>
        public static void AddParams(this SqlParameterCollection collection, List<SqlParameter> list)
        {
            foreach (SqlParameter parameter in list)
            {
                collection.Add(parameter);
            }
        }
    }
}