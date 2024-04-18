using System;
using System.Text;

namespace SasaLib
{
    public static class CharacterCode
    {
        public static Encoding GetActiveCodePage()
        {
            return Console.OutputEncoding;
        }
    }
}
