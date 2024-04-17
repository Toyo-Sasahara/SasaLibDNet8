using SasaLibDNet8;
using System;
using System.IO;

namespace SasaLibDNet8
{
    public static class XmlModify
    {
        /// <summary>
        /// XMLの指定した要素名を別の要素名に置き換え、値も変更
        /// </summary>
        /// <param name="XmlFile">書き換えるXMLファイルのフルパス</param>
        /// <param name="CurrentElement"></param>
        /// <param name="NewElement"></param>
        /// <returns>true:正常終了</returns>
        public static bool XmlTagUpdate(string XmlFile, string CurrentElement, string NewElement, string Value, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            WriteLine($"XmlModify.XmlTagUpdate(\"{XmlFile}\" , \"{CurrentElement}\" , \"{NewElement}\" , \"{Value}\")を実行します");

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            try
            {
                // XMLファイルを読み込む
                xmlDoc.Load(XmlFile);
            }
            catch (Exception ex)
            {
                WriteLine($"XmlTagUpdate(...)で例外発生。　{ex.Message}");
                WriteLine($"XmlTagUpdate(...) {XmlFile}を読み込めません");
                return false;
            }

            //ルート要素を取得する
            System.Xml.XmlElement rootElement = xmlDoc.DocumentElement;

            //ルート要素の子要素を取得する
            System.Xml.XmlNodeList nodelist = rootElement.GetElementsByTagName(CurrentElement);

            //指定したCurrentElementが存在するか？
            if (nodelist.Count > 0)
            {
                //新しいNewElementを作成する
                System.Xml.XmlElement newRoot = xmlDoc.CreateElement(NewElement);
                newRoot.InnerText = Value;

                //CurrentElementをNewElementに置換する
                nodelist.Item(0).ParentNode.ReplaceChild(newRoot, nodelist.Item(0));
            }

            //ファイルに保存する
            xmlDoc.Save(XmlFile);

            return true;
        }

        /// <summary>
        /// XMLの指定した要素名を別の要素名に置き換え、値も変更。新しいファイルにて保存
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="distFile"></param>
        /// <param name="CurrentElement"></param>
        /// <param name="NewElement"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool XmlTagUpdate(string sourceFile, string distFile, string CurrentElement, string NewElement, string Value, SasaLibDelegateWriteLine WriteLine = null)
        {
            if (WriteLine == null) WriteLine = DebugConsole.WriteLine;

            FileFolder.ChangeExtension(distFile, "$$$");
            File.Copy(sourceFile, distFile, true);
            bool ans = XmlTagUpdate(distFile, CurrentElement,NewElement,Value, WriteLine:WriteLine);
            return ans;
        }
    }
}
