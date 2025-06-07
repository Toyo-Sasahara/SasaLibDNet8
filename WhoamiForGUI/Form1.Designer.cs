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
            panel1 = new Panel();
            ExpandDownward_radioButton = new RadioButton();
            ExpandUpward_radioButton = new RadioButton();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblCount, lblInfo });
            statusStrip1.Location = new Point(0, 493);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(807, 22);
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
            tvGroups.Size = new Size(783, 449);
            tvGroups.TabIndex = 2;
            // 
            // txtUser
            // 
            txtUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtUser.Location = new Point(4, 467);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(160, 23);
            txtUser.TabIndex = 3;
            // 
            // btnLoad
            // 
            btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoad.Location = new Point(421, 464);
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
            GetTokenGroups_button.Location = new Point(579, 466);
            GetTokenGroups_button.Name = "GetTokenGroups_button";
            GetTokenGroups_button.Size = new Size(216, 23);
            GetTokenGroups_button.TabIndex = 5;
            GetTokenGroups_button.Text = " ログオン・トークンから所属グループ調査";
            GetTokenGroups_button.UseVisualStyleBackColor = true;
            GetTokenGroups_button.Click += GetTokenGroups_button_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            panel1.Controls.Add(ExpandDownward_radioButton);
            panel1.Controls.Add(ExpandUpward_radioButton);
            panel1.Location = new Point(170, 467);
            panel1.Name = "panel1";
            panel1.Size = new Size(245, 23);
            panel1.TabIndex = 6;
            // 
            // ExpandDownward_radioButton
            // 
            ExpandDownward_radioButton.AutoSize = true;
            ExpandDownward_radioButton.Location = new Point(115, 2);
            ExpandDownward_radioButton.Name = "ExpandDownward_radioButton";
            ExpandDownward_radioButton.Size = new Size(121, 19);
            ExpandDownward_radioButton.TabIndex = 1;
            ExpandDownward_radioButton.Text = "ExpandDwonward";
            ExpandDownward_radioButton.UseVisualStyleBackColor = true;
            // 
            // ExpandUpward_radioButton
            // 
            ExpandUpward_radioButton.AutoSize = true;
            ExpandUpward_radioButton.Checked = true;
            ExpandUpward_radioButton.Location = new Point(4, 2);
            ExpandUpward_radioButton.Name = "ExpandUpward_radioButton";
            ExpandUpward_radioButton.Size = new Size(105, 19);
            ExpandUpward_radioButton.TabIndex = 0;
            ExpandUpward_radioButton.TabStop = true;
            ExpandUpward_radioButton.Text = "ExpandUpward";
            ExpandUpward_radioButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(807, 515);
            Controls.Add(panel1);
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
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private Panel panel1;
        private RadioButton ExpandDownward_radioButton;
        private RadioButton ExpandUpward_radioButton;
    }
}
