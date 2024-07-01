//// SasaLib 笹原専用イメージ処理クラスライブラリ
//// 2018-09-13 株式会社東陽機械製作所 システム開発課
//// 使用プログラム 
//// Wordリスト変換ツール
////
//using BarcodeLib;
//using SasaLib;
//using System;
//using System.Drawing;
//using System.Runtime.Versioning;


//namespace SasaLib
//{
//    /// <summary>
//    /// イメージ関係処理メソッド（スタティック）
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public static partial class ImageUtilBarcode
//    {
//        // 上書き描画関連

//        /// <summary>
//        /// バーコード書き込み
//        /// </summary>
//        /// <param name="image">書き込み先のImageオブジェクト</param>
//        /// <param name="text">バーコード内容</param>
//        /// <param name="posx">Ｘ位置のピクセル単位</param>
//        /// <param name="posy">Ｙ位置のピクセル単位</param>
//        /// <param name="W">バーコード幅（ピクセル）</param>
//        /// <param name="H">バーコード高さ（ピクセル）</param>
//        public static void DrawGUIDBarCodeOnTop(System.Drawing.Image image, string text, long posx, long posy, int W, int H)
//        {
//            // ImageオブジェクトからGraphicsオブジェクトを生成
//            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(image);
//            //
//            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
//            //
//            System.Drawing.Color btnForeColor = System.Drawing.Color.Black;
//            System.Drawing.Color btnBackColor = System.Drawing.Color.White;
//            //バーコードのBitmapを生成
//            System.Drawing.Bitmap barcode = new Bitmap(b.Encode(TYPE.CODE128B, text, btnForeColor, btnBackColor, W, H));
//            // 解像度を書き込み先に合わせる
//            barcode.SetResolution(image.HorizontalResolution, image.VerticalResolution);
//            //
//            Console.WriteLine("バーコード位置・幅 posx,posy,W,H = {0},{1},{2},{3}", posx, posy, W, H);
//            gr.DrawImage(barcode, (float)posx, (float)posy);
//            //
//            gr.Dispose();
//        }

//        /// <summary>
//        /// バーコード書き込み(スケール指定可能)
//        /// </summary>
//        /// <param name="DrawingImage">書き込み先の図面Imageオブジェクト⓪</param>
//        /// <param name="text">バーコード内容</param>
//        /// <param name="posx">Ｘ位置のピクセル単位</param>
//        /// <param name="posy">Ｙ位置のピクセル単位</param>
//        /// <param name="W">バーコード幅（ピクセル）</param>
//        /// <param name="H">バーコード高さ（ピクセル）</param>
//        /// <param name="testmode">trueのとき枠付き</param>
//        public static void DrawGUIDBarCodeOnTop(System.Drawing.Image DrawingImage, string text, long posx, long posy, int W, int H, bool testmode)
//        {
//            Console.WriteLine($"バーコード下地位置・幅 posx,posy,W,H = 位置X:{(float)posx},位置Y:{(float)posy},幅W2:{W},高H2:{H}");

//            // ベースとなる長方形のイメージを作成①　テストモード時は枠を描画するように指示
//            System.Drawing.Image barcodeUnderBitmap = MakeRectangleBaseImage(W, H, DrawingImage.HorizontalResolution, testmode);
//            // ベースイメージ①に描画するグラフィックオブジェクトを取得
//            System.Drawing.Graphics barcodeUnderBitmapGr = System.Drawing.Graphics.FromImage(barcodeUnderBitmap);

//            // バーコードを生成
//            BarcodeLib.Barcode barcodeObj = new BarcodeLib.Barcode();
//            // バーコード色を設定
//            System.Drawing.Color btnForeColor = System.Drawing.Color.Black;
//            System.Drawing.Color btnBackColor = System.Drawing.Color.White;
//            // バーコードそのもののイメージ②を作成
//            System.Drawing.Bitmap barcodeOrgImage = new Bitmap(barcodeObj.Encode(TYPE.CODE128B, text, btnForeColor, btnBackColor, 1338, 125));
//            if (testmode)
//                barcodeOrgImage.Save(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "バーコード本体出力テスト.bmp"));

//            // テストモードではない場合バーコードを描画
//            if (testmode == false)
//                //ベースイメージ①にバーコードデータbarcodeOrgImageを描画する
//                barcodeUnderBitmapGr.DrawImage(barcodeOrgImage, 0, 0, W, H);

//            //ベースイメージ①のグラフィックオブジェクトは不要なため廃棄
//            barcodeUnderBitmapGr.Dispose();

//            // 図面イメージ⓪から、描画用のGraphicsオブジェクトを生成
//            System.Drawing.Graphics DrawingImageGr = System.Drawing.Graphics.FromImage(DrawingImage);

//            //図面イメージ⓪描画用Graphicオブジェクトを使い、ベースイメージを①描画
//            DrawingImageGr.DrawImage(barcodeUnderBitmap, (float)posx, (float)posy);

//            //図面イメージ⓪描画用Graphicオブジェクトは不要なため廃棄
//            DrawingImageGr.Dispose();
//        }

//        /// <summary>
//        /// 指定したイメージの右下を原点とし、指定する位置にGUID Base64改のバーコード、及び文字列を描画
//        /// </summary>
//        /// <param name="image">書き込み先のImageオブジェクト</param>
//        /// <param name="guid"></param>
//        /// <param name="posBX">mm</param>
//        /// <param name="posBY">mm</param>
//        /// <param name="BW">mm</param>
//        /// <param name="BH">mm</param>
//        /// <param name="posTX">mm</param>
//        /// <param name="posTY">mm</param>
//        /// <param name="fonSize"></param>
//        /// <returns></returns>
//        public static bool DrawBarcodeFromRightButtom(System.Drawing.Image image, GUIDExtensions guid, long posBX, long posBY, float BW, float BH, long posTX, long posTY, int fonSize)
//        {
//            try
//            {
//                string _barcodeString = guid.B64FnameString;

//                Console.WriteLine($"■DrawBarcodeFromRightButtom(..) CODE:【{_barcodeString}】");
//                Console.WriteLine($"■■DrawBarcodeFromRightButtom(..) 【バーコード位置 BX:{posBX}mm , BY{posBY}mm】 【幅BW:{BW}mm, 高BH:{BH}mm】");
//                Console.WriteLine($"■■■DrawBarcodeFromRightButtom(..) 【テキスト位置 TX:{posTX}mm, TY:{posTY}mm】 フォント高さ:【{fonSize}】");

//                // mm位置情報をDOT情報に変換するオブジェクトを生成
//                MillimetreToDot dotP = new MillimetreToDot(image);

//                var Hres = image.HorizontalResolution; //DPI
//                var Vres = image.VerticalResolution; //DPI

//                //バーコードピクセルサイズ計算)
//                int barcodeW = (int)ImageUtil.MilliToPixel(BW, Hres);
//                int barcodeH = (int)ImageUtil.MilliToPixel(BH, Vres);
//                Console.WriteLine($"■■■■書き込み先イメージの解像度【Hres:{Hres}, Vres:{Vres}】より、バーコード部分ピクセルサイズは {barcodeW},{barcodeH} となります。");

//                // バーコード描画用のポイントを作成。右下を原点にミリメーターで入力する。
//                Point barcodePoint = dotP.RightButtomCordi(posBX, posBY);
//                // バーコード描画実行
//                DrawGUIDBarCodeOnTop(image, _barcodeString, barcodePoint.X, barcodePoint.Y, barcodeW, barcodeH, false);

//                // バーコード実文字列描画用のポイントを作成。右下を原点にミリメーターで入力する。
//                Point textPoint = dotP.RightButtomCordi(posTX, posTY);
//                // 文字列描画実行
//                ImageUtil.DrawFontToImage(image, _barcodeString, textPoint.X, textPoint.Y, "Arial", fonSize);
//                Console.WriteLine($"■■■■■バーコード及びバーコード文字列をImageオブジェクトに描画しました。正常終了");
//                return true;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"□SasaLib.ImageUtil.DrawBarcodeFromRightButtom(..)にて例外発生。{ex.Message}");
//                return false;
//            }
//        }

//        /// <summary>
//        /// ミリメートル座標をピクセル座標に変換
//        /// </summary>
//        [SupportedOSPlatform("windows")]
//        private class MillimetreToDot
//        {
//            private readonly float rezolutionH;
//            private readonly float rezolutionV;

//            private readonly long hMax;
//            private readonly long wMax;

//            /// <summary>
//            /// コンストラクタ
//            /// </summary>
//            /// <param name="orgin">System.Drawing.Image クラス</param>
//            public MillimetreToDot(System.Drawing.Image orgin)
//            {
//                rezolutionH = orgin.HorizontalResolution;
//                rezolutionV = orgin.VerticalResolution;
//                hMax = orgin.Height;
//                wMax = orgin.Width;
//            }

//            /// <summary>
//            /// コンストラクタ
//            /// </summary>
//            /// <param name="height">イメージ高さ（ドット数）</param>
//            /// <param name="width">イメージ幅（ドット数）</param>
//            /// <param name="dpi">イメージDPI</param>
//            public MillimetreToDot(long height, long width, float dpi)
//            {
//                rezolutionH = dpi;
//                rezolutionV = dpi;
//                hMax = height;
//                wMax = width;
//            }

//            /// <summary>
//            /// 左上原点座標系　ミリメートル座標をドット座標に変更
//            /// </summary>
//            /// <param name="width_mm"></param>
//            /// <param name="height_mm"></param>
//            /// <returns></returns>
//            public System.Drawing.Point LeftTopCordi(double width_mm, double height_mm)
//            {
//                System.Drawing.Point ans;

//                int w = (int)(width_mm / 25.4 * rezolutionH);
//                int H = (int)(height_mm / 25.4 * rezolutionV);

//                ans = new System.Drawing.Point(w, H);
//                return ans;
//            }

//            /// <summary>
//            /// 右下原点座標系　ミリメートル座標をドット座標に変更
//            /// </summary>
//            /// <param name="width_mm"></param>
//            /// <param name="height_mm"></param>
//            /// <returns></returns>
//            public System.Drawing.Point RightButtomCordi(double width_mm, double height_mm)
//            {
//                System.Drawing.Point ans;

//                int w2 = (int)(width_mm / 25.4 * rezolutionH);
//                int h2 = (int)(height_mm / 25.4 * rezolutionV);

//                int W = (int)(wMax - w2);
//                int H = (int)(hMax - h2);

//                ans = new System.Drawing.Point(W, H);
//                return ans;
//            }

//        }


//        /// <summary>
//        /// 幅と高さをピクセルで表現した解像度付きのBitmapImageを作成する
//        /// バーコードの座布団に使用する
//        /// </summary>
//        /// <param name = "W" > 幅(ピクセル) </ param >
//        /// < param name="H">高さ</param>
//        /// <param name = "resolution" > 解像度DPI </ param >
//        /// < param name="rectangleDraw">trueなら枠を描画</param>
//        /// <returns></returns>
//        private static System.Drawing.Bitmap MakeRectangleBaseImage(int W, int H, float resolution, bool rectangleDraw = false)
//        {
//            //バーコード下地Bitmapを生成
//            System.Drawing.Bitmap barcodeUnderBitmap = new Bitmap(W, H);
//            // 解像度を書き込み先に合わせる
//            barcodeUnderBitmap.SetResolution(resolution, resolution);

//            // バーコード下地Bitmapのためのグラフィックオブジェクト
//            System.Drawing.Graphics barcodeGr = System.Drawing.Graphics.FromImage(barcodeUnderBitmap);

//            if (rectangleDraw)
//            {
//                // テストモードなら枠を描画
//                Pen pen = new Pen(Color.Black, 20);
//                barcodeGr.DrawRectangle(pen, 0, 0, W, H);
//            }
//            barcodeGr.Dispose();
//            return barcodeUnderBitmap;
//        }

//    }
//}
