// 【.NETCore 専用】

using System.Net;
using System.Net.Sockets;

namespace SasaLib;

public static class Dhcp2
{
    public static void DhcpClientTest()
    {
        // UDPクライアントソケットを作成
        using var udpClient = new UdpClient();

        // DHCPDISCOVERパケットを作成
        byte[] discoverPacket = CreateDhcpDiscoverPacket();

        // ブロードキャストアドレスを設定
        IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, 67);

        // ブロードキャスト送信を有効化
        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

        DebugConsole.WriteLine("DHCPDISCOVER パケットを送信中...");
        udpClient.Send(discoverPacket, discoverPacket.Length, broadcastEndPoint);

        // 応答を受信
        DebugConsole.WriteLine("DHCPOFFER を待機中...");
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
        byte[] response = udpClient.Receive(ref serverEndPoint);

        // DHCPOFFERの解析
        DebugConsole.WriteLine("DHCPOFFER を受信しました:");
        ParseDhcpOffer(response);
    }

    static byte[] CreateDhcpDiscoverPacket()
    {
        byte[] packet = new byte[244]; // DHCPの最小パケットサイズ

        // トランザクションID (ランダム)
        var random = new Random();
        byte[] transactionId = new byte[4];
        random.NextBytes(transactionId);

        // オペレーションコード: 1 (Request)
        packet[0] = 0x01;

        // ハードウェアタイプ: Ethernet (0x01)
        packet[1] = 0x01;

        // ハードウェアアドレス長: 6 (MACアドレス)
        packet[2] = 0x06;

        // トランザクションID
        Array.Copy(transactionId, 0, packet, 4, 4);

        // クライアントハードウェアアドレス (例: 00:11:22:33:44:55)
        byte[] macAddress = { 0x00, 0x11, 0x22, 0x33, 0x44, 0x55 };
        Array.Copy(macAddress, 0, packet, 28, macAddress.Length);

        // DHCPオプション (Magic Cookie + Discoverオプション)
        byte[] dhcpOptions =
        {
            0x63, 0x82, 0x53, 0x63, // Magic Cookie
            0x35, 0x01, 0x01,       // DHCP Discover
            0x37, 0x03, 0x01, 0x03, 0x06, // Requested Parameters (Subnet Mask, Router, DNS)
            0xFF                    // End Option
        };
        Array.Copy(dhcpOptions, 0, packet, 240, dhcpOptions.Length);

        return packet;
    }

    static void ParseDhcpOffer(byte[] response)
    {
        // トランザクションIDの確認（例として）
        byte[] transactionId = new byte[4];
        Array.Copy(response, 4, transactionId, 0, 4);
        DebugConsole.WriteLine($"Transaction ID: {BitConverter.ToString(transactionId)}");

        // 提供されたIPアドレス
        byte[] offeredIp = new byte[4];
        Array.Copy(response, 16, offeredIp, 0, 4);
        DebugConsole.WriteLine($"Offered IP Address: {string.Join('.', offeredIp)}");

        // DHCPオプションの解析
        int optionsStart = 240; // オプションは240バイト目から始まる
        while (optionsStart < response.Length)
        {
            byte optionType = response[optionsStart];
            if (optionType == 0xFF) break; // オプション終了
            byte optionLength = response[optionsStart + 1];
            byte[] optionData = new byte[optionLength];
            Array.Copy(response, optionsStart + 2, optionData, 0, optionLength);

            DebugConsole.WriteLine($"Option {optionType}: {BitConverter.ToString(optionData)}");

            optionsStart += 2 + optionLength;
        }
    }
}

