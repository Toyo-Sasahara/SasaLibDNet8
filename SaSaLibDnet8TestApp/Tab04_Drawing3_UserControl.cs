using SasaLib;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaSaLibDNet8TestAPP
{
    public partial class Tab04_Drawing3_UserControl : UserControl
    {
        Form1 mainForm;


        public Tab04_Drawing3_UserControl(Form1 form)
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

        private void LogWindow_textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            OrgPictureBox.Image = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog1.FileName;

                    InputImageFIleTextBox.Text = fileName;
                    TIFFFILEFULLPATHLabel.Text = SasaLib.FileFolder.ChangeExtension(fileName, "TIF");

                    OrgPictureBox.Image = System.Drawing.Image.FromFile(fileName);
                }
            }

        }

        private void ConvertButton1_Click(object sender, EventArgs e)
        {
            NewPictureBox.Image = SasaLib.ImageUtil.CreateOutline_extractionTiffCCITT4Image(OrgPictureBox.Image, 363, 199);
        }

        private void ConvertButton2_Click(object sender, EventArgs e)
        {

        }

        private void SaveTiffFileButton_Click(object sender, EventArgs e)
        {
            string tiff = SasaLib.ImageUtil.CreateOutline_extractionTiffCCITT4Image(InputImageFIleTextBox.Text, 363, 199);
            NewPictureBox.Image = System.Drawing.Image.FromFile(tiff);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            EncodedLabel.Text = "変換中・・・";
            DecodedLabel.Text = "変換中・・・";

            var input = InputMessageTextBox.Text;

            SasaLib.Encryption encryption = new Encryption("SasaAuth2.1");

            var encoded = encryption.Encoding(input);

            EncodedLabel.Text = encoded;

            var decoded = encryption.Decoding(EncodedLabel.Text);

            DecodedLabel.Text = decoded;
        }

    }
}
