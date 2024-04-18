namespace SasaLibDNet8
{
    public static class Sound
    {
        public static void Play(string SoundFile= @"C:\Windows\Media\Windows Logon.wav")
        {
            System.Media.SoundPlayer player = null;
            player = new System.Media.SoundPlayer(SoundFile);
            player.Play();
        }
    }
}
