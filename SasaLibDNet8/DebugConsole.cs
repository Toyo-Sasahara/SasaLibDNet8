using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib
{
    public static class DebugConsole
    {
        [System.Diagnostics.DebuggerStepThrough]
        public static void WriteLine(string Message)
        {
            
            Debug.WriteLineIf(true, Message);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void Write(string Message)
        {
            Debug.WriteIf(true, Message);
        }

    }
}
