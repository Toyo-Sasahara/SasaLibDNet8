// SasaLIb イベントループ処理

using System.Windows.Threading;

namespace SasaLib
{
    /// <summary>
    /// MainWindowの部分クラス 
    /// 重要参照 PresentationCore, PresentationFramework System.Windows
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(obj =>
            {
                ((DispatcherFrame)obj).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }
    }

    /// <summary>
    /// イベントループを回す
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public static class DoEvents
    {
        //
        public static void Run()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(obj =>
            {
                ((DispatcherFrame)obj).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }
    }

}
