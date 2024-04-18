
namespace SasaLibTestApp
{
    partial class Tab14_FileSystemWathcer_UserControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ClearLog_button = new System.Windows.Forms.Button();
            this.Folder_textBox = new System.Windows.Forms.TextBox();
            this.Stop_button = new System.Windows.Forms.Button();
            this.Start_button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SourceFolder_textBox = new System.Windows.Forms.TextBox();
            this.Start_Sync_button = new System.Windows.Forms.Button();
            this.DistFolder_textBox = new System.Windows.Forms.TextBox();
            this.Stop_Sync_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // LogWindow_textBox
            // 
            this.LogWindow_textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LogWindow_textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogWindow_textBox.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LogWindow_textBox.Location = new System.Drawing.Point(0, 0);
            this.LogWindow_textBox.Multiline = true;
            this.LogWindow_textBox.Name = "LogWindow_textBox";
            this.LogWindow_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogWindow_textBox.Size = new System.Drawing.Size(665, 549);
            this.LogWindow_textBox.TabIndex = 102;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.ClearLog_button);
            this.splitContainer1.Panel1.Controls.Add(this.Folder_textBox);
            this.splitContainer1.Panel1.Controls.Add(this.Stop_button);
            this.splitContainer1.Panel1.Controls.Add(this.Start_button);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.LogWindow_textBox);
            this.splitContainer1.Size = new System.Drawing.Size(1046, 549);
            this.splitContainer1.SplitterDistance = 371;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 103;
            // 
            // ClearLog_button
            // 
            this.ClearLog_button.Location = new System.Drawing.Point(28, 231);
            this.ClearLog_button.Name = "ClearLog_button";
            this.ClearLog_button.Size = new System.Drawing.Size(75, 23);
            this.ClearLog_button.TabIndex = 4;
            this.ClearLog_button.Text = "ログ消去";
            this.ClearLog_button.UseVisualStyleBackColor = true;
            this.ClearLog_button.Click += new System.EventHandler(this.ClearLog_button_Click);
            // 
            // Folder_textBox
            // 
            this.Folder_textBox.Location = new System.Drawing.Point(109, 175);
            this.Folder_textBox.Name = "Folder_textBox";
            this.Folder_textBox.Size = new System.Drawing.Size(235, 19);
            this.Folder_textBox.TabIndex = 3;
            this.Folder_textBox.Text = "\\\\AD.LOCAL\\技術部共有";
            // 
            // Stop_button
            // 
            this.Stop_button.Location = new System.Drawing.Point(28, 202);
            this.Stop_button.Name = "Stop_button";
            this.Stop_button.Size = new System.Drawing.Size(75, 23);
            this.Stop_button.TabIndex = 2;
            this.Stop_button.Text = "Stop";
            this.Stop_button.UseVisualStyleBackColor = true;
            this.Stop_button.Click += new System.EventHandler(this.Stop_button_Click);
            // 
            // Start_button
            // 
            this.Start_button.Location = new System.Drawing.Point(28, 173);
            this.Start_button.Name = "Start_button";
            this.Start_button.Size = new System.Drawing.Size(75, 23);
            this.Start_button.TabIndex = 1;
            this.Start_button.Text = "Start";
            this.Start_button.UseVisualStyleBackColor = true;
            this.Start_button.Click += new System.EventHandler(this.Start_button_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Stop_Sync_button);
            this.panel1.Controls.Add(this.DistFolder_textBox);
            this.panel1.Controls.Add(this.SourceFolder_textBox);
            this.panel1.Controls.Add(this.Start_Sync_button);
            this.panel1.Location = new System.Drawing.Point(13, 316);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(355, 163);
            this.panel1.TabIndex = 5;
            // 
            // SourceFolder_textBox
            // 
            this.SourceFolder_textBox.Location = new System.Drawing.Point(15, 15);
            this.SourceFolder_textBox.Name = "SourceFolder_textBox";
            this.SourceFolder_textBox.Size = new System.Drawing.Size(316, 19);
            this.SourceFolder_textBox.TabIndex = 5;
            // 
            // Start_Sync_button
            // 
            this.Start_Sync_button.Location = new System.Drawing.Point(96, 119);
            this.Start_Sync_button.Name = "Start_Sync_button";
            this.Start_Sync_button.Size = new System.Drawing.Size(75, 23);
            this.Start_Sync_button.TabIndex = 4;
            this.Start_Sync_button.Text = "Start";
            this.Start_Sync_button.UseVisualStyleBackColor = true;
            this.Start_Sync_button.Click += new System.EventHandler(this.Start_Sync_button_Click);
            // 
            // DistFolder_textBox
            // 
            this.DistFolder_textBox.Location = new System.Drawing.Point(15, 72);
            this.DistFolder_textBox.Name = "DistFolder_textBox";
            this.DistFolder_textBox.Size = new System.Drawing.Size(316, 19);
            this.DistFolder_textBox.TabIndex = 6;
            // 
            // Stop_Sync_button
            // 
            this.Stop_Sync_button.Location = new System.Drawing.Point(186, 119);
            this.Stop_Sync_button.Name = "Stop_Sync_button";
            this.Stop_Sync_button.Size = new System.Drawing.Size(75, 23);
            this.Stop_Sync_button.TabIndex = 7;
            this.Stop_Sync_button.Text = "Start";
            this.Stop_Sync_button.UseVisualStyleBackColor = true;
            this.Stop_Sync_button.Click += new System.EventHandler(this.Stop_Sync_button_Click);
            // 
            // Tab14_FileSystemWathcer_UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "Tab14_FileSystemWathcer_UserControl";
            this.Size = new System.Drawing.Size(1046, 549);
            this.Load += new System.EventHandler(this.Tab14_FileSystemWathcer_Load);
            this.VisibleChanged += new System.EventHandler(this.Tab14_FileSystemWathcer_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox Folder_textBox;
        private System.Windows.Forms.Button Stop_button;
        private System.Windows.Forms.Button Start_button;
        private System.Windows.Forms.Button ClearLog_button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox DistFolder_textBox;
        private System.Windows.Forms.TextBox SourceFolder_textBox;
        private System.Windows.Forms.Button Start_Sync_button;
        private System.Windows.Forms.Button Stop_Sync_button;
    }
}
