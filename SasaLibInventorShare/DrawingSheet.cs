using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.InventorAPI
{
    public class DrawingSheet
    {
        /// <summary>
        /// 
        /// </summary>
        Inventor.Application application;

        /// <summary>
        /// 
        /// </summary>
        Inventor.DrawingDocument drawingDocument;
        

        /// <summary>
        /// 
        /// </summary>
        List<string> sheetNames = new List<string>();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="application"></param>
        public DrawingSheet(Inventor.Application application)
        {
            this.application = application;
            drawingDocument = (Inventor.DrawingDocument)this.application.ActiveDocument;
        }

        /// <summary>
        /// アクティブドキュメントのシート一覧を得る
        /// </summary>
        /// <returns></returns>
        public List<string> GetSheetNames()
        {
            sheetNames.Clear();
            foreach (Inventor.Sheet sheet in drawingDocument.Sheets)
            {
                sheetNames.Add(sheet.Name);
            }
            return sheetNames;
        }

        /// <summary>
        /// シートの現在のタイトルブロックを変更する
        /// </summary>
        /// <param name="strSheetName">タイトルブロック名, null の場合は現在のタイトルブロックjを削除する</param>
        /// <returns></returns>
        public bool ChangeActiveSheet(string strSheetName)
        {
            Inventor.Sheet sheet = drawingDocument.ActiveSheet;

            if (strSheetName == null)
            {
                if (sheet != null)
                {
                    sheet.Delete();

                }
                return true;
            }
            else
            {
                if (sheet != null)
                {
                    sheet.Delete();
                }

                try
                {
                    Inventor.Sheet newsheet = drawingDocument.Sheets[strSheetName];
                    newsheet.Activate();
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"ChangeSheet(..) {ex.Message}");
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// アクティブドキュメントの現在のシート名を得る
        /// </summary>
        /// <returns></returns>
        public string GetActiveSheetName()
        {
            Inventor.Sheet sheet = drawingDocument.ActiveSheet;

            if (sheet == null)
            {
                return null;
            }
            else
            {
                string ans = sheet.Name;
                return ans;
            }
        }

        /// <summary>
        /// アクティブドキュメントの現在のシートのサイズの種類を得る
        /// </summary>
        /// <returns></returns>
        public void GetActiveSheetCurrnetSizeAndOrient(out DrawingSheetSizeEnum Size, out PageOrientationTypeEnum Orient)
        {
            Size = drawingDocument.ActiveSheet.Size;
            Orient = drawingDocument.ActiveSheet.Orientation;
        }

        /// <summary>
        /// アクティブドキュメントの現在のシートサイズを変更する
        /// </summary>
        /// <param name="size"></param>
        /// <param name="orent"></param>
        public void SetActiveSheetCurrnetSizeAndOrient(DrawingSheetSizeEnum size, PageOrientationTypeEnum orent)
        {
            Inventor.Sheet sheet = drawingDocument.ActiveSheet;
            sheet.Size = size;
            sheet.Orientation = orent;
        }
    }
}
