//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Collections;

//namespace SasaLib.ArcSuitePreview
//{
//    // RingBuffer.cs


//    /// <summary>
//    /// 最大容量付きリングバッファーを表します。 https://takap-tech.com/entry/2018/01/02/044400
//    /// </summary>
//    public class RingBuffer<T> : IEnumerable<T>
//    {
//        // -x-x- Fields -x-x-

//        private readonly Queue<T> _queue; // データを実際に格納するオブジェクト

//        // -x-x- Properties -x-x-

//        /// <summary>
//        /// バッファーに格納されている要素の数を取得します。
//        /// </summary>
//        public int Count => this._queue.Count;

//        /// <summary>
//        /// このバッファーの最大容量を取得します。
//        /// </summary>
//        public int MaxCapacity { get; private set; }

//        // -x-x- operators -x-x-

//        /// <summary>
//        /// 指定した位置の要素を参照します。
//        /// </summary>
//        public T this[int index]
//        {
//            get
//            {
//                if (index < 0 || index > this.Count)
//                    throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)}={index}");
//                return _queue.ElementAt(index);
//            }
//        }

//        // -x-x- Constructors -x-x-

//        /// <summary>
//        /// リングバッファーの最大要素数をしていしてオブジェクトを初期化します。
//        /// </summary>
//        public RingBuffer(int maxCapacity)
//        {
//            this.MaxCapacity = maxCapacity;
//            _queue = new Queue<T>(maxCapacity);
//        }

//        // -x-x- Public Methods -x-x-

//        /// <summary>
//        /// 指定した要素をリングバッファーに追加します。
//        /// </summary>
//        public void Add(T item)
//        {
//            _queue.Enqueue(item);
//            if (_queue.Count > this.MaxCapacity)
//            {
//                T removed = this.Pop();

//                // デバッグ用の出力:
//                // Console.WriteLine($"キャパシティを超えているためバッファの先頭データ破棄しました。{removed}");
//            }
//        }

//        /// <summary>
//        /// 末尾の要素を取得しバッファーからデータを削除します。
//        /// </summary>
//        public T Pop() => _queue.Dequeue();

//        /// <summary>
//        /// バッファーの先頭の要素を取得します。データは削除されません。
//        /// </summary>
//        public T First() => _queue.Peek();

//        /// <summary>
//        /// 指定した要素が存在するかどうかを確認します。
//        /// </summary>
//        public bool Contains(T item) => _queue.Contains(item);

//        /// <summary>
//        /// このオブジェクトが管理中の全データを配列として取得します。
//        /// </summary>
//        public T[] ToArray() => _queue.ToArray();

//        /// <summary>
//        /// 現在のリングバッファー内の要素を列挙します。
//        /// （Linqによるより高度な操作をえるようにこのメソッドを定義しておく）
//        /// </summary>
//        public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();

//        // IEnumerator の明示的な実装
//        IEnumerator IEnumerable.GetEnumerator() => _queue.GetEnumerator();
//    }


//    /// <summary>
//    /// RingBuffer<T>テストクラス
//    /// </summary>
//    public static  class RingBufferTest
//    {
//        public static void Main(params string[] args)
//        {
//            // 文字列を5件格納するバッファーを作成
//            var buffer = new RingBuffer<string>(5);

//            // バッファーにデータを追加する
//            buffer.Add("Tako");
//            buffer.Add("Ika");
//            buffer.Add("Saba");
//            buffer.Add("Suzuki");
//            buffer.Add("Awabi");
//            buffer.Add("Surume"); // ここでTakoが消えてSurumeが追加

//            // 中括弧を使ったメンバー初期化子でも初期化できる
//            buffer = new RingBuffer<string>(5) { "Tako", "Ika", "Saba", "Suzuki", "Awabi", "Surume" };

//            // インデクサーでデータを取得（先頭）
//            string str_1 = buffer[0];
//            // >"Ika" ： 先頭がTakoがの次の値になっている

//            // 先頭の要素を取得
//            string str_2 = buffer.First();
//            // > "Ika"

//            // 中身の件数を取得する
//            int count_1 = buffer.Count;
//            // > 5 ： 最大件数

//            // 末尾のデータを取りだして削除
//            string str_3 = buffer.Pop();
//            // > "Ika"

//            // もう一度中身の件数を取得する
//            int count_2 = buffer.Count;
//            // > 4 ： Popしたので1件減っている

//            // 指定した要素が存在するかどうかを取得する
//            if (buffer.Contains("Suzuki"))
//            {
//                Console.WriteLine("OK");
//                // > OK ： 存在する
//            }

//            // 中身を全部列挙する
//            foreach (string item in buffer)
//            {
//                Console.WriteLine(item);
//                // > "Saba"
//                // > "Suzuki"
//                // > "Awabi"
//                // > "Surume"
//            }

//            // IEnumerableを継承しているのでLinq系の便利なメソッドが全て使える

//            // LinqのWhareメソッドでデータを抽出する
//            foreach (var item in buffer.Where(s => s.StartsWith("S")))
//            {
//                Console.WriteLine(item);
//                // > "Saba"
//                // > "Suzuki"
//                // > "Surume"
//            }

//            // LinqのMaxメソッドで最大文字数の要素の文字数を取得する
//            int len = buffer.Max(s => s.Length);
//            // > 6 ： Surumeの6文字

//            // 他にもLinqメソッドは色々使える
//        }
//    }
//}
