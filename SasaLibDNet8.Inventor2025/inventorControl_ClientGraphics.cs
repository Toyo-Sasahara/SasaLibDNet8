using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.InventorAPI
{
    public static partial class InventorControl
    {
        /// <summary>
        /// ■テキストグラフィックを画面中央にいい具合に表示
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="oDoc"></param>
        /// <param name="msg"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <param name="FontSize"></param>
        /// <param name="Red"></param>
        /// <param name="Green"></param>
        /// <param name="Yellow"></param>
        /// <returns></returns>
        public static TextGraphics ClientGraphicsTextWrite(Inventor.Application oApp, Document oDoc, 
            string CollectionName, string msg ,double X = 0, double Y = 0, double Z =0 ,
            string Font = "MS GOTHIC", double FontSize = 20, bool Bold = false ,bool Italic = false,
            byte Red=0,byte Green =255, byte Yellow =0)
        {
            ComponentDefinition oCompDef;
            switch (oDoc.DocumentType)
            {
                case DocumentTypeEnum.kAssemblyDocumentObject:
                    oCompDef = (ComponentDefinition)(((AssemblyDocument)oDoc).ComponentDefinition);
                    break;

                case DocumentTypeEnum.kPartDocumentObject:
                    oCompDef = (ComponentDefinition)(((PartDocument)oDoc).ComponentDefinition);
                    break;

                default:
                    return null;
            }

            ClientGraphics oClientGraphics = default;
            try
            {
                oClientGraphics = oCompDef.ClientGraphicsCollection[CollectionName];
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"ClientGraphicsTextWrite(...)にて例外 {ex.Message}");
                if (oClientGraphics != null)
                {
                    oClientGraphics.Delete();
                }
            }
            oApp.ActiveView.Update();

            if (oClientGraphics == null)
            {
                oClientGraphics = oCompDef.ClientGraphicsCollection.Add(CollectionName);
            }

            GraphicsNode oNode = oClientGraphics.AddNode(1);

            TextGraphics oTextGraphics = oNode.AddTextGraphics();

            oTextGraphics.Text = msg;
            oTextGraphics.Anchor = oApp.TransientGeometry.CreatePoint(X, Y, Z);
            oTextGraphics.Bold = Bold;
            oTextGraphics.Font = Font;
            oTextGraphics.FontSize = FontSize;
            oTextGraphics.HorizontalAlignment = HorizontalTextAlignmentEnum.kAlignTextLeft;
            oTextGraphics.Italic = Italic;
            oTextGraphics.PutTextColor(Red, Green, Yellow);
            oTextGraphics.VerticalAlignment = VerticalTextAlignmentEnum.kAlignTextMiddle;

            oApp.ActiveView.Update();
            return oTextGraphics;
        }

        /// <summary>
        /// ■テキストグラフィックを削除
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="oDoc"></param>
        /// <param name="CollectionName"></param>
        public static void ClientGraphicsDelete(Inventor.Application oApp, Document oDoc ,string CollectionName)
        {
            ComponentDefinition oCompDef;
            switch (oDoc.DocumentType)
            {
                case DocumentTypeEnum.kAssemblyDocumentObject:
                    oCompDef = (ComponentDefinition)(((AssemblyDocument)oDoc).ComponentDefinition);
                    break;

                case DocumentTypeEnum.kPartDocumentObject:
                    oCompDef = (ComponentDefinition)(((PartDocument)oDoc).ComponentDefinition);
                    break;

                default:
                    return;
            }

            ClientGraphics oClientGraphics = default;
            try
            {
                oClientGraphics = oCompDef.ClientGraphicsCollection[CollectionName];
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"ClientGraphicsDelete(...)にて例外 {ex.Message}");

                if (oClientGraphics != null)
                {
                    oClientGraphics.Delete();
                }
            }
            if (oClientGraphics != null)
            {
                oClientGraphics.Delete();
            }

            oApp.ActiveView.Update();

        }

    }
}
