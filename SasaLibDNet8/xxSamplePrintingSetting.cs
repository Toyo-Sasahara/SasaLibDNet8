//using SasaLib;
//using SasaLibDummy;
//using System;
//using System.Collections.Generic;
//using System.Runtime.Versioning;

//namespace Samps
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class CPaperSelectionStandard : XmlSettingFile
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public CPaperSelectionStandard() { }

//        /// <summary>
//        /// 印刷イメージの 用紙サイズと方向のプロパティ（本ライブラリで共通）
//        /// </summary>
//        [System.Xml.Serialization.XmlAttribute("SourcePaperIdent")]
//        public string? SourcePaperIdent { get; set; }

//        /// <summary>
//        /// PageSetting.PaperSize.PaperName プロパティに設定する値(用紙の種類（サイズ）)
//        /// </summary>
//        [System.Xml.Serialization.XmlElement("PaperSizePaperName")]
//        public string? PaperSizePaperName { get; set; }

//        /// <summary>
//        /// PageSetting.PaperSize.RawKindプロパティに設定する値
//        /// </summary>
//        [System.Xml.Serialization.XmlElement("PaperSizeRawKind")]
//        public int PaperSizeRawKind { get; set; }

//        /// <summary>
//        /// PageSetting.PaperSource.SourceName プロパティに設定する値 (プリンタトレイ名)
//        /// </summary>
//        [System.Xml.Serialization.XmlElement("PaperSourceSourceName")]
//        public string? PaperSourceSourceName { get; set; }

//        /// <summary>
//        /// PageSetting.Landscape プロパティに設定する値 true=ページを横向きで印刷
//        /// </summary>
//        [System.Xml.Serialization.XmlElement("Landscape")]
//        public bool Landscape { get; set; }

//       // public List<C用紙選択設定2> 用紙選択設定2o { get; set; }

//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    [System.Xml.Serialization.XmlRoot("PrinterConfig")]
//    public class CPrinterConfig : XmlSettingFile
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public CPrinterConfig() { }

//        /// <summary>
//        /// PrinterSettings.PrinterNameプロパティに設定する値
//        /// </summary>
//        [System.Xml.Serialization.XmlElement("PrinterSettingsPrinterName")]
//        public string? PrinterSettingsPrinterName { get; set; }
//        /// <summary>
//        /// プリンタ固有の用紙選択基準の設定値
//        /// </summary>
//        [System.Xml.Serialization.XmlElement("PaperSelectionStandard")]
//        public List<CPaperSelectionStandard>? PaperSelectionStandard { get; set; }
//    }
//}

//namespace Samps
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class Test2
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public static void start()
//        {
//            //シリアライズする為のprinterConfigインスタンスを生成
//            CPrinterConfig printerConfig = new CPrinterConfig();
//            printerConfig.PaperSelectionStandard = new List<CPaperSelectionStandard>();
//        }

//    }
//}
//namespace Samps
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class TestProg
//    {
//        /// <summary>
//        /// シリアライズのテスト
//        /// </summary>
//        [SupportedOSPlatform("windows")]
//        public static void SerializeTest()
//        {
//            //シリアライズする為のPersonsインスタンスを生成
//            CPrinterConfig printerConfig = new Samps.CPrinterConfig();

//            printerConfig.PaperSelectionStandard = new List<Samps.CPaperSelectionStandard>();


//            //インスタンスに値を設定
//            // プリンタモデル名
//            printerConfig.PrinterSettingsPrinterName = @"DocuCentre-V 5080 -(機械設計）";

//            // A0横データ
//            Samps.CPaperSelectionStandard A0L = new Samps.CPaperSelectionStandard();
//            A0L.SourcePaperIdent = "A0L";
//            A0L.PaperSourceSourceName = "A4縦";
//            A0L.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A0L.PaperSizePaperName = "トレイ１";
//            A0L.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A0L);

//            // A1横データ
//            Samps.CPaperSelectionStandard A1L = new Samps.CPaperSelectionStandard();
//            A1L.SourcePaperIdent = "A1L";
//            A1L.PaperSourceSourceName = "A3７横";
//            A1L.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A1L.PaperSizePaperName = "カセット１";
//            A1L.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A1L);

//            // A2横データ
//            Samps.CPaperSelectionStandard A2L = new Samps.CPaperSelectionStandard();
//            A2L.SourcePaperIdent = "A2L";
//            A2L.PaperSourceSourceName = "";
//            A2L.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A2L.PaperSizePaperName = "";
//            A2L.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A2L);

//            // A3横データ
//            Samps.CPaperSelectionStandard A3L = new Samps.CPaperSelectionStandard();
//            A3L.SourcePaperIdent = "A3L";
//            A3L.PaperSourceSourceName = "";
//            A3L.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A3L.PaperSizePaperName = "";
//            A3L.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A3L);

//            // A4横データ
//            Samps.CPaperSelectionStandard A4L = new Samps.CPaperSelectionStandard();
//            A4L.SourcePaperIdent = "A4L";
//            A4L.PaperSourceSourceName = "";
//            A4L.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A4L.PaperSizePaperName = "";
//            A4L.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A4L);

//            // A0縦データ
//            Samps.CPaperSelectionStandard A0P = new Samps.CPaperSelectionStandard();
//            A0P.SourcePaperIdent = "A0P";
//            A0P.PaperSourceSourceName = "";
//            A0P.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A0P.PaperSizePaperName = "";
//            A0P.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A0P);

//            // A1縦データ
//            Samps.CPaperSelectionStandard A1P = new Samps.CPaperSelectionStandard();
//            A1P.SourcePaperIdent = "A1P";
//            A1P.PaperSourceSourceName = "";
//            A1P.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A1P.PaperSizePaperName = "";
//            A1P.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A1P);

//            // A2縦データ
//            Samps.CPaperSelectionStandard A2P = new Samps.CPaperSelectionStandard();
//            A2P.SourcePaperIdent = "A2P";
//            A2P.PaperSourceSourceName = "";
//            A2P.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A2P.PaperSizePaperName = "";
//            A2P.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A2P);

//            // A3縦データ
//            Samps.CPaperSelectionStandard A3P = new Samps.CPaperSelectionStandard();
//            A3P.SourcePaperIdent = "A3P";
//            A3P.PaperSourceSourceName = "";
//            A3P.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A3P.PaperSizePaperName = "";
//            A3P.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A3P);

//            // A4縦データ
//            Samps.CPaperSelectionStandard A4P = new Samps.CPaperSelectionStandard();
//            A4P.SourcePaperIdent = "A4P";
//            A4P.PaperSourceSourceName = "";
//            A4P.PaperSizeRawKind = (int)System.Drawing.Printing.PaperKind.Custom;
//            A4P.PaperSizePaperName = "";
//            A4P.Landscape = false;
//            printerConfig.PaperSelectionStandard.Add(A4P);

//            //
//            //Samps.CPaperSelectionStandard Axx = new Samps.CPaperSelectionStandard();
//            //A1L.SourcePaperIdent = "";
//            //A1L.PaperSourceSourceName = "";
//            //A1L.PaperSizePaperName = "";
//            //A1L.Landscape = false;
//            //printerConfig.PaperSelectionStandard.Add(A1L);

//            //出力先XMLのストリーム
//            System.IO.FileStream stream = new System.IO.FileStream(@"D:\EXPORT.XML", System.IO.FileMode.Create);
//            System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, System.Text.Encoding.UTF8);

//            //シリアライズ
//            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Samps.CPrinterConfig));
//            serializer.Serialize(writer, printerConfig);

//            writer.Flush();
//            writer.Close();
//        }

//        /// <summary>
//        /// デシリアライズのテスト
//        /// </summary>
//        public static void DeSerializeTest()
//        {
//            System.IO.FileStream fs = new System.IO.FileStream(@"D:\EXPORT.XML", System.IO.FileMode.Open);

//            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(CPrinterConfig));

//            CPrinterConfig printerConfig = (CPrinterConfig)serializer.Deserialize(fs);

//            foreach (CPaperSelectionStandard paperSelectionStandard in printerConfig.PaperSelectionStandard)
//            {
//                Console.WriteLine(String.Format("SourcePaperIdent={0}," +
//                    " PaperSourceSourceName={1}," +
//                    " PaperSizePaperName={2}," +
//                    " Landscape={3}",
//                    paperSelectionStandard.SourcePaperIdent,
//                    paperSelectionStandard.PaperSourceSourceName,
//                    paperSelectionStandard.PaperSizePaperName,
//                    paperSelectionStandard.Landscape));
//            }
//        }
//    }
//}
