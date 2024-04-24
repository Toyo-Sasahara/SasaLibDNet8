using SasaLibDummy;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SasaLib.NumberingSupport
{
    /// <summary>
    /// 
    /// </summary>
    public class NumberingSuppportConfigs
    {
        /// <summary>
        /// 図面種類判別用設定XMLﾌｧｲﾙ
        /// (ｻｰﾊﾞｰ側設定ﾌｧｲﾙ取得先ﾌﾙﾊﾟｽ)
        /// </summary>
        public string SourceNumberTypeConfigXMLfullpath { get; private set; } = @"C:\ProgramData\TOYOCOMMON\NumberTypeConfig.XML";

        /// <summary>
        ///  図面種類判別用設定XMLﾌｧｲﾙ
        /// </summary>
        public string NumberTypeConfigXMLfullpath { get; private set; } = @"C:\Users\Public\Documents\TOYOCOMMON\NumberTypeConfig.XML";

        /// <summary>
        /// 図番ﾊﾟﾀｰﾝとArcSuite登録図面番号対応表ﾌｧｲﾙ
        /// (ｻｰﾊﾞｰ側設定ﾌｧｲﾙ取得先ﾌﾙﾊﾟｽ)
        /// </summary>
        public string SourceConversionFormulaNumberConfigfullpath { get; private set; } = @"C:\ProgramData\TOYOCOMMON\ConversionFormulaNumberConfig.XML";

        /// <summary>
        /// 図番ﾊﾟﾀｰﾝとArcSuite登録図面番号対応表ﾌｧｲﾙ
        ///  (ﾛｰｶﾙ側ﾌﾙﾊﾟｽ名)
        /// </summary>
        public string ConversionFormulaNumberConfigfullpath { get; private set; } = @"C:\Users\Public\Documents\TOYOCOMMON\ConversionFormulaNumberConfig.XML";

        /// <summary>
        /// 
        /// (ｻｰﾊﾞｰ側設定ﾌｧｲﾙ取得先ﾌﾙﾊﾟｽ)
        /// </summary>
        public string SourceStageServerDatabaseConfigXMLfullpath { get; private set; } = @"C:\ProgramData\TOYOCOMMON\StageServerDatabaseConfig.XML";

        /// <summary>
        /// 共通属性名とアークスイート属性名を紐づける XMLﾌｧｲﾙ
        /// (ﾛｰｶﾙ側ﾌﾙﾊﾟｽ名)
        /// </summary>
        public string StageServerDatabaseConfigXMLfullpath { get; private set; } = @"C:\Users\Public\Documents\TOYOCOMMON\StageServerDatabaseConfig.XML";

        /// <summary>
        /// 材質コード・材質名対応 XMLﾌｧｲﾙ
        /// (ｻｰﾊﾞｰ側設定ﾌｧｲﾙ取得先ﾌﾙﾊﾟｽ)
        /// </summary>
        public string SourceMaterialCodeConfigXMLfullpath { get; private set; } = @"C:\ProgramData\TOYOCOMMON\MaterialCodeConfig.XML";

        /// <summary>
        /// 材質コード・材質名対応 XMLﾌｧｲﾙ
        /// (ﾛｰｶﾙ側ﾌﾙﾊﾟｽ名)
        /// </summary>
        public string MaterialCodeConfigXMLfullpath { get; private set; } = @"C:\Users\Public\Documents\TOYOCOMMON\MaterialCodeConfig.XML";

        /// <summary>
        /// 購入先コード・購入先名対応 XMLﾌｧｲﾙ
        /// (ｻｰﾊﾞｰ側設定ﾌｧｲﾙ取得先ﾌﾙﾊﾟｽ)
        /// </summary>
        public string SourcePurchasingManufacturerCodeConfigXMLfullpath { get; private set; } = @"C:\ProgramData\TOYOCOMMON\PurchasingManufacturerCodeConfig.XML";

        /// <summary>
        /// 購入先コード・購入先名対応 XMLﾌｧｲﾙ
        /// (ﾛｰｶﾙ側ﾌﾙﾊﾟｽ名)
        /// </summary>
        public string PurchasingManufacturerCodeConfigXMLfullpath { get; private set; } = @"C:\Users\Public\Documents\TOYOCOMMON\PurchasingManufacturerCodeConfig.XML";

        /// <summary>
        /// 注意ワード情報をダウンロード、デシリアライズ
        /// (ｻｰﾊﾞｰ側設定ﾌｧｲﾙ取得先ﾌﾙﾊﾟｽ)
        /// </summary>
        public string SourcePhrasesToBeAwareXMLfullpath { get; private set; } = @"C:\Users\Public\Documents\TOYOCOMMON\PhrasesToBeAwareConfig.XML";

        string ClientDomainName;
        string ClientUserName;
        string ClientPassword;
        bool ClsLogon;
        string StageServerHost;
        string PipeNameDC;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ClientDomainName"></param>
        /// <param name="ClientUserName"></param>
        /// <param name="ClientPassword"></param>
        /// <param name="ClsLogon"></param>
        /// <param name="StageServerHost"></param>
        /// <param name="PipeNameDC"></param>
        /// <param name="WriteLine"></param>
        public NumberingSuppportConfigs(string ClientDomainName, string ClientUserName, string ClientPassword, bool ClsLogon, string StageServerHost, string PipeNameDC,
                                                                                                SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = Console.WriteLine;

            this.ClientDomainName = ClientDomainName;
            this.ClientUserName = ClientUserName;
            this.ClientPassword = ClientPassword;
            this.ClsLogon = ClsLogon;
            this.StageServerHost = StageServerHost;
            this.PipeNameDC = PipeNameDC;

        }

        /// <summary>
        /// ■表題欄検証データセット（NumberTypeConfig.XML , ConversionFormulaNumberConfig.XML , StageServerDatabaseConfig.XML , MaterialCodeConfig.XML） を準備
        /// </summary>
        /// <param name="WriteLine">ログメッセージのデリゲート</param>
        /// <param name="UseLocalConfigFileOnly">trueの場合はサーバーから読みこみません</param>
        /// <returns></returns>
        public bool ExecuteDownloadAndDeserialize(SasaLibDelegateWriteLine WriteLine = null, SasaLibDelegateWriteLine DebugWriteLine = null, bool UseLocalConfigFileOnly = false)
        {
            if (WriteLine == null) WriteLine = Console.WriteLine;
            if (DebugWriteLine == null) DebugWriteLine = Console.WriteLine;
            bool result = true;

            if (UseLocalConfigFileOnly)
                DebugWriteLine($"□サーバーから図番検証サポートファイル群を読み出さずローカルファイルのみ使用します");

            bool downloadNumberTypeConfiSucess; /// ①NumberTypeConfig.XML をサーバーから複製結果
            bool preparationNumberTypeConfiSucess; /// ①NumberTypeConfig.XML のデシリアライズ結果

            bool downloadSuffixZeroPadVariantNumberTypeConfigSucess;　/// ②SuffixZeroPadVariantNumberTypeConfig.XML をサーバーから複製結果
            bool preparationSuffixZeroPadVariantNumberTypeConfigSucess;　/// ②SuffixZeroPadVariantNumberTypeConfig.XML のデシリアライズ結果

            bool downloadStageServerDatabaseConfigSucess;　/// ③StageServerDatabaseConfig.XML をサーバーから複製結果
            bool preparationStageServerDatabaseConfigSucess;　/// ③StageServerDatabaseConfig.XML のデシリアライズ結果

            bool downloadMaterialCodeConfigSucess; /// ④MaterialCodeConfig.XML をサーバーから複製結果
            bool preparationMaterialCodeConfigSucess; /// ④MaterialCodeConfig.XML のデシリアライズ結果

            /// ①NumberTypeConfig.XML をサーバーから複製します
            if (UseLocalConfigFileOnly == false) { 
                downloadNumberTypeConfiSucess = LoadConfigFileFromStageServer(SourceNumberTypeConfigXMLfullpath, NumberTypeConfigXMLfullpath, DebugWriteLine);
                if (downloadNumberTypeConfiSucess == false)
                    WriteLine($"※ExecuteDownloadAndDeserialize(..) ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourceNumberTypeConfigXMLfullpath}\" を ﾛｰｶﾙPC \"{NumberTypeConfigXMLfullpath}\" へのダウンロードに失敗しました。");
                //else
                //    DebugWriteLine($"■ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourceNumberTypeConfigXMLfullpath}\" を ﾛｰｶﾙPC \"{NumberTypeConfigXMLfullpath}\" へダウンロードしました。");
            }
            /// ①NumberTypeConfig.XML をメモリへﾃﾞシリアライズを行う
            preparationNumberTypeConfiSucess = NumberTypeConfigWork.PreparationConfigData(NumberTypeConfigXMLfullpath, false, DebugWriteLine);
            if (preparationNumberTypeConfiSucess == false)
            {
                WriteLine($"※ ﾛｰｶﾙPC \"{NumberTypeConfigXMLfullpath}\" のデシリアライズに失敗しました。開発初期値データを使用します");
                preparationNumberTypeConfiSucess = NumberTypeConfigWork.PreparationConfigData(NumberTypeConfigXMLfullpath, true, DebugWriteLine);
                result = false;
            }
            //else
            //{
            //    DebugWriteLine($"■\"{NumberTypeConfigXMLfullpath}\" のデシリアライズに成功しました。");
            //}

            /// ②SuffixZeroPadVariantNumberTypeConfig.XML をサーバーから複製します
            if (UseLocalConfigFileOnly == false) { 
                downloadSuffixZeroPadVariantNumberTypeConfigSucess = LoadConfigFileFromStageServer(SourceConversionFormulaNumberConfigfullpath, ConversionFormulaNumberConfigfullpath, DebugWriteLine);
                if (downloadSuffixZeroPadVariantNumberTypeConfigSucess == false)
                    WriteLine($"※ExecuteDownloadAndDeserialize(..) ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourceConversionFormulaNumberConfigfullpath}\" を ﾛｰｶﾙPC \"{ConversionFormulaNumberConfigfullpath}\" へのダウンロードに失敗しました。");
                //else
                //    DebugWriteLine($"■ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourceConversionFormulaNumberConfigfullpath}\" を ﾛｰｶﾙPC \"{ConversionFormulaNumberConfigfullpath}\" へダウンロードしました。");
            }

            /// ②SuffixZeroPadVariantNumberTypeConfig.XMLL をメモリへﾃﾞシリアライズを行う 指定ﾊﾞｰｼﾞｮﾝ未満の場合は新規作成される
            preparationSuffixZeroPadVariantNumberTypeConfigSucess = ConversionFormulaNumberConfigWork.PreparationConfigData(ConversionFormulaNumberConfigfullpath, false, WriteLine);
            if (preparationSuffixZeroPadVariantNumberTypeConfigSucess == false)
            {
                WriteLine($"※ﾛｰｶﾙPC \"{ConversionFormulaNumberConfigfullpath}\" のデシリアライズに失敗しました。開発初期値データを使用します");
                preparationSuffixZeroPadVariantNumberTypeConfigSucess = ConversionFormulaNumberConfigWork.PreparationConfigData(ConversionFormulaNumberConfigfullpath, true, WriteLine);
                result = false;
            }
            //else
            //{
            //    DebugWriteLine($"■\"{ConversionFormulaNumberConfigfullpath}\" のデシリアライズに成功しました。");
            //}


            /// ③StageServerDatabaseConfig.XML をサーバーから複製します
            if (UseLocalConfigFileOnly == false) { 
                downloadStageServerDatabaseConfigSucess = LoadConfigFileFromStageServer(SourceStageServerDatabaseConfigXMLfullpath, StageServerDatabaseConfigXMLfullpath, DebugWriteLine);
                if (downloadStageServerDatabaseConfigSucess == false)
                    WriteLine($"※ExecuteDownloadAndDeserialize(..) ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourceStageServerDatabaseConfigXMLfullpath}\" を ﾛｰｶﾙPC \"{StageServerDatabaseConfigXMLfullpath}\" へのダウンロードに失敗しました。");
                //else
                //    DebugWriteLine($"■ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourceStageServerDatabaseConfigXMLfullpath}\" を ﾛｰｶﾙPC \"{StageServerDatabaseConfigXMLfullpath}\"へダウンロードしました。");
            }
            /// ③StageServerDatabaseConfig.XML をメモリへﾃﾞシリアライズを行う
            preparationStageServerDatabaseConfigSucess = StageServerDatabaseConfigWork.PreparationConfigData(StageServerDatabaseConfigXMLfullpath, false, WriteLine);
            if (preparationStageServerDatabaseConfigSucess == false)
            {
                WriteLine($"※ﾛｰｶﾙPC \"{StageServerDatabaseConfigXMLfullpath}\" のデシリアライズに失敗しました。開発初期値データを使用します");
                preparationStageServerDatabaseConfigSucess = StageServerDatabaseConfigWork.PreparationConfigData(StageServerDatabaseConfigXMLfullpath, true, WriteLine);
                result = false;
            }
            //else
            //{
            //    DebugWriteLine($"■\"{StageServerDatabaseConfigXMLfullpath}\" のデシリアライズに成功しました。");
            //}


            /// ④MaterialCodeConfig.XML をサーバーから複製します
            if (UseLocalConfigFileOnly == false) { 
                downloadMaterialCodeConfigSucess = LoadConfigFileFromStageServer(SourceMaterialCodeConfigXMLfullpath, MaterialCodeConfigXMLfullpath, DebugWriteLine);
                if (downloadMaterialCodeConfigSucess == false)
                    WriteLine($"※ExecuteDownloadAndDeserialize(..) ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourceMaterialCodeConfigXMLfullpath}\" を ﾛｰｶﾙPC \"{MaterialCodeConfigXMLfullpath}\" へのダウンロードに失敗しました。");
                //else
                //    DebugWriteLine($"■ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourceMaterialCodeConfigXMLfullpath}\" を ﾛｰｶﾙPC \"{MaterialCodeConfigXMLfullpath}\" へダウンロードしました。");
            }
            /// ④MaterialCodeConfig.XMLL をメモリへﾃﾞシリアライズを行う 指定ﾊﾞｰｼﾞｮﾝ未満の場合は新規作成される
            preparationMaterialCodeConfigSucess = MaterialCodeConfigWork.PreparationConfigData(MaterialCodeConfigXMLfullpath, false, 1.5d, WriteLine);
            if (preparationMaterialCodeConfigSucess == false)
            {
                WriteLine($"※ﾛｰｶﾙPC \"{MaterialCodeConfigXMLfullpath}\" のデシリアライズに失敗しました。開発初期値データを使用します");
                preparationMaterialCodeConfigSucess = MaterialCodeConfigWork.PreparationConfigData(MaterialCodeConfigXMLfullpath, true, 1.5d, WriteLine);
                result = false;
            }
            //else
            //{
            //    DebugWriteLine($"■\"{MaterialCodeConfigXMLfullpath}\" のデシリアライズに成功しました。");
            //}

            return result;
        }

        /// <summary>
        /// ■購入品メーカーコード検証データ PurchasingManufacturerCodeConfig.XML をサーバーからダウンロード＆メモリへ展開
        /// </summary>
        /// <param name="DebugWriteLine"></param>
        /// <param name="NumberingSuppportConfigsFileLocalOnly"></param>
        /// <returns></returns>
        public bool ExecuteDownloadAndDeserialize_PurchasingManufacturerCodeConfig(SasaLibDelegateWriteLine WriteLine = null, SasaLibDelegateWriteLine DebugWriteLine = null, bool UseLocalConfigFileOnly = false)
        {
            if (DebugWriteLine == null) DebugWriteLine = DebugConsole.WriteLine;
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            bool result = true;

            if (UseLocalConfigFileOnly)
                DebugWriteLine($"□サーバーからPurchasingManufacturerCodeConfig.XMLを読み出さずローカルファイルのみ使用します");
            else
                DebugWriteLine($"■サーバーからPurchasingManufacturerCodeConfig.XMLを読み出しﾛｰｶﾙﾌｧｲﾙを上書きします。");


            bool downloadSucess; /// ⑤PurchasingManufacturerCodeConfig.XML をサーバーから複製結果
            bool preparatonSucess; /// ⑤PurchasingManufacturerCodeConfig.XML のデシリアライズ結果

            /// ⑤PurchasingManufacturerCodeConfig.XML をサーバーから複製します
            if (UseLocalConfigFileOnly == false)
            {
                downloadSucess = LoadConfigFileFromStageServer(SourcePurchasingManufacturerCodeConfigXMLfullpath, PurchasingManufacturerCodeConfigXMLfullpath, DebugWriteLine);
                if (downloadSucess == false)
                    WriteLine($"※ExecuteDownloadAndDeserialize_PurchasingManufacturerCodeConfig(..) \"{SourcePurchasingManufacturerCodeConfigXMLfullpath}\" のダウンロードに失敗しました。");
                else
                    DebugWriteLine($"■ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourcePurchasingManufacturerCodeConfigXMLfullpath}\" を ﾛｰｶﾙPC \"{PurchasingManufacturerCodeConfigXMLfullpath}\"へダウンロードしました。");
            }

            /// ⑤PurchasingManufacturerCodeConfig.XML をメモリへﾃﾞシリアライズを行う
            preparatonSucess = PurchasingManufacturerCodeConfigWork.PreparationConfigData(PurchasingManufacturerCodeConfigXMLfullpath, false, 1.5d, DebugWriteLine);

            if (preparatonSucess == false)
            {
                WriteLine($"※{PurchasingManufacturerCodeConfigXMLfullpath} のデシリアライズに失敗しました。開発初期値データを使用します");
                preparatonSucess = PurchasingManufacturerCodeConfigWork.PreparationConfigData(PurchasingManufacturerCodeConfigXMLfullpath, true, 1.5d, DebugWriteLine);
                result = false;
            }
            else
            {
                DebugWriteLine($"■{PurchasingManufacturerCodeConfigXMLfullpath} のデシリアライズに成功しました。");
            }

            return result;
        }

        /// <summary>
        /// ■PhrasesToBeAwareConfig.XML をサーバーからダウンロード＆メモリへ展開
        /// </summary>
        /// <param name="WriteLine"></param>
        /// <param name="DebugWriteLine"></param>
        /// <param name="UseLocalConfigFileOnly"></param>
        /// <returns></returns>
        public bool ExecuteDownloadAndDeserialize_PhrasesToBeAwareConfig(SasaLibDelegateWriteLine WriteLine = null, SasaLibDelegateWriteLine DebugWriteLine = null, bool UseLocalConfigFileOnly = false)
        {
            if (DebugWriteLine == null) DebugWriteLine = DebugConsole.WriteLine;
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            bool result = true;

            if (UseLocalConfigFileOnly)
                DebugWriteLine($"□ExecuteDownloadAndDeserialize_PhrasesToBeAwareConfig(..) サーバーから PhrasesToBeAwareConfig.XMLを読み出さずローカルファイルのみ使用します");
            else
                DebugWriteLine($"■ExecuteDownloadAndDeserialize_PhrasesToBeAwareConfig(..) サーバーから PhrasesToBeAwareConfig.XMLを読み出しﾛｰｶﾙﾌｧｲﾙを上書きします。");


            bool downloadSucess; /// ⑤PhrasesToBeAware.XML をサーバーから複製結果
            bool preparatonSucess; /// ⑤PhrasesToBeAware.XML のデシリアライズ結果

            /// ⑤PhrasesToBeAwareConfig.XML をサーバーから複製します
            if (UseLocalConfigFileOnly == false)
            {
                downloadSucess = LoadConfigFileFromStageServer(SourcePhrasesToBeAwareXMLfullpath, SourcePhrasesToBeAwareXMLfullpath, DebugWriteLine);
                if (downloadSucess == false)
                    WriteLine($"※ExecuteDownloadAndDeserialize_PhrasesToBeAwareConfig(..) \"{SourcePhrasesToBeAwareXMLfullpath}\" のダウンロードに失敗しました。");
                else
                    WriteLine($"■ExecuteDownloadAndDeserialize_PhrasesToBeAwareConfig(..) ｻｰﾊﾞｰ側ﾌｧｲﾙ \"{SourcePhrasesToBeAwareXMLfullpath}\" を ﾛｰｶﾙPC \"{SourcePhrasesToBeAwareXMLfullpath}\"へダウンロードしました。");
            }

            /// ⑤PhrasesToBeAwareConfig.XML をメモリへﾃﾞシリアライズを行う
            preparatonSucess = PhrasesToBeAwareConfigWork.PreparationConfigData(SourcePhrasesToBeAwareXMLfullpath, false,  DebugWriteLine);

            if (preparatonSucess == false)
            {
                WriteLine($"※{SourcePhrasesToBeAwareXMLfullpath} のデシリアライズに失敗しました。開発初期値データを使用します");
                preparatonSucess = PhrasesToBeAwareConfigWork.PreparationConfigData(SourcePhrasesToBeAwareXMLfullpath, true,  DebugWriteLine);
                result = false;
            }
            else
            {
                DebugWriteLine($"■ExecuteDownloadAndDeserialize_PhrasesToBeAwareConfig(..) \"{SourcePhrasesToBeAwareXMLfullpath}\" のデシリアライズに成功しました。");
            }

            return result;
        }

        /// <summary>
        /// サーバーからファイルを受信
        /// </summary>
        /// <param name="SouceFile"></param>
        /// <param name="DistnationFile"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        bool LoadConfigFileFromStageServer(string SouceFile, string DistnationFile, SasaLibDelegateWriteLine WriteLine = null)
        {

            if (WriteLine == null) WriteLine = Console.WriteLine;

            RemoteClientDRAWCAPTURE remoteClientDC = new RemoteClientDRAWCAPTURE(
                    this.ClientDomainName,
                    this.ClientUserName,
                    this.ClientPassword,
                    this.ClsLogon,
                    this.StageServerHost,
                    this.PipeNameDC);

            //var ans = remoteClientDC.GetTextFileFromPIPE(SouceFile, DistnationFile, WriteLine);
            string resultMsg;
            var ans = remoteClientDC.FileRecv(SouceFile, DistnationFile, out resultMsg, DebugConsole.WriteLine);

            if (ans)
            {
                return ans;
            }
            else
            {
                WriteLine($"※LoadConfigFileFromStageServer(..) ｽﾃｰｼﾞｻｰﾊﾞｰ {StageServerHost}から \"{SouceFile}\" の複製に失敗しました {resultMsg}");
                return ans;
            }
        }

        internal void WrilteLine2(string value)
        {
            DebugConsole.Write(value);
           
        }

    }
}
