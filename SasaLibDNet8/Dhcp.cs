using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SasaLib;

/// <summary>
/// 
/// </summary>
public static class Dhcp
{
    /// <summary>
    /// 
    /// </summary>
    public static void DhcpClientTest()
    {
        Console.WriteLine("DHCP Discover パケットの送信,およびDHCPサーバーからの受信テスト");

        // クライアントMACアドレス (例: ランダムなものを使用)
        byte[] clientMac = GenerateRandomMac();

        // DHCP Discover パケットの生成
        byte[] dhcpDiscoverPacket = BuildDhcpDiscoverPacket(clientMac);

        // UDPソケットを作成してDHCPサーバーに送信
        using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            //クライアントポート68にバインド
            socket.Bind(new IPEndPoint(IPAddress.Any, 68));


            // DHCPサーバーのブロードキャストアドレス
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 67);

            // パケットを送信
            socket.SendTo(dhcpDiscoverPacket, endPoint);

            DebugConsole.WriteLine("DHCP Discover パケットを送信しました。");

            // 応答を受信（オプション）
            byte[] buffer = new byte[1024];
            socket.ReceiveTimeout = 5000; // タイムアウト5秒
            try
            {
                int receivedBytes = socket.Receive(buffer);
                DebugConsole.WriteLine($"受信したバイト数: {receivedBytes}");
                DebugConsole.WriteLine(BitConverter.ToString(buffer, 0, receivedBytes));

                ParseDhcpPacket(buffer, receivedBytes);
            }
            catch (SocketException ex)
            {
                DebugConsole.WriteLine("応答を受信できませんでした: " + ex.Message);
            }
        }

        //using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        //{
        //    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

        //    // クライアントポート68にバインド
        //    socket.Bind(new IPEndPoint(IPAddress.Any, 68));

        //    // DHCPサーバーのブロードキャストアドレス
        //    IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 67);

        //    // パケットを送信
        //    socket.SendTo(dhcpDiscoverPacket, endPoint);
        //    DebugConsole.WriteLine("DHCP Discover パケットを送信しました。");

        //    // 応答を受信
        //    byte[] buffer = new byte[1024];
        //    socket.ReceiveTimeout = 5000; // タイムアウト5秒
        //    try
        //    {
        //        int receivedBytes = socket.Receive(buffer);
        //        DebugConsole.WriteLine($"受信したバイト数: {receivedBytes}");
        //        DebugConsole.WriteLine(BitConverter.ToString(buffer, 0, receivedBytes));
        //    }
        //    catch (SocketException ex)
        //    {
        //        DebugConsole.WriteLine("応答を受信できませんでした: " + ex.Message);
        //    }
        //}

    }

    static byte[] BuildDhcpDiscoverPacket(byte[] mac)
    {
        byte[] packet = new byte[244];

        // BOOTPヘッダー
        packet[0] = 0x01; // Message type: Boot Request (1)
        packet[1] = 0x01; // Hardware type: Ethernet (1)
        packet[2] = 0x06; // Hardware address length: 6
        packet[3] = 0x00; // Hops: 0
        Array.Copy(BitConverter.GetBytes(0x12345678), 0, packet, 4, 4); // Transaction ID
        packet[8] = 0x00; // Seconds elapsed: 0
        packet[9] = 0x00;
        packet[10] = 0x80; // Bootp flags: Broadcast (0x8000)
        packet[11] = 0x00;

        // Client IP address: 0.0.0.0
        // Your (client) IP address: 0.0.0.0
        // Next server IP address: 0.0.0.0
        // Relay agent IP address: 0.0.0.0
        // これらはデフォルトでゼロのまま

        // Client MAC address
        Array.Copy(mac, 0, packet, 28, mac.Length);

        // Magic Cookie (DHCP identifier)
        packet[236] = 0x63;
        packet[237] = 0x82;
        packet[238] = 0x53;
        packet[239] = 0x63;

        // DHCP options
        packet[240] = 0x35; // Option: (53) DHCP Message Type
        packet[241] = 0x01; // Length: 1
        packet[242] = 0x01; // DHCP Discover

        packet[243] = 0xff; // End Option

        return packet;
    }

    static byte[] GenerateRandomMac()
    {
        Random random = new Random();
        byte[] mac = new byte[6];
        random.NextBytes(mac);
        // ユニキャストMACアドレスにするため、最初のバイトの最下位ビットを0に設定
        mac[0] = (byte)(mac[0] & 0xfe);
        return mac;
    }


    static void ParseDhcpPacket(byte[] buffer, int length)
    {
        if (length < 240)
        {
            Console.WriteLine("パケットが短すぎます。");
            return;
        }

        Console.WriteLine("DHCPサーバーからの返信を解析します・・・・");


        // 固定部分
        if (buffer[0] == 1)
        {
            Console.WriteLine($"オペレーションコード(Op): {buffer[0]} (リクエスト)");
        }
        else if (buffer[0] == 2)
        {
            Console.WriteLine($"オペレーションコード(Op): {buffer[0]} (応答)");
        }
        else
        {
            Console.WriteLine($"オペレーションコード(Op): {buffer[0]} (不明な値)");
        }
        Console.WriteLine($"ハードウェアタイプ(Htype): {buffer[1]}");
        Console.WriteLine($" ハードウェアアドレスの長さ(Hlen): {buffer[2]}");
        Console.WriteLine($"ホップ数（リレーエージェント用）(Hops): {buffer[3]}");

        byte[] transactionId = new byte[4];
        Array.Copy(buffer, 4, transactionId, 0, 4);
        Console.WriteLine($"トランザクション ID(Transaction ID): 0x{BitConverter.ToString(transactionId).Replace("-", "")}");

        Console.WriteLine($"経過時間(Seconds elapsed): {BitConverter.ToUInt16(buffer, 8)}");
        Console.WriteLine($"フラグ(Flags): 0x{BitConverter.ToString(buffer, 10, 2)}");

        Console.WriteLine($"クライアントの現在の IP アドレス(Client IP Address): {new IPAddress(buffer[12..16])}");
        Console.WriteLine($"クライアントに提供される IP アドレス(Your IP Address): {new IPAddress(buffer[16..20])}");
        Console.WriteLine($"次のサーバーの IP アドレス(Next Server IP Address): {new IPAddress(buffer[20..24])}");
        Console.WriteLine($"リレーエージェントの IP アドレス(Relay Agent IP Address): {new IPAddress(buffer[24..28])}");

        byte[] macAddress = new byte[buffer[2]];
        Array.Copy(buffer, 28, macAddress, 0, buffer[2]);
        Console.WriteLine($" クライアントのハードウェアアドレス(Client MAC Address): {BitConverter.ToString(macAddress)}");

        // オプション部分
        Console.WriteLine(" DHCP プロトコル識別子(DHCP Options) (通常0x63825363):");
        int optionsStart = 240;
        while (optionsStart < length)
        {
            byte optionType = buffer[optionsStart];
            if (optionType == 0xFF) break; // End of options
            byte optionLength = buffer[optionsStart + 1];
            byte[] optionData = new byte[optionLength];
            Array.Copy(buffer, optionsStart + 2, optionData, 0, optionLength);

            string optionName = null;

            switch (optionType)
            {
                case 1:
                    optionName = "サブネットマスク";
                    outputConsole_TypeIPaddres();
                    break;
                case 3:
                    optionName = "デフォルトゲートウェイ";
                    outputConsole_TypeIPaddres();
                    break;
                case 6:
                    optionName = "DNS サーバー";
                    outputConsole_TypeIPaddres();
                    break;
                case 15:
                    optionName = "ドメイン名";
                    Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName}: {Encoding.ASCII.GetString(optionData)}");
                    break;
                case 51:
                    optionName = "リース期間（秒単位）";
                    Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName}: {BitConverter.ToUInt32(ArrayReverse(optionData))}");
                    break;
                case 53:
                    optionName = "DHCP メッセージタイプ";
                    Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName} {BitConverter.ToString(optionData)}");
                    break;
                case 54:
                    optionName = "DHCP サーバー識別子";
                    Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName} : {BitConverter.ToString(optionData)}");
                    break;
                case 58:
                    optionName = "リース更新送信タイミング(T1,秒)";
                    Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName} : {BitConverter.ToUInt32(ArrayReverse(optionData))}");
                    break;
                case 59:
                    optionName = "リース更新送信タイミング(T2,秒)";
                    Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName}: {BitConverter.ToUInt32(ArrayReverse(optionData))}");
                    break;
                default:
                    optionName = "不明";
                    Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName} : {BitConverter.ToString(optionData)}");
                    break;

                    void outputConsole_TypeIPaddres()
                    {
                        try
                        {
                            if (optionLength == 4)
                                Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName}: {new IPAddress(optionData)}");
                            else if (optionLength == 8)
                                Console.WriteLine($"  Option {optionType} Length:{optionLength} {optionName}:  {BitConverter.ToString(optionData)}");
                            else
                                Console.WriteLine($"  Option {optionType} Length:{optionLength} 想定外のデータレンジ:  {BitConverter.ToString(optionData)}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Option {optionType} Length:{optionLength} Data:{BitConverter.ToString(optionData)} , 例外：{ex.Message}");
                        }

                    }
            }

            optionsStart += 2 + optionLength;
        }

        Console.WriteLine("解析完了");

    }

    static byte[] ArrayReverse(byte[] array )
    {
        Array.Reverse(array);
        return array;
    }
}
