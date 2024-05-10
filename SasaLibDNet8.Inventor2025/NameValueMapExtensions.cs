using Inventor;
using System.Collections.Generic;

namespace SasaLib.InventorAPI
{
    /// <summary>
    /// Autodesk Inventor Interopの世界では、すべてのEnumerable（IEnumerableを実装するオブジェクトタイプ）は1ベースです。確かではありませんが、これがCOM-Interops自体に一般的に当てはまるかどうか.
    /// </summary>
    public static class NameValueMapExtensions
    {
        /// <summary>
        /// NameValueMapsには多くの要素が含まれていないため、KeyValuePair要素を生成する代わりに、完全なディクショナリを返すこともできます。これにより、などの便利なメソッドにアクセスできます.TryGetValue()。
        /// 使い方
        /// IDictionary<string,object> dictionaryContext = Context.ToDictionary();
        /// string  SaveCopyAsFileName dictionaryContext["SaveCopyAsFileName"] as string
        /// </summary>
        /// <param name="nameValueMap"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this NameValueMap nameValueMap)
        {
            var dictionary = new Dictionary<string, object>();
            for (var i = 1; i <= nameValueMap.Count; i++)
                dictionary.Add(nameValueMap.Name[i], nameValueMap.Value[nameValueMap.Name[i]]);
            return dictionary;
        }
    }
}
