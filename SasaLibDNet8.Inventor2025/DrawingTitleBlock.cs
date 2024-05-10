using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.InventorAPI
{
    public class DrawingTitleBlock
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
        Inventor.TitleBlockDefinitions TitleBlockDefinitions;

        /// <summary>
        /// 
        /// </summary>
        List<string> titleBlockNames = new List<string>();


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="application"></param>
        public DrawingTitleBlock(Inventor.Application application)
        {
            this.application = application;
            drawingDocument = (Inventor.DrawingDocument)this.application.ActiveDocument;
            TitleBlockDefinitions = drawingDocument.TitleBlockDefinitions;
        }

        /// <summary>
        /// アクティブドキュメントのTitleBlockDefinitions　を得る
        /// </summary>
        /// <returns></returns>
        public Inventor.TitleBlockDefinitions GetTitleBlockDefinitions()
        {
            return TitleBlockDefinitions;
        }

        /// <summary>
        /// アクティブドキュメントのタイトルブロックの一覧を得る
        /// </summary>
        /// <returns></returns>
        public List<string> GetTitleBlockNames()
        {
            titleBlockNames.Clear();
            foreach (Inventor.TitleBlockDefinition titleBlkDefinition in TitleBlockDefinitions)
            {
                titleBlockNames.Add(titleBlkDefinition.Name);
            }
            return titleBlockNames;
        }

        /// <summary>
        /// シートの現在のタイトルブロックを変更する
        /// </summary>
        /// <param name="strTitleblockName">タイトルブロック名, null の場合は現在のタイトルブロックjを削除する</param>
        /// <returns></returns>
        public bool ChangeTitleBlcok(string strTitleblockName)
        {
            Inventor.Sheet sheet = drawingDocument.ActiveSheet;

            if (strTitleblockName == null)
            {
                if (sheet.TitleBlock != null)
                {
                    sheet.TitleBlock.Delete();

                }
                return true;
            }
            else
            {
                if (sheet.TitleBlock != null)
                {
                    sheet.TitleBlock.Delete();
                }

                try
                {
                    Inventor.TitleBlockDefinition titleBlockDefinition = drawingDocument.TitleBlockDefinitions[strTitleblockName];
                    Inventor.TitleBlock titleBlock = sheet.AddTitleBlock(titleBlockDefinition);
                }
                catch (Exception ex)
                {
                    DebugConsole.WriteLine($"ChangeTitleBlcok(..) {ex.Message}");
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// シートの現在のタイトルブロック名を得る
        /// </summary>
        /// <returns></returns>
        public string GetCurrentTitleBlockName()
        {
            Inventor.Sheet sheet = drawingDocument.ActiveSheet;

            if (sheet.TitleBlock == null)
            {
                return null;
            }
            else
            {
                string ans = sheet.TitleBlock.Name;
                return ans;
            }
        }
    }
}
