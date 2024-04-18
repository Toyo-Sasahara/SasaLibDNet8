namespace SasaLib.ArcSuitePreview
{
    partial class TextDrawForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.drawString_textBox = new System.Windows.Forms.TextBox();
            this.FontSize_numericUpDown = new System.Windows.Forms.NumericUpDown();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontSize_numericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.FontSize_numericUpDown);
            this.panel1.Controls.Add(this.drawString_textBox);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(944, 44);
            this.panel1.TabIndex = 1;
            // 
            // drawString_textBox
            // 
            this.drawString_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drawString_textBox.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.drawString_textBox.Location = new System.Drawing.Point(3, 3);
            this.drawString_textBox.Name = "drawString_textBox";
            this.drawString_textBox.Size = new System.Drawing.Size(869, 31);
            this.drawString_textBox.TabIndex = 1;
            // 
            // FontSize_numericUpDown
            // 
            this.FontSize_numericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FontSize_numericUpDown.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FontSize_numericUpDown.Location = new System.Drawing.Point(878, 3);
            this.FontSize_numericUpDown.Maximum = new decimal(new int[] {
            72,
            0,
            0,
            0});
            this.FontSize_numericUpDown.Minimum = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.FontSize_numericUpDown.Name = "FontSize_numericUpDown";
            this.FontSize_numericUpDown.Size = new System.Drawing.Size(63, 31);
            this.FontSize_numericUpDown.TabIndex = 2;
            this.FontSize_numericUpDown.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // TextDrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 68);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextDrawForm";
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文字列描画";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextDrawForm_FormClosing);
            this.Shown += new System.EventHandler(this.TextDrawForm_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FontSize_numericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.TextBox drawString_textBox;
        internal System.Windows.Forms.NumericUpDown FontSize_numericUpDown;
    }
}