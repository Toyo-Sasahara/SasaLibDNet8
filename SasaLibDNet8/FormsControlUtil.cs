using System.Drawing;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SasaLib
{
    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class FormsControlUtil
    {
        static Color bk_Blink;
        static bool flagBlink = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cl"></param>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        public static async void Blink(object cl, int count = 3, int delay = 200)
        {

            if (flagBlink)
            {
                flagBlink = false;
                // 元のBackColorを退避
                bk_Blink = ((Control)cl).BackColor;

                int i = count;
                while (true)
                {

                    await Task.Delay(delay);
                    ((Control)cl).BackColor = ((Control)cl).BackColor == Color.Red ? Color.Green : Color.Red;

                    if (i > 0)
                        i--;
                    else
                        break;
                }
                ((Control)cl).BackColor = bk_Blink;
            }
            else
            {
                flagBlink = true;
            }
            flagBlink = true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cl"></param>
        /// <param name="defaultcolor"></param>
        /// <param name="count"></param>
        /// <param name="delay"></param>
        public static async void Blink2(object cl, Color defaultcolor,  int count = 3, int delay = 200)
        {

            if (flagBlink)
            {
                flagBlink = false;

                int i = count;
                while (true)
                {

                    await Task.Delay(delay);
                    ((Control)cl).BackColor = ((Control)cl).BackColor == Color.Red ? Color.Green : Color.Red;

                    if (i > 0)
                        i--;
                    else
                        break;
                }
                ((Control)cl).BackColor = defaultcolor;
            }
            else
            {
                flagBlink = true;
            }
            flagBlink = true;

        }

        /// <summary>
        /// バーが徐々に伸びるアニメーションを無効にして、
        /// ProgressBarのValueに値を設定する。
        /// </summary>
        /// <param name="pb">値を設定するProgressBar</param>
        /// <param name="val">設定する値</param>
        public static void SetProgressBarValue(ProgressBar pb, int val)
        {
            if (pb.Value < val)
            {
                //値を増やす時
                if (val < pb.Maximum)
                {
                    //目的の値より一つ大きくしてから、目的の値にする
                    pb.Value = val + 1;
                    pb.Value = val;
                }
                else
                {
                    //最大値にする時
                    //最大値を1つ増やしてから、元に戻す
                    pb.Maximum++;
                    pb.Value = val + 1;
                    pb.Value = val;
                    pb.Maximum--;
                }
            }
            else
            {
                //値を減らす時は、そのまま
                pb.Value = val;
            }
        }

        /// <summary>
        /// マーキースタイルプログレスバーを表示・非表示
        /// </summary>
        /// <param name="pb"></param>
        /// <param name="Value">0で停止、100まで</param>
        public static void MarqueeProgressBar(ProgressBar pb, int Value = 50)
        {
            //ProgressBar1をマーキースタイルにする
            pb.Style = ProgressBarStyle.Marquee;
            //ブロックの移動速度をデフォルトの倍にする
            pb.MarqueeAnimationSpeed = Value;
        }
    }
}
