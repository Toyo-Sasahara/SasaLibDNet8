using Inventor;
using SasaLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Inventor コントロール用共有ソース
/// iProperty 制御
/// </summary>
namespace SasaLib.InventorAPI
{
    public static partial class InventorControl
    {
        #region iProperty 取得・設定 基本

        /// <summary>
        /// iProperty
        /// ■iPropertyを検索して値を取得(プロパティセット名を指定)
        /// </summary>
        /// <param name="invacDoc">Inventor.Documentオブジェクト</param>
        /// <param name="PropertySetName">プロパティセット名を指定("Summary Information" "Document Summary Information" "Design Tracking Properties" "User Defined Properties" のいずれか)</param>
        /// <param name="iPropertyName">iProperty名</param>
        /// <returns></returns>
        public static Inventor.Property GetiPropertyValue(Inventor.Document invacDoc, string PropertySetName, string iPropertyName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            if (invacDoc == null)
            {
                WriteLine($"アクティブドキュメントが指定されないか、nullでした");
                return null;
            }
            Inventor.PropertySet invPropSet = null;
            try
            {
                if (invacDoc.PropertySets.PropertySetExists(PropertySetName, out object _propertySet))
                {
                    invPropSet = invacDoc.PropertySets[PropertySetName];
                }
                else
                {
                    WriteLine($"{PropertySetName}見つかりません ");
                    //MessageBox.Show($"{PropertySetName}見つかりません ");
                    return null;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"■東陽ﾂｰﾙ 例外検知 {PropertySetName}見つかりません ");
                WriteLine($"例外検知 GetiPropertyValue(..) {PropertySetName} が見つかりません ");
            }

            //
            Inventor.Property _property = null;
            try
            {
                // TODO: この箇所。機能するか要チェック！！！
                //_property = invPropSet[iProperyName];

                foreach (Inventor.Property x in invPropSet)
                {
                    WriteLine($"(Inventor.Property.Name == {x.Name}");
                    if (x.Name == iPropertyName)
                    {
                        _property = invPropSet[iPropertyName];
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLine($"ファイルにiProperty名 {iPropertyName}が見つかりません{ex.Message}");
                return null;
            }

            return _property;
        }

        /// <summary>
        /// iProperty
        /// ■iPropertyをInventorドキュメントに設定
        /// 使用先プロジェクト InventorTOYOaddinCommit
        /// 使用先プロジェクト InventorTOYOaddin02
        /// </summary>
        /// <param name="invacDoc">Inventor.Documentオブジェクト</param>
        /// <param name="PropertySetName">プロパティセット名を指定("Summary Information" "Document Summary Information" "Design Tracking Properties" "User Defined Properties" のいずれか)</param>
        /// <param name="iPropertyName">iProperty名<</param>
        /// <param name="Value">iProperty設定値</param>
        /// <returns></returns>
        public static bool SetiPropertyValue(Inventor.Document invacDoc, string PropertySetName, string iPropertyName, object Value, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            if (invacDoc == null)
                return false;

            invacDoc.Update();

            Inventor.PropertySet invPropSet;
            object propertySet;
            if (invacDoc.PropertySets.PropertySetExists(PropertySetName, out propertySet))
            {
                invPropSet = invacDoc.PropertySets[PropertySetName];
            }
            else
            {
                WriteLine($"※SetiPropertyValue(..) {PropertySetName}が見つかりません ");

                return false;
            }

            //

            try
            {
                // iPropertyをセットします。
                if (invPropSet[iPropertyName] != null)
                {
                    invPropSet[iPropertyName].Value = Value;
                }

            }
            catch (Exception ex)
            {
                WriteLine($"iProperty[{iPropertyName}]が無いので作成しました {ex.Message}");
                bool result = UpdateCustomiProperty(invacDoc, iPropertyName, Value);
                return result;
            }

            return true;
        }

        /// <summary>
        /// iProperty
        /// ■カスタムiPropertyの更新
        /// </summary>
        /// <param name="invacDoc"></param>
        /// <param name="iProp"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool UpdateCustomiProperty(Inventor.Document invacDoc, string iProp, object value, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            Inventor.PropertySet invPropSet;

            invPropSet = invacDoc.PropertySets["Inventor User Defined Properties"];
            try
            {
                invPropSet.Add(value, iProp);
                return true;
            }
            catch (Exception ex)
            {
                WriteLine($"SasaLib.InventorAPI.InventorControl.UpdateCustomiProperty(...) 例外検知{ex.Message}");
                return false;
            }

        }

        /// <summary>
        /// iProperty
        /// ■指定したプロパティセットを取得（開発時使用）
        /// 使用先プロジェクト InventorTOYOaddinCommit
        /// 使用先プロジェクト InventorTOYOaddin01
        /// </summary>
        /// <param name="invacDoc"></param>
        /// <param name="PropertySetName"></param>
        /// <returns></returns>
        public static Inventor.PropertySet GetiPropertySet(Inventor.Document invacDoc, string PropertySetName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            Inventor.PropertySet invPropSet;

            foreach (PropertySet propertySet in invacDoc.PropertySets)
            {
                WriteLine($"FullFileName:{invacDoc.FullFileName} , PropertySet:{propertySet.Name} DisplayName:{propertySet.DisplayName} InternalName:{propertySet.InternalName}");
            }

            if (invacDoc.PropertySets.PropertySetExists(PropertySetName, out object aa))
            {
                invPropSet = invacDoc.PropertySets[PropertySetName];
                return invPropSet;
            }
            else
            {
                WriteLine($"{PropertySetName}は見つかりません ");

                return null;
            }
        }

        #endregion iProperty 取得・設定 基本

        // -- //

        #region "Summary Information" 取得・設定

        /// <summary>
        /// Documentのプロパティセット "Summary Information" から 指定したiProperty名の値を取得します
        /// </summary>
        /// <param name="document"></param>
        /// <param name="iPropertyName"></param>
        /// <returns></returns>
        public static object GetiPropertySummaryInformationVaule(Inventor.Document document, string iPropertyName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;
            try
            {
                Property iPropertyAnser = InventorAPI.InventorControl.GetiPropertyValue(document, "Summary Information", iPropertyName, WriteLine);
                if (iPropertyAnser != null)
                    return iPropertyAnser.Value;
                else
                    return null;
            }
            catch (Exception ex)
            {
                WriteLine($"\t※例外 {document.FullFileName} から {iPropertyName} を取得できません. {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// オカレンスからドキュメントを取得し、プロパティセット "Summary Information" から 指定したiProperty名の値を取得します
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="iPropertyName"></param>
        /// <returns></returns>
        public static object GetiPropertySummaryInformationVaule(Inventor.ComponentOccurrence compOcc, string iPropertyName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var ans = GetiPropertySummaryInformationVaule((Document)compOcc.Definition.Document, iPropertyName, WriteLine);
            return ans;
        }

        /// <summary>
        ///  Documentのプロパティセット Summary Information に存在する Property名を指定しその値を設定します
        /// </summary>
        /// <param name="document"></param>
        /// <param name="iPropertyName"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool SetiPropertySummaryInformationVaule(Inventor.Document document, string iPropertyName, object Vaule, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var ans = InventorControl.SetiPropertyValue(document, "Summary Information", iPropertyName, Vaule, WriteLine);
            return ans;
        }

        /// <summary>
        ///  オカレンスからドキュメントを取得し、プロパティセット Summary Information に存在する Property名を指定しその値を設定します
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="iPropertyName"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool SetiPropertySummaryInformationVaule(Inventor.ComponentOccurrence compOcc, string iPropertyName, object Vaule, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            WriteLine($"SetiPropertySummaryInformationVaule(..) ComponentOccurrence.Name: \"{compOcc.Name}\"");
            
            Inventor.Document oDoc = ((Inventor.Document)compOcc.Definition.Document);
            
            WriteLine($"SetiPropertySummaryInformationVaule(..) ((Inventor.Document)compOcc.Definition.Document).FullFileName: \"{((Inventor.Document)compOcc.Definition.Document).FullFileName}\"");

            var ans = SetiPropertySummaryInformationVaule(oDoc, iPropertyName, Vaule, WriteLine);

            return ans;
        }

        #endregion "Summary Information" 取得・設定

        #region "User Defined Properties" 取得・設定

        /// <summary>
        ///  Documentのプロパティセット  "User Defined Properties"から 指定したiProperty名の値を取得します
        /// </summary>
        /// <param name="document"></param>
        /// <param name="iPropertyName"></param>
        /// <returns></returns>
        public static object GetiPropertyUserDefinedPropertiesVaule(Inventor.Document document, string iPropertyName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            try
            {
                Property iPropertyAnser = InventorAPI.InventorControl.GetiPropertyValue(document, "User Defined Properties", iPropertyName, WriteLine);
                if (iPropertyAnser != null)
                    return iPropertyAnser.Value;
                else
                    return null;
            }
            catch (Exception ex)
            {
                WriteLine($"\t※例外 {document.FullFileName} から {iPropertyName} を取得できません. {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// オカレンスからドキュメントを取得し、プロパティセット  "User Defined Properties から 指定したiProperty名の値を取得します
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="iPropertyName"></param>
        /// <returns></returns>
        public static object GetiPropertyUserDefinedPropertiesVaule(Inventor.ComponentOccurrence compOcc, string iPropertyName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var ans = GetiPropertyUserDefinedPropertiesVaule((Document)compOcc.Definition.Document, iPropertyName, WriteLine);
            return ans;
        }

        /// <summary>
        ///  Documentのプロパティセット "User Defined Properties" に存在する Property名を指定しその値を設定します
        /// </summary>
        /// <param name="document"></param>
        /// <param name="iPropertyName"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool SetiPropertyUserDefinedPropertiesVaule(Inventor.Document document, string iPropertyName, object Vaule, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var ans = InventorControl.SetiPropertyValue(document, "User Defined Properties", iPropertyName, Vaule, WriteLine);
            return ans;
        }

        /// <summary>
        /// オカレンスからドキュメントを取得し、プロパティセット "User Defined Properties" に存在する Property名を指定しその値を設定します
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="iPropertyName"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool SetiPropertyUserDefinedPropertiesVaule(Inventor.ComponentOccurrence compOcc, string iPropertyName, object Vaule, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            Inventor.Document oDoc = ((Inventor.Document)compOcc.Definition.Document);
            var ans = SetiPropertyUserDefinedPropertiesVaule(oDoc, iPropertyName, Vaule, WriteLine);
            return ans;
        }

        #endregion "User Defined Properties" 取得・設定

        #region "Design Tracking Properties" 取得・設定

        /// <summary>
        /// Documentのプロパティセット  "Design Tracking Properties"から 指定したiProperty名の値を取得します
        /// </summary>
        /// <param name="document"></param>
        /// <param name="iPropertyName"></param>
        /// <returns></returns>
        public static object GetiPropertyDesignTrackingPropertiesVaule(Inventor.Document document, string iPropertyName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            try
            {
                Property iPropertyAnser = InventorAPI.InventorControl.GetiPropertyValue(document, "Design Tracking Properties", iPropertyName, WriteLine);
                if (iPropertyAnser != null)
                    return iPropertyAnser.Value;
                else
                    return null;
            }
            catch (Exception ex)
            {
                WriteLine($"\t※例外 {document.FullFileName} から {iPropertyName} を取得できません. {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// オカレンスからドキュメントを取得し、プロパティセット  "Design Tracking Properties" から 指定したiProperty名の値を取得します
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="iPropertyName"></param>
        /// <returns></returns>
        public static object GetiPropertyDesignTrackingPropertiesVaule(Inventor.ComponentOccurrence compOcc, string iPropertyName, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var name = compOcc.Name;
            var ans = GetiPropertyDesignTrackingPropertiesVaule((Document)compOcc.Definition.Document, iPropertyName, WriteLine);
            return ans;
        }

        /// <summary>
        /// Documentのプロパティセット "Design Tracking Properties" に存在する Property名を指定しその値を設定します
        /// </summary>
        /// <param name="document"></param>
        /// <param name="iPropertyName"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool SetiPropertyDesignTrackingPropertiesVaule(Inventor.Document document, string iPropertyName, object Vaule, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var ans = InventorControl.SetiPropertyValue(document, "Design Tracking Properties", iPropertyName, Vaule, WriteLine);

            return ans;
        }

        /// <summary>
        /// オカレンスからドキュメントを取得し、プロパティセット Design Tracking Properties" に存在する Property名を指定しその値を設定します
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="iPropertyName"></param>
        /// <param name="Vaule"></param>
        /// <returns></returns>
        public static bool SetiPropertyDesignTrackingPropertiesVaule(Inventor.ComponentOccurrence compOcc, string iPropertyName, object Vaule, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var Name = compOcc.Name;

            Inventor.Document oDoc = ((Inventor.Document)compOcc.Definition.Document);

            var FullFilename = oDoc.FullFileName;

            WriteLine($"■SetiPropertyDesignTrackingPropertiesVaule(..) ComponentOccurrence.Name = \"{Name}\", ComponentOccurrence.Definition.Document.FullFileName =\"{FullFilename}\"");
            
            var ans = SetiPropertyDesignTrackingPropertiesVaule(oDoc, iPropertyName, Vaule, WriteLine);

            return ans;
        }

        #endregion "Design Tracking Properties" 取得・設定

        // -- //

        // iProperty ｺﾝﾃﾝﾂｾﾝﾀｰ情報 取得


        /// <summary>
        /// コンポーネントオカレンスのコレクションの中から指定したiPropery名とその値に合致する最初のオカレンスを取得する
        /// </summary>
        /// <param name="activeAsmDoc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ComponentOccurrence FindFirstiPropertyOccrence(Inventor.ComponentOccurrences oCoccs, string iPropertyName, string propertySetName = "User Defined Properties")
        {
            foreach (ComponentOccurrence oCocc in oCoccs)
            {

                Property iPropertyAnser = InventorAPI.InventorControl.GetiPropertyValue((Inventor.Document)oCocc.Definition.Document, propertySetName, iPropertyName, DebugConsole.WriteLine);
                if (iPropertyAnser == null)
                    continue;
                if (iPropertyAnser.Type == ObjectTypeEnum.kPropertyObject)
                {
                    var value = iPropertyAnser.Value;
                    var type = value.GetType();
                    if (type.FullName == "System.Boolean")
                    {
                        if ((bool)value)
                        {
                            return oCocc;
                        }
                    }
                    else
                    {
                        return null;
                    }

                }
            }
            return null;
        }


        /// <summary>
        /// ｺﾝﾃﾝﾂｾﾝﾀｰ情報を取得
        /// </summary>
        /// <param name="compOcc"></param>
        /// <param name="FamilyId"></param>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public static bool GetiPropertyContentLibraryComponentPropertiesVaule(Inventor.ComponentOccurrence compOcc, out string FamilyId, out string MemberId, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            var ans = GetiPropertyContentLibraryComponentPropertiesVaule((Document)compOcc.Definition.Document, out FamilyId, out MemberId, WriteLine);
            return ans;
        }

        /// <summary>
        /// ｺﾝﾃﾝﾂｾﾝﾀｰ情報を取得
        /// </summary>
        /// <param name="oDoc"></param>
        /// <param name="FamilyId"></param>
        /// <param name="MemberId"></param>
        /// <returns></returns>
        public static bool GetiPropertyContentLibraryComponentPropertiesVaule(Inventor.Document oDoc, out string FamilyId, out string MemberId, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            string iPropertySetName = "B9600981-DEE8-4547-8D7C-E525B3A1727A"; // Content Library Component Properties

            Inventor.PropertySet propSet = null;
            try
            {

                ObjectTypeEnum objType = (Inventor.ObjectTypeEnum)InventorControl.GetComObjectMember(oDoc.PropertySets, "Type");

                Inventor.PropertySets propSets = oDoc.PropertySets;

                foreach (PropertySet _propSet in propSets)
                {
                    DebugConsole.WriteLine($"{_propSet.InternalName} {_propSet.Name} {_propSet.DisplayName}");
                }

                propSet = oDoc.PropertySets[iPropertySetName];

            }
            catch (Exception ex)
            {
                FamilyId = null;
                MemberId = null;
                WriteLine($"※例外検知 InventorControl.GetiPropertyContentLibraryComponentPropertiesVaule(..) {ex.Message}");
                return false;
            }

            try
            {
                Inventor.Property familyId = propSet["FamilyId"];
                FamilyId = familyId.Value.ToString();
                Inventor.Property memberId = propSet["MemberId"];
                MemberId = memberId.Value.ToString();

                return true;
            }
            catch (Exception ex)
            {
                FamilyId = null;
                MemberId = null;
                WriteLine($"\t※例外  ｺﾝﾃﾝﾂｾﾝﾀｰ情報を取得 {oDoc.FullFileName}  {ex.Message}");
                return false;
            }
        }

        // -- //

        /// <summary>
        /// iPropertyのDateTime型のデフォルト値を返す
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDefaultDateTime()
        {
            return new DateTime(1601, 1, 1, 0, 0, 0);
        }

    }
}
