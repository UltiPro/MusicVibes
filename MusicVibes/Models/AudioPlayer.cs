using WMPLib;

namespace MusicVibes.Models
{
    class AudioPlayer
    {
        private WindowsMediaPlayer WMP;
        public AudioPlayer()
        {
            WMP = new WindowsMediaPlayer();
        }
        public void Pause() => WMP.controls.pause();
        public void Play() => WMP.controls.play();
        public void Stop() => WMP.controls.stop();
        public void ChangeVolume(short value) => WMP.settings.volume = value;
    }
}
