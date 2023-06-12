using WMPLib;

namespace MusicVibes.Models
{
    class AudioPlayer
    {
        private WindowsMediaPlayer WMP;
        public AudioPlayer()
        {
            WMP = new WindowsMediaPlayer();
            WMP.settings.volume = 50;
        }
        public void Load(string path)
        {
            WMP.controls.stop();
            WMP.URL = path;
            WMP.controls.play();
        }
        public void Pause() => WMP.controls.pause();
        public void Play() => WMP.controls.play();
        public void ChangeVolume(short value) => WMP.settings.volume = value; // to
        public void SkipTrack(int count) => WMP.controls.currentPosition += count;
        public bool IsJustStarted() => WMP.controls.currentPosition < 2.0d ? true : false;
    }
}
