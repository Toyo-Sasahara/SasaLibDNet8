//using System;
//using System.Collections.Generic;
//using System.Runtime.Versioning;
//using System.Windows.Forms;

//namespace SasaLib
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [SupportedOSPlatform("windows")]
//    public static class ControlExtensions
//    {
//        /// <summary>
//        /// コンボボックスから文字列を検索しIndexを返す
//        /// </summary>
//        /// <param name="cb"></param>
//        /// <param name="text"></param>
//        /// <returns></returns>
//        public static int GetIndexFromComboBox(System.Windows.Forms.ComboBox cb, string text)
//        {
//            foreach (var a in cb.Items)
//            {
//                Console.WriteLine("GetIndexFromComboBox  = {0}", a);
//            }

//            int index = cb.Items.IndexOf(text);

//            return index;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        static List<Control> s_controlList = new List<Control>();

//        /// <summary>
//        /// コンテナ内のすべてのコントロールをList&lt;Control&gt;で返す。再帰検索する
//        /// </summary>
//        /// <param name="container"></param>
//        /// <returns></returns>
//        public static List<Control> CreateControlList(Control container)
//        {
//            foreach (Control control in container.Controls)
//            {
//                s_controlList.Add(control);

//                // 再起処理(パネルやグループボックス用)
//                CreateControlList(control);
//            }
//            return s_controlList;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="PanelControls"></param>
//        /// <param name="eventHandler"></param>
//        public static void SetSameEventToContainer(List<Control> PanelControls, EventHandler eventHandler)
//        {

//            foreach (Control control in PanelControls)
//            {
//                if (control is TextBox)
//                    ((TextBox)control).TextChanged += eventHandler;
//                else if (control is ComboBox)
//                    ((ComboBox)control).TextChanged += eventHandler;
//                else if (control is CheckBox)
//                    ((CheckBox)control).CheckedChanged += eventHandler;
//                else if (control is RadioButton)
//                    ((RadioButton)control).CheckedChanged += eventHandler;
//                else if (control is ListBox)
//                    ((ListBox)control).SelectedIndexChanged += eventHandler;
//            }

//        }

//        /// <summary>
//        /// コンテナ内の複数のコントール（制限あり）に同じイベントハンドラを割り当てる
//        /// </summary>
//        /// <param name="container"></param>
//        /// <param name="eventHandler"></param>
//        public static void SetSameEventToContainer(Control container, EventHandler eventHandler)
//        {
//            var PanelControls = CreateControlList(container); // ClientImpersonationPanel内のコントロールを検索してList化

//            foreach (Control control in PanelControls)
//            {
//                if (control is TextBox)
//                    ((TextBox)control).TextChanged += eventHandler;
//                else if (control is ComboBox)
//                    ((ComboBox)control).TextChanged += eventHandler;
//                else if (control is CheckBox)
//                    ((CheckBox)control).CheckedChanged += eventHandler;
//                else if (control is RadioButton)
//                    ((RadioButton)control).CheckedChanged += eventHandler;
//                else if (control is ListBox)
//                    ((ListBox)control).SelectedIndexChanged += eventHandler;
//            }

//        }
//    }
//}
