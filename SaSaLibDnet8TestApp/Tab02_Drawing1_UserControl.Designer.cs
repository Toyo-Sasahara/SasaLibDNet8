
namespace SaSaLibDNet8TestAPP
{
    partial class Tab02_Drawing1_UserControl
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
            openFileDialog1 = new OpenFileDialog();
            LogWindow_textBox = new TextBox();
            splitContainer3 = new SplitContainer();
            panel4 = new Panel();
            button7 = new Button();
            FormatTextbox = new TextBox();
            LoadImageFile = new Button();
            groupBox5 = new GroupBox();
            MoveImageMili_button = new Button();
            RotateButton = new Button();
            button1 = new Button();
            button4 = new Button();
            button5 = new Button();
            groupBox4 = new GroupBox();
            pictureBox1 = new PictureBox();
            PixSizeTextBox = new TextBox();
            label6 = new Label();
            label7 = new Label();
            BPPtextBox = new TextBox();
            label3 = new Label();
            PaperSizetextBox = new TextBox();
            label2 = new Label();
            ResTextBox = new TextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            button9 = new Button();
            button8 = new Button();
            button6 = new Button();
            groupBox1 = new GroupBox();
            button2 = new Button();
            DXtextBox = new TextBox();
            DYtextBox = new TextBox();
            button3 = new Button();
            ImageComboBox = new ComboBox();
            LoadImageFIleButton = new Button();
            PrintRawLoadButton = new Button();
            panel15 = new Panel();
            imagePictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            panel4.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            panel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)imagePictureBox).BeginInit();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // LogWindow_textBox
            // 
            LogWindow_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            LogWindow_textBox.Font = new Font("MS UI Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            LogWindow_textBox.Location = new Point(15, 635);
            LogWindow_textBox.Margin = new Padding(4);
            LogWindow_textBox.Multiline = true;
            LogWindow_textBox.Name = "LogWindow_textBox";
            LogWindow_textBox.ScrollBars = ScrollBars.Both;
            LogWindow_textBox.Size = new Size(950, 226);
            LogWindow_textBox.TabIndex = 102;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 0);
            splitContainer3.Margin = new Padding(4);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(panel4);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(panel15);
            splitContainer3.Size = new Size(1367, 872);
            splitContainer3.SplitterDistance = 393;
            splitContainer3.SplitterWidth = 5;
            splitContainer3.TabIndex = 103;
            // 
            // panel4
            // 
            panel4.Controls.Add(button7);
            panel4.Controls.Add(FormatTextbox);
            panel4.Controls.Add(LoadImageFile);
            panel4.Controls.Add(groupBox5);
            panel4.Controls.Add(groupBox4);
            panel4.Controls.Add(groupBox2);
            panel4.Controls.Add(groupBox1);
            panel4.Controls.Add(button3);
            panel4.Controls.Add(ImageComboBox);
            panel4.Controls.Add(LoadImageFIleButton);
            panel4.Controls.Add(PrintRawLoadButton);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(4);
            panel4.Name = "panel4";
            panel4.Size = new Size(393, 872);
            panel4.TabIndex = 2;
            // 
            // button7
            // 
            button7.Location = new Point(296, 40);
            button7.Margin = new Padding(4);
            button7.Name = "button7";
            button7.Size = new Size(79, 29);
            button7.TabIndex = 49;
            button7.Text = "button7";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // FormatTextbox
            // 
            FormatTextbox.Location = new Point(206, 6);
            FormatTextbox.Margin = new Padding(4);
            FormatTextbox.Name = "FormatTextbox";
            FormatTextbox.ReadOnly = true;
            FormatTextbox.Size = new Size(168, 23);
            FormatTextbox.TabIndex = 48;
            // 
            // LoadImageFile
            // 
            LoadImageFile.Location = new Point(4, 40);
            LoadImageFile.Margin = new Padding(4);
            LoadImageFile.Name = "LoadImageFile";
            LoadImageFile.Size = new Size(286, 29);
            LoadImageFile.TabIndex = 3;
            LoadImageFile.Text = "Load pm ファイル to TIFFﾌｧｲﾙ";
            LoadImageFile.UseVisualStyleBackColor = true;
            LoadImageFile.Click += LocadImageFile_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(MoveImageMili_button);
            groupBox5.Controls.Add(RotateButton);
            groupBox5.Controls.Add(button1);
            groupBox5.Controls.Add(button4);
            groupBox5.Controls.Add(button5);
            groupBox5.Location = new Point(6, 332);
            groupBox5.Margin = new Padding(4);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(4);
            groupBox5.Size = new Size(284, 138);
            groupBox5.TabIndex = 47;
            groupBox5.TabStop = false;
            groupBox5.Text = "groupBox5";
            // 
            // MoveImageMili_button
            // 
            MoveImageMili_button.Location = new Point(7, 22);
            MoveImageMili_button.Margin = new Padding(4);
            MoveImageMili_button.Name = "MoveImageMili_button";
            MoveImageMili_button.Size = new Size(125, 29);
            MoveImageMili_button.TabIndex = 45;
            MoveImageMili_button.Text = "MoveImageMilli";
            MoveImageMili_button.TextAlign = ContentAlignment.BottomCenter;
            MoveImageMili_button.UseVisualStyleBackColor = true;
            MoveImageMili_button.Click += MoveImageMili_button_Click;
            // 
            // RotateButton
            // 
            RotateButton.Location = new Point(7, 95);
            RotateButton.Margin = new Padding(4);
            RotateButton.Name = "RotateButton";
            RotateButton.Size = new Size(125, 29);
            RotateButton.TabIndex = 30;
            RotateButton.Text = "回転";
            RotateButton.UseVisualStyleBackColor = true;
            RotateButton.Click += RotateButton_Click;
            // 
            // button1
            // 
            button1.Location = new Point(147, 59);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(130, 29);
            button1.TabIndex = 34;
            button1.Text = "STAMP2";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Stamp2Button_Click;
            // 
            // button4
            // 
            button4.Location = new Point(147, 95);
            button4.Margin = new Padding(4);
            button4.Name = "button4";
            button4.Size = new Size(130, 29);
            button4.TabIndex = 37;
            button4.Text = "BarcodeButton";
            button4.UseVisualStyleBackColor = true;
            button4.Click += BarcodeButton_Click;
            // 
            // button5
            // 
            button5.Location = new Point(7, 59);
            button5.Margin = new Padding(4);
            button5.Name = "button5";
            button5.Size = new Size(125, 29);
            button5.TabIndex = 44;
            button5.Text = "LockBitsTes";
            button5.UseVisualStyleBackColor = true;
            button5.Click += LockBitsTestButton_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(PixSizeTextBox);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(BPPtextBox);
            groupBox4.Controls.Add(label3);
            groupBox4.Controls.Add(PaperSizetextBox);
            groupBox4.Controls.Add(label2);
            groupBox4.Controls.Add(ResTextBox);
            groupBox4.Controls.Add(label1);
            groupBox4.Location = new Point(4, 146);
            groupBox4.Margin = new Padding(4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(4);
            groupBox4.Size = new Size(286, 179);
            groupBox4.TabIndex = 46;
            groupBox4.TabStop = false;
            groupBox4.Text = "groupBox4";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(15, 519);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(644, 68);
            pictureBox1.TabIndex = 50;
            pictureBox1.TabStop = false;
            // 
            // PixSizeTextBox
            // 
            PixSizeTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PixSizeTextBox.Location = new Point(72, 22);
            PixSizeTextBox.Margin = new Padding(4);
            PixSizeTextBox.Name = "PixSizeTextBox";
            PixSizeTextBox.ReadOnly = true;
            PixSizeTextBox.Size = new Size(206, 23);
            PixSizeTextBox.TabIndex = 3;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(4, 121);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(39, 15);
            label6.TabIndex = 27;
            label6.Text = "色深さ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(4, 152);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(31, 15);
            label7.TabIndex = 29;
            label7.Text = "形式";
            // 
            // BPPtextBox
            // 
            BPPtextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            BPPtextBox.Location = new Point(72, 118);
            BPPtextBox.Margin = new Padding(4);
            BPPtextBox.Name = "BPPtextBox";
            BPPtextBox.ReadOnly = true;
            BPPtextBox.Size = new Size(206, 23);
            BPPtextBox.TabIndex = 26;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(4, 90);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(59, 15);
            label3.TabIndex = 8;
            label3.Text = "用紙サイズ";
            // 
            // PaperSizetextBox
            // 
            PaperSizetextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PaperSizetextBox.Location = new Point(72, 86);
            PaperSizetextBox.Margin = new Padding(4);
            PaperSizetextBox.Name = "PaperSizetextBox";
            PaperSizetextBox.ReadOnly = true;
            PaperSizetextBox.Size = new Size(206, 23);
            PaperSizetextBox.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(4, 59);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 6;
            label2.Text = "解像度";
            // 
            // ResTextBox
            // 
            ResTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ResTextBox.Location = new Point(72, 55);
            ResTextBox.Margin = new Padding(4);
            ResTextBox.Name = "ResTextBox";
            ResTextBox.ReadOnly = true;
            ResTextBox.Size = new Size(206, 23);
            ResTextBox.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, 28);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 4;
            label1.Text = "画像情報";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button9);
            groupBox2.Controls.Add(button8);
            groupBox2.Controls.Add(button6);
            groupBox2.Location = new Point(13, 576);
            groupBox2.Margin = new Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4);
            groupBox2.Size = new Size(276, 138);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "groupBox2";
            // 
            // button9
            // 
            button9.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button9.Location = new Point(7, 95);
            button9.Margin = new Padding(4);
            button9.Name = "button9";
            button9.Size = new Size(262, 29);
            button9.TabIndex = 48;
            button9.Text = "1bppIndexed";
            button9.UseVisualStyleBackColor = true;
            button9.Click += Format1bppIndexe_Click;
            // 
            // button8
            // 
            button8.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button8.Location = new Point(7, 59);
            button8.Margin = new Padding(4);
            button8.Name = "button8";
            button8.Size = new Size(262, 29);
            button8.TabIndex = 47;
            button8.Text = "Format24bppRgb";
            button8.UseVisualStyleBackColor = true;
            button8.Click += Format24bppRgb_Click;
            // 
            // button6
            // 
            button6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button6.Location = new Point(7, 22);
            button6.Margin = new Padding(4);
            button6.Name = "button6";
            button6.Size = new Size(262, 29);
            button6.TabIndex = 46;
            button6.Text = "ConvertImagePixelFormat";
            button6.UseVisualStyleBackColor = true;
            button6.Click += ConvertImagePixelFormatButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(DXtextBox);
            groupBox1.Controls.Add(DYtextBox);
            groupBox1.Location = new Point(10, 476);
            groupBox1.Margin = new Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4);
            groupBox1.Size = new Size(279, 92);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "ピクセルフォーマット変更";
            // 
            // button2
            // 
            button2.Location = new Point(7, 22);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(265, 29);
            button2.TabIndex = 35;
            button2.Text = "PixcelFormatChange";
            button2.UseVisualStyleBackColor = true;
            button2.Click += PixcelFormatChangeButton_Click;
            // 
            // DXtextBox
            // 
            DXtextBox.Location = new Point(94, 59);
            DXtextBox.Margin = new Padding(4);
            DXtextBox.Name = "DXtextBox";
            DXtextBox.Size = new Size(47, 23);
            DXtextBox.TabIndex = 40;
            DXtextBox.Text = "0";
            // 
            // DYtextBox
            // 
            DYtextBox.Location = new Point(149, 59);
            DYtextBox.Margin = new Padding(4);
            DYtextBox.Name = "DYtextBox";
            DYtextBox.Size = new Size(47, 23);
            DYtextBox.TabIndex = 41;
            DYtextBox.Text = "0";
            // 
            // button3
            // 
            button3.Location = new Point(4, 110);
            button3.Margin = new Padding(4);
            button3.Name = "button3";
            button3.Size = new Size(286, 29);
            button3.TabIndex = 42;
            button3.Text = "currentImage.Save.bmp";
            button3.UseVisualStyleBackColor = true;
            button3.Click += CurrentImagSaveButton_Click;
            // 
            // ImageComboBox
            // 
            ImageComboBox.FormattingEnabled = true;
            ImageComboBox.Location = new Point(160, 8);
            ImageComboBox.Margin = new Padding(4);
            ImageComboBox.Name = "ImageComboBox";
            ImageComboBox.Size = new Size(39, 23);
            ImageComboBox.TabIndex = 1;
            ImageComboBox.Text = "0";
            ImageComboBox.SelectedIndexChanged += ImageComboBox_SelectedIndexChanged;
            // 
            // LoadImageFIleButton
            // 
            LoadImageFIleButton.Location = new Point(4, 79);
            LoadImageFIleButton.Margin = new Padding(4);
            LoadImageFIleButton.Name = "LoadImageFIleButton";
            LoadImageFIleButton.Size = new Size(286, 29);
            LoadImageFIleButton.TabIndex = 2;
            LoadImageFIleButton.Text = "LoadImageFIleButton";
            LoadImageFIleButton.UseVisualStyleBackColor = true;
            LoadImageFIleButton.Click += LoadImageFIleButton_Click;
            // 
            // PrintRawLoadButton
            // 
            PrintRawLoadButton.Location = new Point(4, 4);
            PrintRawLoadButton.Margin = new Padding(4);
            PrintRawLoadButton.Name = "PrintRawLoadButton";
            PrintRawLoadButton.Size = new Size(149, 29);
            PrintRawLoadButton.TabIndex = 0;
            PrintRawLoadButton.Text = "PrintRawLoadButton";
            PrintRawLoadButton.UseVisualStyleBackColor = true;
            PrintRawLoadButton.Click += PrintRawLoadButton_Click;
            // 
            // panel15
            // 
            panel15.Controls.Add(pictureBox1);
            panel15.Controls.Add(LogWindow_textBox);
            panel15.Controls.Add(imagePictureBox);
            panel15.Dock = DockStyle.Fill;
            panel15.Location = new Point(0, 0);
            panel15.Margin = new Padding(4);
            panel15.Name = "panel15";
            panel15.Size = new Size(969, 872);
            panel15.TabIndex = 4;
            // 
            // imagePictureBox
            // 
            imagePictureBox.Location = new Point(4, 8);
            imagePictureBox.Margin = new Padding(4);
            imagePictureBox.Name = "imagePictureBox";
            imagePictureBox.Size = new Size(285, 480);
            imagePictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            imagePictureBox.TabIndex = 3;
            imagePictureBox.TabStop = false;
            // 
            // Tab02_Drawing1_UserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer3);
            Margin = new Padding(4);
            Name = "Tab02_Drawing1_UserControl";
            Size = new Size(1367, 872);
            Load += TabControl1_Load;
            VisibleChanged += TabControl1_VisibleChanged;
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)imagePictureBox).EndInit();
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button LoadImageFile;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button MoveImageMili_button;
        private System.Windows.Forms.Button RotateButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox PixSizeTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox BPPtextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PaperSizetextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ResTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox DXtextBox;
        private System.Windows.Forms.TextBox DYtextBox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox ImageComboBox;
        private System.Windows.Forms.Button LoadImageFIleButton;
        private System.Windows.Forms.Button PrintRawLoadButton;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.PictureBox imagePictureBox;
        private System.Windows.Forms.TextBox FormatTextbox;
        private System.Windows.Forms.Button button7;
        private PictureBox pictureBox1;
    }
}
