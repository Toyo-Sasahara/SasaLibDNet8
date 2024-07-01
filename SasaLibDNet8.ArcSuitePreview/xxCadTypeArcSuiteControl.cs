//using StageServerRemote;
//using System.Runtime.Versioning;
//using System.Windows.Forms;

//namespace SasaLib.ArcSuitePreview
//{
//    /// <summary>
//    /// user:CadType を設定
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class CadTypeArcSuiteControl
//    {
//        string StageServerHost;
//        string PipeName;

//        string ClientDomainName;
//        string ClientUserName;
//        string ClientUserPassword;
//        bool ClsLogon;

//        string ArcSuiteUserName;
//        string ArcSuiteUserPass;


//        public string CurrentCadTypeString;

//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        /// <param name="ClientDomainName"></param>
//        /// <param name="ClientUserName"></param>
//        /// <param name="ClientUserPassword"></param>
//        /// <param name="ClsLogon"></param>
//        /// <param name="StageServerHost"></param>
//        /// <param name="PipeName"></param>
//        /// <param name="ArcSuiteUserName"></param>
//        /// <param name="ArcSuiteUserPass"></param>
//        public CadTypeArcSuiteControl(string ClientDomainName, string ClientUserName, string ClientUserPassword, bool ClsLogon, string StageServerHost, string PipeName, string ArcSuiteUserName, string ArcSuiteUserPass)
//        {

//            this.ClientDomainName = ClientDomainName;
//            this.ClientUserName = ClientUserName;
//            this.ClientUserPassword = ClientUserPassword;
//            this.ClsLogon = ClsLogon;

//            this.StageServerHost = StageServerHost;
//            this.PipeName = PipeName;

//            this.ArcSuiteUserName = ArcSuiteUserName;
//            this.ArcSuiteUserPass = ArcSuiteUserPass;
//        }

//        /// <summary>
//        /// ① user:cadtypeに指定のRemoteClientCADtype.CadTypeを追加・削除
//        /// </summary>
//        /// <param name="arcSuiteZuban"></param>
//        /// <param name="SetMode"></param>
//        /// <param name="MsgText"></param>
//        /// <param name="NewSetCadType"></param>
//        /// <param name="nativeWindow"></param>
//        public bool SetUnsetCadTypeFlag(string arcSuiteZuban, ref string MsgText, RemoteClientCADtype.CadType NewSetCadType, NativeWindow nativeWindow, bool MsgBoxShow = true)
//        {
//            // RemoteClientCADtype クラスのオブジェクトを定義
//            RemoteClientCADtype rmcCadType = new RemoteClientCADtype(
//                ClientDomainName,
//                ClientUserName,
//                ClientUserPassword,
//                ClsLogon,
//                StageServerHost,
//                PipeName
//                );

//            // CurrentCadType を初期化
//            RemoteClientCADtype.CadType CurrentCadType = RemoteClientCADtype.CadType.NotSet;

//            // 現在のuser:cadtypeに指定のCADタイプが含まれているかによって切替
//            if (rmcCadType.ContainArcSuiteAttrCADType(arcSuiteZuban, NewSetCadType, ref CurrentCadType, ArcSuiteUserName, ArcSuiteUserPass) == false)
//            {
//                bool result = rmcCadType.SetUnSetArcSuiteAttrCADType(arcSuiteZuban, NewSetCadType, true, ref CurrentCadType, ArcSuiteUserName, ArcSuiteUserPass);
//                if (result)
//                {
//                    CurrentCadTypeString = NumberingSupport.ArcSuiteSupport.GetCadTypeString(CurrentCadType);

//                    MsgText = $"ArcSuiteの属性 「CADコード user:CadType」に{NewSetCadType}を追加しました。\n" +
//                            $"現在値{CurrentCadTypeString}";
//                    if (MsgBoxShow)
//                        MessageBox.Show(MsgText);
//                }
//                else
//                {
//                    MsgText = $"CadTypeは更新できません。図面{arcSuiteZuban} アークスイート側の図面の状態が取替中や使用禁止ではないでしょうか？";
//                    if (MsgBoxShow)
//                        MessageBox.Show(nativeWindow, MsgText);
//                    return false;
//                }
//            }
//            else
//            {
//                bool result = rmcCadType.SetUnSetArcSuiteAttrCADType(arcSuiteZuban, NewSetCadType, false, ref CurrentCadType, ArcSuiteUserName, ArcSuiteUserPass);
//                if (result)
//                {
//                    this.CurrentCadTypeString = NumberingSupport.ArcSuiteSupport.GetCadTypeString(CurrentCadType);

//                    MsgText = $"ArcSuiteの属性 「CADコード user:CadType」から{NewSetCadType}を除去しました。\n" +
//                        $"現在値{CurrentCadTypeString}";
//                    if (MsgBoxShow)
//                        MessageBox.Show(nativeWindow, MsgText);
//                    return true;
//                }
//                else
//                {
//                    MsgText = $"CadTypeは更新できません。図面{arcSuiteZuban} は CADTYPEの設定はされておりません。({NewSetCadType})";
//                    if (MsgBoxShow)
//                        MessageBox.Show(MsgText);
//                    return false;
//                }
//            }
//            return true;
//        }
//    }
//}
