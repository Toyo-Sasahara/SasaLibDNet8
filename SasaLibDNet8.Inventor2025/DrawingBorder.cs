using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.InventorAPI
{
    /// <summary>
    /// Invenotor 図面枠クラス
    /// </summary>
    public class DrawingBorder
    {
        /// <summary>
        /// アプリケーションオブジェクト
        /// </summary>
        Inventor.Application application;

        /// <summary>
        /// ドローイングドキュメントオブジェクト
        /// </summary>
        Inventor.DrawingDocument drawingActiveDocument;

        /// <summary>
        /// ボーダー（図面枠）デフィニション（定義）
        /// </summary>
        Inventor.BorderDefinitions borderDefinitions;

        /// <summary>
        /// 
        /// </summary>
        List<string> borderNames = new List<string>();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="application"></param>
        public DrawingBorder(Inventor.Application application)
        {
            this.application = application;
            drawingActiveDocument = (Inventor.DrawingDocument)this.application.ActiveDocument;
            borderDefinitions = drawingActiveDocument.BorderDefinitions;
        }

        /// <summary>
        /// アクティブドキュメントの　を得る
        /// </summary>
        /// <returns></returns>
        public Inventor.BorderDefinitions GetBorderDefinitions()
        {
            return borderDefinitions;
        }

        /// <summary>
        /// アクティブドキュメントの一覧を得る
        /// </summary>
        /// <returns></returns>
        public List<string> GetBorderNames()
        {
            borderNames.Clear();
            foreach (Inventor.BorderDefinition borderDefinition in borderDefinitions)
            {
                borderNames.Add(borderDefinition.Name);
            }
            return borderNames;
        }

        /// <summary>
        /// シートの現在のを変更する
        /// </summary>
        /// <param name="strBorderName">名, null の場合は現在のタイトルブロックjを削除する</param>
        /// <returns></returns>
        public bool ChangeBorder(string strBorderName)
        {
            Inventor.Sheet sheet = drawingActiveDocument.ActiveSheet;

            if (strBorderName == null)
            {
                if (sheet.Border != null)
                {
                    sheet.Border.Delete();

                }
                return true;
            }
            else
            {
                if (sheet.Border != null)
                {
                    sheet.Border.Delete();
                }

                try
                {
                    Inventor.BorderDefinition borderDefinition = drawingActiveDocument.BorderDefinitions[strBorderName];

                    if (borderDefinition.IsDefault)
                    {
                        Inventor.DefaultBorder boarder =  sheet.AddDefaultBorder();
                    }
                    else
                    {
                        Inventor.Border border = sheet.AddBorder(borderDefinition);
                    }
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"ChangeBorder(..) {ex.Message}");
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// シートの現在の名を得る
        /// </summary>
        /// <returns></returns>
        public string GetCurrentBorderName()
        {
            Inventor.Sheet sheet = drawingActiveDocument.ActiveSheet;

            if (sheet.Border == null)
            {
                return null;
            }
            else
            {
                string ans = sheet.Border.Name;
                return ans;
            }
        }
        
        /// <summary>
        /// アクティブドキュメントのすべてのシートに対して図面枠と表題欄を指定したファイルの
        /// </summary>
        /// <param name="sourceDocument"></param>
        public void ReplaceBorderAndSheet(string sourceDocument)
        {
            DrawingDocument oFromDocument  = default;
            try
            {
                oFromDocument = (DrawingDocument)application.Documents.Open(sourceDocument, false);

                TitleBlockDefinition oSourceTitleBlockDef = oFromDocument.ActiveSheet.TitleBlock.Definition;
                BorderDefinition oSourceBorderDef = oFromDocument.ActiveSheet.Border.Definition;

                // タイトルブロックを適応する今の図面
                DrawingDocument oTargetDocument = (drawingActiveDocument);

                // ボーダー定義を取得します
                BorderDefinition oNewBorderBlockDef = oSourceBorderDef.CopyTo((_DrawingDocument)oTargetDocument);
                // タイトルブロックの定義を取得します
                TitleBlockDefinition oNewTitleBlockDef = oSourceTitleBlockDef.CopyTo((_DrawingDocument)oTargetDocument);

                // シートを反復してタイトルブロック定義とボーダーを入れ替える
                foreach (Sheet oSheet in oTargetDocument.Sheets)
                {
                    oSheet.Activate();

                    if (oSheet.TitleBlock != null)
                    {
                        oSheet.TitleBlock.Delete();
                    }
                    if (oSheet.Border != null)
                    {
                        oSheet.Border.Delete();
                    }

                    oSheet.AddTitleBlock(oNewTitleBlockDef);
                    oSheet.AddBorder(oNewBorderBlockDef);
                }

                oFromDocument.Close(true);
            }
            catch (Exception ex)
            {
                if (oFromDocument != null)
                    oFromDocument.Close(true);

                DebugConsole.WriteLine($"{ex.Message}");
            }

            /*
            '現在のドキュメント
            Private Sub TitleBlockAndBorderCopyFromExecute(sourceFilename As String)
    
                '読み込むタイトルブロックが含まれる図面
                Dim oFromDocument As DrawingDocument
                Set oFromDocument = ThisApplication.Documents.Open(sourceFilename, False)
    
                ' 新しいソース タイトル ブロックの定義を取得します。
                Dim oSourceTitleBlockDef As TitleBlockDefinition
                Set oSourceTitleBlockDef = oFromDocument.ActiveSheet.TitleBlock.Definition
       
                '
                Dim oSourceBorderDef As BorderDefinition
                Set oSourceBorderDef = oFromDocument.ActiveSheet.Border.Definition
    
    
                'タイトルブロックを適応する今の図面
                Dim oTargetDocument As DrawingDocument
                Set oTargetDocument = ThisApplication.ActiveDocument
      
                Dim oNewBorderBlockDef As BorderDefinition
                Set oNewBorderBlockDef = oSourceBorderDef.CopyTo(oTargetDocument)
      
                ' 新しいタイトルブロックの定義を取得します。
                Dim oNewTitleBlockDef As TitleBlockDefinition
                Set oNewTitleBlockDef = oSourceTitleBlockDef.CopyTo(oTargetDocument)
    
                ' シートを反復してタイトルブロック定義とボーダーを入れ替える
                Dim oSheet As Sheet
                For Each oSheet In oTargetDocument.Sheets
                    oSheet.Activate
        
                    If Not (oSheet.TitleBlock Is Nothing) Then
                        oSheet.TitleBlock.Delete
                    End If
          
                    If Not (oSheet.Border Is Nothing) Then
                        oSheet.Border.Delete
                    End If
        
                    Call oSheet.AddTitleBlock(oNewTitleBlockDef)
                    Call oSheet.AddBorder(oNewBorderBlockDef)
                Next
            End Sub          
            */
        }
    }
}
