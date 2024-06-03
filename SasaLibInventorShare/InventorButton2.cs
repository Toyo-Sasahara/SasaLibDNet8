using Inventor;
using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SasaLib.InventorAPI
{
    public static class InventorButton2
    {

        public static void ButtonChange(Inventor.Application InventorApp, string classId, System.Drawing.Icon standardIcon, System.Drawing.Icon largeIcon)
        {


            IPictureDisp standardIconIPictureDisp = (IPictureDisp)PictureDispConverter.ToIPictureDisp(standardIcon);

            IPictureDisp largeIconIPictureDisp = (IPictureDisp)PictureDispConverter.ToIPictureDisp(largeIcon);

            Inventor.ButtonDefinition btndef = InventorApp.CommandManager.ControlDefinitions[classId] as Inventor.ButtonDefinition;

        }

        public static object IconChange( System.Drawing.Icon standardIcon)
        {


            var result = PictureDispConverter.ToIPictureDisp(standardIcon);

            return result;
        }

    }
}
