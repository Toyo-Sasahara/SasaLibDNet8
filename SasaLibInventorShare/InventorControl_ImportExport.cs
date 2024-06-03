using Inventor;
using SasaLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

//using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
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
        /// AnyCAD
        /// ■AnyCAD対応 ファイル（パーツ若しくはアセンブリ）を Inventor のパーツにインポート
        /// テンプレートファイルが開かれインポートされる
        /// 使用先プロジェクト InventorTOYOaddin01
        /// </summary>
        /// <param name="oApp"></param>
        public static void ImportAnyCadToNewPart(Inventor.Application oApp)
        {
            Inventor.FileDialog oFileDlg;

            oApp.CreateFileDialog(out oFileDlg);
            oFileDlg.Filter = "SolidWorksパーツファイル (*.sldprt)|*.sldprt";
            oFileDlg.FilterIndex = 1;
            oFileDlg.DialogTitle = "SolidWorksパーツファイルを選択";

            oFileDlg.InitialDirectory = @"D:\EPDM";

            oFileDlg.CancelError = true;

            try
            {
                oFileDlg.ShowOpen();

                //MessageBox.Show($"{ oFileDlg.FileName}");

                //var saveFolder = System.IO.Path.GetDirectoryName(oFileDlg.FileName);
                var saveFolder = System.IO.Path.GetTempPath();

                ImportAnyCadToNewPart(oApp, oFileDlg.FileName, saveFolder);

                return;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
                return;
            }
        }

        /// <summary>
        /// AnyCAD
        /// ■AnyCAD対応 ファイル（パーツ若しくはアセンブリ）を Inventor のパーツにインポートして書出し
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="FilePath"></param>
        /// <param name="SaveFolder"></param>
        public static void ImportAnyCadToNewPart(Inventor.Application oApp, string FilePath, string SaveFolder)
        {
            Inventor.PartDocument oDoc = (Inventor.PartDocument)oApp.Documents.Add(Inventor.DocumentTypeEnum.kPartDocumentObject);
            Inventor.PartComponentDefinition oPartCompDef = oDoc.ComponentDefinition;

            // Create the ImportedGenericComponentDefinition bases on an Alias file
            // エイリアスファイルをベースにImportedGenericComponentDefinitionを作成します。
            Inventor.ImportedGenericComponentDefinition oImportedGenericCompDef = (Inventor.ImportedGenericComponentDefinition)oPartCompDef.ReferenceComponents.ImportedComponents.CreateDefinition(FilePath);

            oImportedGenericCompDef.ReferenceModel = false;
            oImportedGenericCompDef.SaveFilesLocation = SaveFolder;
            oImportedGenericCompDef.IncludeAll();

            // Import the Alias 
            // エイリアスのインポート  
            Inventor.ImportedComponent oImportedComp = oPartCompDef.ReferenceComponents.ImportedComponents.Add((Inventor.ImportedComponentDefinition)oImportedGenericCompDef);
        }

        /// <summary>
        /// AnyCAD
        /// ■AnyCAD対応 ファイル（パーツ若しくはアセンブリ）を Inventor のアセンブリにインポート
        /// テンプレートファイルが開かれインポートされる
        /// 使用先プロジェクト InventorTOYOaddin01
        /// </summary>
        /// <param name="oApp"></param>
        public static void ImportAnyCadToNewAssembly_org(Inventor.Application oApp)
        {

            string filename = FileSelectForRead(oApp, "SolidWorksファイルを選択", @"D:\EPDM", "*.sldprt");

            Inventor.AssemblyDocument oDoc = (Inventor.AssemblyDocument)oApp.Documents.Add(Inventor.DocumentTypeEnum.kAssemblyDocumentObject);
            Inventor.AssemblyComponentDefinition oAssyCompDef = oDoc.ComponentDefinition;


            // Create the ImportedGenericComponentDefinition bases on an Alias file
            // エイリアスファイルをベースにImportedGenericComponentDefinitionを作成します。
            Inventor.ImportedGenericComponentDefinition oImportedGenericCompDef = (Inventor.ImportedGenericComponentDefinition)oAssyCompDef.ImportedComponents.CreateDefinition(filename);

            // Set the ReferenceModel to associatively import the Alias file, set this property to False will just convert the
            // Alias ファイルを連想的にインポートするために ReferenceModel を設定します。
            oImportedGenericCompDef.ReferenceModel = true;
            oImportedGenericCompDef.IncludeAll();

            // Import the Alias 
            Inventor.ImportedComponent oImportedComp = oAssyCompDef.ImportedComponents.Add((Inventor.ImportedComponentDefinition)oImportedGenericCompDef);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oApp"></param>
        /// <param name="OpenLocation"></param>
        /// <param name="SaveFilesLocation"></param>
        /// <param name="ipt_replaceStartString"></param>
        /// <param name="iam_replaceStartString"></param>
        public static void ConvertSolidworksModel(Inventor.Application oApp, string OpenLocation = @"D:\EPDM", string SaveFilesLocation = @"D:\MainVault\_製番別\一時的作業", string ipt_replaceStartString = null, string iam_replaceStartString = null)
        {
            string filename = InventorControl.FileSelectForRead(oApp, "SolidWorksファイルを選択", @"D:\EPDM", "*.sldprt;*.sldasm");

            if (string.IsNullOrEmpty(filename) == true)
                return;

            Inventor.AssemblyDocument oDoc = (Inventor.AssemblyDocument)oApp.Documents.Add(Inventor.DocumentTypeEnum.kAssemblyDocumentObject);
            Inventor.AssemblyComponentDefinition oAssyCompDef = oDoc.ComponentDefinition;


            // Create the ImportedGenericComponentDefinition bases on an Alias file
            // エイリアスファイルをベースにImportedGenericComponentDefinitionを作成します。
            Inventor.ImportedGenericComponentDefinition oImportedGenericCompDef = default;
            try
            {
                oImportedGenericCompDef = (Inventor.ImportedGenericComponentDefinition)oAssyCompDef.ImportedComponents.CreateDefinition(filename);

                oImportedGenericCompDef.ReferenceModel = false;
                oImportedGenericCompDef.SaveFilesLocation = SaveFilesLocation.TrimEnd('\\') + "\\"; // 最後は\が必要
                oImportedGenericCompDef.IncludeAll();

                // Import the Alias 
                Inventor.ImportedComponent oImportedComp = oAssyCompDef.ImportedComponents.Add((Inventor.ImportedComponentDefinition)oImportedGenericCompDef);
            }
            catch { }

            ComponentOccurrences oOccurrences = oAssyCompDef.Occurrences;
            int nowCount = 0;

            fileNameChange_Recursive(oOccurrences, ref nowCount, ipt_replaceStartString, iam_replaceStartString);
        }

        private static void fileNameChange_Recursive(Inventor.ComponentOccurrences inCollection, ref int nowCount, string ipt_replaceStartString = null, string iam_replaceStartString = null)
        {
            try
            {
                //現在のコレクション内のコンポーネントを反復処理する
                IEnumerator objoccsEnumerator = inCollection.GetEnumerator();

                // アセンブリ内の個々のオカレンスを表スオブジェクトを宣言
                Inventor.ComponentOccurrence objcompOccurrence;

                while (objoccsEnumerator.MoveNext() == true)
                {
                    // オカレンスオブジェクト
                    objcompOccurrence = (Inventor.ComponentOccurrence)objoccsEnumerator.Current;

                    // 固定ピン
                    objcompOccurrence.Flexible = true;

                    try
                    {
                        var FullFileName0 = ((Inventor.Document)objcompOccurrence.DefinitionReference.ReferencedDefinition.Document).FullFileName;
                        //var FullFileName = objcompOccurrence.DefinitionReference.ReferencedFileDescriptor.FullFileName;
                    }
                    catch { }

                    nowCount++;

                    bool ReferenceMissing = objcompOccurrence.ReferencedDocumentDescriptor.ReferenceMissing;


                    // 関連付けられている外部ファイルの完全なファイル名を返す読み取り専用プロパティです。このプロパティは、IsAssociativelyImported が False を返す場合、空の文字列を返します。
                    try
                    {
                        string AssociativeForeignFilename = objcompOccurrence.AssociativeForeignFilename;
                    }
                    catch { }

                    DocumentDescriptor oRefdocDesc = default;
                    // このドキュメントが保持するネイティブのドキュメント参照を表す記述子の列挙値を返すプロパティです。
                    try
                    {
                        oRefdocDesc = objcompOccurrence.ReferencedDocumentDescriptor;
                    }
                    catch { }

                    //このドキュメントのユーザが表示可能な名前を取得および設定します。既定でファイル名に設定されます。
                    string oRefdocDescDisplayname = oRefdocDesc.DisplayName;

                    //ドキュメントの完全な修飾名を返すプロパティです。この文字列は、ドキュメント名が連続する完全なファイル名で、ドキュメントの一意の識別子です。ドキュメントの名前は、Document オブジェクトの Name プロパティによって返されます。
                    string oRefdocDescFullDocumentName = oRefdocDesc.FullDocumentName;

                    //string oOccurrenceDefnitionDocumentFullFileName = default;
                    string oOccurrenceReferencedFileDescriptorFullFileName = default;

                    try
                    {
                        oOccurrenceReferencedFileDescriptorFullFileName = ((Inventor.Document)objcompOccurrence.DefinitionReference.ReferencedDefinition.Document).FullFileName;
                        //oOccurrenceDefnitionDocumentFullFileName = ((Document)objcompOccurrence.Definition.Document).FullFileName; // ミッシングリンクファイルだと例外が出る
                        //oOccurrenceReferencedFileDescriptorFullFileName = objcompOccurrence.ReferencedFileDescriptor.FullFileName;
                    }
                    catch (Exception ex)
                    {
                        DebugConsole.WriteLine($"※InventorControl.fileNameChange_Recursive(..)にて 例外発生③{ex.Message}");
                    }


                    if (ReferenceMissing == false)
                    {
                    }


                    //WriteLine($"■■■■処理残／総数 {nowCount} / {this.totalFileCount} {objcompOccurrence.Name} {oOccurrenceReferencedFileDescriptorFullFileName} リンク有効:{!ReferenceMissing}");






                    ComponentOccurrencesEnumerator objcompOccurrenceSubOccrrence = default;
                    try
                    {
                        objcompOccurrenceSubOccrrence = objcompOccurrence.SubOccurrences;
                    }
                    catch (Exception ex)
                    {
                        DebugConsole.WriteLine($"例外発生③{ex.Message}");

                    }

                    try
                    {
                        var FullFileName = objcompOccurrence.ReferencedFileDescriptor.FullFileName;

                        //var FullFileName = objcompOccurrence.DefinitionReference.ReferencedFileDescriptor.FullFileName;
                    }
                    catch { }

                    Document oDDDD = objcompOccurrence.DefinitionReference.ReferencedDefinition.Document as Document;


                    //現在のコンポーネントがアセンブリか部品かを判断する
                    if (objcompOccurrence.DefinitionDocumentType == Inventor.DocumentTypeEnum.kPartDocumentObject)
                    {
                        if (string.IsNullOrEmpty(ipt_replaceStartString) == false)
                        {
                            var orgFileName = System.IO.Path.GetFileName(oDDDD.FullFileName);
                            var orgPathName = System.IO.Path.GetDirectoryName(oDDDD.FullFileName);

                            //orgFileName = Regex.Replace(orgFileName, @"^\(SW\)", "");
                            orgFileName = orgFileName.Replace(ipt_replaceStartString, "");
                            orgFileName = $"{ipt_replaceStartString}{orgFileName}";
                            try
                            {
                                oDDDD.FullFileName = System.IO.Path.Combine(orgPathName, orgFileName);
                            }
                            catch { }
                        }
                    }
                    else if (objcompOccurrence.DefinitionDocumentType == Inventor.DocumentTypeEnum.kAssemblyDocumentObject)
                    {
                        if (string.IsNullOrEmpty(iam_replaceStartString) == false)
                        {
                            var orgFileName = System.IO.Path.GetFileName(oDDDD.FullFileName);
                            var orgPathName = System.IO.Path.GetDirectoryName(oDDDD.FullFileName);

                            //orgFileName = Regex.Replace(orgFileName, @"^\(SW\)", "");
                            orgFileName = orgFileName.Replace(iam_replaceStartString, "");
                            orgFileName = $"{iam_replaceStartString}{orgFileName}";

                            oDDDD.FullFileName = System.IO.Path.Combine(orgPathName, orgFileName);
                        }

                        if (objcompOccurrenceSubOccrrence != null)
                        {
                            fileNameChange_Recursive((ComponentOccurrences)objcompOccurrenceSubOccrrence, ref nowCount, ipt_replaceStartString, iam_replaceStartString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteLine($"例外検知② {ex.Message}");
            }
        }


    }
}
