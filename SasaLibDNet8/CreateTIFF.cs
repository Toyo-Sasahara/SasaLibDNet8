using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib
{
    /// <summary>
    /// PMﾌｧｲﾙからTIFF作成
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class CreateTIFF
    {
        /// <summary>
        /// メモリストリームにあるPJLからTIFFイメージを抜き出してファイル化.
        /// ファイル名にはPJLデータ中のジョブ名を一部使用 テスト用コードに格下げ 2021-10-19</summary>
        /// <param name="msPJLrawdata">PJLデータのストリーム</param>
        /// <param name="Path">保存先パス</param>
        /// <param name="saveFile">ファイル名</param>
        /// <param name="rotateFlipType">回転</param>
        /// <param name="debug"></param>
        /// <returns></returns>
        static bool ExtractTiffFile(MemoryStream msPJLrawdata, string Path, string saveFile, RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone, int debug = 1)
        {
            // PJLdecodeオブジェクトを生成
            PJL.PJLdecode pJLdecode = new PJL.PJLdecode(msPJLrawdata);
            Console.WriteLine("デバッグ：ExtractTiffFile(...)① \n", debug);

            // PJLデータか検証後実行
            if (pJLdecode.IsPJL)
            {
                Console.WriteLine("デバッグ：ExtractTiffFile(...)② \n", debug);
                int num = 0;
                foreach (var tiffStream in pJLdecode.imageFileDataStreams)
                {
                    Console.WriteLine("デバッグ：ExtractTiffFile(...)③ \n", debug);
                    num++;

                    System.Drawing.Image Img = ImageUtil.TiffStreamToImage(tiffStream);
                    if (Img != null)
                    {
                        Console.WriteLine("デバッグ：Pixcel Format = " + Img.PixelFormat.ToString() + "\n", debug);
                        Img.RotateFlip(rotateFlipType);
                        // Tiffファイルの保存
                        // 
                        string saveFileName;
                        if (num > 1)
                        {
                            saveFileName = System.IO.Path.Combine(Path, saveFile + "-" + num.ToString("000") + @".TIF");
                        }
                        else
                        {
                            saveFileName = System.IO.Path.Combine(Path, saveFile + @".TIF");
                        }

                        //Img.Save(saveFileName); //強制２値化をしない場合、この行を有効にしてSaveImageToFileを無効にする
                        // 強制2値化後、書き出し
                        ImageUtil.SaveImageToFile(Img, "image/tiff", System.Drawing.Imaging.EncoderValue.CompressionLZW, saveFileName);
                        Img.Dispose();

                    }
                    else
                    {
                        return false;
                    }
                }
                Console.WriteLine("デバッグ：ExtractTiffFile(...)終了 \n", debug);
                return true;
            }
            else
            {
                Console.WriteLine("デバッグ：ExtractTiffFile(...)　PJLではない \n", debug);
                pJLdecode.Dispose();
                return false;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// プリンタ出力ファイル(PJLコード+TIFFデータ からTIFFデータを取り出し保存)テスト用コードに格下げ 2021-10-19
        /// </summary>
        /// <param name="PlotNativeWriteFilePath"></param>
        /// <param name="SaveFolder"></param>
        /// <param name="baseFileName"></param>
        /// <param name="rotate"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool MakeImageFromPJL(string PlotNativeWriteFilePath, string SaveFolder, string baseFileName, RotateFlipType rotate = RotateFlipType.RotateNoneFlipNone)
        {
            int debug = 0;
            Console.WriteLine("■TIFFファイル出力開始・・・・");
            // TIFF化開始
            if (FileFolder.FileExists(PlotNativeWriteFilePath) == true)
            {
                Console.WriteLine("pmファイル準備確認:" + PlotNativeWriteFilePath, debug);

                Console.WriteLine("TIFFファイル出力開始", debug);

                // プリントファイルpmをメモリストリームに読み込み
                System.IO.MemoryStream pjlReadBuff;
                pjlReadBuff = StreamExtensions.StreamFromFile(PlotNativeWriteFilePath);


                if (ExtractTiffFile(pjlReadBuff, SaveFolder, baseFileName, rotate, debug) == false)
                {
                    Console.WriteLine("◇pmファイルからTIFへの変換失敗\n");
                    //FileFolder.RemoveFile(SaveFolder + baseFileName + @".pm");
                    return false;
                }

                //FileFolder.RemoveFile(SaveFolder + baseFileName + @".pm");

                Console.WriteLine("完了\n");
                return true;
            }
            else
            {
                Console.WriteLine("◇TIFFファイル出力失敗\n");
                return false;
            }
        }

    }
}
