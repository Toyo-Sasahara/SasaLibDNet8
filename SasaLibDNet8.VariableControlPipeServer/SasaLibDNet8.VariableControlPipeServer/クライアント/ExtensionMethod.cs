using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SasaLib.VariableControlPipeServer
{
    [SupportedOSPlatform("windows")]
    static class ExtensionMethod
    {
        /// <summary>
        /// 全てのチェックボックスをチェックする拡張メソッド
        /// </summary>
        /// <param name="source">CheckedListBox</param>
        /// <param name="value">チェック値</param>
        public static void CheckAllCheckBoxes(this CheckedListBox source, bool value)
        {
            for (int i = 0; i < source.Items.Count; i++)
            {
                source.SetItemChecked(i, value);
            }
        }


    }
}
