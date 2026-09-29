using SasaLib;
//using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaSaLibDNet8TestAPP
{
    [SupportedOSPlatform("windows")]
    public partial class Tab09_MySQL_UserControl : UserControl
    {
        Form1 mainForm;


        public Tab09_MySQL_UserControl(Form1 form)
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

        private void SasaLibMySql_tbutton_Click(object sender, EventArgs e)
        {
            // Display the ProgressBar control.
            progressBar1.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            progressBar1.Minimum = 0;
            // Set Maximum to the total number of files to copy.
            // Set the initial value of the ProgressBar.
            progressBar1.Value = 0;
            // Set the Step property to a value of 1 to represent each file being copied.
            progressBar1.Step = 1;

            SasaLibMySql_OutPutttextBox.Clear();

            string server = SsaLibMySqlServer_Name_textBox.Text;
            int port;
            if (!int.TryParse(SsaLibMySqlServer_PortNumber_textBox.Text, out port))
            {

            }

            string database = DatabaseComboBox.Text;
            string UserName = SasaLibMySQLUserName_textbox.Text;
            string UserPass = SasaLibMySQLUserPass_textbox.Text;
            string sqlwhereCommand = SasaLibMySql_SQLtextBox.Text;

            string textEnc = textDecode_textBox1.Text;

            SasaLibMySqlConnect ssMysql_db_buhinzu = new SasaLibMySqlConnect(server, port,
                database,
                textEnc,
                UserName,
                UserPass);

            progressBar1.Style = ProgressBarStyle.Marquee;
            SasaLibMySql_tbutton.Enabled = false;

            MySQLanserLinesLabel.Text = $"検索結果;--行";

            Task.Run(() =>
            {
                var result = ssMysql_db_buhinzu.SQLWhere(sqlwhereCommand);

                Invoke((MethodInvoker)delegate
                {
                    progressBar1.Style = ProgressBarStyle.Continuous;

                    if (result == null)
                    {
                        MySQLanserLinesLabel.Text = $"検索結果;エラーです";
                    }
                    else
                    {
                        MySQLanserLinesLabel.Text = $"検索結果;{result.Count}行";

                        progressBar1.Maximum = result.Count;

                        foreach (var a in result)
                        {
                            // Perform the increment on the ProgressBar.
                            progressBar1.PerformStep();
                            SasaLibMySql_OutPutttextBox.AppendText($"{a}\r\n");
                        }
                    }
                    progressBar1.Value = 0;
                    SasaLibMySql_tbutton.Enabled = true;
                    SasaLibMySql_OutPutttextBox.AppendText($"=========================================================================\r\n");
                });
            });
        }
    }
}
