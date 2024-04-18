
namespace SasaLibTestApp
{
    partial class Tab04_Drawing3_UserControl
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.LogWindow_textBox = new System.Windows.Forms.TextBox();
            this.panel14 = new System.Windows.Forms.Panel();
            this.DecodedLabel = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.EncodedLabel = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.InputMessageTextBox = new System.Windows.Forms.TextBox();
            this.button16 = new System.Windows.Forms.Button();
            this.panel12 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.TIFFFILEFULLPATHLabel = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.SaveTiffFileButton = new System.Windows.Forms.Button();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.ConvertButton1 = new System.Windows.Forms.Button();
            this.ConvertButton2 = new System.Windows.Forms.Button();
            this.panel11 = new System.Windows.Forms.Panel();
            this.InputImageFIleTextBox = new System.Windows.Forms.TextBox();
            this.LoadImageButton = new System.Windows.Forms.Button();
            this.panel10 = new System.Windows.Forms.Panel();
            this.NewPictureBox = new System.Windows.Forms.PictureBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.OrgPictureBox = new System.Windows.Forms.PictureBox();
            this.panel14.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewPictureBox)).BeginInit();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrgPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // LogWindow_textBox
            // 
            this.LogWindow_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LogWindow_textBox.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LogWindow_textBox.Location = new System.Drawing.Point(547, 423);
            this.LogWindow_textBox.Multiline = true;
            this.LogWindow_textBox.Name = "LogWindow_textBox";
            this.LogWindow_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogWindow_textBox.Size = new System.Drawing.Size(571, 178);
            this.LogWindow_textBox.TabIndex = 102;
            this.LogWindow_textBox.TextChanged += new System.EventHandler(this.LogWindow_textBox_TextChanged);
            // 
            // panel14
            // 
            this.panel14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel14.Controls.Add(this.DecodedLabel);
            this.panel14.Controls.Add(this.label16);
            this.panel14.Controls.Add(this.EncodedLabel);
            this.panel14.Controls.Add(this.label14);
            this.panel14.Controls.Add(this.label11);
            this.panel14.Controls.Add(this.InputMessageTextBox);
            this.panel14.Controls.Add(this.button16);
            this.panel14.Location = new System.Drawing.Point(0, 423);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(541, 178);
            this.panel14.TabIndex = 107;
            // 
            // DecodedLabel
            // 
            this.DecodedLabel.AutoSize = true;
            this.DecodedLabel.Location = new System.Drawing.Point(115, 111);
            this.DecodedLabel.Name = "DecodedLabel";
            this.DecodedLabel.Size = new System.Drawing.Size(23, 12);
            this.DecodedLabel.TabIndex = 7;
            this.DecodedLabel.Text = "---";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(18, 111);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 12);
            this.label16.TabIndex = 6;
            this.label16.Text = "復号化後文字列";
            // 
            // EncodedLabel
            // 
            this.EncodedLabel.AutoSize = true;
            this.EncodedLabel.Location = new System.Drawing.Point(115, 85);
            this.EncodedLabel.Name = "EncodedLabel";
            this.EncodedLabel.Size = new System.Drawing.Size(23, 12);
            this.EncodedLabel.TabIndex = 5;
            this.EncodedLabel.Text = "---";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(56, 56);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 3;
            this.label14.Text = "入力文字";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 3;
            this.label11.Text = "暗号化後文字列";
            // 
            // InputMessageTextBox
            // 
            this.InputMessageTextBox.Location = new System.Drawing.Point(115, 53);
            this.InputMessageTextBox.Name = "InputMessageTextBox";
            this.InputMessageTextBox.Size = new System.Drawing.Size(100, 19);
            this.InputMessageTextBox.TabIndex = 2;
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(221, 51);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(75, 23);
            this.button16.TabIndex = 1;
            this.button16.Text = "button16";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // panel12
            // 
            this.panel12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel12.Controls.Add(this.comboBox1);
            this.panel12.Controls.Add(this.panel13);
            this.panel12.Controls.Add(this.checkBox6);
            this.panel12.Controls.Add(this.SaveTiffFileButton);
            this.panel12.Controls.Add(this.checkBox5);
            this.panel12.Controls.Add(this.checkBox4);
            this.panel12.Controls.Add(this.ConvertButton1);
            this.panel12.Controls.Add(this.ConvertButton2);
            this.panel12.Location = new System.Drawing.Point(547, 314);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(571, 103);
            this.panel12.TabIndex = 106;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(145, 20);
            this.comboBox1.TabIndex = 20;
            // 
            // panel13
            // 
            this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel13.Controls.Add(this.TIFFFILEFULLPATHLabel);
            this.panel13.Location = new System.Drawing.Point(3, 53);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(404, 20);
            this.panel13.TabIndex = 18;
            // 
            // TIFFFILEFULLPATHLabel
            // 
            this.TIFFFILEFULLPATHLabel.AutoSize = true;
            this.TIFFFILEFULLPATHLabel.Location = new System.Drawing.Point(3, 3);
            this.TIFFFILEFULLPATHLabel.Name = "TIFFFILEFULLPATHLabel";
            this.TIFFFILEFULLPATHLabel.Size = new System.Drawing.Size(106, 12);
            this.TIFFFILEFULLPATHLabel.TabIndex = 16;
            this.TIFFFILEFULLPATHLabel.Text = "TIFF保存ファイルパス";
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(292, 31);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(80, 16);
            this.checkBox6.TabIndex = 19;
            this.checkBox6.Text = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // SaveTiffFileButton
            // 
            this.SaveTiffFileButton.Location = new System.Drawing.Point(332, 78);
            this.SaveTiffFileButton.Name = "SaveTiffFileButton";
            this.SaveTiffFileButton.Size = new System.Drawing.Size(75, 20);
            this.SaveTiffFileButton.TabIndex = 17;
            this.SaveTiffFileButton.Text = "保存実行";
            this.SaveTiffFileButton.UseVisualStyleBackColor = true;
            this.SaveTiffFileButton.Click += new System.EventHandler(this.SaveTiffFileButton_Click);
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(198, 31);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(80, 16);
            this.checkBox5.TabIndex = 18;
            this.checkBox5.Text = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(314, 6);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(80, 16);
            this.checkBox4.TabIndex = 17;
            this.checkBox4.Text = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // ConvertButton1
            // 
            this.ConvertButton1.Location = new System.Drawing.Point(3, 3);
            this.ConvertButton1.Name = "ConvertButton1";
            this.ConvertButton1.Size = new System.Drawing.Size(106, 20);
            this.ConvertButton1.TabIndex = 14;
            this.ConvertButton1.Text = "ConvertButton1";
            this.ConvertButton1.UseVisualStyleBackColor = true;
            this.ConvertButton1.Click += new System.EventHandler(this.ConvertButton1_Click);
            // 
            // ConvertButton2
            // 
            this.ConvertButton2.Location = new System.Drawing.Point(115, 3);
            this.ConvertButton2.Name = "ConvertButton2";
            this.ConvertButton2.Size = new System.Drawing.Size(106, 20);
            this.ConvertButton2.TabIndex = 15;
            this.ConvertButton2.Text = "ConvertButton2";
            this.ConvertButton2.UseVisualStyleBackColor = true;
            this.ConvertButton2.Click += new System.EventHandler(this.ConvertButton2_Click);
            // 
            // panel11
            // 
            this.panel11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel11.Controls.Add(this.InputImageFIleTextBox);
            this.panel11.Controls.Add(this.LoadImageButton);
            this.panel11.Location = new System.Drawing.Point(0, 314);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(541, 103);
            this.panel11.TabIndex = 105;
            // 
            // InputImageFIleTextBox
            // 
            this.InputImageFIleTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputImageFIleTextBox.Location = new System.Drawing.Point(3, 3);
            this.InputImageFIleTextBox.Name = "InputImageFIleTextBox";
            this.InputImageFIleTextBox.Size = new System.Drawing.Size(527, 19);
            this.InputImageFIleTextBox.TabIndex = 15;
            // 
            // LoadImageButton
            // 
            this.LoadImageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadImageButton.Location = new System.Drawing.Point(455, 27);
            this.LoadImageButton.Name = "LoadImageButton";
            this.LoadImageButton.Size = new System.Drawing.Size(75, 20);
            this.LoadImageButton.TabIndex = 14;
            this.LoadImageButton.Text = "イメージ選択";
            this.LoadImageButton.UseVisualStyleBackColor = true;
            this.LoadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // panel10
            // 
            this.panel10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel10.AutoScroll = true;
            this.panel10.BackColor = System.Drawing.Color.Gainsboro;
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Controls.Add(this.NewPictureBox);
            this.panel10.Location = new System.Drawing.Point(547, 2);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(571, 308);
            this.panel10.TabIndex = 104;
            // 
            // NewPictureBox
            // 
            this.NewPictureBox.Location = new System.Drawing.Point(3, 33);
            this.NewPictureBox.Name = "NewPictureBox";
            this.NewPictureBox.Size = new System.Drawing.Size(109, 88);
            this.NewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.NewPictureBox.TabIndex = 1;
            this.NewPictureBox.TabStop = false;
            // 
            // panel9
            // 
            this.panel9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel9.AutoScroll = true;
            this.panel9.BackColor = System.Drawing.Color.Gainsboro;
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.OrgPictureBox);
            this.panel9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(541, 308);
            this.panel9.TabIndex = 103;
            // 
            // OrgPictureBox
            // 
            this.OrgPictureBox.Location = new System.Drawing.Point(3, 3);
            this.OrgPictureBox.Name = "OrgPictureBox";
            this.OrgPictureBox.Size = new System.Drawing.Size(109, 88);
            this.OrgPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.OrgPictureBox.TabIndex = 1;
            this.OrgPictureBox.TabStop = false;
            // 
            // Tab04_Drawing3_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel14);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.LogWindow_textBox);
            this.Name = "Tab04_Drawing3_UserControl";
            this.Size = new System.Drawing.Size(1121, 611);
            this.Load += new System.EventHandler(this.TabControl1_Load);
            this.VisibleChanged += new System.EventHandler(this.TabControl1_VisibleChanged);
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewPictureBox)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OrgPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label DecodedLabel;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label EncodedLabel;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox InputMessageTextBox;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Label TIFFFILEFULLPATHLabel;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.Button SaveTiffFileButton;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Button ConvertButton1;
        private System.Windows.Forms.Button ConvertButton2;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.TextBox InputImageFIleTextBox;
        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.PictureBox NewPictureBox;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.PictureBox OrgPictureBox;
    }
}
