using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLibDNet8
{
    public static class CharacterCode
    {
        public static Encoding GetActiveCodePage()
        {
            return Console.OutputEncoding;
        }
    }
}
