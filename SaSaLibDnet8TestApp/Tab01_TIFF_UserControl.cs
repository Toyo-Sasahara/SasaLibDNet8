using SasaLib;
using SasaLib.PrintConfig;
using SasaLib.ArcSuitePreview;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Versioning;
using SharedClassLibrary;

namespace SaSaLibDNet8TestAPP
{
    [SupportedOSPlatform("windows")]
    public partial class Tab01_TIFF_UserControl : UserControl
    {
        /// フィールド変数

        System.Drawing.Image[] globalImages;
        System.Drawing.Image currentImage = Properties.Resources.TESTIMAGE_A4;

        string currnetPrinterName;
        List<string> globalPrinterCollection = new List<string>();

        List<PaperSize> globalPaperSizeCollection;
        PaperSize currentPapserSize;

        bool currentLandScape;

        List<PaperSource> globalPaperSourceColelection;
        PaperSource currentPaperSource;

        CommonPaperSize currentPaperType = CommonPaperSize.A4P;

        string ImageFileName { get; set; }

        public Tab01_TIFF_UserControl()
        {
            InitializeComponent();

            imagePictureBox.Image = currentImage;

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

        private void LoadImageFIleButton_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                //openFileDialog1.InitialDirectory = "D:\\TCPServer";
                openFileDialog1.Filter = "Image Files(*.BMP;*.PNG;*.TIF;*.TIFF)|*.BMP;*.PNG;*.TIF;*.TIFF|All files (*.*)|*.* ";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    ImageFileName = openFileDialog1.FileName;
                }
            }

            if (ImageFileName != null)
            {
                currentImage = ImageUtil.GetImageFromFile(ImageFileName);

                sasaLibPagePictureControl1.SinglePageImage = currentImage;
                sasaLibPagePictureControl1.SizeMode = PictureBoxSizeMode.Zoom;

            }
            else
            {
            }
            SetImageInfo();

        }


        private bool MultiTiffCreateStart(string newFullFileName, string[] addfilenames)
        {
            bool result = false;
            Image MultiTiffCreateImageObject = null;

            string baseFullFileName = addfilenames[0];

            System.Drawing.Imaging.EncoderValue encoderValue = System.Drawing.Imaging.EncoderValue.CompressionCCITT4;

            bool result1 = MultiTiffCreateBaseImage(baseFullFileName, newFullFileName, ref MultiTiffCreateImageObject, encoderValue);

            if (result1 == true)
            {
                int length = addfilenames.Length;

                for (int i = 1; i < length; i++)
                {
                    result = MultiTiffAddImage(addfilenames[i], ref MultiTiffCreateImageObject, encoderValue);
                }
            }
            MultiTiffCreateImageObject.Dispose();

            return result;
        }


        private bool MultiTiffCreateBaseImage(string baseFullFileName, string targetFullFileName, ref Image MultiTiffCreateImageObject, System.Drawing.Imaging.EncoderValue encoderValue)
        {
            bool result = false;
            FileStream imgStream = null;

            try
            {

                //ImageCodecInfoを取得する
                ImageCodecInfo cdcInfo = SasaLib.ImageUtil.GetEncoderInfo("image/tiff");

                imgStream = new FileStream(baseFullFileName, FileMode.Open, FileAccess.Read);

                MultiTiffCreateImageObject = Image.FromStream(imgStream);

                //
                EncoderParameters encParamaters = new EncoderParameters(2);
                // マルチページを指定する
                encParamaters.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)(EncoderValue.MultiFrame));
                // 圧縮方法を指定する
                encParamaters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)encoderValue);

                MultiTiffCreateImageObject.Save(targetFullFileName, cdcInfo, encParamaters);

                result = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                result = false;
                throw ex;
            }
            finally
            {
                if (imgStream != null)
                {
                    imgStream.Close();
                }
            }
            return result;
        }

        private bool MultiTiffAddImage(string targetFullFileName, ref Image MultiTiffCreateImageObject, System.Drawing.Imaging.EncoderValue encoderValue)
        {
            bool ans = false;
            if (MultiTiffCreateImageObject == null)
            {
                DebugConsole.WriteLine("事前にCreateBaseImageの実行が必要");
                ans = false;
                return ans;
            }

            FileStream imgStream = null;
            Image addedImage = null;

            var encParamaters = new EncoderParameters(2);
            encParamaters.Param[0] = new EncoderParameter(Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
            encParamaters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)encoderValue);
            try
            {

                imgStream = new FileStream(targetFullFileName, FileMode.Open, FileAccess.Read);
                addedImage = Image.FromStream(imgStream);

                MultiTiffCreateImageObject.SaveAdd(addedImage, encParamaters);

                ans = true;
            }
            catch (Exception ex)
            {
                SasaLib.DebugConsole.WriteLine($"例外発生 {ex.Message}");
                ans = false;
                throw ex;
            }
            finally
            {
                if (imgStream != null)
                {
                    imgStream.Close();
                }
                addedImage.Dispose();
            }
            return ans;
        }

        private void CreateMutiTIFF_Test_button_Click(object sender, EventArgs e)
        {
            string[] sourceFullFileName = {
                @"C:\Users\sasahara\source\repos\SasaLibTestFrame\ページ印刷テストファイル-001.TIF",
                @"C:\Users\sasahara\source\repos\SasaLibTestFrame\ページ印刷テストファイル-002.TIF",
                @"C:\Users\sasahara\source\repos\SasaLibTestFrame\ページ印刷テストファイル-003.TIF",
                @"C:\Users\sasahara\source\repos\SasaLibTestFrame\ページ印刷テストファイル-004.TIF",
            };

            MultiTiffCreateStart(@"C:\Users\sasahara\source\repos\SasaLibTestFrame\ページ印刷テストファイル-Merge.TIF",
                sourceFullFileName);
        }

        private void LoadMutiTIFF_Test_button_Click(object sender, EventArgs e)
        {
            string SourceFullFileName = null;
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                //openFileDialog1.InitialDirectory = "D:\\TCPServer";
                openFileDialog1.Filter = "pm files (*.tif;*.tiff)|*.tif;*.tiff|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    SourceFullFileName = openFileDialog1.FileName;
                }
            }

            if (string.IsNullOrWhiteSpace(SourceFullFileName) == false)
            {
                LoadMultiTIFF(SourceFullFileName);
            }
        }

        List<Image> images;
        int CurrentCount;

        private void LoadMultiTIFF(string FullFilename, int StartPageNumber = 1)
        {
            images = GetAllPages(FullFilename);
            sasaLibPagePictureControl1.MultiPageImage = images.ToArray();
            sasaLibPagePictureControl1.SizeMode = PictureBoxSizeMode.Zoom;

            CurrentCount = StartPageNumber - 1;

        }


        private List<Image> GetAllPages(string file)
        {
            List<Image> images = new List<Image>();
            Bitmap bitmap = (Bitmap)Image.FromFile(file);
            int count = bitmap.GetFrameCount(FrameDimension.Page);
            for (int idx = 0; idx < count; idx++)
            {
                // 各フレームをバイテストリームに保存
                bitmap.SelectActiveFrame(FrameDimension.Page, idx);
                MemoryStream byteStream = new MemoryStream();
                bitmap.Save(byteStream, ImageFormat.Tiff);

                // そして、そこから新しい画像を作成します。
                images.Add(Image.FromStream(byteStream));
            }
            return images;
        }

        private void SizeMode_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SizeMode_comboBox.Text)
            {
                case "Normal":
                    sasaLibPagePictureControl1.SizeMode = PictureBoxSizeMode.Normal;
                    break;
                case "StretchImage":
                    sasaLibPagePictureControl1.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                case "AutoSize":
                    sasaLibPagePictureControl1.SizeMode = PictureBoxSizeMode.AutoSize;
                    break;
                case "CenterImage":
                    sasaLibPagePictureControl1.SizeMode = PictureBoxSizeMode.CenterImage;
                    break;
                case "Zoom":
                    sasaLibPagePictureControl1.SizeMode = PictureBoxSizeMode.Zoom;
                    break;
            }
        }

        private void multiPictureControl1_Load(object sender, EventArgs e)
        {

        }

        private void Tab_TIFFTEST_Load(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowArcSuitePreviewForm_button_Click(object sender, EventArgs e)
        {
            try
            {
                ArcsuitePreview arcSuitePreview = new SasaLib.ArcSuitePreview.ArcsuitePreview();
                arcSuitePreview.temporalyDrawingImageFullFileName = ImageFileName;

                // NativeWindow クラスの初期化
                NativeWindow owner = new System.Windows.Forms.NativeWindow();

                // Inventorアプリケーションのウィンドハンドル取得
                //owner.AssignHandle((System.IntPtr)InventorApp.MainFrameHWND);
                ConnectionDataSet connectionDataSet = new ConnectionDataSet();
                ArcSuitePreviewForm ArcSuitePreviewForm = new ArcSuitePreviewForm(new System.Windows.Forms.NativeWindow(), connectionDataSet);
                ArcSuitePreviewForm.DebugMode = checkBox1.Checked;
                ArcSuitePreviewForm.Show();

                ArcSuitePreviewForm.Text = $"ダミー";
                ArcSuitePreviewForm.PreviewSet(arcSuitePreview, "ダミー", "");

                //ArcSuitePreviewForm_ButtonContextMenu.ArcSuitePreviewForm.PreviewSet()
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"ArcSuitePreviewFormTest_Button()にて例外検知 {ex.Message}");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void sasaLibPagePictureControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
