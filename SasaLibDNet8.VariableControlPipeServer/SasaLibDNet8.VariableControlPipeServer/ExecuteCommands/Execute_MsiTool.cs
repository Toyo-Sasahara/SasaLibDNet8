using SasaLib.PIPE;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SasaLib.VariableControlPipeServer.ExecuteCommands
{
    [SupportedOSPlatform("windows")]
    internal class Execute_MsiTool
    {
        NamedPipeServerStream PipeSrvStream { get; }
        int ServerId { get; }
        SasaLibDelegateWriteLine WriteLine { get; }

        internal Execute_MsiTool(int serverId, NamedPipeServerStream pipeSrvStream, SasaLibDelegateWriteLine WriteLine)
        {
            this.ServerId = serverId;
            this.PipeSrvStream = pipeSrvStream;
            this.WriteLine = WriteLine;
        }

        /// <summary>
        /// インストールソフトウェアのバージョン番号を調べて返す
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        internal bool HandShakeProcess_GetInstalledSoftwareVersion(out string errMsg, int timeout = 5000)
        {
            errMsg = null;

            try
            {
                // 接続元のｸﾗｲｱﾝﾄ情報を文字列化
                string clientInfo = NamedPipeClientInfo.GetClientHostAndUser(PipeSrvStream, ServerId);


                StreamString stst = new StreamString(PipeSrvStream);

                stst.WriteString("OK. ソフトウェアコンポーネントを送信してください.");

                string softwareComponentName = stst.ReadString(timeout, WriteLine);

                WriteLine($"◆[ﾊﾟｲﾌﾟｻｰﾊﾞ]GetInstalledSoftwareVersion {clientInfo}より調査するソフトウェアコンポーネント名を受信しました。{softwareComponentName} ");

                string output = GetInstalledSoftwareVersion(softwareComponentName);

                stst.WriteString($"{output}");

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
        /// <param name="installedComponentName"></param>
        /// <returns></returns>
        private string GetInstalledSoftwareVersion(string installedComponentName)
        {
            List<MSIDLL_Utility.ProductInfo> allcomponent = MSIDLL_Utility.GetProductInfo();

            MSIDLL_Utility.ProductInfo resultFindOne = allcomponent.Find(item => item.ProductName.ToUpper() == installedComponentName.ToUpper());

            List<MSIDLL_Utility.ProductInfo> resultFinddAll = allcomponent.FindAll(item => item.ProductName.ToUpper().Contains(installedComponentName.ToUpper()));

            string output = null;

            if (resultFindOne != null)
            {
                output = $"\"{installedComponentName}\" , Ver \"{resultFindOne.VersionString}\"";
            }
            else if (resultFinddAll != null && resultFinddAll.Count > 1)
            {
                List<string> data = new List<string>();

                foreach (var item in resultFinddAll)
                {
                    data.Add($"\"{item.ProductName}\"  VersionString \"{item.VersionString}\"");
                }

                output = $"候補が複数  \"{string.Join(" , ", data)}\"";
            }
            else
            {
                output = $"[{installedComponentName}] - みつかりません";
            }

            return output;
        }
    }
}
