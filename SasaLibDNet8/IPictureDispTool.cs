using System;
using System.IO;
using System.Runtime.Versioning;
using SIP = System.IO.Path;

namespace SasaLib
{
    /// <summary>
    /// アセンブリ stdole, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a を使用している
    /// </summary>
    public static class IPictureDispTool
    {
        /// <summary>
        /// オリジナルサムネイルを取得
        /// アセンブリ stdole, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a を使用している
        /// </summary>
        /// <param name="Thumbnail"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static System.Drawing.Image OriginalImage(stdole.IPictureDisp Thumbnail, int width, int height, bool debug = true)
        {
            //System.Drawing.Image image = IPictuireUtil.GetPictureFromIPicture(Thumbnail);
            System.Drawing.Image image = OleCreateConverter.PictureDispToImage(Thumbnail);

            float scale = Math.Min((float)width / (float)image.Width, (float)height / (float)image.Height);
            // 変更サイズを取得する
            int widthToScale = (int)(image.Width * scale);
            int heightToScale = (int)(image.Height * scale);

            string tempbmpfile = System.IO.Path.Combine(SIP.GetTempPath(), SIP.GetRandomFileName() + @".BMP");

            var retimg = ImageUtil.Myresize(image, widthToScale, heightToScale);
            if (debug)
                Console.WriteLine($"●イメージリサイズ　{image.Width},{image.Height} -> {retimg.Width},{retimg.Height}");

            return retimg;
        }



        /// <summary>
        /// stdole.IPictureDisp Thumbnail から　パーツリスト用輪郭図を生成
        /// アセンブリ stdole, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a を使用している
        /// </summary>
        /// <param name="Thumbnail">stdole.IPictureDisp Thumbnail</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static System.Drawing.Image PartsListImageConvert(stdole.IPictureDisp Thumbnail, int width, int height, string fname = null, bool debug = true)
        {
            string folder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            //System.Drawing.Image image = IPictuireUtil.GetPictureFromIPicture(Thumbnail);
            System.Drawing.Image image = OleCreateConverter.PictureDispToImage(Thumbnail);

            System.Drawing.Bitmap bitmap1 = new System.Drawing.Bitmap(image);


            // 変更サイズを取得する
            float scale = Math.Min((float)width / (float)bitmap1.Width, (float)height / (float)bitmap1.Height);
            int widthToScale = (int)(bitmap1.Width * scale);
            int heightToScale = (int)(bitmap1.Height * scale);
            bitmap1 = ImageUtil.Myresize(bitmap1, widthToScale, heightToScale);
            
            if (debug)
                Console.WriteLine($"●イメージリサイズ　{image.Width},{image.Height} -> {bitmap1.Width},{bitmap1.Height}");


            // ラプラシアンフィルタによる輪郭抽出
            bitmap1 = (System.Drawing.Bitmap)ImageUtil.LaplacianFilter(bitmap1);

            // ネガポジ反転
            bitmap1 = (System.Drawing.Bitmap)ImageUtil.CreateNegativeImage(bitmap1);

            // メディアんフィルタ
            bitmap1 = (System.Drawing.Bitmap)ImageUtil.MedianFilter(bitmap1);

            // 2値化
            bitmap1 = (System.Drawing.Bitmap)ImageUtil.Change2bpFilter(bitmap1, 250);

            MemoryStream tiffStream = new MemoryStream();
            ImageUtil.ImageToTIFF1bppCCITT4Stream(bitmap1, tiffStream);

            if (fname != null)
            {
                string tiffname = FileFolder.ChangeExtension(fname, "TIF");
                string tempbmpfile2 = System.IO.Path.Combine(folder, tiffname);
                StreamExtensions.StreamToFile(tiffStream, tempbmpfile2);
            }

            // 後始末
            tiffStream.Dispose();

            return bitmap1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="fname"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static System.Drawing.Image PartsListImageConvert(System.Drawing.Image Image, int width, int height, string fname)
        {
            string folder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            string tiffname = FileFolder.ChangeExtension(fname, "TIF");

            string tempbmpfile2 = System.IO.Path.Combine(folder, tiffname);

            System.Drawing.Bitmap bitmap1 = new System.Drawing.Bitmap(Image);


            // 変更サイズを取得する
            float scale = Math.Min((float)width / (float)bitmap1.Width, (float)height / (float)bitmap1.Height);
            int widthToScale = (int)(bitmap1.Width * scale);
            int heightToScale = (int)(bitmap1.Height * scale);
            bitmap1 = ImageUtil.Myresize(bitmap1, widthToScale, heightToScale);
            Console.WriteLine($"●イメージリサイズ　{Image.Width},{Image.Height} -> {bitmap1.Width},{bitmap1.Height}");


            // ラプラシアンフィルタによる輪郭抽出
            bitmap1 = (System.Drawing.Bitmap)ImageUtil.LaplacianFilter(bitmap1);

            // ネガポジ反転
            bitmap1 = (System.Drawing.Bitmap)ImageUtil.CreateNegativeImage(bitmap1);

            // メディアんフィルタ
            bitmap1 = (System.Drawing.Bitmap)ImageUtil.MedianFilter(bitmap1);

            // 2値化
            bitmap1 = (System.Drawing.Bitmap)ImageUtil.Change2bpFilter(bitmap1, 250);

            MemoryStream tiffStream = new MemoryStream();
            ImageUtil.ImageToTIFF1bppCCITT4Stream(bitmap1, tiffStream);

            StreamExtensions.StreamToFile(tiffStream, tempbmpfile2);

            // 後始末
            tiffStream.Dispose();

            return bitmap1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap1"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static System.Drawing.Image PartsListImageConvert(System.Drawing.Image bitmap1, int width, int height)
        {
            string folder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            System.Drawing.Image retBmp = PartsListImageConvert(bitmap1, width, height);

            return retBmp;
        }

    }
}
