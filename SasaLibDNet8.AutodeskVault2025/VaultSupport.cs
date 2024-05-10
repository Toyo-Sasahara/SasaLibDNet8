// TODO: 未使用クラス

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.Services.Protocols;
//using System.Xml;

//namespace SasaLib.AutodeskVault
//{
//    public static class VaultSupport
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="e"></param>
//        /// <param name="errorCode"></param>
//        /// <param name="restrictionCodes"></param>
//        public static void GetErrorAndRestrictionCodesString(Exception e, out string errorCode, out List<string> restrictionCodes)
//        {
//            SoapException se = e as SoapException;
//            errorCode = null;
//            restrictionCodes = new List<string>();
//            string[] restrictionErrors = new string[]
//            { "1092", "1387", "1633" };

//            if (se != null)
//            {
//                try
//                {
//                    errorCode = se.Detail["sl:sldetail"]["sl:errorcode"].InnerText.Trim();

//                    if (restrictionErrors.Contains(errorCode))
//                    {
//                        XmlNodeList nodes = se.Detail["sl:sldetail"]["sl:restrictions"].ChildNodes;

//                        foreach (XmlNode node in nodes)
//                        {
//                            if (node.Name == "sl:restriction")
//                            {
//                                XmlElement element = node as XmlElement;
//                                if (element != null)
//                                    restrictionCodes.Add(element.GetAttribute("sl:code"));
//                            }
//                        }
//                    }
//                }
//                catch
//                { }
//            }
//        }

//    }
//}
