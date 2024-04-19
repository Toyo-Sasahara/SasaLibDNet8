
namespace SaSaLibTestApp
{
    partial class Tab03_Drawing2_UserControl
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.barcodeDraw1CheckBox = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.DrawBarcodeFromRightButtomButton = new System.Windows.Forms.Button();
            this.SizeSelectComboBox = new System.Windows.Forms.ComboBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LocadImaeButton = new System.Windows.Forms.Button();
            this.barcodeDraw2CheckBox = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.LocadImaeButton2_Click = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button15 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // LogWindow_textBox
            // 
            this.LogWindow_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogWindow_textBox.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LogWindow_textBox.Location = new System.Drawing.Point(422, 3);
            this.LogWindow_textBox.Multiline = true;
            this.LogWindow_textBox.Name = "LogWindow_textBox";
            this.LogWindow_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogWindow_textBox.Size = new System.Drawing.Size(487, 673);
            this.LogWindow_textBox.TabIndex = 102;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.barcodeDraw1CheckBox);
            this.splitContainer2.Panel1.Controls.Add(this.checkBox1);
            this.splitContainer2.Panel1.Controls.Add(this.DrawBarcodeFromRightButtomButton);
            this.splitContainer2.Panel1.Controls.Add(this.SizeSelectComboBox);
            this.splitContainer2.Panel1.Controls.Add(this.panel8);
            this.splitContainer2.Panel1.Controls.Add(this.LocadImaeButton);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.barcodeDraw2CheckBox);
            this.splitContainer2.Panel2.Controls.Add(this.checkBox2);
            this.splitContainer2.Panel2.Controls.Add(this.LocadImaeButton2_Click);
            this.splitContainer2.Panel2.Controls.Add(this.button20);
            this.splitContainer2.Panel2.Controls.Add(this.panel7);
            this.splitContainer2.Panel2.Controls.Add(this.button15);
            this.splitContainer2.Size = new System.Drawing.Size(1807, 830);
            this.splitContainer2.SplitterDistance = 885;
            this.splitContainer2.TabIndex = 103;
            // 
            // barcodeDraw1CheckBox
            // 
            this.barcodeDraw1CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.barcodeDraw1CheckBox.AutoSize = true;
            this.barcodeDraw1CheckBox.Checked = true;
            this.barcodeDraw1CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.barcodeDraw1CheckBox.Location = new System.Drawing.Point(87, 808);
            this.barcodeDraw1CheckBox.Name = "barcodeDraw1CheckBox";
            this.barcodeDraw1CheckBox.Size = new System.Drawing.Size(92, 16);
            this.barcodeDraw1CheckBox.TabIndex = 15;
            this.barcodeDraw1CheckBox.Text = "BarcodeDraw";
            this.barcodeDraw1CheckBox.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 808);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 16);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // DrawBarcodeFromRightButtomButton
            // 
            this.DrawBarcodeFromRightButtomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DrawBarcodeFromRightButtomButton.Location = new System.Drawing.Point(185, 801);
            this.DrawBarcodeFromRightButtomButton.Name = "DrawBarcodeFromRightButtomButton";
            this.DrawBarcodeFromRightButtomButton.Size = new System.Drawing.Size(75, 23);
            this.DrawBarcodeFromRightButtomButton.TabIndex = 7;
            this.DrawBarcodeFromRightButtomButton.Text = "DrawBarcodeFromRightButtomButton";
            this.DrawBarcodeFromRightButtomButton.UseVisualStyleBackColor = true;
            this.DrawBarcodeFromRightButtomButton.Click += new System.EventHandler(this.DrawBarcodeFromRightButtomButton_Click);
            // 
            // SizeSelectComboBox
            // 
            this.SizeSelectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SizeSelectComboBox.FormattingEnabled = true;
            this.SizeSelectComboBox.Items.AddRange(new object[] {
            "100",
            "90",
            "80",
            "70",
            "60",
            "50",
            "40",
            "30",
            "20",
            "10"});
            this.SizeSelectComboBox.Location = new System.Drawing.Point(87, 774);
            this.SizeSelectComboBox.Name = "SizeSelectComboBox";
            this.SizeSelectComboBox.Size = new System.Drawing.Size(87, 20);
            this.SizeSelectComboBox.TabIndex = 9;
            this.SizeSelectComboBox.Text = "100";
            this.SizeSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.SizeSelectComboBox_SelectedIndexChanged);
            // 
            // panel8
            // 
            this.panel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel8.AutoScroll = true;
            this.panel8.Controls.Add(this.pictureBox1);
            this.panel8.Location = new System.Drawing.Point(3, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(879, 745);
            this.panel8.TabIndex = 11;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(109, 88);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // LocadImaeButton
            // 
            this.LocadImaeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocadImaeButton.Location = new System.Drawing.Point(6, 772);
            this.LocadImaeButton.Name = "LocadImaeButton";
            this.LocadImaeButton.Size = new System.Drawing.Size(75, 23);
            this.LocadImaeButton.TabIndex = 0;
            this.LocadImaeButton.Text = "イメージ選択";
            this.LocadImaeButton.UseVisualStyleBackColor = true;
            this.LocadImaeButton.Click += new System.EventHandler(this.LocadImaeButton_Click);
            // 
            // barcodeDraw2CheckBox
            // 
            this.barcodeDraw2CheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.barcodeDraw2CheckBox.AutoSize = true;
            this.barcodeDraw2CheckBox.Checked = true;
            this.barcodeDraw2CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.barcodeDraw2CheckBox.Location = new System.Drawing.Point(89, 808);
            this.barcodeDraw2CheckBox.Name = "barcodeDraw2CheckBox";
            this.barcodeDraw2CheckBox.Size = new System.Drawing.Size(92, 16);
            this.barcodeDraw2CheckBox.TabIndex = 16;
            this.barcodeDraw2CheckBox.Text = "BarcodeDraw";
            this.barcodeDraw2CheckBox.UseVisualStyleBackColor = true;
            this.barcodeDraw2CheckBox.CheckedChanged += new System.EventHandler(this.barcodeDraw2CheckBox_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(6, 808);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(80, 16);
            this.checkBox2.TabIndex = 15;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // LocadImaeButton2_Click
            // 
            this.LocadImaeButton2_Click.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LocadImaeButton2_Click.Location = new System.Drawing.Point(8, 773);
            this.LocadImaeButton2_Click.Name = "LocadImaeButton2_Click";
            this.LocadImaeButton2_Click.Size = new System.Drawing.Size(75, 23);
            this.LocadImaeButton2_Click.TabIndex = 12;
            this.LocadImaeButton2_Click.Text = "イメージ選択";
            this.LocadImaeButton2_Click.UseVisualStyleBackColor = true;
            this.LocadImaeButton2_Click.Click += new System.EventHandler(this.LocadImaeButton2_Click_Click);
            // 
            // button20
            // 
            this.button20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button20.Location = new System.Drawing.Point(187, 804);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(75, 23);
            this.button20.TabIndex = 13;
            this.button20.Text = "button20";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // panel7
            // 
            this.panel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel7.AutoScroll = true;
            this.panel7.Controls.Add(this.LogWindow_textBox);
            this.panel7.Controls.Add(this.pictureBox2);
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(912, 745);
            this.panel7.TabIndex = 10;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(125, 88);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // button15
            // 
            this.button15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button15.Location = new System.Drawing.Point(98, 773);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 3;
            this.button15.Text = "button15";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // TabControl02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer2);
            this.Name = "TabControl02";
            this.Size = new System.Drawing.Size(1807, 830);
            this.Load += new System.EventHandler(this.TabControl1_Load);
            this.VisibleChanged += new System.EventHandler(this.TabControl1_VisibleChanged);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox barcodeDraw1CheckBox;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button DrawBarcodeFromRightButtomButton;
        private System.Windows.Forms.ComboBox SizeSelectComboBox;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button LocadImaeButton;
        private System.Windows.Forms.CheckBox barcodeDraw2CheckBox;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button LocadImaeButton2_Click;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button15;
    }
}
