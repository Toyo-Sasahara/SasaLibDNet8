namespace SasaLib
{
    /// <summary>
    /// 排他的制御コントロールクラス
    /// </summary>
    public class SasaLib
    {
        static bool busy { get; set; }
        static string Msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void Locked(string Message)
        {
            busy = true;
            Msg = Message;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unlocked()
        {
            busy = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsBusy()
        {
            return busy;
        }
    }
}
