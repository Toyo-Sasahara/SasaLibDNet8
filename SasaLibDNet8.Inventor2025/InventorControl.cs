using Inventor;
using SasaLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Inventor コントロール用共有ソース
/// </summary>
namespace SasaLib.InventorAPI
{
    [SupportedOSPlatform("windows")]

    public static partial class InventorControl
    {
        /// <summary>
        /// iPart
        /// ■指定したInventor.Documentが iPartかiAssmeblyか調べる
        /// </summary>
        /// <param name="oApp">Inventor.Applicationオブジェクト</param>
        /// <param name="oDoc">Inventor.Documentオブジェクト</param>
        /// <returns></returns>
        public static bool Check_iPart_or_iAssembly(Inventor.Document oDoc)
        {
            if (oDoc.DocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
            {
                Inventor.PartDocument oPartDoc = (Inventor.PartDocument)oDoc;
                // Set a reference to the component definition.
                Inventor.PartComponentDefinition oCompDef = oPartDoc.ComponentDefinition;

                if (!oCompDef.IsiPartFactory)
                {
                    System.Windows.Forms.MessageBox.Show("iPartファクトリではない！", "iPart or iAssemblyファクトリか調査");
                    return false;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("iPartファクトリですね", "iPart or iAssemblyファクトリか調査");
                    return true;
                }

            }
            else if (oDoc.DocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
            {
                Inventor.AssemblyDocument oAssyDoc = (Inventor.AssemblyDocument)oDoc;
                // Set a reference to the component definition.
                Inventor.AssemblyComponentDefinition oCompDef = oAssyDoc.ComponentDefinition;
                if (!oCompDef.IsiAssemblyFactory)
                {
                    System.Windows.Forms.MessageBox.Show("iAssemblyファクトリではない！", "iPart or iAssemblyファクトリか調査");
                    return false;
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("iAssemblyファクトリですね", "iPart or iAssemblyファクトリか調査");
                    return true;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("パーツでもアセンブリでもありません！", "iPart or iAssemblyファクトリか調査");

                return false;
            }
        }

        /// <summary>
        /// SelectionPriority
        /// ■選択の優先順位変更
        /// </summary>
        /// <param name="oDoc"></param>
        /// <param name="selectionPriorityEnum"></param>
        public static void SetSelectionPriority(Inventor.Document oDoc, Inventor.SelectionPriorityEnum selectionPriorityEnum)
        {
            // Inventor.SelectionPriorityEnum selProp = 
            // kAnnotationSelectionPriority 67594 注記選択の優先順位
            // kBodySelectionPriority 67593 ボディ選択の優先順位
            // kComponentSelectionPriority 67590 コンポーネント選択の優先順位
            // kEdgeAndFaceSelectionPriority 67587 エッジとフェースの選択の優先順位
            // kEdgeSelectionPriority 67592 エッジ選択の優先順位
            // kFeatureSelectionPriority 67585 フィーチャ選択の優先順位
            // kGroupSelectionPriority 67588 グループ選択の優先順位
            // kPartSelectionPriority 67591 パーツの選択の優先順位
            // kSketchSelectionPriority 67586 スケッチ選択の優先順位
            // kWireSelectionPriority 67589 ワイヤ選択の優先順位

            oDoc.SelectionPriority = selectionPriorityEnum;
        }

        /// <summary>
        /// SuportMethod
        /// ■stdole.IPictreDisp オブジェクトを System.Drawing.Imageに変換します
        /// 使用先プロジェクト InventorTOYOaddin02
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        //public static System.Drawing.Image GetPictureFromIPicture(IPictureDisp picture)
        //{
        //    if (picture == null)
        //        return null;
        //    IntPtr hpal = IntPtr.Zero;
        //    if (picture.Type == 1) // bitmap
        //    {
        //        try
        //        {
        //            hpal = new IntPtr(picture.hPal);
        //        }
        //        catch (COMException ex)
        //        {
        //            DebugConsole.WriteLine($"GetPictureFromIPicture(..)にて例外発生 {ex.Message}");
        //        }
        //    }
        //    Image retval = GetPictureFromParams(new IntPtr(picture.Handle), picture.Type, hpal, picture.Width, picture.Height);
        //    GC.KeepAlive(picture); // Be sure we keep this   picture alive
        //    // until we copied it safely into our image.
        //    return retval;
        //}

        /// <summary>
        /// Private SupportMethod
        /// GetPictureFromIPicture()にて利用
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="type"></param>
        /// <param name="paletteHandle"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static Image GetPictureFromParams(IntPtr handle, int type, IntPtr paletteHandle, int width, int height)
        {
            switch (type)
            {
                case -1:
                    return null;
                case 0:
                    return null;
                case 1:
                    return Image.FromHbitmap(handle, paletteHandle);
                case 2:
                    {

                        WmfPlaceableFileHeader wmfHeader = new WmfPlaceableFileHeader();
                        wmfHeader.BboxRight = (short)width;
                        wmfHeader.BboxBottom = (short)height;
                        return (Image)new Metafile(handle, wmfHeader, false).Clone();
                    }
                case 3:
                    return (Image)Icon.FromHandle(handle).Clone();
                case 4:
                    return (Image)new Metafile(handle, false).Clone();

            }
            throw new ArgumentException("AXUnknownImage", "type");
        }

        /// <summary>
        /// MassProperties
        /// ■マスプロパティ情報を得る
        /// </summary>
        /// <param name="oPartDoc"></param>
        /// <param name="Area"></param>
        /// <param name="Volume"></param>
        /// <param name="VolumeOverridden"></param>
        /// <param name="Mass"></param>
        /// <param name="MassOverridden"></param>
        public static void GetPartMassProps(Inventor.PartDocument oPartDoc, out double Mass, out bool MassOverridden)
        {
            Inventor.MassProperties oMassProps;
            oMassProps = oPartDoc.ComponentDefinition.MassProperties;
            oMassProps.Accuracy = Inventor.MassPropertiesAccuracyEnum.k_Medium;

            Mass = oMassProps.Mass;
            MassOverridden = oMassProps.MassOverridden;
        }

        /// <summary>
        /// ComponentDefinition/BOMStructure
        /// ■コンポーネントについての規定の部品表を表示
        /// </summary>
        /// <param name="oPartDoc"></param>
        /// <returns></returns>
        public static Inventor.BOMStructureEnum GetCompornetBOMStructure(Inventor.Document oDoc)
        {
            if (oDoc != null)
            {
                try
                {
                    Inventor.DocumentTypeEnum documentType = oDoc.DocumentType;

                    switch (documentType)
                    {
                        case Inventor.DocumentTypeEnum.kAssemblyDocumentObject:
                            Inventor.AssemblyDocument assemblyDocument = (Inventor.AssemblyDocument)oDoc;
                            return assemblyDocument.ComponentDefinition.BOMStructure;
                        case Inventor.DocumentTypeEnum.kPartDocumentObject:
                            Inventor.PartDocument partDocument = (Inventor.PartDocument)oDoc;
                            return partDocument.ComponentDefinition.BOMStructure;
                        default:
                            return Inventor.BOMStructureEnum.kDefaultBOMStructure;
                    }
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"GetCompornetBOMStructure(..)で例外発生：{ex.Message}");

                    return Inventor.BOMStructureEnum.kDefaultBOMStructure;
                }

            }
            else
                return Inventor.BOMStructureEnum.kDefaultBOMStructure;
        }

        /// <summary>
        /// ■コンポーネントについての規定の部品表を設定
        /// </summary>
        /// <param name="oDoc"></param>
        /// <param name="bOMStructureEnum"></param>
        /// <returns></returns>
        public static bool SetCompornetBOMStructure(Inventor.Document oDoc, Inventor.BOMStructureEnum bOMStructureEnum)
        {
            try
            {
                Inventor.DocumentTypeEnum documentType = oDoc.DocumentType;

                switch (documentType)
                {
                    case Inventor.DocumentTypeEnum.kAssemblyDocumentObject:
                        Inventor.AssemblyDocument assemblyDocument = (Inventor.AssemblyDocument)oDoc;
                        assemblyDocument.ComponentDefinition.BOMStructure = bOMStructureEnum;
                        return true;
                    case Inventor.DocumentTypeEnum.kPartDocumentObject:
                        Inventor.PartDocument partDocument = (Inventor.PartDocument)oDoc;
                        partDocument.ComponentDefinition.BOMStructure = bOMStructureEnum;
                        return true;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"SetCompornetBOMStructure(..)で例外発生：{ex.Message}");
                return false;
            }
        }
    }
}
