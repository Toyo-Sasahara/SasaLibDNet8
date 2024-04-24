using SasaLibDummy;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.Versioning;
// イメージから線画を描画
namespace SasaLib
{
    [SupportedOSPlatform("windows")]

    /// <summary>
    /// 画像の輪郭抽出
    /// Copyright https://algorithm.joho.info/
    /// 
    /// </summary>
    public static partial class ImageUtil
    {
        /// <summary>
        /// 輪郭抽出フィルタ
        /// </summary>
        public enum OutlineExtractionFilter
        {
            /// <summary>
            /// 一次微分フィルタ
            /// </summary>
            FirstDerivative,
            /// <summary>
            /// ソーベルフィルタ
            /// </summary>
            Sobel,
            /// <summary>
            /// ラプラシアンフィルタ 
            /// </summary>
            Laplacian,
            /// <summary>
            /// プレヴィットフィルタ
            /// </summary>
            Prewitt
        }

        /// <summary>
        /// `イメージファイルを輪郭抽出・イメージサイズ変更・２値化して同じ名前で
        /// </summary>
        /// <param name="OrginalImageFile">入力ファイル</param>
        /// <param name="width">幅のドット数</param>
        /// <param name="height">高さのドット数</param>
        /// <returns></returns>
        public static string CreateOutline_extractionTiffCCITT4Image(string OrginalImageFile, int width, int height)
        {
            if (width > 0 && height > 0)
            {
                //
                string folder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                //
                string tiffname = FileFolder.ChangeExtension(OrginalImageFile, "TIF");
                //
                string tempbmpfile2 = System.IO.Path.Combine(folder, tiffname);
                //
                System.Drawing.Image outImage = ImageUtil.FromFile(OrginalImageFile);
                ///
                outImage = CreateOutline_extractionTiffCCITT4Image(outImage, width, height);

                MemoryStream tiffStream = new MemoryStream();

                ImageUtil.ImageToTIFF1bppCCITT4Stream(outImage, tiffStream);

                StreamExtensions.StreamToFile(tiffStream, tempbmpfile2);

                // 後始末
                tiffStream.Dispose();

                return tempbmpfile2;
            }
            else
            {
                throw new FormatException("width または　height がゼロです");
            }
        }

        /// <summary>
        /// イメージファイルを輪郭抽出・サイズ変更
        /// </summary>
        /// <param name="inPutImage">入力イメージ</param>
        /// <param name="width">変更するサイズ</param>
        /// <param name="height">変更するサイズ</param>
        /// <param name="type"></param>
        /// <param name="Median"></param>
        /// <param name="Negative"></param>
        /// <param name="Thresholding"></param>
        /// <returns></returns>
        public static Bitmap CreateOutline_extractionTiffCCITT4Image(Image inPutImage, int width, int height,
                                                    OutlineExtractionFilter type = OutlineExtractionFilter.Laplacian,
                                                    bool Median = true, bool Negative = true, int Thresholding = 250)
        {
            int orgWidth = inPutImage.Width;
            int orgHeight = inPutImage.Height;

            Bitmap outBitmap;

            // 変更サイズを取得する
            float scale = Math.Min((float)width / (float)inPutImage.Width, (float)height / (float)inPutImage.Height);
            int widthToScale = (int)(inPutImage.Width * scale);
            int heightToScale = (int)(inPutImage.Height * scale);

            try
            {

                outBitmap = ImageUtil.Myresize(inPutImage, widthToScale, heightToScale);
                Console.WriteLine($"●イメージリサイズ　{orgWidth},{orgHeight} -> {inPutImage.Width},{inPutImage.Height}");

                switch (type)
                {
                    case OutlineExtractionFilter.FirstDerivative:
                        //
                        outBitmap = (System.Drawing.Bitmap)ImageUtil.FirstDerivativeFilter(outBitmap);
                        break;
                    case OutlineExtractionFilter.Laplacian:
                        // ラプラシアンフィルタによる輪郭抽出
                        outBitmap = (System.Drawing.Bitmap)ImageUtil.LaplacianFilter(outBitmap);
                        break;
                    case OutlineExtractionFilter.Prewitt:
                        //
                        outBitmap = (System.Drawing.Bitmap)ImageUtil.PrewittFilter(outBitmap);
                        break;
                    case OutlineExtractionFilter.Sobel:
                        //
                        outBitmap = (System.Drawing.Bitmap)ImageUtil.SobelFilter(outBitmap);
                        break;
                }

                if (Median)
                    //// メディアんフィルタ
                    outBitmap = (System.Drawing.Bitmap)ImageUtil.MedianFilter(outBitmap);

                if (Negative)
                    //// ネガポジ反転
                    outBitmap = (System.Drawing.Bitmap)ImageUtil.CreateNegativeImage(outBitmap);

                if (Thresholding > 0)
                    //// 2値化
                    outBitmap = (System.Drawing.Bitmap)ImageUtil.Change2bpFilter(outBitmap, 250);

                return outBitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"2値下処理エラー{ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// 一次微分フィルタによる画像の輪郭抽出を実行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Bitmap FirstDerivativeFilter(Bitmap input)
        {
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = changeImageGray(input);

            // フィルタ用のカーネル
            const int kernelSize = 3; // カーネルサイズ
            double[,] kernel = new double[kernelSize, kernelSize]{
                                {0.0, -1.0, 0.0},
                                {-1.0, 0.0, 1.0},
                                {0.0,  1.0, 0.0}};

            // フィルタ処理
            byte[,] img2 = firstDerivativeFiltering(img, kernel);

            // byte配列をbitmapへ変換
            var output = GetBitmap(img2);

            return output;
        }

        /// <summary>
        /// ソーベルフィルタによる画像の輪郭抽出を実行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Bitmap SobelFilter(Bitmap input)
        {
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = changeImageGray(input);

            // フィルタ用のマスク
            const int kernelSize = 3;
            double[,] kernel = new double[kernelSize, kernelSize]{
                                {-1.0,-2,-2},
                                {0.0,0.0,0.0},
                                {1.0,2.0,1.0}};
            // フィルタ処理
            byte[,] img2 = sobelFiltering(img, kernel);

            // byte配列をbitmapへ変換
            var output = GetBitmap(img2);

            return output;

        }

        /// <summary>
        /// ラプラシアンフィルタによる画像の輪郭抽出を実行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Bitmap LaplacianFilter(Bitmap input)
        {
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = changeImageGray(input);

            // フィルタ用のマスク
            const int maskSize = 3;
            double[,] mask = new double[maskSize, maskSize]{
                                {1.0,1.0,1.0},
                                {1.0,-8.0,1.0},
                                {1.0,1.0,1.0}};
            // フィルタ処理
            byte[,] img2 = laplacianFiltering(img, mask);

            // byte配列をbitmapへ変換
            var output = GetBitmap(img2);

            return output;

        }

        /// <summary>
        /// プレヴィットフィルタによる画像の輪郭抽出を実行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Bitmap PrewittFilter(Bitmap input)
        {
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = changeImageGray(input);

            // フィルタ用のカーネル
            const int kernelSize = 3; // カーネルサイズ
            double[,] kernel = new double[kernelSize, kernelSize]{
                                {-1.0, 0.0, 1.0},
                                {-1.0, 0.0, 1.0},
                                {-1.0, 0.0, 1.0}};

            // フィルタ処理
            byte[,] img2 = prewittFiltering(img, kernel);

            // byte配列をbitmapへ変換
            var output = GetBitmap(img2);

            return output;

        }


        /// <summary>
        /// 画像のエッジ強調を実行
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Bitmap SharpnessFilter(Bitmap input)
        {
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = changeImageGray(input);

            // フィルタ用のマスク
            const int maskSize = 3;
            double[,] mask = new double[maskSize, maskSize]{
                                {-0.11,-0.11,-0.11},
                                {-0.11,1.88,-0.11},
                                {-0.11,-0.11,-0.11}};

            // フィルタ処理
            byte[,] img2 = sharpnessFiltering(img, mask);

            // byte配列をbitmapへ変換
            var output = GetBitmap(img2);

            return output;

        }

        /// <summary>
        /// エンボスフィルタ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Bitmap EmbossFilter(Bitmap input)
        {
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = changeImageGray(input);

            // フィルタ用のカーネル
            const int kernelSize = 3; // カーネルサイズ
            double[,] kernel = new double[kernelSize, kernelSize]{
                                {-2.0, -1.0, 0.0},
                                {-1.0,  1.0, 1.0},
                                { 0.0,  1.0, 2.0}};

            // フィルタ処理
            byte[,] img2 = embossFiltering(img, kernel);

            // byte配列をbitmapへ変換
            var output = GetBitmap(img2);

            return output;

        }

        /// <summary>
        /// 2値化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="threhold">閾値を超えたピクセルを白とする</param>
        /// <returns></returns>
        public static Bitmap Change2bpFilter(Bitmap input, int threhold)
        {
            // フィルタ処理
            byte[,] img2 = changeImage2bp(input, threhold);

            // byte配列をbitmapへ変換
            var output = GetBitmap(img2);

            return output;

        }

        /// <summary>
        /// メディアんフィルタ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Bitmap MedianFilter(Bitmap input)
        {
            // 画像の読み込み(グレースケールに変換)
            byte[,] img = changeImageGray(input);

            // フィルタ用のカーネル
            // フィルタ用のマスク
            const int kernelSize = 3;
            double[,] kernel = new double[kernelSize, kernelSize]{
                                {1.0/9.0,1.0/9.0,1.0/9.0},
                                {1.0/9.0,1.0/9.0,1.0/9.0},
                                {1.0/9.0,1.0/9.0,1.0/9.0}};

            // フィルタ処理
            byte[,] img2 = embossFiltering(img, kernel);

            // byte配列をbitmapへ変換
            var output = GetBitmap(img2);

            return output;
        }

        /// <summary>
        /// 画像ファイルをグレースケール変換してbyte配列にする
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static byte[,] LoadImageGray(string filename)
        {
            Bitmap img = new Bitmap(filename);
            int w = img.Width;
            int h = img.Height;
            byte[,] dst = new byte[w, h];

            // bitmapクラスの画像ピクセル値を配列に挿入
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    // グレイスケールに変換
                    dst[j, i] = (byte)((img.GetPixel(j, i).R + img.GetPixel(j, i).B + img.GetPixel(j, i).G) / 3);
                }
            }
            return dst;
        }


        /// <summary>
        /// 画像(System.Drwing.Image)をグレースケール変換してbyte配列にする
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static byte[,] changeImageGray(System.Drawing.Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;
            byte[,] dst = new byte[w, h];

            // bitmapクラスの画像ピクセル値を配列に挿入
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    // グレイスケールに変換
                    dst[j, i] = (byte)((img.GetPixel(j, i).R + img.GetPixel(j, i).B + img.GetPixel(j, i).G) / 3);
                }
            }
            return dst;
        }

        /// <summary>
        /// 二値化
        /// </summary>
        /// <param name="img"></param>
        /// <param name="threshold">閾値を超えたピクセルを白とする</param>
        /// <returns></returns>
        private static byte[,] changeImage2bp(System.Drawing.Bitmap img, int threshold)
        {
            int w = img.Width;
            int h = img.Height;
            byte[,] dst = new byte[w, h];

            // bitmapクラスの画像ピクセル値を配列に挿入
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    //
                    var gray = (byte)((img.GetPixel(j, i).R + img.GetPixel(j, i).B + img.GetPixel(j, i).G) / 3);

                    if (gray > threshold)
                        dst[j, i] = 0xff;
                    else
                        dst[j, i] = 0x00;
                }
            }
            return dst;
        }

        /// <summary>
        /// byte2次元配列からBitmapファイルを保存
        /// </summary>
        /// <param name="src"></param>
        /// <param name="filename"></param>
        private static void SaveImage(byte[,] src, string filename)
        {
            // 画像の幅・高さ
            int w = src.GetLength(0);
            int h = src.GetLength(1);

            Bitmap img = new Bitmap(w, h);

            // ピクセル値のセット
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    img.SetPixel(j, i, Color.FromArgb(src[j, i], src[j, i], src[j, i]));
                }
            }

            // 画像の保存
            img.Save(filename);
        }

        /// <summary>
        /// byte2次元配列からSystem.Drawing.Bitmapを得る
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private static System.Drawing.Bitmap GetBitmap(byte[,] src)
        {
            // 画像の幅・高さ
            int w = src.GetLength(0);
            int h = src.GetLength(1);

            Bitmap img = new Bitmap(w, h);

            // ピクセル値のセット
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    img.SetPixel(j, i, Color.FromArgb(src[j, i], src[j, i], src[j, i]));
                }
            }
            return img;
        }

        /// <summary>
        /// double型をbyte型に変換 
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private static byte Double2Byte(double num)
        {
            if (num > 255.0) return 255;
            else if (num < 0) return 0;
            else return (byte)num;
        }

        /// <summary>
        /// シャープネスフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        private static byte[,] sharpnessFiltering(byte[,] src, double[,] mask)
        {
            // 縦横サイズを配列から読み取り
            int w = src.GetLength(0);
            int h = src.GetLength(1);
            // マスクサイズの取得
            int masksize = mask.GetLength(0);
            // 出力画像用の配列
            byte[,] dst = new byte[w, h];

            // 画像処理
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double sum = 0;
                    for (int k = -masksize / 2; k <= masksize / 2; k++)
                    {
                        for (int n = -masksize / 2; n <= masksize / 2; n++)
                        {
                            if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                            {
                                sum += src[j + n, i + k] * mask[n + masksize / 2, k + masksize / 2];
                            }
                        }
                    }

                    dst[j, i] = Double2Byte(sum);
                }
            }
            return dst;
        }

        /// <summary>
        /// エンボスフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="kernel"></param>
        /// <returns></returns>
        private static byte[,] embossFiltering(byte[,] src, double[,] kernel)
        {
            // 画像の幅・高さ
            int w = src.GetLength(0);
            int h = src.GetLength(1);

            // カーネルサイズ
            int kernelSize = kernel.GetLength(0);

            // 出力画像用の配列
            byte[,] dst = new byte[w, h];

            // 畳み込み処理
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double sum = 0;
                    for (int k = -kernelSize / 2; k <= kernelSize / 2; k++)
                    {
                        for (int n = -kernelSize / 2; n <= kernelSize / 2; n++)
                        {
                            if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                            {
                                sum += src[j + n, i + k] * kernel[n + kernelSize / 2, k + kernelSize / 2];
                            }
                        }
                    }

                    dst[j, i] = Double2Byte(sum);
                }
            }
            return dst;
        }


        /// <summary>
        /// メディアんフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="kernel"></param>
        /// <returns></returns>
        private static byte[,] medianFiltering(byte[,] src, double[,] kernel)
        {
            // 縦横サイズを配列から読み取り
            int w = src.GetLength(0);
            int h = src.GetLength(1);
            // マスクサイズの取得
            int kernelSize = kernel.GetLength(0);
            // 出力画像用の配列
            byte[,] dst = new byte[w, h];

            // 画像処理
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double sum = 0;
                    for (int k = -kernelSize / 2; k <= kernelSize / 2; k++)
                    {
                        for (int n = -kernelSize / 2; n <= kernelSize / 2; n++)
                        {
                            if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                            {
                                sum += src[j + n, i + k] * kernel[n + kernelSize / 2, k + kernelSize / 2];
                            }
                        }
                    }

                    dst[j, i] = Double2Byte(sum);
                }
            }
            return dst;
        }


        /// <summary>
        /// 画像の輪郭抽出(一次微分フィルタ)
        /// </summary>
        /// <param name="src"></param>
        /// <param name="kernel"></param>
        /// <returns></returns>
        private static byte[,] firstDerivativeFiltering(byte[,] src, double[,] kernel)
        {
            // 画像の幅・高さ
            int w = src.GetLength(0);
            int h = src.GetLength(1);

            // カーネルサイズ
            int kernelSize = kernel.GetLength(0);

            // 出力画像用の配列
            byte[,] dst = new byte[w, h];

            // 畳み込み処理
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double sum = 0;
                    for (int k = -kernelSize / 2; k <= kernelSize / 2; k++)
                    {
                        for (int n = -kernelSize / 2; n <= kernelSize / 2; n++)
                        {
                            if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                            {
                                sum += src[j + n, i + k] * kernel[n + kernelSize / 2, k + kernelSize / 2];
                            }
                        }
                    }

                    dst[j, i] = Double2Byte(sum);
                }
            }
            return dst;
        }

        /// <summary>
        /// 画像の輪郭抽出(ソーベルフィルタ)
        /// </summary>
        /// <param name="src"></param>
        /// <param name="kernel"></param>
        /// <returns></returns>
        static byte[,] sobelFiltering(byte[,] src, double[,] kernel)
        {
            // 縦横サイズを配列から読み取り
            int w = src.GetLength(0);
            int h = src.GetLength(1);
            // マスクサイズの取得
            int kernelSize = kernel.GetLength(0);
            // 出力画像用の配列
            byte[,] dst = new byte[w, h];

            // 画像処理
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double sum = 0;
                    for (int k = -kernelSize / 2; k <= kernelSize / 2; k++)
                    {
                        for (int n = -kernelSize / 2; n <= kernelSize / 2; n++)
                        {
                            if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                            {
                                sum += src[j + n, i + k] * kernel[n + kernelSize / 2, k + kernelSize / 2];
                            }
                        }
                    }

                    dst[j, i] = Double2Byte(sum);
                }
            }
            return dst;
        }

        /// <summary>
        /// 画像の輪郭抽出(ラプラシアンフィルタ)
        /// </summary>
        /// <param name="src"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        static byte[,] laplacianFiltering(byte[,] src, double[,] mask)
        {
            // 縦横サイズを配列から読み取り
            int w = src.GetLength(0);
            int h = src.GetLength(1);
            // マスクサイズの取得
            int masksize = mask.GetLength(0);
            // 出力画像用の配列
            byte[,] dst = new byte[w, h];

            // 画像処理
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double sum = 0;
                    for (int k = -masksize / 2; k <= masksize / 2; k++)
                    {
                        for (int n = -masksize / 2; n <= masksize / 2; n++)
                        {
                            if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                            {
                                sum += src[j + n, i + k] * mask[n + masksize / 2, k + masksize / 2];
                            }
                        }
                    }

                    dst[j, i] = Double2Byte(sum);
                }
            }
            return dst;
        }

        /// <summary>
        /// 画像の輪郭抽出(プレヴィットフィルタ)
        /// </summary>
        /// <param name="src"></param>
        /// <param name="kernel"></param>
        /// <returns></returns>
        static byte[,] prewittFiltering(byte[,] src, double[,] kernel)
        {
            // 画像の幅・高さ
            int w = src.GetLength(0);
            int h = src.GetLength(1);

            // カーネルサイズ
            int kernelSize = kernel.GetLength(0);

            // 出力画像用の配列
            byte[,] dst = new byte[w, h];

            // 畳み込み処理
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double sum = 0;
                    for (int k = -kernelSize / 2; k <= kernelSize / 2; k++)
                    {
                        for (int n = -kernelSize / 2; n <= kernelSize / 2; n++)
                        {
                            if (j + n >= 0 && j + n < w && i + k >= 0 && i + k < h)
                            {
                                sum += src[j + n, i + k] * kernel[n + kernelSize / 2, k + kernelSize / 2];
                            }
                        }
                    }

                    dst[j, i] = Double2Byte(sum);
                }
            }
            return dst;
        }


        /// <summary>
        /// 引数で指定されたカラー画像を二値化する。　https://qiita.com/Nuits/items/4a2fbc0f4e8583bd5531
        /// 使用している技術 LockBits Marshal.Copy UnlockBits for ループ ネスト
        /// </summary>
        /// <param name="src">変換対象のカラー画像。</param>
        /// <returns>変換結果の二値画像</returns>
        public static System.Drawing.Bitmap HispeedImageBinarizer(System.Drawing.Bitmap src, float threshold = 254)
        {
            // 変換結果のBitmapを作成する
            System.Drawing.Bitmap dest =
                new System.Drawing.Bitmap(
                    src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format1bppIndexed);

            // Bitmapをロックし、BitmapDataを取得する
            System.Drawing.Imaging.BitmapData srcBitmapData =
                src.LockBits(
                    new System.Drawing.Rectangle(0, 0, src.Width, src.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly, src.PixelFormat);

            System.Drawing.Imaging.BitmapData destBitmapData =
                dest.LockBits(
                    new System.Drawing.Rectangle(0, 0, dest.Width, dest.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly, dest.PixelFormat);

            // 変換対象のカラー画像の情報をバイト列へ書き出す
            byte[] srcPixels = new byte[srcBitmapData.Stride * src.Height];
            System.Runtime.InteropServices.Marshal.Copy(srcBitmapData.Scan0, srcPixels, 0, srcPixels.Length);

            // 二値画像へ変換した結果を保管するバイト配列を作成する
            byte[] destPixels = new byte[destBitmapData.Stride * destBitmapData.Height];
            for (int y = 0; y < destBitmapData.Height; y++)
            {
                for (int x = 0; x < destBitmapData.Width; x++)
                {
                    // 24bitカラーを256階層のグレースケールに変換し、threshold 以上であれば白と判定する
                    if (threshold <= ConvertToGrayscale(srcPixels, x, y, srcBitmapData.Stride))
                    {
                        // 二値画像は1ビットずつ格納されるため、座標は8で割ったアドレスのバイトに格納されている
                        int pos = (x >> 3) + destBitmapData.Stride * y;
                        // 該当のビットを立てることで、白にする
                        destPixels[pos] |= (byte)(0x80 >> (x & 0x7));
                    }
                }
            }
            // 二値データを保管したバイト列を結果となるBitmapDataへ書き出す
            System.Runtime.InteropServices.Marshal.Copy(destPixels, 0, destBitmapData.Scan0, destPixels.Length);

            // BitmapDataのロックを解除する
            src.UnlockBits(srcBitmapData);
            dest.UnlockBits(destBitmapData);

            return dest;
        }

        const int RedFactor = (int)(0.298912 * 1024);
        const int GreenFactor = (int)(0.586611 * 1024);
        const int BlueFactor = (int)(0.114478 * 1024);
        /// <summary>
        /// 指定された座標のピクセルのグレースケール値を求める
        /// </summary>
        /// <param name="srcPixels">変換元画像のBitmapデータのバイト配列</param>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        /// <param name="stride">スキャン幅</param>
        /// <returns>変換結果</returns>
        private static float ConvertToGrayscale(byte[] srcPixels, int x, int y, int stride)
        {
            int position = x * 3 + stride * y;
            byte b = srcPixels[position + 0];
            byte g = srcPixels[position + 1];
            byte r = srcPixels[position + 2];

            return (r * RedFactor + g * GreenFactor + b * BlueFactor) >> 10;
        }
    }
}