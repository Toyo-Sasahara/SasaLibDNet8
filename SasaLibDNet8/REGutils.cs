using System;
using System.Runtime.Versioning;

namespace SasaLib
{
    [SupportedOSPlatform("windows")]
    public static class REGutils
    {
        /// <summary>
        /// レジストリ書き換え・追加
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="ValueName">"string"|"int"|"StringArray"|"Bytes"</param>
        /// <param name="Value"></param>
        public static void SetValue(string Key, string ValueName, object Value)
        {
            Microsoft.Win32.Registry.SetValue(Key, ValueName, Value);
        }

        public static string GetValue(string Key, string ValueName, object Value)
        {
            string stringValue = (string)Microsoft.Win32.Registry.GetValue(Key, ValueName, Value);

            return stringValue;
        }

        /// <summary>
        /// レジストリ追加。またはもとに戻す
        /// </summary>
        /// <param name="KeyName">レジストリキー</param>
        /// <param name="ValueName">名前</param>
        /// <param name="Value">値</param>
        /// <param name="flag">true,指定した文字列が値にある場合はその値を削除|false,値を既存文字列の後部に追加</param>
        public static void SetValueAdd(string KeyName, string ValueName, string Value, bool flag = false)
        {
            string stringValue = (string)Microsoft.Win32.Registry.GetValue(KeyName, ValueName, Value);

            if (stringValue.Contains(Value))
            {
                if (flag)
                {
                    stringValue = stringValue.Replace(Value, "");
                    Microsoft.Win32.Registry.SetValue(KeyName, ValueName, stringValue);

                }
            }
            else
            {
                /// 既存の値の後ろに指定した文字列を追加して更新
                Microsoft.Win32.Registry.SetValue(KeyName, ValueName, stringValue + Value);
            }
        }

        /// <summary>
        /// スタートアップに追加
        /// </summary>
        /// <param name="ProcessName">プログラム識別名</param>
        /// <param name="ExecutablePath">実行ファイルパス</param>
        /// <param name="forced">すでにProductNameが登録されているとき強制する場合:true</param>
        /// <returns>すでに登録がある場合:false,登録に失敗:false,登録が実行された場合:true</returns>
        public static bool AddHKCU_CurrentVersionRun(string ProcessName, string ExecutablePath, bool forced = false)
        {
            string keyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";

            try
            {
                string stringValue = (string)Microsoft.Win32.Registry.GetValue(keyName, ProcessName, null);
                if (stringValue == null)
                {
                    Microsoft.Win32.Registry.SetValue(keyName, ProcessName, ExecutablePath);
                    return true;
                }
                else
                {
                    if (forced)
                    {
                        Microsoft.Win32.Registry.SetValue(keyName, ProcessName, ExecutablePath);
                        return true;
                    }
                    return false;
                }

            }
            catch (Exception ex)
            {
                DebugConsole.WriteLine($"AddHKCU_CurrentVersionRun(..)にて例外検知{ex.Message}");
                return false;
            }
        }

        /// <summary>
        ///  スタートアップから削除
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <param name="forced"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        public static bool RemoveHKCU_CurrentVersionRun(string ProcessName, bool forced = false)
        {

            try
            {
                //レジストリの削除
                //キーを書き込み許可で開く

                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

                if (regkey != null)
                {
                    if (forced)
                    {
                        //次のようにすると指定した値が見つからなくてもエラーが出ない

                        regkey.DeleteValue(ProcessName, false);
                        regkey.Close();
                        return true;
                    }
                    else
                    {
                        //キーにある値の削除
                        regkey.DeleteValue(ProcessName);
                        regkey.Close();
                        return true;
                    }

                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RemoveHKCU_CurrentVersionRun(..)にて例外発生。{ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// スタートアップに追加されているかどうかを調べる
        /// </summary>
        /// <param name="ProcessName">調査するプロセス名</param>
        /// <returns>true:登録済み</returns>
        [SupportedOSPlatform("windows")]
        public static bool CheckHKCU_CurrentVersionRun(string ProcessName)
        {
            string keyName = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run";
            string stringValue = (string)Microsoft.Win32.Registry.GetValue(keyName, ProcessName, null);
            if (stringValue == null)
                return false;
            else
                return true;
        }

    }
}
