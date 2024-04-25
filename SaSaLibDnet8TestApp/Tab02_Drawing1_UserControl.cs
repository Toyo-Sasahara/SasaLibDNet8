using SasaLib;
using SasaLibPictureBoxControlLibrary;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaSaLibDNet8TestAPP
{
    [SupportedOSPlatform("windows")]
    public partial class Tab02_Drawing1_UserControl : UserControl
    {
        Form1 mainForm;

        System.Drawing.Image[] globalImages;
        System.Drawing.Image currentImage = Properties.Resources.TESTIMAGE_A4;

        public Tab02_Drawing1_UserControl(Form1 form)
        {
            this.mainForm = form;
            InitializeComponent();
            SetToControls();

            var eventHandler = new System.EventHandler(TabControl1Changed);

            imagePictureBox.Image = currentImage;

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


        private void PrintRawLoadButton_Click(object sender, EventArgs e)
        {
            string fileName = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                //openFileDialog1.InitialDirectory = "D:\\TCPServer";
                openFileDialog1.Filter = "pm files (*.pm;*.plt;*.out)|*.pm;*.plt;*.out|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                }
            }

            if (fileName != null)
            {
                SasaLib.PJL.PJLdecode pJLdecode = new SasaLib.PJL.PJLdecode(fileName);

                // PJLデータか検証後実行
                if (pJLdecode.IsPJL)
                {
                    globalImages = pJLdecode.GetTiffImages2();

                    currentImage = globalImages[0];
                    //currentImage = ImageUtil.ChangePixcelFormat(globalImages[0], System.Drawing.Imaging.PixelFormat.Format24bppRgb);


                    imagePictureBox.Image = currentImage;
                    SetImageInfo();
                }
            }

        }

        /// <summary>
        /// イメージ情報をTextBoxに反映
        /// </summary>
        void SetImageInfo()
        {
            if (currentImage == null)
            {
                Console.WriteLine("currentImage がnull");
                return;
            }

            Size sz;
            sz = currentImage.Size;

            PixSizeTextBox.Text = "ピクセルの大きさ W:" + sz.Width + " H:" + sz.Height;

            ResTextBox.Text = "DPI= H,V" + currentImage.HorizontalResolution + "," + currentImage.VerticalResolution;

            ImageUtil.PaperSizeCabinet paperSizeCabinet = ImageUtil.GetPaparSize(currentImage);

            if (paperSizeCabinet != null)
            {
                PaperSizetextBox.Text = paperSizeCabinet.PaperName;

                BPPtextBox.Text = currentImage.PixelFormat.ToString();
                {
                    FormatTextbox.Text = currentImage.RawFormat.ToString();
                }


            }


            //currentImage.Save(@"D:\temp.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }
        /// <summary>
        /// pm出力ファイルロードボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocadImageFile_Click(object sender, EventArgs e)
        {
            string fileName = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.Filter = "pm files (*.pm;*.plt;*.out;*.prn)|*.pm;*.plt;*.out;*.prn|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                }
            }

            string savefolder = System.IO.Path.GetDirectoryName(fileName);
            string savetiffFile = System.IO.Path.ChangeExtension(fileName, "tif");
            string basefile = System.IO.Path.GetFileNameWithoutExtension(fileName);

            if (fileName != null)
            {
                CreateTIFF.MakeImageFromPJL(fileName, savefolder, basefile);
            }
        }

        private void LoadImageFIleButton_Click(object sender, EventArgs e)
        {
            string fileName = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = "D:\\TCPServer";
                openFileDialog1.Filter = "Image Files(*.BMP;*.PNG;*.TIF;*.TIFF)|*.BMP;*.PNG;*.TIF;*.TIFF|All files (*.*)|*.* ";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                }
            }

            if (fileName != null)
            {
                currentImage = ImageUtil.GetImageFromFile(fileName);
                imagePictureBox.Image = currentImage;
            }
            else
            {
                imagePictureBox.Image = Properties.Resources.TESTIMAGE_A4;
            }
            SetImageInfo();
        }

        private void ImageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CurrentImagSaveButton_Click(object sender, EventArgs e)
        {
                currentImage.Save(@"D:\currentImage.Save.bmp");
        }

        /// <summary>
        /// MoveImageMIlli()のテスト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveButton_Click(object sender, EventArgs e)
        {
            currentImage = ImageUtil.MoveImageMilli(currentImage, -5, -5);
            imagePictureBox.Image = currentImage;
            SetImageInfo();

        }

        private void MoveImageMili_button_Click(object sender, EventArgs e)
        {
            currentImage = ImageUtil.MoveImageMilli(currentImage, -5, -5);
            imagePictureBox.Image = currentImage;
            SetImageInfo();

        }

        /// <summary>
        /// LockBitsのテスト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LockBitsTestButton_Click(object sender, EventArgs e)
        {
            // Create a new bitmap.
            // 新しいビットマップを作成します。
            Bitmap bmpImage = new Bitmap(@"d:\images.jpg");

            // Lock the bitmap's bits.
            // ビットマップのビットをロックします。
            Rectangle rect = new Rectangle(0, 0, bmpImage.Width, bmpImage.Height);
            System.Drawing.Imaging.BitmapData bmpData = bmpImage.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmpImage.PixelFormat);

            // Get the address of the first line.
            // 最初の行のアドレスを取得します。
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            // ビットマップのバイトを保持する配列を宣言します。
            int bytes = Math.Abs(bmpData.Stride) * bmpImage.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            // RGB値を配列にコピーします。
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            // Set every third value to 255. A 24bpp bitmap will look red. 
            // 3番目の値を255に設定します.24bppのビットマップが赤く表示されます。
            for (int counter = 2; counter < rgbValues.Length; counter += 3)
                rgbValues[counter] = 255;

            // Copy the RGB values back to the bitmap
            // RGB値を再びビットマップにコピーする
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            // ビットのロックを解除します。
            bmpImage.UnlockBits(bmpData);


            // Draw the modified image.
            // 変更されたイメージを描画します。
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(currentImage);
            gr.DrawImage(bmpImage, 0, 150);

            imagePictureBox.Image = currentImage;
        }

        /// <summary>
        /// スタンプを付与するテスト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stamp2Button_Click(object sender, EventArgs e)
        {
            if (currentImage == null) { Console.WriteLine("currentImage がnull"); return; }
            Bitmap stampImg = new Bitmap(@"stamp.png");

            ImageUtil.OverwritingImage(currentImage, stampImg, -130f, -7f);


            imagePictureBox.Image = currentImage;

            SetImageInfo();

        }

        private void RotateButton_Click(object sender, EventArgs e)
        {
            if (currentImage == null) { Console.WriteLine("currentImage がnull"); return; }
            Console.WriteLine("★{0},{1}", currentImage.HorizontalResolution, currentImage.VerticalResolution);

            ImageUtil.RotateImage((Bitmap)currentImage, RotateFlipType.Rotate90FlipNone);
            //続けてピクセルフォーマットをなぜか再適応する必要がある。
            currentImage = ImageUtil.ChangePixelFormat((Bitmap)currentImage, PixelFormat.Format24bppRgb);

            Console.WriteLine("★{0},{1}", currentImage.HorizontalResolution, currentImage.VerticalResolution);

            imagePictureBox.Image = currentImage;
            SetImageInfo();

            GC.Collect();

        }

        /// <summary>
        /// バーコードを付与するテスト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BarcodeButton_Click(object sender, EventArgs e)
        {
            if (currentImage == null) { Console.WriteLine("currentImage がnull"); return; }


            GUIDExtensions guide = new GUIDExtensions(true);
            ImageUtilBarcode.DrawBarcodeFromRightButtom(currentImage, guide, 100, 10, 165, 10, 8, 10, 25);
            imagePictureBox.Image = currentImage;
            SetImageInfo();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PixcelFormatChangeButton_Click(object sender, EventArgs e)
        {
            Bitmap newImg = new Bitmap(currentImage.Width, currentImage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            float dx = Int32.Parse(DXtextBox.Text);
            float dy = Int32.Parse(DYtextBox.Text);
            ImageUtil.ConvertImagePixelFormat(currentImage, newImg, System.Drawing.Imaging.PixelFormat.Format24bppRgb, dx, dy);


            currentImage = newImg;

            imagePictureBox.Image = currentImage;
            SetImageInfo();

        }

        private void ConvertImagePixelFormatButton_Click(object sender, EventArgs e)
        {
            /// 分割せずにピクセルフォーマットを変換
            currentImage = ImageUtil.ConvertImagePixelFormat_Deprecated((Bitmap)currentImage,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            imagePictureBox.Image = currentImage;
            SetImageInfo();

        }

        /// <summary>
        /// ChangePixelFormat() 24bppRgb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Format24bppRgb_Click(object sender, EventArgs e)
        {
            currentImage = ImageUtil.ChangePixelFormat((Bitmap)currentImage, PixelFormat.Format24bppRgb);
            imagePictureBox.Image = currentImage;
            SetImageInfo();
        }

        /// <summary>
        /// ChangePixelFormat() 1bppIndexed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Format1bppIndexe_Click(object sender, EventArgs e)
        {
            currentImage = ImageUtil.ChangePixelFormat((Bitmap)currentImage, PixelFormat.Format1bppIndexed);
            imagePictureBox.Image = currentImage;
            SetImageInfo();

        }

        private void button7_Click(object sender, EventArgs e)
        {
           currentImage = Properties.Resources.TESTIMAGE_A4;
            imagePictureBox.Image = currentImage;
            SetImageInfo();


        }
    }
}
