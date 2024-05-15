using Inventor;
//using Microsoft.VisualBasic.Compatibility.VB6;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Versioning;
using System.Text;

namespace SasaLib.InventorAPI
{
    [SupportedOSPlatform("windows")]
    public static class Test_InventorControl_Body
    {
        /// <summary>
        /// 次のサンプルでは、アセンブリ内の複数のパーツ間でボディをコピー(関連付けて、または関連付けないで)する方法を例示します。
        /// このサンプルを実行する前に、2 つのパーツが含まれるアセンブリを作成します。このサンプルは、最初のパーツから 2 番目のパーツにボディをコピーします。
        /// </summary>
        /// <param name="inventorApp"></param>
        public static void AssociativeBodyCopy1(Inventor.Application inventorApp)
        {

            // アクティブなアセンブリドキュメントへの参照を設定する。
            AssemblyDocument oAssemblyDoc = (AssemblyDocument)inventorApp.ActiveDocument;
            AssemblyComponentDefinition oAssemblyDef = oAssemblyDoc.ComponentDefinition;

            ComponentOccurrence oOccurrence1 = oAssemblyDef.Occurrences[1];
            PartComponentDefinition oOccrrence1PartDef = (PartComponentDefinition)oOccurrence1.Definition;

            ComponentOccurrence oOccurrence2 = oAssemblyDef.Occurrences[2];
            PartComponentDefinition oOccurrence2PartDef = (PartComponentDefinition)oOccurrence2.Definition;

            // 最初の部分からソリッドボディのソースを取得する。
            SurfaceBody oOccrence1PartdefSourceBody = oOccrrence1PartDef.SurfaceBodies[1];
            object oOccrence1SourceBodyProxy = null;
            oOccurrence1.CreateGeometryProxy(oOccrence1PartdefSourceBody, out oOccrence1SourceBodyProxy);

            // 連想サーフェスベースフィーチャーを作成する。
            ObjectCollection oCollection = inventorApp.TransientObjects.CreateObjectCollection();
            oCollection.Add((SurfaceBodyProxy)oOccrence1SourceBodyProxy);

            NonParametricBaseFeatureDefinition oOccrence2PartDefFeatureDefinition = oOccurrence2PartDef.Features.NonParametricBaseFeatures.CreateDefinition();
            oOccrence2PartDefFeatureDefinition.BRepEntities = oCollection;
            oOccrence2PartDefFeatureDefinition.OutputType = BaseFeatureOutputTypeEnum.kSurfaceOutputType;
            oOccrence2PartDefFeatureDefinition.TargetOccurrence = oOccurrence2;
            oOccrence2PartDefFeatureDefinition.IsAssociative = true;

            NonParametricBaseFeature oBaseFeature1 = oOccurrence2PartDef.Features.NonParametricBaseFeatures.AddByDefinition(oOccrence2PartDefFeatureDefinition);

            // 非結合的なソリッドベースのフィーチャーを作成する。
            NonParametricBaseFeatureDefinition oFeatureDef2 = oOccurrence2PartDef.Features.NonParametricBaseFeatures.CreateDefinition();

            oFeatureDef2.BRepEntities = oCollection;
            oFeatureDef2.OutputType = BaseFeatureOutputTypeEnum.kSolidOutputType;
            oFeatureDef2.TargetOccurrence = oOccurrence2;

            NonParametricBaseFeature oBaseFeature2 = oOccurrence2PartDef.Features.NonParametricBaseFeatures.AddByDefinition(oFeatureDef2);

            oAssemblyDoc.Update();
        }

        public static void AssociativeBodyCopy2(Inventor.Application inventorApp)
        {
            try
            {
                // アクティブなアセンブリドキュメントへの参照を設定する。
                AssemblyDocument oActiveAssemblyDoc = (AssemblyDocument)inventorApp.ActiveDocument;

                string activeAssemblyDocDirectoryName = System.IO.Path.GetDirectoryName(oActiveAssemblyDoc.FullFileName);
                string activeAssemblyDocFilenmae = System.IO.Path.GetFileName(oActiveAssemblyDoc.FullFileName);
                string activeAssemblyDocDisplayname = oActiveAssemblyDoc.DisplayName;

                ObjectTypeEnum oType;
                if (inventorApp.ActiveDocument.SelectSet.Count == 1)
                {
                    oType = InventorControl.GetInventorObjectType(inventorApp.ActiveDocument.SelectSet[1]);

                    if (oType != ObjectTypeEnum.kComponentOccurrenceObject && oType != ObjectTypeEnum.kComponentOccurrenceProxyObject && oType != ObjectTypeEnum.kSurfaceBodyObject && oType != ObjectTypeEnum.kSurfaceBodyProxyObject)
                    {
                        return;
                    }
                }
                else
                    return;


                // デバイスタグ一覧を示すオカレンス
                ComponentOccurrence oOccurrenceDeviceTagBom;
                PartDocument oDeviceTagBomPartDoc;
                var oDeviceTabBomPartDisplayName = activeAssemblyDocDisplayname + " デバイスタグ一覧";

                // デバイスタグ用コンポーネントオカレンスの存在確認を実施
                oOccurrenceDeviceTagBom = InventorControl.FindFirstiPropertyOccrence(oActiveAssemblyDoc.ComponentDefinition.Occurrences, "デバイスタグ一覧");

                if (oOccurrenceDeviceTagBom == null)
                {
                    // 新しい部品を作成しますが、目には見えません。
                    oDeviceTagBomPartDoc = inventorApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                                                       inventorApp.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject),
                                                       false) as PartDocument;

                    bool setiProp = InventorControl.SetiPropertyUserDefinedPropertiesVaule(oDeviceTagBomPartDoc as Inventor.Document, "デバイスタグ一覧", true);

                    if (setiProp)
                    {
                        // 部品のファイル名を定義します。この時点では保存されませんが、ユーザーが後で
                        // 部品を保存する場合にこの名前が使用されます。
                        oDeviceTagBomPartDoc.FullFileName = System.IO.Path.Combine(activeAssemblyDocDirectoryName, System.IO.Path.ChangeExtension(oDeviceTabBomPartDisplayName, "ipt"));
                        oDeviceTagBomPartDoc.DisplayName = oDeviceTabBomPartDisplayName;

                        // 一時的なジオメトリオブジェクトへの参照を設定します。
                        TransientGeometry oTG = inventorApp.TransientGeometry;
                        // ある程度任意の位置にオカレンスをアセンブリに挿入します。
                        Inventor.Matrix oMatrix = oTG.CreateMatrix();
                        oMatrix.Translation.AddVector(oTG.CreateVector(2, 3, 4));
                        oMatrix.SetToRotation(0.5, oTG.CreateVector(1, 0, 0), oTG.CreatePoint(2, 3, 4));

                        oOccurrenceDeviceTagBom = oActiveAssemblyDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(
                                            oDeviceTagBomPartDoc.ComponentDefinition as Inventor.ComponentDefinition, oMatrix);

                    }
                } // デバイスタグ一覧パーツが未配置の場合
                else
                {
                    oDeviceTagBomPartDoc = oOccurrenceDeviceTagBom.Definition.Document as Inventor.PartDocument;
                } // デバイスタグ一覧パーツはすでに配置済みの場合

                Inventor.SurfaceBodies oSurfaceBodies = oDeviceTagBomPartDoc.ComponentDefinition.SurfaceBodies;
                PartFeatures oFeatures = oDeviceTagBomPartDoc.ComponentDefinition.Features;
                PartComponentDefinition oDeviceTagBomPartDef = oDeviceTagBomPartDoc.ComponentDefinition;




                SurfaceBody oSourceBody;
                object oSourceBodyProxy = null;
                string oTagOccrenceiPropertyPartName = null;
                ComponentOccurrence oTagOccurrence = null;

                if (oType == ObjectTypeEnum.kComponentOccurrenceObject || oType == ObjectTypeEnum.kComponentOccurrenceProxyObject)
                {

                    // 選択対象のComponent Occrence を取得する
                    oTagOccurrence = inventorApp.ActiveDocument.SelectSet[1] as ComponentOccurrence;
                }
                else if (oType == ObjectTypeEnum.kSurfaceBodyObject || oType == ObjectTypeEnum.kSurfaceBodyProxyObject)
                {
                    var oTagSurfaceBody = inventorApp.ActiveDocument.SelectSet[1] as SurfaceBody;
                    var oTagSurfaceBodyParentType = InventorControl.GetInventorObjectType(oTagSurfaceBody.Parent);
                    if (oTagSurfaceBodyParentType == ObjectTypeEnum.kComponentOccurrenceObject || oTagSurfaceBodyParentType == ObjectTypeEnum.kComponentOccurrenceProxyObject)
                        oTagOccurrence = oTagSurfaceBody.Parent as ComponentOccurrence;
                }

                // 

                if (oTagOccurrence == null)
                {

                    return;
                }

                oTagOccrenceiPropertyPartName = InventorControl.GetiPropertyDesignTrackingPropertiesVaule(oTagOccurrence, "Part Number") as String;

                ComponentDefinition oTagOccurrenceComponentDef = (ComponentDefinition)oTagOccurrence.Definition;

                // A1 最初の部分からソリッドボディのソースを取得する。
                oSourceBody = oTagOccurrenceComponentDef.SurfaceBodies[1];


                oTagOccurrence.CreateGeometryProxy(oSourceBody, out oSourceBodyProxy);


                // A2 連想サーフェスベースフィーチャーを作成する。
                NonParametricBaseFeatureDefinition oFeatureDef1 = oDeviceTagBomPartDef.Features.NonParametricBaseFeatures.CreateDefinition();
                ObjectCollection oCollection = inventorApp.TransientObjects.CreateObjectCollection();
                oCollection.Add((SurfaceBodyProxy)oSourceBodyProxy);

                oFeatureDef1.BRepEntities = oCollection;
                oFeatureDef1.OutputType = BaseFeatureOutputTypeEnum.kSurfaceOutputType;
                oFeatureDef1.TargetOccurrence = oOccurrenceDeviceTagBom;
                oFeatureDef1.IsAssociative = true;

                NonParametricBaseFeature oBaseFeature1 = oDeviceTagBomPartDef.Features.NonParametricBaseFeatures.AddByDefinition(oFeatureDef1);

                // oFeaturesから PartFeature.Nameを抽出した新しいリストを作成
                List<string> partFeatureNames = oFeatures.Cast<Inventor.PartFeature>()
                                                         .Select(item =>
                                                         {
                                                             DebugConsole.WriteLine($"PartFeature.Name = {item.Name}");
                                                             return item.Name;
                                                         })
                                                         .ToList();
                //  oFeaturesから PartFeature.SurfaceBody.Nameを抽出した新しいリストを作成
                List<string> surfaceBodyNames = oFeatures.Cast<Inventor.PartFeature>()
                                         .Select(item =>
                                         {
                                             DebugConsole.WriteLine($"PartFeature.SurfaceBody.Name = {item.SurfaceBody.Name}");
                                             return item.SurfaceBody.Name;
                                         })
                                         .ToList();


                string partFeatureNewName = AddNewLineWithIncrementedNumber(partFeatureNames, oTagOccrenceiPropertyPartName + "_フィーチャ");
                oBaseFeature1.Name = partFeatureNewName;
                string surfaceBodyNewName = AddNewLineWithIncrementedNumber(surfaceBodyNames, oTagOccrenceiPropertyPartName);
                oBaseFeature1.SurfaceBody.Name = surfaceBodyNewName;


                //// A3 非結合的なソリッドベースのフィーチャーを作成する。
                //NonParametricBaseFeatureDefinition oFeatureDef2 = oPartDef2.Features.NonParametricBaseFeatures.CreateDefinition();

                //oFeatureDef2.BRepEntities = oCollection;
                //oFeatureDef2.OutputType = BaseFeatureOutputTypeEnum.kSolidOutputType;
                //oFeatureDef2.TargetOccurrence = oOccurrence2;

                //NonParametricBaseFeature oBaseFeature2 = oPartDef2.Features.NonParametricBaseFeatures.AddByDefinition(oFeatureDef2);

                oActiveAssemblyDoc.Update();

            }
            catch (Exception ex)
            {

            }
        }

        public static bool GenerateSurfaceFromDeviceTag(Inventor.AssemblyDocument oActiveAssemblyDoc, Inventor.SelectSet objects)
        {
            Inventor.Application inventorApp = oActiveAssemblyDoc.Parent as Inventor.Application;

            string activeAssemblyDocDirectoryName = System.IO.Path.GetDirectoryName(oActiveAssemblyDoc.FullFileName);
            string activeAssemblyDocFilenmae = System.IO.Path.GetFileName(oActiveAssemblyDoc.FullFileName);
            string activeAssemblyDocDisplayname = oActiveAssemblyDoc.DisplayName;

            ObjectTypeEnum oType;
            if (objects.Count < 1)
            {
                return false;
            }

            // デバイスタグ一覧を示すオカレンス
            ComponentOccurrence oOccurrenceDeviceTagBom;
            PartDocument oDeviceTagBomPartDoc;
            var oDeviceTabBomPartDisplayName = activeAssemblyDocDisplayname + " デバイスタグ一覧";

            // デバイスタグ用コンポーネントオカレンスの存在確認を実施
            oOccurrenceDeviceTagBom = InventorControl.FindFirstiPropertyOccrence(oActiveAssemblyDoc.ComponentDefinition.Occurrences, "デバイスタグ一覧");

            if (oOccurrenceDeviceTagBom == null)
            {
                // 新しい部品を作成しますが、目には見えません。
                oDeviceTagBomPartDoc = inventorApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                                                   inventorApp.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject),
                                                   false) as PartDocument;

                bool setiProp = InventorControl.SetiPropertyUserDefinedPropertiesVaule(oDeviceTagBomPartDoc as Inventor.Document, "デバイスタグ一覧", true);

                if (setiProp)
                {
                    // 部品のファイル名を定義します。この時点では保存されませんが、ユーザーが後で
                    // 部品を保存する場合にこの名前が使用されます。
                    oDeviceTagBomPartDoc.FullFileName = System.IO.Path.Combine(activeAssemblyDocDirectoryName, System.IO.Path.ChangeExtension(oDeviceTabBomPartDisplayName, "ipt"));
                    oDeviceTagBomPartDoc.DisplayName = oDeviceTabBomPartDisplayName;

                    // 一時的なジオメトリオブジェクトへの参照を設定します。
                    TransientGeometry oTG = inventorApp.TransientGeometry;
                    // ある程度任意の位置にオカレンスをアセンブリに挿入します。
                    Inventor.Matrix oMatrix = oTG.CreateMatrix();
                    oMatrix.Translation.AddVector(oTG.CreateVector(2, 3, 4));
                    oMatrix.SetToRotation(0.5, oTG.CreateVector(1, 0, 0), oTG.CreatePoint(2, 3, 4));

                    oOccurrenceDeviceTagBom = oActiveAssemblyDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(
                                        oDeviceTagBomPartDoc.ComponentDefinition as Inventor.ComponentDefinition, oMatrix);

                }
            } // デバイスタグ一覧パーツが未配置の場合
            else
            {
                oDeviceTagBomPartDoc = oOccurrenceDeviceTagBom.Definition.Document as Inventor.PartDocument;
            } // デバイスタグ一覧パーツはすでに配置済みの場合


            Inventor.SurfaceBodies oSurfaceBodies = oDeviceTagBomPartDoc.ComponentDefinition.SurfaceBodies;
            PartFeatures oFeatures = oDeviceTagBomPartDoc.ComponentDefinition.Features;
            PartComponentDefinition oDeviceTagBomPartDef = oDeviceTagBomPartDoc.ComponentDefinition;

            foreach (var selectOne in objects)
            {




                SurfaceBody oSourceBody;
                object oSourceBodyProxy = null;
                string oTagOccrenceiPropertyPartName = null;
                ComponentOccurrence oTagOccurrence = null;


            }
            return true;
        }

        private static string AddNewLineWithIncrementedNumber(List<string> A, string newLine)
        {
            int maxNumber = 0;
            foreach (string line in A)
            {
                string[] parts = line.Split('-'); // 行を分割して数値の部分を取得
                if (parts.Length > 1)
                {
                    var x = parts.Last().Trim();

                    if (int.TryParse(x, out int number))
                    {
                        if (number > maxNumber)
                        {
                            maxNumber = number;
                        }
                    }
                }
            }

            maxNumber++; // 一番大きい数値に+1

            // 新しい行を追加
            if (maxNumber > 0)
            {
                A.Add($"{newLine} - {maxNumber}");

                return $"{newLine} - {maxNumber}";
            }
            else
            {
                A.Add(newLine);

                return newLine;
            }
        }
    }
}
