
namespace SaSaLibDNet8TestAPP
{
    partial class Tab11_SysConfigurator_UserControl
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
            button22 = new Button();
            button1 = new Button();
            button2 = new Button();
            checkBox1 = new CheckBox();
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
            LogWindow_textBox.Location = new Point(682, 4);
            LogWindow_textBox.Margin = new Padding(4);
            LogWindow_textBox.Multiline = true;
            LogWindow_textBox.Name = "LogWindow_textBox";
            LogWindow_textBox.ScrollBars = ScrollBars.Both;
            LogWindow_textBox.Size = new Size(522, 669);
            LogWindow_textBox.TabIndex = 102;
            // 
            // button22
            // 
            button22.Location = new Point(4, 4);
            button22.Margin = new Padding(4);
            button22.Name = "button22";
            button22.Size = new Size(191, 76);
            button22.TabIndex = 103;
            button22.Text = "button22";
            button22.UseVisualStyleBackColor = true;
            button22.Click += button22_Click;
            // 
            // button1
            // 
            button1.Location = new Point(53, 227);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 104;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(134, 227);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 105;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(134, 256);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(83, 19);
            checkBox1.TabIndex = 106;
            checkBox1.Text = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // Tab11_SysConfigurator_UserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(checkBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(button22);
            Controls.Add(LogWindow_textBox);
            Margin = new Padding(4);
            Name = "Tab11_SysConfigurator_UserControl";
            Size = new Size(1220, 686);
            Load += TabControl1_Load;
            VisibleChanged += TabControl1_VisibleChanged;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.Button button22;
        private Button button1;
        private Button button2;
        private CheckBox checkBox1;
    }
}
