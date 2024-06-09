using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SasaLibDummy
{
    /// <summary>
    /// 
    /// </summary>
    public class ClsLogonDummy : IDisposable
    {
        // TODO: ClsLogonDummy を 書き換える必要

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Domain"></param>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <param name="UsingClsLogon"></param>
        /// <param name="debugConsoleMsg"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public ClsLogonDummy(string Domain, string UserName, string Password, bool UsingClsLogon = false, bool debugConsoleMsg = false, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
        }
    }

    // TODO: ClsLogonDummy を 書き換えた
    //new WithFakeAccount(DomainName, UserName, UserPassword, ClsLogon, () =>
    //{
    //});

}
