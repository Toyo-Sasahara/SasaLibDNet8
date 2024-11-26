namespace SaSaLibDNet8TestAPP
{
    partial class Tab15_Networks_UserControl
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
            this.button1 = new Button();
            this.textBox1 = new TextBox();
            this.groupBox1 = new GroupBox();
            this.groupBox2 = new GroupBox();
            this.GetLog_textbox = new Button();
            this.LogWindow_textBox = new TextBox();
            this.SEndMail_button = new Button();
            this.panel8 = new Panel();
            this.label8 = new Label();
            this.subject_textBox = new TextBox();
            this.panel7 = new Panel();
            this.label7 = new Label();
            this.Message_textbox = new TextBox();
            this.panel6 = new Panel();
            this.label6 = new Label();
            this.Password_textBox = new TextBox();
            this.panel5 = new Panel();
            this.label5 = new Label();
            this.UserName_textBox = new TextBox();
            this.panel4 = new Panel();
            this.label4 = new Label();
            this.SendToAddr_textBox = new TextBox();
            this.panel3 = new Panel();
            this.label3 = new Label();
            this.FromAddr_textBox = new TextBox();
            this.panel2 = new Panel();
            this.label2 = new Label();
            this.port_textBox = new TextBox();
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.ServerAddr_textBox = new TextBox();
            this.DhcpClientTest_button = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.button1.Location = new Point(672, 22);
            this.button1.Margin = new Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new Size(88, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += button1_Click;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.textBox1.Location = new Point(7, 22);
            this.textBox1.Margin = new Padding(4, 4, 4, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(86, 23);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "8.8.8.8";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new Point(4, 4);
            this.groupBox1.Margin = new Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new Padding(4, 4, 4, 4);
            this.groupBox1.Size = new Size(768, 64);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DhcpClientTest_button);
            this.groupBox2.Controls.Add(this.GetLog_textbox);
            this.groupBox2.Controls.Add(this.LogWindow_textBox);
            this.groupBox2.Controls.Add(this.SEndMail_button);
            this.groupBox2.Controls.Add(this.panel8);
            this.groupBox2.Controls.Add(this.panel7);
            this.groupBox2.Controls.Add(this.panel6);
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Location = new Point(4, 75);
            this.groupBox2.Margin = new Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new Padding(4, 4, 4, 4);
            this.groupBox2.Size = new Size(681, 556);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // GetLog_textbox
            // 
            this.GetLog_textbox.Anchor = AnchorStyles.Left;
            this.GetLog_textbox.Location = new Point(13, 524);
            this.GetLog_textbox.Margin = new Padding(4, 4, 4, 4);
            this.GetLog_textbox.Name = "GetLog_textbox";
            this.GetLog_textbox.Size = new Size(166, 25);
            this.GetLog_textbox.TabIndex = 104;
            this.GetLog_textbox.Text = "送信履歴取得";
            this.GetLog_textbox.UseVisualStyleBackColor = true;
            this.GetLog_textbox.Click += GetLog_textbox_Click;
            // 
            // LogWindow_textBox
            // 
            this.LogWindow_textBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.LogWindow_textBox.Font = new Font("MS UI Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
            this.LogWindow_textBox.Location = new Point(20, 342);
            this.LogWindow_textBox.Margin = new Padding(4, 4, 4, 4);
            this.LogWindow_textBox.Multiline = true;
            this.LogWindow_textBox.Name = "LogWindow_textBox";
            this.LogWindow_textBox.ScrollBars = ScrollBars.Both;
            this.LogWindow_textBox.Size = new Size(510, 173);
            this.LogWindow_textBox.TabIndex = 103;
            // 
            // SEndMail_button
            // 
            this.SEndMail_button.Location = new Point(334, 306);
            this.SEndMail_button.Margin = new Padding(4, 4, 4, 4);
            this.SEndMail_button.Name = "SEndMail_button";
            this.SEndMail_button.Size = new Size(181, 29);
            this.SEndMail_button.TabIndex = 3;
            this.SEndMail_button.Text = "メッセージ送信";
            this.SEndMail_button.UseVisualStyleBackColor = true;
            this.SEndMail_button.Click += SEndMail_button_Click;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label8);
            this.panel8.Controls.Add(this.subject_textBox);
            this.panel8.Location = new Point(7, 155);
            this.panel8.Margin = new Padding(4, 4, 4, 4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new Size(511, 36);
            this.panel8.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new Point(4, 11);
            this.label8.Margin = new Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new Size(62, 15);
            this.label8.TabIndex = 4;
            this.label8.Text = "FromAddr:";
            // 
            // subject_textBox
            // 
            this.subject_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.subject_textBox.Location = new Point(85, 8);
            this.subject_textBox.Margin = new Padding(4, 4, 4, 4);
            this.subject_textBox.Name = "subject_textBox";
            this.subject_textBox.Size = new Size(422, 23);
            this.subject_textBox.TabIndex = 1;
            this.subject_textBox.Text = "Test Mail";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label7);
            this.panel7.Controls.Add(this.Message_textbox);
            this.panel7.Location = new Point(7, 199);
            this.panel7.Margin = new Padding(4, 4, 4, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new Size(511, 104);
            this.panel7.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new Point(4, 11);
            this.label7.Margin = new Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new Size(32, 15);
            this.label7.TabIndex = 4;
            this.label7.Text = "msg:";
            // 
            // Message_textbox
            // 
            this.Message_textbox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Message_textbox.Location = new Point(43, 8);
            this.Message_textbox.Margin = new Padding(4, 4, 4, 4);
            this.Message_textbox.Multiline = true;
            this.Message_textbox.Name = "Message_textbox";
            this.Message_textbox.Size = new Size(464, 92);
            this.Message_textbox.TabIndex = 1;
            this.Message_textbox.Text = "メール送信テスト\r\nあいうえお\r\nMail Send test\r\n";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label6);
            this.panel6.Controls.Add(this.Password_textBox);
            this.panel6.Location = new Point(266, 66);
            this.panel6.Margin = new Padding(4, 4, 4, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(252, 36);
            this.panel6.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new Point(4, 11);
            this.label6.Margin = new Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(60, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Password:";
            // 
            // Password_textBox
            // 
            this.Password_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.Password_textBox.Location = new Point(68, 8);
            this.Password_textBox.Margin = new Padding(4, 4, 4, 4);
            this.Password_textBox.Name = "Password_textBox";
            this.Password_textBox.PasswordChar = '*';
            this.Password_textBox.Size = new Size(180, 23);
            this.Password_textBox.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.UserName_textBox);
            this.panel5.Location = new Point(7, 66);
            this.panel5.Margin = new Padding(4, 4, 4, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(252, 36);
            this.panel5.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(4, 11);
            this.label5.Margin = new Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(64, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "UserName:";
            // 
            // UserName_textBox
            // 
            this.UserName_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.UserName_textBox.Location = new Point(85, 8);
            this.UserName_textBox.Margin = new Padding(4, 4, 4, 4);
            this.UserName_textBox.Name = "UserName_textBox";
            this.UserName_textBox.Size = new Size(163, 23);
            this.UserName_textBox.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.SendToAddr_textBox);
            this.panel4.Location = new Point(266, 110);
            this.panel4.Margin = new Padding(4, 4, 4, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(252, 36);
            this.panel4.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(4, 11);
            this.label4.Margin = new Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(74, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "SendToAddr:";
            // 
            // SendToAddr_textBox
            // 
            this.SendToAddr_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.SendToAddr_textBox.Location = new Point(85, 8);
            this.SendToAddr_textBox.Margin = new Padding(4, 4, 4, 4);
            this.SendToAddr_textBox.Name = "SendToAddr_textBox";
            this.SendToAddr_textBox.Size = new Size(163, 23);
            this.SendToAddr_textBox.TabIndex = 1;
            this.SendToAddr_textBox.Text = "sasaha@live.jp";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.FromAddr_textBox);
            this.panel3.Location = new Point(7, 110);
            this.panel3.Margin = new Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(252, 36);
            this.panel3.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(4, 11);
            this.label3.Margin = new Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new Size(62, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "FromAddr:";
            // 
            // FromAddr_textBox
            // 
            this.FromAddr_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.FromAddr_textBox.Location = new Point(85, 8);
            this.FromAddr_textBox.Margin = new Padding(4, 4, 4, 4);
            this.FromAddr_textBox.Name = "FromAddr_textBox";
            this.FromAddr_textBox.Size = new Size(163, 23);
            this.FromAddr_textBox.TabIndex = 1;
            this.FromAddr_textBox.Text = "mrsasaha@gmail.com";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.port_textBox);
            this.panel2.Location = new Point(262, 22);
            this.panel2.Margin = new Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(255, 36);
            this.panel2.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(4, 11);
            this.label2.Margin = new Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(78, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "port(25or587)";
            // 
            // port_textBox
            // 
            this.port_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.port_textBox.Location = new Point(183, 8);
            this.port_textBox.Margin = new Padding(4, 4, 4, 4);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new Size(68, 23);
            this.port_textBox.TabIndex = 1;
            this.port_textBox.Text = "25";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ServerAddr_textBox);
            this.panel1.Location = new Point(7, 22);
            this.panel1.Margin = new Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(252, 36);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(4, 11);
            this.label1.Margin = new Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(68, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "ServerAddr:";
            // 
            // ServerAddr_textBox
            // 
            this.ServerAddr_textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ServerAddr_textBox.Location = new Point(85, 8);
            this.ServerAddr_textBox.Margin = new Padding(4, 4, 4, 4);
            this.ServerAddr_textBox.Name = "ServerAddr_textBox";
            this.ServerAddr_textBox.Size = new Size(163, 23);
            this.ServerAddr_textBox.TabIndex = 1;
            this.ServerAddr_textBox.Text = "ms.ss.local";
            // 
            // DhcpClientTest_button
            // 
            this.DhcpClientTest_button.Location = new Point(544, 22);
            this.DhcpClientTest_button.Name = "DhcpClientTest_button";
            this.DhcpClientTest_button.Size = new Size(130, 23);
            this.DhcpClientTest_button.TabIndex = 105;
            this.DhcpClientTest_button.Text = "DhcpClientTest";
            this.DhcpClientTest_button.UseVisualStyleBackColor = true;
            this.DhcpClientTest_button.Click += DhcpClientTest_button_Click;
            // 
            // Tab15_Networks_UserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(this.groupBox2);
            Controls.Add(this.groupBox1);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Tab15_Networks_UserControl";
            Size = new Size(1384, 785);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox ServerAddr_textBox;
        private System.Windows.Forms.Button SEndMail_button;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox UserName_textBox;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SendToAddr_textBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FromAddr_textBox;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Password_textBox;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox Message_textbox;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox subject_textBox;
        private System.Windows.Forms.TextBox LogWindow_textBox;
        private System.Windows.Forms.Button GetLog_textbox;
        private Button DhcpClientTest_button;
    }
}
