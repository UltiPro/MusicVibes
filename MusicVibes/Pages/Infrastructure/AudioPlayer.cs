using WMPLib;

namespace MusicVibes.Pages.Infrastructure;

class AudioPlayer
{
    private WindowsMediaPlayer WMP;
    private MainPage mainPage;
    public AudioPlayer(MainPage mainPage)
    {
        WMP = new WindowsMediaPlayer();
        WMP.settings.volume = 50;
        WMP.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(TrackEnded);
        this.mainPage = mainPage;
    }
    public void Load(string path)
    {
        WMP.controls.stop();
        WMP.URL = path;
        Play();
    }
    public void Pause() => WMP.controls.pause();
    public void Play() => WMP.controls.play();
    public void SkipTrack(int count) => WMP.controls.currentPosition += count;
    public bool IsJustStarted() => WMP.controls.currentPosition < 2.0d ? true : false;
    public void ChangeVolume(int value) => WMP.settings.volume = value;
    public void Mute(bool value) => WMP.settings.mute = value;
    private void TrackEnded(int state)
    {
        if (state == (int)WMPPlayState.wmppsMediaEnded)
        {
            int temp_volume = WMP.settings.volume;
            WMP = new WindowsMediaPlayer();
            WMP.settings.volume = temp_volume;
            WMP.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(TrackEnded);
            mainPage.ChangeMusic(new object(), new System.Windows.RoutedEventArgs(), mainPage.musicFiles.Count == 0 ? -1 : ++mainPage.currentId >= mainPage.musicFiles.Count ? 0 : mainPage.currentId);
        }
    }
    public string CurrentTrackDuration { get { return WMP.controls.currentPositionString; } }
}