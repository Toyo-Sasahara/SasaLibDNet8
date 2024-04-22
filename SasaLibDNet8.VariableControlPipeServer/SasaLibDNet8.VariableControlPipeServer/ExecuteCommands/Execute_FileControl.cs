using SasaLib;
using SasaLib.PIPE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SasaLib.VariableControlPipeServer.ExecuteCommands
{
    [SupportedOSPlatform("windows")]
    internal class Execute_FileControl
    {
        NamedPipeServerStream PipeSrvStream { get; }
        int ServerId { get; }
        SasaLibDelegateWriteLine WriteLine { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pipeSrvStream"></param>
        /// <param name="serverId"></param>
        /// <param name="writeLine"></param>
        internal Execute_FileControl(int serverId, NamedPipeServerStream pipeSrvStream,  SasaLibDelegateWriteLine writeLine)
        {
            PipeSrvStream = pipeSrvStream;
            ServerId = serverId;
            WriteLine = writeLine;
        }

        /// <summary>
        /// ファイルのバージョン情報を調べて返す
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        internal bool HandShakeProcess_GetVersionInfo(out string errMsg, int timeout = 5000)
        {
            errMsg = null;

            try
            {
                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(PipeSrvStream, ServerId);


                StreamString stst = new StreamString(PipeSrvStream);

                stst.WriteString("OK. Send Target FullFileName.");

                string FullFileName = stst.ReadString(timeout, WriteLine);

                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]GetVersionInfo {clientInfo}より調査するファイルのFullFileNameを受信しました。{FullFileName} ");

                FullFileName = System.Environment.ExpandEnvironmentVariables(FullFileName);
                WriteLine($"◆環境変数展開後 {FullFileName} ");

                if (System.IO.File.Exists(FullFileName) == false)
                {
                    errMsg = $"ファイル{FullFileName}は存在しません";
                    return false;
                }

                stst.WriteString("OK. Send Request information type (FileVersion,ProductVersion,FileDescription,OriginalFilename,InternalName).");

                string request = stst.ReadString(timeout, WriteLine);

                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]GetVersionInfo {clientInfo}より Request information type を受信しました。{request} ");

                string output = null;

                System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(FullFileName);

                if (ver != null)
                {


                    switch (request)
                    {
                        case "FileVersion":

                            output = ver.FileVersion;
                            break;

                        case "ProductVersion":
                            output = ver.ProductVersion;
                            break;

                        case "FileDescription":
                            output = ver.FileDescription;
                            break;

                        case "OriginalFilename":
                            output = ver.OriginalFilename;
                            break;

                        case "InternalName":
                            output = ver.InternalName;
                            break;


                        default:
                            output = null;
                            break;
                    }

                }

                stst.WriteString(output);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        internal bool HandShakeProcess_CheckFileHash(out string errMsg, int timeout = 5000)
        {
            errMsg = null;

            try
            {
                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(PipeSrvStream, ServerId);


                StreamString stst = new StreamString(PipeSrvStream);

                stst.WriteString("OK. Send Target FullFileName.");


                string FullFileName = stst.ReadString(timeout, WriteLine);

                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]CheckFileHash {clientInfo}より調査するファイルのFullFileNameを受信しました。{FullFileName} ");

                FullFileName = System.Environment.ExpandEnvironmentVariables(FullFileName);
                WriteLine($"◆環境変数展開後 {FullFileName} ");

                if (System.IO.File.Exists(FullFileName) == false)
                {
                    errMsg = $"ファイル{FullFileName}は存在しません";
                    return false;
                }

                stst.WriteString("OK. Send Hash Type (MD5).");

                string HashType = stst.ReadString(timeout, WriteLine);

                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]CheckFileHash {clientInfo}よりHashTypeを受信しました。{HashType} ");

                string fileHashCode = null;

                switch (HashType)
                {
                    case "MD5":
                        string md5 = FileFolder.GetMD5FileHash(FullFileName);

                        fileHashCode = md5;
                        break;
                    default:
                        return false;
                }

                stst.WriteString(fileHashCode);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ﾌｧｲﾙタイムスタンプを調査して返す
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool HandShakeProcess_GetFileTimeStamp(out string errMsg, int timeout = 5000)
        {
            errMsg = null;

            try
            {
                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(PipeSrvStream, ServerId);

        StreamString stst = new StreamString(PipeSrvStream);

        stst.WriteString("OK. Send Target FullFileName.");


                string FullFileName = stst.ReadString(timeout, WriteLine);

        WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]GetFileTimeStamp {clientInfo}より調査するファイルのFullFileNameを受信しました。{FullFileName} ");

        FullFileName = System.Environment.ExpandEnvironmentVariables(FullFileName);
                WriteLine($"◆環境変数展開後 {FullFileName} ");

                if (System.IO.File.Exists(FullFileName) == false)
                {
                    errMsg = $"ファイル{FullFileName}は存在しません";
                    return false;
                }

    stst.WriteString("OK. Send TimeStampType (CreationTime,LastWriteTime,LastAccessTime).");

                string TimeStampType = stst.ReadString(timeout, WriteLine);

    WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]GetFileTimeStamp {clientInfo}よりTimeStampTypeを受信しました。{TimeStampType} ");

    string fileTimeStamp = null;

                switch (TimeStampType)
                {
                    case "CreationTime":
                        fileTimeStamp = System.IO.File.GetCreationTime(FullFileName).ToString();
                        break;
                    case "LastWriteTime":
                        fileTimeStamp = System.IO.File.GetLastWriteTime(FullFileName).ToString();
                        break;
                    case "LastAccessTime":
                        fileTimeStamp = System.IO.File.GetLastAccessTime(FullFileName).ToString();
                        break;
                    default:
                        errMsg = $"TimeStampType {TimeStampType}は 定義されていません";
                        return false;
                }

stst.WriteString(fileTimeStamp);

return true;
            }
            catch
            {
    return false;
}
        }

    }
}
