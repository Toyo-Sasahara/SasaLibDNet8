using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using SasaLib;

namespace SasaLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ShelLinkComObject : IDisposable
    {
        private readonly IShellLinkW? shell;
        private readonly IPersistFile? persist;
        private bool disposedValue;

        /// <summary>デフォルトコンストラクタ。</summary>
        public ShelLinkComObject()
        {
            this.shell = (IShellLinkW)new ShellLinkObject();
            this.persist = this.shell as IPersistFile;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
            // TODO: 大きなフィールドを null に設定します

            if (!disposedValue)
            {

                    if (this.persist != null)
                        Marshal.ReleaseComObject(this.persist);
                    if (this.shell != null)
                        Marshal.ReleaseComObject(this.shell);


                disposedValue = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkFilePath"></param>
        /// <param name="targetPath"></param>
        /// <param name="workingfolder"></param>
        /// <param name="description"></param>
        /// <param name="iconLocationFile"></param>
        /// <param name="iconLocationNum"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public string ReadShortcutFile(string linkFilePath, out string targetPath, out string workingfolder, out string description, out string iconLocationFile, out int iconLocationNum, SasaLibDelegateWriteLine? WriteLine = null)
        {
            iconLocationNum = -1;

            if (WriteLine == null) WriteLine = Console.WriteLine;

            try
            {
                if (System.IO.File.Exists(linkFilePath) == false)
                    throw new Exception($"指定したパス; \"{linkFilePath}\" が存在しないかアクセス不能");

            }
            catch (Exception ex)
            {
                targetPath = null;
                workingfolder = null;
                description = null; iconLocationFile = null;
                iconLocationNum = 0;

                WriteLine(ex.Message);
                return null;
            }



            try
            {

                IShellLinkW? shell = (IShellLinkW)new ShellLinkObject();
                IPersistFile? persist = shell as IPersistFile;

                persist?.Load(linkFilePath, 0x00000000);

                WIN32_FIND_DATAW data = new WIN32_FIND_DATAW();

                StringBuilder sb_srcPath = new StringBuilder(IShellLinkW.MAX_PATH, IShellLinkW.MAX_PATH);
                shell?.GetPath(sb_srcPath, sb_srcPath.Capacity, ref data, SLGP_FLAGS.UNCPRIORITY);
                targetPath = sb_srcPath.ToString();

                StringBuilder sb_workingPath = new StringBuilder(IShellLinkW.MAX_PATH, IShellLinkW.MAX_PATH);
                shell?.GetWorkingDirectory(sb_workingPath, IShellLinkW.MAX_PATH);
                workingfolder = sb_workingPath.ToString();

                StringBuilder sb_description = new StringBuilder(IShellLinkW.MAX_PATH, IShellLinkW.MAX_PATH);
                shell?.GetWorkingDirectory(sb_description, IShellLinkW.MAX_PATH);
                description = sb_description.ToString();

                StringBuilder sb_iconLocationFile = new StringBuilder(IShellLinkW.MAX_PATH, IShellLinkW.MAX_PATH);
                shell?.GetIconLocation(sb_iconLocationFile, IShellLinkW.MAX_PATH, out iconLocationNum);
                iconLocationFile = sb_iconLocationFile.ToString();

                return targetPath;
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"FileFolder.ReadShortcutFile(..)例外 {ex.Message} {ex.InnerException}");
                targetPath = null;
                workingfolder = null;
                description = null; iconLocationFile = null;
                iconLocationNum = -1;

                return null;
            }

        }

        /// <summary>
        /// ショートカット作成
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="targetPath"></param>
        /// <param name="workingFolder"></param>
        /// <param name="description"></param>
        /// <param name="iconLocationFile"></param>
        /// <param name="iconLocationNum"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public  bool CreateShortcutFile(string fullPath, string targetPath, string? workingFolder = null, string description = "新しいｼｮｰﾄｶｯﾄ", string iconLocationFile = "notepad.exe", int iconLocationNum = 0, SasaLibDelegateWriteLine? WriteLine = null)
        {
            if (WriteLine == null) WriteLine = Console.WriteLine;


            try
            {
                if (System.IO.File.Exists(targetPath) == false)
                    throw new Exception($"ターゲットパス; \"{targetPath}\" が存在しないかアクセス不能");

            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
                return false;
            }

            try
            {

                IShellLinkW link = (IShellLinkW)new ShellLink();

                // setup shortcut information
                link.SetDescription(description);
                link.SetWorkingDirectory(workingFolder);
                link.SetIconLocation(iconLocationFile, iconLocationNum);
                link.SetPath(targetPath);

                // save it
                IPersistFile file = (IPersistFile)link;
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));
                file.Save(fullPath, true);
                WriteLine($"SasaLib.FileFolder.CreateShortCut(..) ショートカットファイル \"{fullPath}\" を作成または更新しました");
                return true;
            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"FileFolder.CreateShortCut(..)例外 ショートカットファイル \"{fullPath}\"（ターゲット:\"{targetPath}\"） の作成に失敗しました {ex.Message} {ex.InnerException}");

                return false;
            }
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public struct FILETIME
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 dwLowDateTime;

        /// <summary>
        /// 
        /// </summary>
        public UInt32 dwHighDateTime;
    }

    /// <summary>
    /// 
    /// </summary>
    public unsafe struct WIN32_FIND_DATAW
    {
        /// <summary>
        /// 
        /// </summary>
        public UInt32 dwFileAttributes;
        /// <summary>
        /// 
        /// </summary>
        public FILETIME ftCreationTime;
        /// <summary>
        /// 
        /// </summary>
        public FILETIME ftLastAccessTime;
        /// <summary>
        /// 
        /// </summary>
        public FILETIME ftLastWriteTime;
        /// <summary>
        /// 
        /// </summary>
        public UInt32 nFileSizeHigh;
        /// <summary>
        /// 
        /// </summary>
        public UInt32 nFileSizeLow;
        /// <summary>
        /// 
        /// </summary>
        public UInt32 dwReserved0;
        /// <summary>
        /// 
        /// </summary>
        public UInt32 dwReserved1;
        /// <summary>
        /// 
        /// </summary>
        public fixed Char cFileName[256];
        /// <summary>
        /// 
        /// </summary>
        public fixed Char cAlternateFileName[14];
    }

    /// <summary>取得するパス情報のタイプを指定するフラグを表します。</summary>
    [CLSCompliant(false), Flags]
    internal enum SLGP_FLAGS : uint
    {
        SHORTPATH = 1,
        UNCPRIORITY = 2,
        RAWPATH = 4,
    }

    //This could always be better adjusted to take more input instead of just being set.
    //Then outside of your class but in your namespace
    [ComImport]
    [Guid("00021401-0000-0000-C000-000000000046")]
    internal class ShellLink
    {
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    internal interface IShellLinkW
    {
        internal const int MAX_PATH = 260;

        /// <summary>
        /// シェルリンクオブジェクトのパスとファイル名を取得します。
        /// </summary>
        /// <param name="pszFile"></param>
        /// <param name="cch"></param>
        /// <param name="pfd"></param>
        /// <param name="fFlags"></param>
        //HRESULT GetPath([out, size_is(cch)] LPWSTR pszFile, [in] int cch, [in, out, ptr] WIN32_FIND_DATAW *pfd, [in] DWORD fFlags);
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cch, ref WIN32_FIND_DATAW pfd, SLGP_FLAGS fFlags);

        /// <summary>
        /// シェルリンクのリストを取得します。
        /// </summary>
        /// <param name="ppidl"></param>
        //HRESULT GetIDList([out] LPITEMIDLIST * ppidl);
        void GetIDList(out IntPtr ppidl);

        /// <summary>
        /// シェルリンクのリストを設定します。
        /// </summary>
        /// <param name="pidl"></param>
        //HRESULT SetIDList([in] LPCITEMIDLIST pidl);
        void SetIDList(IntPtr pidl);

        /// <summary>
        /// シェルリンクの説明文字列を取得します。
        /// </summary>
        /// <param name="pszName"></param>
        /// <param name="cchMaxName"></param>
        //HRESULT GetDescription([out, size_is(cch)] LPWSTR pszName, int cch);
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);

        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);

        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);

        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

        void GetHotkey(out short pwHotkey);

        void SetHotkey(short wHotkey);

        void GetShowCmd(out int piShowCmd);

        void SetShowCmd(int iShowCmd);

        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);

        /// <summary>
        /// シェルリンクオブジェクトを検索してシェルリンクを解決します。
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="fFlags"></param>
        void Resolve(IntPtr hwnd, int fFlags);

        /// <summary>
        /// シェルリンクパスとファイル名を設定します。
        /// </summary>
        /// <param name="pszFile"></param>
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);

    }

    /// <summary>IShellLinkWの実装クラスを表します。</summary>
    [CLSCompliant(false), ComImport, ClassInterface(ClassInterfaceType.None), Guid("00021401-0000-0000-C000-000000000046")]
    internal class ShellLinkObject
    {
    }

}
