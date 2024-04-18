using System;
using System.IO.Pipes;
using System.Text;

namespace SasaLib.PIPE
{
    /// <summary>
    /// </summary>
    public class PiepServer
    {
        //名前付きパイプのサーバです
        NamedPipeServerStream server;


        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PiepServer()
        {
            //サーバを初期化します.
            //パイプの名前, パイプの方向, 
            //データの種類, パイプ共有の最大数, (非)同期接続.
            server = new NamedPipeServerStream("testpipe", PipeDirection.InOut,1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

            try
            {
                //非同期で接続待機します.
                //コールバックを設定します.
                server.BeginWaitForConnection(ConnectCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// BeginWaitForConnectionを実行する際に指定するコールバックです.
        /// クライアントが接続された際に呼び出されます.
        /// </summary>
        /// <param name="result"></param>
        public void ConnectCallback(IAsyncResult result)
        {
            //出力して通知する.
            Console.WriteLine("Client Connect.");

            //1回のBeginWaitForConnectionとセットにして、1回実行される必要があります.
            server.EndWaitForConnection(result);

            //End…を実行しない場合に接続は完了しません.
            //"IsConnected = false"であることが確認できます.
        }

        /// <summary>
        /// BeginWriteを実行する際に指定するコールバックです.
        /// 非同期送信操作が完了した際に呼び出されます.
        /// </summary>
        /// <param name="result"></param>
        public void WriteCallback(IAsyncResult result)
        {
            //1回のBeginWriteとセットにして、1回実行される必要があります.
            server.EndWrite(result);
            //出力して通知します.
            Console.WriteLine("Send.");
        }





        public void TestSendMessage()
        {
            //サーバにクライアントが接続されていることを確認します.
            if (server.IsConnected)
            {
                //送信するメッセージを作ります.
                byte[] message = Encoding.Unicode.GetBytes("Hello client.");
                //非同期でメッセージを送信します.
                //コールバックを設定します.
                server.BeginWrite(message, 0, message.Length, WriteCallback, null);



                
            }
        }

    }
}

/*
 非同期名前付きパイプの基礎 / サーバーSide

非同期操作を行う名前付きパイプについて解説します。このページの上のページが関連する項目の先頭です。サンプルプログラムも項目の先頭に公開されます。ここではサーバ側の実装について解説しています。

初期化と受信待機
先にサンプルコードを示して後に続いて解説することにします。

まずはサーバから作っていきます。名前付きパイプのサーバ、NamedPipeServerStreamを用意して初期化します。初期化時には、[パイプの名前、パイプの方向、最大共有数、パイプがあつかうデータの種類、同期であるか非同期であるか]を指定します(他にもありますが)。

パイプの方向とは、サーバがデータの受信を行うか、送信を行うか、あるいは両方を行うか、を指定します。ここではサーバからクラアイントへデータを送信するサンプルを作るため、PipeDirection.Outを指定しています。

パイプがあつかうデータの種類とは、送受信されるデータが1byte単位であつかわれるか、2byte単位であつかわれるのかで決定されます。このサンプルで指定しているPipeTransmissionMode.Byteは前者で、データを1byteずつあつかうための設定値です。一方でPipeTransmissionMode.Messageを指定すると、2byteずつのデータをあつかうようになります。これは全角文字、日本語の文字列などを送受信する場合に利用されます。

接続待機の開始

最後に、クライアントの接続を待ちます。これがもし同期処理であった場合、クライアントが接続されるまで、サーバを管理するスレッドが停止します。つまりサーバが停止します。

非同期に接続を開始する場合、BeginWaitForConnectionメソッドを利用します。引数にはコールバックと、任意のデータを与えることができますが、ここでは任意のデータはあつかわないものとします(クライアントsideで解説)。

コールバックに指定したメソッドは、クライアントが接続されたタイミングで実行されます。コールバックの内容については次の項目で解説します。


非同期な接続待機を実行する際に指定するコールバックメソッドです。重要な点は1つだけです。名前付きパイプの非同期通信では、非同期処理の操作と、その非同期処理の終了操作を、1対1で実行する必要があります。1回の非同期操作の実行に対し、非同期終了操作が行われない場合、予期しない不具合が発生したり、例外が発生したりします。

ここでは非同期な接続待機の終了のため、EndWaitForConnectionを実行します。これによって非同期接続待機を終了することができます。またこれを実行しなかった場合に、NamedPipeServerStreamの値、IsConnectedは常にfalseを開始します。コールバックの呼び出しの有無にかかわらずです。


サンプルプログラムでは、サーバはボタンを押すたびにデータを送信するようにしています。送信するデータの生成方法については、ここでは詳細の解説は避け、サンプルコードのみで示します。

非同期にデータを送信するには、BeginWriteメソッドを利用します。送信するデータ、送信するデータの開始位置、最大送信数、送信操作終了時に呼び出すコールバック、任意のデータを引数に指定します。

コールバックで重要となるのは、先と同じく、非同期操作の終了処理です。1回のBeginWriteの実行に対し、1回のEndWriteを実行する必要があります。

これでサーバ側の解説は終わりです。
 */
