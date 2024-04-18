// SasaLib 笹原専用イメージ処理クラスライブラリ
// 2018-09-13 株式会社東陽機械製作所 システム開発課
// 使用プログラム 
// Wordリスト変換ツール
//
//using BarcodeLib;
using SasaLib.PrintConfig;
using SasaLibDNet8;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using System.Windows.Media.Imaging;

namespace SasaLib
{
    [SupportedOSPlatform("windows")]

    /// <summary>
    /// 
    /// </summary>
    public struct PM
    {
        public int Width;
        public int Height;
    }

    /// <summary>
    /// イメージ関係処理メソッド（スタティック）
    /// </summary>
    public static partial class ImageUtil
    {
        /// <summary>
        /// 色のデータ型を別のデータ型に変換するImageオブジェクト
        /// </summary>
        static System.Drawing.ImageConverter imgconv = new System.Drawing.ImageConverter();

        // イメージオブジェクト種類変換・ イメージ生成関連　メソッド群

        /// <summary>
        /// バイト配列をImageオブジェクトに変換
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static System.Drawing.Image ByteArrayToImage(byte[] b)
        {
            System.Drawing.Image img = (System.Drawing.Image)imgconv.ConvertFrom(b);
            return img;
        }

        /// <summary>
        /// イメージオブジェクトをバイト配列に変換
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArry(System.Drawing.Image img)
        {
            byte[] ba;
            ba = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return ba;
        }

        /// <summary>
        /// ImageオブジェクトをBitmapImageオブジェクトに変換
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static System.Windows.Media.Imaging.BitmapImage GetBitmapImage(System.Drawing.Image image)
        {
            // Imageオブジェクトをイメージファイル化する出力先ストリームを準備
            using (Stream st = new MemoryStream())
            {
                //// 指定画像形式でメモリーストリームに保存
                image.Save(st, System.Drawing.Imaging.ImageFormat.Tiff);

                //WPF用のBitmapImageオブジェクトを生成
                BitmapImage bitmapImage = new BitmapImage();
                // ストリームはまだ使用できるので改めて再利用。biにデータを読み込む
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = st;
                bitmapImage.EndInit();
                // ストリームをクローズ
                st.Close();
                //BitmapImageデーターを PartImageBitmapImageに保存
                return bitmapImage;
            }
        }

        /// <summary>
        /// 指定したImageオブジェクトをTIFF形式に変換し、指定したMemoryStreamに格納
        /// </summary>
        /// <param name="image"></param>
        /// <param name="msTiff2bpp"></param>
        public static void ImageToTIFF1bppCCITT4Stream(System.Drawing.Image image, MemoryStream msTiff2bpp)
        {
            //SaveImageToFile(orgImage, "image/tiff", System.Drawing.Imaging.EncoderValue.CompressionCCITT4, @"D:\1-orgImage.tif");


            // ImageCodecInfoを取得する
            ImageCodecInfo ici = GetEncoderInfo("image/tiff");
            // オプション設定
            EncoderParameters ep = new EncoderParameters(1);
            //// カラー深度
            //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 24L);
            // 圧縮方法を指定する
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)System.Drawing.Imaging.EncoderValue.CompressionCCITT4);

            image.Save(msTiff2bpp, ici, ep);
            msTiff2bpp.Position = 0;
        }
        public static void ImageToTIFF24LbppStream(System.Drawing.Image image, MemoryStream msTiff24Lbpp)
        {
            // ImageCodecInfoを取得する
            ImageCodecInfo ici = GetEncoderInfo("image/tiff");
            // オプション設定
            EncoderParameters ep = new EncoderParameters(1);
            //// カラー深度
            ep.Param[0] = new EncoderParameter(Encoder.ColorDepth, 24L);
            // 圧縮方法を指定する
            ep.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionNone);

            image.Save(msTiff24Lbpp, ici, ep);

        }

        /// <summary>
        /// 指定したImageオブジェクトのピクセル深度を変更した新しいBitmapイメージを返す
        /// 大きなイメージではメモリ不足になります _Deprecated=推奨されない
        /// </summary>
        /// <param name="image"></param>
        /// <param name="pf"></param>
        /// <returns></returns>
        public static System.Drawing.Image ChangePixcelFormat_Deprecated(System.Drawing.Image image, System.Drawing.Imaging.PixelFormat pf, float dHmm = 0, float dVmm = 0)
        {
            System.Drawing.Imaging.PixelFormat PIXELFORMAT = image.PixelFormat;

            System.Drawing.Bitmap workImage = new System.Drawing.Bitmap(image.Width, image.Height, pf);

            // 解像度を設定
            workImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Graphicsオブジェクトに接続
            System.Drawing.Graphics gr_newImage = System.Drawing.Graphics.FromImage(workImage);

            // offset指示がある場合
            if (dHmm != 0f || dVmm != 0f)
            {
                Console.WriteLine("オフセット {0},{1}", dHmm, dVmm);
                float dHpixel = dHmm / 25.4f * image.HorizontalResolution;
                float dVpixel = dVmm / 25.4f * image.VerticalResolution;
                System.Drawing.Drawing2D.Matrix mx = new System.Drawing.Drawing2D.Matrix();
                mx.Translate(dHpixel, dVpixel);
                gr_newImage.Transform = mx;
                mx.Dispose();
            }

            gr_newImage.DrawImage(image, new Point(0, 0));

            gr_newImage.Dispose();

            return workImage;
        }

        /// <summary>
        /// Bitmapイメージのピクセル深度を変更して返す。
        /// 分割しないのでメモリ不足注意_Deprecated=推奨されない
        /// </summary>
        /// <param name="bitmap">元のBitmap</param>
        /// <param name="pxf">ピクセルフォーマット</param>
        /// <returns></returns>
        public static System.Drawing.Bitmap ConvertImagePixelFormat_Deprecated(System.Drawing.Bitmap bitmpaSource,
            System.Drawing.Imaging.PixelFormat pxf = System.Drawing.Imaging.PixelFormat.Format24bppRgb)
        {
            var rect = new Rectangle(0, 0, bitmpaSource.Width, bitmpaSource.Height);
            return bitmpaSource.Clone(rect, pxf);
        }

        /// <summary>
        /// ピクセル深度を変更する。指定したsrcImgオブジェクトを指定したdistBMPに行う。メモリ対策で1/4づつ変換する.
        /// ソースイメージの解像度を適応させる
        /// </summary>
        /// <param name="srcImg"></param>
        /// <param name="distBMP"></param>
        /// <param name="pixelFormat"></param>
        /// <param name="dHmm"></param>
        /// <param name="dVmm"></param>
        public static void ConvertImagePixelFormat(System.Drawing.Image srcImg, System.Drawing.Bitmap distBMP,
            System.Drawing.Imaging.PixelFormat pixelFormat, float dHmm = 0, float dVmm = 0)
        {
            void SplitImageDraw(System.Drawing.Bitmap source, System.Drawing.Image inBmp, Rectangle srcRect)
            {
                Console.WriteLine("SplidImageDraw Rectangle {0}", srcRect);
                Console.WriteLine("SplidImageDraw Resolution H:{0} V:{1}", inBmp.HorizontalResolution, inBmp.VerticalResolution);
                Graphics gr_bitmap = Graphics.FromImage(source);

                if (dHmm != 0f || dVmm != 0f)
                {
                    Console.WriteLine("オフセット {0},{1}", dHmm, dVmm);

                    float dHpixel = MilliToPixel(dHmm, inBmp.HorizontalResolution);
                    float dVpixel = MilliToPixel(dVmm, inBmp.VerticalResolution);
                    System.Drawing.Drawing2D.Matrix mx = new System.Drawing.Drawing2D.Matrix();
                    mx.Translate(dHpixel, dVpixel);
                    gr_bitmap.Transform = mx;
                    mx.Dispose();
                }

                GraphicsUnit units = GraphicsUnit.Pixel;
                // Draw image to screen.
                gr_bitmap.DrawImage(inBmp, srcRect, srcRect, units);
                //
                gr_bitmap.Dispose();
            }


            int Width = srcImg.Width;
            int Height = srcImg.Height;

            // Create rectangle for displaying image.
            int W = srcImg.Width / 4;
            int H = srcImg.Height / 4;

            SplitImageDraw(distBMP, srcImg, new Rectangle(W * 0, H * 0, W * 1, H * 2));
            SplitImageDraw(distBMP, srcImg, new Rectangle(W * 1, H * 0, W * 1, H * 2));
            SplitImageDraw(distBMP, srcImg, new Rectangle(W * 2, H * 0, W * 1, H * 2));
            SplitImageDraw(distBMP, srcImg, new Rectangle(W * 3, H * 0, W * 1, H * 2));

            SplitImageDraw(distBMP, srcImg, new Rectangle(W * 0, H * 2, W * 1, H * 2));
            SplitImageDraw(distBMP, srcImg, new Rectangle(W * 1, H * 2, W * 1, H * 2));
            SplitImageDraw(distBMP, srcImg, new Rectangle(W * 2, H * 2, W * 1, H * 2));
            SplitImageDraw(distBMP, srcImg, new Rectangle(W * 3, H * 2, W * 3, H * 3));

            // 解像度を設定

            distBMP.SetResolution(srcImg.HorizontalResolution, srcImg.VerticalResolution);

            srcImg.Dispose();
        }

        public static Bitmap ConvertPixelFormat(Bitmap bmp, System.Drawing.Imaging.PixelFormat pxf)
        {

            if (bmp.PixelFormat == pxf) return bmp;

            var bmp2 = new Bitmap(bmp.Width, bmp.Height, pxf);

            using (var g = Graphics.FromImage(bmp2))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.DrawImageUnscaled(bmp, 0, 0);
            };

            return bmp2;
        }


        /// <summary>
        /// ストリング文字列をImageオブジェクト化
        /// </summary>
        /// <param name="drawString">描画する文字列</param>
        /// <param name="fontName">フォント名</param>
        /// <param name="fontSize">フォント大きさ</param>
        /// <param name="width">イメージの幅</param>
        /// <param name="height">イメージのオフセット</param>
        /// <param name="my"></param>
        /// <param name="mx"></param>
        /// <returns></returns>
        public static System.Drawing.Image TextToImage(string drawString, string fontName, int fontSize, int width, int height, int mx, int my)
        {
            Console.WriteLine("drawString={0}\n", drawString);
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(width, height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            //Fontを作成
            System.Drawing.Font fnt = new System.Drawing.Font(fontName, fontSize);
            //文字列を表示する範囲を指定する
            RectangleF rectbase = new RectangleF(mx, my, width, height);
            //rectの四角を描く
            g.FillRectangle(Brushes.White, rectbase);

            //文字列を表示する範囲を指定する
            RectangleF rect = new RectangleF(mx, my, width - mx, height - my);
            //文字を書く
            g.DrawString(drawString, fnt, Brushes.Black, rect);

            //リソースを解放する
            fnt.Dispose();
            g.Dispose();

            return canvas;
        }

        /// <summary>
        /// System.Drawing.ImageをSystem.Windows.Controls.Imageに変換する
        /// </summary>
        /// <param name="gdiImg"></param>
        /// <returns></returns>
        public static System.Windows.Controls.Image ConvertDrawingImageToWPFImage(System.Drawing.Image gdiImg)
        {


            System.Windows.Controls.Image img = new System.Windows.Controls.Image();

            //convert System.Drawing.Image to WPF image
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(gdiImg);
            IntPtr hBitmap = bmp.GetHbitmap();
            System.Windows.Media.ImageSource WpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            img.Source = WpfBitmap;
            img.Width = 500;
            img.Height = 600;
            img.Stretch = System.Windows.Media.Stretch.Fill;
            return img;
        }

        /// <summary>
        /// System.Windows.Media.Imaging.BitmapSource から System.Drawing.Bitmapを得る
        /// </summary>
        /// <param name="bitmapsource"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap BitmapSourceToBitmap(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
                bitmap.SetResolution((float)bitmapsource.DpiX, (float)bitmapsource.DpiY);
                return bitmap;
            }
        }

        /// <summary>
        /// TIFFイメージが格納されているStreamから、Imageオブジェクト(bitmap)を生成
        /// </summary>
        /// <param name="tiffStream"></param>
        /// <returns></returns>
        public static System.Drawing.Image TiffStreamToImage(Stream tiffStream)
        {
            Bitmap bitmapOrg = new Bitmap(tiffStream);

            // 画像解像度を変更して新しいBitmapオブジェクトを作成
            Bitmap bmpNew = new Bitmap(bitmapOrg.Width, bitmapOrg.Height);
            bmpNew.SetResolution(bitmapOrg.HorizontalResolution, bitmapOrg.VerticalResolution);

            // 新しいBitmapオブジェクトに元の画像内容を描画
            Graphics g = Graphics.FromImage(bmpNew);
            g.DrawImage(bitmapOrg, 0, 0, bitmapOrg.Width, bitmapOrg.Height);
            g.Dispose();

            // System.Drawing.Image im = new System.Drawing.Bitmap(bmpNew);
            bitmapOrg.Dispose();
            return bmpNew;
        }

        /// <summary>
        /// TIFFイメージが格納されているStreamから Bitmapオブジェクトをここで生成.Imageで返す。
        /// </summary>
        /// <param name="tiffStream"></param>
        /// <returns></returns>
        public static System.Drawing.Image TiffStreamToImage2(Stream tiffStream)
        {
            System.Drawing.Image newImage = new Bitmap(tiffStream);

            return newImage;
        }


        // 上書き描画関連

        /// <summary>
        /// フォントをイメージオブジェクトに上書きする
        /// </summary>
        /// <param name="newImage"></param>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
        /// <param name="fontName">フォントネーム</param>
        /// <param name="fontsize">フォントサイズ</param>
        public static void DrawFontToImage(System.Drawing.Image newImage, string str, long posx, long posy, string fontName, int fontsize)
        {
            // ImageオブジェクトからGraphicsオブジェクトを生成
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(newImage);

            //文字列描画する
            System.Drawing.Font fnt = new System.Drawing.Font(fontName, fontsize);
            gr.DrawString(str, fnt, System.Drawing.Brushes.Black, posx, posy);
            fnt.Dispose();
            gr.Dispose();
        }


        /// <summary>
        /// X方向のセンタリング対応
        /// </summary>
        /// <param name="newImage"></param>
        /// <param name="text"></param>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
        /// <param name="fontName"></param>
        /// <param name="fontsize"></param>
        /// <param name="centering"></param>
        /// <param name="posEndx"></param>
        public static void DrawFontToImage(System.Drawing.Image newImage, string text, long posx, long posy, string fontName, int fontsize, bool centering = false, long posEndx = 0,bool debugmode = false)
        {
            // ImageオブジェクトからGraphicsオブジェクトを生成
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(newImage);

            // フォントを設定
            System.Drawing.Font font = new System.Drawing.Font(fontName, fontsize);

            // サイズを計測
            SizeF textSize = gr.MeasureString(text, font);
            long textWidht = (long)textSize.Width;
            long textHeight = (long)textSize.Height;

            if (debugmode)
            {
                // テストモードなら枠を描画
                Pen pen = new Pen(Color.Black, width: 1);
                if (posEndx > 0)
                    gr.DrawRectangle(pen, posx, posy, posEndx, textHeight);
                else
                    gr.DrawRectangle(pen, posx, posy, textWidht, textHeight);
            }

            if (centering == true)
            {
                long targetWidth = posEndx - posx;

                long centeringSlidePosx = (targetWidth - textWidht) / 2; // センタリングの場合,posxからずらす距離

                posx = posx + centeringSlidePosx;    // ｘを移動         

            }



            //文字列描画する
            gr.DrawString(text, font, System.Drawing.Brushes.Black, posx, posy);

            font.Dispose();
            gr.Dispose();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newImage"></param>
        /// <param name="str"></param>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
        /// <param name="fontName"></param>
        /// <param name="fontsize"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawFontToImage(System.Drawing.Image newImage, string str, long posx, long posy, string fontName, int fontsize, out int width, out int height)
        {
            // ImageオブジェクトからGraphicsオブジェクトを生成
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(newImage);

            //文字列描画する
            System.Drawing.Font fnt = new System.Drawing.Font(fontName, fontsize);
            gr.DrawString(str, fnt, System.Drawing.Brushes.Black, posx, posy);

            SizeF stringSize = gr.MeasureString(str, fnt);
            Size sz = stringSize.ToSize();
            width = sz.Width;
            height = sz.Height;


            fnt.Dispose();
            gr.Dispose();
        }

        /// <summary>
        /// イメージを別のイメージに上書き。互換性のためのラッパー
        /// </summary>
        /// <param name="srcImage"></param>
        /// <param name="stampImg"></param>
        /// <param name="pf">不要です</param>
        /// <param name="dXmm"></param>
        /// <param name="dYmm"></param>
        public static void OverwritingImage_Old(System.Drawing.Image srcImage, System.Drawing.Image stampImg, System.Drawing.Imaging.PixelFormat pf, float dXmm, float dYmm)
        {
            OverwritingImage_Old(srcImage, stampImg, dXmm, dYmm);
        }

        /// <summary>
        /// イメージを別のイメージに上書き。
        /// </summary>
        /// <param name="srcImage">追加する元のベースイメージ.このメソッドの呼び出し後変更されます</param>
        /// <param name="stampImg">追加するイメージ</param>
        /// <param name="pf">新しいピクセルフォーマット</param>
        /// <param name="dXmm">右下を原点とした場合の横方向移動量</param>
        /// <param name="dYmm">右下を原点とした場合の上下方向移動量</param>
        private static void OverwritingImage_Old(System.Drawing.Image srcImage, System.Drawing.Image stampImg, float dXmm, float dYmm)
        {
            // 
            float HDPI = srcImage.HorizontalResolution;
            float VDPI = srcImage.VerticalResolution;

            float dPX = (dXmm / 25.4f) * HDPI;
            float dPY = (dYmm / 25.4f) * VDPI;

            // 描画先のDPIに合わせてスケール変更
            stampImg = ImageUtil.Myresize(stampImg, (HDPI / stampImg.HorizontalResolution) / 5);

            // Graphicsオブジェクトに接続
            Graphics gr_srcImage = Graphics.FromImage(srcImage);
            // ビットマップは左上が原点。
            // スタンプＸ座標＝ソースイメージ幅ースタンプイメージ幅ーソースイメージ右からスタンプイメージ右までの距離
            int x = (srcImage.Width - stampImg.Width) - (int)dPX;
            int y = (srcImage.Height - stampImg.Height) - (int)dPY;

            gr_srcImage.DrawImage(stampImg, x, y, stampImg.Width, stampImg.Height);
            gr_srcImage.Dispose();

        }

        /// <summary>
        /// イメージを別のイメージに上書き。
        /// </summary>
        /// <param name="srcImage">追加する元のベースイメージ.Dpiは正確に設定しておくことこのメソッドの呼び出し後変更されます</param>
        /// <param name="stampImg">追加するイメージ Dpiは正確に設定しておくこと</param>
        /// <param name="dXmm">右下を原点とした場合の横方向移動量</param>
        /// <param name="dYmm">右下を原点とした場合の上下方向移動量</param>
        public static void OverwritingImage(System.Drawing.Image srcImage, System.Drawing.Image stampImg, float dXmm, float dYmm)
        {
            // 
            float HDPI = srcImage.HorizontalResolution;
            float VDPI = srcImage.VerticalResolution;

            float dPX = (dXmm / 25.4f) * HDPI;
            float dPY = (dYmm / 25.4f) * VDPI;

            var s = (HDPI / stampImg.HorizontalResolution);

            // 描画先のDPIに合わせてスケール変更
            stampImg = ImageUtil.Myresize(stampImg, s);

            // Graphicsオブジェクトに接続
            Graphics gr_srcImage = Graphics.FromImage(srcImage);
            // ビットマップは左上が原点。
            // スタンプＸ座標＝ソースイメージ幅ースタンプイメージ幅ーソースイメージ右からスタンプイメージ右までの距離
            int x = (srcImage.Width - stampImg.Width) - (int)dPX;
            int y = (srcImage.Height - stampImg.Height) - (int)dPY;

            gr_srcImage.DrawImage(stampImg, x, y, stampImg.Width, stampImg.Height);
            gr_srcImage.Dispose();

        }

        /// <summary>
        /// 指定したイメージの右下を原点とし、指定する位置に文字列を描画
        /// </summary>
        /// <param name="image"></param>
        /// <param name="UserName"></param>
        /// <param name="posAX"></param>
        /// <param name="posAY"></param>
        /// <param name="posBX"></param>
        /// <param name="posBY"></param>
        /// <param name="fonSize"></param>
        /// <returns></returns>
        public static bool DrawTextImageFromRightButtom(System.Drawing.Image image, string UserName, long posAX, long posAY, long posBX, long posBY, int fonSize)
        {
            // mm位置情報をDOT情報に変換するオブジェクトを生成
            MillimetreToDot dotP = new MillimetreToDot(image);

            //右下を原点に、ミリメーターで入力
            Point userNamePoint = dotP.RightButtomCordi(posAX, posAY);
            Point DateText = dotP.RightButtomCordi(posBX, posBY);

            Point UserNameTextPoint = dotP.RightButtomCordi(posAX, posAY);
            // 文字列イメージ上書き
            DrawFontToImage(image, UserName, UserNameTextPoint.X, UserNameTextPoint.Y, "Arial", fonSize);

            return true;
        }

        /// <summary>
        /// 指定イメージを塗りつぶし
        /// </summary>
        /// <param name="image"></param>
        /// <param name="brush">色：exp Brushes.White</param>
        public static void DrawFillImage(System.Drawing.Image image, System.Drawing.Brush brush, int offsetX = 0, int offsetY = 0)
        {
            // ImageオブジェクトからGraphicsオブジェクトを生成
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.FillRectangle(brush, offsetX, offsetY, image.Width - offsetX * 2, image.Height - offsetY * 2);
            g.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="brush"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="lineWidth"></param>
        public static void DrawRectangleLine(System.Drawing.Image image, Brush brush, int width, int height, int lineWidth)
        {
            // ImageオブジェクトからGraphicsオブジェクトを生成
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            Pen p = new Pen(brush, lineWidth);
            g.DrawRectangle(p, 0, 0, width - lineWidth, height - lineWidth);
            g.Dispose();
        }

        // 調査関連

        /// <summary>
        /// MimeTypeで指定されたImageCodecInfoを探して返す
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //GDI+ に組み込まれたイメージ エンコーダに関する情報をすべて取得
            System.Drawing.Imaging.ImageCodecInfo[] encs =
                System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            //指定されたMimeTypeを探して見つかれば返す
            foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs)
            {
                if (enc.MimeType == mimeType)
                {
                    return enc;
                }
            }
            return null;
        }

        /// <summary>
        /// ImageFormatで指定されたImageCodecInfoを探して返す
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoderInfo(ImageFormat f)
        {
            System.Drawing.Imaging.ImageCodecInfo[] encs =
                System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            foreach (System.Drawing.Imaging.ImageCodecInfo enc in encs)
            {
                if (enc.FormatID == f.Guid)
                {
                    return enc;
                }
            }
            return null;
        }

        /// <summary>
        /// Imageオブジェクトから用紙サイズを以上の基準で判定する
        /// A0(841×1189mm) A1(594×841mm) A2(420×594mm) A3(297×420mm) A4(210×297mm)
        /// </summary>
        /// <param name="image">Imageオブジェクト</param>
        /// <returns></returns>
        public static PaperSizeCabinet GetPaparSize(System.Drawing.Image image, double gosa = 2.5d, SasaLibDelegateWriteLine WriteLine = null)
        {
            PM imagePaperSize = GetPaperSizeMillimeter(image);

            List<PaperSizeCabinet> paperSizes = new List<PaperSizeCabinet>
            {
                new PaperSizeCabinet("A0縦", 841, 1189, CommonPaperSize.A0P),
                new PaperSizeCabinet("A0横", 1189, 841, CommonPaperSize.A0L),
                new PaperSizeCabinet("A1縦", 594, 841, CommonPaperSize.A1P),
                new PaperSizeCabinet("A1横", 841, 594, CommonPaperSize.A1L),
                new PaperSizeCabinet("A2縦", 420, 594, CommonPaperSize.A2P),
                new PaperSizeCabinet("A2横", 594, 420, CommonPaperSize.A2L),
                new PaperSizeCabinet("A3縦", 297, 420, CommonPaperSize.A3P),
                new PaperSizeCabinet("A3横", 420, 297, CommonPaperSize.A3L),
                new PaperSizeCabinet("A4縦", 210, 297, CommonPaperSize.A4P),
                new PaperSizeCabinet("A4横", 297, 210, CommonPaperSize.A4L)
            };

            /// 内部メソッド (比較)
            bool Nealist(double var, double _gosa, double target)
            {
                if (Math.Abs(var - target) <= _gosa)
                {
                    return true;
                }
                else
                    return false;
            }

            foreach (PaperSizeCabinet pc in paperSizes)
            {
                if (Nealist(pc.Width, gosa, imagePaperSize.Width) && Nealist(pc.Height, gosa, imagePaperSize.Height))
                {
                    if (WriteLine != null)
                        WriteLine($"★★★用紙サイズ(mm) Width={imagePaperSize.Width},Height={imagePaperSize.Height} 用紙名{pc.PaperName}");
                    return pc;
                }
            }
            if (WriteLine != null)
                WriteLine($"▼ImageUtil.GetPaparSize(...) 用紙サイズ(mm) 判別できませン {imagePaperSize.Width} , {imagePaperSize.Height}");
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        //public static string OLDGetPaparSize(System.Drawing.Image image)
        //{

        //    PM imagePaperSize = GetPaperSizeMillimeter(image);

        //    List<PaperSizeCabinet> paperSizes = new List<PaperSizeCabinet>
        //    {
        //        new PaperSizeCabinet("A0縦", 841, 1189, CommonPaperSize.A0P),
        //        new PaperSizeCabinet("A0横", 1189, 841, CommonPaperSize.A0L),
        //        new PaperSizeCabinet("A1縦", 594, 841, CommonPaperSize.A1P),
        //        new PaperSizeCabinet("A1横", 841, 594, CommonPaperSize.A1L),
        //        new PaperSizeCabinet("A2縦", 420, 594, CommonPaperSize.A2P),
        //        new PaperSizeCabinet("A2横", 594, 420, CommonPaperSize.A2L),
        //        new PaperSizeCabinet("A3縦", 297, 420, CommonPaperSize.A3P),
        //        new PaperSizeCabinet("A3横", 420, 297, CommonPaperSize.A3L),
        //        new PaperSizeCabinet("A4縦", 210, 297, CommonPaperSize.A4P),
        //        new PaperSizeCabinet("A4横", 297, 210, CommonPaperSize.A4L)
        //    };

        //    foreach (PaperSizeCabinet pc in paperSizes)
        //    {
        //        if (pc.Width == imagePaperSize.Width && pc.Height == imagePaperSize.Height)
        //        {
        //            //Console.WriteLine("★★★用紙サイズ(mm) Width={0},Height={1} 用紙名{2}", imagePaperSize.Width, imagePaperSize.Height, pc.PaperName);
        //            return pc.PaperName;
        //        }
        //    }
        //    Console.WriteLine("★★★用紙サイズ(mm) 判別できませン", imagePaperSize.Width, imagePaperSize.Height);
        //    return "";
        //}


        /// <summary>
        /// Imageオブジェクト から 実際のサイズをmmで計算する
        /// </summary>
        /// <param name="image">Imageオブジェクト</param>
        /// <returns>返り値はsasaLIB\PM</returns>
        public static PM GetPaperSizeMillimeter(System.Drawing.Image image)
        {
            Console.WriteLine("Imageオブジェクト から 実際のサイズをmmで計算します・・・");
            double mmWidth = image.Width / image.HorizontalResolution * 25.4;
            double mmHeight = image.Height / image.VerticalResolution * 25.4;
            PM pm;
            pm.Width = (int)Math.Round(mmWidth, 0, MidpointRounding.AwayFromZero);
            pm.Height = (int)Math.Round(mmHeight, 0, MidpointRounding.AwayFromZero);
            return pm;
        }

        /// <summary>
        /// imageオブジェクト から 実際のサイズをmmで計算する（解像度をImageオブジェクトから取得しない）
        /// </summary>
        /// <param name="image"></param>
        /// <param name="Dpi"></param>
        /// <returns></returns>
        public static PM GetPaperSizeMillimeter(System.Drawing.Image image, float Dpi, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine != null)
                WriteLine("Imageオブジェクト から 実際のサイズをmmで計算します・・・");
            double mmWidth = image.Width / Dpi * 25.4;
            double mmHeight = image.Height / Dpi * 25.4;
            PM pm;
            pm.Width = (int)Math.Round(mmWidth, 0, MidpointRounding.AwayFromZero);
            pm.Height = (int)Math.Round(mmHeight, 0, MidpointRounding.AwayFromZero);
            return pm;
        }

        // サイズ向き変換関連

        public static System.Drawing.Bitmap Myresize(System.Drawing.Image image, float scale)
        {
            int newW = Convert.ToInt32(image.Width * scale);
            int newH = Convert.ToInt32(image.Height * scale);

            var ret = Myresize(image, newW, newH);
            return ret;
        }
        /// <summary>
        /// imageオブジェクトのリサイズ
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width">ピクセル数</param>
        /// <param name="height">ピクセル数</param>
        /// <returns></returns>
        public static System.Drawing.Bitmap Myresize(System.Drawing.Image image, int width, int height)
        {
            //補間方法を指定して画像を縮小して描画する
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(width, height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);
            //補間方法として最近傍補間を指定する
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //補間方法として高品質双三次補間を指定する
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //画像を縮小して描画する
            g.DrawImage(image, 0, 0, width, height);
            //BitmapとGraphicsオブジェクトを破棄
            g.Dispose();
            return canvas;
        }

        /// <summary>
        /// 解像度を変更
        /// </summary>
        /// <param name="image"></param>
        /// <param name="DPI"></param>
        /// <param name="interpolationMode"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap ChangeResolution(System.Drawing.Image image, float DPI, System.Drawing.Drawing2D.InterpolationMode interpolationMode)
        {
            if (image.HorizontalResolution != DPI && image.VerticalResolution != DPI)
            {
                double scale = DPI / image.HorizontalResolution;

                double width = image.Width * scale;
                double height = image.Height * scale;
                int newWidth = Convert.ToInt32(width);
                int newHeight = Convert.ToInt32(height);

                //補間方法を指定して画像を縮小して描画する
                //描画先とするImageオブジェクトを作成する
                Bitmap canvas = new Bitmap(newWidth, newHeight);

                //解像度セット
                canvas.SetResolution(DPI, DPI);

                //ImageオブジェクトのGraphicsオブジェクトを作成する
                Graphics g = Graphics.FromImage(canvas);

                //補間方法を指定する
                g.InterpolationMode = interpolationMode;

                //画像を縮小して描画する
                g.DrawImage(image, 0, 0, newWidth, newHeight);
                //BitmapとGraphicsオブジェクトを破棄
                g.Dispose();

                return canvas;

            }
            return (Bitmap)image;
        }

        public static System.Drawing.Bitmap ChangeSize(System.Drawing.Image orgImg, int newWidth, int newHeight, System.Drawing.Drawing2D.InterpolationMode interpolationMode)
        {
            //補間方法を指定して画像を縮小して描画する
            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(newWidth, newHeight);

            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics canvas_g = Graphics.FromImage(canvas);

            //補間方法を指定する
            //canvas_g.InterpolationMode = interpolationMode;

            //画像を縮小して描画する
            canvas_g.DrawImage(orgImg, 0, 0, newWidth, newHeight);
            //BitmapとGraphicsオブジェクトを破棄
            canvas_g.Dispose();

            return canvas;
        }


        public static System.Drawing.Bitmap ChangeResolution(System.Drawing.Bitmap orgbmg, float NewDPIH, float NewDPIV, System.Drawing.Drawing2D.InterpolationMode InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;
            var pformat = orgbmg.PixelFormat;

            float orgDPIH = orgbmg.HorizontalResolution; // 元のDPIを取得
            float orgDPIV = orgbmg.VerticalResolution; // 元のDPIを取得

            WriteLine($"オリジナルのbitmapの仕様 Width[pixcel]={orgbmg.Width},Height[pixcel]={orgbmg.Height},DPIH={orgDPIH},DPIV={orgDPIV}");

            double newWidthdouble = orgbmg.Width * NewDPIH / orgDPIH;
            double newHeightdouble = orgbmg.Height * NewDPIV / orgDPIV;
            WriteLine($"変換後のサイズ NewDPIH={NewDPIH}, (dobule)Width = {newWidthdouble}  ,   NewDPIV={NewDPIV}, (dobule)Height = {newHeightdouble}");

            int newWidth = Convert.ToInt32(newWidthdouble);
            int newHeight = Convert.ToInt32(newHeightdouble);
            WriteLine($"変換後のサイズ (int)Width = {newWidth}  ,  (int)Height = {newHeight}");

            //描画先とするImageオブジェクトを作成する
            Bitmap canvas = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            using (Graphics canvas_g = Graphics.FromImage(canvas)) //BitmapオブジェクトのGraphicsオブジェクトを作成する
            {
                //canvas_g.InterpolationMode = InterpolationMode; //補間方法を指定する
                canvas_g.DrawImage(orgbmg, 0, 0, newWidth, newHeight); //画像を縮小して描画する
                canvas_g.Dispose(); //BitmapとGraphicsオブジェクトを破棄
            }

            canvas.SetResolution(NewDPIH, NewDPIV);  // 新しいDPIを設定


            MemoryStream tiffStream = new MemoryStream();

            ImageUtil.ImageToTIFF1bppCCITT4Stream(canvas, tiffStream);
            System.Drawing.Image distimage = System.Drawing.Image.FromStream(tiffStream);

            tiffStream.Dispose();

            return canvas;
        }

        /// <summary>
        /// imageを回転. 指定されたImageオブジェクトを操作する
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="rotateFlipType"></param>
        public static void RotateImage(System.Drawing.Bitmap bmp, System.Drawing.RotateFlipType rotateFlipType)
        {
            bmp.RotateFlip(rotateFlipType);
        }

        /// <summary>
        /// イメージ移動 mmで指示
        /// </summary>
        /// <param name="image"></param>
        /// <param name="Xmilli"></param>
        /// <param name="Ymilli"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap MoveImageMilli(System.Drawing.Image image, float Xmilli, float Ymilli)
        {

            float rezolutionH = image.HorizontalResolution;
            float rezolutionV = image.VerticalResolution;


            float PX = MilliToPixel(Xmilli, rezolutionH);
            float PY = MilliToPixel(Ymilli, rezolutionV);

            Console.WriteLine("rezoH={0},rezoV={1}", rezolutionH, rezolutionV);

            Console.WriteLine("{0},{1} -> {2},{3}", Xmilli, Ymilli, PX, PY);

            var bmp = MoveImage(image, PX, PY);
            GC.Collect();
            return bmp;
        }

        /// <summary>
        /// イメージ移動 mmで指示. 1bppイメージ専用
        /// </summary>
        /// <param name="image"></param>
        /// <param name="Xmilli"></param>
        /// <param name="Ymilli"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap Move1bppImageMilli(System.Drawing.Image image, float Xmilli, float Ymilli)
        {

            float rezolutionH = image.HorizontalResolution;
            float rezolutionV = image.VerticalResolution;


            float PX = MilliToPixel(Xmilli, rezolutionH);
            float PY = MilliToPixel(Ymilli, rezolutionV);

            Console.WriteLine("rezoH={0},rezoV={1}", rezolutionH, rezolutionV);

            Console.WriteLine("{0},{1} -> {2},{3}", Xmilli, Ymilli, PX, PY);

            var bmp = Move1bppImage(image, PX, PY);
            //GC.Collect();
            return bmp;
        }

        /// <summary>
        /// イメージ移動 ピクセルで指示
        /// </summary>
        /// <param name="orgImage"></param>
        /// <param name="Xpont"></param>
        /// <param name="Ypoint"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap MoveImage(System.Drawing.Image orgImage, float Xpont, float Ypoint)
        {
            // 元のイメージから内容が同じイメージを作成
            System.Drawing.Bitmap newImage = new Bitmap(orgImage);

            //newImageについて全体を移動

            //newImageを操作するGraphicsオブジェクトを取得
            Graphics gr_newImage = Graphics.FromImage(newImage);

            //平行移動の準備
            System.Drawing.Drawing2D.Matrix mx = new System.Drawing.Drawing2D.Matrix();
            mx.Translate(Xpont, Ypoint);
            gr_newImage.Transform = mx;

            // newImaeについてイメージの移動を実行
            gr_newImage.DrawImage(newImage, new Point(0, 0));

            // オリジナルの解像度を元に再設定
            newImage.SetResolution(orgImage.HorizontalResolution, orgImage.VerticalResolution);

            return (Bitmap)newImage;
        }

        static bool dummy()
        {
            return false; // このメソッドの内容は何でもよい
        }

        /// <summary>
        /// イメージ移動 ピクセルで指示。1bpイメージ専用。System.Drawing.Image.GetThumbnailImage()を使用する。
        /// </summary>
        /// <param name="orgImage"></param>
        /// <param name="Xpont"></param>
        /// <param name="Ypoint"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap Move1bppImage(System.Drawing.Image orgImage, float Xpont, float Ypoint)
        {
            //GetThumbnailImageを利用して変換
            Image image = orgImage.GetThumbnailImage(orgImage.Width, orgImage.Height, new Image.GetThumbnailImageAbort(dummy), IntPtr.Zero);

            //newImageを操作するGraphicsオブジェクトを取得
            Graphics gr_newImage = Graphics.FromImage(image);

            //平行移動の準備
            System.Drawing.Drawing2D.Matrix mx = new System.Drawing.Drawing2D.Matrix();
            mx.Translate(Xpont, Ypoint);
            gr_newImage.Transform = mx;

            // newImaeについてイメージの移動を実行
            gr_newImage.DrawImage(image, new Point(0, 0));

            // 座標系をリセットする
            gr_newImage.ResetTransform();

            // オフセット指定が0以外なら、移動後の塗りつぶしを実行する
            if (Xpont != 0 || Ypoint != 0)
            {
                float x = 0;
                float y = 0;
                float Width = 0;
                float Height = 0;

                if (Xpont > 0)
                {
                    x = 0;
                    y = 0;
                    Width = Xpont;
                    Height = image.Height;
                }
                else if (Xpont < 0)
                {
                    x = image.Width - (Xpont * -1);
                    y = 0;
                    Width = image.Width;
                    Height = image.Height;
                }
                gr_newImage.FillRectangle(Brushes.White, x, y, Width, Height);

                if (Ypoint > 0)
                {
                    x = 0;
                    y = 0;
                    Width = image.Width;
                    Height = Ypoint;
                }
                else if (Ypoint < 0)
                {
                    x = 0;
                    y = image.Height - (Ypoint * -1);
                    Width = image.Width;
                    Height = image.Height;
                }
                gr_newImage.FillRectangle(Brushes.White, x, y, Width, Height);
            }

            // オリジナルの解像度を元に再設定
            Bitmap bmp = (Bitmap)image;
            bmp.SetResolution(orgImage.HorizontalResolution, orgImage.VerticalResolution);
            return bmp;
        }

        /// <summary>
        /// Bitmapの一部を切り出したBitmapオブジェクトを返す
        /// 使用方法は var bmpRoi = ImageRoi(bmpSrc, new Rectangle(150, 80, 100, 100));
        /// </summary>
        /// <param name="srcRect">元のBitmapクラスオブジェクト</param>
        /// <param name="roi">切り出す領域</param>
        /// <returns>切り出したBitmapオブジェクト</returns>
        public static System.Drawing.Bitmap ImageRoi(Bitmap src, Rectangle roi)
        {
            //////////////////////////////////////////////////////////////////////
            // srcRectとroiの重なった領域を取得（画像をはみ出した領域を切り取る）

            // 画像の領域
            var imgRect = new Rectangle(0, 0, src.Width, src.Height);
            // はみ出した部分を切り取る(重なった領域を取得)
            var roiTrim = Rectangle.Intersect(imgRect, roi);
            // 画像の外の領域を指定した場合
            if (roiTrim.IsEmpty == true) return null;

            //////////////////////////////////////////////////////////////////////
            // 画像の切り出し

            // 切り出す大きさと同じサイズのBitmapオブジェクトを作成
            var dst = new Bitmap(roiTrim.Width, roiTrim.Height, src.PixelFormat);
            // BitmapオブジェクトからGraphicsオブジェクトの作成
            var g = Graphics.FromImage(dst);
            // 描画先
            var dstRect = new Rectangle(0, 0, roiTrim.Width, roiTrim.Height);
            // 描画
            g.DrawImage(src, dstRect, roiTrim, GraphicsUnit.Pixel);
            // 解放
            g.Dispose();

            return dst;
        }


        //二値化、グレースケール関連

        /// <summary>
        /// 指定した画像からグレースケール画像を作成する
        /// </summary>
        /// <param name="img">基の画像</param>
        /// <returns>作成されたグレースケール画像</returns>
        public static System.Drawing.Image CreateGrayscaleImage(System.Drawing.Image img)
        {
            //グレースケールの描画先となるImageオブジェクトを作成
            Bitmap newImg = new Bitmap(img.Width, img.Height);
            //newImgのGraphicsオブジェクトを取得
            Graphics g = Graphics.FromImage(newImg);

            //ColorMatrixオブジェクトの作成
            //グレースケールに変換するための行列を指定する
            System.Drawing.Imaging.ColorMatrix cm =
                new System.Drawing.Imaging.ColorMatrix(
                    new float[][]{
                new float[]{0.299f, 0.299f, 0.299f, 0 ,0},
                new float[]{0.587f, 0.587f, 0.587f, 0, 0},
                new float[]{0.114f, 0.114f, 0.114f, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
                    });
            //ImageAttributesオブジェクトの作成
            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();
            //ColorMatrixを設定する
            ia.SetColorMatrix(cm);

            //ImageAttributesを使用してグレースケールを描画
            g.DrawImage(img,
                new Rectangle(0, 0, img.Width, img.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

            //リソースを解放する
            g.Dispose();

            return newImg;
        }

        /// <summary>
        /// 指定した画像からグレースケール画像を作成する(違う設定)
        /// </summary>
        /// <param name="img">基の画像</param>
        /// <returns>作成されたグレースケール画像</returns>
        public static System.Drawing.Image CreateGrayscaleImage2(System.Drawing.Image img)
        {
            //グレースケールの描画先となるImageオブジェクトを作成
            Bitmap newImg = new Bitmap(img.Width, img.Height);
            //newImgのGraphicsオブジェクトを取得
            Graphics g = Graphics.FromImage(newImg);

            //ColorMatrixオブジェクトの作成
            //グレースケールに変換するための行列を指定する
            System.Drawing.Imaging.ColorMatrix cm =
                new System.Drawing.Imaging.ColorMatrix(
                    new float[][]{
        new float[]{0.3086f, 0.3086f, 0.3086f, 0 ,0},
        new float[]{0.6094f, 0.6094f, 0.6094f, 0, 0},
        new float[]{0.0820f, 0.0820f, 0.0820f, 0, 0},
        new float[]{0, 0, 0, 1, 0},
        new float[]{0, 0, 0, 0, 1}
            });
            //ImageAttributesオブジェクトの作成
            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();
            //ColorMatrixを設定する
            ia.SetColorMatrix(cm);

            //ImageAttributesを使用してグレースケールを描画
            g.DrawImage(img,
                new Rectangle(0, 0, img.Width, img.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

            //リソースを解放する
            g.Dispose();

            return newImg;

        }

        /// <summary>
        /// ２値化する。(Bitmapオブジェクトを返す)
        /// </summary>
        /// <param name="img">Imageオブジェクト</param>
        /// <param name="fThresh">閾値 0～1 Long</param>
        /// <returns>Bitmapオブジェクト</returns>
        public static System.Drawing.Bitmap ImageBinarization(this System.Drawing.Image img, float fThresh)
        {
            Color color;
            Bitmap bmp = new Bitmap(img);

            // 指定されたビットマップから1dotずつの値を取得し、取得した値が閾値を超えているかチェックする
            for (Int32 y = 0; y < bmp.Size.Height; y++)
            {
                for (Int32 x = 0; x < bmp.Size.Width; x++)
                {
                    color = bmp.GetPixel(x, y);

                    // 閾値より大きい？
                    float fTemp = color.GetBrightness();
                    //Console.WriteLine("Brightness={0}", fTemp);
                    if (fTemp < fThresh)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }
                }
            }
            //一番下のラインのみ、バグ？　その対策
            for (int x = 0; x < bmp.Size.Width; x++)
            {
                bmp.SetPixel(x, bmp.Size.Height - 1, Color.White);
            }

            return bmp;
        }

        /// <summary>
        /// 2値下する。(Imageオブジェクトを返す)
        /// </summary>
        /// <param name="img"></param>
        /// <param name="fThresh"></param>
        /// <returns></returns>
        public static System.Drawing.Image ImageBinarizationOutImage(this System.Drawing.Image img, float fThresh)
        {
            Color color;
            Bitmap bmp = new Bitmap(img);

            // 指定されたビットマップから1dotずつの値を取得し、取得した値が閾値を超えているかチェックする
            for (Int32 y = 0; y < bmp.Size.Height; y++)
            {
                for (Int32 x = 0; x < bmp.Size.Width; x++)
                {
                    color = bmp.GetPixel(x, y);

                    // 閾値より大きい？
                    float fTemp = color.GetBrightness();
                    //Console.WriteLine("Brightness={0}", fTemp);
                    if (fTemp < fThresh)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }
                }
            }
            //一番下のラインのみ、バグ？　その対策
            for (int x = 0; x < bmp.Size.Width; x++)
            {
                bmp.SetPixel(x, bmp.Size.Height - 1, Color.White);
            }

            return img;
        }

        /// <summary>
        /// 指定された画像からネガティブイメージを作成する
        /// </summary>
        /// <param name="img">基の画像</param>
        /// <returns>作成されたネガティブイメージ</returns>
        public static System.Drawing.Image CreateNegativeImage(Image img)
        {
            //ネガティブイメージの描画先となるImageオブジェクトを作成
            Bitmap negaImg = new Bitmap(img.Width, img.Height);
            //negaImgのGraphicsオブジェクトを取得
            Graphics g = Graphics.FromImage(negaImg);

            //ColorMatrixオブジェクトの作成
            System.Drawing.Imaging.ColorMatrix cm =
                new System.Drawing.Imaging.ColorMatrix();
            //ColorMatrixの行列の値を変更して、色が反転されるようにする
            cm.Matrix00 = -1;
            cm.Matrix11 = -1;
            cm.Matrix22 = -1;
            cm.Matrix33 = 1;
            cm.Matrix40 = cm.Matrix41 = cm.Matrix42 = cm.Matrix44 = 1;

            //ImageAttributesオブジェクトの作成
            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();
            //ColorMatrixを設定する
            ia.SetColorMatrix(cm);

            //ImageAttributesを使用して色が反転した画像を描画
            g.DrawImage(img,
                new Rectangle(0, 0, img.Width, img.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);

            //リソースを解放する
            g.Dispose();

            return negaImg;
        }


        // ファイル入出力処理関連

        /// <summary>
        /// Imageオブジェクトを指定形式に変換し保存。フォルダがない場合は作成される。同名ファイルは上書き
        /// exp ImageUtil.SaveImageToFile(image, "image/tiff", EncoderValue.CompressionCCITT4, @"D:\TEST\test.tiff");
        /// </summary>
        /// <param name="img"></param>
        /// <param name="mimeType"></param>
        /// <param name="compress"></param>
        /// <param name="distFilePath"></param>
        /// <returns></returns>
        public static bool SaveImageToFile(System.Drawing.Image img, string mimeType, EncoderValue compress, string distFilePath)
        {
            string folder = Path.GetDirectoryName(distFilePath);
            string fileName = Path.GetFileName(distFilePath);

            bool ans = SaveImageToFile(img, mimeType, compress, folder, fileName);
            return ans;
        }

        /// <summary>
        /// Imageオブジェクトを指定形式に変換し保存。フォルダがない場合は作成される。同名ファイルは上書き
        /// exp ImageUtil.SaveImageToFile(image, "image/tiff", EncoderValue.CompressionCCITT4, @"D:\TEST", @"test.tiff");
        /// </summary>
        /// <param name="img">System.Windows.Image</param>
        /// <param name="mimeType">MIME Type </param>
        /// <param name="compress"></param>
        /// <param name="folder">保存先フォルダ(最後に\はつけないこと)</param>
        /// <param name="fileName">保存ファイル名</param>
        /// <returns></returns>
        public static bool SaveImageToFile(System.Drawing.Image img, string mimeType, EncoderValue compress, string folder, string fileName)
        {
            string fullpath = folder.TrimEnd(System.IO.Path.DirectorySeparatorChar) + System.IO.Path.DirectorySeparatorChar + fileName;
            //Console.WriteLine("WriteFilePath={0}\n", fullpath);
            //Console.WriteLine("img.RawFormat={0}\n", img.RawFormat);

            // ImageCodecInfoを取得する
            ImageCodecInfo ici = GetEncoderInfo(mimeType);
            if (ici == null)
                return false;

            // オプション設定
            EncoderParameters ep = new EncoderParameters(1);
            //// カラー深度
            //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 24L);
            // 圧縮方法を指定する
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)compress);

            try
            {
                //ディレクトリの存在確認.なければ作成
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(fullpath)))
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullpath));
                    Console.WriteLine("ディレクトリ新規作成{0}\n", System.IO.Path.GetDirectoryName(fullpath));
                }
                img.Save(fullpath, ici, ep);

                return true;
            }
            catch (IOException ioex)
            {
                Eventlog.Log.WriteEntry("SasaLib ImageUtil Class", EventLogEntryType.Error, 0, $"▲SaveImageToFile(System.Drawing.Image img, string mimeType, EncoderValue compress, string {folder}, string {fileName}), 失敗,IOException={ioex.Message}");

                return false;
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib ImageUtil Class", EventLogEntryType.Error, 0, $"▲SaveImageToFile(System.Drawing.Image img, string mimeType, EncoderValue compress, string {folder}, string {fileName}), 失敗,Exception={ex.Message}");

                return false;
            }

        }

        /// <summary>
        /// System.Windows.Imageオブジェクトを指定形式に変換しファイル保存。タイムスタンプ指定
        /// フォルダがない場合は作成されます
        /// </summary>
        /// <param name="img">System.Windows.Imageクラスのオブジェクト</param>
        /// <param name="mimeType">ファイルの種類</param>
        /// <param name="compress">圧縮方法</param>
        /// <param name="distFilePath">保存ファイルパス</param>
        /// <param name="dateTime"></param>
        /// <param name="absSave"></param>
        /// <returns></returns>
        public static bool SaveImageToFile(System.Drawing.Image img, string mimeType, EncoderValue compress, string distFilePath, DateTime dateTime, bool absSave)
        {

            Console.WriteLine("img.RawFormat={0}\n", img.RawFormat);
            Console.WriteLine("distFilePath={0}\n", distFilePath);
            Console.WriteLine("DateTIme.Now={0}\n", DateTime.Now);

            // ImageCodecInfoを取得する
            ImageCodecInfo ici = GetEncoderInfo(mimeType);
            if (ici == null)
                return false;

            // オプション設定
            EncoderParameters ep = new EncoderParameters(1);
            //// カラー深度
            //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 24L);
            // 圧縮方法を指定する
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)compress);

            //

            try
            {
                bool save = false;

                //ディレクトリの存在確認.なければ作成
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(distFilePath)))
                {
                    //Directory.CreateDirectory・・・存在している以外は
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(distFilePath));
                    Console.WriteLine("ディレクトリ新規作成{0}\n", System.IO.Path.GetDirectoryName(distFilePath));
                }
                // ファイルの存在確認
                if (FileFolder.FileExists(distFilePath))
                // 存在した場合
                {
                    // 存在したファイルのタイムスタンプを取得
                    DateTime existFfileTime = System.IO.File.GetCreationTime(distFilePath);

                    // 存在したファイルの方が、書き込もうとしたfileのタイムスタンプより古い場合
                    if (existFfileTime < dateTime)
                    {
                        save = true;
                        Console.WriteLine("同名ファイルが存在するが古いため上書きします:{0}\n", distFilePath);
                    }
                    else
                    {
                        if (absSave)
                        {
                            save = true;
                            Console.WriteLine("強制保存モードです:{0}\n", distFilePath);
                        }
                        else
                        {
                            save = false;
                            Console.WriteLine("同名ファイルが存在するが新しいため上書きしません:{0}\n", distFilePath);
                        }
                    }
                }
                // ファイルが存在しない
                else
                {
                    save = true;
                }
                // フラグによってセーブするかを決める。
                if (save)
                {
                    img.Save(distFilePath, ici, ep);
                    //img.Dispose();
                    //
                    System.IO.File.SetCreationTime(distFilePath, dateTime);
                    System.IO.File.SetLastWriteTime(distFilePath, dateTime);
                    System.IO.File.SetLastAccessTime(distFilePath, dateTime);

                }
                //img.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib ImageUtil Class", EventLogEntryType.Error, 0, $"▲SaveImageToFile(System.Drawing.Image img, string mimeType, EncoderValue compress, string distFilePath, DateTime dateTime, bool absSave), 失敗,Exception={ex.Message}");

                Console.WriteLine("イメージ保存に例外発生＝{0}", ex.Message);
                //img.Dispose();
                return false;
            }
        }

        /// <summary>
        /// System.Windows.Imageオブジェクトを指定形式でファイル保存。タイムスタンプ指定
        /// </summary>
        /// <param name="img">Imageオブジェクト 例： SaSaImageUtil.ByteArrayToImage(bytes)</param>
        /// <param name="mimeType">MimeType 例： "image/tiff"</param>
        /// <param name="compress">EncoderValue.CompressionCCITT4</param>
        /// <param name="folder">保存先フォルダ(なければ生成)  例：D:\TEST\ABC (最後に\なしのこと)</param>
        /// <param name="fileName">保存ファイル名 例：M-10201-123RL.tiff (拡張子を含めて渡すこと)</param>
        /// <param name="dateTime">比較するためのタイムスタンプ。このタイムスタンプより既存イメージが古ければ上書きされる</param>
        /// <param name="absSave">trueの場合強制上書き</param>
        /// <returns></returns>
        public static bool SaveImageToFile(System.Drawing.Image img, string mimeType, EncoderValue compress, string folder, string fileName, DateTime dateTime, bool absSave)
        {

            string fullpath = folder.TrimEnd(System.IO.Path.DirectorySeparatorChar) + System.IO.Path.DirectorySeparatorChar + fileName;
            //Console.WriteLine("WriteFilePath={0}\n", fullpath);
            //Console.WriteLine("img.RawFormat={0}\n", img.RawFormat);

            // ImageCodecInfoを取得する
            ImageCodecInfo ici = GetEncoderInfo(mimeType);
            if (ici == null)
                return false;

            // オプション設定
            EncoderParameters ep = new EncoderParameters(1);
            //// カラー深度
            //ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.ColorDepth, 24L);
            // 圧縮方法を指定する
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)compress);

            //

            try
            {
                bool save = false;

                //ディレクトリの存在確認.なければ作成
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(fullpath)))
                {
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullpath));
                    Console.WriteLine("ディレクトリ新規作成{0}\n", System.IO.Path.GetDirectoryName(fullpath));
                }
                // ファイルの存在確認
                if (FileFolder.FileExists(fullpath))
                // 存在した場合
                {
                    // 存在したファイルのタイムスタンプを取得
                    DateTime existFfileTime = System.IO.File.GetCreationTime(fullpath);

                    // 存在したファイルの方が、書き込もうとしたfileのタイムスタンプより古い場合
                    if (existFfileTime < dateTime)
                    {
                        save = true;
                        Console.WriteLine("イラストが存在し、古かったの上書します{0}\n", fullpath);
                    }
                    else
                    {
                        if (absSave)
                        {
                            save = true;
                        }
                        else
                        {
                            save = false;
                            Console.WriteLine("イラストが存在し、新しかったので上書をやめました{0}\n", fullpath);
                        }
                    }
                }
                // ファイルが存在しない
                else
                {
                    save = true;
                }
                // フラグによってセーブするかを決める。
                if (save)
                {
                    img.Save(fullpath, ici, ep);
                    //img.Dispose();
                    //
                    System.IO.File.SetCreationTime(fullpath, dateTime);
                    System.IO.File.SetLastWriteTime(fullpath, dateTime);
                    System.IO.File.SetLastAccessTime(fullpath, dateTime);

                }
                //               img.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("▲SasaLib ImageUtil Class", EventLogEntryType.Error, 0, $"SaveImageToFile(System.Drawing.Image img, string mimeType, EncoderValue compress, string folder, string fileName, DateTime dateTime, bool absSave), 失敗,Exception={ex.Message}");

                //img.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 指定したパスのイメージファイルから、BitmapImageクラスにイメージを読み込む　isLocked=true で ファイルをロック
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isLocked"></param>
        /// <returns></returns>
        public static BitmapImage GetBitmapImage(string filePath, bool isLocked = false)
        {
            BitmapImage bmpImage = new BitmapImage();
            if (isLocked == false)
            {
                FileStream stream = File.OpenRead(filePath);
                // BitmapImageにファイルから画像を読み込む
                bmpImage.BeginInit();
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.StreamSource = stream;
                bmpImage.EndInit();
                stream.Close();
                return bmpImage;
            }
            else
            {
                // 既に読み込まれていたら解放する
                if (bmpImage != null) { bmpImage = null; }
                // BitmapImageにファイルから画像を読み込む
                bmpImage = new BitmapImage();
                bmpImage.BeginInit();
                bmpImage.UriSource = new Uri(filePath);
                bmpImage.EndInit();
                return bmpImage;
            }
        }

        /// <summary>
        /// 画像ファイルをよみ System.Drawing.Imageクラスのオブジェクトを生成（バイト配列を返す）
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileFullPath"></param>
        /// <returns>byte配列のImageオブジェクト、fileなし＝null</returns>
        public static byte[] GetTImageFileToByteArray(string fileFullPath)
        {
            /// Tifffile --> |Image.FromFile()| --> Image --> |ImageToByteArry()|  --> 

            System.Drawing.Image img;

            if (FileFolder.FileExists(fileFullPath) == true)
            {
                img = System.Drawing.Image.FromFile(fileFullPath);


                byte[] byteArray = ImageToByteArry(img);
                return byteArray;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 指定したファイルからImageオブジェクトへ読み込み
        /// </summary>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetImageFromFile(string fileFullPath)
        {
            if (FileFolder.FileExists(fileFullPath) == true)
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(fileFullPath);
                return img;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 指定したファイルをロックせずに、System.Drawing.Imageを作成する。
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image。</returns>
        public static System.Drawing.Image CreateImageFromFile(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return img;
        }

        /// <summary>
        /// 指定したファイルをロックせずに、System.Drawing.Imageを作成する。
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image。</returns>
        public static System.Drawing.Image FromFile(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
                fs.Close();
                return img;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ImageUtil.FromFile({filename})にて例外発生. 内容{ex.Message}");
                fs.Close();
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static System.Drawing.Image GetCCITT4ImageFromFile(string fullPath, bool GCExecute = false)
        {
            if (GCExecute == true)
                GC.Collect();
            try
            {
                System.Drawing.Image CurrentImage;

                /// ①ImageオブジェクトorgをTIFFファイルから作成
                System.Drawing.Image org = System.Drawing.Image.FromFile(fullPath);
                /// ②メモリストリームmsを作成
                MemoryStream ms = new MemoryStream();
                /// ③メモリストリームmsにImageオブジェクトobjをCCITT4圧縮し作成
                ImageUtil.ImageToTIFF1bppCCITT4Stream(org, ms);
                /// ④不要になったImageオブジェクトobjを強制解放
                org.Dispose();
                /// ⑤ メモリストリームmsをImageオブジェクトに戻し currentImageに作成
                CurrentImage = ImageUtil.TiffStreamToImage(ms);
                /// ⑥不要になったメモリストリームmsを強制解放
                ms.Dispose();

                return CurrentImage;
            }
            catch (IOException ioex)
            {
                Eventlog.Log.WriteEntry("SASALIB", EventLogEntryType.Error, 9000, $"：{ioex.Message}");
                return null;
            }
        }

        // ImageUtil.SaveImageToFile(_pl.PartImageData.ImageBinarization(0.9f), "image/tiff", EncoderValue.CompressionCCITT4, folder, _pl.TechsPartNumber + ".tiff", _pl.WordFileTimeStamp, OptSw.AbsoluteIllustSave);

        // 座標変換関連

        /// <summary>
        /// 用紙の識別名と対応する縦と横の長さ
        /// </summary>
        public class PaperSizeCabinet
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="name">識別名</param>
            /// <param name="width">用紙の縦幅 (mm)</param>
            /// <param name="height">用紙の縦幅 (mm)</param>
            /// <param name="commonPaperSize">CommonPaperSize (各種アプリで使う標準用紙サイズと向き)</commonPaperSize>
            public PaperSizeCabinet(string name, int width, int height, CommonPaperSize commonPaperSize)
            {
                PaperName = name;
                Width = width;
                Height = height;
                CommonPaperSize = commonPaperSize;
            }

            public string PaperName { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }

            public CommonPaperSize CommonPaperSize { get; set; }
        }

        /// <summary>
        /// ミリメートル座標をピクセル座標に変換
        /// </summary>
        public class MillimetreToDot
        {
            private readonly float rezolutionH;
            private readonly float rezolutionV;

            private readonly long hMax;
            private readonly long wMax;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="orgin">System.Drawing.Image クラス</param>
            public MillimetreToDot(System.Drawing.Image orgin)
            {
                rezolutionH = orgin.HorizontalResolution;
                rezolutionV = orgin.VerticalResolution;
                hMax = orgin.Height;
                wMax = orgin.Width;
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="height">イメージ高さ（ドット数）</param>
            /// <param name="width">イメージ幅（ドット数）</param>
            /// <param name="dpi">イメージDPI</param>
            public MillimetreToDot(long height, long width, float dpi)
            {
                rezolutionH = dpi;
                rezolutionV = dpi;
                hMax = height;
                wMax = width;
            }

            /// <summary>
            /// 左上原点座標系　ミリメートル座標をドット座標に変更
            /// </summary>
            /// <param name="width_mm"></param>
            /// <param name="height_mm"></param>
            /// <returns></returns>
            public System.Drawing.Point LeftTopCordi(double width_mm, double height_mm)
            {
                System.Drawing.Point ans;

                int w = (int)(width_mm / 25.4 * rezolutionH);
                int H = (int)(height_mm / 25.4 * rezolutionV);

                ans = new System.Drawing.Point(w, H);
                return ans;
            }

            /// <summary>
            /// 右下原点座標系　ミリメートル座標をドット座標に変更
            /// </summary>
            /// <param name="width_mm"></param>
            /// <param name="height_mm"></param>
            /// <returns></returns>
            public System.Drawing.Point RightButtomCordi(double width_mm, double height_mm)
            {
                System.Drawing.Point ans;

                int w2 = (int)(width_mm / 25.4 * rezolutionH);
                int h2 = (int)(height_mm / 25.4 * rezolutionV);

                int W = (int)(wMax - w2);
                int H = (int)(hMax - h2);

                ans = new System.Drawing.Point(W, H);
                return ans;
            }

        }

        /// <summary>
        /// ミリメートルをピクセルに変換。拡張メソッド
        /// </summary>
        /// <param name="milli"></param>
        /// <param name="Resolution"></param>
        /// <returns></returns>
        public static long MilliToPixel(this float milli, float Resolution)
        {
            return (long)((float)milli * Resolution / 25.4f);
        }
        /// -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// ピクセルフォーマット変更。速い
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="pixelFormat"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap ChangePixelFormat(System.Drawing.Bitmap bitmap, System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);

            System.Drawing.Imaging.BitmapData bitmapData = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, pixelFormat);
            try
            {
                System.Drawing.Bitmap convertedBitmap = new System.Drawing.Bitmap(bitmap.Width, bitmap.Height, pixelFormat);
                System.Drawing.Imaging.BitmapData convertedBitmapData = convertedBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly, pixelFormat);
                try
                {
                    NativeMethods.CopyMemory(convertedBitmapData.Scan0, bitmapData.Scan0, (uint)bitmapData.Stride * (uint)bitmapData.Height);
                }
                finally
                {
                    convertedBitmap.UnlockBits(convertedBitmapData);
                }
                convertedBitmap.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
                return convertedBitmap;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        static class NativeMethods
        {

            const string KERNEL32 = "Kernel32.dll";

            [System.Runtime.InteropServices.DllImport(KERNEL32)]
            public extern static void CopyMemory(IntPtr dest, IntPtr src, uint length);

        }

        /// <summary>
        /// 指定されたファイルがロックされているかどうかを返します。
        /// </summary>
        /// <param name="path">検証したいファイルへのフルパス</param>
        /// <returns>ロックされているかどうか</returns>
        public static bool IsFileLocked(string path)
        {
            FileStream stream = null;

            try
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return false;
        }

    }
}
