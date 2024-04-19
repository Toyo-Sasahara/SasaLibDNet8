using SasaLib;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaSaLibTestApp
{
    public partial class Tab06_StringProcessing_UserControl : UserControl
    {
        Form1 mainForm;


        public Tab06_StringProcessing_UserControl(Form1 form)
        {
            this.mainForm = form;
            InitializeComponent();
            SetToControls();

            var eventHandler = new System.EventHandler(TabControl1Changed);


        }

        private void TabControl1_VisibleChanged(object sender, EventArgs e)
        {
            LogWindow_textBox.AppendText("TabControl1_VisibleChanged(..)実行・・・\r\n");

            SetToControls();
        }

        bool flag = false;

        /// <summary>
        /// コントロールに変化があったなら
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TabControl1Changed(object sender, EventArgs e)
        {
        }

        private void TabControl1_Load(object sender, EventArgs e)
        {
        }

        public void SetToControls()
        {
        }

        /// <summary>
        /// メインスレッド外からの呼び出しも考慮したﾛｸﾞｳｨﾝﾄﾞｳ変更メソッド
        /// </summary>
        /// <param name="msg"></param>
        public void LogWindowWriteLine(string msg)
        {
            try
            {
                if (this.InvokeRequired)
                {//https://qiita.com/taiyakisun/items/15b57df979eae7562aef
                    this.Invoke(new Action<string>(this.UpdateText), msg);
                }
                else
                {
                    UpdateText($"{msg}\n");
                }
            }
            catch (Exception ex)
            {
                this.LogWindow_textBox.AppendText($"例外検知{ex.Message}\r\n");

            }
        }
        private void UpdateText(string msg)
        {
            this.LogWindow_textBox.AppendText($"{msg}\r\n");
        }



        private void SelectReceveFullFileName_button_Click(object sender, EventArgs e)
        {

        }


        private void TestSplitButton_Click(object sender, EventArgs e)
        {
            string input = InputTextBox.Text;
            TestSplit(input);
            // WriteLine(anser);
        }

        public static void TestSplit(string str)
        {
            // 文字列を全角と半角にわけて配列化します。
            Regex.Split(str, "([A-Z0-9-]+)", RegexOptions.IgnoreCase).DisplayAll();

            // 文字列を全角と半角にわけて配列化します。
            Regex.Split(str, "([A-Z-]+)", RegexOptions.IgnoreCase).DisplayAll();
        }


    }
}
