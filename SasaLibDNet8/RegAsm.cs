using System;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace SasaLib
{
    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class RegAsm
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FindRegAsmX64v4Path()
        {

            try
            {
               var result = FileFolder.GetFilesMostDeep(@"C:\Windows\Microsoft.NET", "REGASM.EXE");

                Array.Sort(result);

                string rr = @"\\Windows\\Microsoft.NET\\Framework64\\v4";

                int num = Array.FindIndex(result, s => Regex.IsMatch(s, rr, RegexOptions.IgnoreCase));

                return result[num];

            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"例外発生 SasaLib.RegAsm.FindRegAsmX64v4Path(..) {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string FindRegAsmX64v2Path()
        {

            try
            {
                var result = FileFolder.GetFilesMostDeep(@"C:\Windows\Microsoft.NET", "REGASM.EXE");

                Array.Sort(result);

                string rr = @"\\Windows\\Microsoft.NET\\Framework64\\v2";

                int num = Array.FindIndex(result, s => Regex.IsMatch(s, rr, RegexOptions.IgnoreCase));

                return result[num];

            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"例外発生 SasaLib.RegAsm.FindRegAsmX64v2Path(..) {ex.Message}");
                return null;
            }
        }

        static string FindRegAsm(string searchString = @"Framework64\\v4")
        {

            try
            {
                var result = FileFolder.GetFilesMostDeep(@"C:\Windows\Microsoft.NET", "REGASM.EXE");

                Array.Sort(result);

                string rr = @"\\Windows\\Microsoft.NET\\" + searchString;

                int num = Array.FindIndex(result, s => Regex.IsMatch(s, rr, RegexOptions.IgnoreCase));

                return result[num];

            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"例外発生 SasaLib.RegAsm.FindRegAsm(..) {ex.Message}");
                return null;
            }
        }
    }
}
