//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Json;
//using System.Text;
//using System.Text.Json;

//namespace SasaLib.PIPE
//{
//    /// <summary>
//    /// 【非推奨】オブジェクトとバイト配列の変換
//    /// </summary>
//    /// <typeparam name="TObject"></typeparam>
//    public class ObjectConverter<TObject>
//    {
//#pragma warning disable SYSLIB0011 // 型またはメンバーが旧型式です
//        private readonly IFormatter _iformatter;
//#pragma warning restore SYSLIB0011 // 型またはメンバーが旧型式です

//#pragma warning disable SYSLIB0011 // 型またはメンバーが旧型式です
//        /// <summary>
//        /// 【非推奨】
//        /// </summary>
//        /// <param name="formatter"></param>
//        public ObjectConverter(IFormatter? formatter = null)
//#pragma warning restore SYSLIB0011 // 型またはメンバーが旧型式です
//        {
//#pragma warning disable SYSLIB0011 // 型またはメンバーが旧型式です
//            _iformatter = formatter ?? new BinaryFormatter();
//#pragma warning restore SYSLIB0011 // 型またはメンバーが旧型式です
//        }

//        /// <summary>
//        /// オブジェクト=>バイト配列
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <returns></returns>
//        public byte[] ToByteArray(TObject obj)
//        {
//            using (var stream = new MemoryStream())
//            {
//                _iformatter.Serialize(stream, obj);
//                return stream.ToArray();
//            }
//        }

//        /// <summary>
//        /// バイト配列=>オブジェクト(例外を検知)
//        /// </summary>
//        /// <param name="bytes"></param>
//        /// <param name="exception"></param>
//        /// <returns></returns>
//        public TObject FromByteArray(byte[] bytes, out Exception exception)
//        {

//            exception = null;

//            using (var stream = new MemoryStream(bytes))
//            {
//                try
//                {
//                    return (TObject)_iformatter.Deserialize(stream);
//                }
//                catch (Exception ex)
//                {
//                    exception = ex;
//                    return default;
//                }
//            }
//        }

//        //--//

//        /// <summary>
//        /// オブジェクト=>バイト配列 (JSON経由)
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="obj"></param>
//        /// <param name="mssize"></param>
//        /// <param name="exception"></param>
//        /// <returns></returns>
//        public byte[] ToByteArrayViaJSON(TObject obj, out long mssize, out Exception exception)
//        {
//            exception = null;
//            mssize = 0;

//            if (obj == null)
//                return null;

//            try
//            {
//                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(TObject));

//                using (MemoryStream memoryStream = new MemoryStream())
//                {
//                    jsonSerializer.WriteObject(memoryStream, obj);
//                    mssize = memoryStream.Length;

//                    byte[] output = memoryStream.ToArray();
//                    return output;
//                }
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//                return null;
//            }
//        }

//        /// <summary>
//        /// バイト配列=>オブジェクト (JSON経由)
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="byteArray"></param>
//        /// <param name="exception"></param>
//        /// <returns></returns>
//        // バイト配列をオブジェクトにデシリアライズするメソッド
//        public TObject FromByteArrayViaJSON(byte[] data, out Exception exception)
//        {
//            exception = null;

//            if (data == null || data.Length == 0)
//                return default;

//            try
//            {
//                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(TObject));

//                using (MemoryStream memoryStream = new MemoryStream(data))
//                {
//                    TObject obj = (TObject)jsonSerializer.ReadObject(memoryStream);
//                    return obj;
//                }
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//                return default;
//            }
//        }

//        //--//

//        // オブジェクトをバイト配列にシリアライズするメソッド
//        public byte[] ToByteArrayViaJSON2(TObject obj, out Exception exception)
//        {
//            exception = null;

//            if (obj == null)
//                return null;

//            try
//            {
//                // JsonSerializerを使ってオブジェクトをJSON形式のバイト配列にシリアライズする
//                string jsonString = JsonSerializer.Serialize(obj);
//                byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
//                return byteArray;
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//                return null;
//            }
//        }

//        /// <summary>
//        // バイト配列をオブジェクトにデシリアライズするメソッド
//        /// </summary>
//        /// <param name="data"></param>
//        /// <param name="exception"></param>
//        /// <returns></returns>
//        public TObject FromByteArrayViaJSON2(byte[] data, out Exception exception)
//        {
//            exception = null;

//            if (data == null || data.Length == 0)
//                return default;

//            try
//            {
//                // バイト配列をUTF-8文字列に変換
//                string jsonString = Encoding.UTF8.GetString(data);

//                // JsonSerializerを使ってJSON文字列をオブジェクトにデシリアライズする
//                TObject obj = JsonSerializer.Deserialize<TObject>(jsonString);
//                return obj;
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//                return default;
//            }
//        }

//        //--//

//        /// <summary>
//        // Bitmapをバイト配列に変換するメソッド
//        /// </summary>
//        /// <param name="bitmap"></param>
//        /// <param name="format"></param>
//        /// <returns></returns>
//        /// <exception cref="ArgumentNullException"></exception>
//        public byte[] ToByteArrayFromBitmap(Bitmap bitmap,ImageFormat format)
//        {
//            if (bitmap == null)
//                throw new ArgumentNullException(nameof(bitmap));

//            using (MemoryStream memoryStream = new MemoryStream())
//            {
//                // BitmapをJPEG形式でメモリストリームに保存
//                bitmap.Save(memoryStream, format);

//                // メモリストリームからバイト配列に変換して返す
//                return memoryStream.ToArray();
//            }
//        }

//        /// <summary>
//        // バイト配列をBitmapに変換するメソッド
//        /// </summary>
//        /// <param name="byteArray"></param>
//        /// <param name="exception"></param>
//        /// <returns></returns>
//        /// <exception cref="ArgumentException"></exception>
//        public Bitmap FromByteArrayToBitmap(byte[] byteArray, out Exception exception)
//        {
//            exception = null;

//            try
//            {
//                if (byteArray == null || byteArray.Length == 0)
//                    throw new ArgumentException("Byte array is null or empty.", nameof(byteArray));

//                using (MemoryStream memoryStream = new MemoryStream(byteArray))
//                {
//                    // メモリストリームからBitmapオブジェクトを作成して返す
//                    Bitmap bitmap = new Bitmap(memoryStream);
//                    return bitmap;
//                }

//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//                return null;
//            }
//        }

//        //--//

//        /// <summary>
//        /// オブジェクト=>バイト配列 (ダイレクト)
//        /// </summary>
//        /// <param name="obj"></param>
//        /// <param name="mssize"></param>
//        /// <param name="exception"></param>
//        /// <returns></returns>
//        /// <exception cref="ArgumentNullException"></exception>
//        public byte[] ObjectToByteArrayViaDirect(TObject obj, out long mssize, out Exception exception)
//        {
//            if (obj == null)
//                throw new ArgumentNullException(nameof(obj));

//            exception = null;

//            try
//            {
//                using (MemoryStream memoryStream = new MemoryStream())
//                {
//                    using (BinaryWriter writer = new BinaryWriter(memoryStream))
//                    {
//                        // ここでオブジェクトをバイナリ形式に書き込む
//                        _WriteObject(writer, obj);
//                    }

//                    mssize = memoryStream.Length;

//                    return memoryStream.ToArray();
//                }
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//                mssize = 0L;
//                return null;
//            }

//            // オブジェクトをバイナリ形式で書き込むヘルパーメソッド
//            void _WriteObject<T>(BinaryWriter writer, T obj)
//            {
//                // ここでオブジェクトの各プロパティをバイナリ形式で書き込む
//                // 例えば、プリミティブ型や配列などを順番に書き込む
//                if (typeof(T) == typeof(int))
//                {
//                    writer.Write((int)(object)obj);
//                }
//                else if (typeof(T) == typeof(string))
//                {
//                    writer.Write((string)(object)obj);
//                }
//                else if (typeof(T) == typeof(Bitmap))
//                {
//                    Bitmap bitmap = (Bitmap)(object)obj;
//                    // Bitmapの幅と高さを書き込む
//                    writer.Write(bitmap.Width);
//                    writer.Write(bitmap.Height);

//                    // Bitmapのピクセルデータを書き込む
//                    for (int y = 0; y < bitmap.Height; y++)
//                    {
//                        for (int x = 0; x < bitmap.Width; x++)
//                        {
//                            Color pixelColor = bitmap.GetPixel(x, y);
//                            writer.Write(pixelColor.ToArgb());
//                        }
//                    }

//                }
//                // 他の型についても同様に処理を追加する
//                else
//                {
//                    //throw new NotSupportedException($"Type '{typeof(T).FullName}' is not supported.");
//                }
//            }
//        }

//        //--//

//        // オブジェクトをバイト配列にシリアライズするメソッド
//        public byte[] ToByteArrayViaDirect2(object obj)
//        {
//            if (obj == null)
//                return null;

//            using (MemoryStream memoryStream = new MemoryStream())
//            using (BinaryWriter writer = new BinaryWriter(memoryStream))
//            {
//                // オブジェクトの型名を書き込む（デシリアライズ時に必要）
//                string typeName = obj.GetType().AssemblyQualifiedName;
//                writer.Write(typeName);

//                // オブジェクトをバイト配列にシリアライズする
//                _SerializeValue(obj, writer);

//                return memoryStream.ToArray();
//            }

//            // 値をシリアライズするメソッド
//            void _SerializeValue(object value, BinaryWriter writer)
//            {
//                if (value == null)
//                {
//                    writer.Write((byte)DataType.Null);
//                }
//                else if (value is int intValue)
//                {
//                    writer.Write((byte)DataType.Int32);
//                    writer.Write(intValue);
//                }
//                else if (value is string stringValue)
//                {
//                    writer.Write((byte)DataType.String);
//                    writer.Write(stringValue);
//                }
//                else if (value is Bitmap bitmap)
//                {
//                    // Bitmapの幅と高さを書き込む
//                    writer.Write(bitmap.Width);
//                    writer.Write(bitmap.Height);

//                    // Bitmapのピクセルデータを書き込む
//                    for (int y = 0; y < bitmap.Height; y++)
//                    {
//                        for (int x = 0; x < bitmap.Width; x++)
//                        {
//                            Color pixelColor = bitmap.GetPixel(x, y);
//                            writer.Write(pixelColor.ToArgb());
//                        }
//                    }

//                }
//                else
//                {
//                    // オブジェクト型の場合
//                    Type type = value.GetType();
//                    writer.Write((byte)DataType.Object);
//                    writer.Write(type.AssemblyQualifiedName);

//                    // オブジェクトのフィールド数を書き込む
//                    var fields = type.GetFields();
//                    writer.Write(fields.Length);

//                    // オブジェクトの各フィールドを書き込む
//                    foreach (var field in fields)
//                    {
//                        object fieldValue = field.GetValue(value);
//                        _SerializeValue(fieldValue, writer);
//                    }
//                }
//            }

//        }

//        // バイト配列をオブジェクトにデシリアライズするメソッド
//        public object FromByteArrayViaDirect2(byte[] bytes)
//        {
//            if (bytes == null || bytes.Length == 0)
//                return null;

//            using (MemoryStream memoryStream = new MemoryStream(bytes))
//            using (BinaryReader reader = new BinaryReader(memoryStream))
//            {
//                // 型名を読み取る
//                string typeName = reader.ReadString();

//                // 型を取得
//                Type objectType = Type.GetType(typeName);
//                if (objectType == null)
//                    throw new SerializationException($"Failed to find type '{typeName}' during deserialization.");

//                // オブジェクトをデシリアライズする
//                object obj = _DeserializeObjectGraph(objectType, reader);
//                return obj;
//            }

//            // オブジェクトグラフを再帰的にデシリアライズするメソッド
//            object _DeserializeObjectGraph(Type objectType, BinaryReader reader)
//            {
//                if (objectType == null)
//                    throw new ArgumentNullException(nameof(objectType));

//                // オブジェクトのインスタンスを生成
//                object obj = Activator.CreateInstance(objectType);

//                // オブジェクトのフィールド数を読み取る
//                int fieldCount = reader.ReadInt32();

//                // フィールドをデシリアライズしてセットする
//                for (int i = 0; i < fieldCount; i++)
//                {
//                    // フィールド名と値を読み取る
//                    string fieldName = reader.ReadString();
//                    object fieldValue = _DeserializeValue(reader);

//                    // フィールドに値をセットする
//                    var field = objectType.GetField(fieldName);
//                    if (field != null)
//                        field.SetValue(obj, fieldValue);
//                }

//                return obj;
//            }

//            // 値をデシリアライズするメソッド
//            object _DeserializeValue(BinaryReader reader)
//            {
//                byte dataType = reader.ReadByte();

//                switch ((DataType)dataType)
//                {
//                    case DataType.Null:
//                        return null;
//                    case DataType.Int32:
//                        return reader.ReadInt32();
//                    case DataType.String:
//                        return reader.ReadString();
//                    case DataType.Object:
//                        // 再帰的にオブジェクトをデシリアライズする
//                        string typeName = reader.ReadString();
//                        Type objectType = Type.GetType(typeName);
//                        if (objectType == null)
//                            throw new SerializationException($"Failed to find type '{typeName}' during deserialization.");

//                        return _DeserializeObjectGraph(objectType, reader);
//                    default:
//                        throw new SerializationException($"Unknown data type: {dataType}");
//                }
//            }

//        }

//        //--//

//        // データタイプを示す列挙型
//        private enum DataType : byte
//        {
//            Null = 0,
//            Int32 = 1,
//            String = 2,
//            Object = 3
//            // 他のデータ型が必要な場合は追加することができます
//        }

//    }
//}
