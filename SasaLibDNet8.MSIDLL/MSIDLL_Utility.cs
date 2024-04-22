using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// 
/// </summary>
namespace SasaLib
{
    /// <summary>
    /// https://clown.cube-soft.jp/entry/20100329/1269844611
    /// </summary>
    [SupportedOSPlatform("windows")]
    public abstract class MSIDLL_Utility
    {
        /// <summary>
        /// filename に対応するパスを検索する．ignore_case が true の場合. 大文字/小文字を区別しない．対応するパスが見つからなかった場合はnullを返す
        /// 検索可能なパスは，Windows Installer 経由でインストールされたプログラムのみ．
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ignore_case"></param>
        /// <returns></returns>
        public static string Find(System.String filename, out List<KeyValuePair<string, string>> ComponentIDs, bool ignore_case = false)
        {

            string resutlPath = null;

            if (ignore_case) filename = filename.ToLower();
            System.Text.StringBuilder component = new System.Text.StringBuilder(64);

            ComponentIDs = new List<KeyValuePair<string, string>>();

            // ERROR_NO_MORE_ITEMS = 259
            for (UInt32 i = 0; MSIDLL_Wrapper.MsiEnumComponents(i, component) != 259; i++)
            {
                System.String path = null;
                try
                {
                    var componentID = component.ToString();

                    path = GetComponentFullpath(componentID);


                    if (path == null) continue;

                    System.String compare = (ignore_case) ? System.IO.Path.GetFileName(path).ToLower() : System.IO.Path.GetFileName(path);

                    if (compare == filename)
                        ComponentIDs.Add(new KeyValuePair<string, string>(componentID, path));

                    resutlPath = path;
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"SasaLib.MSIDLL_Utility.Find(..) 例外トラップ {ex.Message}, path = \"{path}\"");
                }
            }

            if (string.IsNullOrWhiteSpace(resutlPath) == false)
                return resutlPath;
            else
                return null;
        }

        /// <summary>
        /// フルファイル名からコンポーネントIDを得ます
        /// </summary>
        /// <param name="FullFilename"></param>
        /// <param name="ct"></param>
        /// <param name="ignore_case"></param>
        /// <returns></returns>
        //public static string GetComponentID(System.String FullFilename, CancellationToken ct, bool ignore_case = true, SasaLibDelegateWriteLine DebugWriteLine = null)
        //{
        //    if (DebugWriteLine == null) DebugWriteLine = Console.WriteLine;

        //    if (ignore_case) FullFilename = FullFilename.ToUpper();
        //    System.Text.StringBuilder component = new System.Text.StringBuilder(64);

        //    string result = null;

        //    // ERROR_NO_MORE_ITEMS = 259
        //    // MsiEnumComponents 関数は、すべての製品にインストールされているコンポーネントを列挙します。この関数は、呼び出されるたびに 1 つのコンポーネント コードを取得します。
        //    for (UInt32 i = 0; MSIDLL_Wrapper.MsiEnumComponents(i, component) != 259; i++)
        //    {
        //        if (ct.IsCancellationRequested == true)
        //        {
        //            return null;
        //        }

        //        System.String path = null;
        //        try
        //        {
        //            var componentID = component.ToString();

        //            path = GetComponentFullpath(componentID);


        //            if (path == null) continue;

        //            System.String compare = (ignore_case) ? path.ToUpper() : path;

        //            DebugWriteLine($"\"{compare}\" , {componentID}");
        //            if (compare == FullFilename)
        //            {
        //                result = componentID;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            DebugWriteLine($"SasaLib.MSIDLL_Utility.Find(..) 例外トラップ {ex.Message}, path = \"{path}\"");
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        ///  フルファイル名からコンポーネントIDを得ます
        /// </summary>
        /// <param name="FullFilename"></param>
        /// <param name="ct"></param>
        /// <param name="ignore_case"></param>
        /// <param name="DebugWriteLine"></param>
        /// <returns></returns>
        public static List<string> GetComponentIDs(System.String FullFilename, CancellationToken ct, bool ignore_case = true, SasaLibDelegateWriteLine DebugWriteLine = null)
        {
            if (DebugWriteLine == null) DebugWriteLine = Console.WriteLine;

            List<string> componentIDs = new List<string>();

            if (ignore_case) FullFilename = FullFilename.ToUpper();
            System.Text.StringBuilder component = new System.Text.StringBuilder(64);

            UInt32 _fff(UInt32 i, System.Text.StringBuilder lpComponentBuf)
            {
                try
                {
                    // ERROR_NO_MORE_ITEMS = 259
                    // MsiEnumComponents 関数は、すべての製品にインストールされているコンポーネントを列挙します。この関数は、呼び出されるたびに 1 つのコンポーネント コードを取得します。

                    var funcanser = MSIDLL_Wrapper.MsiEnumComponents(i, component);
                    return funcanser;
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"{ex.Message}");
                    return 0;
                }
            }

            for (UInt32 i = 0; _fff(i, component) != 259; i++)
            {

                if (ct.IsCancellationRequested == true)
                {
                    return null;
                }

                System.String path = null;
                var componentID = component.ToString();

                path = GetComponentFullpath(componentID);


                if (path == null) continue;

                System.String compare = (ignore_case) ? path.ToUpper() : path;

                if (compare == FullFilename)
                {
                    DebugWriteLine($"\"{compare}\" , {componentID}");
                    componentIDs.Add(componentID);
                }
                else
                {
                    DebugConsole.WriteLine($"-> {componentID}");
                }
            }

            return componentIDs;
        }

        public static async Task<List<string>> GetComponentIDsAsync(System.String FullFilename, CancellationToken ct, bool ignore_case = true, SasaLibDelegateWriteLine DebugWriteLine = null)
        {
            if (DebugWriteLine == null) DebugWriteLine = Console.WriteLine;

            List<string> componentIDs = new List<string>();

            if (ignore_case) FullFilename = FullFilename.ToUpper();
            System.Text.StringBuilder component = new System.Text.StringBuilder(64);


            List<string> list = await Task.Run(() =>
            {

                //try
                //{
                // ERROR_NO_MORE_ITEMS = 259
                // MsiEnumComponents 関数は、すべての製品にインストールされているコンポーネントを列挙します。この関数は、呼び出されるたびに 1 つのコンポーネント コードを取得します。
                for (UInt32 i = 0; MSIDLL_Wrapper.MsiEnumComponents(i, component) != 259; i++)
                {

                    if (ct.IsCancellationRequested == true)
                    {
                        return null;
                    }

                    System.String path = null;
                    var componentID = component.ToString();

                    path = GetComponentFullpath(componentID);


                    if (path == null) continue;

                    System.String compare = (ignore_case) ? path.ToUpper() : path;

                    if (compare == FullFilename)
                    {
                        //DebugWriteLine($"\"{compare}\" , {componentID}");
                        componentIDs.Add(componentID);
                    }
                    else
                    {
                        DebugConsole.WriteLine($"-> {componentID}");
                    }
                }


                return componentIDs;

            });

            List<string> results = list;

            return results;
            //    }
            //                catch (Exception ex)
            //                {
            //                    DebugWriteLine($"SasaLib.MSIDLL_Utility.Find(..) 例外トラップ {ex.Message}, path = \"{path}\"");
            //}
        }


        public static List<string> GetClientID(string componentId, CancellationToken ct, SasaLibDelegateWriteLine DebugWriteLine = null)
        {
            if (DebugWriteLine == null) DebugWriteLine = Console.WriteLine;


            List<string> componentIDs = new List<string>();

            //if (ignore_case) FullFilename = FullFilename.ToUpper();

            System.Text.StringBuilder component = new System.Text.StringBuilder(64);

            /// MsiEnumClients 関数は、インストールされている特定のコンポーネントのクライアントを列挙します。 関数は、呼び出されるたびに 1 つの製品コードを取得します。
            /// 
            // UINT MsiEnumClients(
            //  [in] LPCSTR szComponent,
            //  [in] DWORD iProductIndex,
            //  [out] LPSTR lpProductBuf
            //);            /// );

            try
            {
                for (UInt32 i = 0; MSIDLL_Wrapper.MsiEnumClients(componentId, i, component) != 259; i++)
                {

                    if (ct.IsCancellationRequested == true)
                    {
                        return null;
                    }

                    //System.String path = null;
                    var componentID = component.ToString();

                    //path = GetComponentFullpath(componentID);


                    //if (path == null) continue;

                    //System.String compare = (ignore_case) ? path.ToUpper() : path;

                    //DebugWriteLine($"\"{compare}\" , {componentID}");
                    componentIDs.Add(componentID);
                }

                return componentIDs;

            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"{ex.Message}");

                return null;
            }
        }

        /// <summary>
        /// 指定されたコンポーネントID に対応するパスを検索する．見つからなかった場合は null を返す．
        /// </summary>
        /// <param name="componentID"></param>
        /// <returns></returns>
        public static System.String GetComponentFullpath(System.String componentID)
        {

            System.Text.StringBuilder product = new System.Text.StringBuilder(64);
            if (MSIDLL_Wrapper.MsiGetProductCode(componentID, product) != 0)
            {
                return null;
            }

            UInt32 len = 2048;
            while (true)
            {
                DoEvents.Run();

                UInt32 result = len;
                try
                {
                    System.Text.StringBuilder path = new System.Text.StringBuilder((int)len);

                    //MsiGetComponentPath 関数は、インストールされているコンポーネントへの完全パスを返します。コンポーネントのキー パスがレジストリ キーの場合、レジストリ キーが返されます。
                    var anser = MSIDLL_Wrapper.MsiGetComponentPath(product.ToString(), componentID, path, ref result);


                    if (anser < 0)
                    {
                        //DebugConsole.WriteLine($"anser = {anser}");
                        break;
                    }

                    if (result <= len)
                    {
                        return path.ToString();
                    }
                    else if (result > len)
                    {
                        DebugConsole.WriteLine($"必要なバッファサイズ result = {result} > 現在のバッファサイズ:{len} のため len バッファをふやします");
                        len *= 2; // バッファ不足
                        DebugConsole.WriteLine($"現在のバッファサイズ:len = {len} としました");
                    }
                    else if (result > 536870912)
                    {
                        break;
                    }

                }
                catch (Exception ex)
                {
                    DebugConsole.Write($"MSIDLL_Utility.GetComponentFullpath(..)にて例外トラップ {ex.Message}");
                }
            }
            return null;
        }

        /// <summary>
        /// プロダクトコードによっていんすとーるされたコンポーネントIDを得る
        /// </summary>
        /// <param name="product"></param>
        /// <param name="ct"></param>
        /// <param name="ignore_case"></param>
        /// <param name="DebugWriteLine"></param>
        /// <returns></returns>
        public static List<string> GetComponents_Test(System.String productID, CancellationToken ct, bool ignore_case = true, SasaLibDelegateWriteLine DebugWriteLine = null)
        {
            if (DebugWriteLine == null) DebugWriteLine = Console.WriteLine;

            List<string> componetIDs = new List<string>();

            //if (ignore_case) FullFilename = FullFilename.ToUpper();
            System.Text.StringBuilder component = new System.Text.StringBuilder(64);

            // ERROR_NO_MORE_ITEMS = 259
            // MsiEnumComponents 関数は、すべての製品にインストールされているコンポーネントを列挙します。この関数は、呼び出されるたびに 1 つのコンポーネント コードを取得します。
            for (UInt32 i = 0; MSIDLL_Wrapper.MsiEnumComponents(i, component) != 259; i++)
            {
                if (ct.IsCancellationRequested == true)
                {
                    return null;
                }
                string componentID = null;
                try
                {
                    componentID = component.ToString();

                    System.Text.StringBuilder sbPProductCode = new System.Text.StringBuilder(64);
                    MSIDLL_Wrapper.MsiGetProductCode(componentID, sbPProductCode);

                    string pID = sbPProductCode.ToString();

                    // System.String compare = (ignore_case) ? path.ToUpper() : path;


                    if (pID == productID)
                    {
                        DebugWriteLine($"\"{pID}\" , {componentID}");
                        componetIDs.Add(componentID);
                    }

                }
                catch (Exception ex)
                {
                    DebugWriteLine($"SasaLib.MSIDLL_Utility.Find(..) 例外トラップ {ex.Message}, componentID = \"{componentID}\"");
                }
            }

            return componetIDs;
        }


        /// <summary>
        /// 
        /// </summary>
        [SupportedOSPlatform("windows")]
        public class ProductInfo
        {
            public string ProductCode;
            public string ProductName;
            public string VersionString;
            public DateTime InstallDate;
            public string LocalPackage;
        }

        /// <summary>
        /// ｺﾝﾎﾟｰﾈﾝﾄIDを含むプロダクトを検索
        /// </summary>
        /// <param name="ComponentName"></param>
        /// <returns></returns>
        public static ProductInfo FindProductName(string ComponentID)
        {
            StringBuilder sbProductCode = new StringBuilder(39);
            List<ProductInfo> mystructs = new List<ProductInfo>();

            System.Text.StringBuilder sbPProductCode = new System.Text.StringBuilder(64);
            MSIDLL_Wrapper.MsiGetProductCode(ComponentID, sbPProductCode);

            uint iIdx = 0;
            while (0 == MSIDLL_Wrapper.MsiEnumProducts(iIdx++, sbProductCode))
            {
                Int32 productNameLen = 512;
                StringBuilder sbProductName = new StringBuilder(productNameLen);
                MSIDLL_Wrapper.MsiGetProductInfo(sbProductCode.ToString(), "ProductName", sbProductName, ref productNameLen);


                Int32 ProductIDLen = 512;
                StringBuilder sbProductID = new StringBuilder(ProductIDLen);
                MSIDLL_Wrapper.MsiGetProductInfo(sbProductCode.ToString(), "ProductID", sbProductID, ref ProductIDLen);

                DebugConsole.WriteLine($"ProductName = \"{sbProductName.ToString()}\" {sbProductCode.ToString()}");
                mystructs.Add(new ProductInfo() { ProductName = sbProductName.ToString(), ProductCode = sbProductCode.ToString() });

                //if (sbProductName.ToString().Contains(ComponentName))
                //{
                //    Int32 installDirLen = 1024;
                //    StringBuilder sbInstallDir = new StringBuilder(installDirLen);

                //    MSIDLL_Wrapper.MsiGetProductInfo(sbProductCode.ToString(), "InstallLocation", sbInstallDir, ref installDirLen);

                //    Console.WriteLine($"ProductName {sbProductName}: {sbInstallDir}");

                //    anser = sbInstallDir.ToString();
                //}
            }

            ProductInfo found = mystructs.FirstOrDefault(abc => abc.ProductCode == sbPProductCode.ToString());

            return found;
        }

        /// <summary>
        /// ﾌﾟﾛﾀﾞｸﾄIDからﾌﾟﾛﾀﾞｸﾄ情報を取得
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static ProductInfo GetProductInfo(string ProductID)
        {

            //StringBuilder sbProductID = new StringBuilder(ProductID); // string ProductID から StringBuidler sbProductID を 生成。比較対象とする

            List<ProductInfo> productInfos = new List<ProductInfo>(); // ProductInfoオブジェクトを保持するListを生成

            uint iIdx = 0;

            System.Text.StringBuilder sbPProductCodeInComponentID = new System.Text.StringBuilder(64);

            while (0 == MSIDLL_Wrapper.MsiEnumProducts(iIdx++, sbPProductCodeInComponentID))
            {
                Int32 productNameLen = 512;
                StringBuilder sbProductName = new StringBuilder(productNameLen);

                MSIDLL_Wrapper.MsiGetProductInfo(sbPProductCodeInComponentID.ToString(), "ProductName", sbProductName, ref productNameLen);


                Int32 productVersionStringLen = 512;
                StringBuilder sbProductVersionString = new StringBuilder(productVersionStringLen);
                MSIDLL_Wrapper.MsiGetProductInfo(sbPProductCodeInComponentID.ToString(), "VersionString", sbProductVersionString, ref productVersionStringLen);


                productInfos.Add(new ProductInfo() { ProductName = sbProductName.ToString(), ProductCode = sbPProductCodeInComponentID.ToString(), VersionString = sbProductVersionString.ToString() });

            } // List<ProductInfo> productInfos コレクションを新規作成


            //ProductInfo foundProductInfo = productInfos.FirstOrDefault(item => item.ProductCode == sbProductID.ToString()); // productInfos オブジェクトから指定したプロダクトIDを検索
            ProductInfo foundProductInfo = productInfos.FirstOrDefault(item => item.ProductCode == ProductID); // productInfos オブジェクトから指定したプロダクトIDを検索

            return foundProductInfo;
        }

        /// <summary>
        /// すべてのインストール済みコンポーネント情報を得る
        /// </summary>
        /// <returns></returns>
        public static List<ProductInfo> GetProductInfo()
        {
            List<ProductInfo> productInfos = new List<ProductInfo>(); // ProductInfoオブジェクトを保持するListを生成

            uint iIdx = 0;
            System.Text.StringBuilder sbPProductCodeInComponentID = new System.Text.StringBuilder(64);
            while (0 == MSIDLL_Wrapper.MsiEnumProducts(iIdx++, sbPProductCodeInComponentID))
            {
                //Int32 productNameLen = 512;
                //StringBuilder sbProductName = new StringBuilder(productNameLen);
                //MSIDLL_Wrapper.MsiGetProductInfo(sbPProductCodeInComponentID.ToString(), "ProductName", sbProductName, ref productNameLen);

                //Int32 productVersionStringLen = 512;
                //StringBuilder sbProductVersionString = new StringBuilder(productVersionStringLen);
                //MSIDLL_Wrapper.MsiGetProductInfo(sbPProductCodeInComponentID.ToString(), "VersionString", sbProductVersionString, ref productVersionStringLen);

                StringBuilder prperty_ProductName = _GetProperty("ProductName");
                if (string.IsNullOrWhiteSpace(prperty_ProductName.ToString()))
                {
                    DebugConsole.WriteLine($"※エラー： {sbPProductCodeInComponentID.ToString()} はコンポーネントIDがNULLまたは空文字です");
                }
                StringBuilder prperty_VersionString = _GetProperty("VersionString");

                StringBuilder prperty_InstallDateStr = _GetProperty("InstallDate");
                DateTime prperty_InstallDate;
                try
                {
                    prperty_InstallDate = DateTime.ParseExact(prperty_InstallDateStr.ToString(), "yyyyMMdd", null);
                }
                catch
                {
                    prperty_InstallDate = DateTime.MinValue;
                }

                StringBuilder prperty_LocalPackage = _GetProperty("LocalPackage");
                StringBuilder prperty_InstalledProductName = _GetProperty("InstalledProductName");


                productInfos.Add(new ProductInfo()
                {
                    ProductCode = sbPProductCodeInComponentID.ToString(),
                    ProductName = prperty_ProductName.ToString(),
                    VersionString = prperty_VersionString.ToString(),
                    InstallDate = prperty_InstallDate,
                    LocalPackage = prperty_LocalPackage.ToString(),
                });

            } // List<ProductInfo> productInfos コレクションを新規作成

            StringBuilder _GetProperty(string propertyName)
            {
                Int32 StringLen = 512;
                StringBuilder resultSb = new StringBuilder(StringLen);
                MSIDLL_Wrapper.MsiGetProductInfo(sbPProductCodeInComponentID.ToString(), propertyName, resultSb, ref StringLen);
                return resultSb;
            }

            return productInfos;
        }

    }
}

// サンプル
//namespace SearchComponent
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            for (int i = 0; i < args.Length; i++)
//            {
//                System.String path = SasaLib.MSIDLL_Utility.Find(args[i], true);
//                if (path == null) System.Console.WriteLine("{0} is not found", args[i]);
//                else System.Console.WriteLine(path);
//            }
//        }
//    }
//}