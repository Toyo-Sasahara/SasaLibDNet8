using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SasaLib.ArcSuitePreview
{
    class MultiPage : IDisposable
    {
        /// <summary>
        /// マルチページのImageデータを保持するﾊﾞｯﾌｧ
        /// </summary>
        public Image[] Images
        {
            get { return ImageBuffer.ToArray(); }
            set
            {
                if (value != null)
                {
                    ImageBuffer.Clear();
                    ImageBuffer.AddRange(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<System.Drawing.Image> ImageBuffer = new List<System.Drawing.Image>();


        /// <summary>
        /// 現在の表示ページを保持
        /// </summary>
        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }
        private int currentPage;

        public MultiPage(string imageFillFileName, int currentPage = 1)
        {
            FromFile(imageFillFileName);
            this.currentPage = currentPage;
        }

        public Image GetCurrentImage()
        {
            return ImageBuffer[currentPage];
        }

        public Image GetImage(int pageNo)
        {
            return ImageBuffer[pageNo];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageSelect"></param>
        //public void ShowSelectPage(int PageSelect)
        //{
        //    if (ImageBuffer != null && ImageBuffer.Count > 0)
        //    {
        //        if (0 < PageSelect && PageSelect < ImageBuffer.Count + 1)
        //        {
        //            currentPage = PageSelect - 1;

        //            //form.MiniPanelPictureBox.Image = ImageBuffer[currentPage];
        //            //form.sourceBitmap = (System.Drawing.Bitmap)ImageBuffer[currentPage];
        //            //form.IntegratedSearchForm_Resize(null, null);
        //            //// ピクチャーボックスにフィット
        //            //form.ViewFit();

        //            //form.sasaLibPageControl1.PageNumberTextBox.Text = $"{currentPage + 1} / {ImageBuffer.Count}";
        //        }
        //    }
        //}

        /// <summary>
        /// マルチページTiff対応のイメージファイルを読み込んで
        /// </summary>
        /// <param name="FullFilename"></param>
        /// <param name="StartPageNumber"></param>
        private void FromFile(string FullFilename)
        {
            var images = GetAllPages(FullFilename);
            if (images != null)
            {
                this.Images = images.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullfileName"></param>
        /// <returns></returns>
        private List<Image> GetAllPages(string fullfileName)
        {
            if (string.IsNullOrWhiteSpace(fullfileName) == false)
            {

                System.IO.FileStream fs = new System.IO.FileStream(fullfileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                try
                {
                    List<Image> images = new List<Image>();
                    Bitmap bitmap = (Bitmap)System.Drawing.Image.FromStream(fs);

                    int count = bitmap.GetFrameCount(FrameDimension.Page);
                    for (int idx = 0; idx < count; idx++)
                    {
                        // 各フレームをバイテストリームに保存
                        bitmap.SelectActiveFrame(FrameDimension.Page, idx);
                        MemoryStream byteStream = new MemoryStream();
                        bitmap.Save(byteStream, ImageFormat.Tiff);

                        // そして、そこから新しい画像を作成します。
                        images.Add(System.Drawing.Image.FromStream(byteStream));
                    }

                    fs.Close();
                    return images;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SasaLib.ImageUtil.FromFile({fullfileName})にて例外発生. 内容{ex.Message}");
                    fs.Close();
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void MultiPageImageClear()
        {
            ImageBuffer.Clear();
        }

        public void Dispose()
        {
            ImageBuffer.Clear();
        }
    }
}
