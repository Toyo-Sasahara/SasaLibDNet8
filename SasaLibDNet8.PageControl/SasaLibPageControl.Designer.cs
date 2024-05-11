using System.ComponentModel;
using System.ComponentModel.Design;

namespace SasaLib
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    partial class SasaLibPageControl
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
            this.MainPanel = new System.Windows.Forms.Panel();
            this.PageNumberTextBox = new System.Windows.Forms.TextBox();
            this.XPlus_button = new System.Windows.Forms.Button();
            this.XMinus_button = new System.Windows.Forms.Button();
            this.YPlus_Button = new System.Windows.Forms.Button();
            this.YMinus_Button = new System.Windows.Forms.Button();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.PageNumberTextBox);
            this.MainPanel.Controls.Add(this.XPlus_button);
            this.MainPanel.Controls.Add(this.XMinus_button);
            this.MainPanel.Controls.Add(this.YPlus_Button);
            this.MainPanel.Controls.Add(this.YMinus_Button);
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(220, 40);
            this.MainPanel.TabIndex = 5;
            // 
            // PageNumberTextBox
            // 
            this.PageNumberTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PageNumberTextBox.Location = new System.Drawing.Point(37, 12);
            this.PageNumberTextBox.Name = "PageNumberTextBox";
            this.PageNumberTextBox.ReadOnly = true;
            this.PageNumberTextBox.Size = new System.Drawing.Size(66, 19);
            this.PageNumberTextBox.TabIndex = 0;
            this.PageNumberTextBox.Text = "00 / 00";
            this.PageNumberTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // XPlus_button
            // 
            this.XPlus_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.XPlus_button.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.XPlus_button.Location = new System.Drawing.Point(109, 12);
            this.XPlus_button.Name = "XPlus_button";
            this.XPlus_button.Size = new System.Drawing.Size(23, 18);
            this.XPlus_button.TabIndex = 3;
            this.XPlus_button.Text = ">";
            this.XPlus_button.UseVisualStyleBackColor = true;
            // 
            // XMinus_button
            // 
            this.XMinus_button.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.XMinus_button.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.XMinus_button.Location = new System.Drawing.Point(8, 12);
            this.XMinus_button.Name = "XMinus_button";
            this.XMinus_button.Size = new System.Drawing.Size(23, 18);
            this.XMinus_button.TabIndex = 4;
            this.XMinus_button.Text = "<";
            this.XMinus_button.UseVisualStyleBackColor = true;
            // 
            // YPlus_Button
            // 
            this.YPlus_Button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.YPlus_Button.Location = new System.Drawing.Point(138, 2);
            this.YPlus_Button.Name = "YPlus_Button";
            this.YPlus_Button.Size = new System.Drawing.Size(78, 19);
            this.YPlus_Button.TabIndex = 5;
            this.YPlus_Button.Text = "↑";
            this.YPlus_Button.UseVisualStyleBackColor = true;
            // 
            // YMinus_Button
            // 
            this.YMinus_Button.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.YMinus_Button.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.YMinus_Button.Location = new System.Drawing.Point(138, 20);
            this.YMinus_Button.Name = "YMinus_Button";
            this.YMinus_Button.Size = new System.Drawing.Size(78, 19);
            this.YMinus_Button.TabIndex = 6;
            this.YMinus_Button.Text = "↓";
            this.YMinus_Button.UseVisualStyleBackColor = true;
            // 
            // SasaPageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainPanel);
            this.Name = "SasaPageControl";
            this.Size = new System.Drawing.Size(220, 40);
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        public System.Windows.Forms.Button YMinus_Button;
        public System.Windows.Forms.Button YPlus_Button;
        public System.Windows.Forms.Button XPlus_button;
        public System.Windows.Forms.Button XMinus_button;
        public System.Windows.Forms.TextBox PageNumberTextBox;
    }
}
