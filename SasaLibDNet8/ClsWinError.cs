using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SasaLib
{
    /// <summary>
    /// Code from https://qiita.com/h-ymmr/items/48aa308b8219f35f7255
    /// </summary>
    class ClsWinError
    {
        [DllImport("kernel32.dll")]
        public static extern uint FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr Arguments);

        private const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;

        public ClsWinError()
        {
        }

        public String GetWinErrMessage(int intErrCode)
        {
            StringBuilder objSb = new StringBuilder(255);
            FormatMessage(
              FORMAT_MESSAGE_FROM_SYSTEM,
              IntPtr.Zero,
              (uint)intErrCode,
              0,
              objSb,
              objSb.Capacity,
              IntPtr.Zero);
            return objSb.ToString();
        }
    }
}