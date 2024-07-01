//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Runtime.Versioning;

//namespace SasaLib.ArcSuitePreview
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    class MultiPage : IDisposable
//    {
//        /// <summary>
//        /// マルチページのImageデータを保持するﾊﾞｯﾌｧ
//        /// </summary>
//        public Image[] Images
//        {
//            get { return ImageBuffer.ToArray(); }
//            set
//            {
//                if (value != null)
//                {
//                    ImageBuffer.Clear();
//                    ImageBuffer.AddRange(value);
//                }
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        private List<System.Drawing.Image> ImageBuffer = new List<System.Drawing.Image>();


//        /// <summary>
//        /// 現在の表示ページを保持
//        /// </summary>
//        public int CurrentPage
//        {
//            get { return currentPage; }
//            set { currentPage = value; }
//        }
//        private int currentPage;

//        public MultiPage(string imageFillFileName, int currentPage = 1)
//        {
//            FromFile(imageFillFileName);
//            this.currentPage = currentPage;
//        }

//        public Image GetCurrentImage()
//        {
//            return ImageBuffer[currentPage];
//        }

//        public Image GetImage(int pageNo)
//        {
//            return ImageBuffer[pageNo];
//        }

//        /// <summary>
//        /// マルチページTiff対応のイメージファイルを読み込んで
//        /// </summary>
//        /// <param name="FullFilename"></param>
//        /// <param name="StartPageNumber"></param>
//        private void FromFile(string FullFilename)
//        {
//            var images = GetAllPages(FullFilename);
//            if (images != null)
//            {
//                this.Images = images.ToArray();
//            }
//        }

//        /// <summary>
//        /// マルチフレームのイメージをファイルから読み込む。マルチフレームか否かは System.Drawing.Image.GetFrameCount(..)にて判別
//        /// </summary>
//        /// <param name="fullfileName"></param>
//        /// <returns></returns>
//        private List<Image> GetAllPages(string fullfileName, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            if (string.IsNullOrWhiteSpace(fullfileName) == false)
//            {
//                if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

//                System.IO.FileStream fs = new System.IO.FileStream(fullfileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
//                try
//                {
//                    List<Image> images = new List<Image>();
//                    Bitmap bitmap = (Bitmap)System.Drawing.Image.FromStream(fs);

//                    int count = bitmap.GetFrameCount(FrameDimension.Page);

//                    WriteLine($"GetAllPages()");
//                    for (int idx = 0; idx < count; idx++)
//                    {
//                        // 各フレームをバイテストリームに保存
//                        bitmap.SelectActiveFrame(FrameDimension.Page, idx);
//                        MemoryStream byteStream = new MemoryStream();
//                        bitmap.Save(byteStream, ImageFormat.Tiff);

//                        // そして、そこから新しい画像を作成します。
//                        images.Add(System.Drawing.Image.FromStream(byteStream));
//                    }

//                    fs.Close();
//                    return images;
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"SasaLib.ImageUtil.FromFile({fullfileName})にて例外発生. 内容{ex.Message}");
//                    fs.Close();
//                    return null;
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }

//        public void MultiPageImageClear()
//        {
//            ImageBuffer.Clear();
//        }

//        public void Dispose()
//        {
//            ImageBuffer.Clear();
//        }
//    }
//}
