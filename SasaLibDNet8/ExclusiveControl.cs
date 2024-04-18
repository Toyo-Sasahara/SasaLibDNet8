namespace SasaLibDNet8
{
    /// <summary>
    /// 排他的制御コントロールクラス
    /// </summary>
    public class SasaLibDNet8
    {
        static bool busy { get; set; }
        static string Msg { get; set; }


        public void Locked(string Message)
        {
            busy = true;
            Msg = Message;
        }

        public void Unlocked()
        {
            busy = false;
        }

        public bool IsBusy()
        {
            return busy;
        }
    }
}
