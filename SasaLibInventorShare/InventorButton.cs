using Inventor;
using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SasaLib.InventorAPI
{

    /// <summary>
    ///コマンドボタンの基本クラス
    /// </summary>
    public abstract class InventorButton
	{
		#region データメンバー

		//プライベートデータメンバー：
		private Inventor.ButtonDefinitionSink_OnExecuteEventHandler ButtonDefinition_OnExecuteEventDelegate;

        #endregion

        #region プロパティ

        /// <summary>
        /// Buttonクラスで使う namespcae Inventor、Applicationクラスのオブジェクト
        /// </summary>
        public static Inventor.Application InventorApp { set; get; }

        public Inventor.ButtonDefinition ButtonDefinition { get; }

		#endregion

		#region メソッド
		/// <summary>
		/// Buttonコンストラクタ
		/// </summary>
		/// <param name="displayName">ユーザに表示されるテキスト</param>
		/// <param name="internalName">内部名。Inventor 内の他のすべてのコントロール定義とコマンド バーに対して一意である必要</param>
		/// <param name="classification"> ControlDefinition の分類を設定。CommandTypesEnum </param>
		/// <param name="clientId">クライアントを一意に識別する文字列です</param>
		/// <param name="description">この定義の説明テキスト。を定義に関連付けられているコントロールの上に移動すると、ステータス バーに表示されます。</param>
		/// <param name="tooltip">この定義のツール チップ テキスト</param>
		/// <param name="standardIcon">この定義を使用するコントロールに使用する標準サイズのアイコンを指定 16×16ピクセル</param>
		/// <param name="largeIcon">この定義を使用するコントロールに使用するラージサイズのアイコンを指定 32×32ピクセル</param>
		/// <param name="buttonDisplayType">ボタンにテキストとアイコンを表示するかどうかを示す定数</param>
		public InventorButton(string displayName, string internalName, Inventor.CommandTypesEnum classification,
			string clientId, string description, string tooltip, Icon standardIcon, Icon largeIcon, Inventor.ButtonDisplayEnum buttonDisplayType)
		{
			try
			{
                //get IPictureDisp for icons

                //                IPictureDisp standardIconIPictureDisp;
                //#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                //                standardIconIPictureDisp = (IPictureDisp)Support.IconToIPicture(standardIcon);
                //#pragma warning restore CS0618 // 型またはメンバーが旧型式です

                //                IPictureDisp largeIconIPictureDisp;
                //#pragma warning disable CS0618 // 型またはメンバーが旧型式です
                //                largeIconIPictureDisp = (IPictureDisp)Support.IconToIPicture(largeIcon);
                //#pragma warning restore CS0618 // 型またはメンバーが旧型式です



				// アセンブリ stdole, Version = 7.0.3300.0, Culture = neutral, PublicKeyToken = b03f5f7f11d50a3a
                IPictureDisp standardIconIPictureDisp = (IPictureDisp)PictureDispConverter.ToIPictureDisp(standardIcon);

                IPictureDisp largeIconIPictureDisp = (IPictureDisp)PictureDispConverter.ToIPictureDisp(largeIcon); ;


                //create button definition
                ButtonDefinition = InventorApp.CommandManager.ControlDefinitions.AddButtonDefinition(displayName, internalName, classification, clientId, description, tooltip, standardIconIPictureDisp , largeIconIPictureDisp, buttonDisplayType);												
				//enable the button
                ButtonDefinition.Enabled = true;
				
				//connect the button event sink
                ButtonDefinition_OnExecuteEventDelegate = new Inventor.ButtonDefinitionSink_OnExecuteEventHandler(ButtonDefinition_OnExecute);
                ButtonDefinition.OnExecute += ButtonDefinition_OnExecuteEventDelegate;
			}
			catch(Exception e)
			{
				MessageBox.Show($"Inventorアドイン ボタン追加クラスで例外 {displayName}{internalName}{e.ToString()}");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="displayName"></param>
		/// <param name="internalName"></param>
		/// <param name="commandType"></param>
		/// <param name="clientId"></param>
		/// <param name="description"></param>
		/// <param name="tooltip"></param>
		/// <param name="buttonDisplayType"></param>
		public InventorButton(string displayName, string internalName, Inventor.CommandTypesEnum commandType, string clientId, string description, string tooltip, Inventor.ButtonDisplayEnum buttonDisplayType)
		{
			try
			{			
				//create button definition
                ButtonDefinition = InventorApp.CommandManager.ControlDefinitions.AddButtonDefinition(displayName, internalName, commandType, clientId, description, tooltip, Type.Missing, Type.Missing, buttonDisplayType);
								
				//enable the button
                ButtonDefinition.Enabled = true;
				
				//connect the button event sink
				ButtonDefinition_OnExecuteEventDelegate = new Inventor.ButtonDefinitionSink_OnExecuteEventHandler(ButtonDefinition_OnExecute);
                ButtonDefinition.OnExecute += ButtonDefinition_OnExecuteEventDelegate;
			}
			catch(Exception e)
			{
				MessageBox.Show($"■東陽ﾂｰﾙ ボタン追加クラスで例外 {displayName}{internalName}{e.ToString()}");
			}
		}

		abstract protected void ButtonDefinition_OnExecute(Inventor.NameValueMap context);

		#endregion
	}
}
