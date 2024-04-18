using System;
using System.IO.Pipes;
using System.Text;

namespace SasaLibDNet8.PIPE
{
    class Program
    {

        //名前付きパイプのクライアントです.
        static NamedPipeClientStream client;

        //受信したデータを保管する配列です.
        static byte[] receiveField;

        /// <summary>
        /// コールバックと一緒に指定する任意のStateのためのサンプル構造体を定義します.
        /// </summary>
        struct SampleState
        {
            public int count;
        }

        /// <summary>
        /// Default EntryPoint.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //クライアントを初期化します.
            //サーバ名,パイプの名前,パイプの方向,(非)同期接続
            //サーバ名はローカルの時"."を指定します.
            client = new NamedPipeClientStream
                (".", "testpipe", PipeDirection.InOut, PipeOptions.Asynchronous);

            //接続を試みます.
            //接続に関しては、非同期の枠組みが用意されていません.
            client.Connect(3000);

            //接続したら非同期の受信待機を開始します.
            if (client.IsConnected)
            {
                BeginRead(new SampleState());
            }

            //コンソールを閉じないようにしておく.
            Console.Read();
        }

        /// <summary>
        /// 非同期の受信を開始するメソッドです.
        /// </summary>
        static void BeginRead(SampleState userObject)
        {
            //受信データを保存する領域を初期化しておきます.
            receiveField = new byte[100];

            //受信待機を開始します.
            //受信データの保存先, 受け取りデータの保存開始位置, 最大保存量,
            //受信時のコールバック, 任意の情報, を指定します
            client.BeginRead
                (receiveField, 0, receiveField.Length,
                ReceiveCallback, userObject);

            Console.WriteLine("Now waiting.");
        }

        /// <summary>
        /// BeginReadを実行する際に指定するコールバックです.
        /// サーバからメッセージを受信した際に呼び出されます.
        /// </summary>
        /// <param name="result"></param>
        static void ReceiveCallback(IAsyncResult result)
        {

            //受信待機を終了します.
            //1回のBeginReadとセットにして、1回実行される必要があります.
            client.EndRead(result);

            //接続されていなかったらリソースを開放して終了します.
            if (client.IsConnected == false)
            {
                Console.WriteLine("DisConnect.");
                client.Dispose();
                return;
            }

            //任意に設定した情報を取得します
            //ここでは受信回数を数えます.
            SampleState userObject = ((SampleState)result.AsyncState);
            userObject.count++;

            //受信内容と受診回数を出力します.
            Console.WriteLine
                ("Receive [" + userObject.count + "] "
                    + Encoding.Unicode.GetString(receiveField));

            //再度待機を開始して無限に受信します.
            BeginRead(userObject);
        }

    }
}
