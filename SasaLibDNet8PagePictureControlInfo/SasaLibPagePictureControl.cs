using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing.Imaging;
using System.IO;
using SasaLib;

namespace SasaLibPictureBoxControlLibrary
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class SasaLibPagePictureControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public Image[] MultiPageImage
        {
            get { return ImageBuffer.ToArray(); }
            set
            {
                if (value != null)
                {
                    ImageBuffer.Clear();
                    ImageBuffer.AddRange(value);
                    ShowSelectPage(1);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Image SinglePageImage
        {
            get { return pictureBox1.Image; }
            set
            {
                if (value != null)
                {
                    ImageBuffer.Clear();
                    ImageBuffer.Add(value);
                    ShowSelectPage(1);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public PictureBoxSizeMode SizeMode
        {
            get { return pictureBox1.SizeMode; }
            set
            {
                if (value == PictureBoxSizeMode.AutoSize || value == PictureBoxSizeMode.Normal)
                {
                    this.AutoScroll = true;
                    pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                }
                else
                {
                    pictureBox1.Size = this.Size;
                    this.AutoScroll = false;
                    pictureBox1.SizeMode = value;
                }
            }
        }

        /// <summary>
        /// ラベル
        /// </summary>
        public string ButtomLabel
        {
            get { return Buttom_label.Text; }
            set { Buttom_label.Text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<System.Drawing.Image> ImageBuffer = new List<System.Drawing.Image>();

        /// <summary>
        /// 
        /// </summary>
        private int currentPage;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SasaLibPagePictureControl()
        {
            InitializeComponent();

            pictureBox1.Location = new Point(0, 0);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.AutoScroll = true;
            Page_textBox.Focus();
        }

        /// <summary>
        /// マルチページTiff対応
        /// </summary>
        /// <param name="FullFilename"></param>
        /// <param name="StartPageNumber"></param>
        public void FromFile(string FullFilename, int StartPageNumber = 1)
        {
            var images = GetAllPages(FullFilename);
            this.MultiPageImage = images.ToArray();
            this.SizeMode = PictureBoxSizeMode.Zoom;

            currentPage = StartPageNumber - 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageSelect"></param>
        public void ShowSelectPage(int PageSelect)
        {
            if (ImageBuffer != null && ImageBuffer.Count > 0)
            {
                if (0 < PageSelect && PageSelect < ImageBuffer.Count + 1)
                {
                    currentPage = PageSelect - 1;
                    pictureBox1.Image = ImageBuffer[currentPage];

                    Page_textBox.Text = $"{currentPage + 1} / {ImageBuffer.Count}";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MultiPictureControl_SizeChanged(object sender, EventArgs e)
        {
            int ypos = NavigatePanel.Location.Y;
            int xpos = (this.Width / 2) - (NavigatePanel.Width / 2);

            this.NavigatePanel.Location = new System.Drawing.Point(xpos, ypos);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_button_Click(object sender, EventArgs e)
        {
            if (ImageBuffer != null && ImageBuffer.Count > 0)
            {
                if (currentPage < ImageBuffer.Count - 1)
                    currentPage++;
                pictureBox1.Image = ImageBuffer[currentPage];

                Page_textBox.Text = $"{currentPage + 1} / {ImageBuffer.Count}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reverce_button_Click(object sender, EventArgs e)
        {
            if (ImageBuffer != null && ImageBuffer.Count > 0)
            {

                if (currentPage > 0)
                    currentPage--;

                pictureBox1.Image = ImageBuffer[currentPage];

                Page_textBox.Text = $"{currentPage + 1} / {ImageBuffer.Count}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            Page_textBox.SelectAll();

            if (e.KeyCode == Keys.Enter)
            {

                int i;
                if (int.TryParse(Page_textBox.Text, out i))
                {
                    Console.WriteLine(i);

                    if (-1 < i && i < ImageBuffer.Count + 1)
                    {
                        currentPage = i - 1;
                        pictureBox1.Image = ImageBuffer[currentPage];
                    }
                    else
                    {
                        //Console.WriteLine("数値に変換できません");

                    }
                }
                Page_textBox.Text = $"{currentPage + 1} / {ImageBuffer.Count}";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private List<Image> GetAllPages(string file)
        {
            List<Image> images = new List<Image>();
            // Bitmap bitmap = (Bitmap)Image.FromFile(file);

            Bitmap bitmap = (Bitmap)ImageUtil.FromFile(file);
            
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
    }
}
