using System.Runtime.Versioning;

namespace SasaLib
{
    /// <summary>
    /// 
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class Sound
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SoundFile"></param>
        public static void Play(string SoundFile= @"C:\Windows\Media\Windows Logon.wav")
        {
            System.Media.SoundPlayer player = null;
            player = new System.Media.SoundPlayer(SoundFile);
            player.Play();
        }
    }
}
