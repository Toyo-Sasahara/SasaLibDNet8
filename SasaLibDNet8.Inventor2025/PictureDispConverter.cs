using Inventor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace SasaLib.InventorAPI
{
    /// <summary>
    /// IPictureDisp への変更 (from bmp, from icon)
    /// </summary>
    public sealed class PictureDispConverter
    {
        //Picture Types
        public const short PICTYPE_UNINITIALIZED = -1;
        public const short PICTYPE_NONE = 0;
        public const short PICTYPE_BITMAP = 1;
        public const short PICTYPE_METAFILE = 2;
        public const short PICTYPE_ICON = 3;
        public const short PICTYPE_ENHMETAFILE = 4;


        [DllImport("oleaut32.dll",  EntryPoint = "OleCreatePictureIndirect",
            ExactSpelling = true, PreserveSig = false)]

        private static extern IPictureDisp

            OleCreatePictureIndirect(
                [MarshalAs(UnmanagedType.AsAny)] object picdesc,
                ref Guid iid,
                [MarshalAs(UnmanagedType.Bool)] bool fOwn);


        static Guid iPictureDispGuid = typeof(IPictureDisp).GUID;



        private static class PICTDESC
        {
            [StructLayout(LayoutKind.Sequential)]
            public class Icon
            {
                internal int cbSizeOfStruct =

                    Marshal.SizeOf(typeof(PICTDESC.Icon));

                internal int picType = PICTYPE_ICON;

                internal IntPtr hicon = IntPtr.Zero;

                internal int unused1;

                internal int unused2;

                internal Icon(System.Drawing.Icon icon)
                {
                    this.hicon = icon.ToBitmap().GetHicon();
                }
            }


            [StructLayout(LayoutKind.Sequential)]
            public class Bitmap
            {
                internal int cbSizeOfStruct =  Marshal.SizeOf(typeof(PICTDESC.Bitmap));

                internal int picType = PICTYPE_BITMAP;

                internal IntPtr hbitmap = IntPtr.Zero;

                internal IntPtr hpal = IntPtr.Zero;

                internal int unused;

                internal Bitmap(System.Drawing.Bitmap bitmap)
                {
                    this.hbitmap = bitmap.GetHbitmap();
                }
            }
        }

        public static IPictureDisp ToIPictureDisp( System.Drawing.Icon icon)
        {

            PICTDESC.Icon pictIcon = new PICTDESC.Icon(icon);

            return OleCreatePictureIndirect(

                pictIcon, ref iPictureDispGuid, true);
        }

        public static IPictureDisp ToIPictureDisp( System.Drawing.Bitmap bmp)

        {

            PICTDESC.Bitmap pictBmp = new PICTDESC.Bitmap(bmp);

            return OleCreatePictureIndirect(pictBmp, ref iPictureDispGuid, true);

        }


        /// <summary>
        /// IPictureDispをImageに（BitmapとMetafaile形式にのみ対応）
        /// </summary>
        /// <param name="pictureDisp"></param>
        /// <returns></returns>
        //public static Image PictureDispToImage(IPictureDisp pictureDisp)
        //{


        //    Image image = null;

        //    if (pictureDisp != null && pictureDisp.Type == PICTYPE_BITMAP)
        //    {

        //        IntPtr paletteHandle = new IntPtr(pictureDisp.hPal);

        //        IntPtr bitmapHandle = new IntPtr(pictureDisp.Handle);

        //        image = Image.FromHbitmap(bitmapHandle, paletteHandle);

        //    }
        //    else if (pictureDisp != null && pictureDisp.Type == PICTYPE_METAFILE)
        //    {
        //        image = new Metafile(new IntPtr(pictureDisp.Handle), new WmfPlaceableFileHeader());

        //    }

        //    return image;

        //}

    }
}
