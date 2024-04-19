using SasaLib;
using SasaLib.PrintConfig;
//using SasaLib.Winlogon;
using StageServerRemote;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static SasaLib.Winlogon.ClsLogon;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SaSaLibTestApp
{
    public partial class Tab05_Printer_UserControl : UserControl
    {
        Form1 mainForm;

        System.Drawing.Image currentImage = Properties.Resources.TESTIMAGE_A4;

        List<string> globalPrinterCollection = new List<string>();

        string currnetPrinterName;
        List<PaperSize> globalPaperSizeCollection;
        PaperSize currentPapserSize;

        bool currentLandScape;
        List<PaperSource> globalPaperSourceColelection;
        PaperSource currentPaperSource;


        CommonPaperSize currentPaperType = CommonPaperSize.A4P;


        public Tab05_Printer_UserControl(Form1 form)
        {
            this.mainForm = form;
            InitializeComponent();
            SetToControls();

            var eventHandler = new System.EventHandler(TabControl1Changed);

            // 標準のプリンタ名をセット
            PrintercomboBox.Text = Printing.GetDefaultPrinterName();
            currnetPrinterName = PrintercomboBox.Text;

            //ComboBoxのプリンタ名に対するPaperSouceを呼び出し
            globalPaperSizeCollection = SasaLib.Printing.GetPaperSizeObjects(PrintercomboBox.Text);
            globalPaperSourceColelection = SasaLib.Printing.GetPapserSouceObjects(PrintercomboBox.Text);

            List<CommonPaperSize> ps = new List<CommonPaperSize>();

            //プリンタ一覧をComboBoxへ
            globalPrinterCollection.Clear();
            /// 
            foreach (string prName in Printing.GetInstaledPrinterNames())
            {
                globalPrinterCollection.Add(prName);
            }
            /// コンボボックスへ登録完了
            PrintercomboBox.Items.AddRange(globalPrinterCollection.ToArray());

            //ペーパーサイズとトレイ情報をComboBoxへ現在の選択も初期化
            SetPrinterPropertysToComboBox();


            //ランドスケープ情報を初期化
            currentLandScape = true;

            // 用紙サイズ（タイプ）をコンボボックスへ
            RequestPaperSizeComboBox.Items.AddRange(new object[] {
            CommonPaperSize.A0L.ToString(),
                        CommonPaperSize.A1L.ToString(),
                        CommonPaperSize.A2L.ToString(),
                        CommonPaperSize.A3L.ToString(),
                        CommonPaperSize.A4P.ToString(),
            });

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
        /// 現在のPaper記憶Souceの値をTextBoxへ反映させる
        /// </summary>
        void SetPrinterPaperSource()
        {

            PaperSourceKindTextBox.Text = currentPaperSource.Kind.ToString();
            PaperSourceRawkindTextbox.Text = currentPaperSource.RawKind.ToString();
            PaperSoucekindTextBox.Text = currentPaperSource.Kind.ToString();

        }

        /// <summary>
        /// 現在のPaper記憶Sizeの値をTextBoxへ反映させる
        /// System.Drawing.Printing.PaperSize globalPapserSizeの各プロパティをテキストボックスにセットするメソッド
        /// </summary>
        void SetPrinterPaperSize()
        {
            PapserSizeKindTextBox.Text = currentPapserSize.Kind.ToString();
            PaperSizeRawKindTextBox.Text = currentPapserSize.RawKind.ToString();
            PaperSizeWidthTextBox.Text = currentPapserSize.Width.ToString();
            PaperSizeHeightTextBox.Text = currentPapserSize.Height.ToString();
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
            this.LogWindow_textBox.AppendText($"{msg}\r\n");
        }



        private void PrintercomboBox_TextChanged(object sender, EventArgs e)
        {
            //現在のコンボボックスの値を取得
            currnetPrinterName = PrintercomboBox.Text;

            //プリンタの2大設定を読み出し。

            globalPaperSizeCollection = SasaLib.Printing.GetPaperSizeObjects(PrintercomboBox.Text);
            globalPaperSourceColelection = SasaLib.Printing.GetPapserSouceObjects(PrintercomboBox.Text);

            //ペーパーサイズとトレイ情報をComboBoxへ現在の選択も初期化
            SetPrinterPropertysToComboBox();
        }

        private void PaperSizecomboBox_TextChanged(object sender, EventArgs e)
        {
            globalPaperSizeCollection = SasaLib.Printing.GetPaperSizeObjects(PrintercomboBox.Text);

            int i = PaperSizecomboBox.Items.IndexOf(PaperSizecomboBox.Text);
            currentPapserSize = globalPaperSizeCollection[i];
            SetPrinterPaperSize();
        }

        private void PaperSourcesComboBox_TextChanged(object sender, EventArgs e)
        {
            globalPaperSourceColelection = SasaLib.Printing.GetPapserSouceObjects(PrintercomboBox.Text);

            int i = PaperSourcesComboBox.Items.IndexOf(PaperSourcesComboBox.Text);
            currentPaperSource = globalPaperSourceColelection[i];
            SetPrinterPaperSource();

        }

        private void RequestPaperSoucecomboBox_TextChanged(object sender, EventArgs e)
        {
            System.Drawing.Printing.PaperSource _paperSouceNameAnserd = Printing.FindPaperSouce(currnetPrinterName, RequestPaperSoucecomboBox.Text);

            if (_paperSouceNameAnserd != null)
            {
                Console.WriteLine("PaperSouce変更={0}", _paperSouceNameAnserd);
                PaperSourcesComboBox.Text = _paperSouceNameAnserd.SourceName;

            }
        }

        private void RequestPaperSizeComboBox_TextChanged(object sender, EventArgs e)
        {
            System.Drawing.Printing.PaperSize _paperSizeAnserd = Printing.FindPaperSize(currnetPrinterName, RequestPaperSizeComboBox.Text);
            if (_paperSizeAnserd != null)
            {
                Console.WriteLine("PaperSize変更={0}", _paperSizeAnserd);
                PaperSizecomboBox.Text = _paperSizeAnserd.PaperName;

            }

        }

        private void RequestPaperSizeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (RequestPaperSizeComboBox.SelectedItem)
            {
                case "A0L":
                    currentPaperType = CommonPaperSize.A0L;
                    currentLandScape = true;
                    LandscapeRadioButton.Checked = true;
                    break;
                case "A1L":
                    currentPaperType = CommonPaperSize.A1L;
                    currentLandScape = true;
                    LandscapeRadioButton.Checked = true;
                    break;
                case "A2L":
                    currentPaperType = CommonPaperSize.A2L;
                    currentLandScape = true;
                    LandscapeRadioButton.Checked = true;
                    break;
                case "A3L":
                    currentPaperType = CommonPaperSize.A3L;
                    currentLandScape = true;
                    LandscapeRadioButton.Checked = true;
                    break;
                case "A4P":
                    currentPaperType = CommonPaperSize.A4P;
                    currentLandScape = false;
                    PortraitRadioButton.Checked = true;
                    break;
            }
        }


        /// <summary>
        /// 印刷ドライバ固有の設定情報をコンボボックスへ反映
        /// </summary>
        void SetPrinterPropertysToComboBox()
        {
            // PaperSizeの処理
            // System.Drawing.Printing.PrinterSettings.PaperSizeCollection から System.Drawing.Printing.PaperSizeを取り出しコンボボックスにセット
            //①コンボボックスを一度クリアする
            PaperSizecomboBox.Items.Clear();

            //②コンボボックスにPaperSize情報を設定
            foreach (System.Drawing.Printing.PaperSize ps in globalPaperSizeCollection)
            {
                PaperSizecomboBox.Items.Add(ps.PaperName);
            }
            //③フィールド変数globalPapserSizeに最初のPaperSizeオブジェクトを設定
            currentPapserSize = globalPaperSizeCollection[0];

            //④globalPapserSize.PaperName をコンボボックスのTextプロパティに設定
            PaperSizecomboBox.Text = currentPapserSize.PaperName;

            /// 以下PaperSourceの設定
            // System.Drawing.Printing.PrinterSettings.PaperSourceCollection から　System.Drawing.Printing.PaperSourceを取り出しコンボボックスにセット
            //①コンボボックスを一度クリアする
            PaperSourcesComboBox.Items.Clear();
            //②コンボボックスにPaperSouce情報を設定
            try
            {

                foreach (System.Drawing.Printing.PaperSource ps in globalPaperSourceColelection)
                {
                    PaperSourcesComboBox.Items.Add(ps.SourceName);
                }
                //③フィールド変数globalPaperSourceに最初のPaperSizeオブジェクトを設定
                currentPaperSource = globalPaperSourceColelection[0];

                //④コンボボックスに設定
                PaperSourcesComboBox.Text = currentPaperSource.SourceName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //現在のコンボボックスの値を取得
            currnetPrinterName = PrintercomboBox.Text;

            var num = SasaLib.PrinterStatus.GetNumberofPrintQues(currnetPrinterName);

            PrintQuieLabel.Text = $"{currnetPrinterName}キュー残：{num}";
        }

        private void RequestPaperSizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LandscapeRadioButton_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void PrintButton_Click(object sender, EventArgs e)
        {

            //プリントイメージの準備
            Printing printingObj
            = new Printing(
                currentImage, // 印刷イメージ
                System.Drawing.Imaging.PixelFormat.Format24bppRgb, // ビット深度変換値
                3, // レンダリング方式
                Int32.Parse(OffsetPrintX.Text), // Xオフセット値(mm)
                Int32.Parse(OffsePrintY.Text)); // Yオフセット値(mm)
            foreach (RadioButton a in ScaleModePanel.Controls)

            {
                if (a.Checked)
                {
                    switch (a.Name)
                    {
                        case "NoopRadioButton":
                            printingObj.BeforePrintExtractMode = BeforeExtractType.原寸;
                            break;
                        case "PageBoundsRadiboButton":
                            printingObj.BeforePrintExtractMode = BeforeExtractType.ページサイズ範囲;
                            break;
                        case "MarginBoundsRadioButton":
                            printingObj.BeforePrintExtractMode = BeforeExtractType.ページサイズ範囲;
                            break;
                        case "OffsetOnlyRadioButton":
                            printingObj.BeforePrintExtractMode = BeforeExtractType.原寸オフセット;
                            break;

                        default:
                            Console.WriteLine("スケール設定を確認できない");
                            break;
                    }
                }
            }


            var ps = new PrinterSettings(); // works fine
            //using (var i = new ImpersonatedUser("Administrator", "SS", "Fuminano8"))
            //{
            //    // 印刷処理を実行
            //    printingObj.PrintImage(currnetPrinterName, currentPapserSize, currentLandScape, currentPaperSource);
            //}

        }



    }
}
