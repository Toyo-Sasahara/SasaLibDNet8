using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Xml.Serialization;


namespace SasaLibDNet8
{
    /// <summary>
    /// https://takamints.hatenablog.jp/entry/save-the-app-settings-to-editable-xml-from-csharp
    /// 
    /// </summary>
    public class XmlSettingFile
    {
        /// <summary>
        /// 設定ファイルを読み込む。指定したファイルがない場合は作成される。作成時には必要に応じてフォルダも作成される
        /// </summary>
        /// <param name="folderId">
        ///     スペシャルフォルダを指定する</param>
        /// <param name="initial">
        ///     ファイルがない場合の既定値を持つインスタンス</param>
        ///     ※ここで設定ファイルのテンプレートとなるクラス名を指定
        /// <param name="filename">
        ///     ファイル名。
        ///     省略時はクラス名を採用する</param>
        /// <returns></returns>
        public static XmlSettingFile Load(Environment.SpecialFolder folderId, XmlSettingFile initial, string filename = null)
        {
            //ファイル名を生成
            string pathname = CreatePathname(folderId, initial, filename);

            //設定ファイルがなければ既定の値で生成する
            if (!File.Exists(pathname))
            {
                Save(pathname, initial);
                AllowUsersReadAndWrite(pathname);
            }

            //設定ファイルを読み込む
            return Read(pathname, initial.GetType());
        }

        /// <summary>
        /// 設定ファイルを読み込む。指定したファイルがない場合は作成される。作成時には必要に応じてフォルダも作成される
        /// </summary>
        /// <param name="FolderPath">フォルダパス</param>
        /// <param name="initial">ファイルがない場合の既定値を持つインスタンス</param>
        /// <param name="Filename">ファイル名</param>
        /// <returns></returns>
        public static XmlSettingFile Load(string FolderPath, XmlSettingFile initial, string Filename)
        {
            //拡張子がXMLでない場合はXMLに変更する
            if (Path.GetExtension(Filename) == "")
            {
                Filename = Path.ChangeExtension(Filename, "xml");
            }

            // ディレクトリが存在しないなら作成
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            //ファイル名を生成
            string pathname = Path.Combine(FolderPath, Filename);

            //設定ファイルがなければ既定の値で生成する
            if (!File.Exists(pathname))
            {
                Save(pathname, initial);
                AllowUsersReadAndWrite(pathname);
            }

            //設定ファイルを読み込む
            return Read(pathname, initial.GetType());
        }
        /// <summary>
        /// 設定ファイル名。
        /// 実際に読み込んだもの。
        /// 書き戻す場合にも利用する。
        /// </summary>
        [XmlIgnore] public string Filename { get; private set; }

        /// <summary>
        /// 設定ファイルへ書き戻す。
        /// プログラムから設定値を変更した場合などに使用。
        /// </summary>
        public void Save()
        {
            //Console.WriteLine($"SasaLib.XmlSettingFile.Save({Filename})");
            Save(Filename, this);
        }

        #region StaticPrivate

        /// <summary>
        /// 与えられた情報からファイル名を生成する
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="initial"></param>
        /// <param name="filename">nullの場合ファイル名はクラス名が採用される</param>
        /// <returns></returns>
        static private string CreatePathname(
            Environment.SpecialFolder folderId,
            XmlSettingFile initial,
            string filename)
        {
            //ファイル名が指定されていないならクラス名
            if (filename == null)
            {
                filename = initial.GetType().Name;
            }

            //拡張子がXMLでない場合はXMLに変更する
            if (Path.GetExtension(filename) == "")
            {
                filename = Path.ChangeExtension(filename, "xml");
            }

            //ファイル名を決定
            string pathname = Path.Combine(CreateFolder(folderId), filename);

            return pathname;
        }

        /// <summary>
        /// スペシャルフォルダ以下のパスを生成
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        static private string CreateFolder(
            Environment.SpecialFolder folderId)
        {
            //アセンブリのファイルバージョン情報を利用してフォルダを決定
            var verinfo = FileVersionInfo.GetVersionInfo(
                Assembly.GetExecutingAssembly().Location);

            // 空白の項目はパスに含まれない。
            // 例えば CompanyName が空の場合、
            // @"C:\ProgramData\ProductName\1.0.0.0" などとなる
            string path = Path.Combine(new string[] {
                Environment.GetFolderPath(folderId),
                verinfo.CompanyName,
                verinfo.ProductName,
                verinfo.ProductVersion,
            });

            //フォルダがないなら生成する
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 設定を設定ファイルに保存
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="setting"></param>
        static private void Save(string filename, XmlSettingFile setting)
        {
            try
            {
                var writer = new StreamWriter(filename, false, new UTF8Encoding(false));
                var xml = new XmlSerializer(setting.GetType());
                xml.Serialize(writer, setting);
                writer.Close();
            }
            catch (IOException ioe)
            {
                Eventlog.Log.WriteEntry("SASALIB", EventLogEntryType.Error, 0, $"XmlSettingFile.C Save失敗 {ioe.Message}");
            }
        }

        /// <summary>
        /// 設定ファイルを読み込む
        /// </summary>
        /// <param name="filename">設定ファイル</param>
        /// <param name="type">設定データの型</param>
        /// <returns>type型のインスタンス。
        /// ファイルない場合はnullを返す。</returns>
        static private XmlSettingFile Read(string filename, Type type)
        {
            var reader = new StreamReader(filename, new UTF8Encoding(false));
            var xml = new XmlSerializer(type);
            try
            {
                var setting = xml.Deserialize(reader) as XmlSettingFile;
                reader.Close();
                if (setting != null)
                {
                    setting.Filename = filename;
                }
                return setting;
            }
            catch (Exception ex)
            {
                reader.Close();

                // *.err を事前削除
                FileFolder.RemoveFile(FileFolder.ChangeExtension(filename, "err"));
                // エラーファイルをの拡張子を変更。
                FileFolder.ChangeExtensionExcute(filename, "err");
                Eventlog.Log.WriteEntry("SASALIB", EventLogEntryType.Error, 0, $" XmlSettingFile Read(..) 例外発生\n基本設定ファイル{filename}を名前変更しました。次回起動時標準設定で再構築されます。\n{ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 指定ファイルにUsersグループのユーザーの読み書き権限を与える
        /// </summary>
        /// <param name="filename"></param>
        static private void AllowUsersReadAndWrite(string filename)
        {
            FileSystemAccessRule rule = new FileSystemAccessRule(
                new NTAccount("Users"),
                FileSystemRights.Write | FileSystemRights.Read,
                AccessControlType.Allow);
            FileSecurity security = File.GetAccessControl(filename);
            security.AddAccessRule(rule);
            File.SetAccessControl(filename, security);
        }

        #endregion

    }
}

/*　①　XMLの定義用に次のクラスを準備する
 *　
     /// <summary>
    /// XMLアプリケーション設定サンプル
    /// </summary>
    public class XMLconfigPreparation : SasaLib.XmlSettingFile
    {
        /// <summary>
        /// 既定の設定情報を生成。
        /// (デフォルトコンストラクタは必ず必要)
        /// </summary>
        public XMLconfigPreparation() { }

        //以下のメンバーはXMLに保存される

        public string ListenAddres { get; set; } = "127.0.0.1";
        public string OutputPrinter { get; set; } = "Brother MFC-J6770CDW Printer";
        public int FeatureOption { get; set; } = 0;
        public bool DebugOption { get; set; } = false;

        //保存したくないメンバーは以下のように宣言
        [System.Xml.Serialization.XmlIgnore]
        public string NotSaved;
    }

    ②　XMLの定義をインスタンス化する

    /// <summary>
    /// XML設定ファイルの定義を行う
    /// </summary>
    static public XMLconfigPreparation confSet = new XMLconfigPreparation();


    ③ XML設定を読み込むメソッドを準備する

    /// <summary>
    /// XML設定ファイルを読み込む
    /// </summary>
    static void ReadSeeting()
    {
        try
        {
            // 設定ファイル位置
            // C:\ProgramData\(CompanyName)\(ProductName)\(ProductVersion)\SampleSetting.xml
            // C:\ProgramData\ソケットサーバーconsole1\1.0.0.0
            // Environment.SpecialFolder共用体を参照
            //
            confSet = SasaLib.XmlSettingFile.Load(
                Environment.SpecialFolder.MyDocuments,
                new XMLconfigPreparation()) as XMLconfigPreparation;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:{0}", ex.Message);
        }
        finally
        {
            confSet.Save();
        }
    }

    ④ 初期化メソッドからXML設定を読み込むメソッドをコールする
    public static int Main(String[] args)
    {
        ReadSeeting();
    }

 */
