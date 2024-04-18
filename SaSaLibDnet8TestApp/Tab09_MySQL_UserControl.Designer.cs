
namespace SasaLibTestApp
{
    partial class Tab09_MySQL_UserControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tab09_MySQL_UserControl));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.LogWindow_textBox = new System.Windows.Forms.TextBox();
            this.MySQLanserLinesLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.DatabaseComboBox = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SasaLibMySQLUserPass_textbox = new System.Windows.Forms.TextBox();
            this.SasaLibMySQLUserName_textbox = new System.Windows.Forms.TextBox();
            this.SsaLibMySqlServer_Name_textBox = new System.Windows.Forms.TextBox();
            this.SasaLibMySql_SQLtextBox = new System.Windows.Forms.TextBox();
            this.SasaLibMySql_OutPutttextBox = new System.Windows.Forms.TextBox();
            this.SasaLibMySql_tbutton = new System.Windows.Forms.Button();
            this.SsaLibMySqlServer_PortNumber_textBox = new System.Windows.Forms.TextBox();
            this.textDecode_textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // LogWindow_textBox
            // 
            this.LogWindow_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogWindow_textBox.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LogWindow_textBox.Location = new System.Drawing.Point(5, 400);
            this.LogWindow_textBox.Multiline = true;
            this.LogWindow_textBox.Name = "LogWindow_textBox";
            this.LogWindow_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogWindow_textBox.Size = new System.Drawing.Size(1047, 149);
            this.LogWindow_textBox.TabIndex = 102;
            // 
            // MySQLanserLinesLabel
            // 
            this.MySQLanserLinesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MySQLanserLinesLabel.AutoSize = true;
            this.MySQLanserLinesLabel.Location = new System.Drawing.Point(3, 385);
            this.MySQLanserLinesLabel.Name = "MySQLanserLinesLabel";
            this.MySQLanserLinesLabel.Size = new System.Drawing.Size(41, 12);
            this.MySQLanserLinesLabel.TabIndex = 112;
            this.MySQLanserLinesLabel.Text = "label17";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(945, 127);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(88, 23);
            this.progressBar1.TabIndex = 111;
            // 
            // DatabaseComboBox
            // 
            this.DatabaseComboBox.AutoCompleteCustomSource.AddRange(new string[] {
            "sampledb050",
            "dbkumizu",
            "dblayout"});
            this.DatabaseComboBox.FormattingEnabled = true;
            this.DatabaseComboBox.Items.AddRange(new object[] {
            "sampledb050",
            "dbkumizu",
            "dblayout"});
            this.DatabaseComboBox.Location = new System.Drawing.Point(451, 3);
            this.DatabaseComboBox.Name = "DatabaseComboBox";
            this.DatabaseComboBox.Size = new System.Drawing.Size(121, 20);
            this.DatabaseComboBox.TabIndex = 110;
            this.DatabaseComboBox.Text = "db_buhinzu";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(0, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(106, 12);
            this.label13.TabIndex = 109;
            this.label13.Text = "SET NAMES binary;";
            // 
            // SasaLibMySQLUserPass_textbox
            // 
            this.SasaLibMySQLUserPass_textbox.Location = new System.Drawing.Point(345, 4);
            this.SasaLibMySQLUserPass_textbox.Name = "SasaLibMySQLUserPass_textbox";
            this.SasaLibMySQLUserPass_textbox.Size = new System.Drawing.Size(100, 19);
            this.SasaLibMySQLUserPass_textbox.TabIndex = 108;
            this.SasaLibMySQLUserPass_textbox.Text = "readonlyuser";
            // 
            // SasaLibMySQLUserName_textbox
            // 
            this.SasaLibMySQLUserName_textbox.Location = new System.Drawing.Point(239, 4);
            this.SasaLibMySQLUserName_textbox.Name = "SasaLibMySQLUserName_textbox";
            this.SasaLibMySQLUserName_textbox.Size = new System.Drawing.Size(100, 19);
            this.SasaLibMySQLUserName_textbox.TabIndex = 107;
            this.SasaLibMySQLUserName_textbox.Text = "readonlyuser";
            // 
            // SsaLibMySqlServer_Name_textBox
            // 
            this.SsaLibMySqlServer_Name_textBox.Location = new System.Drawing.Point(0, 3);
            this.SsaLibMySqlServer_Name_textBox.Name = "SsaLibMySqlServer_Name_textBox";
            this.SsaLibMySqlServer_Name_textBox.Size = new System.Drawing.Size(100, 19);
            this.SsaLibMySqlServer_Name_textBox.TabIndex = 106;
            this.SsaLibMySqlServer_Name_textBox.Text = "ub2";
            // 
            // SasaLibMySql_SQLtextBox
            // 
            this.SasaLibMySql_SQLtextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SasaLibMySql_SQLtextBox.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SasaLibMySql_SQLtextBox.Location = new System.Drawing.Point(0, 54);
            this.SasaLibMySql_SQLtextBox.Multiline = true;
            this.SasaLibMySql_SQLtextBox.Name = "SasaLibMySql_SQLtextBox";
            this.SasaLibMySql_SQLtextBox.Size = new System.Drawing.Size(928, 130);
            this.SasaLibMySql_SQLtextBox.TabIndex = 105;
            this.SasaLibMySql_SQLtextBox.Text = resources.GetString("SasaLibMySql_SQLtextBox.Text");
            // 
            // SasaLibMySql_OutPutttextBox
            // 
            this.SasaLibMySql_OutPutttextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SasaLibMySql_OutPutttextBox.Location = new System.Drawing.Point(0, 190);
            this.SasaLibMySql_OutPutttextBox.Multiline = true;
            this.SasaLibMySql_OutPutttextBox.Name = "SasaLibMySql_OutPutttextBox";
            this.SasaLibMySql_OutPutttextBox.ReadOnly = true;
            this.SasaLibMySql_OutPutttextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.SasaLibMySql_OutPutttextBox.Size = new System.Drawing.Size(1052, 192);
            this.SasaLibMySql_OutPutttextBox.TabIndex = 104;
            // 
            // SasaLibMySql_tbutton
            // 
            this.SasaLibMySql_tbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SasaLibMySql_tbutton.Location = new System.Drawing.Point(945, 156);
            this.SasaLibMySql_tbutton.Name = "SasaLibMySql_tbutton";
            this.SasaLibMySql_tbutton.Size = new System.Drawing.Size(88, 23);
            this.SasaLibMySql_tbutton.TabIndex = 103;
            this.SasaLibMySql_tbutton.Text = "Execute";
            this.SasaLibMySql_tbutton.UseVisualStyleBackColor = true;
            this.SasaLibMySql_tbutton.Click += new System.EventHandler(this.SasaLibMySql_tbutton_Click);
            // 
            // SsaLibMySqlServer_PortNumber_textBox
            // 
            this.SsaLibMySqlServer_PortNumber_textBox.Location = new System.Drawing.Point(106, 3);
            this.SsaLibMySqlServer_PortNumber_textBox.Name = "SsaLibMySqlServer_PortNumber_textBox";
            this.SsaLibMySqlServer_PortNumber_textBox.Size = new System.Drawing.Size(100, 19);
            this.SsaLibMySqlServer_PortNumber_textBox.TabIndex = 113;
            this.SsaLibMySqlServer_PortNumber_textBox.Text = "13306";
            // 
            // textDecode_textBox1
            // 
            this.textDecode_textBox1.Location = new System.Drawing.Point(578, 3);
            this.textDecode_textBox1.Name = "textDecode_textBox1";
            this.textDecode_textBox1.Size = new System.Drawing.Size(100, 19);
            this.textDecode_textBox1.TabIndex = 114;
            this.textDecode_textBox1.Text = "utf8";
            // 
            // Tab09_MySQL_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textDecode_textBox1);
            this.Controls.Add(this.SsaLibMySqlServer_PortNumber_textBox);
            this.Controls.Add(this.MySQLanserLinesLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.DatabaseComboBox);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.SasaLibMySQLUserPass_textbox);
            this.Controls.Add(this.SasaLibMySQLUserName_textbox);
            this.Controls.Add(this.SsaLibMySqlServer_Name_textBox);
            this.Controls.Add(this.SasaLibMySql_SQLtextBox);
            this.Controls.Add(this.SasaLibMySql_OutPutttextBox);
            this.Controls.Add(this.SasaLibMySql_tbutton);
            this.Controls.Add(this.LogWindow_textBox);
            this.Name = "Tab09_MySQL_UserControl";
            this.Size = new System.Drawing.Size(1055, 568);
            this.Load += new System.EventHandler(this.TabControl1_Load);
            this.VisibleChanged += new System.EventHandler(this.TabControl1_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.Label MySQLanserLinesLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ComboBox DatabaseComboBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox SasaLibMySQLUserPass_textbox;
        private System.Windows.Forms.TextBox SasaLibMySQLUserName_textbox;
        private System.Windows.Forms.TextBox SsaLibMySqlServer_Name_textBox;
        private System.Windows.Forms.TextBox SasaLibMySql_SQLtextBox;
        private System.Windows.Forms.TextBox SasaLibMySql_OutPutttextBox;
        private System.Windows.Forms.Button SasaLibMySql_tbutton;
        private System.Windows.Forms.TextBox SsaLibMySqlServer_PortNumber_textBox;
        private System.Windows.Forms.TextBox textDecode_textBox1;
    }
}
