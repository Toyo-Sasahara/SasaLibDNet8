//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics.Eventing.Reader;
//using System.Linq;
//using System.Management;
//using System.Net;
//using System.Net.Mail;
//using System.Net.NetworkInformation;
//using System.Runtime.Versioning;
//using System.Threading.Tasks;
//using System.Windows.Documents;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class Net
//    {
//        /// <summary>
//        /// PC名を返す
//        /// </summary>
//        /// <returns></returns>
//        public static string GetHOSTNAME()
//        {
//            return Environment.MachineName;
//        }

//        /// <summary>
//        /// ログオン中のユーザー名を返す
//        /// </summary>
//        /// <returns></returns>
//        public static string GetLONGONNAME()
//        {
//            return Environment.UserName;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="address"></param>
//        /// <param name="Count"></param>
//        /// <returns></returns>
//        public static async Task<bool> CheckPingAsync(string address, int Count = 3)
//        {
//            bool ans = false;
//            Ping sender = new Ping();

//            await Task.Run(() =>
//            {
//                for (int i = 0; i < Count; i++)
//                {
//                    try
//                    {
//                        PingReply reply;
//                        reply = sender.Send(address);

//                        if (reply.Status == IPStatus.Success)
//                        {
//#if DEBUG
//                            //    Console.WriteLine("Reply from {0}: bytes={1} time={2}ms TTL={3}", reply.Address, reply.Buffer.Length,  reply.RoundtripTime, reply.Options.Ttl);
//#endif
//                            ans = true;
//                        }
//                        else
//                        {
//#if DEBUG
//                            Console.WriteLine($"[{address}]へのPing送信に失敗。({reply.Status}) Count変数={Count}");
//#endif
//                            ans = false;
//                        }

//                    }
//                    catch (PingException pingex)
//                    {
//                        var a = pingex;

//                        Console.WriteLine($"CheckPingで例外発生(宛先:{address}, Count変数={Count})：" + a.InnerException);
//                    }

//                    // ping送信の間隔を取る
//                    if (i < Count + 1)
//                    {
//                        System.Threading.Thread.Sleep(500);
//                    }
//                }

//            });

//            return ans;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="address"></param>
//        /// <param name="Count"></param>
//        /// <returns></returns>
//        public static bool CheckPing(string address, int Count = 2)
//        {
//            bool ans = false;
//            Ping sender = new Ping();


//            for (int i = 0; i < Count; i++)
//            {
//                try
//                {
//                    PingReply reply;
//                    reply = sender.Send(address);

//                    if (reply.Status == IPStatus.Success)
//                    {
//#if DEBUG
//                        //    Console.WriteLine("Reply from {0}: bytes={1} time={2}ms TTL={3}", reply.Address, reply.Buffer.Length,  reply.RoundtripTime, reply.Options.Ttl);
//#endif
//                        ans = true;
//                    }
//                    else
//                    {
//#if DEBUG
//                        Console.WriteLine($"[{address}]へのPing送信に失敗。({reply.Status}) Count変数={Count}");
//#endif
//                        ans = false;
//                    }

//                }
//                catch (PingException pingex)
//                {
//                    var a = pingex;

//                    Console.WriteLine($"CheckPingで例外発生(宛先:{address}, Count変数={Count})：" + a.InnerException);
//                }

//                // ping送信の間隔を取る
//                if (i < Count + 1)
//                {
//                    System.Threading.Thread.Sleep(200);
//                }
//            }
//            return ans;
//        }

//        /// <summary>
//        /// IPアドレスの文字列からホスト名を返します。
//        /// </summary>
//        /// <param name="ipAdd"></param>
//        /// <returns></returns>
//        public static string DnsGetHostName(string ipAdd)
//        {
//            //IPHostEntryオブジェクトを取得
//            System.Net.IPHostEntry iphe = System.Net.Dns.GetHostEntry(ipAdd);

//            return iphe.HostName;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="ipAdd"></param>
//        /// <returns></returns>
//        public static string DnsGetHostNameOrIP(string ipAdd)
//        {
//            try
//            {
//                //IPHostEntryオブジェクトを取得
//                System.Net.IPHostEntry iphe = System.Net.Dns.GetHostEntry(ipAdd);

//                return iphe.HostName;
//            }
//            catch
//            {
//                return ipAdd;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="host"></param>
//        /// <returns></returns>
//        public static string DnsGetIpAddressOLD(string host)
//        {
//            try
//            {
//#pragma warning disable CS0618 // 型またはメンバーが旧型式です
//                IPHostEntry hostInfo = Dns.GetHostByName(host);
//#pragma warning restore CS0618 // 型またはメンバーが旧型式です

//                for (int index = 0; index < hostInfo.AddressList.Length; index++)
//                {
//                    Console.WriteLine(hostInfo.AddressList[index]);
//                }

//                return hostInfo.AddressList[0].ToString();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.Message);
//                return host;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="newDnsServers"></param>
//        /// <returns></returns>
//        [SupportedOSPlatform("windows")]
//        public static string[] DnsSetServer(string[] newDnsServers)
//        {
//            try
//            {
//                string[] curDnsServers = null;


//                // ルート\CIMv2のWMIオブジェクトを取得
//                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMv2", "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");

//                // 各ネットワークアダプターに対して処理
//                foreach (ManagementObject queryObj in searcher.Get())
//                {
//                    // DNSサーバーの配列を取得
//                    var _curDnsServers = (string[])queryObj["DNSServerSearchOrder"];
//                    if (_curDnsServers != null && _curDnsServers.Length > 0)
//                    {
//                        var _curDnsServersList = _curDnsServers.ToList();

//                        foreach (string newdnsServer in newDnsServers)
//                        {
//                            if (_curDnsServersList.Contains(newdnsServer) == false)
//                            {
//                                _curDnsServersList.Insert(0, newdnsServer);
//                            }
//                        }


//                        // DNSサーバーの設定を更新
//                        ManagementBaseObject newDnsServerSet = queryObj.GetMethodParameters("SetDNSServerSearchOrder");
//                        // 新しいDNSサーバーに置き換える
//                        newDnsServerSet["DNSServerSearchOrder"] = _curDnsServersList.ToArray();
//                        ManagementBaseObject setDnsServer = queryObj.InvokeMethod("SetDNSServerSearchOrder", newDnsServerSet, null);

//                        var check = (string[])queryObj["DNSServerSearchOrder"];
//                        if (check != null)
//                        {
//                            curDnsServers = check;
//                        }

//                    }
//                }


//                return curDnsServers;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"{ex.Message} {ex.InnerException}");

//                return null;
//            }
//        }

//    }

//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public class Mail
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        public System.Net.Mail.SmtpClient SmtpCllient { get; private set; }
//        private System.Net.Mail.MailMessage msg;
//        private DateTime dt;

//        /// <summary>
//        /// 
//        /// </summary>
//        public struct MsgSended
//        {
//            /// <summary>
//            /// 
//            /// </summary>
//            public string FromAddress;
//            /// <summary>
//            /// 
//            /// </summary>
//            public string ToAddress;
//            /// <summary>
//            /// 
//            /// </summary>
//            public string subject;
//            /// <summary>
//            /// 
//            /// </summary>
//            public string Message;
//            /// <summary>
//            /// 
//            /// </summary>
//            public DateTime SendedDateTime;
//            /// <summary>
//            /// 
//            /// </summary>
//            public string ErrMsg;
//        }

//        private static List<MsgSended> _MsgSendedLog= new List<MsgSended>();

//        /// <summary>
//        /// 
//        /// </summary>
//        public static List<MsgSended> MsgSendedLog
//        {
//            get
//            { return _MsgSendedLog; }
//        }

//        /// <summary>
//        /// コンストラクタ
//        /// </summary>
//        /// <param name="SMTPHostname">SMTPサーバホスト名またはIPアドレス</param>
//        /// <param name="SMTPport">SMTPサーバーのポート番号</param>
//        /// <param name="UserName">SMTPサーバに接続するユーザー名(平文)</param>
//        /// <param name="Password">SMTPサーバに接続するパスワード(平文)</param>
//        [SupportedOSPlatform("windows")]
//        public Mail(string SMTPHostname, int SMTPport = 25, string UserName = "", string Password = "")
//        {
//            if (UserName == null)
//                UserName = "";
//            if (Password == null)
//                Password = "";


//            if (string.IsNullOrWhiteSpace(SMTPHostname))
//            {
//                Eventlog.Log.WriteEntry("SasaLib", EventLogEntryType.Error, 9700,
//                   $"SasaLib.Mail.Mail(...)でエラー。\n" +
//                   $"SMTPHostnameが指定されていません" +
//                   $""
//                   , false, true);
//                return;
//            }

//            SmtpCllient = new System.Net.Mail.SmtpClient();


//            SmtpCllient.Host = SMTPHostname;
//            SmtpCllient.Port = SMTPport;
//            SmtpCllient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
//            //ユーザー名とパスワードを設定する
//            SmtpCllient.Credentials = new System.Net.NetworkCredential(UserName, Password);

//            //Task.Run(() => CountDownAsync());
//        }


//        //private async void CountDownAsync()
//        //{
//        //    while (true)
//        //    {
//        //        await Task.Delay(1000); // 1000ミリ秒待機するという仕事の完了を待ち、

//        //        if (cc > 0)
//        //        {
//        //            cc--; // ccが0になるまでデクリメント
//        //        }
//        //    }
//        //}

//        /// <summary>
//        /// Eメール送信実行（lowレベル実行メソッド） 
//        /// https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpexception.statuscode?view=netframework-4.8#System_Net_Mail_SmtpException_StatusCode
//        /// </summary>
//        /// <param name="FromAddress">送信元アドレス</param>
//        /// <param name="ToAddress">送信先アドレス</param>
//        /// <param name="subject">タイトル</param>
//        /// <param name="Message">メッセージ本文</param>
//        /// <param name="eventViewVerbose">イベントビューアに詳細な送信情報を送る場合true</param>
//        /// <param name="sendInterLockTimeSec">指定秒数以内に 送信元 ,送信先 ,件名,送信内容が同じものを送ろうとした場合に無視しイベントビューアに記録する</param>
//        public bool MsgSend(string FromAddress, string ToAddress, string subject, string Message, bool eventViewVerbose = false, int sendInterLockTimeSec = 120)
//        {          

//            var result =  _MsgSendedLog.FindIndex(item =>
//                item.FromAddress.ToUpper() == FromAddress.ToUpper() &&
//                item.ToAddress.ToUpper() == ToAddress.ToUpper() &&
//                item.subject == subject &&
//                item.Message == Message && 
//                (DateTime.Now - item.SendedDateTime < new TimeSpan(0,0,sendInterLockTimeSec))
//            );
//            if (result != -1)
//            {
//                string sendedtToAddress = _MsgSendedLog[result].ToAddress;
//                DateTime sendedTime = _MsgSendedLog[result].SendedDateTime;

//                Eventlog.Log.WriteEntry("SasaLibMail", EventLogEntryType.Error, 9700,
//                    $"SasaLib.Mail.MsgSend() が 呼ばれましたが 送信元:{FromAddress} ,送信先:{ToAddress} ,件名:{subject} ,送信内容が同じものを {sendInterLockTimeSec} 秒以内に送信ずみでしたので安全のため送信をキャンセルしました");
//                return false;
//            }
//            else
//            {
//            }

//            if (string.IsNullOrWhiteSpace(FromAddress) || string.IsNullOrWhiteSpace(ToAddress))
//            {
//                Eventlog.Log.WriteEntry("SasaLibMail", EventLogEntryType.Error, 9700,
//                   $"SasaLib.Mail.Mail(...)でエラー。\n" +
//                   $"FromAddressまたはToAddress、もしくは両方が指定されていません" +
//                   $""
//                   , false, true);
//                return false;
//            }

//            if (eventViewVerbose)
//            {
//                Eventlog.Log.WriteEntry("SasaLibMail", EventLogEntryType.Information, 9700,
//                    $"System.Net.Mail.MailMessage(..)が呼び出されました．内容は次の通り\n" +
//                    $"送信元:{FromAddress}, 送信先:{ToAddress}, \nタイトル:{subject}\n" +
//                    $"{Message}\n======================================================="
//                    , false, false);
//            }
//            else
//            {
//                Eventlog.Log.WriteEntry("SasaLibMail", EventLogEntryType.Information, 9700,
//                    $"System.Net.Mail.MailMessage(..)が呼び出されました．送信元:{FromAddress}, 送信先:{ToAddress}, タイトル:{subject}", false, false);
//            }

//            //string encodedSubject = String.Format("=?iso-2022-jp?B?{0}?=",Convert.ToBase64String(System.Text.Encoding.GetEncoding("iso-2022-jp").GetBytes(subject)));
//            //string encodedMsg = String.Format("=?iso-2022-jp?B?{0}?=", Convert.ToBase64String(System.Text.Encoding.GetEncoding("iso-2022-jp").GetBytes(Message)));
//            string encodedSubject = subject;
//            //string encodedMsg = Message;

//            dt = DateTime.Now;
//            string encodedMsg = dt.ToString() + "\r" + Message;

//            msg = new System.Net.Mail.MailMessage(FromAddress, ToAddress, encodedSubject, encodedMsg);

//            //Console.WriteLine($"subject = {encodedSubject}\n");
//            //Console.WriteLine($"msg = {encodedMsg}\n");

//            try
//            {
//                //メッセージを送信する
//                //if (cc == 0)
//                //{
//                    SmtpCllient.Send(msg);
//                //    cc = WaitTime;
//                //}

//            }
//            catch (SmtpFailedRecipientsException ex)
//            {
//                for (int i = 0; i < ex.InnerExceptions.Length; i++)
//                {
//                    SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
//                    if (status == SmtpStatusCode.MailboxBusy ||
//                        status == SmtpStatusCode.MailboxUnavailable)
//                    {
//                        Eventlog.Log.WriteEntry("SasaLibMail", EventLogEntryType.Warning, 9700, $"SMTP Delivery failed - retrying in 10 seconds.");

//                        _MsgSendedLog.Add(new MsgSended
//                        {
//                            FromAddress = FromAddress,
//                            ToAddress = ToAddress,
//                            subject = subject,
//                            Message = Message,
//                            SendedDateTime = DateTime.Now,
//                            ErrMsg = $"SMTP Delivery failed - retrying in 10 seconds."
//                        });

//                        System.Threading.Thread.Sleep(10000);
//                        SmtpCllient.Send(msg);
//                    }
//                    else
//                    {
//                        Eventlog.Log.WriteEntry("SasaLibMail", EventLogEntryType.Information, 9700, $"Failed to deliver message to {ex.InnerExceptions[i].FailedRecipient}");

//                        _MsgSendedLog.Add(new MsgSended
//                        {
//                            FromAddress = FromAddress,
//                            ToAddress = ToAddress,
//                            subject = subject,
//                            Message = Message,
//                            SendedDateTime = DateTime.Now,
//                            ErrMsg = $"Failed to deliver message to {ex.InnerExceptions[i].FailedRecipient}"
//                        });

//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"SasaLib.Mail.MsgSend()で例外発生:\"{ex.Message}\" \"{ex.InnerException}\" SendToAddr ={ToAddress} , FromAddr ={FromAddress} , Msg ={Message}");
//                Eventlog.Log.WriteEntry("SasaLibMail", EventLogEntryType.Warning, 9700, $"SasaLib.Mail.MsgSend()で例外発生 \"{ex.Message}\" \"{ex.InnerException}\"\r\n{ex.StackTrace}\r\n" +
//                    $"SendToAddr ={ToAddress} , FromAddr ={FromAddress} , Msg ={Message}");

//                _MsgSendedLog.Add(new MsgSended
//                {
//                    FromAddress = FromAddress,
//                    ToAddress = ToAddress,
//                    subject = subject,
//                    Message = Message,
//                    SendedDateTime = DateTime.Now,
//                    ErrMsg = $"SasaLib.Mail.MsgSend()で例外発生 \"{ex.Message}\" , \"{ex.InnerException}\" , {ex.StackTrace}"
//                });

//                if (msg != null)
//                    msg.Dispose();

//                return false;
//            }

//            _MsgSendedLog.Add(new MsgSended
//            {
//                FromAddress = FromAddress,
//                ToAddress = ToAddress,
//                subject = subject,
//                Message = Message,
//                SendedDateTime = DateTime.Now,
//                ErrMsg = null
//            });

//            if (msg != null)
//                msg.Dispose();

//            return true;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        public void Close()
//        {
//            try
//            {
//                if (msg != null)
//                    msg.Dispose();
//                //sc.Dispose();
//            }
//            catch (Exception ex)
//            {
//                Eventlog.Log.WriteEntry("SasaLibMail", EventLogEntryType.Error, 9700, $"SasaLib.Mail.Close()で例外発生{ex.Message}");
//            }
//        }
//    }
//}