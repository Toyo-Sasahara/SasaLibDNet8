//using System;

//namespace SasaLib
//{
//    /// <summary>
//    /// System.Runtime.Serialization.ObjectManager で、オブジェクトのシリアル化と逆シリアル化の間に、より大きな配列サイズを使用するかどうかを制御します。 BinaryFormatter などの型による大きなオブジェクト グラフのシリアル化と逆シリアル化のパフォーマンスを向上させるには、このスイッチを true に設定します。
//    /// AppContext クラスで使用される、新機能に対するオプトアウト メカニズムを指定する。
//    /// </summary>
//    public static class DotNetFrameworkAppContextSetSwitch
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="isEnabled"></param>
//        /// <returns></returns>
//        public static bool SwitchSystemRuntimeSerializationUseNewMaxArraySize(bool isEnabled)
//        {
//            const string switchName = "Switch.System.Runtime.Serialization.UseNewMaxArraySize";

//            // 取得を試みたスイッチの現在の状態
//            bool currentSwitch_IsEnabled;

//            if (isEnabled)
//            {
//                // スイッチの値の取得を試みる
//                if (AppContext.TryGetSwitch(switchName, out currentSwitch_IsEnabled) == false)
//                {
//                }

//                // 
//                if (currentSwitch_IsEnabled)
//                {
//                    // 
//                } // 既にイネーブルの場合は何もしない
//                else
//                {
//                    //
//                    AppContext.SetSwitch(switchName, true);
//                } // まだ ディスイネーブルの場合はイネーブルに設定する

//            } // スイッチをイネーブルに切り替える指示の場合
//            else 
//            {
//                // スイッチの値の取得を試みる
//                if (AppContext.TryGetSwitch(switchName, out currentSwitch_IsEnabled) == true)
//                {
//                }

                
//                if (currentSwitch_IsEnabled == false)
//                {
//                    //
//                } // 既にディスイネーブルの場合は何もしない
//                else
//                {
//                    //
//                    AppContext.SetSwitch(switchName, false);
//                } // まだイネーブルの場合は ディスイネーブルに設定する

//            }// スイッチをディスイネーブルに切り替える指示の場合

//            // 再度スイッチの取得を試みて、結果が指示(isEnabled)通りならturuを返す
//            if (AppContext.TryGetSwitch(switchName, out currentSwitch_IsEnabled) == isEnabled)
//                return true;
//            else
//                return false;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="switchName"></param>
//        /// <returns></returns>
//        public static bool GetCurrentStatus(string switchName = "Switch.System.Runtime.Serialization.UseNewMaxArraySize")
//        {
//            // 取得を試みたスイッチの現在の状態
//            bool currentSwitch_IsEnabled;
//            // スイッチの値の取得を試みる
//            var result = AppContext.TryGetSwitch(switchName, out currentSwitch_IsEnabled);
//            return currentSwitch_IsEnabled;
//        }
//    }
//}
