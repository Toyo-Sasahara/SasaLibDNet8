using SasaLibDNet8;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Versioning;
using System.Xml.Serialization;
using static SasaLib.PrinterSimple;
using static System.Drawing.Printing.PrinterSettings;

namespace SasaLib
{
    [SupportedOSPlatform("windows")]

    /// <summary>
    /// プリンタコンフィギュレーション準備
    /// 
    /// </summary>
    public class XMLconfigPrinterPreparation : XmlSettingFile
    {
        /// <summary>
        /// 既定の設定情報を生成。
        /// (デフォルトコンストラクタは必ず必要)
        /// </summary>
        public XMLconfigPrinterPreparation() { }

        //以下のメンバーはXMLに保存される

        //印刷に使用するプリンタ名(Windowsコントロールパネルでの名前)
        public string OutputPrinter { get; set; } = "Brother MFC-J6770CDW Printer";
        public int OutputPrinter_A4_Tray { get; set; } = 1;
        public int OutputPrinter_A3_Tray { get; set; } = 2;
        public int OutputPrinter_A2_Tray { get; set; } = 2;
        public int OutputPrinter_A1_Tray { get; set; } = 2;
        public int OutputPrinter_A0_Tray { get; set; } = 2;

        public bool OutputPrinter_imgA4portrait_Landscape { get; set; } = false;
        public bool OutputPrinter_imgA3portrait_Landscape { get; set; } = true;
        public bool OutputPrinter_imgA2portrait_Landscape { get; set; } = true;
        public bool OutputPrinter_imgA1portrait_Landscape { get; set; } = true;
        public bool OutputPrinter_imgA0portrait_Landscape { get; set; } = true;

        public bool OutputPrinter_imgA4landscape_Landscape { get; set; } = true;
        public bool OutputPrinter_imgA3landscape_Landscape { get; set; } = false;
        public bool OutputPrinter_imgA2landscape_Landscape { get; set; } = false;
        public bool OutputPrinter_imgA1landscape_Landscape { get; set; } = false;
        public bool OutputPrinter_imgA0landscape_Landscape { get; set; } = false;

        public System.Drawing.Point BARCODEprintPosition { get; set; } = new System.Drawing.Point(100, 8);
        public System.Drawing.Point GUIDprintPosition { get; set; } = new System.Drawing.Point(135, 8);
        public int GUIDfontsize { get; set; } = 8;
        public int BarcodeFontsize { get; set; } = 15;

        public int FeatureOption { get; set; } = 0;
        public bool DebugOption { get; set; } = false;
        //
        // 以下はこれから有効にする設定
        //public PType Printertype { get; set; } = new PType("D1234");

        //保存したくないメンバーは以下のように宣言
        [XmlIgnore]
        public string NotSaved;
    }

    /// <summary>
    /// SasaLibのプリンタ制御クラス
    /// </summary>
    public class Printing
    {
        static public XMLconfigPrinterPreparation confSet = new XMLconfigPrinterPreparation();

        /// <summary>
        /// ドキュメントの印刷中に、印刷ステータス ダイアログ ボックスやプリンター キューなどで表示するドキュメントの名前
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// 印刷直前レンダリングでのBitmapオブジェクト
        /// </summary>
        System.Drawing.Bitmap PrintBmp;

        /// <summary>
        /// 印刷直前レンダリングのピクセルサイズを保持
        /// </summary>
        public float Width { get { return _width; } }
        public float Height { get { return _height; } }
        float _width;
        float _height;

        /// <summary>
        /// 印刷直前レンダリングでの解像度を保持
        /// </summary>
        public float DPIH { get { return _dpih; } }
        public float DPIV { get { return _dpiv; } }
        float _dpih;
        float _dpiv;

        /// <summary>
        /// 印刷直前レンダリングでのオフセット値を保持
        /// </summary>
        public float PXmm { get { return _pxmm; } }
        public float PYmm { get { return _pymm; } }
        float _pxmm = 0;
        float _pymm = 0;

        /// <summary>
        /// 印刷直前レンダリングでの用紙の大きさに対する出力前処置の選択を設定
        /// </summary>
        public BeforeExtractType BeforePrintExtractMode { get; set; } = BeforeExtractType.原寸オフセット;

        /// <summary>
        /// 印刷直前レンダリングピクセル深度を保持
        /// </summary>
        public System.Drawing.Imaging.PixelFormat PixelFormat { get { return _pixelFormat; } }
        System.Drawing.Imaging.PixelFormat _pixelFormat;

        /// <summary>
        /// 印刷イベントハンドラでイメージファイルを保存するときのフルパス名をセット
        /// nullの時はなにもしない
        /// </summary>
        public string BeforePrintingImageSaveFilepath { get; set; } = null;

        /// <summary>
        /// SasaPrintingクラスののコンストラクト。プリント用にPrintBmpオブジェクトを生成
        /// </summary>
        /// <param name="image"></param>
        /// <param name="_printDrawingPixelFormat">最終印刷時に展開するピクセルフォーマットt</param>
        /// <param name="RenderinType">1=,2=,3=ChangePixelFormat()を使用</param>
        /// <param name="dXmm">印刷時のオフセット位置 X</param>
        /// <param name="dYmm">印刷時のオフセット位置 Y</param>
        public Printing(System.Drawing.Image image,
            System.Drawing.Imaging.PixelFormat _printDrawingPixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb,
            int RenderinType = 3,
            long dXmm = 0, long dYmm = 0)
        {
            // 印刷オフセットをフィールドに設定
            _pxmm = dXmm;
            _pymm = dYmm;

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
                _width = PrintBmp.Width;
                _height = PrintBmp.Height;
                _dpih = PrintBmp.HorizontalResolution;
                _dpiv = PrintBmp.VerticalResolution;
                _pixelFormat = PrintBmp.PixelFormat;
            }
            else
            {
                Console.WriteLine("SasaPrinting() Imageがnull");
            }
        }

        /// <summary>
        /// ImageデータからプリンタをコントロールするようにprintDocを設定
        /// </summary>
        /// <param name="printDocIn"></param>
        PrintDocument PrintingPreprocessing(PrintDocument printDocIn)
        {
            try
            {
                Console.WriteLine("★★★★PrintingPreprocessing Start★★★★");
                if (printDocIn.PrinterSettings.PrintToFile == true)
                {
                    Console.WriteLine("★出力ファイル名=\"" + printDocIn.PrinterSettings.PrintFileName + "\"");
                }

                //
                printDocIn.DefaultPageSettings.PaperSource = GetPrinterSettingPaperSource(printDocIn, PaperSourceKind.FormSource);



                ImageUtil.PaperSizeCabinet psize = ImageUtil.GetPaparSize(PrintBmp);
                PaperSize papeSize = new PaperSize();
                bool landscape;

                switch (psize.PaperName)
                {
                    case "A4縦":
                        //number = confSet.OutputPrinter_A4_Tray;
                        landscape = false;
                        papeSize = GetPageSettingPaperSize(printDocIn, PaperKind.A4);
                        printDocIn.DefaultPageSettings.PaperSize = papeSize;
                        break;
                    case "A3縦":
                        //number = confSet.OutputPrinter_A3_Tray;
                        landscape = false;
                        papeSize = GetPageSettingPaperSize(printDocIn, PaperKind.A3);
                        printDocIn.DefaultPageSettings.PaperSize = papeSize;
                        break;

                    case "A2縦":
                        //number = confSet.OutputPrinter_A2_Tray;
                        landscape = false;
                        papeSize = GetPageSettingPaperSize(printDocIn, PaperKind.A2);
                        printDocIn.DefaultPageSettings.PaperSize = papeSize;
                        break;
                    case "A1縦":
                        //number = confSet.OutputPrinter_A1_Tray;
                        landscape = false;
                        papeSize = GetPageSettingPaperSize(printDocIn, PaperKind.Custom);
                        printDocIn.DefaultPageSettings.PaperSize = papeSize;
                        break;
                    case "A0縦":
                        //number = confSet.OutputPrinter_A0_Tray;
                        landscape = false;
                        papeSize = GetPageSettingPaperSize(printDocIn, PaperKind.Custom);
                        printDocIn.DefaultPageSettings.PaperSize = papeSize;
                        break;

                    case "A4横":
                        //number = confSet.OutputPrinter_A4_Tray;
                        landscape = true;
                        papeSize = GetPageSettingPaperSize(printDocIn, PaperKind.A4);
                        printDocIn.DefaultPageSettings.PaperSize = papeSize;
                        break;
                    case "A3横":
                        //number = confSet.OutputPrinter_A3_Tray;
                        landscape = true;
                        papeSize = GetPageSettingPaperSize(printDocIn, PaperKind.A3);
                        printDocIn.DefaultPageSettings.PaperSize = papeSize;
                        break;

                    case "A2横":
                        //number = confSet.OutputPrinter_A2_Tray;
                        landscape = true;
                        papeSize = GetPageSettingPaperSize(printDocIn, PaperKind.A2);
                        printDocIn.DefaultPageSettings.PaperSize = papeSize;
                        break;
                    case "A1横":
                        //number = confSet.OutputPrinter_A1_Tray;
                        landscape = true;
                        landscape = true;
                        printDocIn.DefaultPageSettings.PaperSize = GetPageSettingPaperSize(printDocIn, PaperKind.Custom);
                        Console.WriteLine("▽printDocIn.DefaultPageSettings.PaperSize.Height" + printDocIn.DefaultPageSettings.PaperSize.Height);
                        Console.WriteLine("▽printDocIn.DefaultPageSettings.PaperSize.Kind" + printDocIn.DefaultPageSettings.PaperSize.Kind);
                        Console.WriteLine("▽printDocIn.DefaultPageSettings.PaperSize.PaperName" + printDocIn.DefaultPageSettings.PaperSize.PaperName);
                        Console.WriteLine("▽printDocIn.DefaultPageSettings.PaperSize.RawKind" + printDocIn.DefaultPageSettings.PaperSize.RawKind);
                        Console.WriteLine("▽printDocIn.DefaultPageSettings.PaperSize.Width" + printDocIn.DefaultPageSettings.PaperSize.Width);
                        break;
                    case "A0横":
                        //number = confSet.OutputPrinter_A0_Tray;
                        landscape = true;
                        printDocIn.DefaultPageSettings.PaperSize = GetPageSettingPaperSize(printDocIn, PaperKind.Custom);
                        break;
                    default:
                        Console.WriteLine("▲印刷前・用紙サイズの認識に失敗！！PrintingPreprocessing");
                        //number = 0;
                        printDocIn.DefaultPageSettings.PaperSize = GetPageSettingPaperSize(printDocIn, PaperKind.Custom);
                        landscape = false;
                        break;

                }

                Margins margins = new Margins(0, 0, 0, 0);


                printDocIn.DefaultPageSettings.Margins = margins;
                //printDocIn.OriginAtMargins = true;
                printDocIn.DefaultPageSettings.Landscape = landscape;

                PrintDocument prDocOut = new PrintDocument();
                prDocOut = printDocIn;

                Console.WriteLine("★★★★PrintingPreprocessing End★★★★");

                return prDocOut;
            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib Printing Class", EventLogEntryType.Error, 0, $"▲PrintingPreprocessing(PrintDocument printDocIn), 失敗,Exception={ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// XML設定ファイルを読み込む
        /// </summary>
        public static void ReadSeeting()
        {
            try
            {
                // Environment.SpecialFolder共用体を参照
                //
                confSet = XmlSettingFile.Load(Environment.SpecialFolder.CommonDocuments,
                    new XMLconfigPrinterPreparation()) as XMLconfigPrinterPreparation;

                Console.WriteLine("印刷設定ファイルを読み込みました：{0}", confSet.Filename);

            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib Printing Class", EventLogEntryType.Error, 0, $"▲ReadSeeting(), 失敗,Exception={ex.Message}");

            }
            finally
            {
                confSet.Save();
            }
        }

        /// プリンタ情報取得＆設定

        /// <summary>
        /// デフォルトプリンタ名(Windowsコントロールパネルでの名前)を取得
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrinterName()
        {
            PrintDocument printDoc = new PrintDocument();
            return printDoc.DefaultPageSettings.PrinterSettings.PrinterName;
        }

        /// <summary>
        /// コンピュータにインストールされているすべてのプリンタ名(Windowsコントロールパネルでの名前)を得る
        /// </summary>
        /// <returns>プリンタ名(Windowsコントロールパネルでの名前)のStringCollection</returns>
        public static StringCollection GetInstaledPrinterNames()
        {
            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine("InstalledPrinter=\"{0}\"", printerName);
            }
            return InstalledPrinters;
        }

        /// <summary>
        /// 指定したプリンタ(Windowsコントロールパネルでの名前)が存在するかチェック
        /// </summary>
        /// <param name="PrinterName"></param>
        /// <returns></returns>
        public static bool ExistsPrinter(string PrinterName)
        {
            StringCollection _printers = GetInstaledPrinterNames();
            foreach (string _prname in _printers)
            {
                if (_prname == PrinterName)
                {
                    return true;
                }
            }
            return false;
        }


        // プリンタから PaperSouceやPaperSize情報を取得

        /// <summary>
        /// 指定した SouceNameを含む PaperSouceオブジェクトを検索して返す
        /// </summary>
        /// <param name="printer">プリンタ名(Windowsコントロールパネルでの名前)</param>
        /// <param name="SourceName">PaperSouceオブジェクト</param>
        /// <returns></returns>
        public static PaperSource FindPaperSouce(string printer, string SourceName)
        {
            // PaperSourceCollectionをList化
            List<PaperSource> psList = GetPapserSouceObjects(printer);
            //LINQにて最初に見つかったSourceNameが所属するPaperSouceを返す
            PaperSource ans = psList.FirstOrDefault(value => value.SourceName == SourceName);
            return ans;
        }

        /// <summary>
        /// 指定した PaperNameを含む PaperSizeオブジェクトを検索して返す
        /// </summary>
        /// <param name="printer"></param>
        /// <param name="PaperName"></param>
        /// <returns></returns>
        public static PaperSize FindPaperSize(string printer, string PaperName)
        {
            // PaperSizeCollectionをList化
            List<PaperSize> psList = GetPaperSizeObjects(printer);
            //LINQにて最初に見つかったSourceNameが所属するPaperSouceを返す
            PaperSize ans = psList.FirstOrDefault(value => value.PaperName == PaperName);
            return ans;
        }

        /// 情報一覧取得

        /// <summary>
        /// 指定したプリンタで選択できる用紙サイズ情報をすべて取得する
        /// </summary>
        /// <param name="printerName">プリンタ名(Windowsコントロールパネルでの名前)</param>
        /// <returns>用紙状況</returns>
        public static List<PaperSize> GetPaperSizeObjects(string printer)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrinterSettings.PrinterName = printer;
            List<PaperSize> _paperSizeList = new List<PaperSize>();
            if (printDoc.PrinterSettings.PaperSizes.Count > 0)
            {
                foreach (PaperSize x in printDoc.PrinterSettings.PaperSizes)
                {
                    _paperSizeList.Add(x);
                }
                return _paperSizeList;
            }
            else
            {
                Eventlog.Log.WriteEntry("SasaLib Printing Class", EventLogEntryType.Error, 0, $"▲GetPaperSizeObjects(), プリンタ名 \"{printer}\" がコントロールパネルのプリンタ一覧から見つかりません ");
                return null;
            }
        }

        /// <summary>
        /// 指定したプリンタで選択できるトレイ情報を取得する
        /// </summary>
        /// <param name="printerName">プリンタ名(Windowsコントロールパネルでの名前)</param>
        /// <returns>トレイ情報</returns>
        public static List<PaperSource> GetPapserSouceObjects(string printer)
        {

            PrintDocument printDoc = new PrintDocument();

            printDoc.PrinterSettings.PrinterName = printer;

            List<PaperSource> _papserSourceList = new List<PaperSource>();

            if (printDoc.PrinterSettings.PaperSources.Count > 0)
            {
                foreach (PaperSource x in printDoc.PrinterSettings.PaperSources)
                {
                    _papserSourceList.Add(x);
                }
                //return printDoc.PrinterSettings.PaperSources;
                return _papserSourceList;
            }
            else
            {
                Eventlog.Log.WriteEntry("SasaLib Printing Class", EventLogEntryType.Error, 0, $"▲GetPapserSouceObjects(), プリンタ名(Windowsコントロールパネルでの名前) が見つかりません {printer}");
                return null;
            }
        }

        /// <summary>
        /// PaperSourceKind を指定し、PaperSourceプロパティを取得する
        /// </summary>
        /// <param name="printDoc"></param>
        /// <param name="paperSouceKind"></param>
        /// <returns></returns>
        public PaperSource GetPrinterSettingPaperSource(PrintDocument printDoc, PaperSourceKind paperSouceKind)
        {
            PrinterSettings ps = new PrinterSettings();
            printDoc.PrinterSettings = ps;
            PaperSource old = ps.PaperSources[0];
            try
            {
                IEnumerable<PaperSource> paperSources = ps.PaperSources.Cast<PaperSource>();
                PaperSource paperSource = paperSources.First<PaperSource>(source => source.Kind == paperSouceKind);
                return paperSource;

            }
            catch (Exception ex)
            {
                Eventlog.Log.WriteEntry("SasaLib Printing Class", EventLogEntryType.Error, 0, $"▲GetPrinterSettingPaperSource(PrintDocument printDoc, PaperSourceKind paperSouceKind), 失敗,Exception={ex.Message}");

                return old;
            }

        }

        /// <summary>
        /// PakerKindを指定し、PageSizeプロパティを取得する。
        /// </summary>
        /// <param name="printDoc"></param>
        /// <param name="paperKind"></param>
        /// <returns></returns>
        public PaperSize GetPageSettingPaperSize(PrintDocument printDoc, PaperKind paperKind)
        {
            PrinterSettings ps = new PrinterSettings();
            printDoc.PrinterSettings = ps;

            IEnumerable<PaperSize> paperSizes = ps.PaperSizes.Cast<PaperSize>();
            PaperSize papeSize = paperSizes.First<PaperSize>(size => size.Kind == paperKind); // setting paper size to A4 size
            return papeSize;
        }



        /// 印刷開始指令

        /// <summary>
        /// プリント開始（ダイアログ表示）
        /// </summary>
        public void PrintImageObjDialog()
        {
            //PrintDocumentオブジェクトの作成
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            //PrintPageイベントハンドラの追加
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(Pd_PrintPage);

            //PrintDialogクラスの作成
            System.Windows.Forms.PrintDialog pdlg = new System.Windows.Forms.PrintDialog();
            //PrintDocumentを指定
            pdlg.Document = pd;
            //印刷の選択ダイアログを表示する
            if (pdlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //OKがクリックされた時は印刷する
                pd.Print();
            }
        }

        /// <summary>
        /// ダイアログ表示なしで通常のプリンタにて印刷
        /// </summary>
        public void PrintImage()
        {
            //PrintDocumentオブジェクトの作成
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            //PrintPageイベントハンドラの追加
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(Pd_PrintPage);
            //印刷を開始する
            pd.Print();

        }

        /// <summary>
        /// 印刷を実行するように指令
        /// </summary>
        /// <param name="printername">string</param>
        /// <param name="PaperSize">System.Drawing.Printing.PaperSize</param>
        /// <param name="Landscape">bool</param>
        /// <param name="PaperSource">System.Drawing.Printing.PaperSource</param>
        /// <param name="outFilePath">string</param>
        public void PrintImage(string printername, // プリンタドライバ名
            System.Drawing.Printing.PaperSize PaperSize, // プリンタドライバ固有の用紙情報
            bool Landscape,// イメージの向き。横向きならTrue
            System.Drawing.Printing.PaperSource PaperSource, // プリンタドライバ固有のトレイ情報          
            string outFilePath = null) //プリントデータをファイルに保存
        {

            PrintDocument printDoc = new PrintDocument();

            printDoc.PrinterSettings.PrinterName = printername;
            printDoc.DefaultPageSettings.Landscape = Landscape;
            printDoc.DefaultPageSettings.PaperSize = PaperSize;
            printDoc.DefaultPageSettings.PaperSource = PaperSource;
            printDoc.DocumentName = DocumentName;

            // プリントファイル出力モード
            if (outFilePath != null)
            {
                printDoc.PrinterSettings.PrintFileName = outFilePath;
                printDoc.PrinterSettings.PrintToFile = true;
            }

            {
                Console.WriteLine("★出力先名=\"" + printername + "\"");
                if (printDoc.PrinterSettings.PrintToFile)
                {
                    Console.WriteLine("★ファイル出力モード:{0}", outFilePath);
                }
                Console.WriteLine("★DefaultPageSettings.PaperSize {0}", printDoc.DefaultPageSettings.PaperSize);
                Console.WriteLine("★DefaultPageSettings.PaperSource {0}", printDoc.DefaultPageSettings.PaperSource);
                Console.WriteLine("★DefaultPageSettings.Landscape {0}", printDoc.DefaultPageSettings.Landscape);
                Console.WriteLine("★DefaultPageSettings.Marginse {0}", printDoc.DefaultPageSettings.Margins);
            }

            printDoc.PrintPage += new PrintPageEventHandler(Pd_PrintPage);

            try
            {
                printDoc.Print();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"※PrintDocument.Print()にて例外検知 {ex.Message} {ex.InnerException}");
            }
        }


        /// 印刷実行関係

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
            if (BeforePrintingImageSaveFilepath != null && BeforePrintingImageSaveFilepath != "") { PrintBmp.Save(BeforePrintingImageSaveFilepath); }

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
