using SasaLib;
using SasaLib.PIPE;
using SasaLib.SysConfigurator;
using SasaLib.VariableControlPipeClient;
using SasaLib.VariableControlPipeServer;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Networking;

namespace SaSaLibDNet8TestAPP
{
    [SupportedOSPlatform("windows")]
    public partial class Tab11_SysConfigurator_UserControl : UserControl
    {
        Form1 mainForm;


        public Tab11_SysConfigurator_UserControl(Form1 form)
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

        private void button22_Click(object sender, EventArgs e)
        {
            TestRun.Execute(@"D:\TESTDATA.CONF");

        }

        private DialogResult PipeServerUserMsgBoxShow(string Message, string Title, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon)
        {
            var result = MessageBox.Show(this, Message, Title, messageBoxButtons, messageBoxIcon);
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogWindowWriteLine("パイプサーバースタート");

            ConfigTest.Config = new ConfigTest();

            VariableControlPipeServer oPipeServe = new VariableControlPipeServer("ABCDE", "Accept Ver 1.22.08", ConfigTest.Config, DebugConsole.WriteLine, 0, PipeServerUserMsgBoxShow);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            VariableControlPipeClient remote = new VariableControlPipeClient("", "", "", false, "localhost", "ABCDE");
            //remote.pileCltStremConnectTimeOut = 20000;

            object startDateTImeObj;


            //コントロールに対する処理
            //WriteLine($"Inventor 利用状況ﾁｪｯｸ開始・・");
            startDateTImeObj = remote.GetValueAndValueType_DataCommand(CMDNAME.StartUpDateTime, objectConvNew: checkBox1.Checked, WriteLine: DebugConsole.WriteLine);
            //WriteLine($"Inventor 利用状況ﾁｪｯｸ終了。ﾘﾋﾟｰﾄﾓｰﾄﾞ{LoopcheckMode}");


        }

        int logNumber = 0;
        int LimitNumberLines = 10;

        private void _WriteLine(string value)
        {

            if (logNumber > LimitNumberLines)
            {
                try
                {
                    MethodInvoker method = () =>
                    {
                        LogWindow_textBox.Lines = LogWindow_textBox.Lines.Skip(LogWindow_textBox.Lines.Length - LimitNumberLines).ToArray();
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }

                }
                catch (Exception ex)
                {
                    SasaLib.Eventlog.Log.WriteEntry("InventorTOYOaddin", EventLogEntryType.Error, 0, $"※LogWindowForm.WriteLine(...),  LogTextBox.Lines 失敗,Exception={ex.Message} value = {value}");
                }

                logNumber = 0;
            } // LimitNumberLinesより行数が増えたときの処理

            logNumber++;

            try
            {
                MethodInvoker method = () =>
                {                        /// UIを操作する処理
                    LogWindow_textBox.AppendText($"{logNumber}:" + value + "\r\n");

                };
                if (InvokeRequired) { Invoke(method); } else { method(); }

            }
            catch (Exception ex)
            {
                SasaLib.Eventlog.Log.WriteEntry("InventorTOYOaddin", EventLogEntryType.Error, 0, $"※LogWindowForm.WriteLine(...), LogTextBox.AppendText失敗,Exception={ex.Message} value = {value}");
            }

        }

    }
}
