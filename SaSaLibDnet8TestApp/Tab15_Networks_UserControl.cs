using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Office.PInvoke;
using SasaLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SasaLib.Mail;

namespace SaSaLibDNet8TestAPP
{
    [SupportedOSPlatform("windows")]
    public partial class Tab15_Networks_UserControl : UserControl
    {
        SasaLib.Mail mail;
        public Tab15_Networks_UserControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] newDnsServers = new string[] { textBox1.Text }; // or string[] newDnsServers = new string[] {  textBox1.Text }.ToArray();

            SasaLib.Net.DnsSetServer(newDnsServers);
        }

        private void SEndMail_button_Click(object sender, EventArgs e)
        {
            string serverAddr = ServerAddr_textBox.Text;
            int port = Int32.Parse(port_textBox.Text);
            string FromAddr = FromAddr_textBox.Text;
            string SendToAddr = SendToAddr_textBox.Text;
            string UserName = UserName_textBox.Text;
            string Password = Password_textBox.Text;
            string subject = subject_textBox.Text;
            string Message = Message_textbox.Text;

            SasaLib.Mail mail = new SasaLib.Mail(serverAddr, port, UserName, Password);

            bool result = mail.MsgSend(FromAddress:FromAddr, ToAddress:SendToAddr, 
                subject: subject,
                Message: Message
                        );

            LogWindow_textBox.AppendText($"SasaLib.Mail.MsgSend(..)実行 戻り値:{result}\r\n");

        }


        private void GetLog_textbox_Click(object sender, EventArgs e)
        {
            List<MsgSended> mailSendedLog = Mail.MsgSendedLog;
            if (mailSendedLog.Count>0)
            {
                MsgSended lastsend = mailSendedLog.Last();

                LogWindow_textBox.AppendText($"{mailSendedLog.Count}件 最後 {lastsend.SendedDateTime} Subject:{lastsend.subject} ErrMsg:{lastsend.ErrMsg}\r\n");

            }
            else
            {
                LogWindow_textBox.AppendText($"{mailSendedLog.Count}件\r\n");

            }

        }
    }
}
