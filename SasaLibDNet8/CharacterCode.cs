using System;
using System.Text;

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
