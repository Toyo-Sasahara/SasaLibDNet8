using Inventor;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace SasaLib.InventorAPI
{
    /// <summary>
    /// テストクラス・（サーフェス）
    /// </summary>
    public static class Test_InventorControl_Surface
    {
        /// <summary>
        /// このサンプルでは、パーツから別のパーツへのサーフェス ボディのコピーを示します。
        /// これは[上の階層に移動]コマンドと同じですが、API の方がはるかに柔軟性があります。
        /// サンプルを自己完結型にするために、パーツから別のパーツへのボディのコピーを示すために使用される 2 つのパーツを
        /// オンザフライで作成します。
        /// ボディをパーツにコピーする場合は、新しいパーツ内での位置を定義するために、サーフェス ボディとマトリックスを指定してください。
        /// このサンプルでは、アセンブリ内のこれらのパーツの位置に基づいてマトリクスを作成します。
        /// </summary>
        /// <param name="inventorApp"></param>
        public static void CopyBodyFromPartToPart(Inventor.Application inventorApp)
        {
            /*
             * このプログラムの最初の部分では、アセンブリを作成して、コピー本体のAPIを実証できるようにします。
             * どのアセンブリでも潜在的に使用できますが、サンプルが特定のケースに対して単純化されている方がわかりやすいです。
             * これにより、2つの部分を含むアセンブリが作成されます。サンプルの興味深い点は、アセンブリと部品がメモリ内にのみ存在し、
             * ディスクに書き込まれないことです。ファイル名が割り当てられており、保存を実行すると指定したファイル名を使用して
             * ディスクに書き込まれます。
             */
            try
            {
                var asmFullFilename = "C:\\Temp\\CopyBodyTestAsm.iam";

                if (System.IO.File.Exists(asmFullFilename))
                {
                    System.IO.File.Delete(asmFullFilename);
                }

                //inventorApp = System.Runtime.InteropServices.Marshal.GetActiveObject("Inventor.Application") as Inventor.Application;

                // 新しいアセンブリを作成
                AssemblyDocument oAsmDoc = inventorApp.Documents.Add(DocumentTypeEnum.kAssemblyDocumentObject,
                                                    inventorApp.FileManager.GetTemplateFile(DocumentTypeEnum.kAssemblyDocumentObject))
                                                    as AssemblyDocument;

                //アセンブリのファイル名を定義します。この時点では保存されませんが、ユーザーが後で
                //アセンブリを保存する場合にこの名前が使用されます。
                oAsmDoc.FullFileName = asmFullFilename;

                // 新しい部品を作成しますが、目には見えません。
                PartDocument oPartDoc1 = inventorApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                                                    inventorApp.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject),
                                                    false) as PartDocument;

                var part1FullFilename = "C:\\Temp\\CopyBodyTestPart1.ipt";
                if (System.IO.File.Exists(part1FullFilename))
                {
                    System.IO.File.Delete(part1FullFilename);
                }

                // 部品のファイル名を定義します。この時点では保存されませんが、ユーザーが後で
                // 部品を保存する場合にこの名前が使用されます。
                oPartDoc1.FullFileName = part1FullFilename;


                // BRep およびそのジオメトリ フィーチャ拘束を含む)このファイルに含まれるプライマリ ComponentDefinition を取得する
                PartComponentDefinition oCompDef = oPartDoc1.ComponentDefinition;

                //  ComponentDefinition で定義されたすべての平面スケッチをカプセル化する PlanarSketches コレクション オブジェクトを取得します。 
                PlanarSketches oCompoDefSketches = oCompDef.Sketches;

                //  入力平面エンティティに基づいて、新しいスケッチを作成
                PlanarSketch oSketch = oCompoDefSketches.Add(oCompDef.WorkPlanes[1]);

                // 一時的なジオメトリオブジェクトへの参照を設定します。
                TransientGeometry oTG = inventorApp.TransientGeometry;

                // 指定した位置と半径に新しいスケッチ円を作成します
                oSketch.SketchCircles.AddByCenterRadius(oTG.CreatePoint2d(0, 0), 2);

                // PlanarSketch から Profiles コレクション オブジェクトを得ます
                Profiles oProfiles = oSketch.Profiles;

                //  ソリッド フィーチャを作成するために複数のパスを含むプロファイルを作成します
                Profile oProfile = oProfiles.AddForSolid();

                // PartFeatures コレクション オブジェクトを得ます
                PartFeatures oFeatures = oCompDef.Features;

                // 既存の ExtrudeFeature オブジェクトを得ます
                ExtrudeFeatures oExtrudeFeatures = oFeatures.ExtrudeFeatures;

                // 新しい ExtrudeDefinition オブジェクトを作成します。作成されるオブジェクトは、押し出しフィーチャを表しませんが、
                // その代わり、押し出しフィーチャを定義する情報のリプレゼンテーションです。このオブジェクトを
                // ExtrudeFeatures.Add メソッドへの入力として使用して実際のフィーチャを作成することができます
                ExtrudeDefinition oExtrudeDef = oExtrudeFeatures.CreateExtrudeDefinition(oProfile, PartFeatureOperationEnum.kJoinOperation);

                // 範囲を「距離」の範囲に変更するメソッドです。このメソッドが非対称の押し出しに対して呼び出されると、2 番目の方向範囲は削除されます。範囲の値を変更するには、Extent プロパティによって返されたオブジェクトのプロパティを使用します。
                oExtrudeDef.SetDistanceExtent(3, PartFeatureExtentDirectionEnum.kPositiveExtentDirection);

                // 押し出しフィーチャーを作成
                ExtrudeFeature oExtrude = oExtrudeFeatures.Add(oExtrudeDef);

                // 作業サーフェスとして2番目の押し出し特徴を作成します。

                // ある程度任意の位置にオカレンスをアセンブリに挿入します。
                Inventor.Matrix oMatrix = oTG.CreateMatrix();
                oMatrix.Translation.AddVector(oTG.CreateVector(2, 3, 4));
                oMatrix.SetToRotation(0.5, oTG.CreateVector(1, 0, 0), oTG.CreatePoint(2, 3, 4));


                oSketch = oPartDoc1.ComponentDefinition.Sketches.Add(
                            oPartDoc1.ComponentDefinition.WorkPlanes[1]);
                oSketch.SketchLines.AddAsTwoPointRectangle(oTG.CreatePoint2d(-3, -3), oTG.CreatePoint2d(3, 3));
                oProfile = oSketch.Profiles.AddForSolid();

                oExtrudeDef = oCompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(oProfile, PartFeatureOperationEnum.kSurfaceOperation);
                oExtrudeDef.SetDistanceExtent(1.5, PartFeatureExtentDirectionEnum.kPositiveExtentDirection);
                oExtrude = oCompDef.Features.ExtrudeFeatures.Add(oExtrudeDef);

                ComponentOccurrence oOcc1 = oAsmDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(
                                                oPartDoc1.ComponentDefinition as Inventor.ComponentDefinition, oMatrix);


                {
                    var part2FullFilename = "C:\\Temp\\CopyBodyTestPart2.ipt";
                    if (System.IO.File.Exists(part2FullFilename))
                    {
                        System.IO.File.Delete(part2FullFilename);
                    }

                    //  2番目の新しい部品を、目に見えないように作成します。
                    PartDocument oPartDoc2 = inventorApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                                                        inventorApp.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject),
                                                        false) as PartDocument;
                    // 部品のファイル名を定義します。この時点では保存されませんが、ユーザーが後で部品を保存する場合にこの名前が使用されます。
                    oPartDoc2.FullFileName = part2FullFilename;


                    // 2番目の部品を、ある程度任意の位置に挿入してアセンブリに組み込みます。
                    oMatrix = oTG.CreateMatrix();
                    oMatrix.Translation.AddVector(oTG.CreateVector(-1, -1, -1));
                    oMatrix.SetToRotation(0.5, oTG.CreateVector(1, 1, 0), oTG.CreatePoint(1, 1, 1));
                    ComponentOccurrence oOcc2 = oAsmDoc.ComponentDefinition.Occurrences.AddByComponentDefinition(
                                                    oPartDoc2.ComponentDefinition as Inventor.ComponentDefinition, oMatrix);

                    //最初の部品から作業サーフェスを表す表面ボディを取得します。この場合、作業サーフェスが1つしかないことがわかっているので、これは単にコレクション内の最初の作業サーフェスを取得します。
                    WorkSurface oPartDoc1WorkSurface = oPartDoc1.ComponentDefinition.WorkSurfaces[1];
                    SurfaceBody oBody = oPartDoc1WorkSurface.SurfaceBodies[1];

                    // 表面ボディを1つの部品から別の部品にコピーする際に使用する行列を定義します。
                    // 任意の行列を使用できますが、この場合、ボディの位置をアセンブリ空間に対して
                    // 同じにしたいです。オカレンスがアセンブリ内の異なる位置にあるため、
                    // 行列は一つのオカレンスからもう一方のオカレンスへの変換を考慮する必要があります。


                    //現在表面ボディが存在するオカレンスから変換を取得します。これにより、表面ボディの変換がアセンブリ空間に対するものと定義されます。
                    oMatrix = oOcc1.Transformation;

                    // 2番目のオカレンスの行列を取得し、それを反転します。2番目のオカレンスの変換の逆行列は、
                    // アセンブリ空間からそのオカレンスの部品空間への変換を定義します。
                    Inventor.Matrix oMatrix2 = oOcc2.Transformation;
                    oMatrix2.Invert();

                    // これらの行列を組み合わせます。
                    oMatrix.PreMultiplyBy(oMatrix2);

                    // 最初の部品から表面ボディを2番目の部品にコピーします。新しいボディが既存のボディの上に直接配置されているため、グラフィック的には何も変化しないはずですが、新しいボディが2番目の部品にあることを確認できます。
                    oPartDoc2.ComponentDefinition.Features.NonParametricBaseFeatures.Add(oBody, oMatrix);

                    Console.WriteLine("Surface copied successfully.");
                }

                //inventorApp.Quit();


            }
            catch (Exception ex)
            {
                ;
            }

        }

        public static void CopyBodyFromPartToPart2(Inventor.Application inventorApp)
        {
            try
            {

                PartDocument oPartDoc2 = inventorApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                                    inventorApp.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject),
                                    false) as PartDocument;


                SelectSet oSelecset = inventorApp.ActiveDocument.SelectSet;
                var oSelectOne = oSelecset[1];
                Inventor.ObjectTypeEnum oType = InventorControl.GetInventorObjectType(oSelectOne);

                Inventor.ComponentOccurrence oCompoOcc = oSelectOne as ComponentOccurrence;

                Inventor.Document oRefDoc = oCompoOcc.Definition.Document as Inventor.Document;

                Inventor.ComponentDefinitions componentDefinitions = ((PartDocument)oRefDoc).ComponentDefinition as ComponentDefinitions;

                Inventor.ComponentDefinition componentDefinition = ((PartDocument)oRefDoc).ComponentDefinition as ComponentDefinition;
                var x = componentDefinition.SurfaceBodies;
                SurfaceBody oBody1 = x[1];


                // 一時的なジオメトリオブジェクトへの参照を設定します。
                TransientGeometry oTG = inventorApp.TransientGeometry;

                Inventor.Matrix oMatrix = oTG.CreateMatrix();
                oMatrix.Translation.AddVector(oTG.CreateVector(2, 3, 4));
                oMatrix.SetToRotation(0.5, oTG.CreateVector(1, 0, 0), oTG.CreatePoint(2, 3, 4));



                //部品から作業サーフェスを表す表面ボディを取得します。
                WorkSurface oWorkSurface = ((PartDocument)oRefDoc).ComponentDefinition.WorkSurfaces[1];
                SurfaceBody oBody = oWorkSurface.SurfaceBodies[1];

            }
            catch (Exception ex)
            {

            }

        }
    }
}
