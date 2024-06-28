//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public static class WindowProcess
//    {
//        /// <summary>
//        /// Process.MainWindowTitleを使用する方法
//        /// 指定された文字列を含むウィンドウタイトルを持つプロセスを取得します。
//        /// </summary>
//        /// <param name="windowTitle">ウィンドウタイトルに含む文字列。</param>
//        /// <returns>該当するプロセスの配列。</returns>
//        public static System.Diagnostics.Process[] GetProcessesByWindowTitle(string windowTitle)
//        {
//            System.Collections.ArrayList list = new System.Collections.ArrayList();

//            //すべてのプロセスを列挙する
//            foreach (System.Diagnostics.Process p
//                in System.Diagnostics.Process.GetProcesses())
//            {
//                //指定された文字列がメインウィンドウのタイトルに含まれているか調べる
//                if (0 <= p.MainWindowTitle.IndexOf(windowTitle))
//                {
//                    //含まれていたら、コレクションに追加
//                    list.Add(p);
//                }
//            }

//            //コレクションを配列にして返す
//            return (System.Diagnostics.Process[])
//                list.ToArray(typeof(System.Diagnostics.Process));
//        }

//        /// <summary>
//        /// 指定された文字列をウィンドウのタイトルとクラス名に含んでいるプロセスを
//        /// すべて取得する。
//        /// </summary>
//        /// <param name="windowText">ウィンドウのタイトルに含むべき文字列。
//        /// nullを指定すると、classNameだけで検索する。</param>
//        /// <param name="className">ウィンドウが属するクラス名に含むべき文字列。
//        /// nullを指定すると、windowTextだけで検索する。</param>
//        /// <returns>見つかったプロセスの配列。</returns>
//        public static Process[] GetProcessesByWindow(string windowText, string className)
//        {
//            //検索の準備をする
//            foundProcesses = new ArrayList();
//            foundProcessIds = new ArrayList();
//            searchWindowText = windowText;
//            searchClassName = className;

//            //ウィンドウを列挙して、対象のプロセスを探す
//            EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);

//            //結果を返す
//            return (Process[])foundProcesses.ToArray(typeof(Process));
//        }

//        private static string searchWindowText = null;
//        private static string searchClassName = null;
//        private static ArrayList foundProcessIds = null;
//        private static ArrayList foundProcesses = null;

//        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

//        [DllImport("user32.dll")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        private extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc,
//            IntPtr lparam);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern int GetWindowText(IntPtr hWnd,
//            StringBuilder lpString, int nMaxCount);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern int GetWindowTextLength(IntPtr hWnd);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern int GetClassName(IntPtr hWnd,
//            StringBuilder lpClassName, int nMaxCount);

//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern int GetWindowThreadProcessId(
//            IntPtr hWnd, out int lpdwProcessId);

//        private static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
//        {
//            if (searchWindowText != null)
//            {
//                //ウィンドウのタイトルの長さを取得する
//                int textLen = GetWindowTextLength(hWnd);
//                if (textLen == 0)
//                {
//                    //次のウィンドウを検索
//                    return true;
//                }
//                //ウィンドウのタイトルを取得する
//                StringBuilder tsb = new StringBuilder(textLen + 1);
//                GetWindowText(hWnd, tsb, tsb.Capacity);
//                //タイトルに指定された文字列を含むか
//                if (tsb.ToString().IndexOf(searchWindowText) < 0)
//                {
//                    //含んでいない時は、次のウィンドウを検索
//                    return true;
//                }
//            }

//            if (searchClassName != null)
//            {
//                //ウィンドウのクラス名を取得する
//                StringBuilder csb = new StringBuilder(256);
//                GetClassName(hWnd, csb, csb.Capacity);
//                //クラス名に指定された文字列を含むか
//                if (csb.ToString().IndexOf(searchClassName) < 0)
//                {
//                    //含んでいない時は、次のウィンドウを検索
//                    return true;
//                }
//            }

//            //プロセスのIDを取得する
//            int processId;
//            GetWindowThreadProcessId(hWnd, out processId);
//            //今まで見つかったプロセスでは無いことを確認する
//            if (!foundProcessIds.Contains(processId))
//            {
//                foundProcessIds.Add(processId);
//                //プロセスIDをからProcessオブジェクトを作成する
//                foundProcesses.Add(Process.GetProcessById(processId));
//            }

//            //次のウィンドウを検索
//            return true;
//        }
//    }

//    /// <summary>
//    /// EnumWindowsを使用する方法
//    /// </summary>
//    public class WindowHandle2
//    {
//        private static string searchWindowText = null;
//        private static string searchClassName = null;
//        private static ArrayList foundProcessIds = null;
//        private static ArrayList foundProcesses = null;

//        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

//        [DllImport("user32.dll")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        private extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern int GetWindowTextLength(IntPtr hWnd);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern int GetWindowThreadProcessId(
//            IntPtr hWnd, out int lpdwProcessId);


//        /// <summary>
//        /// 指定された文字列をウィンドウのタイトルとクラス名に含んでいるプロセスを
//        /// すべて取得する。
//        /// </summary>
//        /// <param name="windowText">ウィンドウのタイトルに含むべき文字列。
//        /// nullを指定すると、classNameだけで検索する。</param>
//        /// <param name="className">ウィンドウが属するクラス名に含むべき文字列。
//        /// nullを指定すると、windowTextだけで検索する。</param>
//        /// <returns>見つかったプロセスの配列。</returns>
//        public Process[] GetProcessesByWindow(string windowText, string className)
//        {
//            //検索の準備をする
//            foundProcesses = new ArrayList();
//            foundProcessIds = new ArrayList();
//            searchWindowText = windowText;
//            searchClassName = className;

//            //ウィンドウを列挙して、対象のプロセスを探す
//            EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);

//            //結果を返す
//            return (Process[])foundProcesses.ToArray(typeof(Process));
//        }

//        private bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
//        {
//            if (searchWindowText != null)
//            {
//                //ウィンドウのタイトルの長さを取得する
//                int textLen = GetWindowTextLength(hWnd);
//                if (textLen == 0)
//                {
//                    //次のウィンドウを検索
//                    return true;
//                }
//                //ウィンドウのタイトルを取得する
//                StringBuilder tsb = new StringBuilder(textLen + 1);
//                GetWindowText(hWnd, tsb, tsb.Capacity);
//                //タイトルに指定された文字列を含むか
//                if (tsb.ToString().IndexOf(searchWindowText) < 0)
//                {
//                    //含んでいない時は、次のウィンドウを検索
//                    return true;
//                }
//            }

//            if (searchClassName != null)
//            {
//                //ウィンドウのクラス名を取得する
//                StringBuilder csb = new StringBuilder(256);
//                GetClassName(hWnd, csb, csb.Capacity);
//                //クラス名に指定された文字列を含むか
//                if (csb.ToString().IndexOf(searchClassName) < 0)
//                {
//                    //含んでいない時は、次のウィンドウを検索
//                    return true;
//                }
//            }

//            //プロセスのIDを取得する
//            int processId;
//            GetWindowThreadProcessId(hWnd, out processId);
//            //今まで見つかったプロセスでは無いことを確認する
//            if (!foundProcessIds.Contains(processId))
//            {
//                foundProcessIds.Add(processId);
//                //プロセスIDをからProcessオブジェクトを作成する
//                foundProcesses.Add(Process.GetProcessById(processId));
//            }

//            //次のウィンドウを検索
//            return true;
//        }
//    }

//    /// <summary>
//    /// Win32を使用する
//    /// </summary>
//    public static class WindowsHandle_Win32
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="titile"></param>
//        /// <returns></returns>
//        public static IntPtr FindWindow(string titile = "無題 - メモ帳")
//        {
//            //タイトルが"無題 - メモ帳"のウィンドウを探す
//            IntPtr hWnd = FindWindow(null, titile);
//            if (hWnd != IntPtr.Zero)
//            {
//                //ウィンドウを作成したプロセスのIDを取得する
//                int processId;
//                GetWindowThreadProcessId(hWnd, out processId);
//                //Processオブジェクトを作成する
//                Process p = Process.GetProcessById(processId);

//                Console.WriteLine("プロセス名:" + p.ProcessName);

//                return hWnd;
//            }
//            else
//            {
//                Console.WriteLine("見つかりませんでした。");

//                return hWnd;
//            }
//        }

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern IntPtr FindWindow(
//            string lpClassName, string lpWindowName);

//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern int GetWindowThreadProcessId(
//            IntPtr hWnd, out int lpdwProcessId);
//    }
//}
