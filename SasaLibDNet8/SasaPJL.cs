using SasaLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SasaLib.PJL
{
    /// <summary>
    /// PJL言語で記述されたプリンタ印刷データをデコードするクラス
    /// </summary>
    public class PJLdecode : MemoryStream
    {
        /// <summary>
        /// プリンタ印刷データストリームを保存
        /// </summary>
        private MemoryStream msbufPrintData = new MemoryStream();

        /// <summary>
        /// sbufPrintdataストリームの中から グラフィックデータのみを保管
        /// </summary>
        public Stream[] imageFileDataStreams { get { return rawDataStreams; } }
        /// <summary>
        /// 
        /// </summary>
        private Stream[] rawDataStreams;

        /// <summary>
        /// PJLデータならTrue
        /// </summary>
        public bool IsPJL { get { return ispjl; } }
        bool ispjl = false;

        /// <summary>
        /// PJLのジョブデータ名
        /// </summary>
        public string JobNameStr { get { return jobNameStr; } }
        string jobNameStr = "Noname";

        byte[] PJL_START_CodeBytes = System.Text.Encoding.ASCII.GetBytes("\x1b" + "%-12345X@PJL");
        byte[] PJL_ENTER_LANGUAGE_CodeBytes = System.Text.Encoding.ASCII.GetBytes("@PJL ENTER LANGUAGE");
        byte[] PJL_ENTER_LANGUAGE_TIFF6_CodeBytes = System.Text.Encoding.ASCII.GetBytes("@PJL ENTER LANGUAGE = TIFF6" + "\x0a");
        byte[] PJL_DATA_END_CodeBytes = System.Text.Encoding.ASCII.GetBytes("\x1b" + "%-12345X" + "\x0a");
        byte[] PJL_COMMENT_OID_JOB_NAME_CodeBytes = System.Text.Encoding.ASCII.GetBytes("@PJL COMMENT OID_ATT_JOB_NAME");
        byte[] PJL_OID_JOB_NAME_End_CodeBytes = System.Text.Encoding.ASCII.GetBytes(";" + "\x0a");
        byte[] PJL_JOB_NAME_CodeBytes = System.Text.Encoding.ASCII.GetBytes("@PJL JOB NAME");
        byte[] PJL_SET_JOBNAME_CodeBytes = System.Text.Encoding.ASCII.GetBytes("@PJL SET JOBNAME");
        byte[] PJL_SET_USERNAME_CodeBytes = System.Text.Encoding.ASCII.GetBytes("@PJL SET USERNAME");

        byte[] LineFead_Bytes = System.Text.Encoding.ASCII.GetBytes("\x0a");

        /// <summary>
        /// コンストラクタ。フィールド MemorsyStraem msbufに 使用するストリームを複写する
        /// </summary>
        /// <param name="ms">使用するストリーム</param>
        public PJLdecode(MemoryStream ms)
        {
            Init(ms);
        }
        public PJLdecode(string fpath)
        {
            MemoryStream ms = StreamExtensions.StreamFromFile(fpath);
            Init(ms);
        }

        bool Init(MemoryStream ms)
        {
            msbufPrintData.Position = 0;
            ms.Position = 0;
            ms.CopyTo(msbufPrintData);

            ispjl = CheckPJLdata();
            if (IsPJL)
            {
                jobNameStr = GetOID_ATT_JOB_NAME();
                GetRawData();
                return true;
            }
            return false;
        }

        /// <summary>
        /// msbufがプリンタファイル（PJL言語を使用）かを検証
        /// </summary>
        /// <returns></returns>
        bool CheckPJLdata()
        {
            byte[] readFrontByte = new byte[PJL_START_CodeBytes.Length];
            msbufPrintData.Seek(0, SeekOrigin.Begin);
            msbufPrintData.Read(readFrontByte, 0, PJL_START_CodeBytes.Length);

            bool isEqual1 = ((IStructuralEquatable)PJL_START_CodeBytes).Equals(readFrontByte, StructuralComparisons.StructuralEqualityComparer);
            if (isEqual1 != true)
            {
                Console.WriteLine("CheckPJLdata() PJLデータではありません");
            }
            return isEqual1;
        }

        /// <summary>
        /// ATT_JOB_NAME を取得する
        /// </summary>
        /// <returns></returns>
        string GetOID_ATT_JOB_NAME()
        {
            // "@PJL COMMENT OID_ATT_JOB_NAME "が最初にヒットしたポジションから 検索文字列分 進んだ位置
            long endposEnter_OIDATTJOB_NAME = StreamExtensions.SearchStreamByteArray(msbufPrintData, PJL_COMMENT_OID_JOB_NAME_CodeBytes, 0) + PJL_COMMENT_OID_JOB_NAME_CodeBytes.Length;
            long endposEnd_OIDATTJOB_NAME = StreamExtensions.SearchStreamByteArray(msbufPrintData, PJL_OID_JOB_NAME_End_CodeBytes, endposEnter_OIDATTJOB_NAME);
            if (endposEnter_OIDATTJOB_NAME != -1)
            {
                MemoryStream distst = new MemoryStream();
                StreamExtensions.CopyOtherStream(msbufPrintData, endposEnter_OIDATTJOB_NAME, endposEnd_OIDATTJOB_NAME, distst);
                //Shift-jisとしてよみこみ
                string JobName = Encoding.GetEncoding(932).GetString(distst.ToArray()).Trim(' ', '"');
                Console.WriteLine(JobName);
                distst.Dispose();
                return JobName;
            }
            else
                return "NotName";
        }

        /// <summary>
        /// msbufPrintDataからRawDATAを抽出
        /// </summary>
        private bool GetRawData()
        {
            if (IsPJL)
            {

                List<Stream> streamList = new List<Stream>();
                int num = 0;

                long ans;
                long start1 = 0;
                do
                {
                    ans = StreamExtensions.SearchStreamByteArray(msbufPrintData, PJL_ENTER_LANGUAGE_CodeBytes, start1);
                    if (ans > -1)
                    {
                        long TypeNameStart = ans + PJL_ENTER_LANGUAGE_CodeBytes.Length;

                        long lineFadePos = StreamExtensions.SearchStreamByteArray(msbufPrintData, LineFead_Bytes, TypeNameStart);

                        if (lineFadePos > -1)
                        {
                            byte[] langNameBytes = StreamExtensions.StreamFromGetBytes(msbufPrintData, TypeNameStart, lineFadePos);
                            string langNameStr = System.Text.Encoding.ASCII.GetString(langNameBytes).Trim('"', '=', ' ').Trim(' ');

                            // Rawデータのスタートポジションとエンドポジションん
                            long startRawPos = lineFadePos + 1;
                            Console.WriteLine("1 msbufPrintData.Position={0}", msbufPrintData.Position);

                            long endRawPos = StreamExtensions.SearchStreamByteArray(msbufPrintData, PJL_DATA_END_CodeBytes, startRawPos);

                            Console.WriteLine("2 msbufPrintData.Position={0}", msbufPrintData.Position);
                            Console.WriteLine("Raw Data Start={0}, End={1}", startRawPos, endRawPos);



                            start1 = endRawPos;

                            switch (langNameStr)
                            {
                                case "TIFF6":
                                    Console.WriteLine("Type Tiff");

                                    MemoryStream distSt = new MemoryStream();

                                    StreamExtensions.CopyOtherStream(msbufPrintData, startRawPos, endRawPos, distSt);
                                    streamList.Add(distSt);
                                    num++;

                                    break;
                                default:
                                    Console.WriteLine("認識できないRaw");
                                    break;
                            }

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                } while (ans > -1);

                rawDataStreams = streamList.ToArray();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// フィールド msbufPrintData ストリームからTIFFストリームを取り出し
        /// </summary>
        /// <returns>tiffデータへのストリーム配列</returns>
        public Stream[] GetTiffStreams()
        {
            long start = 0;
            long end = 0;
            int num = 0;

            List<Stream> streamList = new List<Stream>();

            while (true)
            {
                long pos = StreamExtensions.SearchStreamByteArray(msbufPrintData, PJL_ENTER_LANGUAGE_TIFF6_CodeBytes, start);
                if (pos > -1)
                {
                    start = pos + PJL_ENTER_LANGUAGE_TIFF6_CodeBytes.Length;
                    end = StreamExtensions.SearchStreamByteArray(msbufPrintData, PJL_DATA_END_CodeBytes, start);
                    //Console.WriteLine("num= {0} start={1} , end ={2}", num, start, end);
                    MemoryStream distSt = new MemoryStream();

                    StreamExtensions.CopyOtherStream(msbufPrintData, start, end, distSt);
                    streamList.Add(distSt);
                    num++;
                }
                else
                {
                    Console.WriteLine("List.Add={0}", num);
                    break;
                }

            }
            return streamList.ToArray();

        }

        /// <summary>
        /// ImageのListを取得　TiffStreamToImage()を使用してストリームをImageにする場合
        /// ページ数が多いとメモリオーバーフロー
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Image[] GetTiffImages()
        {
            List<System.Drawing.Image> imageList = new List<System.Drawing.Image>();

            foreach (Stream tiffStream in GetTiffStreams())
            {
                System.Drawing.Image im = ImageUtil.TiffStreamToImage(tiffStream);

                imageList.Add(im);
            }
            return imageList.ToArray();

        }

        /// <summary>
        /// TiffStreamToImage2()を使用してストリームをImageにする場合
        /// </summary>
        /// <returns></returns>
        public System.Drawing.Image[] GetTiffImages2()
        {
            List<System.Drawing.Image> imageList = new List<System.Drawing.Image>();

            foreach (Stream tiffStream in GetTiffStreams())
            {
                System.Drawing.Image im = ImageUtil.TiffStreamToImage2(tiffStream);

                imageList.Add(im);
            }
            return imageList.ToArray();

        }

        /// <summary>
        /// メモリストリームにあるPJLからTIFFイメージを抜き出してファイル化.
        /// ファイル名にはPJLデータ中のジョブ名を一部使用
        /// </summary>
        /// <param name="msPJLrawdata">PJLデータのストリーム</param>
        /// <param name="savePathFile">保存先フォルダ</param>
        /// <param name="rotateFlipType">回転</param>
        public static void WriteTiffFiles(MemoryStream msPJLrawdata, string savePath,
           System.Drawing.RotateFlipType rotateFlipType = System.Drawing.RotateFlipType.RotateNoneFlipNone)
        {
            // PJLdecodeオブジェクトを生成
            PJL.PJLdecode pJLdecode = new PJL.PJLdecode(msPJLrawdata);


            // PJLデータか検証後実行
            if (pJLdecode.IsPJL)
            {

                //GUID生成
                GUIDExtensions gUIDExtensions = new GUIDExtensions(true);

                Console.WriteLine("JobNameAndRnd={0}", pJLdecode.JobNameStr);

                int num = 0;
                // 配列pjl.RawDataStreamsからtiffデータを取り出し 
                foreach (var tiffStream in pJLdecode.imageFileDataStreams)
                {
                    num++;

                    ///①新しくGUIDをセット
                    gUIDExtensions.SetNewGUID();
                    Console.WriteLine("TIFFデータ処理を開始 {0} 回目 GUID Code={1}", num, gUIDExtensions.B64String);

                    ///②tiffストリームから System.Drawing.Image を生成
                    System.Drawing.Image newImage = ImageUtil.TiffStreamToImage(tiffStream);
                    Console.WriteLine("Pixcel Format =|0|", newImage.PixelFormat.ToString());

                    ImageUtil.RotateImage((System.Drawing.Bitmap)newImage, rotateFlipType);

                    string fileName = pJLdecode.JobNameStr.Replace(@"\", "￥").Replace(@":", "：") + "_" + num.ToString("000") + "-" + gUIDExtensions.B64String + ".tif";


                    newImage.Dispose();

                    //元がtiffのストリームなので、そのままtiffファイル化
                    StreamExtensions.StreamToFile(tiffStream, savePath + @"\" + fileName);
                } // End of foreach

            }

        }

        /// <summary>
        /// MemoryStreamの PJLデータからTIFFデータをStreamにて取得
        /// </summary>
        /// <param name="msPJLrawdata"></param>
        /// <returns></returns>
        public static bool ExtractTIFFstream(MemoryStream msPJLrawdata, out Stream tiffStream1st)
        {
            // PJLdecodeオブジェクトを生成
            PJL.PJLdecode pJLdecode = new PJL.PJLdecode(msPJLrawdata);

            // PJLデータか検証後実行
            if (pJLdecode.IsPJL)
            {
                tiffStream1st = pJLdecode.imageFileDataStreams[0];

                System.Drawing.Image Img = ImageUtil.TiffStreamToImage(tiffStream1st);

                //Img.Save(saveFileName); //強制２値化をしない場合、この行を有効にしてSaveImageToFileを無効にする             
                // 強制2値化後、書き出し
                // ImageUtil.SaveImageToFile(Img, "image/tiff", System.Drawing.Imaging.EncoderValue.CompressionCCITT4, @"D:\TESTTEST.tif");
                Img.Dispose();


                Console.WriteLine("デバッグ：ExtractTiffFile(...)終了 \n");
                return true;
            }
            else
            {
                Console.WriteLine("デバッグ：ExtractTiffFile(...)　PJLではない \n");
                pJLdecode.Dispose();
                tiffStream1st = null;
                return false;
            }
        }


    }
}
