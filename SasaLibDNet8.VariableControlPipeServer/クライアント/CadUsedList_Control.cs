using SasaLib.PIPE;
using SasaLib.VariableControlPipeServer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SasaLib.VariableControlPipeClient
{
    [SupportedOSPlatform("windows")]
    public partial class CadUsedList_Control : UserControl
    {
        public SasaLibDelegateWriteLine WriteLine;

        public int SleepSec { get; set; } = 120;

        // 全チェック状態保持

        public bool InventorLoopcheckMode;
        public bool AutocadLoopcheckMode;
        public bool SoliworksLoopcheckMode;

        const string InventorPIPENAME = "InventorTOYOaddin";
        const string AutoCadPIPENAME = "AutoCadTOYOaddin";
        const string SolidWorksPIPENAME = "SolidworksTOYOaddin";

        /// <summary>
        /// 
        /// </summary>
        public List<string> hosts { get; set; } = new List<string>
        {
            "192.168.7.10",
            "MPA01",
            "MPB01",
            "MPB02",
            "MPB03",
            "MPC01",
            "MPC02",
            "MPC03",
            "MPC04",
            "MPC05",
            "MPC06",
            "MPC07",
            "MPC08",
            "MPC09",
            "MPC10",
            "MPC11",
            "MPC12",
            "MPC13",
            "MPC14",
            "MPC15",
            "MPC16",
            "MPC17",
            "MPC18",
            "MPC19",
            "MPC20",
            "MPC21",
            "MPC22",
            "MPC23",
            "MPC24",
            "MPC25",
            "MPC26",
            "MPC27",
            "MPC28",
        };

        /// <summary>
        /// デザイナーでは、引数無しのコンストラクターが必要
        /// </summary>
        public CadUsedList_Control()
        {
            InitializeComponent();
            ValueSetGet_panel.Visible = VariableControlPipeServer.Properties.Settings.Default.ShowSetGetValuePanel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CadUsedList_Control_Load(object sender, EventArgs e)
        {
            this.WriteLine = ClientLogWrite;

            Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
        }

        private void Host_CheckedListBox_Clear(CheckedListBox checkedListBox)
        {
            checkedListBox.Items.Clear();

            for (int i = 0; i < hosts.Count; i++)
            {
                checkedListBox.Items.Add(hosts[i]);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        private void ClientLogWrite(string msg)
        {
            System.Windows.Forms.MethodInvoker method = () =>
            {                        /// UIを操作する処理
                try
                {
                    LogTextBox.AppendText($"{DateTime.Now}:" + msg + "\r\n");
                }
                catch { }

            };
            if (InvokeRequired) { Invoke(method); } else { method(); }
        }


        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Inventor_hostCheck_button_Click(object sender, EventArgs e)
        {
            InventorLoopcheckMode = false;

            do
            {
                await Task.Run(() =>
                {
                    System.Windows.Forms.MethodInvoker method = () =>
                    {
                        //コントロールに対する処理
                        WriteLine($"Inventor利用状況ﾁｪｯｸ開始・・");
                        Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
                        var result = CheckArrivedHostCheckBoxesAsync(Inventor_Host_CheckedListBox, InventorPIPENAME);
                        WriteLine($"Inventor利用状況ﾁｪｯｸ終了。ﾘﾋﾟｰﾄﾓｰﾄﾞ{InventorLoopcheckMode}");
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }
                });

                if (InventorLoopcheckMode)
                {
                    await Task.Delay(SleepSec * 1000);
                    WriteLine($"{SleepSec} * 1000 sec経過しました。実行開始");
                }
            }
            while (InventorLoopcheckMode);
        }

        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AutoCad_hostCheck_button_Click(object sender, EventArgs e)
        {
            AutocadLoopcheckMode = false;

            do
            {
                await Task.Run(() =>
                {
                    System.Windows.Forms.MethodInvoker method = () =>
                    {
                        // コントロールに対する処理
                        WriteLine($"AutoCad利用状況ﾁｪｯｸ開始・・");
                        Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
                        var result = CheckArrivedHostCheckBoxesAsync(AutoCad_Host_CheckedListBox, AutoCadPIPENAME);
                        WriteLine($"AutoCad利用状況ﾁｪｯｸ終了。ﾘﾋﾟｰﾄﾓｰﾄﾞ{InventorLoopcheckMode}");
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }
                });

                if (AutocadLoopcheckMode)
                {
                    await Task.Delay(SleepSec * 1000);
                    WriteLine($"{SleepSec} * 1000 sec経過しました。実行開始");
                }
            }
            while (AutocadLoopcheckMode);
        }

        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SolidWorks_hostCheck_button_Click(object sender, EventArgs e)
        {
            SoliworksLoopcheckMode = false;

            do
            {
                await Task.Run(() =>
                {
                    System.Windows.Forms.MethodInvoker method = () =>
                    {
                        WriteLine($"Solidworks利用状況ﾁｪｯｸ開始・・");
                        // コントロールに対する処理
                        Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
                        var result = CheckArrivedHostCheckBoxesAsync(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME);
                        WriteLine($"Solidworks利用状況ﾁｪｯｸ終了。ﾘﾋﾟｰﾄﾓｰﾄﾞ{InventorLoopcheckMode}");
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }
                });

                if (SoliworksLoopcheckMode)
                {
                    await Task.Delay(SleepSec * 1000);
                    WriteLine($"{SleepSec} * 1000 sec経過しました。実行開始");
                }
            }
            while (SoliworksLoopcheckMode);
        }

        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// ■CommitConfig.Configオブジェクト値の調査
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Inventor_CommitConfigConfig_button_Click(object sender, EventArgs e)
        {
            InventorLoopcheckMode = false;

            do
            {
                await Task.Run(() =>
                {
                    System.Windows.Forms.MethodInvoker method = () =>
                    {
                        //コントロールに対する処理
                        WriteLine($"ﾁｪｯｸ開始・・");

                        Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);

                        var result = CheckArrivedHostCheckBoxesAsync(Inventor_Host_CheckedListBox, InventorPIPENAME);

                        WriteLine($"ﾁｪｯｸ終了。ﾘﾋﾟｰﾄﾓｰﾄﾞ{InventorLoopcheckMode}");
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }
                });

                if (InventorLoopcheckMode)
                {
                    await Task.Delay(SleepSec * 1000);
                    WriteLine($"{SleepSec} * 1000 sec経過しました。実行開始");
                }
            }
            while (InventorLoopcheckMode);
        }


        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// </summary>
        /// <param name="checkBoxListboxSource"></param>
        /// <param name="PIPENAME"></param>
        private async Task CheckArrivedHostCheckBoxesAsync(CheckedListBox checkBoxListboxSource, string PIPENAME)
        {
            string hostname;
            for (int i = 0; i < hosts.Count; i++)
            {
                try
                {
                    hostname = hosts[i];
                    var output = await Task.Run(() =>
                    {
                        DoEvents.Run();
                        VariableControlPipeClient oVCPipeClient = new VariableControlPipeClient("", "", "", false, hostname, PIPENAME);
                        string result_UserDomainFullName = oVCPipeClient.GetZeroValue_DataCommandAsync(CMDNAME.GetCurrentUserDomainFullName).Result;

                        if (result_UserDomainFullName == null)
                            return null;

                        object startDateTImeObj;
                        startDateTImeObj = oVCPipeClient.GetValueAndValueType_DataCommandAsync(CMDNAME.StartUpDateTime, WriteLine:WriteLine).Result;

                        string resultStr = null;
                        if (result_UserDomainFullName != null && startDateTImeObj != null)
                        {
                            resultStr = $"{result_UserDomainFullName} | 開始:{(DateTime)startDateTImeObj}";
                        }
                        else if (result_UserDomainFullName != null && startDateTImeObj == null)
                        {
                            resultStr = $"{result_UserDomainFullName} | 開始時刻不明";

                        }
                        return resultStr;

                    });

                    if (output != null)
                    {
                        //WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄUI] ｺﾝﾄﾛｰﾙ名:{checkBoxListboxSource.Name} count={i} {checkBoxListboxSource.Items[i]}, {hostname}:{PIPENAME} {output} ");
                        checkBoxListboxSource.SetItemChecked(i, true);
                        checkBoxListboxSource.Items[i] = $"〇{hostname}:{output}";
                    }
                    else
                    {
                        //WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄUI] ｺﾝﾄﾛｰﾙ名:{checkBoxListboxSource.Name} count={i} {checkBoxListboxSource.Items[i]}, {hostname}:{PIPENAME} みつかりません ");
                        checkBoxListboxSource.SetItemChecked(i, false);
                        checkBoxListboxSource.Items[i] = $"×{hostname}";
                    }

                }
                catch (Exception ex)
                {
                    WriteLine($"※CheckArrivedHostCheckBoxesAsync(..)内にて例外検知 {ex.Message}");

                }
            }
        }

        /// <summary>
        /// CommitConfig.Config オブジェクト パラメータ・フィールド値
        /// </summary>
        /// <param name="checkBoxListboxSource"></param>
        /// <param name="PIPENAME"></param>
        /// <param name="CommitConfig_ParamaterName"></param>
        /// <returns></returns>
        private async Task CheckCOMMITCONFIG_VAULE(bool setmode, CheckedListBox checkBoxListboxSource, string PIPENAME, string CommitConfig_ParamaterName, string CommitConfigValue)
        {
            if (setmode == true && (string.IsNullOrWhiteSpace(CommitConfig_ParamaterName) || string.IsNullOrWhiteSpace(CommitConfigValue)))
            {
                WriteLine($"オーダーエラー 入力値が想定外");
                return;
            }

            CheckedListBox.CheckedIndexCollection CheckedIndices = checkBoxListboxSource.CheckedIndices;

            int i = 0;
            foreach (var checkedNumber in CheckedIndices)
            {
                i++;
                string hostname = hosts[(int)checkedNumber];

                var output = await Task.Run(() =>
                {
                    DoEvents.Run();
                    VariableControlPipeClient oVCPipeClient = new VariableControlPipeClient("", "", "", false, hostname, PIPENAME);
                    string result_UserDomainFullName = oVCPipeClient.GetZeroValue_DataCommandAsync(CMDNAME.GetCurrentUserDomainFullName).Result;

                    if (result_UserDomainFullName == null)
                        return null;

                    object resutlValue;

                    resutlValue = oVCPipeClient.GetSetValueAndValueType_DataCommandAsync(CommitConfig_ParamaterName, CommitConfigValue, setmode, WriteLine: WriteLine).Result;

                    if (resutlValue != null)
                    {
                        KeyValuePair<string, object> anser = (KeyValuePair<string, object>)resutlValue;
                        string resultStr = $"型({anser.Key})　, 値：{anser.Value}";
                        return resultStr;
                    }
                    else
                    {
                        return $"フィールドまたはパラメータ \"{CommitConfig_ParamaterName}\" は存在しません";
                    }

                });

                if (output != null)
                {
                    System.Windows.Forms.MethodInvoker method = () =>
                    {
                        // コントロールに対する処理
                        checkBoxListboxSource.Items[(int)checkedNumber] = $"〇{hostname}:{output}";
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }
                }
                else
                {
                    System.Windows.Forms.MethodInvoker method = () =>
                    {
                        // コントロールに対する処理
                        checkBoxListboxSource.Items[(int)checkedNumber] = $"×{hostname}";
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="fileVersionType"></param>
        /// <param name="outputGetVersionInfo"></param>
        private void ShowFileVerson(string fullFileName, string hostname, string PipeName, string CurrentUserDomainUserFUllName, string fileVersionType, string outputGetVersionInfo)
        {
            WriteLine("");
            WriteLine($"\tﾎｽﾄ名:{hostname} ﾕｰｻﾞｰ名:{CurrentUserDomainUserFUllName} 接続先ﾊﾟｲﾌﾟ名:{PipeName}");
            WriteLine($"\t調査対象 : \"{fullFileName}\"");
            WriteLine($"\tGetVersionInfo({fileVersionType}) : \"{outputGetVersionInfo}\"");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="fileHashType"></param>
        /// <param name="fileHash"></param>
        private void ShowFileHash(string fullFileName, string hostname, string PipeName, string CurrentUserDomainUserFUllName, string fileHashType, string fileHash)
        {
            WriteLine("");
            WriteLine($"\tﾎｽﾄ名:{hostname} ﾕｰｻﾞｰ名:{CurrentUserDomainUserFUllName} 接続先ﾊﾟｲﾌﾟ名:{PipeName}");
            WriteLine($"\t調査対象 : \"{fullFileName}\"");
            WriteLine($"\tCheckFileHash({fileHashType}) : \"{fileHash}\"");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullFileName"></param>
        /// <param name="hostname"></param>
        /// <param name="CurrentUserDomainUserFUllName"></param>
        /// <param name="fileTimeStampType"></param>
        /// <param name="fileTimeStamp"></param>
        private void ShowFileTimeStamp(string fullFileName, string hostname, string PipeName, string CurrentUserDomainUserFUllName, string fileTimeStampType, string fileTimeStamp)
        {
            WriteLine("");
            WriteLine($"\tﾎｽﾄ名:{hostname} ﾕｰｻﾞｰ名:{CurrentUserDomainUserFUllName} 接続先ﾊﾟｲﾌﾟ名:{PipeName}");
            WriteLine($"\t調査対象 : \"{fullFileName}\"");
            WriteLine($"\tGetFileTimeStamp({fileTimeStampType}) : \"{fileTimeStamp}\"");
        }

        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        public void AllCheckStart(object sender, EventArgs e, bool repeat = false)
        {
            InventorLoopcheckMode = repeat;
            AutocadLoopcheckMode = repeat;
            SoliworksLoopcheckMode = repeat;

            Inventor_hostCheck_button_Click(sender, e);
            AutoCad_hostCheck_button_Click(sender, e);
            SolidWorks_hostCheck_button_Click(sender, e);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CAD_Addin_Version_button_Click(object sender, EventArgs e)
        {
            WriteLine("CAD_Addin_Version_button_Click　調査開始");

            InventorLoopcheckMode = false;
            var FullFileName = @"C:\ProgramData\Autodesk\Inventor addins\TOYOM\InventorTOYOaddinCommit\InventorTOYOaddinCommit.dll";
            Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            VersionOrHash_Check(Inventor_Host_CheckedListBox, InventorPIPENAME, FullFileName, CMDNAME.GetVersionInfo, "FileVersion");
            Inventor_checkfile_textBox.Text = FullFileName;

            AutocadLoopcheckMode = false;
            var FullFileName2 = @"C:\ProgramData\TOYOACADCONNECTOR\TOYOACAD2015COMMITTOOL.dll";
            Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            VersionOrHash_Check(AutoCad_Host_CheckedListBox, AutoCadPIPENAME, FullFileName2, CMDNAME.GetVersionInfo, "FileVersion");
            Autocad_checkfile_textBox.Text = FullFileName2;

            SoliworksLoopcheckMode = false;
            var FullFileName3 = @"C:\ProgramData\TOYOSOLIDWORKSADDIN\SolidworksTOYOaddinCommit.dll";
            Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
            VersionOrHash_Check(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, FullFileName3, CMDNAME.GetVersionInfo, "FileVersion");
            Solidworks_checkfile_textBox.Text = FullFileName3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SasaLib_Version_button_Click(object sender, EventArgs e)
        {
            WriteLine("SasaLib_Version_button_Click　調査開始");

            InventorLoopcheckMode = false;
            var FullFileName = @"C:\ProgramData\Autodesk\Inventor addins\TOYOM\InventorTOYOaddinCommit\SasaLib.dll";
            Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            VersionOrHash_Check(Inventor_Host_CheckedListBox, InventorPIPENAME, FullFileName, CMDNAME.GetVersionInfo, "FileVersion");
            Inventor_checkfile_textBox.Text = FullFileName;

            AutocadLoopcheckMode = false;
            var FullFileName2 = @"C:\ProgramData\TOYOACADCONNECTOR\SasaLib.dll";
            Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            VersionOrHash_Check(AutoCad_Host_CheckedListBox, AutoCadPIPENAME, FullFileName2, CMDNAME.GetVersionInfo, "FileVersion");
            Autocad_checkfile_textBox.Text = FullFileName2;

            SoliworksLoopcheckMode = false;
            var FullFileName3 = @"C:\ProgramData\TOYOSOLIDWORKSADDIN\SasaLib.dll";
            Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
            VersionOrHash_Check(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, FullFileName3, CMDNAME.GetVersionInfo, "FileVersion");
            Solidworks_checkfile_textBox.Text = FullFileName3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void STAGINGSYSTEM_SHARE_Version_button_Click(object sender, EventArgs e)
        {
            WriteLine("STAGINGSYSTEM_SHARE_Version_button_Click　調査開始");

            InventorLoopcheckMode = false;
            var FullFileName = @"C:\ProgramData\Autodesk\Inventor addins\TOYOM\InventorTOYOaddinCommit\GenerateTIFFdrawing.dll";
            Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            VersionOrHash_Check(Inventor_Host_CheckedListBox, InventorPIPENAME, FullFileName, CMDNAME.GetVersionInfo, "FileVersion");
            Inventor_checkfile_textBox.Text = FullFileName;

            AutocadLoopcheckMode = false;
            var FullFileName2 = @"C:\ProgramData\TOYOACADCONNECTOR\GenerateTIFFdrawing.dll";
            Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            VersionOrHash_Check(AutoCad_Host_CheckedListBox, AutoCadPIPENAME, FullFileName2, CMDNAME.GetVersionInfo, "FileVersion");
            Autocad_checkfile_textBox.Text = FullFileName2;

            SoliworksLoopcheckMode = false;
            var FullFileName3 = @"C:\ProgramData\TOYOSOLIDWORKSADDIN\GenerateTIFFdrawing.dll";
            Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
            VersionOrHash_Check(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, FullFileName3, CMDNAME.GetVersionInfo, "FileVersion");
            Solidworks_checkfile_textBox.Text = FullFileName3;

            WriteLine("STAGINGSYSTEM_SHARE_Version_button_Click　ループを抜けました");
        }

        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CAD_Addin_Hash_Button_Click(object sender, EventArgs e)
        {
            WriteLine("CAD_Addin_Hash_Button_Click　調査開始 MD5値");

            InventorLoopcheckMode = false;
            var FullFileName = @"C:\ProgramData\Autodesk\Inventor addins\TOYOM\InventorTOYOaddinCommit\InventorTOYOaddinCommit.dll";
            Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            VersionOrHash_Check(Inventor_Host_CheckedListBox, InventorPIPENAME, FullFileName, CMDNAME.CheckFileHash, "MD5");
            Inventor_checkfile_textBox.Text = FullFileName;

            AutocadLoopcheckMode = false;
            var FullFileName2 = @"C:\ProgramData\TOYOACADCONNECTOR\TOYOACAD2015COMMITTOOL.dll";
            Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            VersionOrHash_Check(AutoCad_Host_CheckedListBox, AutoCadPIPENAME, FullFileName2, CMDNAME.CheckFileHash, "MD5");
            Autocad_checkfile_textBox.Text = FullFileName2;

            SoliworksLoopcheckMode = false;
            var FullFileName3 = @"C:\ProgramData\TOYOSOLIDWORKSADDIN\SolidworksTOYOaddinCommit.dll";
            Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
            VersionOrHash_Check(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, FullFileName3, CMDNAME.CheckFileHash, "MD5");
            Solidworks_checkfile_textBox.Text = FullFileName3;
            WriteLine("CAD_Addin_Hash_Button_Click　ループを抜けました");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SasaLib_Hash_button_Click(object sender, EventArgs e)
        {
            WriteLine("SasaLib_Hash_button_Click　調査開始 MD5値");

            InventorLoopcheckMode = false;
            var FullFileName = @"C:\ProgramData\Autodesk\Inventor addins\TOYOM\InventorTOYOaddinCommit\SasaLib.dll";
            Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            VersionOrHash_Check(Inventor_Host_CheckedListBox, InventorPIPENAME, FullFileName, CMDNAME.CheckFileHash, "MD5");
            Inventor_checkfile_textBox.Text = FullFileName;

            AutocadLoopcheckMode = false;
            var FullFileName2 = @"C:\ProgramData\TOYOACADCONNECTOR\SasaLib.dll";
            Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            VersionOrHash_Check(AutoCad_Host_CheckedListBox, AutoCadPIPENAME, FullFileName2, CMDNAME.CheckFileHash, "MD5");
            Autocad_checkfile_textBox.Text = FullFileName2;

            SoliworksLoopcheckMode = false;
            var FullFileName3 = @"C:\ProgramData\TOYOSOLIDWORKSADDIN\SasaLib.dll";
            Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
            VersionOrHash_Check(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, FullFileName3, CMDNAME.CheckFileHash, "MD5");
            Solidworks_checkfile_textBox.Text = FullFileName3;


            WriteLine("SasaLib_Hash_button_Click　ループを抜けました");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void STAGINGSYSTEM_SHARE_Hash_button_Click(object sender, EventArgs e)
        {
            WriteLine("STAGINGSYSTEM_SHARE_Hash_button_Click　調査開始 MD5値");

            InventorLoopcheckMode = false;
            var FullFileNameForInventor = @"C:\ProgramData\Autodesk\Inventor addins\TOYOM\InventorTOYOaddinCommit\GenerateTIFFdrawing.dll";
            Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            VersionOrHash_Check(Inventor_Host_CheckedListBox, InventorPIPENAME, FullFileNameForInventor, CMDNAME.CheckFileHash, "MD5");
            Inventor_checkfile_textBox.Text = FullFileNameForInventor;

            AutocadLoopcheckMode = false;
            var FullFileNameAutocad = @"C:\ProgramData\TOYOACADCONNECTOR\GenerateTIFFdrawing.dll";
            Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            VersionOrHash_Check(AutoCad_Host_CheckedListBox, AutoCadPIPENAME, FullFileNameAutocad, CMDNAME.CheckFileHash, "MD5");
            Autocad_checkfile_textBox.Text = FullFileNameAutocad;

            SoliworksLoopcheckMode = false;
            var FullFileNameSolidworks = @"C:\ProgramData\TOYOSOLIDWORKSADDIN\GenerateTIFFdrawing.dll";
            Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
            VersionOrHash_Check(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, FullFileNameSolidworks, CMDNAME.CheckFileHash, "MD5");
            Solidworks_checkfile_textBox.Text = FullFileNameSolidworks;


            WriteLine("STAGINGSYSTEM_SHARE_Hash_button_Click　ループを抜けました");

        }

        private void GetInstalledSoftware_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SoftwareComponentName_comboBox.Text))
            {
                return;
            }
            WriteLine($"GetInstalledSoftware_button_Cl　調査開始 {SoftwareComponentName_comboBox.Text} が含まれるインストール済みコンポーネント情報");

            SoftwareComponentName_comboBox.Text = SoftwareComponentName_comboBox.Text.TrimStart('"').TrimEnd('"');

            InventorLoopcheckMode = false;
            Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            SoftwareComponentInstalledVersion_Check(Inventor_Host_CheckedListBox, InventorPIPENAME, SoftwareComponentName_comboBox.Text);

            AutocadLoopcheckMode = false;
            Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            SoftwareComponentInstalledVersion_Check(AutoCad_Host_CheckedListBox, AutoCadPIPENAME, SoftwareComponentName_comboBox.Text);

            SoliworksLoopcheckMode = false;
            Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
            SoftwareComponentInstalledVersion_Check(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, SoftwareComponentName_comboBox.Text);
        }


        private void FileHash_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CheckFileHash_target_fullfilename_textBox.Text))
            {
                return;
            }

            WriteLine($"FileHash_button_Click　調査開始 {CheckFileHash_target_fullfilename_textBox.Text} の MD5値");

            InventorLoopcheckMode = false;
            CheckFileHash_target_fullfilename_textBox.Text = CheckFileHash_target_fullfilename_textBox.Text.TrimStart('"').TrimEnd('"');
            var FullFileName = CheckFileHash_target_fullfilename_textBox.Text;
            //Host_CheckedListBox_Clear(Inventor_Host_CheckedListBox);
            VersionOrHash_Check(Inventor_Host_CheckedListBox, InventorPIPENAME, FullFileName, CMDNAME.CheckFileHash, "MD5");
            Inventor_checkfile_textBox.Text = FullFileName;


            AutocadLoopcheckMode = false;
            var FullFileName2 = CheckFileHash_target_fullfilename_textBox.Text;
            //Host_CheckedListBox_Clear(AutoCad_Host_CheckedListBox);
            VersionOrHash_Check(AutoCad_Host_CheckedListBox, AutoCadPIPENAME, FullFileName2, CMDNAME.CheckFileHash, "MD5");
            Autocad_checkfile_textBox.Text = FullFileName2;

            SoliworksLoopcheckMode = false;
            var FullFileName3 = CheckFileHash_target_fullfilename_textBox.Text;
            //Host_CheckedListBox_Clear(SolidWorks_Host_CheckedListBox);
            VersionOrHash_Check(SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, FullFileName, CMDNAME.CheckFileHash, "MD5");
            Solidworks_checkfile_textBox.Text = FullFileName;

            WriteLine("FileHash_button_Click　ループを抜けました");

        }




        // ----------------------------------------------------------------------------------- //

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkedListBox"></param>
        /// <param name="PIPENAME"></param>
        private async void VersionOrHash_Check(CheckedListBox checkedListBox, string PIPENAME, string FullFileName, string Command, string mode)
        {
            CheckedListBox.CheckedIndexCollection CheckedIndices = checkedListBox.CheckedIndices;

            string filename = System.IO.Path.GetFileName(FullFileName);
            string hostname;
            for (int i = 0; i < hosts.Count; i++)
            {
                hostname = hosts[i];
                var output = await Task.Run(() =>
                {
                    VariableControlPipeClient oVCPipeClient = new VariableControlPipeClient("", "", "", false, hostname, PIPENAME);
                    string result_UserDomainFullName = oVCPipeClient.GetZeroValue_DataCommandAsync(CMDNAME.GetCurrentUserDomainFullName).Result;
                    string result = oVCPipeClient.GetTwoValue_DataCommandAsync(Command, FullFileName, mode).Result;
                    string resultStr = null;
                    if (result_UserDomainFullName != null)
                    {
                        resultStr = $"{result_UserDomainFullName} |{filename} {result}";
                    }
                    return resultStr;
                });

                if (output != null)
                {
                    //WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄUI] ｺﾝﾄﾛｰﾙ名:{checkBoxListboxSource.Name} count={i} {checkBoxListboxSource.Items[i]}, {hostname}:{PIPENAME} {output} ");
                    System.Windows.Forms.MethodInvoker method = () =>
                    {
                        checkedListBox.SetItemChecked(i, true);
                        checkedListBox.Items[i] = $"〇{hostname}:{output}";
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }
                }
                else
                {
                    //WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄUI] ｺﾝﾄﾛｰﾙ名:{checkBoxListboxSource.Name} count={i} {checkBoxListboxSource.Items[i]}, {hostname}:{PIPENAME} みつかりません ");

                    System.Windows.Forms.MethodInvoker method = () =>
                    {
                        checkedListBox.SetItemChecked(i, false);
                        checkedListBox.Items[i] = $"×{hostname}";
                    };
                    if (InvokeRequired) { Invoke(method); } else { method(); }
                }

                //SasaLib.DoEvents.Run();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkedListBox"></param>
        /// <param name="PIPENAME"></param>
        /// <param name="SoftwareComponentName"></param>
        private async void SoftwareComponentInstalledVersion_Check(CheckedListBox checkedListBox, string PIPENAME, string SoftwareComponentName)
        {
            CheckedListBox.CheckedIndexCollection CheckedIndices = checkedListBox.CheckedIndices;


            string hostname;
            for (int i = 0; i < hosts.Count; i++)
            {
                hostname = hosts[i];
                var output = await Task.Run(() =>
                {
                    VariableControlPipeClient oVCPipeClient = new VariableControlPipeClient("", "", "", false, hostname, PIPENAME);

                    string result = oVCPipeClient.GetOneValue_DataCommandAsync(CMDNAME.GetInstalledSoftwareVersion, SoftwareComponentName).Result;
                    return result;
                });



                if (output != null)
                {
                    WriteLine($"{hostname} -> {output}");
                    checkedListBox.SetItemChecked(i, true);
                    checkedListBox.Items[i] = $"〇{hostname} {output}";
                }
                else
                {

                    //WriteLine($"◇[ﾊﾟｲﾌﾟｸﾗｲｱﾝﾄUI] ｺﾝﾄﾛｰﾙ名:{checkBoxListboxSource.Name} count={i} {checkBoxListboxSource.Items[i]}, {hostname}:{PIPENAME} みつかりません ");
                    checkedListBox.SetItemChecked(i, false);
                    checkedListBox.Items[i] = $"〇{hostname} {output}";
                }

                //SasaLib.DoEvents.Run();
            }
        }

        /// <summary>
        /// 文字列からsourceListBoxのindexを取得し、そのidexから　List<string> hosts の該当文字列を取得</string>
        /// </summary>
        /// <param name="sourceListBox"></param>
        /// <param name="hosts"></param>
        /// <param name="itemStr"></param>
        /// <returns></returns>
        string getHostNameIndexOf(ListBox sourceListBox, List<string> hosts, string itemStr)
        {
            var index = sourceListBox.Items.IndexOf(itemStr);
            var hostname = hosts[index];
            return hostname;
        }


        private void Inventor_host_CheckClear_button_Click(object sender, EventArgs e)
        {
            Inventor_Host_CheckedListBox.CheckAllCheckBoxes(false);
        }

        private void Autocad_host_CheckClear_button_Click(object sender, EventArgs e)
        {
            AutoCad_Host_CheckedListBox.CheckAllCheckBoxes(false);
        }

        private void Solidworks_host_CheckClear_button_Click(object sender, EventArgs e)
        {
            SolidWorks_Host_CheckedListBox.CheckAllCheckBoxes(false);
        }

        private void value_button_Click(object sender, EventArgs e)
        {
            if (VariableControlPipeServer.Properties.Settings.Default.ShowSetGetValuePanel)
            {
                VariableControlPipeServer.Properties.Settings.Default.ShowSetGetValuePanel = false;
                ValueSetGet_panel.Visible = false;
            }
            else
            {
                VariableControlPipeServer.Properties.Settings.Default.ShowSetGetValuePanel = true;
                ValueSetGet_panel.Visible = true;
            }

            VariableControlPipeServer.Properties.Settings.Default.Save();
        }

        private async Task XmlFileTagUpdate1(CheckedListBox.CheckedIndexCollection CheckedIndices, string PIPENAME)
        {

            if (CheckedIndices.Count == 0)
                return;
            int i = 0;
            foreach (var checkedNumber in CheckedIndices)
            {
                i++;
                string hostname = hosts[(int)checkedNumber];

                XmlFileFullPath_textBox.Text = XmlFileFullPath_textBox.Text.TrimStart('"').TrimEnd('"');

                Task_XmlFileTagUpdate(hostname, PIPENAME, XmlFileFullPath_textBox.Text, CurrentElement_textBox.Text, NewEllement_textbox.Text, SetVaule_textbox.Text);

                await Task.Delay(2000);

            }

            await Task.Delay(5000);

            WriteLine($"終了");

        }

        private void Task_XmlFileTagUpdate(string hostname, string PIPENAME, string XmlFileFullPath, string CurrentElement, string NewEllement, string SetVaule)
        {
            WriteLine($"オーダー先 \\\\{hostname}\\PIPE\\{PIPENAME} {XmlFileFullPath} {CurrentElement} {NewEllement} {SetVaule}");
            VariableControlPipeClient remote = new VariableControlPipeClient("", "", "", false, hostname, PIPENAME);
            var result1 = remote.Command_ConnnectStartAsync(CMDNAME.XmlFileTagUpdate, _Method_XmlFileTagUpdate, WriteLine);


            bool _Method_XmlFileTagUpdate(NamedPipeClientStream pipeCltStream)
            {
                WriteLine("XmlFileTagUpdate(..) スタート");

                StreamString stst = new StreamString(pipeCltStream);

                string ServerResPon1 = stst.ReadString();

                WriteLine($"ServerResPon1 = {ServerResPon1}");

                stst.WriteString(XmlFileFullPath); // XmlFileFullPath 送信

                string ServerResPon2 = stst.ReadString();

                WriteLine($"ServerResPon2 = {ServerResPon2}");

                stst.WriteString(CurrentElement); // CurrentElement 送信

                string ServerResPon3 = stst.ReadString();

                WriteLine($"ServerResPon3 = {ServerResPon3}");

                stst.WriteString(NewEllement); // NewEllement 送信

                string ServerResPon4 = stst.ReadString();

                WriteLine($"ServerResPon4 = {ServerResPon4}");

                stst.WriteString(SetVaule); // SetVaule 送信

                object receveObj;
                using (BinaryReader reader = new BinaryReader(pipeCltStream, Encoding.UTF8, true))
                {
                    receveObj = reader.ReadObject<Object>();
                }
                WriteLine($"ReadObject = {receveObj}");

                if ((bool)receveObj == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private async void XMLFILEUPDATE_button_Click(object sender, EventArgs e)
        {

            await XmlFileTagUpdate1(Inventor_Host_CheckedListBox.CheckedIndices, InventorPIPENAME);
            await XmlFileTagUpdate1(AutoCad_Host_CheckedListBox.CheckedIndices, AutoCadPIPENAME);
            await XmlFileTagUpdate1(SolidWorks_Host_CheckedListBox.CheckedIndices, SolidWorksPIPENAME);

            WriteLine($"終了");

        }


        private async void CommitConfig_GetSet_button_Click(object sender, EventArgs e)
        {
            bool setmode = SetMode_checkBox.Checked;

            //コントロールに対する処理
            await CheckCOMMITCONFIG_VAULE(setmode, Inventor_Host_CheckedListBox, InventorPIPENAME, CommitConfigParameterName_comboBox.Text, CommitConfigValue_comboBox.Text);

            await CheckCOMMITCONFIG_VAULE(setmode, AutoCad_Host_CheckedListBox, AutoCadPIPENAME, CommitConfigParameterName_comboBox.Text, CommitConfigValue_comboBox.Text);

            await CheckCOMMITCONFIG_VAULE(setmode, SolidWorks_Host_CheckedListBox, SolidWorksPIPENAME, CommitConfigParameterName_comboBox.Text, CommitConfigValue_comboBox.Text);


            SetMode_checkBox.Checked = false;

            CommitConfigValue_comboBox.Text = null;

        }

    }
}
