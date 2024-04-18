
namespace SasaLibTestApp
{
    //partial class Tab10_MSIDLL_UserControl
    //{
    //    /// <summary> 
    //    /// 必要なデザイナー変数です。
    //    /// </summary>
    //    private System.ComponentModel.IContainer components = null;

    //    /// <summary> 
    //    /// 使用中のリソースをすべてクリーンアップします。
    //    /// </summary>
    //    /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing && (components != null))
    //        {
    //            components.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    #region コンポーネント デザイナーで生成されたコード

    //    /// <summary> 
    //    /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
    //    /// コード エディターで変更しないでください。
    //    /// </summary>
    //    private void InitializeComponent()
    //    {
    //        this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
    //        this.panel19 = new System.Windows.Forms.Panel();
    //        this.GetProductInfo_button = new System.Windows.Forms.Button();
    //        this.PRODUCTID_textBox = new System.Windows.Forms.TextBox();
    //        this.panel18 = new System.Windows.Forms.Panel();
    //        this.FindFromComponentId_button = new System.Windows.Forms.Button();
    //        this.COMPONENTID_textBox = new System.Windows.Forms.TextBox();
    //        this.panel17 = new System.Windows.Forms.Panel();
    //        this.FindProductName_button = new System.Windows.Forms.Button();
    //        this.textBox3 = new System.Windows.Forms.TextBox();
    //        this.panel16 = new System.Windows.Forms.Panel();
    //        this.Find_button = new System.Windows.Forms.Button();
    //        this.FileNameTextBox = new System.Windows.Forms.TextBox();
    //        this.ShowAllApplicationButton = new System.Windows.Forms.Button();
    //        this.panel1 = new System.Windows.Forms.Panel();
    //        this.FindComponentID_button = new System.Windows.Forms.Button();
    //        this.FullFilename_textBox = new System.Windows.Forms.TextBox();
    //        this.panel2 = new System.Windows.Forms.Panel();
    //        this.button1 = new System.Windows.Forms.Button();
    //        this.textBox1 = new System.Windows.Forms.TextBox();
    //        this.LogWindow_textBox = new System.Windows.Forms.TextBox();
    //        this.panel3 = new System.Windows.Forms.Panel();
    //        this.button2 = new System.Windows.Forms.Button();
    //        this.textBox2 = new System.Windows.Forms.TextBox();
    //        this.button3 = new System.Windows.Forms.Button();
    //        this.panel19.SuspendLayout();
    //        this.panel18.SuspendLayout();
    //        this.panel17.SuspendLayout();
    //        this.panel16.SuspendLayout();
    //        this.panel1.SuspendLayout();
    //        this.panel2.SuspendLayout();
    //        this.panel3.SuspendLayout();
    //        this.SuspendLayout();
    //        // 
    //        // openFileDialog1
    //        // 
    //        this.openFileDialog1.FileName = "openFileDialog1";
    //        // 
    //        // panel19
    //        // 
    //        this.panel19.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.panel19.Controls.Add(this.GetProductInfo_button);
    //        this.panel19.Controls.Add(this.PRODUCTID_textBox);
    //        this.panel19.Location = new System.Drawing.Point(12, 132);
    //        this.panel19.Name = "panel19";
    //        this.panel19.Size = new System.Drawing.Size(1369, 37);
    //        this.panel19.TabIndex = 108;
    //        // 
    //        // GetProductInfo_button
    //        // 
    //        this.GetProductInfo_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
    //        this.GetProductInfo_button.Location = new System.Drawing.Point(930, 8);
    //        this.GetProductInfo_button.Name = "GetProductInfo_button";
    //        this.GetProductInfo_button.Size = new System.Drawing.Size(436, 23);
    //        this.GetProductInfo_button.TabIndex = 2;
    //        this.GetProductInfo_button.Text = "ﾌﾟﾛﾀﾞｸﾄIDからﾌﾟﾛﾀﾞｸﾄ情報を取得する GetProductInfo_button(..)";
    //        this.GetProductInfo_button.UseVisualStyleBackColor = true;
    //        this.GetProductInfo_button.Click += new System.EventHandler(this.GetProductInfo_button_Click);
    //        // 
    //        // PRODUCTID_textBox
    //        // 
    //        this.PRODUCTID_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.PRODUCTID_textBox.Location = new System.Drawing.Point(7, 10);
    //        this.PRODUCTID_textBox.Name = "PRODUCTID_textBox";
    //        this.PRODUCTID_textBox.Size = new System.Drawing.Size(917, 19);
    //        this.PRODUCTID_textBox.TabIndex = 3;
    //        this.PRODUCTID_textBox.Text = "{B7C45CEC-9EA8-460D-9466-8A53B313424C}";
    //        // 
    //        // panel18
    //        // 
    //        this.panel18.Controls.Add(this.FindFromComponentId_button);
    //        this.panel18.Controls.Add(this.COMPONENTID_textBox);
    //        this.panel18.Location = new System.Drawing.Point(12, 46);
    //        this.panel18.Name = "panel18";
    //        this.panel18.Size = new System.Drawing.Size(1293, 37);
    //        this.panel18.TabIndex = 106;
    //        // 
    //        // FindFromComponentId_button
    //        // 
    //        this.FindFromComponentId_button.Location = new System.Drawing.Point(857, 8);
    //        this.FindFromComponentId_button.Name = "FindFromComponentId_button";
    //        this.FindFromComponentId_button.Size = new System.Drawing.Size(436, 23);
    //        this.FindFromComponentId_button.TabIndex = 2;
    //        this.FindFromComponentId_button.Text = "コンポーネントID からそれに対応するパスを検索(FindFromComponentId)";
    //        this.FindFromComponentId_button.UseVisualStyleBackColor = true;
    //        this.FindFromComponentId_button.Click += new System.EventHandler(this.FindFromComponentId_button_Click);
    //        // 
    //        // COMPONENTID_textBox
    //        // 
    //        this.COMPONENTID_textBox.Location = new System.Drawing.Point(7, 10);
    //        this.COMPONENTID_textBox.Name = "COMPONENTID_textBox";
    //        this.COMPONENTID_textBox.Size = new System.Drawing.Size(844, 19);
    //        this.COMPONENTID_textBox.TabIndex = 3;
    //        this.COMPONENTID_textBox.Text = "{C3971460-2664-3EB5-1033-BBAAD3952819}";
    //        // 
    //        // panel17
    //        // 
    //        this.panel17.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.panel17.Controls.Add(this.FindProductName_button);
    //        this.panel17.Controls.Add(this.textBox3);
    //        this.panel17.Location = new System.Drawing.Point(11, 89);
    //        this.panel17.Name = "panel17";
    //        this.panel17.Size = new System.Drawing.Size(1370, 37);
    //        this.panel17.TabIndex = 107;
    //        // 
    //        // FindProductName_button
    //        // 
    //        this.FindProductName_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
    //        this.FindProductName_button.Location = new System.Drawing.Point(931, 8);
    //        this.FindProductName_button.Name = "FindProductName_button";
    //        this.FindProductName_button.Size = new System.Drawing.Size(436, 23);
    //        this.FindProductName_button.TabIndex = 2;
    //        this.FindProductName_button.Text = "ｺﾝﾎﾟｰﾈﾝﾄIDからそれが含まれるプロダクトIDを検索(FindProductName)";
    //        this.FindProductName_button.UseVisualStyleBackColor = true;
    //        this.FindProductName_button.Click += new System.EventHandler(this.FindProductName_button_Click);
    //        // 
    //        // textBox3
    //        // 
    //        this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.textBox3.Location = new System.Drawing.Point(7, 10);
    //        this.textBox3.Name = "textBox3";
    //        this.textBox3.Size = new System.Drawing.Size(918, 19);
    //        this.textBox3.TabIndex = 3;
    //        this.textBox3.Text = "{C3971460-2664-3EB5-1033-BBAAD3952819}";
    //        // 
    //        // panel16
    //        // 
    //        this.panel16.Controls.Add(this.Find_button);
    //        this.panel16.Controls.Add(this.FileNameTextBox);
    //        this.panel16.Location = new System.Drawing.Point(11, 175);
    //        this.panel16.Name = "panel16";
    //        this.panel16.Size = new System.Drawing.Size(572, 37);
    //        this.panel16.TabIndex = 105;
    //        // 
    //        // Find_button
    //        // 
    //        this.Find_button.Location = new System.Drawing.Point(322, 6);
    //        this.Find_button.Name = "Find_button";
    //        this.Find_button.Size = new System.Drawing.Size(247, 23);
    //        this.Find_button.TabIndex = 2;
    //        this.Find_button.Text = "ﾌｧｲﾙ名がMSIﾃﾞｰﾀﾍﾞｰｽに存在するか検索";
    //        this.Find_button.UseVisualStyleBackColor = true;
    //        this.Find_button.Click += new System.EventHandler(this.Find_button_Click);
    //        // 
    //        // FileNameTextBox
    //        // 
    //        this.FileNameTextBox.Location = new System.Drawing.Point(7, 10);
    //        this.FileNameTextBox.Name = "FileNameTextBox";
    //        this.FileNameTextBox.Size = new System.Drawing.Size(309, 19);
    //        this.FileNameTextBox.TabIndex = 3;
    //        // 
    //        // ShowAllApplicationButton
    //        // 
    //        this.ShowAllApplicationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
    //        this.ShowAllApplicationButton.Location = new System.Drawing.Point(1251, 218);
    //        this.ShowAllApplicationButton.Name = "ShowAllApplicationButton";
    //        this.ShowAllApplicationButton.Size = new System.Drawing.Size(125, 23);
    //        this.ShowAllApplicationButton.TabIndex = 104;
    //        this.ShowAllApplicationButton.Text = "［］アプリケーション一覧";
    //        this.ShowAllApplicationButton.UseVisualStyleBackColor = true;
    //        this.ShowAllApplicationButton.Click += new System.EventHandler(this.ShowAllApplicationButton_Click);
    //        // 
    //        // panel1
    //        // 
    //        this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.panel1.Controls.Add(this.FindComponentID_button);
    //        this.panel1.Controls.Add(this.FullFilename_textBox);
    //        this.panel1.Location = new System.Drawing.Point(12, 3);
    //        this.panel1.Name = "panel1";
    //        this.panel1.Size = new System.Drawing.Size(1367, 37);
    //        this.panel1.TabIndex = 109;
    //        this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
    //        // 
    //        // FindComponentID_button
    //        // 
    //        this.FindComponentID_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
    //        this.FindComponentID_button.Location = new System.Drawing.Point(931, 8);
    //        this.FindComponentID_button.Name = "FindComponentID_button";
    //        this.FindComponentID_button.Size = new System.Drawing.Size(433, 23);
    //        this.FindComponentID_button.TabIndex = 2;
    //        this.FindComponentID_button.Text = "ﾌｧｲﾙﾌﾙﾊﾟｽからｺﾝﾎﾟｰﾈﾝﾄIDを検索(FindComponentID)";
    //        this.FindComponentID_button.UseVisualStyleBackColor = true;
    //        this.FindComponentID_button.Click += new System.EventHandler(this.FindComponentID_button_Click);
    //        // 
    //        // FullFilename_textBox
    //        // 
    //        this.FullFilename_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.FullFilename_textBox.Location = new System.Drawing.Point(7, 10);
    //        this.FullFilename_textBox.Name = "FullFilename_textBox";
    //        this.FullFilename_textBox.Size = new System.Drawing.Size(918, 19);
    //        this.FullFilename_textBox.TabIndex = 3;
    //        this.FullFilename_textBox.Text = " C:\\ProgramData\\Autodesk\\Inventor Addins\\TOYOM\\InventorTOYOaddinCommit\\GenerateTI" +
    //"FFdrawing.dll";
    //        // 
    //        // panel2
    //        // 
    //        this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.panel2.Controls.Add(this.button1);
    //        this.panel2.Controls.Add(this.textBox1);
    //        this.panel2.Location = new System.Drawing.Point(589, 175);
    //        this.panel2.Name = "panel2";
    //        this.panel2.Size = new System.Drawing.Size(792, 37);
    //        this.panel2.TabIndex = 110;
    //        // 
    //        // button1
    //        // 
    //        this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
    //        this.button1.Location = new System.Drawing.Point(446, 8);
    //        this.button1.Name = "button1";
    //        this.button1.Size = new System.Drawing.Size(343, 23);
    //        this.button1.TabIndex = 2;
    //        this.button1.Text = "ｱﾌﾟﾘｹｰｼｮﾝ名称から正規のｲﾝｽﾄｰﾙ名を検索";
    //        this.button1.UseVisualStyleBackColor = true;
    //        this.button1.Click += new System.EventHandler(this.button1_Click);
    //        // 
    //        // textBox1
    //        // 
    //        this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.textBox1.Location = new System.Drawing.Point(7, 10);
    //        this.textBox1.Name = "textBox1";
    //        this.textBox1.Size = new System.Drawing.Size(433, 19);
    //        this.textBox1.TabIndex = 3;
    //        this.textBox1.Text = "Inventor";
    //        // 
    //        // LogWindow_textBox
    //        // 
    //        this.LogWindow_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
    //        | System.Windows.Forms.AnchorStyles.Right)));
    //        this.LogWindow_textBox.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
    //        this.LogWindow_textBox.Location = new System.Drawing.Point(8, 331);
    //        this.LogWindow_textBox.Multiline = true;
    //        this.LogWindow_textBox.Name = "LogWindow_textBox";
    //        this.LogWindow_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
    //        this.LogWindow_textBox.Size = new System.Drawing.Size(1370, 299);
    //        this.LogWindow_textBox.TabIndex = 102;
    //        // 
    //        // panel3
    //        // 
    //        this.panel3.Controls.Add(this.button2);
    //        this.panel3.Controls.Add(this.textBox2);
    //        this.panel3.Location = new System.Drawing.Point(11, 218);
    //        this.panel3.Name = "panel3";
    //        this.panel3.Size = new System.Drawing.Size(572, 37);
    //        this.panel3.TabIndex = 111;
    //        // 
    //        // button2
    //        // 
    //        this.button2.Location = new System.Drawing.Point(301, 6);
    //        this.button2.Name = "button2";
    //        this.button2.Size = new System.Drawing.Size(268, 23);
    //        this.button2.TabIndex = 2;
    //        this.button2.Text = "プロダクトIDに関係するファイルを検索";
    //        this.button2.UseVisualStyleBackColor = true;
    //        this.button2.Click += new System.EventHandler(this.button2_Click);
    //        // 
    //        // textBox2
    //        // 
    //        this.textBox2.Location = new System.Drawing.Point(7, 10);
    //        this.textBox2.Name = "textBox2";
    //        this.textBox2.Size = new System.Drawing.Size(275, 19);
    //        this.textBox2.TabIndex = 3;
    //        this.textBox2.Text = "{F535B2CF-C9BB-4162-B03A-02D6971F32CC}";
    //        // 
    //        // button3
    //        // 
    //        this.button3.Location = new System.Drawing.Point(12, 302);
    //        this.button3.Name = "button3";
    //        this.button3.Size = new System.Drawing.Size(125, 23);
    //        this.button3.TabIndex = 112;
    //        this.button3.Text = "ログクリア";
    //        this.button3.UseVisualStyleBackColor = true;
    //        this.button3.Click += new System.EventHandler(this.button3_Click);
    //        // 
    //        // Tab10_MSIDLL_UserControl
    //        // 
    //        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
    //        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    //        this.Controls.Add(this.button3);
    //        this.Controls.Add(this.panel3);
    //        this.Controls.Add(this.panel2);
    //        this.Controls.Add(this.panel1);
    //        this.Controls.Add(this.panel19);
    //        this.Controls.Add(this.panel18);
    //        this.Controls.Add(this.panel17);
    //        this.Controls.Add(this.panel16);
    //        this.Controls.Add(this.ShowAllApplicationButton);
    //        this.Controls.Add(this.LogWindow_textBox);
    //        this.Name = "Tab10_MSIDLL_UserControl";
    //        this.Size = new System.Drawing.Size(1398, 633);
    //        this.Load += new System.EventHandler(this.TabControl1_Load);
    //        this.VisibleChanged += new System.EventHandler(this.TabControl1_VisibleChanged);
    //        this.panel19.ResumeLayout(false);
    //        this.panel19.PerformLayout();
    //        this.panel18.ResumeLayout(false);
    //        this.panel18.PerformLayout();
    //        this.panel17.ResumeLayout(false);
    //        this.panel17.PerformLayout();
    //        this.panel16.ResumeLayout(false);
    //        this.panel16.PerformLayout();
    //        this.panel1.ResumeLayout(false);
    //        this.panel1.PerformLayout();
    //        this.panel2.ResumeLayout(false);
    //        this.panel2.PerformLayout();
    //        this.panel3.ResumeLayout(false);
    //        this.panel3.PerformLayout();
    //        this.ResumeLayout(false);
    //        this.PerformLayout();

    //    }

    //    #endregion
    //    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    //    private System.Windows.Forms.Panel panel19;
    //    private System.Windows.Forms.Button GetProductInfo_button;
    //    private System.Windows.Forms.TextBox PRODUCTID_textBox;
    //    private System.Windows.Forms.Panel panel18;
    //    private System.Windows.Forms.Button FindFromComponentId_button;
    //    private System.Windows.Forms.TextBox COMPONENTID_textBox;
    //    private System.Windows.Forms.Panel panel17;
    //    private System.Windows.Forms.Button FindProductName_button;
    //    private System.Windows.Forms.TextBox textBox3;
    //    private System.Windows.Forms.Panel panel16;
    //    private System.Windows.Forms.Button Find_button;
    //    private System.Windows.Forms.TextBox FileNameTextBox;
    //    private System.Windows.Forms.Button ShowAllApplicationButton;
    //    private System.Windows.Forms.Panel panel1;
    //    private System.Windows.Forms.Button FindComponentID_button;
    //    private System.Windows.Forms.TextBox FullFilename_textBox;
    //    private System.Windows.Forms.Panel panel2;
    //    private System.Windows.Forms.Button button1;
    //    private System.Windows.Forms.TextBox textBox1;
    //    private System.Windows.Forms.TextBox LogWindow_textBox;
    //    private System.Windows.Forms.Panel panel3;
    //    private System.Windows.Forms.Button button2;
    //    private System.Windows.Forms.TextBox textBox2;
    //    private System.Windows.Forms.Button button3;
    //}
}
