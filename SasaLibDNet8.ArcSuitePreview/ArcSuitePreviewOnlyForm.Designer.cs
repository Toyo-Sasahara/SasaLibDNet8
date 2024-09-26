
namespace SasaLib.ArcSuitePreview
{
    partial class ArcSuitePreviewOnlyForm
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
            this.components = new System.ComponentModel.Container();
            this.PreviewPanel = new Panel();
            this.ArcSuite_Status_label = new Label();
            this.Debug_panel = new Panel();
            this.FindTimeStamp_label = new Label();
            this.SCALE_numericUpDown = new NumericUpDown();
            this.Y_numericUpDown = new NumericUpDown();
            this.X_numericUpDown = new NumericUpDown();
            this.button1 = new Button();
            this.label2 = new Label();
            this.lblDst = new Label();
            this.label1 = new Label();
            this.ArcsuitePreviewForm_Msg_label = new Label();
            this.ArcSuitePreviewPictureBox = new PictureBox();
            this.groupBox1 = new GroupBox();
            this.DrawingInfoLabel3 = new Label();
            this.DrawingInfoLabel2 = new Label();
            this.modelcreationonorder_label = new Label();
            this.DrawingInfoLabel4 = new Label();
            this.ArcSuiteCreatedOn_label = new Label();
            this.TitleBlockScale_button = new Button();
            this.Hide_button = new Button();
            this.ArcSuiteWebSearchAndView_button = new Button();
            this.toolTip1 = new ToolTip(this.components);
            this.ClipBoardTextSearch_button = new Button();
            this.PreviewPanel.SuspendLayout();
            this.Debug_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.SCALE_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.Y_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.X_numericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.ArcSuitePreviewPictureBox).BeginInit();
            this.groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // PreviewPanel
            // 
            this.PreviewPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.PreviewPanel.Controls.Add(this.ArcSuite_Status_label);
            this.PreviewPanel.Controls.Add(this.Debug_panel);
            this.PreviewPanel.Controls.Add(this.ArcsuitePreviewForm_Msg_label);
            this.PreviewPanel.Controls.Add(this.ArcSuitePreviewPictureBox);
            this.PreviewPanel.Location = new Point(10, 11);
            this.PreviewPanel.Margin = new Padding(0);
            this.PreviewPanel.Name = "PreviewPanel";
            this.PreviewPanel.Size = new Size(590, 422);
            this.PreviewPanel.TabIndex = 12;
            // 
            // ArcSuite_Status_label
            // 
            this.ArcSuite_Status_label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ArcSuite_Status_label.BackColor = Color.Transparent;
            this.ArcSuite_Status_label.Enabled = false;
            this.ArcSuite_Status_label.Font = new Font("Microsoft Sans Serif", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.ArcSuite_Status_label.ForeColor = Color.Red;
            this.ArcSuite_Status_label.Location = new Point(76, 279);
            this.ArcSuite_Status_label.Margin = new Padding(4, 0, 4, 0);
            this.ArcSuite_Status_label.Name = "ArcSuite_Status_label";
            this.ArcSuite_Status_label.Size = new Size(470, 80);
            this.ArcSuite_Status_label.TabIndex = 2;
            this.ArcSuite_Status_label.Text = "...";
            this.ArcSuite_Status_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Debug_panel
            // 
            this.Debug_panel.Controls.Add(this.FindTimeStamp_label);
            this.Debug_panel.Controls.Add(this.SCALE_numericUpDown);
            this.Debug_panel.Controls.Add(this.Y_numericUpDown);
            this.Debug_panel.Controls.Add(this.X_numericUpDown);
            this.Debug_panel.Controls.Add(this.button1);
            this.Debug_panel.Controls.Add(this.label2);
            this.Debug_panel.Controls.Add(this.lblDst);
            this.Debug_panel.Controls.Add(this.label1);
            this.Debug_panel.Location = new Point(14, 142);
            this.Debug_panel.Margin = new Padding(4);
            this.Debug_panel.Name = "Debug_panel";
            this.Debug_panel.Size = new Size(467, 120);
            this.Debug_panel.TabIndex = 21;
            // 
            // FindTimeStamp_label
            // 
            this.FindTimeStamp_label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.FindTimeStamp_label.Font = new Font("MS UI Gothic", 9F);
            this.FindTimeStamp_label.Location = new Point(8, 91);
            this.FindTimeStamp_label.Margin = new Padding(4, 0, 4, 0);
            this.FindTimeStamp_label.Name = "FindTimeStamp_label";
            this.FindTimeStamp_label.Size = new Size(258, 24);
            this.FindTimeStamp_label.TabIndex = 34;
            this.FindTimeStamp_label.Text = "-";
            // 
            // SCALE_numericUpDown
            // 
            this.SCALE_numericUpDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.SCALE_numericUpDown.DecimalPlaces = 3;
            this.SCALE_numericUpDown.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            this.SCALE_numericUpDown.Location = new Point(401, 62);
            this.SCALE_numericUpDown.Margin = new Padding(4);
            this.SCALE_numericUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            this.SCALE_numericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 196608 });
            this.SCALE_numericUpDown.Name = "SCALE_numericUpDown";
            this.SCALE_numericUpDown.Size = new Size(62, 23);
            this.SCALE_numericUpDown.TabIndex = 33;
            this.SCALE_numericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // Y_numericUpDown
            // 
            this.Y_numericUpDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.Y_numericUpDown.Location = new Point(346, 62);
            this.Y_numericUpDown.Margin = new Padding(4);
            this.Y_numericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.Y_numericUpDown.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
            this.Y_numericUpDown.Name = "Y_numericUpDown";
            this.Y_numericUpDown.Size = new Size(48, 23);
            this.Y_numericUpDown.TabIndex = 32;
            // 
            // X_numericUpDown
            // 
            this.X_numericUpDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.X_numericUpDown.Location = new Point(289, 62);
            this.X_numericUpDown.Margin = new Padding(4);
            this.X_numericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            this.X_numericUpDown.Minimum = new decimal(new int[] { 10000, 0, 0, int.MinValue });
            this.X_numericUpDown.Name = "X_numericUpDown";
            this.X_numericUpDown.Size = new Size(48, 23);
            this.X_numericUpDown.TabIndex = 31;
            // 
            // button1
            // 
            this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.button1.BackColor = Color.Transparent;
            this.button1.Location = new Point(289, 88);
            this.button1.Margin = new Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new Size(174, 22);
            this.button1.TabIndex = 30;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.label2.Font = new Font("MS UI Gothic", 9F);
            this.label2.Location = new Point(7, 66);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(258, 24);
            this.label2.TabIndex = 29;
            this.label2.Text = "-";
            // 
            // lblDst
            // 
            this.lblDst.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.lblDst.Font = new Font("MS UI Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.lblDst.Location = new Point(8, 8);
            this.lblDst.Margin = new Padding(4, 0, 4, 0);
            this.lblDst.Name = "lblDst";
            this.lblDst.Size = new Size(455, 24);
            this.lblDst.TabIndex = 20;
            this.lblDst.Text = "ﾎｲｰﾙﾎﾞﾀﾝﾄﾞﾗｯｸﾞで移動。ﾎｲｰﾙ回転で拡縮. ";
            // 
            // label1
            // 
            this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.label1.Font = new Font("MS UI Gothic", 9F);
            this.label1.Location = new Point(7, 32);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(258, 24);
            this.label1.TabIndex = 28;
            this.label1.Text = "-";
            // 
            // ArcsuitePreviewForm_Msg_label
            // 
            this.ArcsuitePreviewForm_Msg_label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ArcsuitePreviewForm_Msg_label.BackColor = Color.Transparent;
            this.ArcsuitePreviewForm_Msg_label.Enabled = false;
            this.ArcsuitePreviewForm_Msg_label.Font = new Font("メイリオ", 18F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.ArcsuitePreviewForm_Msg_label.Location = new Point(14, 19);
            this.ArcsuitePreviewForm_Msg_label.Margin = new Padding(4, 0, 4, 0);
            this.ArcsuitePreviewForm_Msg_label.Name = "ArcsuitePreviewForm_Msg_label";
            this.ArcsuitePreviewForm_Msg_label.Size = new Size(561, 195);
            this.ArcsuitePreviewForm_Msg_label.TabIndex = 1;
            this.ArcsuitePreviewForm_Msg_label.Text = "しばらくお待ちください";
            this.ArcsuitePreviewForm_Msg_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ArcSuitePreviewPictureBox
            // 
            this.ArcSuitePreviewPictureBox.Dock = DockStyle.Fill;
            this.ArcSuitePreviewPictureBox.Location = new Point(0, 0);
            this.ArcSuitePreviewPictureBox.Margin = new Padding(4);
            this.ArcSuitePreviewPictureBox.Name = "ArcSuitePreviewPictureBox";
            this.ArcSuitePreviewPictureBox.Size = new Size(590, 422);
            this.ArcSuitePreviewPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.ArcSuitePreviewPictureBox.TabIndex = 0;
            this.ArcSuitePreviewPictureBox.TabStop = false;
            this.ArcSuitePreviewPictureBox.MouseDown += pictureBox1_MouseDown;
            this.ArcSuitePreviewPictureBox.MouseMove += pictureBox1_MouseMove;
            this.ArcSuitePreviewPictureBox.MouseUp += pictureBox1_MouseUp;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.groupBox1.Controls.Add(this.DrawingInfoLabel3);
            this.groupBox1.Controls.Add(this.DrawingInfoLabel2);
            this.groupBox1.Controls.Add(this.modelcreationonorder_label);
            this.groupBox1.Controls.Add(this.DrawingInfoLabel4);
            this.groupBox1.Controls.Add(this.ArcSuiteCreatedOn_label);
            this.groupBox1.Location = new Point(14, 438);
            this.groupBox1.Margin = new Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new Padding(4);
            this.groupBox1.Size = new Size(442, 174);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ｱｰｸｽｲｰﾄでの属性値";
            // 
            // DrawingInfoLabel3
            // 
            this.DrawingInfoLabel3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.DrawingInfoLabel3.Font = new Font("MS UI Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.DrawingInfoLabel3.Location = new Point(7, 84);
            this.DrawingInfoLabel3.Margin = new Padding(4, 0, 4, 0);
            this.DrawingInfoLabel3.Name = "DrawingInfoLabel3";
            this.DrawingInfoLabel3.Size = new Size(428, 22);
            this.DrawingInfoLabel3.TabIndex = 19;
            this.DrawingInfoLabel3.Text = "---";
            // 
            // DrawingInfoLabel2
            // 
            this.DrawingInfoLabel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.DrawingInfoLabel2.Font = new Font("MS UI Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.DrawingInfoLabel2.Location = new Point(7, 55);
            this.DrawingInfoLabel2.Margin = new Padding(4, 0, 4, 0);
            this.DrawingInfoLabel2.Name = "DrawingInfoLabel2";
            this.DrawingInfoLabel2.Size = new Size(428, 22);
            this.DrawingInfoLabel2.TabIndex = 16;
            this.DrawingInfoLabel2.Text = "---";
            // 
            // modelcreationonorder_label
            // 
            this.modelcreationonorder_label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.modelcreationonorder_label.Font = new Font("MS UI Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.modelcreationonorder_label.Location = new Point(7, 29);
            this.modelcreationonorder_label.Margin = new Padding(4, 0, 4, 0);
            this.modelcreationonorder_label.Name = "modelcreationonorder_label";
            this.modelcreationonorder_label.Size = new Size(428, 22);
            this.modelcreationonorder_label.TabIndex = 17;
            this.modelcreationonorder_label.Text = "---";
            // 
            // DrawingInfoLabel4
            // 
            this.DrawingInfoLabel4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.DrawingInfoLabel4.Font = new Font("MS UI Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.DrawingInfoLabel4.Location = new Point(7, 141);
            this.DrawingInfoLabel4.Margin = new Padding(4, 0, 4, 0);
            this.DrawingInfoLabel4.Name = "DrawingInfoLabel4";
            this.DrawingInfoLabel4.Size = new Size(428, 22);
            this.DrawingInfoLabel4.TabIndex = 18;
            this.DrawingInfoLabel4.Text = "---";
            // 
            // ArcSuiteCreatedOn_label
            // 
            this.ArcSuiteCreatedOn_label.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.ArcSuiteCreatedOn_label.Font = new Font("MS UI Gothic", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.ArcSuiteCreatedOn_label.Location = new Point(7, 112);
            this.ArcSuiteCreatedOn_label.Margin = new Padding(4, 0, 4, 0);
            this.ArcSuiteCreatedOn_label.Name = "ArcSuiteCreatedOn_label";
            this.ArcSuiteCreatedOn_label.Size = new Size(428, 22);
            this.ArcSuiteCreatedOn_label.TabIndex = 21;
            this.ArcSuiteCreatedOn_label.Text = "---";
            // 
            // TitleBlockScale_button
            // 
            this.TitleBlockScale_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.TitleBlockScale_button.Location = new Point(486, 441);
            this.TitleBlockScale_button.Margin = new Padding(4);
            this.TitleBlockScale_button.Name = "TitleBlockScale_button";
            this.TitleBlockScale_button.Size = new Size(114, 35);
            this.TitleBlockScale_button.TabIndex = 25;
            this.TitleBlockScale_button.Text = "右下部拡大";
            this.TitleBlockScale_button.UseVisualStyleBackColor = true;
            this.TitleBlockScale_button.Click += TitleBlockScale_button_Click;
            // 
            // Hide_button
            // 
            this.Hide_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.Hide_button.Location = new Point(486, 579);
            this.Hide_button.Margin = new Padding(4);
            this.Hide_button.Name = "Hide_button";
            this.Hide_button.Size = new Size(114, 35);
            this.Hide_button.TabIndex = 26;
            this.Hide_button.Text = "閉じる";
            this.Hide_button.UseVisualStyleBackColor = true;
            this.Hide_button.Click += Hide_button_Click;
            // 
            // ArcSuiteWebSearchAndView_button
            // 
            this.ArcSuiteWebSearchAndView_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.ArcSuiteWebSearchAndView_button.Location = new Point(486, 492);
            this.ArcSuiteWebSearchAndView_button.Margin = new Padding(4);
            this.ArcSuiteWebSearchAndView_button.Name = "ArcSuiteWebSearchAndView_button";
            this.ArcSuiteWebSearchAndView_button.Size = new Size(114, 35);
            this.ArcSuiteWebSearchAndView_button.TabIndex = 27;
            this.ArcSuiteWebSearchAndView_button.Text = "Web版で再検索";
            this.ArcSuiteWebSearchAndView_button.UseVisualStyleBackColor = true;
            this.ArcSuiteWebSearchAndView_button.Click += ArcSuiteWebSearchAndView_button_Click;
            // 
            // ClipBoardTextSearch_button
            // 
            this.ClipBoardTextSearch_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.ClipBoardTextSearch_button.Font = new Font("MS UI Gothic", 9F);
            this.ClipBoardTextSearch_button.Location = new Point(486, 531);
            this.ClipBoardTextSearch_button.Margin = new Padding(4);
            this.ClipBoardTextSearch_button.Name = "ClipBoardTextSearch_button";
            this.ClipBoardTextSearch_button.Size = new Size(114, 41);
            this.ClipBoardTextSearch_button.TabIndex = 29;
            this.ClipBoardTextSearch_button.Text = "ｸﾘｯﾌﾟﾎﾞｰﾄﾞ\r\n文字列を検索";
            this.ClipBoardTextSearch_button.UseVisualStyleBackColor = false;
            this.ClipBoardTextSearch_button.Click += ClipBoardTextSearch_button_Click;
            // 
            // ArcSuitePreviewOnlyForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(611, 626);
            ControlBox = false;
            Controls.Add(this.ClipBoardTextSearch_button);
            Controls.Add(this.ArcSuiteWebSearchAndView_button);
            Controls.Add(this.Hide_button);
            Controls.Add(this.TitleBlockScale_button);
            Controls.Add(this.groupBox1);
            Controls.Add(this.PreviewPanel);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Margin = new Padding(4);
            Name = "ArcSuitePreviewOnlyForm";
            Text = "■東陽ﾂｰﾙ ArcSuite登録済み図面";
            FormClosing += ArcSuitePreviewForm_FormClosing;
            Load += ArcSuitePreviewForm_Load;
            Shown += ArcSuitePreviewForm_Shown;
            this.PreviewPanel.ResumeLayout(false);
            this.Debug_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.SCALE_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.Y_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.X_numericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.ArcSuitePreviewPictureBox).EndInit();
            this.groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel PreviewPanel;
        private System.Windows.Forms.Label ArcsuitePreviewForm_Msg_label;
        public System.Windows.Forms.Label ArcSuite_Status_label;
        private System.Windows.Forms.PictureBox ArcSuitePreviewPictureBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label DrawingInfoLabel3;
        private System.Windows.Forms.Label DrawingInfoLabel2;
        private System.Windows.Forms.Label modelcreationonorder_label;
        private System.Windows.Forms.Label DrawingInfoLabel4;
        private System.Windows.Forms.Label ArcSuiteCreatedOn_label;
        private System.Windows.Forms.Button TitleBlockScale_button;
        private System.Windows.Forms.Button Hide_button;
        private System.Windows.Forms.Button ArcSuiteWebSearchAndView_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDst;
        private System.Windows.Forms.Panel Debug_panel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown SCALE_numericUpDown;
        private System.Windows.Forms.NumericUpDown Y_numericUpDown;
        private System.Windows.Forms.NumericUpDown X_numericUpDown;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label FindTimeStamp_label;
        private System.Windows.Forms.Button ClipBoardTextSearch_button;
    }
}