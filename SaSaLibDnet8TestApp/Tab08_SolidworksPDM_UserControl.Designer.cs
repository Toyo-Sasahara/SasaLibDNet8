
namespace SasaLibTestApp
{
    partial class Tab08_SolidworksPDM_UserControl
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
            this.PDMSEARCH_button = new System.Windows.Forms.Button();
            this.SEARCH_Text_textBox = new System.Windows.Forms.TextBox();
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
            this.LogWindow_textBox.Location = new System.Drawing.Point(16, 48);
            this.LogWindow_textBox.Multiline = true;
            this.LogWindow_textBox.Name = "LogWindow_textBox";
            this.LogWindow_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogWindow_textBox.Size = new System.Drawing.Size(1017, 491);
            this.LogWindow_textBox.TabIndex = 102;
            // 
            // PDMSEARCH_button
            // 
            this.PDMSEARCH_button.Location = new System.Drawing.Point(408, 13);
            this.PDMSEARCH_button.Name = "PDMSEARCH_button";
            this.PDMSEARCH_button.Size = new System.Drawing.Size(118, 23);
            this.PDMSEARCH_button.TabIndex = 104;
            this.PDMSEARCH_button.Text = "PDMｻｰﾊﾞｰを検索";
            this.PDMSEARCH_button.UseVisualStyleBackColor = true;
            this.PDMSEARCH_button.Click += new System.EventHandler(this.PDMSEARCH_button_Click);
            // 
            // SEARCH_Text_textBox
            // 
            this.SEARCH_Text_textBox.Location = new System.Drawing.Point(57, 15);
            this.SEARCH_Text_textBox.Name = "SEARCH_Text_textBox";
            this.SEARCH_Text_textBox.Size = new System.Drawing.Size(345, 19);
            this.SEARCH_Text_textBox.TabIndex = 103;
            this.SEARCH_Text_textBox.Text = "%.SLDPRT;%.SLDASM";
            // 
            // Tab08_SolidworksPDM_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PDMSEARCH_button);
            this.Controls.Add(this.SEARCH_Text_textBox);
            this.Controls.Add(this.LogWindow_textBox);
            this.Name = "Tab08_SolidworksPDM_UserControl";
            this.Size = new System.Drawing.Size(1046, 549);
            this.Load += new System.EventHandler(this.TabControl1_Load);
            this.VisibleChanged += new System.EventHandler(this.TabControl1_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.Button PDMSEARCH_button;
        private System.Windows.Forms.TextBox SEARCH_Text_textBox;
    }
}
