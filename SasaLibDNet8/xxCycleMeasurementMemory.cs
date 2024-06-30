//using System;
//using System.Runtime.CompilerServices;
//using System.Timers;

//namespace SasaLib
//{
//    /// <summary>
//    /// 現在のメモリ使用状況を一定時間ごとに取得するクラス
//    /// </summary>
//    public class CycleMeasurementMemory : IDisposable
//    {
//        //
//        // Fields
//        // - - - - - - - - - - - - - - - - - - -

//        /// <summary>通知先のハンドラ</summary>
//        private Action<object, EventArgs> _elapsed;
//        /// <summary>計測用の定周期タイマー</summary>
//        private System.Timers.Timer timer;

//        /// <summary>
//        /// 一定時間ごとに呼ばれるメモリ使用量通知イベント
//        /// </summary>
//        public Action<object, EventArgs> Elaplsed
//        {
//            set
//            {
//                ArgumentUtility.NullCheck(value);
//                this._elapsed = value;
//            }
//            get { return this._elapsed; }
//        }

//        //
//        // Constructors
//        // - - - - - - - - - - - - - - - - - - -

//        /// <summary>
//        /// 既定の初期値でオブジェクトを初期化します。
//        /// </summary>
//        public CycleMeasurementMemory()
//        {
//            // デフォルトの出力先を設定する
//            _elapsed = (object o, EventArgs e) => {
//                Console.WriteLine(Environment.WorkingSet + "byte");
//            };
//        }

//        //
//        // Public Methods
//        // - - - - - - - - - - - - - - - - - - -

//        /// <summary>
//        /// 計測間隔を指定してメモリの測定を開始します。
//        /// 10ミリ秒以下は指定できません。
//        /// </summary>
//        public void Start(TimeSpan interval)
//        {
//            if (interval < TimeSpan.FromMilliseconds(10))
//            {
//                throw new ArgumentOutOfRangeException(
//                    "interval", "Can not specify less than 10 milli second.");
//            }
//            if (this.timer != null)
//            {
//                return; // 測定中の再スタートは無視
//            }
//            this.timer = new System.Timers.Timer(interval.TotalMilliseconds);
//            this.timer.Elapsed += Timer_Elapsed;
//            this.timer.Start();
//        }

//        /// <summary>
//        /// 計測を中止します。
//        /// </summary>
//        public void Stop()
//        {
//            using (this.timer) { }
//            this.timer = null;
//        }

//        /// <summary>
//        /// IDisposableの実装 - オブジェクト使用している資源を解放します。
//        /// </summary>
//        public void Dispose()
//        {
//            using (this.timer) { }
//            this._elapsed = null;
//        }

//        /// <summary>
//        /// メモリ使用量を計測します。
//        /// </summary>
//        public static MemoryEventArgs Measurement()
//        {
//            return new MemoryEventArgs(DateTime.Now, Environment.WorkingSet);
//        }

//        //
//        // Private Methods
//        // - - - - - - - - - - - - - - - - - - -

//        /// <summary>
//        /// メモリを計測します。
//        /// </summary>
//        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
//        {
//            this.timer.Stop();
//            this._elapsed(this, Measurement());
//            this.timer.Start();
//        }
//    }

//    /// <summary>
//    /// メモリ通知イベントで使用するイベントオブジェクト
//    /// </summary>
//    public class MemoryEventArgs : EventArgs
//    {

//        //
//        // Properties
//        // - - - - - - - - - - - - - - - - - - -

//        /// <summary>
//        /// 計測時刻を取得します。
//        /// </summary>
//        public DateTime Time { get; private set; }

//        /// <summary>
//        /// 計測時のメモリ使用量を取得します。
//        /// </summary>
//        public long WorkingSet { get; private set; }

//        //
//        // Constructors
//        // - - - - - - - - - - - - - - - - - - -

//        /// <summary>
//        /// 計測時刻とメモリ使用量を指定してオブジェクトを初期化します。
//        /// </summary>
//        public MemoryEventArgs(DateTime time, long workingSet)
//        {
//            this.Time = time;
//            this.WorkingSet = workingSet;
//        }
//    }

//    /// <summary>
//    /// 汎用引数チェック機能を提供するクラス
//    /// </summary>
//    public static class ArgumentUtility
//    {

//        /// <summary>
//        /// オブジェクトがnullかどうか確認し、nullの場合例外を発生させます。
//        /// </summary>
//        public static void NullCheck(object o, [CallerMemberName]string calledBy = "")
//        {
//            if (o == null) { throw new ArgumentNullException(calledBy); }
//        }
//    }
//}

///*
//使い方
//このクラスを以下のように宣言してStartメソッドを呼び出すと1秒に1回コンソールへ計測結果が表示されるようになります。

//    var counter = new CycleMeasurementMemory();
//    counter.Start(TimeSpan.FromMilliseconds(1000)); // 1秒間隔で計測実行

//もし、コンソール以外の場所へ計測結果を取得したい場合、Elapsedへ好きなメソッドを指定するとStartのintervalごとに
//そのメソッドが呼び出されます。

//    var counter = new CycleMeasurementMemory();

//    counter.Elaplsed = (object o, MemoryEventArgs e) => {
//        Console.WriteLine(e.Time.ToString() + ", " + e.WorkingSet + "byte");
//    };

//使い終わったらStopかDisposeを呼び出してください。

//    counter.Stop();

//もしくは

//    using(this.counter) { }

//    コードの著作権は放棄します。
//もしよければコピペして使ってください。
//*/