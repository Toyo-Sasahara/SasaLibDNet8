using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib
{
    /// <summary>
    /// 
    /// https://wiki.dobon.net/index.php?.NET%A5%D7%A5%ED%A5%B0%A5%E9%A5%DF%A5%F3%A5%B0%B8%A6%B5%E6%2F112
    /// </summary>
    public class ImageMagicWrapper
    {
        ImageMagick.MagickImage magickImage;

        public ImageMagicWrapper(string filePath, out System.Drawing.Bitmap bmp, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            magickImage = new ImageMagick.MagickImage(filePath);

            bmp = magickImage.ToBitmap(imageFormat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="image"></param>
        /// <param name="quality">int?：Null許容型</param>
        public void Process(out System.Drawing.Bitmap bmp, System.Drawing.Imaging.ImageFormat imageFormat, int width, int height, int? quality)
        {
            magickImage.Strip();

            if (quality.HasValue)
            {
                magickImage.Quality = quality.Value;
            }

            //ステンシルの幅と高さの比率
            var templateRate = (double)width / height;

            //元画像のアスペクト比
            var nowRate = (double)magickImage.Width / magickImage.Height;

            if (templateRate < nowRate)
            {
                //高さによるスケール
                // Resize each image in the collection to a width of 200. When zero is specified for the height
                // the height will be calculated with the aspect ratio.
                magickImage.Resize(0, height);
                magickImage.ChopHorizontal(width, magickImage.Width - width);
            }
            else
            {
                //幅方向にズーム
                magickImage.Resize(width, 0);
                magickImage.ChopVertical(height, magickImage.Height - height);
            }

            bmp = magickImage.ToBitmap(imageFormat);


        }
        public void Process2(out System.Drawing.Bitmap bmp, System.Drawing.Imaging.ImageFormat imageFormat, int width, int height, int? quality)
        {
            //magickImage.Strip();

            //magickImage.Quality = quality.Value;


            Percentage percent5 = new Percentage(5);

            magickImage.ColorFuzz = percent5;
            //magickImage.Trim();
           magickImage.Edge(5);

            //magickImage.Negate();

            Percentage percent50 = new Percentage(50);
            //magickImage.Threshold(percent50);

            //magickImage.Crop(363,199 , Gravity.Center);

           // magickImage.Resize(363, 199);
            bmp = magickImage.ToBitmap(imageFormat);


        }

    }
}
