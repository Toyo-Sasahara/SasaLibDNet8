using System.Runtime.Versioning;
using System.Windows.Forms;

namespace SasaLib
{
    public static class ContorlValidate
    {
        /// <summary>
        /// 主にTextBoxのkeyPressイベントから呼び出し、数値文字であることを確認
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [SupportedOSPlatform("windows")]
        public static void CheckInputNumeric(object sender, KeyPressEventArgs e)
        {
            // 制御文字は入力可
            if (char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                return;
            }
            
            // 数字(0-9)は入力可
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 数字(0-9)は入力可
            if (char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 小数点は１つだけ入力可
            if (e.KeyChar == '.')
            {
                TextBox target = sender as TextBox;
                if (target.Text.IndexOf('.') < 0)
                {
                    // 複数のピリオド入力はNG
                    e.Handled = false;
                    return;
                }
            }

            // -は１つだけ入力可
            if (e.KeyChar == '-')
            {
                TextBox target = sender as TextBox;
                if (target.Text.IndexOf('-') < 0)
                {
                    // 複数の-入力はNG
                    e.Handled = false;
                    return;
                }
            }
            // 上記以外は入力不可
            e.Handled = true;

        }

    }
}
