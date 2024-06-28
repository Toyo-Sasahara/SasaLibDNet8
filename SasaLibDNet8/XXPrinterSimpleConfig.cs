//#if NETCOREAPP
//using System.Collections.Generic;
//using System.Runtime.Versioning;
//#else
//using System.Collections.Generic;
//using System.Runtime.Versioning;
//#endif

//namespace SasaLib
//{
//    /// <summary>
//    /// プリンタに固有な設定を保持する構造体`
//    /// </summary>
//    public struct PaperSizeAndSource
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public PrintConfig.CommonPaperSize CommonPaperSizeEnum;
//        /// <summary>
//        /// 
//        /// </summary>
//        public string Comment;
//        /// <summary>
//        /// 用紙の種類名(string System.Drawing.Printing.PaperSize.PaperName)
//        /// </summary>
//        public string PaperName;

//        //public int width;

//        //public int height;
//        /// <summary>
//        /// 
//        /// </summary>
//        public PrinterSimple.BeforeExtractType BeforeExtractType;
//        /// <summary>
//        /// 
//        /// </summary>
//        public bool LandScape;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int Xoffset;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int Yoffset;
//        /// <summary>
//        /// 
//        /// </summary>
//        public string SourceName;
//    }

//    /// <summary>
//    /// プリンタに固有な設定を保持するオブジェクトを生成するクラス
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    internal class PrinterSimpleConfig
//    {
//        /// <summary>
//        /// プリンタに固有な設定を保持するオブジェクト
//        /// </summary>
//        internal List<PaperSizeAndSource> PaperSizeAndSources;

//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        internal PrinterSimpleConfig()
//        {
//            PaperSizeAndSources = new List<PaperSizeAndSource>()
//            {
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A0横向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A0L,PaperName="A0",Xoffset=0,Yoffset=0,LandScape=true,SourceName="自動"    ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A0縦向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A0P,PaperName="A0",Xoffset=0,Yoffset=0,LandScape=false ,SourceName="自動"  ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A1横向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A1L,PaperName="A1",Xoffset=0,Yoffset=0,LandScape=true ,SourceName="自動"   ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A1縦向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A1P,PaperName="A1",Xoffset=0,Yoffset=0,LandScape=false ,SourceName="自動"  ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A2横向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A2L,PaperName="A2",Xoffset=0,Yoffset=0,LandScape=true ,SourceName="自動"   ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A2縦向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A2P,PaperName="A2",Xoffset=0,Yoffset=0,LandScape=false ,SourceName="自動"  ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A3横向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A3L,PaperName="A3",Xoffset=0,Yoffset=0,LandScape=true ,SourceName="自動"   ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A3縦向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A3P,PaperName="A3",Xoffset=0,Yoffset=0,LandScape=false  ,SourceName="自動" ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A4横向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A4L,PaperName="A4",Xoffset=0,Yoffset=0,LandScape=true ,SourceName="自動"   ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//                new PaperSizeAndSource{Comment="Microsoft Print to PDF A4縦向き",CommonPaperSizeEnum=PrintConfig.CommonPaperSize.A4P,PaperName="A4",Xoffset=0,Yoffset=0,LandScape=false  ,SourceName="自動" ,BeforeExtractType = PrinterSimple.BeforeExtractType.ページサイズ範囲},
//            };

//        }
//    }
//}
