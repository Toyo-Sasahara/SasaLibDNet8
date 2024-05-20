using System;
using System.IO;

namespace SasaLib
{
    /// <summary>
    /// 一時ファイルm名を取得する取得。使用後削除される機能付き
    /// 使用方法
    /// 
    ///    using (var tempFile = new ManagedTemporaryFile(C:\Windows\System32\Temp))
    ///     {
    ///        File.WriteAllText(tempFile.FullName,"hello");
    ///     ....
    ///     }
    ///     https://shuhelohelo.hatenablog.com/entry/2020/10/22/210835
    /// </summary>
    public class ManagedTemporaryFile : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private string fullfileName;

        /// <summary>
        /// 
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// テンポラリファイル名。Path.Combine(folder, Path.GetRandomFileName());
        /// </summary>
        public string FullTempFileName
        {
            get => fullfileName;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="folder">テンポラリファイルを作成するフォルダ</param>
        /// <param name="identifier"></param>
        public ManagedTemporaryFile(string folder, string identifier = "")
        {
            fullfileName = Path.Combine(folder, identifier + Path.GetRandomFileName());
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            //GC前にプログラム的にリソースを破棄するので
            //管理,非管理リソース両方が破棄されるようにする
            Dispose(true);
            GC.SuppressFinalize(this);//破棄処理は完了しているのでGC不要の合図
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            if (disposing)
            {
                //管理リソースの破棄処理
            }

            //非管理リソースの破棄処理
            try
            {
                File.Delete(this.fullfileName);
            }
            catch
            {
                throw;
            }

            disposedValue = true;
        }

        /// <summary>
        /// 
        /// </summary>
        ~ManagedTemporaryFile()
        {
            //GC時に実行されるデストラクタでは非管理リソースの削除のみ
            Dispose(false);
        }
    }
}
