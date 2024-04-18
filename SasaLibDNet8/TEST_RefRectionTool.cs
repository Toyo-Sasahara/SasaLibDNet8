using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib
{
    public static  class TEST_RefRectionTool
    {
        public static string GetMethodName()
        {
            // StackTraceを取得
            StackTrace stackTrace = new StackTrace();

            // 現在実行中のフレームを取得
            StackFrame stackFrame = stackTrace.GetFrame(1);

            // メソッド情報を取得
            MethodBase method = stackFrame.GetMethod();

            // メソッド名を取得
            string methodName = method.Name;

            return methodName;
        }
    }
}
