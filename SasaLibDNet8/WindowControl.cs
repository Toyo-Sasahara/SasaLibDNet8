using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace SasaLibDNet8
{
    /// https://tech.sanwasystem.com/entry/2015/11/25/171004

    /// <summary>
    /// 
    /// </summary>
    public class WindowControl
    {
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int MK_LBUTTON = 0x0001;
        public static int GWL_STYLE = -16;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hWnd, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);


        static void Main(string[] args)
        {
            // 電卓のトップウィンドウのウィンドウハンドル（※見つかることを前提としている）
            var mainWindowHandle = Process.GetProcessesByName("calc")[0].MainWindowHandle;

            // 対象のボタンを探す
            var hWnd = FindTargetButton(GetWindow(mainWindowHandle));

            // マウスを押して放す
            SendMessage(hWnd, WM_LBUTTONDOWN, MK_LBUTTON, 0x000A000A);
            SendMessage(hWnd, WM_LBUTTONUP, 0x00000000, 0x000A000A);
        }

        // 全てのボタンを列挙し、その10番目のボタンのウィンドウハンドルを返す
        public static IntPtr FindTargetButton(WindowParams top)
        {
            var all = GetAllChildWindows(top, new List<WindowParams>());
            return all.Where(x => x.ClassName == "Button").Skip(9).First().hWnd;
        }


        // 指定したウィンドウの全ての子孫ウィンドウを取得し、リストに追加する
        public static List<WindowParams> GetAllChildWindows(WindowParams parent, List<WindowParams> dest)
        {
            dest.Add(parent);
            EnumChildWindows(parent.hWnd).ToList().ForEach(x => GetAllChildWindows(x, dest));
            return dest;
        }

        // 与えた親ウィンドウの直下にある子ウィンドウを列挙する（孫ウィンドウは見つけてくれない）
        public static IEnumerable<WindowParams> EnumChildWindows(IntPtr hParentWindow)
        {
            IntPtr hWnd = IntPtr.Zero;
            while ((hWnd = FindWindowEx(hParentWindow, hWnd, null, null)) != IntPtr.Zero) { yield return GetWindow(hWnd); }
        }

        // ウィンドウハンドルを渡すと、ウィンドウテキスト（ラベルなど）、クラス、スタイルを取得してWindowsクラスに格納して返す
        public static WindowParams GetWindow(IntPtr hWnd)
        {
            int textLen = GetWindowTextLength(hWnd);
            string windowText = null;
            if (0 < textLen)
            {
                //ウィンドウのタイトルを取得する
                StringBuilder windowTextBuffer = new StringBuilder(textLen + 1);
                GetWindowText(hWnd, windowTextBuffer, windowTextBuffer.Capacity);
                windowText = windowTextBuffer.ToString();
            }

            //ウィンドウのクラス名を取得する
            StringBuilder classNameBuffer = new StringBuilder(256);
            GetClassName(hWnd, classNameBuffer, classNameBuffer.Capacity);

            // スタイルを取得する
            int style = GetWindowLong(hWnd, GWL_STYLE);
            return new WindowParams() { hWnd = hWnd, Title = windowText, ClassName = classNameBuffer.ToString(), Style = style };
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public Point TestGetWiindowRect(IntPtr handle)
        {

            RECT rect;
            bool flag = GetWindowRect(handle, out rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            Console.WriteLine("height:{0} width:{1}", height, width);

            return new Point(width, height);
        }

        public IWin32Window GetWindowFromHost(int hwnd)
        {
            IWin32Window window = null;
            IntPtr handle = new IntPtr(hwnd);

            try
            {
                NativeWindow nativeWindow = new NativeWindow();
                nativeWindow.AssignHandle(handle);
                window = nativeWindow;
            }
            finally
            {
                handle = IntPtr.Zero;
            }

            return window;
        }

    }

    public class WindowParams
    {
        public string ClassName;
        public string Title;
        public IntPtr hWnd;
        public int Style;
    }

    // エラー有。開発中
    //public static class TestProgram
    //{
    //    public const int WM_LBUTTONDOWN = 0x201;
    //    public const int WM_LBUTTONUP = 0x202;
    //    public const int MK_LBUTTON = 0x0001;

    //    [DllImport("user32.dll")]
    //    public static extern int SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

    //    static void TestMain(string[] args)
    //    {
    //        SasaLib.WindowControl windowControl = new SasaLib.WindowControl();

    //        // 電卓のトップウィンドウのウィンドウハンドル（※見つかることを前提としている）
    //        var mainWindowHandle = Process.GetProcessesByName("calc")[0].MainWindowHandle;

    //        Window window = new Window();
    //        window.hWnd = mainWindowHandle;

    //        // 対象のボタンを探す（これでボタンのハンドルが取得できる）
    //        var hWnd = windowControl.FindTargetButton(window);

    //        // マウスを押してから放す
    //        SendMessage(hWnd, WM_LBUTTONDOWN, MK_LBUTTON, 0x000A000A);
    //        SendMessage(hWnd, WM_LBUTTONUP, 0x00000000, 0x000A000A);
    //    }
    //}
}
