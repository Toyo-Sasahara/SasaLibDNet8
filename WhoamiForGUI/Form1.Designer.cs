namespace WhoamiForGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            statusStrip1 = new StatusStrip();
            lblCount = new ToolStripStatusLabel();
            lblInfo = new ToolStripStatusLabel();
            tvGroups = new TreeView();
            txtUser = new TextBox();
            btnLoad = new Button();
            GetTokenGroups_button = new Button();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblCount, lblInfo });
            statusStrip1.Location = new Point(0, 519);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(706, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblCount
            // 
            lblCount.Name = "lblCount";
            lblCount.Size = new Size(118, 17);
            lblCount.Text = "toolStripStatusLabel1";
            // 
            // lblInfo
            // 
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(118, 17);
            lblInfo.Text = "toolStripStatusLabel1";
            // 
            // tvGroups
            // 
            tvGroups.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tvGroups.Location = new Point(12, 12);
            tvGroups.Name = "tvGroups";
            tvGroups.Size = new Size(682, 475);
            tvGroups.TabIndex = 2;
            // 
            // txtUser
            // 
            txtUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtUser.Location = new Point(4, 493);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(160, 23);
            txtUser.TabIndex = 3;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoad.Location = new Point(170, 493);
            btnLoad.Name = "btnLoad";
            btnLoad.Size = new Size(85, 23);
            btnLoad.TabIndex = 4;
            btnLoad.Text = "読み込み";
            btnLoad.UseVisualStyleBackColor = true;
            btnLoad.Click += BtnLoad_Click;
            // 
            // GetTokenGroups_button
            // 
            GetTokenGroups_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            GetTokenGroups_button.Location = new Point(478, 492);
            GetTokenGroups_button.Name = "GetTokenGroups_button";
            GetTokenGroups_button.Size = new Size(216, 23);
            GetTokenGroups_button.TabIndex = 5;
            GetTokenGroups_button.Text = " ログオン・トークンから所属グループ調査";
            GetTokenGroups_button.UseVisualStyleBackColor = true;
            GetTokenGroups_button.Click += GetTokenGroups_button_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(706, 541);
            Controls.Add(GetTokenGroups_button);
            Controls.Add(btnLoad);
            Controls.Add(txtUser);
            Controls.Add(tvGroups);
            Controls.Add(statusStrip1);
            Name = "Form1";
            Text = "Form1";
            Load += MainForm_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblCount;
        private TreeView tvGroups;
        private TextBox txtUser;
        private Button btnLoad;
        private ToolStripStatusLabel lblInfo;
        private Button GetTokenGroups_button;
    }
}
