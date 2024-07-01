//using System;
//using System.Runtime.Versioning;
//using System.Windows.Forms;

//namespace SasaLib.NumberingSupport
//{
//    [SupportedOSPlatform("windows")]
//    public static class MaterialCodeCheck
//    {
//        /// <summary>
//        /// 材質名から適する材質コードが選択されているかを比較する。正しい材質コ―ド または 材質名・材質コードのいずれかに入力が無い場合,または問合せにYESと答えた場合はtrue
//        /// </summary>
//        /// <param name="MATERIAL"></param>
//        /// <param name="MATERIALCODE"></param>
//        /// <returns></returns>
//        public static bool ConfirmDiscrepancy(string MATERIAL, string MATERIALCODE, NativeWindow NativeWindow = null, SasaLibDelegateWriteLine WriteLine = null)
//        {
//            if (WriteLine == null) WriteLine = Console.WriteLine;

//            //入力値が空文字かNULLかチェック
//            if (String.IsNullOrWhiteSpace(MATERIAL) || string.IsNullOrWhiteSpace(MATERIALCODE))
//            {
//                WriteLine($"※MaterialCodeCheck.ConfirmDiscrepancy(..) 材質または材質コード どちらかまたはどちらも入力がありません。無視します");
//                return true;
//            }
//            MATERIAL = MATERIAL.ToUpper();
//            MATERIALCODE = MATERIALCODE.ToUpper();

//            var code = MaterialCodeConfigWork.GetMaterialCode(MATERIAL);
//            if (string.IsNullOrWhiteSpace(code) == false)
//            {
//                string analyzedmaterialcode = code.ToUpper();

//                WriteLine($"■MaterialCodeCheck.ConfirmDiscrepancy(..) データベースより材質名{MATERIAL}に合致するのは{MATERIALCODE}と判断されました");

//                DialogResult dlgresult = DialogResult.None;
//                if (analyzedmaterialcode != MATERIALCODE)
//                {
//                    dlgresult = MessageBox.Show(NativeWindow, $"材質[{MATERIAL}]に対してコード[{MATERIALCODE}]が指定されています。よろしいでしょうか？", "■表題欄 事前チェック", MessageBoxButtons.YesNo);
//                }

//                switch (dlgresult)
//                {
//                    case DialogResult.Yes:
//                        return true;

//                    case DialogResult.No:
//                        return false;

//                    default:
//                        return true;
//                }
//            }
//            else
//            {
//                WriteLine($"■MaterialCodeCheck.ConfirmDiscrepancy(..) データベースより材質名{MATERIAL}に合致するものは見つかりません true にてメソッドを終わらせます");
//                return true;
//            }
//        }
//    }
//}
