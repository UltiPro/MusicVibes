using WMPLib;

namespace MusicVibes.Models
{
    class AudioPlayer
    {
        private WindowsMediaPlayer WMP;
        public AudioPlayer() => WMP = new WindowsMediaPlayer();
        public void Start(string path)
        {
            Stop();
            WMP.settings.volume = 10;
            WMP.URL = path;
            WMP.controls.play();
        }
        public void Pause() => WMP.controls.pause();
        public void Play() => WMP.controls.play();
        public void Stop() => WMP.controls.stop();
        public void ChangeVolume(short value) => WMP.settings.volume = value;
    }
}
