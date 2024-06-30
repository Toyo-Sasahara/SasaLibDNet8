//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Runtime.Versioning;
//using System.Text;
//using System.Threading.Tasks;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class Csv
//    {
//        /// <summary>
//        /// CSV形式の文字列をArrayListに変換 
//        /// </summary>
//        /// <param name="csvText">CSVの内容が入ったString 例　string st =　
//        ///     "A,1,2" + "\x0a" +
//        ///     "B,2,3" + "\x0a" +
//        ///     "C,4,5" + "\x0a" +
//        ///     "D,6,7" + "\x0a"; </param>
//        /// <param name="Qswitch"></param>
//        /// <returns>変換結果のArrayList</returns>
//        /// <exception cref="ApplicationException"></exception>
//        public static System.Collections.ArrayList CsvStringToArrayList(string csvText, bool Qswitch = true)
//        {
//            //前後の改行を削除しておく
//            csvText = csvText.Trim(new char[] { '\r', '\n' });

//            System.Collections.ArrayList csvRecords =
//                new System.Collections.ArrayList();
//            System.Collections.ArrayList csvFields =
//                new System.Collections.ArrayList();

//            int csvTextLength = csvText.Length;
//            int startPos = 0, endPos;
//            string field = "";

//            while (true)
//            {
//                //空白を飛ばす
//                while (startPos < csvTextLength &&
//                    (csvText[startPos] == ' ' || csvText[startPos] == '\t'))
//                {
//                    startPos++;
//                }

//                //データの最後の位置を取得
//                if (startPos < csvTextLength && csvText[startPos] == '"')
//                {
//                    //"で囲まれているとき
//                    //最後の"を探す
//                    endPos = startPos;
//                    while (true)
//                    {
//                        endPos = csvText.IndexOf('"', endPos + 1);
//                        if (endPos < 0)
//                        {
//                            throw new ApplicationException("\"が不正");
//                        }
//                        //"が2つ続かない時は終了
//                        if (endPos + 1 == csvTextLength || csvText[endPos + 1] != '"')
//                        {
//                            break;
//                        }
//                        //"が2つ続く
//                        endPos++;
//                    }

//                    //一つのフィールドを取り出す
//                    field = csvText.Substring(startPos, endPos - startPos + 1);
//                    if (Qswitch == true)
//                    {
//                        //""を"にする
//                        field = field.Substring(1, field.Length - 2).Replace("\"\"", "\"");
//                    }
//                    endPos++;
//                    //空白を飛ばす
//                    while (endPos < csvTextLength &&
//                        csvText[endPos] != ',' && csvText[endPos] != '\n')
//                    {
//                        endPos++;
//                    }
//                }
//                else
//                {
//                    //"で囲まれていない
//                    //カンマか改行の位置
//                    endPos = startPos;
//                    while (endPos < csvTextLength &&
//                        csvText[endPos] != ',' && csvText[endPos] != '\n')
//                    {
//                        endPos++;
//                    }

//                    //一つのフィールドを取り出す
//                    field = csvText.Substring(startPos, endPos - startPos);
//                    //後の空白を削除
//                    field = field.TrimEnd();
//                }

//                //フィールドの追加
//                csvFields.Add(field);

//                //行の終了か調べる
//                if (endPos >= csvTextLength || csvText[endPos] == '\n')
//                {
//                    //行の終了
//                    //レコードの追加
//                    csvFields.TrimToSize();
//                    csvRecords.Add(csvFields);
//                    csvFields = new System.Collections.ArrayList(
//                        csvFields.Count);

//                    if (endPos >= csvTextLength)
//                    {
//                        //終了
//                        break;
//                    }
//                }

//                //次のデータの開始位置
//                startPos = endPos + 1;
//            }

//            csvRecords.TrimToSize();
//            return csvRecords;
//        }


//        /// <summary>
//        /// CSVファイルを指定した文字エンコードでArrayListに格納する。
//        /// </summary>
//        /// <param name="csvFileFullPath"></param>
//        /// <param name="Encordstring"></param>
//        /// <returns></returns>
//        public static System.Collections.ArrayList CsvFileToArrayList(string csvFileFullPath, string Encordstring = "Shift_JIS")
//        {
//            try
//            {
//                using (FileStream stream = new FileStream(csvFileFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
//                using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.GetEncoding(Encordstring)))
//                {
//                    System.Collections.ArrayList arrayList = new System.Collections.ArrayList();
//                    string line = sr.ReadToEnd();
//                    arrayList = CsvStringToArrayList(line, false);
//                    if (arrayList.Count < 1)
//                    {
//                        return null;
//                    }
//                    return arrayList;
//                }
//            }
//            catch (IOException ioe)
//            {
//                Eventlog.Log.WriteEntry("SasaLib CSV Class", EventLogEntryType.Error, 0, $"▲Read(...),失敗,IOException={ioe.Message}");

//                return null;
//            }
//        }

//        /// <summary>
//        /// ■ストリームからCSVをArryListに保存
//        /// </summary>
//        /// <param name="sr"></param>
//        /// <returns></returns>
//        public static System.Collections.ArrayList Read(StreamReader sr)
//        {
//            try
//            {
//                // csvファイルを開く
//                // ストリームの末尾まで繰り返す
//                // ファイルから
//                System.Collections.ArrayList Datas = new System.Collections.ArrayList();
//                string line = sr.ReadToEnd();
//                Datas = CsvStringToArrayList(line);
//                return Datas;
//            }
//            catch (System.Exception e)
//            {
//                // ファイルを開くのに失敗したとき
//                Eventlog.Log.WriteEntry("SasaLib CSV Class", EventLogEntryType.Error, 0, $"▲Read(...),失敗,Exception={e.Message}");

//                return null;
//            }
//        }

//        /// <summary>
//        /// ■ArrayList csvdataの1行目をヘッダーとし、指定したkey文字列が出現する最初のカラム番号を返す
//        /// </summary>
//        /// <param name="csvdata"></param>
//        /// <param name="key"></param>
//        /// <param name="ignoreCase"></param>
//        /// <returns></returns>
//        public static int GetFromCsvArrayList(ArrayList csvdata, string key, bool ignoreCase = false)
//        {
//            var headder = (ArrayList)csvdata[0];


//            int i = 0;
//            for (; i < headder.Count; i++)
//            {
//                if (ignoreCase)
//                {
//                    if (((string)headder[i]).ToLower() == key.ToLower())
//                    {
//                        return i;
//                    }
//                }
//                else
//                {
//                    if ((string)headder[i] == key)
//                    {
//                        return i;
//                    }
//                }
//            }


//            return -1;
//        }

//        /// <summary>
//        /// ■1行目 がヘッダーであるCSV形式のArrayListからキー名keyを指定し、lineで指定した0から始まる行番号においてキーと一致する文字列を返す
//        /// </summary>
//        /// <param name="csvdata">CSVデータ</param>
//        /// <param name="key">検索キー</param>
//        /// <param name="line">検索結果</param>
//        /// <param name="ignoreCase"></param>
//        /// <returns></returns>
//        /// <exception cref="FormatException"></exception>
//        public static string GetFromCsvArrayList(ArrayList csvdata, string key, int line, bool ignoreCase = false)
//        {
//            var headder = (ArrayList)csvdata[0];
//            var data = (ArrayList)csvdata[line];

//            if (headder.Count != data.Count)
//            {
//                throw new FormatException("ArrayList csvdata の1行目ヘッダーと、指定行lineのデータ数が一致しません");
//            }

//            int i = 0;
//            for (; i < headder.Count; i++)
//            {
//                if (ignoreCase)
//                {
//                    if (((string)headder[i]).ToLower() == key.ToLower())
//                    {
//                        string ans = (string)data[i];

//                        return ans;
//                    }
//                }
//                else
//                {
//                    if ((string)headder[i] == key)
//                    {
//                        string ans = (string)data[i];

//                        return ans;
//                    }
//                }
//            }


//            return null;
//        }

//        /// <summary>
//        /// ■1行目 がヘッダーであるCSV形式のArrayListからキー名keyを指定し、lineで指定した0から始まる行番号において
//        /// キーと一致するカラムの文字列をセット
//        /// </summary>
//        /// <param name="csvdata">CSVデータ</param>
//        /// <param name="key">キー名を指定</param>
//        /// <param name="value">値</param>
//        /// <param name="line">値を格納するデータ列 ０から始まる行番号</param>
//        /// <param name="ignoreCase">大文字小文字を無視</param>
//        public static void SetToCsvArrayList(ref ArrayList csvdata, string key, string value, int line, bool ignoreCase = false)
//        {
//            var headder = (ArrayList)csvdata[0];
//            var data = (ArrayList)csvdata[line];

//            if (headder.Count != data.Count)
//            {
//                throw new FormatException("ArrayList csvdata の1行目ヘッダーと、指定行lineのデータ数が一致しません");
//            }

//            int i = 0;
//            for (; i < headder.Count; i++)
//            {
//                if (ignoreCase)
//                {
//                    if (((string)headder[i]).ToLower() == key.ToLower())
//                    {
//                        break;
//                    }
//                }
//                else
//                {
//                    if ((string)headder[i] == key)
//                    {
//                        break;
//                    }
//                }
//            }

//            ((ArrayList)csvdata[line])[i] = value;

//        }

//        /// <summary>
//        /// ■CSVArrayListをファイルに保存
//        /// </summary>
//        /// <param name="CsvStringToArrayList"> CSVデータ
//        ///     "A,1,2" + "\x0a" +
//        ///     "B,2,3" + "\x0a" +
//        ///     "C,4,5" + "\x0a" +
//        ///     "D,6,7" + "\x0a";
//        /// </param>
//        /// <param name="filePath"></param>
//        /// <param name="Encordstring"></param>
//        /// <returns></returns>
//        public static bool CsvArrayListToSave(System.Collections.ArrayList CsvStringToArrayList, string filePath, string Encordstring = "Shift_JIS")
//        {

//            Encoding Enc = Encoding.GetEncoding(Encordstring);


//            string csvStr = "";

//            foreach (Object a in CsvStringToArrayList)
//            {
//                foreach (var b in (IEnumerable)a)
//                {
//                    //Console.WriteLine(b);
//                    csvStr += $"{b},";
//                }
//                csvStr = csvStr.TrimEnd(',') + Environment.NewLine;
//            }

//            using (StreamWriter writer = new StreamWriter(filePath, false, Enc))
//            {
//                //ファイルにテキストを書き込む
//                writer.WriteLine(csvStr);

//            } // （3）usingブロックを抜けるときにファイルが閉じられる

//            return false;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="CsvStringToArrayList"></param>
//        /// <returns></returns>
//        public static string CsvArrayListToString(System.Collections.ArrayList CsvStringToArrayList)
//        {
//            string headder = "";

//            foreach (var b in (IEnumerable)CsvStringToArrayList)
//            {
//                headder += $"{b},";
//            }
//            headder = headder.TrimEnd(',');

//            return headder;

//        }

//    }
//}
