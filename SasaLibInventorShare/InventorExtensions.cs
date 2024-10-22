using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Media.Imaging;
using Inventor;
using System.IO;

public static class InventorExtensions
{
    /// <summary>
    /// 拡張メソッド
    /// </summary>
    /// <param name="document"></param>
    /// <param name="thumbWidth"></param>
    /// <param name="thumbHeight"></param>
    /// <returns></returns>
    public static BitmapImage GetThumbnailAsBitmapImage(this Document document, int thumbWidth = 30 , int thumbHeight = 30)
    {
        BitmapImage docThumbnail;
        if (document == null)
        {
            return null;
        }

        try
        {
            if (document.Thumbnail == null)
            {
                return null;
            }

            Metafile thumbnailMetafile = new Metafile(new IntPtr(document.Thumbnail.Handle), new WmfPlaceableFileHeader());
            System.Drawing.Image.GetThumbnailImageAbort imageCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            Bitmap thumbnailBitmap = thumbnailMetafile.GetThumbnailImage(300, 300, imageCallBack, IntPtr.Zero) as Bitmap;
            docThumbnail = ConvertToBitmapImage(thumbnailBitmap);
            thumbnailMetafile.Dispose();
            docThumbnail = null;
            return docThumbnail;
        }
        catch (Exception ex) when (ex is System.Runtime.InteropServices.COMException || ex is ArgumentException)
        {
            return null;
        }
    }

    private static BitmapImage ConvertToBitmapImage(System.Drawing.Bitmap bitmap)
    {
        if (bitmap == null)
        {
            return null;
        }

        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        MemoryStream editStream = new MemoryStream();
        bitmap.Save(editStream, ImageFormat.Png);
        _ = editStream.Seek(0, SeekOrigin.Begin);
        bitmapImage.StreamSource = editStream;
        bitmapImage.EndInit();
        return bitmapImage;
    }

    private static bool ThumbnailCallback()
    {
        return false;
    }
}