//using System;
//using System.Collections.Generic;
//using System.Drawing.Printing;
//using System.Runtime.Serialization;
//using System.Runtime.Versioning;
//using System.Xml;

//namespace SasaLib.PrintConfig
//{
//    /// <summary>
//    /// 標準用紙
//    /// </summary>
//    [Serializable]
//    public enum CommonPaperSize {
//        /// <summary>
//        /// 
//        /// </summary>
//        A0L,
//        /// <summary>
//        /// 
//        /// </summary>
//        A0P,
//        /// <summary>
//        /// 
//        /// </summary>
//        A1L,
//        /// <summary>
//        /// 
//        /// </summary>
//        A1P,
//        /// <summary>
//        /// 
//        /// </summary>
//        A2L,
//        /// <summary>
//        /// 
//        /// </summary>
//        A2P,
//        /// <summary>
//        /// 
//        /// </summary>
//        A3L,
//        /// <summary>
//        /// 
//        /// </summary>
//        A3P,
//        /// <summary>
//        /// 
//        /// </summary>
//        A4L,
//        /// <summary>
//        /// 
//        /// </summary>
//        A4P,
//        /// <summary>
//        /// 
//        /// </summary>
//        A0Lx2,
//        /// <summary>
//        /// 
//        /// </summary>
//        A1Lx2,
//        /// <summary>
//        /// 
//        /// </summary>
//        Custom = 99,
//        /// <summary>
//        /// 
//        /// </summary>
//        Unknown = 9999 
//    }

//    /// <summary>
//    /// 出力方法の選択（プリンタに依存させない）
//    /// </summary>
//    public enum CommonOutputTyep {
//        /// <summary>
//        /// 
//        /// </summary>
//        自動,
//        /// <summary>
//        /// 
//        /// </summary>
//        A1自動縮小,
//        /// <summary>
//        /// 
//        /// </summary>
//        A2自動縮小,
//        /// <summary>
//        /// 
//        /// </summary>
//        A3自動縮小,
//        /// <summary>
//        /// 
//        /// </summary>
//        A4自動縮小,
//        /// <summary>
//        /// 
//        /// </summary>
//        Custom = 99
//    }

//    /// <summary>
//    /// 印刷時の印刷領域
//    /// </summary>
//    public enum BeforeExtractType {
//        /// <summary>
//        /// 
//        /// </summary>
//        原寸,
//        /// <summary>
//        /// 
//        /// </summary>
//        原寸オフセット,
//        /// <summary>
//        /// 
//        /// </summary>
//        マージン範囲,
//        /// <summary>
//        /// 
//        /// </summary>
//        ページサイズ範囲,
//        /// <summary>
//        /// 
//        /// </summary>
//        Custom = 99
//    }

//    /// <summary>
//    /// 用紙サイズを縦・横の長さから選択
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public static class PaperCheck
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        struct JisPaper
//        {
//            public int Width { get; set; }
//            public int Height { get; set; }
//        }

//        //static Dictionary<JisPaper, CommonPaperSize> SizeDictionary = new Dictionary<JisPaper, CommonPaperSize>()
//        //{
//        //    { new JisPaper{Width= 841,Height=1189 },CommonPaperSize.A0P},
//        //    { new JisPaper{Width=1189,Height= 841 },CommonPaperSize.A0L},
//        //    { new JisPaper{Width= 594,Height= 841 },CommonPaperSize.A1P},
//        //    { new JisPaper{Width= 841,Height= 594 },CommonPaperSize.A1L},
//        //    { new JisPaper{Width= 420,Height= 594 },CommonPaperSize.A2P},
//        //    { new JisPaper{Width= 594,Height= 420 },CommonPaperSize.A2L},
//        //    { new JisPaper{Width= 297,Height= 420 },CommonPaperSize.A3P},
//        //    { new JisPaper{Width= 420,Height= 297 },CommonPaperSize.A3L},
//        //    { new JisPaper{Width= 210,Height= 297 },CommonPaperSize.A4P},
//        //    { new JisPaper{Width= 297,Height= 210 },CommonPaperSize.A4L}
//        //};

//        //public static Dictionary<CommonPaperSize, JisPaper> PaperSize2 = new Dictionary<CommonPaperSize, JisPaper>()
//        //{
//        //    {CommonPaperSize.A0P, new JisPaper{Width= 841,Height=1189 }},
//        //    {CommonPaperSize.A0L, new JisPaper{Width=1189,Height= 841 }},
//        //    {CommonPaperSize.A1P, new JisPaper{Width= 594,Height= 841 }},
//        //    {CommonPaperSize.A1L, new JisPaper{Width= 841,Height= 594 }},
//        //    {CommonPaperSize.A2P, new JisPaper{Width= 420,Height= 594 }},
//        //    {CommonPaperSize.A2L, new JisPaper{Width= 594,Height= 420 }},
//        //    {CommonPaperSize.A3P, new JisPaper{Width= 297,Height= 420 }},
//        //    {CommonPaperSize.A3L, new JisPaper{Width= 420,Height= 297 }},
//        //    {CommonPaperSize.A4P, new JisPaper{Width= 210,Height= 297 }},
//        //    {CommonPaperSize.A4L, new JisPaper{Width= 297,Height= 210 }}
//        //};

//        /// <summary>
//        /// 
//        /// </summary>
//        struct JISPaper
//        {
//            public int Width { get; set; }
//            public int Height { get; set; }
//            public CommonPaperSize Name;
//            public string DisplayName;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        static List<JISPaper> JISpaperSize = new List<JISPaper>()
//        {
//             new JISPaper{Name = CommonPaperSize.A0P,DisplayName="A0縦", Width= 841,Height=1189},
//             new JISPaper{Name = CommonPaperSize.A0L,DisplayName="A0横", Width=1189,Height= 841},
//             new JISPaper{Name = CommonPaperSize.A1P,DisplayName="A1縦", Width= 594,Height= 841},
//             new JISPaper{Name = CommonPaperSize.A1L,DisplayName="A1横", Width= 841,Height= 594},
//             new JISPaper{Name = CommonPaperSize.A2P,DisplayName="A2縦", Width= 420,Height= 594},
//             new JISPaper{Name = CommonPaperSize.A2L,DisplayName="A2横", Width= 594,Height= 420},
//             new JISPaper{Name = CommonPaperSize.A3P,DisplayName="A3縦", Width= 297,Height= 420},
//             new JISPaper{Name = CommonPaperSize.A3L,DisplayName="A3横", Width= 420,Height= 297},
//             new JISPaper{Name = CommonPaperSize.A4P,DisplayName="A4縦", Width= 210,Height= 297},
//             new JISPaper{Name = CommonPaperSize.A4L,DisplayName="A4横", Width= 297,Height= 210}
//        };

//        /// <summary>
//        /// enum CommonPaperSize から 表示名を取得
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <returns></returns>
//        public static string GetDisplayName(CommonPaperSize cp)
//        {
//            foreach (JISPaper jpaper in JISpaperSize)
//            {
//                if (jpaper.Name == cp)
//                {
//                    return jpaper.DisplayName;
//                }
//            }
//            return "定型外";       
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="cp"></param>
//        /// <param name="isNormalDirection"></param>
//        /// <returns></returns>
//        public static System.Drawing.Size GetCommonPaperSize(CommonPaperSize cp, bool isNormalDirection = true)
//        {
//            JISPaper a = JISpaperSize.Find(x => x.Name == cp);
//            System.Drawing.Size size;
            
//            if (isNormalDirection)
//                size = new System.Drawing.Size(a.Width, a.Height);
//            else
//                size = new System.Drawing.Size(a.Height, a.Width);

//            return size;
//        }

//        /// <summary>
//        /// 縦横の長さ(mm)から用紙サイズ判別
//        /// </summary>
//        /// <param name="size"></param>
//        /// <param name="gosa"></param>
//        /// <returns></returns>
//        [SupportedOSPlatform("windows")]
//        public static CommonPaperSize GetJISpaperSize(PM size, double gosa)
//        {
//            return (GetJISpaperSize(size.Width, size.Height, gosa));
//        }

//        /// <summary>
//        /// 縦横の長さ(mm)から用紙サイズ判別
//        /// </summary>
//        /// <param name="X">X mm</param>
//        /// <param name="Y">Y mm</param>
//        /// <param name="gosa">規定サイズからの±逸脱可能範囲 mm</param>
//        /// <returns></returns>
//        public static CommonPaperSize GetJISpaperSize(double X, double Y, double gosa)
//        {
//            bool Nealist(double var, double _gosa, double target)
//            {
//                if (Math.Abs(var - target) <= _gosa)
//                {
//                    return true;
//                }
//                else
//                    return false;
//            }

//            foreach (JISPaper a in JISpaperSize)
//            {
//                if (Nealist(a.Width, gosa, X) && Nealist(a.Height, gosa, Y))
//                {
//                    return a.Name;
//                }
//            }
//            return CommonPaperSize.Custom;

//        }
//    }



//    /// <summary>
//    /// コンフィグレーションクラス
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class RootConfigClass
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public string Tittle;
//        /// <summary>
//        /// 
//        /// </summary>
//        public string Comment;
//        /// <summary>
//        /// 
//        /// </summary>
//        public string Version;
//        /// <summary>
//        /// 
//        /// </summary>
//        public Dictionary<string, PrinterClass> Printer = new Dictionary<string, PrinterClass>();
//    }

//    /// <summary>
//    /// プリンタークラス
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class PrinterClass
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public string PrinterName;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int IDnumber;
//        /// <summary>
//        /// 
//        /// </summary>
//        public Dictionary<CommonPaperSize, PaperClass> Paper = new Dictionary<CommonPaperSize, PaperClass>();
//    }

//    /// <summary>
//    /// 用紙クラス
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class PaperClass
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public CommonPaperSize CommonPaperSize;
//        /// <summary>
//        /// 
//        /// </summary>
//        public string CommonPaperName;
//        /// <summary>
//        /// 
//        /// </summary>
//        public long Width;
//        /// <summary>
//        /// 
//        /// </summary>
//        public long Height;
//        /// <summary>
//        /// 
//        /// </summary>
//        public ConfigClass Settings = new ConfigClass();
//    }

//    /// <summary>
//    /// 保存内容の保持クラス（孫）
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class ConfigClass
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public string プリンタ名 { get; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public CommonPaperSize 用紙サイズ { get; }
//        /// <summary>
//        /// 
//        /// </summary>
//        public CommonOutputTyep 出力方法 = CommonOutputTyep.自動;
//        /// <summary>
//        /// 
//        /// </summary>
//        public bool ランドスケープ = true;
//        /// <summary>
//        /// 
//        /// </summary>
//        public BeforeExtractType 印刷前展開 = BeforeExtractType.原寸オフセット;
//        /// <summary>
//        /// 
//        /// </summary>
//        public float Xオフセット値mm = 0;
//        /// <summary>
//        /// 
//        /// </summary>
//        public float Yオフセット値mm = 0;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int PaperSizeHeight;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int PaperSizeWidth;
//        /// <summary>
//        /// 
//        /// </summary>
//        public System.Drawing.Printing.PaperKind PaperSizeKind = PaperKind.A4;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int PaperSizeRawKind;
//        /// <summary>
//        /// 
//        /// </summary>
//        public string PaperSizePaperName;
//        /// <summary>
//        /// 
//        /// </summary>
//        public System.Drawing.Printing.PaperSourceKind PaperSourceKind = PaperSourceKind.AutomaticFeed;
//        /// <summary>
//        /// 
//        /// </summary>
//        public int PaperSourceRawKind;
//        /// <summary>
//        /// 
//        /// </summary>
//        public string PaperSourceSourceName;
//    }


//    /// <summary>
//    /// 設定管理クラス
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class ConfigInit
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public RootConfigClass Root;

//        private const string FILENAME = @"D:\クラスシリアライズテスト.xml";

//        /// <summary>
//        /// 設定管理クラスのコンストラクタ
//        /// </summary>
//        public ConfigInit()
//        {
//            Root = new RootConfigClass();

//            //
//            AddPrinterConfig("Default", 0);
//            foreach (PaperClass pc in GetCommonPaperSize())
//            {
//                ModifySetting("Default", pc.CommonPaperSize);

//            }
//            //
//            AddPrinterConfig(Printing.GetDefaultPrinterName(), 0);
//            foreach (PaperClass pc in GetCommonPaperSize())
//            {
//                ModifySetting(Printing.GetDefaultPrinterName(), pc.CommonPaperSize);

//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void ClassTest()
//        {
//        }

//        /// <summary>
//        /// プリンタを追加
//        /// </summary>
//        /// <param name="PrinterName"></param>
//        /// <param name="ID"></param>
//        public bool AddPrinterConfig(string PrinterName, int ID)
//        {
//            if (Root.Printer.ContainsKey(PrinterName) == false)
//            {
//                PrinterClass PrinterObj = new PrinterClass
//                {
//                    // 新しくプリンタobjを確保
//                    PrinterName = PrinterName,
//                    IDnumber = ID
//                }; Root.Printer.Add(PrinterName, PrinterObj); // 設定値objのドライバリストに新しく追加

//                //var aa = GetCommonPaperSize();
//                //foreach (var a in aa)
//                //{
//                //    Root.Printer[PrinterName].Paper.Add(a.CommonPaperSize,
//                //        new PaperClass { CommonPaperSize = a.CommonPaperSize, CommonPaperName = a.CommonPaperName, Width = a.Width, Height = a.Height });
//                //}
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }


//        /// <summary>
//        /// 標準用紙をリストで取得
//        /// </summary>
//        /// <returns></returns>
//        public static List<PaperClass> GetCommonPaperSize()
//        {
//            List<PaperClass> obj = new List<PaperClass>()
//            {
//                new PaperClass {CommonPaperSize=CommonPaperSize.A0P, CommonPaperName="A0縦", Width= 841 ,Height=1189},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A0L, CommonPaperName="A0横", Width=1189 ,Height= 841},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A1P, CommonPaperName="A1縦", Width= 594 ,Height= 841},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A1L, CommonPaperName="A1横", Width= 841 ,Height= 594},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A2P, CommonPaperName="A2縦", Width= 420 ,Height= 594},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A2L, CommonPaperName="A2横", Width= 594 ,Height= 420},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A3P, CommonPaperName="A3縦", Width= 297 ,Height= 420},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A3L, CommonPaperName="A3横", Width= 420 ,Height= 297},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A4P, CommonPaperName="A4縦", Width= 210 ,Height= 297},
//                new PaperClass {CommonPaperSize=CommonPaperSize.A4L, CommonPaperName="A4横", Width= 297 ,Height= 210}
//            };

//            return obj;
//        }

//        /// <summary>
//        /// プリンタ設定を変更
//        /// </summary>
//        /// <param name="a1"></param>
//        /// <param name="aa"></param>
//        public void ModifySetting(string a1, CommonPaperSize aa)
//        {
//            if (Root.Printer[a1].Paper.ContainsKey(aa))
//            {
//                Root.Printer[a1].Paper[aa].Settings.ランドスケープ = true;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="filepath"></param>
//        /// <returns></returns>
//        public bool Save(string filepath)
//        {
//            //DataContractSerializerオブジェクトを作成
//            //オブジェクトの型を指定する
//            DataContractSerializer serializer = new DataContractSerializer(typeof(RootConfigClass));

//            //BOMが付かないUTF-8で、書き込むファイルを開く
//            XmlWriterSettings settings = new XmlWriterSettings()
//            {
//                Indent = true,
//                IndentChars = "\t"
//            };
//            settings.Encoding = new System.Text.UTF8Encoding(false);
//            XmlWriter xw = XmlWriter.Create(FILENAME, settings);
//            //シリアル化し、XMLファイルに保存する
//            serializer.WriteObject(xw, Root);

//            //ファイルを閉じる
//            xw.Close();
//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="filepath"></param>
//        /// <returns></returns>
//        public bool Load(string filepath)
//        {
//            //DataContractSerializerオブジェクトを作成
//            DataContractSerializer serializer = new DataContractSerializer(typeof(RootConfigClass));
//            //読み込むファイルを開く
//            XmlReader xr = XmlReader.Create(FILENAME);
//            //XMLファイルから読み込み、逆シリアル化する
//            Root = (RootConfigClass)serializer.ReadObject(xr);
//            //ファイルを閉じる
//            xr.Close();
//            return true;
//        }
//    }

//}


