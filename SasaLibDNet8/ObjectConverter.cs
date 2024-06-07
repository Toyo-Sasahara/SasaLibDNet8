using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SasaLib.PIPE
{
    /// <summary>
    /// オブジェクトとバイト配列の変換
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class ObjectConverter<TObject>
    {
#pragma warning disable SYSLIB0011 // 型またはメンバーが旧型式です
        private readonly IFormatter _formatter;
#pragma warning restore SYSLIB0011 // 型またはメンバーが旧型式です

#pragma warning disable SYSLIB0011 // 型またはメンバーが旧型式です
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatter"></param>
        public ObjectConverter(IFormatter? formatter = null)
#pragma warning restore SYSLIB0011 // 型またはメンバーが旧型式です
        {
#pragma warning disable SYSLIB0011 // 型またはメンバーが旧型式です
            _formatter = formatter ?? new BinaryFormatter();
#pragma warning restore SYSLIB0011 // 型またはメンバーが旧型式です
        }

        /// <summary>
        /// オブジェクト=>バイト配列
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public byte[] ToByteArray(TObject obj)
        {
            using (var stream = new MemoryStream())
            {
                _formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// バイト配列=>オブジェクト
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public TObject FromByteArray(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return (TObject)_formatter.Deserialize(stream);
            }
        }
    }
}