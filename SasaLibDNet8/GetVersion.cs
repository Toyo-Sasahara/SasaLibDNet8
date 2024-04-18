using System;
//using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;

namespace SasaLibDNet8
{
    public class Asm
    {
        /// <summary>
        /// 2019年4月時点obsolete。SaSaLib.AssemblyInfo.GetFileVersion(string AsmPath)を使用のこと。
        /// </summary>
        /// <returns></returns>
        public static string GetFileVersion()
        {
            // 現在実行しているアセンブリ(.exeのアセンブリ情報)を取得する
            var assm = System.Reflection.Assembly.GetExecutingAssembly();

            // アセンブリのファイルパスを取得する
            var path = (new Uri(assm.CodeBase)).LocalPath;

            // アセンブリのFileVersionInfoを取得する
            var versionInfo = FileVersionInfo.GetVersionInfo(path);

            // ファイル名とバージョンを取得して表示する
            //Console.WriteLine("{0} {1}", versionInfo.FileName, versionInfo.FileVersion);
            return versionInfo.FileVersion;
        }

        public static string GetFileVersion(string AsmFilePath)
        {
            System.Diagnostics.FileVersionInfo vi = System.Diagnostics.FileVersionInfo.GetVersionInfo(AsmFilePath);

            //バージョン番号
            Console.WriteLine("FileVersion:{0}", vi.FileVersion);
            //メジャーバージョン番号
            Console.WriteLine("FileMajorPart:{0}", vi.FileMajorPart);
            //マイナバージョン番号
            Console.WriteLine("FileMinorPart:{0}", vi.FileMinorPart);
            //プライベートパート番号
            Console.WriteLine("FilePrivatePart:{0}", vi.FilePrivatePart);
            //ビルド番号
            Console.WriteLine("FileBuildPart:{0}", vi.FileBuildPart);
            //プライベートバージョン
            Console.WriteLine("PrivateBuild:{0}", vi.PrivateBuild);
            //スペシャルビルド
            Console.WriteLine("SpecialBuild:{0}", vi.SpecialBuild);

            //説明
            Console.WriteLine("FileDescription:{0}", vi.FileDescription);
            //著作権
            Console.WriteLine("LegalCopyright:{0}", vi.LegalCopyright);
            //会社名
            Console.WriteLine("CompanyName:{0}", vi.CompanyName);
            //コメント
            Console.WriteLine("Comments:{0}", vi.Comments);
            //内部名
            Console.WriteLine("InternalName:{0}", vi.InternalName);
            //言語
            Console.WriteLine("Language:{0}", vi.Language);
            //商標
            Console.WriteLine("LegalTrademarks:{0}", vi.LegalTrademarks);
            //オリジナルファイル名
            Console.WriteLine("OriginalFilename:{0}", vi.OriginalFilename);

            //製品名
            Console.WriteLine("ProductName:{0}", vi.ProductName);
            //製品バージョン
            Console.WriteLine("ProductVersion:{0}", vi.ProductVersion);
            //製品メジャーバージョン番号
            Console.WriteLine("ProductMajorPart:{0}", vi.ProductMajorPart);
            //製品マイナバージョン番号
            Console.WriteLine("ProductMinorPart:{0}", vi.ProductMinorPart);
            //製品プライベートバージョン番号
            Console.WriteLine("ProductPrivatePart:{0}", vi.ProductPrivatePart);
            //製品ビルド番号
            Console.WriteLine("ProductBuildPart:{0}", vi.ProductBuildPart);

            //デバッグ情報があるか
            Console.WriteLine("IsDebug:{0}", vi.IsDebug);
            //パッチされているか
            Console.WriteLine("IsPatched:{0}", vi.IsPatched);
            //プレリリースか
            Console.WriteLine("IsPreRelease:{0}", vi.IsPreRelease);
            //スペシャルビルドか
            Console.WriteLine("IsSpecialBuild:{0}", vi.IsSpecialBuild);
            return vi.FileVersion;
        }

        /// <summary>
        /// 名前とバージョンを取得して表示する
        /// 2019年4月時点obsolete。SaSaLib.AssemblyInfo.GetAssemlblyVersion()を使用のこと。
        /// </summary>
        /// <returns></returns>
        public static string GetAsmVersion()
        {
            // 現在実行しているアセンブリ(.exeのアセンブリ)を取得する
            var assm = System.Reflection.Assembly.GetExecutingAssembly();

            // AssemblyNameを取得する
            var name = assm.GetName();

            // 名前とバージョンを取得して表示する
            //Console.WriteLine("{0} {1}", name.Name, name.Version);
            //Console.WriteLine("string = {0}", name.Version.ToString());
            return name.Version.ToString();
        }

        public static string GetAsmVersion(string AsmFilePath)
        {
            System.Diagnostics.FileVersionInfo vi = System.Diagnostics.FileVersionInfo.GetVersionInfo(AsmFilePath);

            //バージョン番号
            Console.WriteLine("FileVersion:{0}", vi.FileVersion);
            //メジャーバージョン番号
            Console.WriteLine("FileMajorPart:{0}", vi.FileMajorPart);
            //マイナバージョン番号
            Console.WriteLine("FileMinorPart:{0}", vi.FileMinorPart);
            //プライベートパート番号
            Console.WriteLine("FilePrivatePart:{0}", vi.FilePrivatePart);
            //ビルド番号
            Console.WriteLine("FileBuildPart:{0}", vi.FileBuildPart);
            //プライベートバージョン
            Console.WriteLine("PrivateBuild:{0}", vi.PrivateBuild);
            //スペシャルビルド
            Console.WriteLine("SpecialBuild:{0}", vi.SpecialBuild);

            //説明
            Console.WriteLine("FileDescription:{0}", vi.FileDescription);
            //著作権
            Console.WriteLine("LegalCopyright:{0}", vi.LegalCopyright);
            //会社名
            Console.WriteLine("CompanyName:{0}", vi.CompanyName);
            //コメント
            Console.WriteLine("Comments:{0}", vi.Comments);
            //内部名
            Console.WriteLine("InternalName:{0}", vi.InternalName);
            //言語
            Console.WriteLine("Language:{0}", vi.Language);
            //商標
            Console.WriteLine("LegalTrademarks:{0}", vi.LegalTrademarks);
            //オリジナルファイル名
            Console.WriteLine("OriginalFilename:{0}", vi.OriginalFilename);

            //製品名
            Console.WriteLine("ProductName:{0}", vi.ProductName);
            //製品バージョン
            Console.WriteLine("ProductVersion:{0}", vi.ProductVersion);
            //製品メジャーバージョン番号
            Console.WriteLine("ProductMajorPart:{0}", vi.ProductMajorPart);
            //製品マイナバージョン番号
            Console.WriteLine("ProductMinorPart:{0}", vi.ProductMinorPart);
            //製品プライベートバージョン番号
            Console.WriteLine("ProductPrivatePart:{0}", vi.ProductPrivatePart);
            //製品ビルド番号
            Console.WriteLine("ProductBuildPart:{0}", vi.ProductBuildPart);

            //デバッグ情報があるか
            Console.WriteLine("IsDebug:{0}", vi.IsDebug);
            //パッチされているか
            Console.WriteLine("IsPatched:{0}", vi.IsPatched);
            //プレリリースか
            Console.WriteLine("IsPreRelease:{0}", vi.IsPreRelease);
            //スペシャルビルドか
            Console.WriteLine("IsSpecialBuild:{0}", vi.IsSpecialBuild);
            return vi.ProductVersion;
        }
    }

    public class ClickOnce
    {
        /// <summary>
        /// マニフェスト指定版
        /// </summary>
        /// <returns></returns>
        public static Version GetPublishedVersion(string exemanifest)
        {
            XmlDocument xmlDoc = new XmlDocument();
            //System.Reflection.Assembly asmCurrent = System.Reflection.Assembly.GetExecutingAssembly();
            //string expath = new Uri(asmCurrent.GetName().CodeBase).LocalPath;
            //string exemanifest = expath + ".manifest";

            try
            {
                xmlDoc.Load(exemanifest);
                string retval = string.Empty;
                if (xmlDoc.HasChildNodes)
                {
                    retval = xmlDoc.ChildNodes[1].ChildNodes[0].Attributes.GetNamedItem("version").Value.ToString();
                }
                return new Version(retval);
            }
            catch (IOException e)
            {
                Eventlog.Log.WriteEntry("SasaLib ClickOnce Class", EventLogEntryType.Error, 0, $"▲GetPublishedVersion(), 失敗,Exception={e.Message}");

                return null;
            }
        }

        /// <summary>
        /// 2019年4月時点obsolete。GetPublishedVersion(string exemanifest)の方を使うこと
        /// </summary>
        /// <returns></returns>
        public static Version GetPublishedVersion()
        {
            XmlDocument xmlDoc = new XmlDocument();
            System.Reflection.Assembly asmCurrent = System.Reflection.Assembly.GetExecutingAssembly();
            string expath = new Uri(asmCurrent.GetName().CodeBase).LocalPath;
            string exemanifest = expath + ".manifest";
            return GetPublishedVersion(exemanifest);
        }

        /// <summary>
        /// ClickOne
        /// </summary>アプリの時の起動URL
        //public static void GetUrlParameters()
        //{
        //    // ClickOnceアプリの場合のときのみ以下のコードを実行
        //    if (ApplicationDeployment.IsNetworkDeployed == false)
        //    {
        //        return;
        //    }

        //    // 起動URLを取得
        //    string url =
        //      ApplicationDeployment.CurrentDeployment.ActivationUri.AbsoluteUri;

        //    // クエリ部分を抽出
        //    Uri myUri = new Uri(url);
        //    string queryString = myUri.Query;
        //    if (String.IsNullOrEmpty(queryString))
        //    {
        //        return;
        //    }

        //    // 各パラメータを分離して抽出
        //    string userName = "名無し";
        //    string message = "メッセージはありません";
        //    string[] nameValuePairs = queryString.Split('&');
        //    foreach (string pair in nameValuePairs)
        //    {
        //        string[] vars = pair.Split('=');
        //        if (vars.Length != 2)
        //        {
        //            continue;
        //        }
        //        vars[0] = vars[0].Replace("?", "");  // “?”は削る
        //        if (string.Compare(vars[0], "username", true) == 0)
        //        {
        //            userName = HttpUtility.UrlDecode(vars[1]);
        //        }
        //        else if (string.Compare(vars[0], "message", true) == 0)
        //        {
        //            message = HttpUtility.UrlDecode(vars[1]);
        //        }
        //    }

        //    // 取得した各パラメータをメッセージボックスで表示
        //    System.Windows.MessageBox.Show(userName + "さん、" + message + "。");
        //}

        /// <summary>
        /// 実行中のメイン・アセンブリのフル・パスを取得
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.DebuggerStepThrough]
        public static string GetAssemblyPath()
        {
            // 1. 実行中のメイン・アセンブリのフル・パスを取得する
            Assembly asm = Assembly.GetEntryAssembly();
            string fullPath = asm.Location;

            // 2. フル・パスからディレクトリ・パス部分を抽出する
            string dirPath = Path.GetDirectoryName(fullPath);

            return dirPath;
        }
    }
}
