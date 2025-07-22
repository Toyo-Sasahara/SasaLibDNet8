using Inventor;
using SasaLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
//using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Inventor コントロール用共有ソース
/// </summary>
namespace SasaLib.InventorAPI
{
    public static partial class InventorControl
    {
        /// <summary>
        /// Unknown
        // ■C#にてInventorオブジェクトを利用するテクニック
        // 理由・・・ Inventor APIはまだCOMベースであるため、常にSystem._ComObjectを返すため、リフレクションを使用することはできません。回避策は次のとおりです。すべてのInventorオブジェクトには、ObjectTypeEnum列挙を返すTypeプロパティがあります。これは、特定のInventorオブジェクトタイプを見つけるために使用できます。
        /// 使用先プロジェクト InventorTOYOaddin02
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static object GetComObjectMember(object obj, string objectType, Action<string> WriteLine = null)
        {
            if (WriteLine == null) WriteLine = Console.WriteLine;

            try
            {
                if (obj != null)
                {
                    System.Type invokeType = obj.GetType();
                    object tmp = invokeType.InvokeMember(objectType, BindingFlags.GetProperty, null, obj, null);
                    return tmp;
                }
                else
                {
                    WriteLine("想定外エラーGetComObjectMember(...)にてnullオブジェクトが指定されました！！");
                    return null;
                }
            }
            catch (Exception ex)
            {
                WriteLine($"GetComObjectMember(...) 例外検知 {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// ■直接 Inventor.ObjectTypeEnum を 返します
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public static ObjectTypeEnum GetInventorObjectType(object obj, Action<string> WriteLine = null)
        {
            if (WriteLine == null) WriteLine = Console.WriteLine;


            if (obj != null)
            {
                return (ObjectTypeEnum)GetComObjectMember(obj, "Type");
            }
            else
            {
                WriteLine("想定外エラーGetComObjectMember(...)にてnullオブジェクトが指定されました！！");
                return 0;
            }

        }

    }
}
