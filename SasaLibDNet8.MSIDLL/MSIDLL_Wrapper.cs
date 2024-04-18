using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

/// <summary>
/// 
/// </summary>
namespace SasaLib
{
    /// <summary>
    /// https://clown.cube-soft.jp/entry/20100329/1269844611
    /// </summary>
    public abstract class MSIDLL_Wrapper
    {
        private const System.String MSI_DLL = "msi.dll";

        /// <summary>
        /// MsiEnumComponents 関数は、すべての製品にインストールされているコンポーネントを列挙します。この関数は、呼び出されるたびに 1 つのコンポーネント コードを取得します。
        /// 
        ///  UINT MsiEnumComponents (
        ///    DWORD  iComponentIndex,
        ///    LPTSTR lpComponentBuf
        ///  );
        ///  http://msdn.microsoft.com/en-us/library/aa370097(VS.85).aspx
        /// </summary>
        /// <param name="iComponentIndex"></param>
        /// <param name="lpComponentBuf"></param>
        /// <returns></returns>
        [DllImport(MSI_DLL, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static UInt32 MsiEnumComponents(UInt32 iComponentIndex, System.Text.StringBuilder lpComponentBuf);

        /// <summary>
        /// MsiEnumClients 関数は、インストールされている特定のコンポーネントのクライアントを列挙します。 関数は、呼び出されるたびに 1 つの製品コードを取得します。
        /// 
        //UINT MsiEnumClientsA(
        //  [in] LPCSTR szComponent,
        //  [in] DWORD iProductIndex,
        //  [out] LPSTR lpProductBuf
        //);
        /// https://learn.microsoft.com/ja-jp/windows/win32/api/msi/nf-msi-msienumclientsa
        /// </summary>
         /// <param name="szComponentID"></param>
         /// <param name="iProductIndex"></param>
         /// <param name="lpComponentBuf"></param>
         /// <returns></returns>
        [DllImport(MSI_DLL, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static UInt32 MsiEnumClients(String szComponentID, UInt32 iProductIndex, System.Text.StringBuilder lpComponentBuf);


        /// <summary>
        /// MsiGetProductCode 関数は、アプリケーションのインストール済みまたはアドバタイズされたコンポーネントのコンポーネント コードを使用して、アプリケーションの製品コードを返します。初期化時に、アプリケーションは、どの製品コードでインストールまたはアドバタイズされたかを判別する必要があります。
        /// 
        ///  UINT MsiGetProductCode (
        ///    LPCTSTR szComponent,  // component ID
        ///    LPTSTR  lpProductBuf  // product code for the client product
        ///  );
        ///  http://msdn.microsoft.com/en-us/library/aa370129(VS.85).aspx
        /// </summary>
        /// <param name="szComponentID"></param>
        /// <param name="lpProductBuf"></param>
        /// <returns></returns>
        [DllImport(MSI_DLL, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static UInt32 MsiGetProductCode(String szComponentID, System.Text.StringBuilder lpProductBuf);

        /// <summary>
        /// MsiGetComponentPath 関数は、インストールされているコンポーネントへの完全パスを返します。コンポーネントのキー パスがレジストリ キーの場合、レジストリ キーが返されます。
        /// 
        /// INSTALLSTATE MsiGetComponentPath (
        ///   LPCTSTR szProduct,    // product code for the client product
        ///   LPCTSTR szComponent,  // component ID
        ///   LPTSTR  lpPathBuf,
        ///   DWORD*  pcchBuf
        /// );
        /// 
        /// http://msdn.microsoft.com/en-us/library/aa370112(VS.85).aspx
        /// </summary>
        /// <param name="szProduct"></param>
        /// <param name="szComponent"></param>
        /// <param name="lpPathBuf"></param>
        /// <param name="pcchBuf"></param>
        /// <returns></returns>
        [DllImport(MSI_DLL, CharSet = CharSet.Auto, SetLastError = true)]
        public extern static Int32 MsiGetComponentPath(String szProduct, String szComponent,
            System.Text.StringBuilder lpPathBuf, ref UInt32 pcchBuf);

        /// <summary>
        /// MsiEnumProducts 関数は、現在アドバタイズまたはインストールされているすべての製品を列挙します。ユーザーごとマシンごとのインストール コンテキストと提供情報の両方にインストールされている製品が列挙されます。
        /// 
        /// UINT MsiEnumProductsA(
        /// [in]
        /// DWORD iProductIndex,
        /// [out] LPSTR lpProductBuf
        /// );
        /// 
        /// https://learn.microsoft.com/ja-jp/windows/win32/api/msi/nf-msi-msienumproductsa
        /// </summary>
        /// <param name="iProductIndex"></param>
        /// <param name="lpProductBuf"></param>
        /// <returns></returns>
        [DllImport("msi.dll", SetLastError = true)]
        public extern static Int32 MsiEnumProducts(uint iProductIndex, StringBuilder lpProductBuf);

        /// <summary>
        /// MsiGetProductInfo 関数は、公開およびインストールされている製品の製品情報を返します。
        /// 
        /// UINT MsiGetProductInfoA(
        /// [in]
        /// LPCSTR szProduct,
        /// [in]      LPCSTR szAttribute,
        /// [out]     LPSTR lpValueBuf,
        /// [in, out] LPDWORD pcchValueBuf
        /// );
        /// 
        /// https://learn.microsoft.com/ja-jp/windows/win32/api/msi/nf-msi-msigetproductinfoa
        /// </summary>
        /// <param name="product"></param>
        /// <param name="property"></param>
        /// <param name="valueBuf"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        [DllImport("msi.dll", CharSet = CharSet.Unicode)]
        public extern static Int32 MsiGetProductInfo(string product, string property, [Out] StringBuilder valueBuf, ref Int32 len);
    }
}
