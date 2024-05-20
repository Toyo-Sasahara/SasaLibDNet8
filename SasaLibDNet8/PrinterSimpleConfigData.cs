using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.Versioning;

namespace SasaLib
{
    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("windows")]
    internal class PrinterSimpleConfigData
    {
        /// <summary>
        /// プリンタの準備完了か
        /// </summary>
        internal bool Ready = false;

        /// <summary>
        /// プリンターに依存する用紙設定情報
        /// </summary>
        PrinterSimpleConfig printerSimpleConfig = new PrinterSimpleConfig();

        /// <summary>
        /// プリンター名（Windowsが認識しているドライバ名）
        /// </summary>
        internal string PrinterName { get; set; }

        /// <summary>
        /// プリンタで使用する用紙サイズコレクション
        /// </summary>
        List<PaperSize> systemDrawingPrintingPaperSizeList;

        /// <summary>
        /// プリンタで使用する用紙供給元のコレクション
        /// </summary>
        List<PaperSource> systemDrawingPrintingPaperSourceList;

        /// <summary>
        /// 印刷時のオフセット値
        /// </summary>
        internal struct Offset { public int X; public int Y; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrinterName"></param>
        /// <param name="paperSizeAndSources"></param>
        internal PrinterSimpleConfigData(string PrinterName, List<PaperSizeAndSource> paperSizeAndSources)
        {
            this.printerSimpleConfig.PaperSizeAndSources = paperSizeAndSources;

            // プリンタ名を プロパティへ
            this.PrinterName = PrinterName;

            // 指定したプリンタで選択できる用紙サイズ情報をすべて取得する
            var paperSizeObjects = Printing.GetPaperSizeObjects(PrinterName);
            if (paperSizeObjects == null)
            {
                Ready = false;
                return;
            }
            else
            {
                systemDrawingPrintingPaperSizeList = paperSizeObjects;
            }

            // 指定したプリンタで選択できる出力先情報をすべて取得する
            List<PaperSource> oPapserSouce = Printing.GetPapserSouceObjects(PrinterName);
            systemDrawingPrintingPaperSourceList = oPapserSouce;

            // 準備完了フラグ
            Ready = true;
        }

        /// <summary>
        /// 現在のプリンタ設定から 指定したCommonPaperSizeに一致したSystem.Drawing.Printing.PaperSize オブジェクトを返す。
        /// </summary>
        /// <param name="commonPaperSize">CommonPaperSize paperSize</param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        internal PaperSize GetPaperSize(PrintConfig.CommonPaperSize commonPaperSize)
        {
            List<PaperSizeAndSource> paperSizeAndSourceList = printerSimpleConfig.PaperSizeAndSources;
            PaperSizeAndSource paperSizeAndSource1 = paperSizeAndSourceList.Find(b => b.CommonPaperSizeEnum == commonPaperSize);

            if (paperSizeAndSource1.CommonPaperSizeEnum == commonPaperSize)
            {
                foreach (PaperSize systemPaperSize in systemDrawingPrintingPaperSizeList)
                {
                    if (systemPaperSize.PaperName == paperSizeAndSource1.PaperName)
                    {
                        return systemPaperSize;
                    }
                }
            }
            
            // プリンタドライバに用紙が無い場合の処理
            // 
            PaperSize paperSize = systemDrawingPrintingPaperSizeList.Find(c => c.PaperName == "A2");
            if (paperSize != null)
                return paperSize;

            return null;
        }

        /// <summary>
        /// 印刷用Bmpの展開パターンを決定
        /// </summary>
        /// <param name="commonPaperSize"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        internal PrinterSimple.BeforeExtractType GetBeforePrintExtractMode(PrintConfig.CommonPaperSize commonPaperSize)
        {
            foreach (PaperSizeAndSource paperSizeAndSource in printerSimpleConfig.PaperSizeAndSources)
            {
                if (paperSizeAndSource.CommonPaperSizeEnum == commonPaperSize)
                {
                    foreach (PaperSize systemPaperSize in systemDrawingPrintingPaperSizeList)
                    {
                        if (systemPaperSize.PaperName == paperSizeAndSource.PaperName)
                        {
                            return paperSizeAndSource.BeforeExtractType;
                        }
                    }
                }
            }
            return PrinterSimple.BeforeExtractType.ページサイズ範囲;
        }


        /// <summary>
        /// 現在のプリンタ設定から 指定したCommonPaperSizeに一致したSystem.Drawing.Printing.PaperSource オブジェクトを返す。
        /// </summary>
        /// <param name="paperSize"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        internal PaperSource GetPaperSource(PrintConfig.CommonPaperSize paperSize)
        {
            if (systemDrawingPrintingPaperSourceList != null)
            {
                foreach (var a in printerSimpleConfig.PaperSizeAndSources)
                {
                    if (a.CommonPaperSizeEnum == paperSize)
                    {
                        foreach (PaperSource curPaperSource in systemDrawingPrintingPaperSourceList)
                        {
                            if (curPaperSource.SourceName == a.SourceName)
                            {
                                return curPaperSource;
                            }
                        }
                    }
                }
            }
            Console.WriteLine("");
            PaperSource defaultSource = new PaperSource();
            return defaultSource;
        }

        /// <summary>
        /// 現在のプリンタ設定から 指定したCommonPaperSizeに一致したLandScapeを返す
        /// </summary>
        /// <param name="paperSize"></param>
        /// <returns></returns>
        internal bool GetLandScape(PrintConfig.CommonPaperSize paperSize)
        {
            foreach (var a in printerSimpleConfig.PaperSizeAndSources)
            {
                if (a.CommonPaperSizeEnum == paperSize)
                {
                    return a.LandScape;
                }
            }
            return false;
        }

        /// <summary>
        /// 印刷時の用紙オフセット
        /// </summary>
        /// <param name="paperSize"></param>
        /// <returns></returns>
        internal Offset GetOffset(PrintConfig.CommonPaperSize paperSize)
        {
            foreach (var a in printerSimpleConfig.PaperSizeAndSources)
            {
                if (a.CommonPaperSizeEnum == paperSize)
                {
                    Offset offset = new Offset { X = a.Xoffset, Y = a.Yoffset };
                    return offset;
                }
            }
            return new Offset { X = 0, Y = 0 };

        }
    }
}
