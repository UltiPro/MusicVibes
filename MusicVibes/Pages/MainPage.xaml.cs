#pragma warning disable CS8602

using MusicVibes.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FBD = System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using WMPLib;

namespace MusicVibes.Pages
{
    public partial class MainPage : Page
    {
        private AudioPlayer audioPlayer;
        private BitmapImage stopImage = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/Pause.png", UriKind.RelativeOrAbsolute));
        private BitmapImage startImage = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/Play.png", UriKind.RelativeOrAbsolute));
        private int currentId;
        private bool isPlaying;
        public ObservableCollection<MusicFile> musicFiles { get; set; } = new ObservableCollection<MusicFile>();
        private string[] extensions = new string[] { "mp3", "wav" };
        public MainPage()
        {
            InitializeComponent();
            audioPlayer = new AudioPlayer();
            currentId = -1;
            isPlaying = false;
            MusicList.onMusicChange += ChangeMusic;
        }
        public bool LoadFiles()
        {
            using (FBD.FolderBrowserDialog folderBrowserDialog = new FBD.FolderBrowserDialog())
            {
                try
                {
                    folderBrowserDialog.ShowDialog();
                    var tempMusicFiles = new DirectoryInfo(folderBrowserDialog.SelectedPath).GetFiles("*.*").Where(file => extensions.Contains(Path.GetExtension(file.FullName).TrimStart('.').ToLowerInvariant()));
                    musicFiles.Clear();
                    //tutaj
                    WindowsMediaPlayerClass wmp_temp = new WindowsMediaPlayerClass();
                    int i = 0;
                    foreach (var musicFile in tempMusicFiles)
                    {
                        musicFiles.Add(new MusicFile(i, musicFile.FullName, musicFile.Name.Remove(musicFile.Name.Length - 4), musicFile.Extension, wmp_temp.newMedia(musicFile.FullName).durationString));
                        i++;
                    }
                    //tutaj
                    return true;
                }
                catch (Exception e)
                {
                    // logger ?
                    return false;
                }
            }
        }
        private void ChangeMusic(object sender, RoutedEventArgs e, int id)
        {
            if (musicFiles.Count == 0) return;
            audioPlayer.Load(musicFiles[id].FilePath);
            StartStopImage.Source = stopImage;
            NowPlaying.Text = musicFiles[id].FileName;
            currentId = id;
            isPlaying = true;
        }
        private void SkipStart(object sender, RoutedEventArgs e) => ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : !audioPlayer.IsJustStarted() ? currentId : --currentId < 0 ? (musicFiles.Count - 1) : currentId );
        private void Skip10Start(object sender, RoutedEventArgs e) => audioPlayer.SkipTrack(currentId == -1 ? 0 : -10);
        private void StartPauseMusic(object sender, RoutedEventArgs e)
        {
            if ( currentId == -1 )
            {
                ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : 0);
                return;
            }
            if (isPlaying)
            {
                audioPlayer.Pause();
                StartStopImage.Source = startImage;
            }
            else
            {
                audioPlayer.Play();
                StartStopImage.Source = stopImage;
            }
            isPlaying = !isPlaying;
        }
        private void Skip10End(object sender, RoutedEventArgs e) => audioPlayer.SkipTrack(currentId == -1 ? 0 : 10);
        private void SkipEnd(object sender, RoutedEventArgs e) => ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : ++currentId >= musicFiles.Count ? 0 : currentId);

        // inna logika

        private void VolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //end
        }


        

        private void ControlMouseEnter(object sender, MouseEventArgs e) => (sender as Button).BeginAnimation(Button.OpacityProperty, new DoubleAnimation(1.0d, 0.65d, TimeSpan.FromSeconds(0.2d))); 
        private void ControlMouseLeave(object sender, MouseEventArgs e) => (sender as Button).BeginAnimation(Button.OpacityProperty, new DoubleAnimation(0.75d, 1d, TimeSpan.FromSeconds(0.2d)));

        
    }
}
