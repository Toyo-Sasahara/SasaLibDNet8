using System;
using System.Text.RegularExpressions;

namespace SasaLibDNet8
{
    public static class StringParsing
    {
        /// <summary>
        ///  [keyword]:"2022/08/17 17:57:45" を含む文字列の中から、引用符に囲まれた文字列を抽出する。引用符の中に引用符があるものは非対応。同じキー
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string SpecifiedStringExtraction(string source, string keyword, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = Console.WriteLine;

            try
            {
                MatchCollection matches = Regex.Matches(source, @"\[" + keyword + @"\]:(\""(?:[^\""]+|\""\"")*\""|[^,]*)");
                if (matches.Count == 1)
                {
                    if (string.IsNullOrWhiteSpace(matches[0].Value) == false)
                    {
                        string str1 = matches[0].Value;
                        string str2 = str1.Replace(@"[" + keyword + @"]:", "");
                        string result = str2.TrimStart('"').TrimEnd('"');

                        if (string.IsNullOrWhiteSpace(result) == false)
                        {
                            // 抽出OK
                            return result;
                        }
                        else
                        {
                            WriteLine($"※抽出失敗:str1 = \"{str1}\" , \"{str2}\" result = \"{result}\"");
                            return null;
                        }
                    }
                    else
                    {
                        WriteLine("※抽出失敗:見つかりません");
                        return null;
                    }
                }
                else if (matches.Count > 1)
                {
                    WriteLine("※抽出失敗:同じkeywordが文字列中に2回以上出現");
                    return null;
                }
                else
                {
                    WriteLine("※抽出失敗:見つかりません matches.Count = 0");
                    return null;
                }
            }
            catch (Exception ex)
            {
                WriteLine($"※抽出失敗:{ex.Message}");
                return null;
            }
        }

        public static void SpecifiedStringExtraction_Test()
        {
            string input = @"[#5.OK SourceFile]:""C:\ProgramData\TOYOCOMMON\Inventor\ContentCenter24.DAT"" [CTime]:""2022/08/20 11:25:04"" [MTime]:""2022/08/19 10:25:04"" [ABCD]:""2022/08/19 ""XXX"" [A123]:""2022/08/19 \\""XXX\\"" 10:25:04"" Next Send Target Data Size.";


            string output1 = SpecifiedStringExtraction(input, "CTime");
            string output2 = SpecifiedStringExtraction(input, "MTime");
            string output3 = SpecifiedStringExtraction(input, "ABCD");
            string output4 = SpecifiedStringExtraction(input, "#5.OK SourceFile");
            string output5 = SpecifiedStringExtraction(input, "ABCDE");


            Console.WriteLine($"output1 = \"{output1}\"");
            Console.WriteLine($"output2 = \"{output2}\"");
            Console.WriteLine($"output3 = \"{output3}\"");
            Console.WriteLine($"output4 = \"{output4}\"");
            Console.WriteLine($"output5 = \"{output5}\"");
            /*
             * 出力結果
output1 = "2022/08/20 11:25:04"
output2 = "2022/08/19 10:25:04"
output3 = "2022/08/19 " ※二重引用符には非対応
output4 = "C:\ProgramData\TOYOCOMMON\Inventor\ContentCenter24.DAT"
output5 = ""
             */

        }
    }
}
