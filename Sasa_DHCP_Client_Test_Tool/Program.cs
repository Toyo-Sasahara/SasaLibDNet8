// See https://aka.ms/new-console-template for more information
using SasaLib;


Console.WriteLine("DHCPクライアントの動作をテストします。");
Console.WriteLine("このツールは、DHCPクライアントの送受信処理を実行し、動作確認を行うためのテストツールです。");
Console.WriteLine("");
Dhcp.DhcpClientTest();

Console.WriteLine("続行するにはキーを押してください...");
Console.ReadKey(true);
