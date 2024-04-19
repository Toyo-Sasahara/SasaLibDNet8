using SasaLib;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaSaLibTestApp
{
    public partial class Tab03_Drawing2_UserControl : UserControl
    {
        Form1 mainForm;

        ImageMagicWrapper imageMagic;

        public Tab03_Drawing2_UserControl(Form1 form)
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

        private void LocadImaeButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                //openFileDialog1.InitialDirectory = "D:\\TCPServer";
                openFileDialog1.Filter = "All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                System.Drawing.Bitmap bmp;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog1.FileName;

                    imageMagic = new ImageMagicWrapper(fileName, out bmp, ImageFormat.Bmp);

                    pictureBox1.Image = bmp;

                }
            }

        }

        private void DrawBarcodeFromRightButtomButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                ImageUtilBarcode.DrawBarcodeFromRightButtom(pictureBox1.Image, new GUIDExtensions(true), 105, 8, 85, 8, 150, 8, 8);
                pictureBox1.Refresh();
            }


        }

        private void SizeSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string x = SizeSelectComboBox.Text;
            int a = Convert.ToInt32(x);
            float scale = a / 100f;
            pictureBox1.Image = ImageUtil.Myresize(pictureBox1.Image, scale);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Dock = DockStyle.Fill;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Dock = DockStyle.None;
            }
            pictureBox1.Refresh();

        }

        private void LocadImaeButton2_Click_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                //openFileDialog1.InitialDirectory = "D:\\TCPServer";
                openFileDialog1.Filter = "All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;
                System.Drawing.Bitmap bmp;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog1.FileName;

                    imageMagic = new ImageMagicWrapper(fileName, out bmp, ImageFormat.Bmp);

                    pictureBox2.Image = bmp;

                }

            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            System.Drawing.Bitmap bmp;

            imageMagic.Process2(out bmp, ImageFormat.Bmp, 400, 400, 10);

            pictureBox2.Image = bmp;

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox2.Dock = DockStyle.Fill;
            }
            else
            {
                pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox2.Dock = DockStyle.None;

            }

            pictureBox2.Refresh();

        }

        private void barcodeDraw2CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                ImageUtilBarcode.DrawBarcodeFromRightButtom(pictureBox2.Image, new GUIDExtensions(true), 105, 8, 85, 8, 150, 8, 8);
                pictureBox2.Refresh();
            }

        }
    }
}
