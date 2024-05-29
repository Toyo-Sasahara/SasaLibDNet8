
// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");
SasaLib.FileFolder.CreateShortcutFile(@"D:\インベンター りーどおんりー もーど 起動.lnk", @"C:\Program Files\Autodesk\Inventor 2025\Bin\InvRO.exe");

string targetPath="";
string workingfolder = "";
string description = "";
string iconpath = "";
int iconNum;
SasaLib.FileFolder.ReadShortcutFile(@"D:\インベンター りーどおんりー もーど 起動.lnk", ref targetPath, ref workingfolder, ref description, ref iconpath);


