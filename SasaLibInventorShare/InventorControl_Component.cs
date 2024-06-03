using Inventor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SasaLib.InventorAPI
{
    public static partial class InventorControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oActiveDocument"></param>
        /// <param name="InternalName"></param>
        /// <returns></returns>
        public static ComponentOccurrence GetComponetOccurrenceOne(Document oActiveDocument, string InternalName)
        {
            AssemblyDocument oAsmDoc = (AssemblyDocument)oActiveDocument;
            AssemblyComponentDefinition oAsmDef = oAsmDoc.ComponentDefinition;
            SelectSet selectSet = oAsmDoc.SelectSet;
            ComponentOccurrencesEnumerator oLeafOccs = oAsmDef.Occurrences.AllLeafOccurrences;

            foreach (ComponentOccurrence oOcc in oLeafOccs)
            {
                Document document = (Inventor.Document)oOcc.Definition.Document;
                if (document.InternalName == InternalName)
                {
                    return oOcc;
                }
            }
            return null;
        }

        /// <summary>
        /// アセンブリドキュメントの直下にオカレンスが含まれるか調査する
        /// </summary>
        /// <param name="document"></param>
        /// <param name="occurrence"></param>
        /// <returns></returns>
        static bool IsOccurrenceInDocument(AssemblyDocument document, ComponentOccurrence occurrence)
        {
            var documentFullFileName = document.FullFileName;

            foreach (ComponentOccurrence docOccurrence in document.ComponentDefinition.Occurrences)
            {
                if (docOccurrence.Equals(occurrence))
                {
                    return true;
                }
            }

            return false;
        }
    }

}
