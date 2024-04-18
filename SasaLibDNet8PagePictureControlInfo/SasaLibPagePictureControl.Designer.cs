namespace SasaLibPictureBoxControlLibrary
{
    partial class SasaLibPagePictureControl
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
            this.Page_textBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.NavigatePanel = new System.Windows.Forms.Panel();
            this.Next_button = new System.Windows.Forms.Button();
            this.Reverce_button = new System.Windows.Forms.Button();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.Buttom_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.NavigatePanel.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Page_textBox
            // 
            this.Page_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Page_textBox.Location = new System.Drawing.Point(31, 6);
            this.Page_textBox.Name = "Page_textBox";
            this.Page_textBox.Size = new System.Drawing.Size(57, 19);
            this.Page_textBox.TabIndex = 1;
            this.Page_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Page_textBox_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(57, 53);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // NavigatePanel
            // 
            this.NavigatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NavigatePanel.AutoScroll = true;
            this.NavigatePanel.Controls.Add(this.Page_textBox);
            this.NavigatePanel.Controls.Add(this.Next_button);
            this.NavigatePanel.Controls.Add(this.Reverce_button);
            this.NavigatePanel.Location = new System.Drawing.Point(163, 422);
            this.NavigatePanel.Name = "NavigatePanel";
            this.NavigatePanel.Size = new System.Drawing.Size(120, 32);
            this.NavigatePanel.TabIndex = 5;
            // 
            // Next_button
            // 
            this.Next_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Next_button.Location = new System.Drawing.Point(94, 4);
            this.Next_button.Name = "Next_button";
            this.Next_button.Size = new System.Drawing.Size(21, 23);
            this.Next_button.TabIndex = 1;
            this.Next_button.Text = ">";
            this.Next_button.UseVisualStyleBackColor = true;
            this.Next_button.Click += new System.EventHandler(this.Next_button_Click);
            // 
            // Reverce_button
            // 
            this.Reverce_button.Location = new System.Drawing.Point(4, 4);
            this.Reverce_button.Name = "Reverce_button";
            this.Reverce_button.Size = new System.Drawing.Size(21, 23);
            this.Reverce_button.TabIndex = 0;
            this.Reverce_button.Text = "<";
            this.Reverce_button.UseVisualStyleBackColor = true;
            this.Reverce_button.Click += new System.EventHandler(this.Reverce_button_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainPanel.Controls.Add(this.pictureBox1);
            this.MainPanel.Location = new System.Drawing.Point(3, 3);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(359, 413);
            this.MainPanel.TabIndex = 6;
            // 
            // Buttom_label
            // 
            this.Buttom_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Buttom_label.AutoSize = true;
            this.Buttom_label.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Buttom_label.Location = new System.Drawing.Point(13, 430);
            this.Buttom_label.Name = "Buttom_label";
            this.Buttom_label.Size = new System.Drawing.Size(48, 16);
            this.Buttom_label.TabIndex = 7;
            this.Buttom_label.Text = "-----";
            // 
            // SasaLibPagePictureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.Buttom_label);
            this.Controls.Add(this.NavigatePanel);
            this.Controls.Add(this.MainPanel);
            this.Name = "SasaLibPagePictureControl";
            this.Size = new System.Drawing.Size(363, 464);
            this.SizeChanged += new System.EventHandler(this.MultiPictureControl_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.NavigatePanel.ResumeLayout(false);
            this.NavigatePanel.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Page_textBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel NavigatePanel;
        private System.Windows.Forms.Button Next_button;
        private System.Windows.Forms.Button Reverce_button;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Label Buttom_label;
    }
}
