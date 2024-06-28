//using Microsoft.VisualStudio.OLE.Interop;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public static partial class TextFile
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="path"></param>
//        /// <param name="lines"></param>
//        /// <param name="encoding"></param>
//        /// <returns></returns>
//        public static string Tail(string path, int lines = 1, string encoding = "UTF-8")
//        {
//            int BUFFER_SIZE = 32;       // バッファーサイズ(あえて小さく設定)
//            int offset = 0;
//            int loc = 0;
//            int foundCount = 0;
//            var buffer = new byte[BUFFER_SIZE];
//            bool isFirst = true;
//            bool isFound = false;

//            // ファイル共有モードで開く
//            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
//            {
//                // 検索ブロック位置の繰り返し
//                for (int i = 0; ; i++)
//                {
//                    // ブロック開始位置に移動
//                    offset = Math.Min((int)fs.Length, (i + 1) * BUFFER_SIZE);
//                    loc = 0;
//                    if (fs.Length <= i * BUFFER_SIZE)
//                    {
//                        // ファイルの先頭まで達した場合
//                        if (foundCount > 0 || fs.Length > 0) break;

//                        // 行が未存在
//                        throw new ArgumentOutOfRangeException("NOT FOUND DATA");
//                    }

//                    fs.Seek(-offset, SeekOrigin.End);

//                    // ブロックの読み込み
//                    int readLength = offset - BUFFER_SIZE * i;
//                    for (int j = 0; j < readLength; j += fs.Read(buffer, j, readLength - j)) ;

//                    // ブロック内の改行コードの検索
//                    for (int k = readLength - 1; k >= 0; k--)
//                    {
//                        if (buffer[k] == 0x0A)
//                        {
//                            if (isFirst && k == readLength - 1) continue;
//                            if (++foundCount == lines)
//                            {
//                                // 所定の行数が見つかった場合
//                                loc = k + 1;
//                                isFound = true;
//                                break;
//                            }
//                        }
//                    }
//                    isFirst = false;
//                    if (isFound) break;
//                }

//                // 見つかった場合
//                fs.Seek(-offset + loc, SeekOrigin.End);

//                using (var sr = new StreamReader(fs, Encoding.GetEncoding(encoding)))
//                {
//                    return sr.ReadToEnd();
//                }
//            }
//        }

//    }

//    /// <summary>
//    /// テキスト式設定ファイルを操作するクラス
//    /// </summary>
//    public class ConfigFile
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public string FilePath;

//        /// <summary>
//        ///  ファイルの内容を格納する文字列配列
//        /// </summary>
//        public string[] lines;
//        Encoding enc;

//        /// <summary>
//        /// ファイルを開いて文字列配列に格納
//        /// </summary>
//        /// <param name="FilePath">開くファイルのフルパス</param>
//        [System.Diagnostics.DebuggerStepThrough]
//        public ConfigFile(string FilePath)
//        {
//            this.FilePath = FilePath;

//            enc = TextFile.DetectEncodingFromBOM(FilePath);
//            lines = File.ReadAllLines(FilePath, enc);
//        }

//        /// <summary>
//        /// ファイルを保存して閉じる
//        /// </summary>
//        public void SaveAndClose()
//        {
//            File.WriteAllLines(FilePath, lines, enc);
//        }

//        /// <summary>
//        /// 指定した文字列を末尾に追加して閉じる
//        /// </summary>
//        /// <param name="AddLines">追加する文字列</param>
//        public void AppenddAndClose(IEnumerable<string> AddLines)
//        {
//            File.AppendAllLines(FilePath, AddLines);
//        }

//        /// <summary>
//        /// 指定した文字列が含まれる最初の行を返す
//        /// </summary>
//        /// <param name="SearchString">検索文字列</param>
//        /// <returns>見つかった最初の行番号</returns>
//        public int FindFirstLine(string SearchString)
//        {

//            for (int i = 0; i < lines.GetLength(0); i++)
//            {

//                    if (System.Text.RegularExpressions.Regex.IsMatch(lines[i], SearchString, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
//                    {
//                        Console.WriteLine($"一致した内容：{lines[i]}");
//                    return i;
//                }
//            }
//            return -1;
//        }

//        /// <summary>
//        /// ファイル内のパターンに一致した文字列を置き換える
//        /// </summary>
//        /// <param name="sPattern">検索パターン</param>
//        /// <param name="repraceStr">置き換える文字列</param>
//        /// <returns>成功:True</returns>
//        public bool FindFirstAndReplaceLine(string sPattern, string repraceStr)
//        {
//            try
//            {

//                for (int i = 0; i < lines.Length; i++)
//                {
//                    if (System.Text.RegularExpressions.Regex.IsMatch(lines[i], sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
//                    {
//                        Console.WriteLine($"一致した内容：{lines[i]}");
//                        lines[i] = repraceStr;
//                        return true;
//                    }
//                }
//                Array.Resize(ref lines, lines.Length + 1);
//                lines[lines.Length - 1] = repraceStr;
//                return true;
//            }
//            catch (IOException ioe)
//            {
//                Console.WriteLine($"{ioe.Message}");
//                return false;
//            }
//}

//        /// <summary>
//        /// 指定した文字列が存在する行を削除
//        /// </summary>
//        /// <param name="sPattern"></param>
//        /// <returns>成功:True</returns>
//        public bool FindFirstAndDeleteLine(string sPattern)
//        {
//            try
//            {
//                bool ans = false;
//                string[] newLines=new string[lines.Length];
//                List<string> dist = new List<string>() { };

//                for (int i = 0; i < lines.Length; i++)
//                {
//                    string str = lines[i];
//                    if (str != null)
//                    {
//                        if (System.Text.RegularExpressions.Regex.IsMatch(str, sPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
//                        {
//                            ans = true;
//                        }
//                        else
//                        {
//                            dist.Add(lines[i]);
//                        }
//                    }
//                }

//                lines = dist.ToArray();
//                return ans;
//            }
//            catch (IOException ioe)
//            {
//                Console.WriteLine($"{ioe.Message}");
//                return false;
//            }
//        }
//    }

//    public static class WshTest
//    {
//        // https://answers.microsoft.com/en-us/windows/forum/all/how-to-create-a-desktop-shortcut-in-c-visual/9eb48ee4-d672-4025-8cf0-bddbe19c05ac

//        public static void createShortcut()
//        {
//            IShellLink link = (IShellLink)new ShellLink();

//            // setup shortcut information
//            link.SetDescription("This is the description when hovered over.");
//            link.SetPath(@"C:\LoactiontoYourFile\YourProgram.exe");

//            // save it
//            IPersistFile file = (IPersistFile)link;
//            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
//            file.Save(Path.Combine(desktopPath, "Shortcut Display Name.lnk"), 0);
//        }


//        //This could always be better adjusted to take more input instead of just being set.
//        //Then outside of your class but in your namespace
//        [ComImport]
//        [Guid("00021401-0000-0000-C000-000000000046")]
//        internal class ShellLink
//        {
//        }

//        [ComImport]
//        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
//        [Guid("000214F9-0000-0000-C000-000000000046")]
//        internal interface IShellLink
//        {
//            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
//            void GetIDList(out IntPtr ppidl);
//            void SetIDList(IntPtr pidl);
//            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
//            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
//            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
//            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
//            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
//            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
//            void GetHotkey(out short pwHotkey);
//            void SetHotkey(short wHotkey);
//            void GetShowCmd(out int piShowCmd);
//            void SetShowCmd(int iShowCmd);
//            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
//            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
//            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
//            void Resolve(IntPtr hwnd, int fFlags);
//            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
//        }

//    }
//}
