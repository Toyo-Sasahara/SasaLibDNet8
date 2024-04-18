using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SasaLib
{
    public class OleCreateConverter
    {
        [DllImport("oleaut32.dll", EntryPoint = "OleCreatePictureIndirect",
            CharSet = CharSet.Ansi, ExactSpelling = true, PreserveSig = true)]

        private static extern int OleCreatePictureIndirect(

            [In] PictDescBitmap pictdesc, ref Guid iid, bool fOwn,

            [MarshalAs(UnmanagedType.Interface)] out object ppVoid);


        //Picture Types
        public const short PICTYPE_UNINITIALIZED = -1;
        public const short PICTYPE_NONE = 0;
        public const short PICTYPE_BITMAP = 1;
        public const short PICTYPE_METAFILE = 2;
        public const short PICTYPE_ICON = 3;
        public const short PICTYPE_ENHMETAFILE = 4;


        [StructLayout(LayoutKind.Sequential)]
        [SupportedOSPlatform("windows")]
        internal class PictDescBitmap
        {

            internal int cbSizeOfStruct = Marshal.SizeOf(typeof(PictDescBitmap));

            internal int pictureType = PICTYPE_BITMAP;

            internal IntPtr hBitmap = IntPtr.Zero;

            internal IntPtr hPalette = IntPtr.Zero;

            internal int unused = 0;

            internal PictDescBitmap(Bitmap bitmap)

            {

                this.hBitmap = bitmap.GetHbitmap();

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static stdole.IPictureDisp ImageToPictureDisp(Image image)
        {
            if (image == null || !(image is Bitmap))

            {

                return null;

            }


            PictDescBitmap pictDescBitmap = new PictDescBitmap((Bitmap)image);

            object ppVoid = null;

            Guid iPictureDispGuid = typeof(stdole.IPictureDisp).GUID;

            OleCreatePictureIndirect(pictDescBitmap, ref iPictureDispGuid, true, out ppVoid);

            stdole.IPictureDisp picture = (stdole.IPictureDisp)ppVoid;

            return picture;

        }


        /// <summary>
        /// IPictureDispをImageに変更 (BitmapとMetafileのみ)
        /// </summary>
        /// <param name="pictureDisp"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static Image PictureDispToImage(stdole.IPictureDisp pictureDisp)
        {
            Image image = null;

            if (pictureDisp != null && pictureDisp.Type == PICTYPE_BITMAP)
            {

                IntPtr paletteHandle = new IntPtr(pictureDisp.hPal);

                IntPtr bitmapHandle = new IntPtr(pictureDisp.Handle);

                image = Image.FromHbitmap(bitmapHandle, paletteHandle);

            }
            else if (pictureDisp != null && pictureDisp.Type == PICTYPE_METAFILE)
            {
                image = new Metafile(new IntPtr(pictureDisp.Handle), new WmfPlaceableFileHeader());

            }

            return image;

        }

    }
}
