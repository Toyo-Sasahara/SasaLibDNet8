using SasaLib;
using SasaLib.SysConfigurator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

[SupportedOSPlatform("windows")]
public class SysConfiguration
{
    public string COMMENT01 = @"【DATETIME】 この命令ファイルの適用日時";
    public DateTime DATETIME = DateTime.MinValue;
    public string COMMENT02 = @"【AES_iv_Base64】 この命令ファイルで暗号化された文字列のためのIV値";
    public string AES_iv_Base64 = "AES暗号複合に使う iv キーを Base64化したもの";

    public string COMMENT03 = @"【LogFileName】ログファイル名を指定 ";
    /// <summary>
    /// ログファイル名を指定
    /// </summary>
    public string LogFileName;

    public string COMMENT04 = @"【LogFolderName】ログ出力先を指定 ";
    /// <summary>
    /// ログ出力先を指定
    /// </summary>
    public string LogFolderName;



    public string COMMENT06 = @"【GetFileLists】 指定したファイルをサーバより取得します";
    /// <summary>
    /// 
    /// </summary>
    public List<GetFulFileName> GetFileLists = new List<GetFulFileName>();

    public string COMMENT07 = "【SetAttributeFiles】 指定したファイルの属性を設定";
    /// <summary>
    /// 
    /// </summary>
    public List<SetAttributeFile> SetAttributeFiles = new List<SetAttributeFile>();

    public string COMMENT08 = "【RemoveFiles】 指定したファイルを削除";
    /// <summary>
    /// 
    /// </summary>
    public List<RemoveFile> RemoveFiles = new List<RemoveFile>();


    public string COMMENT09 = "【SetRegistries】 レジストリを設定";
    /// <summary>
    /// 
    /// </summary>
    public List<SetRegistry> SetRegistries = new List<SetRegistry>();


    public string COMMENT10 = "【RemoveRegistries】 レジストリを削除";
    /// <summary>
    /// 
    /// </summary>
    public List<RemoveRegistry> RemoveRegistries = new List<RemoveRegistry>();

    public string COMMENT11 = "【ExecuteProcess】 指定したプロセスを実行";
    /// <summary>
    /// 
    /// </summary>
    public List<ExecuteProcess> ExecuteProcesses = new List<ExecuteProcess>();

    /// <summary>
    /// シリアライズのためにはコンストラクタは必要
    /// </summary>
    public SysConfiguration() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ConfigFullPath"></param>
    /// <param name="ControlData"></param>
    /// <param name="LogWrite"></param>
    /// <returns></returns>
    public bool ReadloadConfigData(string ConfigFullPath, out SysConfiguration ControlData, SasaLibDelegateWriteLine LogWrite)
    {

        if (LogWrite == null) LogWrite = DebugConsole.WriteLine;
        System.IO.StreamReader sr = null;
        try
        {
            // 既に追加指令書ファイルがある場合はあらかじめデシリアライズする
            if (FileFolder.FileExists(ConfigFullPath) == true)
            {
                // ｻｰﾊﾞｰからﾛｰｶﾙへ複写した追加指令書ファイル内容を復元する
                XmlSerializer serializer = new XmlSerializer(typeof(SysConfiguration));
                sr = new System.IO.StreamReader(ConfigFullPath, new System.Text.UTF8Encoding(false));
                ControlData = (SysConfiguration)serializer.Deserialize(sr);
                sr.Close();


                //LogWrite($"■コントロールファイル \"{System.IO.Path.GetFileName(ConfigFullPath)}\" をメモリに展開しました。");


                return true;
            }
            else
            {
                LogWrite($"※コントロールファイル \"{System.IO.Path.GetFileName(ConfigFullPath)}\" が見つかりません。");

                ControlData = null;
                return false;
            } // 指定ﾌｧｲﾙが存在しない場合
        }
        catch (Exception ex)
        {
            LogWrite($"※SysConfiguration.PreparationConfigData(..) 追加指令書ファイル{ConfigFullPath}の読み込みｴﾗｰ. 例外発生 {ex.Message} {ex.InnerException}");
            ControlData = null;

            if (sr != null)
                sr.Close();

            return false;
        }
    }

    public bool SaveConfig(string ConfigFullPath, SysConfiguration ControlData, SasaLibDelegateWriteLine LogWrite = null)
    {

        bool result = false;
        XmlSerializer serializer = new XmlSerializer(typeof(SysConfiguration));
        using (StreamWriter sw = new StreamWriter(ConfigFullPath, false, Encoding.UTF8))
        {
            try
            {
                serializer.Serialize(sw, ControlData);
                //if (LogWrite != null) LogWrite($"■InstructionsToInventorToyoAddinWork.SaveConfig(..) 追加指令を　ファイル名 {ConfigFullPath} として保存しました");
                result = true;
            }
            catch (IOException ioe)
            {
                if (LogWrite != null) LogWrite($"※InstructionsToInventorToyoAddinWork.SaveConfig(..) 追加指令の保存（ファイル名 {ConfigFullPath}）で例外発生{ioe.Message}");
                result = false;
            }
        }
        return result;
    }

    public void CreateSmapleConfig(string ConfigFolder, SasaLibDelegateWriteLine LogWrite = null)
    {

        string sampleConfigFile = System.IO.Path.Combine(ConfigFolder, "SysConfiguration.Sample.Conf.SMP");

        LogWrite($"■サンプルConfigurationファイルを作成 \"{sampleConfigFile}\"");
        SaveConfig(sampleConfigFile, this, LogWrite);

    }

    /// <summary>
    /// 以下のコメント文では書き方によって例外が発生する。（<!-- -->のネストなど）
    /// </summary>
    [XmlAnyElement("VersionComment")]
    public XmlComment VersionComment { get { return new XmlDocument().CreateComment(@"
    ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
    ■ 書式説明
　　　条件
　　　① <SysConfiguration>～</SysConfiguration> が1つで構成されていること。
　　　② SysConfiguration タグの中に必要なタグ

    ① DATETIME タグ
    例
    <DATETIME>2022-08-31T00:00:14</DATETIME>
    
    DATETIME ﾀｸﾞ は この追加設定ファイルが適用後 各PC の適用済み追加設定ﾌｧｲﾙ(*.Applied)のﾌｧｲﾙ名の一部として使用される。TOYOｱﾄﾞｲﾝはこのﾌｧｲﾙが存在していた場合は適応済みとしてスキップする
    
　　② AES_iv_Base64　タグ
　　例
　　<AES_iv_Base64>b9+YAGc2fTv/yTt5TdSBcg==</AES_iv_Base64>

　　AES_iv_Base64 ﾀｸﾞ は このファイル内で使用されたAES暗号を複合するためのiv値。


　　③ LogFileName  タグ
　　このコントロールファイルの記述を実行したときに記録ログとして保存するファイル名

　　④ LogFolderName タグ


    ■ ファイル取り込みブロック
    <GetFileLists> GetFileLists ﾀｸﾞは設定ﾌｧｲﾙ内で１つのみ
        <GetFulFileName> ﾌｧｲﾙ取込指定ﾌﾞﾛｯｸ開始
            <ServerSideFullFileName> ｻｰﾊﾞｰ側から受信するﾌﾙﾊﾟｽﾌｧｲﾙ名 </ServerSideFullFileName> 必須項目
            <LocalSideFullFileName> ｸﾗｲｱﾝﾄ側に再現するﾌﾙﾊﾟｽﾌｧｲﾙ名（環境変数対応） </LocalSideFullFileName>　必須項目
            <UnZip>true ならzipﾌｧｲﾙとして解凍する</UnZip>
            <UnZipLocalFolder> UnZipがtrueなら 解凍するフォルダ名（環境変数対応）</UnZipLocalFolder> UnZiP=trueなら必須項目 すでにﾛｰｶﾙｻｲﾄﾞに存在した場合は解凍しません
            <DeleteZipFileAfterUnzipped> trueなら解凍後zipﾌｧｲﾙは消去されます（テスト用。次にこの設定ﾌｧｲﾙが読み込まれた際にｻｰﾊﾞｰから再度読み込まれ解凍されます） </DeleteZipFileAfterUnzipped>
        </GetFulFileName> ﾌｧｲﾙ取込指定ﾌﾞﾛｯｸ終了
        
        // 例
        <GetFulFileName>  命令行の開始
            <ServerSideFullFileName>C:\ProgramData\TOYOCOMMON\Inventor\Searches0505.zip</ServerSideFullFileName>
            <LocalSideFullFileName>%APPDATA%\Autodesk\VaultCommon\Servers\Services_Security_1_7_2019\acvlt3\Vaults\MainVault\Searches0505.zip</LocalSideFullFileName>
            <UnZip>true</UnZip>
            <UnZipLocalFolder>%APPDATA%\Autodesk\VaultCommon\Servers\Services_Security_1_7_2019\acvlt3\Vaults\MainVault</UnZipLocalFolder>               
        </GetFulFileName>

        <GetFulFileName>
            以下同様
        </GetFulFileName>
    </GetFileLists>

    ■ ファイル削除ブロック
	<RemoveFiles> // RemoveFiles ﾀｸﾞは設定ﾌｧｲﾙ内で１つのみ
		<RemoveFile> 
			<LocalSideFullFileName>C:\TOYO Inventor\Template\standard.dwg</LocalSideFullFileName>
		</RemoveFile>
		<RemoveFile>
			<LocalSideFullFileName>C:\TOYO Inventor\Template</LocalSideFullFileName> // LocalSideFullFileName ﾀｸﾞの文字列が ディレクトリの場合 サブディレクトリも削除の対象とします
		</RemoveFile>
	</RemoveFiles>

    ■ レジストリ設定ブロック
    <SetRegistries> // SetRegistries ﾀｸﾞは設定ﾌｧｲﾙ内で１つのみ
        <SetRegistry>
            <KeyName>キー名</KeyName> // HKEY_CURRENT_USER\ではじまること（現在の仕様）
            <ValueName>値の名前</ValueName >
            <RegistryValueKind>値の種類</RegistryValueKind> // 現状 String , DWord,  Binary
            <Value>C:\Program Files\Autodesk\Vault Client 2022\Explorer\JobProcessor.exe</Value> // 文字列として扱われる <RegistryValueKind>Binary</RegistryValueKind>とするとByte配列として渡される(UTF-8)
        </SetRegistry> // <SetRegistry>～</SetRegistry>を1ブロックとして記述する。
        
        // 例
        <SetRegistry>  命令行の開始
            <KeyName>HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run</KeyName>
            <ValueName>JobProcessor</ValueName >
            <RegistryValueKind>String</RegistryValueKind>
            <Value>C:\Program Files\Autodesk\Vault Client 2022\Explorer\JobProcessor.exe</Value>
        </SetRegistry>
        
        <SetRegistry>
            <KeyName>HKEY_CURRENT_USER\SOFTWARE\Autodesk\AutoCAD\R23.1\ACAD-3005:411\Profiles\&lt;&lt;ACADMPP&gt;&gt;\General</KeyName>
            <ValueName>UseStartUpDialog</ValueName >
            <RegistryValueKind>DWord</RegistryValueKind>
            <Value>2</Value>
        </SetRegistry>

        <SetRegistry>
            以下同様
        </SetRegistry>
    </SetRegistries>

    ■ レジストリ削除ブロック
    <RemoveRegistries> // RemoveRegistries ﾀｸﾞは設定ﾌｧｲﾙ内で１つのみ
        <RemoveRegistry>  命令行の開始
            <OpenKeyName>HKEY_CURRENT_USER\Software\Autodesk\AutoCAD\R23.1\ACAD-3005:411\Profiles\&lt;&lt;ACADMPP&gt;&gt;\Editor Configuration</OpenKeyName>
            <ValueName>NAV2DVCUBEDISPLAYSETTING</ValueName> // 値を削除する場合
        </RemoveRegistry> <RemoveRegistry>～<RemoveRegistry>を1ブロックとして記述する。
        
        <RemoveRegistry>  命令行の開始
            <OpenKeyName>HKEY_CURRENT_USER\Software\SASAHASOFTWARE</OpenKeyName>
            <SubKeyName>TESTTOOL</SubKeyName> // サブキーを削除する場合
        </RemoveRegistry>

        <RemoveRegistry>
            以下同様
        </RemoveRegistry>
    </RemoveRegistries>

    ■ 外部コマンド実行ブロック
    <ExecuteProcesses> // ExecuteProcesses ﾀｸﾞは設定ﾌｧｲﾙ内で１つのみ
        <ExecuteProcess>  命令行の開始
            <FileName>c:\windows\system32\cmd.exe</FileName>
            <Arguments>/c Dir D:\</Arguments>
            <UserName>Administrator</UserName>
            <StdOut>true</StdOut>
            <Password>xxxxx</Password> // パスワードは <Password></Password> か<EncryptedPassword></EncryptedPassword> のどちらかのみに記述すること
            <EncryptedPassword>3VjfY/EbJZ78XZlfjGJkOiq6yDxxxxxxxxxx=</EncryptedPassword>
            <Domain>SS</Domain>
        </ExecuteProcess>
        
        <ExecuteProcess>
            以下同様
        </ExecuteProcess>
    </ExecuteProcesses>


    ■補足 よく使われるエスケープ文字
    < → &lt;
    > → &gt;
    & → &amp;
    "" →  &quot;
    ' →  &apos;
    ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
    "); } set { } }

}
