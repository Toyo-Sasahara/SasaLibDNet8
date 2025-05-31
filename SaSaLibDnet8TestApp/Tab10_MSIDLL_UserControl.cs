using SasaLib;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SasaLib.MSIDLL_Utility;

namespace SaSaLibDNet8TestAPP
{
    [SupportedOSPlatform("windows")]
    public partial class Tab10_MSIDLL_UserControl : UserControl
    {
        Form1 mainForm;


        public Tab10_MSIDLL_UserControl(Form1 form)
        {
            this.mainForm = form;
            InitializeComponent();
            SetToControls();

            var eventHandler = new System.EventHandler(TabControl1Changed);


        }

        private void TabControl1_VisibleChanged(object sender, EventArgs e)
        {
            LogWindow_textBox.AppendText("TabControl1_VisibleChanged(..)実行・・・\r\n");

            SetToControls();
        }

        bool flag = false;

        /// <summary>
        /// コントロールに変化があったなら
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TabControl1Changed(object sender, EventArgs e)
        {
        }

        private void TabControl1_Load(object sender, EventArgs e)
        {
        }

        public void SetToControls()
        {
        }

        /// <summary>
        /// メインスレッド外からの呼び出しも考慮したﾛｸﾞｳｨﾝﾄﾞｳ変更メソッド
        /// </summary>
        /// <param name="msg"></param>
        public void LogWindowWriteLine(string msg)
        {
            try
            {
                if (this.InvokeRequired)
                {//https://qiita.com/taiyakisun/items/15b57df979eae7562aef
                    this.Invoke(new Action<string>(this.UpdateText), msg);
                }
                else
                {
                    UpdateText($"{msg}\n");
                }
            }
            catch (Exception ex)
            {
                this.LogWindow_textBox.AppendText($"例外検知{ex.Message}\r\n");

            }
        }
        private void UpdateText(string msg)
        {
            MethodInvoker method = () =>
            {
                this.LogWindow_textBox.AppendText($"{msg}\r\n");
            }; if (InvokeRequired) { Invoke(method); } else { method(); }
        }



        private void SelectReceveFullFileName_button_Click(object sender, EventArgs e)
        {

        }

        private void FindProductName_button_Click(object sender, EventArgs e)
        {
            SasaLib.MSIDLL_Utility.ProductInfo product;


            product = MSIDLL_Utility.FindProductName(textBox3.Text);
            if (product == null)
            {
                UpdateText($"{textBox3.Text} is not found");
            }
            else
            {
                UpdateText($"ｺﾝﾎﾟｰﾈﾝﾄID {textBox3.Text} を含む プロダクトID {product.ProductCode} 名前:{product.ProductName}");
            }

        }

        private void GetProductInfo_button_Click(object sender, EventArgs e)
        {

            var result = GetProductInfo(PRODUCTID_textBox.Text);

            if (result != null)
                UpdateText($"プロダクトID {PRODUCTID_textBox.Text} に対応するProductNameがみつかりました {result.ProductName} {result.VersionString}\r\n");

        }

        private void FindFromComponentId_button_Click(object sender, EventArgs e)
        {
            string result = MSIDLL_Utility.GetComponentFullpath(COMPONENTID_textBox.Text);

            UpdateText($"コンポーネントID {COMPONENTID_textBox.Text} に対応するパスがみつかりました {result}\r\n");

        }

        private void Find_button_Click(object sender, EventArgs e)
        {
            List<KeyValuePair<string, string>> componentIds;

            System.String path = MSIDLL_Utility.Find(FileNameTextBox.Text, out componentIds);

            if (path == null)
            {
                UpdateText($"{FileNameTextBox.Text} はデータベース内に見つかりませんでした\r\n");
            }
            else
            {
                UpdateText($"ファイル名 {FileNameTextBox.Text} に対応するパスがみつかりました {path}\r\n");
            }


        }

        private void ShowAllApplicationButton_Click(object sender, EventArgs e)
        {
            List<MSIDLL_Utility.ProductInfo> resultGetProductInf = GetProductInfo();

            resultGetProductInf.ForEach(x => UpdateText(x.ProductName));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void FindComponentID_button_Click(object sender, EventArgs e)
        //{
        //    FullFilename_textBox.Text = FullFilename_textBox.Text.TrimStart('"').TrimEnd('"');

        //    CancellationTokenSource cts = new CancellationTokenSource();
        //    CancellationToken token = cts.Token;
        //    UpdateText($"{FullFilename_textBox.Text} のコンポーネントＩＤの検索をﾊﾞｯｸｸﾞﾗｳﾝﾄﾞにて開始します・・・");

        //    Task.Run(() =>
        //     {
        //         MethodInvoker method = () =>
        //         {
        //             System.String componentID = MSIDLL_Utility.FindComponentID(FullFilename_textBox.Text, token, true, UpdateText);
        //             Task<List<string>> componentIDs = MSIDLL_Utility.FindComponentIDsAsync(FullFilename_textBox.Text, token, true, UpdateText);

        //             if (componentIDs == null || componentIDs.Result.Count == 0)
        //             {
        //                 UpdateText($"{FullFilename_textBox.Text} はデータベース内に見つかりませんでした");
        //             }
        //             else if (componentIDs.Result.Count == 1)
        //             {
        //                 UpdateText($"ファイル名 {FullFilename_textBox.Text} を含む コンポーネントＩＤが1件みつかりました {componentIDs.Result[0]}");
        //             }
        //             else if (componentIDs.Result.Count > 1)
        //             {
        //                 UpdateText($"ファイル名 {FullFilename_textBox.Text} を含む コンポーネントＩＤが{componentIDs.Result.Count}件みつかりました");
        //                 foreach (string componentID in componentIDs.Result)
        //                 {
        //                     UpdateText($"\tコンポーネントＩＤ {componentID}");
        //                 }

        //                 UpdateText($"");
        //             }

        //         }; if (InvokeRequired) { Invoke(method); } else { method(); }
        //     });
        //    UpdateText($"ファイル名 {FullFilename_textBox.Text} を含む コンポーネントＩＤの検索を終了します");

        //}

        private void FindComponentID_button_Click(object sender, EventArgs e)
        {
            FullFilename_textBox.Text = FullFilename_textBox.Text.TrimStart('"').TrimEnd('"');

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            UpdateText($"{FullFilename_textBox.Text} のコンポーネントＩＤの検索をﾊﾞｯｸｸﾞﾗｳﾝﾄﾞにて開始します・・・");

            Task.Run(() =>
             {
                 MethodInvoker method = () =>
                 {
                     List<string> componetIDs = MSIDLL_Utility.GetComponentIDs(FullFilename_textBox.Text, token, true, UpdateText);

                     if (componetIDs != null)
                     {
                         if (componetIDs.Count == 1)
                         {
                            UpdateText($"ファイル名 {FullFilename_textBox.Text} を含む コンポーネントＩＤが1件のみみつかりました {componetIDs[0]}");
                         }
                         else if (componetIDs.Count > 1)
                         {
                             UpdateText($"ファイル名 {FullFilename_textBox.Text} を含む コンポーネントＩＤが {componetIDs.Count} 件みつかりました");
                             foreach (string componentID in componetIDs)
                             {
                                 UpdateText($"\t{componentID}");
                             }
                         }
                     }
                     else
                     {
                        UpdateText($"{FullFilename_textBox.Text} はデータベース内に見つかりませんでした");
                    }

                 }; if (InvokeRequired) { Invoke(method); } else { method(); }
             });
            UpdateText($"ファイル名 {FullFilename_textBox.Text} を含む コンポーネントＩＤの検索を終了します");

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DebugWriteLine(string msg)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<MSIDLL_Utility.ProductInfo> resultGetProductInf = GetProductInfo();

            List<MSIDLL_Utility.ProductInfo> resultFinddAll = resultGetProductInf.FindAll(item => item.ProductName.ToUpper().Contains(textBox1.Text.ToUpper()));




            if (resultFinddAll != null)
            {
                UpdateText("==========================================================================================================");
                foreach (var item in resultFinddAll)
                {
                    UpdateText($"ﾌﾟﾛﾀﾞｸﾄ名: \"{item.ProductName}\" ﾊﾞｰｼﾞｮﾝ: \"{item.VersionString}\" ﾌﾟﾛﾀﾞｸﾄID: \"{item.ProductCode}\"");
                }
                UpdateText("==========================================================================================================");

                //string resultFindAlltoStr = string.Join(", ", resultFinddAll.ConvertAll(item => item.ProductName).ToArray());
                //UpdateText($"プロダクト名 {textBox1.Text} が {resultFinddAll.Count} 件みつかりました \"{resultFindAlltoStr}\"");

                //UpdateText("==========================================================================================================");
            }

            //UpdateText("==========================================================================================================");

            //MSIDLL_Utility.ProductInfo resultFirstOrDefault = resultGetProductInf.FirstOrDefault(item => item.ProductName.ToUpper() == textBox1.Text.ToUpper());
            //if (resultFirstOrDefault != null)
            //{
            //    UpdateText($"プロダクト名 {textBox1.Text} が みつかりました \"{resultFirstOrDefault.VersionString}\"  \"{resultFirstOrDefault.ProductCode}\"");
            //}

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            var ComponentIDs = MSIDLL_Utility.GetComponents_Test(textBox2.Text, token, DebugWriteLine: UpdateText);

            UpdateText($"プロダクトコード {textBox2.Text} がインストールしたファイルは、全部で {ComponentIDs.Count} 件 あります");

            foreach (string componentID in ComponentIDs)
            {

                var result = MSIDLL_Utility.GetClientID(componentID, token);

                //string fullpath = MSIDLL_Utility.GetComponentFullpath(componentID);

                //List<string> result = MSIDLL_Utility.GetComponentIDs(fullpath, token);
                if (result.Count > 1)
                {
                    UpdateText($"コンポーネントID：{componentID}は複数のコンポーネントIDにより参照");
                    foreach (string dupRefscomponentID in result)
                    {
                        UpdateText($"\t{dupRefscomponentID}");
                    }
                }
                else
                {
                    var componentpath = MSIDLL_Utility.GetComponentFullpath(componentID);
                    UpdateText($"コンポーネントID：{componentID} {componentpath}");
                }
            }

            UpdateText($"終了");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LogWindow_textBox.Clear();
        }
    }
}
