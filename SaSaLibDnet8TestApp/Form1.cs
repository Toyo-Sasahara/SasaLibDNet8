// EPDM.Interop.epdm.dll
using SasaLib;
using SasaLib.PrintConfig;
using SasaLib.SolidWorks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SasaLib.MSIDLL_Utility;

namespace SaSaLibTestApp
{
    public partial class Form1 : Form
    {
        /// フィールド変数

        //保存内容の保持クラス（親）
        public class 親教室クラス
        {
            public int 教室番号;
            public string 教室名称;
            public List<子名札クラス> 子名札リストobj = new List<子名札クラス>();
        }

        //保存内容の保持クラス（子）
        public class 子名札クラス
        {
            public string 氏名;
            public int 年齢;
            public float 身長;
        }

        /// イベントハンドラ

        public Form1()
        {
            InitializeComponent();

            AddTabPages(tabControl, "FileSystemWathcer", new Tab14_FileSystemWathcer_UserControl(this));

            AddTabPages(tabControl, "TabControl13", new Tab13_UserControl(this));

            AddTabPages(tabControl, "ﾌｧｲﾙﾊﾝﾄﾞﾘﾝｸﾞ", new Tab12_FileHandling_UserControl(this));

            AddTabPages(tabControl, "SysConfigurator", new Tab11_SysConfigurator_UserControl(this));

            AddTabPages(tabControl, "MSIDLL", new Tab10_MSIDLL_UserControl(this));

            AddTabPages(tabControl, "MySQL", new Tab09_MySQL_UserControl(this));

            AddTabPages(tabControl, "SolidworksPDM", new Tab08_SolidworksPDM_UserControl(this));

            AddTabPages(tabControl, "AutoDeskVault", new Tab07_AutodeskVault_UserControl(this));

            AddTabPages(tabControl, "文字列加工", new Tab06_StringProcessing_UserControl(this));

            AddTabPages(tabControl, "印刷", new Tab05_Printer_UserControl(this));

            AddTabPages(tabControl, "描画３", new Tab04_Drawing3_UserControl(this));

            AddTabPages(tabControl, "描画２", new Tab03_Drawing2_UserControl(this));

            AddTabPages(tabControl, "描画１", new Tab02_Drawing1_UserControl(this));

            AddTabPages(tabControl, "TIFF", new Tab01_TIFF_UserControl());

            AddTabPages(tabControl, "ネットワーク", new Tab15_Networks_UserControl());

            tabControl.SelectedIndex = 0;

        }

        /// <summary>
        /// ■タブコントロールを追加します
        /// </summary>
        /// <param name="tabname">名前</param>
        /// <param name="userControlTab">ユーザーコントロールからの派生を指定</param>
        private void AddTabPages(TabControl tabControl, string tabname, UserControl userControlTab, int pageIndex = 0)
        {

            // タブコントロールにタブページを追加
            var tabPage = new System.Windows.Forms.TabPage(tabname);



            // バグ対策 http://www.windows-tech.info/3/c1cc81e4f98c46b9.php
            IntPtr h = this.tabControl.Handle;

            tabControl.TabPages.Insert(pageIndex, tabPage);

            // タブページにページの内容（UserControl派生）を追加
            tabPage.Controls.Add(userControlTab);


            // タブページのサイズに合わせて広げたい場合はこの設定
            userControlTab.Dock = System.Windows.Forms.DockStyle.Fill;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var result = RegAsm.FindRegAsmX64v4Path();
        }
    }
}