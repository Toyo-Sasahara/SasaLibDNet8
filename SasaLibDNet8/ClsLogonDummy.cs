using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SasaLibDummy
{
    public class ClsLogonDummy : IDisposable
    {
        // TODO: ClsLogonDummy を 書き換える必要
        public ClsLogonDummy(string Domain, string UserName, string Password, bool UsingClsLogon = false, bool debugConsoleMsg = false, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
        }

        public void Dispose()
        {
        }
    }

}
