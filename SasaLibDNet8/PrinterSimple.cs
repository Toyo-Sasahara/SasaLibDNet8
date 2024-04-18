using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace SasaLib
{
    public class PrinterSimple
    {
        /// <summary>
         /// 印刷時の印刷領域
         /// </summary>
        public enum BeforeExtractType { 原寸, 原寸オフセット, マージン範囲, ページサイズ範囲, Custom = 99 }
        /// <summary>
        /// 印刷直前レンダリングでのオフセット値を保持
        /// </summary>
        public float PXmm { get { return _pxmm; } }
        public float PYmm { get { return _pymm; } }
        float _pxmm = 0;
        float _pymm = 0;

        /// <summary>
        /// 
        /// </summary>
        public bool Ready = false;

        /// <summary>
        /// 印刷直前レンダリングでのBitmapオブジェクト
        /// </summary>
        System.Drawing.Bitmap PrintBmp = default;

        /// <summary>
        ///  プリントファイルを保持
        /// </summary>
        System.Drawing.Image currentImage;

        /// <summary>
        /// 標準用紙サイズ
        /// </summary>
        PrintConfig.CommonPaperSize currentPaperSize;

        /// <summary>
        /// 印刷に使うプリンタノコンフィグを保持
        /// </summary>
        PrinterSimpleConfigData printerConfigData;

        /// <summary>
        /// 印刷直前レンダリングでの用紙の大きさに対する出力前処置の選択を設定
        /// </summary>
        public BeforeExtractType BeforePrintExtractMode { get; set; } = BeforeExtractType.原寸オフセット;

        /// <summary>
        /// 印刷イベントハンドラでイメージファイルを保存するときのフルパス名をセット
        /// nullの時はなにもしない
        /// </summary>
        public string TestModeBeforePrintingImageSaveFilepath { get; set; } = null;


        /// <summary>
        /// ★コンストラクタ。①TIFFファイル名と、出力先プリンタの設定ファイルを受け取る
        /// 
        /// </summary>
        /// <param name="printerImage"></param>
        public PrinterSimple(System.Drawing.Image printerImage, string WindowsPrinterName, List<PaperSizeAndSource> paperSizeAndSources)
        {
            this.printerConfigData = new PrinterSimpleConfigData(WindowsPrinterName, paperSizeAndSources);
            this.currentImage = printerImage;

            // サイズ
            //var W = currentImage.Width;
            //var H = currentImage.Height;
            // 解像度
            //var resW = currentImage.VerticalResolution;
            //var resH = currentImage.HorizontalResolution;
            // ピクセルフォーマット
            //var pixelFormat = currentImage.PixelFormat;

            // 解像度をもとに用紙サイズ(mm)を取得
            PM size = ImageUtil.GetPaperSizeMillimeter(currentImage,400);
            /// イメージから用紙サイズと向きを推察
            currentPaperSize = PrintConfig.PaperCheck.GetJISpaperSize(size.Width, size.Height, 10);



            //BeforePrintExtractMode 
            Ready = printerConfigData.Ready;
        }

        /// <summary>
        /// 印刷実行
        /// </summary>
        /// <param name="PrinterOutputFileNameFullPath"></param>
        /// <param name="BeforePrintImageFullPath"></param>
        public bool PrintExecute(string DocumentName, string PrinterOutputFileNameFullPath = "")
        {
            if (this.Ready == false)
                return false;

            // プリンタドライバ名
            string printerName = printerConfigData.PrinterName;

            // プリンタドライバ用 ペーパーサイズの設定
            PaperSize paperSize = printerConfigData.GetPaperSize(currentPaperSize);

            // プリンタドライバに送るイメージの向き
            bool landScape = printerConfigData.GetLandScape(currentPaperSize);

            // プリンタドライバに送る用紙供給元
            PaperSource paperSource = printerConfigData.GetPaperSource(currentPaperSize);

            // 印刷時のオフセット情報
            var ofs = printerConfigData.GetOffset(currentPaperSize);

            //
            BeforePrintExtractMode = printerConfigData.GetBeforePrintExtractMode(currentPaperSize);



            CreatePrintingBitmap(currentImage, System.Drawing.Imaging.PixelFormat.Format1bppIndexed, 3, ofs.X, ofs.Y);

            //PrintImage(printerName, paperSize, landScape, paperSource, DocumentName, PrinterOutputFileNameFullPath);


            PrintDocument printDoc = new PrintDocument();

            try
            {
                printDoc.PrinterSettings.PrinterName = printerName;
                printDoc.DefaultPageSettings.Landscape = landScape;
                printDoc.DefaultPageSettings.PaperSize = paperSize;
                printDoc.DefaultPageSettings.PaperSource = paperSource;
                printDoc.DocumentName = DocumentName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PrintDocumentにて例外 {ex.Message}");
            }

            // プリントファイル出力モード
            if (PrinterOutputFileNameFullPath != null)
            {
                printDoc.PrinterSettings.PrintFileName = PrinterOutputFileNameFullPath;
                printDoc.PrinterSettings.PrintToFile = true;
            }

            {
                Console.WriteLine("★出力先名=\"" + printerName + "\"");
                if (printDoc.PrinterSettings.PrintToFile)
                {
                    Console.WriteLine("★ファイル出力モード:{0}", printDoc.PrinterSettings.PrintFileName);
                }
                Console.WriteLine("★DefaultPageSettings.PaperSize {0}", printDoc.DefaultPageSettings.PaperSize);
                Console.WriteLine("★DefaultPageSettings.PaperSource {0}", printDoc.DefaultPageSettings.PaperSource);
                Console.WriteLine("★DefaultPageSettings.Landscape {0}", printDoc.DefaultPageSettings.Landscape);
                Console.WriteLine("★DefaultPageSettings.Marginse {0}", printDoc.DefaultPageSettings.Margins);
            }

            printDoc.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
            printDoc.Print();

            currentImage.Dispose();
            return true;
        }

        /// <summary>
        /// SasaPrintingクラスののコンストラクト。プリント用にPrintBmpオブジェクトを生成
        /// </summary>
        /// <param name="image"></param>
        /// <param name="_printDrawingPixelFormat">最終印刷時に展開するピクセルフォーマットt</param>
        /// <param name="RenderinType">1=,2=,3=ChangePixelFormat()を使用</param>
        /// <param name="dXmm">印刷時のオフセット位置 X</param>
        /// <param name="dYmm">印刷時のオフセット位置 Y</param>
        void CreatePrintingBitmap(System.Drawing.Image image,
            System.Drawing.Imaging.PixelFormat _printDrawingPixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb,
            int RenderinType = 3,
            long dXmm = 0, long dYmm = 0)
        {
            // 印刷オフセットをフィールドに設定
            float _pxmm = dXmm;
            float _pymm = dYmm;

            // 受け取ったImageを元に新しいImageを作成
            if (image != null)
            {
                //    public enum BeforeExtractType { 原寸, 原寸オフセット, マージン範囲, ページサイズ範囲, Custom = 99 }

                switch (RenderinType)
                {
                    case 1:
                        Console.WriteLine("PrintBmpの分割作成が選択されています");
                        ///
                        PrintBmp = new System.Drawing.Bitmap(image.Width, image.Height, _printDrawingPixelFormat);
                        ///
                        ImageUtil.ConvertImagePixelFormat(image, PrintBmp, _printDrawingPixelFormat, dXmm, dYmm);
                        break;

                    case 2:
                        PrintBmp = (System.Drawing.Bitmap)ImageUtil.ChangePixcelFormat_Deprecated(image, _printDrawingPixelFormat, dXmm, dYmm);
                        break;

                    case 3:
                        PrintBmp = ImageUtil.ChangePixelFormat((System.Drawing.Bitmap)image, _printDrawingPixelFormat);
                        break;

                }

                Console.WriteLine("Fromat:{0}, Width={1}, Height={2},W Dpi={3}, H Dpi={4}, PixelFormat={5}", PrintBmp.RawFormat,
                            PrintBmp.Width, PrintBmp.Height, PrintBmp.HorizontalResolution, PrintBmp.VerticalResolution, PrintBmp.PixelFormat);

                // 印刷イメージ情報をフィールドに反映
                float _width = PrintBmp.Width;
                float _height = PrintBmp.Height;
                float _dpih = PrintBmp.HorizontalResolution;
                float _dpiv = PrintBmp.VerticalResolution;
                System.Drawing.Imaging.PixelFormat _pixelFormat = PrintBmp.PixelFormat;
            }
            else
            {
                Console.WriteLine("SasaPrinting() Imageがnull");
            }
        }

        /// <summary>
        /// 印刷を実行するように指令
        /// </summary>
        /// <param name="printername">string</param>
        /// <param name="PaperSize">System.Drawing.Printing.PaperSize</param>
        /// <param name="Landscape">bool</param>
        /// <param name="PaperSource">System.Drawing.Printing.PaperSource</param>
        /// <param name="outFilePath">string</param>
        /// <summary>
        /// イベントハンドラ・現在選択されているプリンタへimgを出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Console.WriteLine("◆プリンタイベントハンドラ Pd_PrintPage() 開始");
            Console.WriteLine("◆e.PageSettings.PrinterSettings.PrinterName = {0}", e.PageSettings.PrinterSettings.PrinterName);
            Console.WriteLine("◆e.PageSettings.PaperSize = {0}", e.PageSettings.PaperSize);
            Console.WriteLine("◆e.PageSettings.PaperSource = {0}", e.PageSettings.PaperSource);
            Console.WriteLine("◆e.PageSettings.PrintableArea = {0}", e.PageSettings.PrintableArea);
            Console.WriteLine("◆e.PageSettings.Landscape = {0}", e.PageSettings.Landscape);
            Console.WriteLine("◆e.MarginBounds = {0}", e.MarginBounds);
            Console.WriteLine("◆e.PageSettings.Margins = {0}", e.PageSettings.Margins);
            Console.WriteLine("◆PrintBmp.RawFormat:{0}", PrintBmp.RawFormat.ToString());
            Console.WriteLine("◆PrintBmp.PixelFormat{0}", PrintBmp.PixelFormat.ToString());
            Console.WriteLine("◆PrintBmp.HorizontalResolution=" + PrintBmp.HorizontalResolution + ", PrintBmp.VerticalResolution=" + PrintBmp.VerticalResolution);

            /// 印刷直前イメージデータをファイル化
            if (TestModeBeforePrintingImageSaveFilepath != null && TestModeBeforePrintingImageSaveFilepath != "") { PrintBmp.Save(TestModeBeforePrintingImageSaveFilepath); }

            //印刷直前イメージデータを描画
            switch (BeforePrintExtractMode)
            {
                case BeforeExtractType.原寸:
                    Console.WriteLine("◆◆スケールモード 何もしない");
                    e.Graphics.DrawImage(PrintBmp, new System.Drawing.Point(0, 0));
                    break;
                case BeforeExtractType.ページサイズ範囲:
                    Console.WriteLine("◆◆スケールモード PageBounds");
                    e.Graphics.DrawImage(PrintBmp, e.PageBounds);
                    break;
                case BeforeExtractType.マージン範囲:
                    Console.WriteLine("◆◆スケールモード MarginBounds");
                    e.Graphics.DrawImage(PrintBmp, e.MarginBounds);
                    break;
                case BeforeExtractType.原寸オフセット:
                    Console.WriteLine("◆◆スケールモードOffsetOnly");
                    Console.WriteLine("　　オフセットX={0},オフセットY={1}", PXmm, PYmm);
                    Console.WriteLine("　　H,V Resolution={0},{1}", PrintBmp.HorizontalResolution, PrintBmp.VerticalResolution);
                    float Xpixel = PXmm / 25.4f * 100f;
                    float Ypixel = PYmm / 25.4f * 100f;
                    Console.WriteLine("　　H,V Pixel={0},{1}", Xpixel, Ypixel);
                    e.Graphics.DrawImage(PrintBmp, new System.Drawing.Point((int)Xpixel, (int)Ypixel));
                    break;
            }
            //
            //次のページがないことを通知する
            e.HasMorePages = false;
            //後始末をする
            PrintBmp.Dispose();
            Console.WriteLine("◆プリンタイベントハンドラ終了");
        }

    }
}
