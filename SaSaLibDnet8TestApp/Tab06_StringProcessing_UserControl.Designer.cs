
namespace SaSaLibTestApp
{
    partial class Tab06_StringProcessing_UserControl
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
            this.TestSplitButton = new System.Windows.Forms.Button();
            this.ResultTextBox = new System.Windows.Forms.TextBox();
            this.InputTextBox = new System.Windows.Forms.TextBox();
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
            this.LogWindow_textBox.Location = new System.Drawing.Point(585, 3);
            this.LogWindow_textBox.Multiline = true;
            this.LogWindow_textBox.Name = "LogWindow_textBox";
            this.LogWindow_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogWindow_textBox.Size = new System.Drawing.Size(448, 536);
            this.LogWindow_textBox.TabIndex = 102;
            // 
            // TestSplitButton
            // 
            this.TestSplitButton.Location = new System.Drawing.Point(183, 134);
            this.TestSplitButton.Name = "TestSplitButton";
            this.TestSplitButton.Size = new System.Drawing.Size(75, 23);
            this.TestSplitButton.TabIndex = 105;
            this.TestSplitButton.Text = "button11";
            this.TestSplitButton.UseVisualStyleBackColor = true;
            this.TestSplitButton.Click += new System.EventHandler(this.TestSplitButton_Click);
            // 
            // ResultTextBox
            // 
            this.ResultTextBox.Location = new System.Drawing.Point(28, 175);
            this.ResultTextBox.Multiline = true;
            this.ResultTextBox.Name = "ResultTextBox";
            this.ResultTextBox.Size = new System.Drawing.Size(371, 192);
            this.ResultTextBox.TabIndex = 104;
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(40, 103);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(348, 19);
            this.InputTextBox.TabIndex = 103;
            this.InputTextBox.Text = "吾輩は1匹の猫であるAbc吾輩は1匹の子犬である987";
            // 
            // TabControl04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TestSplitButton);
            this.Controls.Add(this.ResultTextBox);
            this.Controls.Add(this.InputTextBox);
            this.Controls.Add(this.LogWindow_textBox);
            this.Name = "TabControl04";
            this.Size = new System.Drawing.Size(1046, 549);
            this.Load += new System.EventHandler(this.TabControl1_Load);
            this.VisibleChanged += new System.EventHandler(this.TabControl1_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.Button TestSplitButton;
        private System.Windows.Forms.TextBox ResultTextBox;
        private System.Windows.Forms.TextBox InputTextBox;
    }
}
