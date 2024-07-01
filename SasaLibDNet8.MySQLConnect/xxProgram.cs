//using MySql.Data.MySqlClient;
//using SasaLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SasaLibMySQLConnect
//{
//    //   class Program
//    //   {

//    //       // バージョン情報取得SQL
//    //       private static readonly string SelectVersion = "SELECT version()";

//    //       // private static readonly string AddPARTNUMBER = "INSERT INTO T01Prefecture VALUES(NULL, 'XX-12345-005', '', '笹原裕貴', '', '2021/09/08', '')";

//    //       static MySqlDataReader results;

//    //       static void Main(string[] args)
//    //       {
//    //           try
//    //           {
//    //               //TestMthod();


//    //               SasaLibMySqlConnect sasaLibMySqlConnect = new SasaLibMySqlConnect("ads1", 3306, "sampledb050", "sjis", "toyo_readonly", "");
//    ////               sasaLibMySqlConnect.DrawingNumberFindFirst("t01prefecture", "M-40410-042RL");


//    //               return;



//    //           }
//    //           catch (Exception e)
//    //           {
//    //               Console.WriteLine(e.Message);
//    //           }

//    //           Console.ReadKey();
//    //       }


//    //       static void TestMthod()
//    //       {
//    //           // 接続情報
//    //           string Server = "ads1";      // ホスト名
//    //           int Port = 3306;                  // ポート番号
//    //           string Database = "sampledb050";        // データベース名
//    //           string Uid = "toyo_readonly";              // ユーザ名
//    //           string Pwd = ""; // パスワード

//    //           // 接続文字列
//    //           //        private static readonly string ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}; Pwd={Pwd}";
//    //           //        private static readonly string ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}; Pwd={Pwd}; SslMode=none;";
//    //           //        private static readonly string ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}; Pwd={Pwd}; Charset='binary'";
//    //           string ConnectionString = $"Server={Server}; Port={Port}; Database={Database}; Uid={Uid}; Pwd={Pwd}; Charset='sjis'";


//    //           try
//    //           {
//    //               // コネクションオブジェクトとコマンドオブジェクトを生成します。
//    //               using (var connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString))
//    //               using (var command = new MySql.Data.MySqlClient.MySqlCommand())
//    //               {
//    //                   // コネクションをオープンします。

//    //                   connection.Open();

//    //                   // バージョン情報取得SQLを実行します。
//    //                   command.Connection = connection;
//    //                   command.CommandText = SelectVersion;
//    //                   var value = command.ExecuteScalar();
//    //                   var versionNo = value as string;

//    //                   // 取得したバージョン番号を表示します。
//    //                   Console.WriteLine($"バージョン番号={versionNo}");

//    //                   // SQLを実行します。
//    //                   byte[] NAMEBYTE = StringUtil.StringToSystemByteArraySJIS("");
//    //                   //command.CommandText = "SET NAMES binary;SELECT * FROM t01prefecture WHERE PREF_NAME LIKE 'XX-1234%';";
//    //                   command.CommandText = "SET NAMES binary;SELECT * FROM t01prefecture WHERE PREF_YOTEI LIKE '笹%';";

//    //                   var line = command.ExecuteNonQuery();
//    //                   results = command.ExecuteReader();

//    //                   foreach (System.Data.Common.DbDataRecord aa in results)
//    //                   {

//    //                       //Console.WriteLine($"{results["PREF_CD"]} ,{STR("PREF_NAME")} ,{STR("PREF_CUSTOMER")} ,{STR("PREF_MATHINE")} ,{STR("PREF_YOTEI")},{results["PREF_DATE"]},{STR("PREF_PLUS")}");

//    //                       //string text = System.Text.Encoding.GetEncoding("shift_jis").GetString((byte[])(results["PREF_CD"]));
//    //                       Console.WriteLine($"");

//    //                       Console.WriteLine($"PREF_CD,PREF_NAME,PREF_RL,PREF_YOTEI,PREF_HYOU,PREF_DATE,PREF_PLUS");
//    //                       Console.WriteLine($"{aa["PREF_CD"]},{STR(aa, "PREF_NAME")},{STR(aa, "PREF_RL")},{STR(aa, "PREF_YOTEI")},{STR(aa, "PREF_HYOU")},{aa["PREF_DATE"]},{STR(aa, "PREF_PLUS")}");

//    //                       //Console.WriteLine($"{results["PREF_CD"]} {STR("PREF_NAME")} {STR("PREF_CUSTOMER")}  {STR("PREF_MATHINE")} {STR("PREF_YOTEI")}");
//    //                       //                        Console.WriteLine($"{StringUtil.SystemByteArraySJIStoString(results["PREF_NAME"])}");
//    //                       //                      Console.WriteLine($"{StringUtil.SystemByteArraySJIStoString(results["PREF_CUSTOMER"])}");
//    //                       //  Console.WriteLine($"");
//    //                       //    Console.WriteLine($"{StringUtil.SystemByteArraySJIStoString(results["PREF_YOTEI"])}");
//    //                       Console.WriteLine($"---------------------------------------\n");
//    //                   }


//    //               }

//    //           }
//    //           catch (Exception e)
//    //           {
//    //               Console.WriteLine(e.Message);
//    //           }

//    //           Console.ReadKey();
//    //       }
//    //       public static string STR(System.Data.Common.DbDataRecord dr, string KEY)
//    //       {
//    //           return StringUtil.SystemByteArraySJIStoString(dr[KEY]);
//    //       }

//    //       public static string STR(string KEY)
//    //       {
//    //           return StringUtil.SystemByteArraySJIStoString(results[KEY]);
//    //       }

//    //       /// <summary>
//    //       /// 文字コードを判別する
//    //       /// </summary>
//    //       /// <remarks>
//    //       /// Jcode.pmのgetcodeメソッドを移植したものです。
//    //       /// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
//    //       /// Jcode.pmの著作権情報
//    //       /// Copyright 1999-2005 Dan Kogai <dankogai@dan.co.jp>
//    //       /// This library is free software; you can redistribute it and/or modify it
//    //       ///  under the same terms as Perl itself.
//    //       /// </remarks>
//    //       /// <param name="bytes">文字コードを調べるデータ</param>
//    //       /// <returns>適当と思われるEncodingオブジェクト。
//    //       /// 判断できなかった時はnull。</returns>
//    //       public static System.Text.Encoding GetCode(byte[] bytes)
//    //       {
//    //           const byte bEscape = 0x1B;
//    //           const byte bAt = 0x40;
//    //           const byte bDollar = 0x24;
//    //           const byte bAnd = 0x26;
//    //           const byte bOpen = 0x28;    //'('
//    //           const byte bB = 0x42;
//    //           const byte bD = 0x44;
//    //           const byte bJ = 0x4A;
//    //           const byte bI = 0x49;

//    //           int len = bytes.Length;
//    //           byte b1, b2, b3, b4;

//    //           //Encode::is_utf8 は無視

//    //           bool isBinary = false;
//    //           for (int i = 0; i < len; i++)
//    //           {
//    //               b1 = bytes[i];
//    //               if (b1 <= 0x06 || b1 == 0x7F || b1 == 0xFF)
//    //               {
//    //                   //'binary'
//    //                   isBinary = true;
//    //                   if (b1 == 0x00 && i < len - 1 && bytes[i + 1] <= 0x7F)
//    //                   {
//    //                       //smells like raw unicode
//    //                       return System.Text.Encoding.Unicode;
//    //                   }
//    //               }
//    //           }
//    //           if (isBinary)
//    //           {
//    //               return null;
//    //           }

//    //           //not Japanese
//    //           bool notJapanese = true;
//    //           for (int i = 0; i < len; i++)
//    //           {
//    //               b1 = bytes[i];
//    //               if (b1 == bEscape || 0x80 <= b1)
//    //               {
//    //                   notJapanese = false;
//    //                   break;
//    //               }
//    //           }
//    //           if (notJapanese)
//    //           {
//    //               return System.Text.Encoding.ASCII;
//    //           }

//    //           for (int i = 0; i < len - 2; i++)
//    //           {
//    //               b1 = bytes[i];
//    //               b2 = bytes[i + 1];
//    //               b3 = bytes[i + 2];

//    //               if (b1 == bEscape)
//    //               {
//    //                   if (b2 == bDollar && b3 == bAt)
//    //                   {
//    //                       //JIS_0208 1978
//    //                       //JIS
//    //                       return System.Text.Encoding.GetEncoding(50220);
//    //                   }
//    //                   else if (b2 == bDollar && b3 == bB)
//    //                   {
//    //                       //JIS_0208 1983
//    //                       //JIS
//    //                       return System.Text.Encoding.GetEncoding(50220);
//    //                   }
//    //                   else if (b2 == bOpen && (b3 == bB || b3 == bJ))
//    //                   {
//    //                       //JIS_ASC
//    //                       //JIS
//    //                       return System.Text.Encoding.GetEncoding(50220);
//    //                   }
//    //                   else if (b2 == bOpen && b3 == bI)
//    //                   {
//    //                       //JIS_KANA
//    //                       //JIS
//    //                       return System.Text.Encoding.GetEncoding(50220);
//    //                   }
//    //                   if (i < len - 3)
//    //                   {
//    //                       b4 = bytes[i + 3];
//    //                       if (b2 == bDollar && b3 == bOpen && b4 == bD)
//    //                       {
//    //                           //JIS_0212
//    //                           //JIS
//    //                           return System.Text.Encoding.GetEncoding(50220);
//    //                       }
//    //                       if (i < len - 5 &&
//    //                           b2 == bAnd && b3 == bAt && b4 == bEscape &&
//    //                           bytes[i + 4] == bDollar && bytes[i + 5] == bB)
//    //                       {
//    //                           //JIS_0208 1990
//    //                           //JIS
//    //                           return System.Text.Encoding.GetEncoding(50220);
//    //                       }
//    //                   }
//    //               }
//    //           }

//    //           //should be euc|sjis|utf8
//    //           //use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
//    //           int sjis = 0;
//    //           int euc = 0;
//    //           int utf8 = 0;
//    //           for (int i = 0; i < len - 1; i++)
//    //           {
//    //               b1 = bytes[i];
//    //               b2 = bytes[i + 1];
//    //               if (((0x81 <= b1 && b1 <= 0x9F) || (0xE0 <= b1 && b1 <= 0xFC)) &&
//    //                   ((0x40 <= b2 && b2 <= 0x7E) || (0x80 <= b2 && b2 <= 0xFC)))
//    //               {
//    //                   //SJIS_C
//    //                   sjis += 2;
//    //                   i++;
//    //               }
//    //           }
//    //           for (int i = 0; i < len - 1; i++)
//    //           {
//    //               b1 = bytes[i];
//    //               b2 = bytes[i + 1];
//    //               if (((0xA1 <= b1 && b1 <= 0xFE) && (0xA1 <= b2 && b2 <= 0xFE)) ||
//    //                   (b1 == 0x8E && (0xA1 <= b2 && b2 <= 0xDF)))
//    //               {
//    //                   //EUC_C
//    //                   //EUC_KANA
//    //                   euc += 2;
//    //                   i++;
//    //               }
//    //               else if (i < len - 2)
//    //               {
//    //                   b3 = bytes[i + 2];
//    //                   if (b1 == 0x8F && (0xA1 <= b2 && b2 <= 0xFE) &&
//    //                       (0xA1 <= b3 && b3 <= 0xFE))
//    //                   {
//    //                       //EUC_0212
//    //                       euc += 3;
//    //                       i += 2;
//    //                   }
//    //               }
//    //           }
//    //           for (int i = 0; i < len - 1; i++)
//    //           {
//    //               b1 = bytes[i];
//    //               b2 = bytes[i + 1];
//    //               if ((0xC0 <= b1 && b1 <= 0xDF) && (0x80 <= b2 && b2 <= 0xBF))
//    //               {
//    //                   //UTF8
//    //                   utf8 += 2;
//    //                   i++;
//    //               }
//    //               else if (i < len - 2)
//    //               {
//    //                   b3 = bytes[i + 2];
//    //                   if ((0xE0 <= b1 && b1 <= 0xEF) && (0x80 <= b2 && b2 <= 0xBF) &&
//    //                       (0x80 <= b3 && b3 <= 0xBF))
//    //                   {
//    //                       //UTF8
//    //                       utf8 += 3;
//    //                       i += 2;
//    //                   }
//    //               }
//    //           }
//    //           //M. Takahashi's suggestion
//    //           //utf8 += utf8 / 2;

//    //           System.Diagnostics.Debug.WriteLine(
//    //               string.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8));
//    //           if (euc > sjis && euc > utf8)
//    //           {
//    //               //EUC
//    //               return System.Text.Encoding.GetEncoding(51932);
//    //           }
//    //           else if (sjis > euc && sjis > utf8)
//    //           {
//    //               //SJIS
//    //               return System.Text.Encoding.GetEncoding(932);
//    //           }
//    //           else if (utf8 > euc && utf8 > sjis)
//    //           {
//    //               //UTF8
//    //               return System.Text.Encoding.UTF8;
//    //           }

//    //           return null;
//    //       }
//    //   }
//}
