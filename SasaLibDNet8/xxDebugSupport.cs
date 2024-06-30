//using System;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;

//namespace SasaLibDNet8
//{
//    // .Netのデバッグサポート系
//    public static class DebugSupport
//    {
//        /// <summary>
//        /// 呼び出し元を表示するサンプル。TraceMeaageSampleが何処から呼び出されたかを知る。
//        /// </summary>
//        /// <param name="message"></param>
//        /// <param name="memberName"></param>
//        /// <param name="sourceFilePath"></param>
//        /// <param name="sourceLineNumber"></param>
//        public static void TraceMessageSample(string message,
//        [CallerMemberName] string memberName = "",
//        [CallerFilePath] string sourceFilePath = "",
//        [CallerLineNumber] int sourceLineNumber = 0
//            )
//        {
//            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++\n");
//            Console.WriteLine("メンバー名：" + memberName);
//            Console.WriteLine("ソースファイルパス：" + sourceFilePath);
//            Console.WriteLine("ソースファイル行番号：" + sourceLineNumber);
//            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++++++++\n");
//        }

//        //呼び出し元とか表示
//        public static void TraceMessage2(string message,
//        [CallerMemberName] string memberName = "",
//        [CallerFilePath] string sourceFilePath = "",
//        [CallerLineNumber] int sourceLineNumber = 0)
//        {
//            System.Diagnostics.Trace.WriteLine("\n");
//            System.Diagnostics.Trace.WriteLine("トレースメッセージ：" + message);
//            System.Diagnostics.Trace.WriteLine("メンバー名：" + memberName);
//            System.Diagnostics.Trace.WriteLine("ソースファイルパス：" + sourceFilePath);
//            System.Diagnostics.Trace.WriteLine("ソースファイル行番号：" + sourceLineNumber);
//            System.Diagnostics.Trace.WriteLine("\n");
//        }

//        public static void TextBarGraph(int percent)
//        {
//            // Console.CursorVisible = false;

//            char[] bars = { '／', '―', '＼', '｜' };

//            for (int i = 0; i < 100; i++)
//            {
//                // 回転する棒を表示
//                Console.Write(bars[i % 4]);

//                // 進むパーセンテージを表示
//                Console.Write("{0, 4:d0}%", i + 1);

//                // カーソル位置を初期化
//                Console.SetCursorPosition(0, Console.CursorTop);

//            }

//            Console.CursorVisible = true;
//        }

//    }

//}

//namespace SasaLibDNet8
//{
//    /// <summary>
//    /// Debug時のみコンソールにメッセージを出力させるクラス
//    /// </summary>
//    public static class DebugConsole
//    {
//        [System.Diagnostics.DebuggerStepThrough]
//        public static void WriteLine(string Message)
//        {
//            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
//            Debug.WriteLine(Message);
//            Debug.Listeners.Clear();
//        }

//        [System.Diagnostics.DebuggerStepThrough]
//        public static void Write(string Message)
//        {
//            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
//            Debug.Write(Message);
//            Debug.Listeners.Clear();
//        }

//    }

//}

