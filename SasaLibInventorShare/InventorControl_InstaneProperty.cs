using Inventor;
using SasaLib;
using SasaLib.InventorAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;

namespace SasaLib.InventorAPI
{
    public static partial class InventorControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="PropertyName"></param>
        /// <param name="PropertySetName"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        public static Inventor.Property GetInstanceProperty(Inventor.ComponentOccurrence compOcc, string PropertySetName, string PropertyName, Action<string> WriteLine = null)
        {
            if (WriteLine == null) { WriteLine = DebugConsole.WriteLine; }

            
            try
            {
                if (compOcc.OccurrencePropertySetsEnabled)
                {
                    Inventor.PropertySet instancePropSet = compOcc.OccurrencePropertySets[PropertySetName];

                    foreach (Inventor.Property a in instancePropSet)
                    {
                        if (a.Name == PropertyName)
                        {
                            Inventor.Property result = instancePropSet[PropertyName];

                            WriteLine($"■InventorControl.GetInstanceProperty(..) オブジェクトタイプ：{InventorControl.GetInventorObjectType(result)}");

                            return result;
                        }
                    }
                    WriteLine($"■InventorControl.GetInstanceProperty(..) プロパティ Name:\"{PropertyName}\" はみつからない");
                    return null;
                }
                else
                {
                    WriteLine($"■InventorControl.GetInstanceProperty(..) インスタンスプロパティは設定されていない");

                    return null;
                }
            }
            catch (Exception ex)
            {
                WriteLine($"※InventorControl.GetInstanceProperty(..)  にて例外検知 {ex.Message} {ex.InnerException}");
                return null;
            }

        }

        public static Inventor.PropertySet GetInstancePropertySet(Inventor.ComponentOccurrence compOcc, string PropertySetName, Action<string> WriteLine = null)
        {
            if (WriteLine == null) { WriteLine = DebugConsole.WriteLine; }

            try
            {
                if (compOcc.OccurrencePropertySetsEnabled)
                {
                    Inventor.PropertySet instancePropSet = compOcc.OccurrencePropertySets[PropertySetName];

                    return instancePropSet;
                }
                else
                {
                    WriteLine($"■InventorControl.GetInstancePropertySet(..) ComponentOccurrence.Name \"{compOcc.Name}\" にインスタンスプロパティは設定されていない");

                    return null;
                }
            }
            catch (Exception ex)
            {
                WriteLine($"※InventorControl.GetInstancePropertySet(..) にて例外検知 {ex.Message} {ex.InnerException}");
                return null;
            }

        }

        /// <summary>
        /// インスタンスプロパティを追加
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="PropertyName"></param>
        /// <param name="Value"></param>
        /// <param name="PropertySetName"></param>
        /// <param name="WriteLine"></param>
        /// <returns></returns>
        private static bool _SetInstancePropertyValue(Inventor.ComponentOccurrence compOcc, string PropertySetName, string PropertyName, object Value, Action<string> WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            try
            {
                compOcc.OccurrencePropertySetsEnabled = true;
            }
            catch
            {
                WriteLine($"※SetInstancePropertyValue(..)にて例外 OccurrencePropertySetsEnabled を trueに 設定できません");
                return false;
            }

            Inventor.PropertySet invPropSet;

            object propertySet;
            if (compOcc.OccurrencePropertySets.PropertySetExists(PropertySetName, out propertySet))
            {
                invPropSet = compOcc.OccurrencePropertySets[PropertySetName];
            }
            else
            {
                WriteLine($"※SetInstancePropertyValue(..) {PropertySetName}が見つかりません ");

                return false;
            }

            try
            {
                // iPropertyをセットします。
                if (invPropSet[PropertyName] != null)
                {
                    invPropSet[PropertyName].Value = Value;
                }

            }
            catch (Exception ex)
            {
                WriteLine($"インスタンスプロパティ[{PropertyName}]が無いので作成しました");
                bool result = UpdateCustomInstanceProperty(compOcc, PropertyName, Value);
                return result;
            }

            //Inventor.Property instacenProp = invPropSet.Add(Value, PropertyName);
            //WriteLine($"■SetInstancePropertyValue.Add(..) 成功 DisplayName:\"{instacenProp.DisplayName}\" Name:\"{instacenProp.Name}\" Value:\"{instacenProp.Value}\"");
            return true;

        }
        public static bool SetInstancePropertyValue(Inventor.ComponentOccurrence compOcc, string PropertySetName, string PropertyName, object Value, Action<string> WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;
            try
            {

                var activeEditDocument = ((Inventor.Application)compOcc.Application).ActiveEditDocument;
                WriteLine($"■SetInstancePropertyValue(..) ActiveEditDocument.FullFileName = {activeEditDocument.FullFileName}");


                if (InventorControl.IsOccurrenceInDocument((AssemblyDocument)activeEditDocument,compOcc))
                {
                    return _SetInstancePropertyValue(compOcc, PropertySetName, PropertyName, Value, WriteLine);

                }
                else
                {
                    WriteLine($"※SetInstancePropertyValue(..) 指定されたオカレンス{compOcc.Name}はｱｸﾃｨﾌﾞﾄﾞｷｭﾒﾝﾄ \"{activeEditDocument.FullFileName}\" の直下の子ではありません");

                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteLine($"※SetInstancePropertyValue(..) にて例外検知 {ex.Message}");
                return false;
            }
        }

        private static bool UpdateCustomInstanceProperty(Inventor.ComponentOccurrence compOcc, string PropertyName, object value, Action<string> WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            Inventor.PropertySet invPropSet;

            invPropSet = compOcc.OccurrencePropertySets["Inventor User Defined Properties"];
            try
            {
                invPropSet.Add(value, PropertyName);
                return true;
            }
            catch (Exception ex)
            {
                WriteLine($"SasaLib.InventorAPI.InventorControl.UpdateCustomInstanceProperty(...) 例外検知{ex.Message}");
                return false;
            }

        }

        public static object GetiInstancePropertyUserDefinedPropertiesVaule(Inventor.ComponentOccurrence compOcc, string PropertyName, Action<string> WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            try
            {
                Property ans = GetInstanceProperty(compOcc, "User Defined Properties", PropertyName, WriteLine);
                if (ans != null)
                    return ans.Value;
                else
                    return null;
            }
            catch (Exception ex)
            {
                WriteLine($"\t※GetiInstancePropertyUserDefinedPropertiesVaule(..) 例外検知 {PropertyName} を取得できません. {ex.Message}");
                return null;
            }

        }

        public static Inventor.PropertySet GetiInstancePropertyUserDefinedPropertySet(Inventor.ComponentOccurrence compOcc,Action<string> WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            try
            {
                Inventor.PropertySet ans = GetInstancePropertySet(compOcc, "User Defined Properties", WriteLine);
                if (ans != null)
                    return ans;
                else
                    return null;
            }
            catch (Exception ex)
            {
                WriteLine($"\t※GetiInstancePropertyUserDefinedProperties(..) 例外検知. {ex.Message}");
                return null;
            }

        }



        public static bool SetiInstancePropertyUserDefinedPropertiesVaule(Inventor.ComponentOccurrence compOcc, string PropertyName, object Vaule, Action<string> WriteLine = null)
        {
            var ans = InventorControl.SetInstancePropertyValue(compOcc, "User Defined Properties", PropertyName, Vaule, WriteLine);
            return ans;
        }

    }
}
