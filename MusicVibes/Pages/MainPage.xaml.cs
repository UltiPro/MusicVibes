﻿#pragma warning disable CS8602

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FBD = System.Windows.Forms;
using WMPLib;
using MusicVibes.Models;
using MusicVibes.Pages.Infrastructure;

namespace MusicVibes.Pages;

public partial class MainPage : Page
{
    private AudioPlayer audioPlayer;
    private BitmapImage stopImage = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/Pause.png", UriKind.RelativeOrAbsolute));
    private BitmapImage startImage = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/Play.png", UriKind.RelativeOrAbsolute));
    private BitmapImage muteVolume = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/VolumeMute.png", UriKind.RelativeOrAbsolute));
    private BitmapImage unmuteVolume = new BitmapImage(new Uri("pack://application:,,,/Images/Controls/Volume.png", UriKind.RelativeOrAbsolute));
    private Thread theardRefresher;
    private List<MusicFile> deletedMusicFiles;
    private string[] extensions = new string[] { "mp3", "wav" };
    private bool isPlaying, isMute, isDragging;
    public ObservableCollection<MusicFile> musicFiles { get; } = new ObservableCollection<MusicFile>();
    public string? currentPath;
    public int currentVolume { get => audioPlayer.GetVolume(); }
    public int currentId;
    public bool stopApp;

    public MainPage(int volume, string path)
    {
        audioPlayer = new AudioPlayer(this, volume);
        LoadFiles(path);
        theardRefresher = new Thread(() => UpdateTrackDuration());
        deletedMusicFiles = new List<MusicFile>();
        isPlaying = false;
        isMute = false;
        isDragging = false;
        currentId = -1;
        stopApp = false;
        InitializeComponent();
        VolumeSlider.Value = volume;
        MusicList.onMusicChange += ChangeMusic;
        MusicList.onFocusChange += ChangeFocus;
        theardRefresher.Start();
    }

    public bool LoadFilesFromDialog()
    {
        using (FBD.FolderBrowserDialog folderBrowserDialog = new FBD.FolderBrowserDialog())
        {
            try
            {
                folderBrowserDialog.ShowDialog();
                return LoadFiles(folderBrowserDialog.SelectedPath);
            }
            catch
            {
                return false;
            }
        }
    }

    public bool LoadFilesFromPlaylists(string path) => LoadFiles(path);

    private bool LoadFiles(string path)
    {
        try
        {
            if (!Directory.Exists(path)) return false;
            currentPath = path;
            musicFiles.Clear();
            WindowsMediaPlayerClass WMPC = new WindowsMediaPlayerClass();
            IWMPMedia iWMPMedia;
            int i = 1;
            foreach (FileInfo musicFile in new DirectoryInfo(path).GetFiles("*.*").Where(file => extensions.Contains(Path.GetExtension(file.FullName).TrimStart('.').ToLowerInvariant())))
            {
                iWMPMedia = WMPC.newMedia(musicFile.FullName);
                musicFiles.Add(new MusicFile(i, musicFile.FullName, Path.GetFileNameWithoutExtension(musicFile.Name), iWMPMedia.duration, iWMPMedia.durationString));
                i++;
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void UpdateTrackDuration()
    {
        while (!stopApp)
        {
            Dispatcher.Invoke(() =>
            {
                if (isPlaying || DurationSlider.Value != audioPlayer.CurrentTrackDuration)
                {
                    CurrentDurationInfo.Text = audioPlayer.CurrentTrackDurationString != string.Empty ? audioPlayer.CurrentTrackDurationString : "00:00";
                    DurationSlider.Value = audioPlayer.CurrentTrackDuration;
                }
            });
            Thread.Sleep(100);
        }
    }

    public void ChangeMusic(object sender, RoutedEventArgs e, int id)
    {
        if (musicFiles.Count == 0) return;
        isPlaying = true;
        currentId = id;
        audioPlayer.Load(musicFiles[currentId].FilePath);
        NowPlaying.Text = musicFiles[currentId].FileName;
        StartStopImage.Source = stopImage;
        DurationSlider.Maximum = musicFiles[currentId].FileDuration;
        DurationSlider.Value = 0.0d;
        DurationInfo.Text = musicFiles[currentId].FileDurationString;
        MusicList.SetItemFocus(currentId);
    }

    private void SearchTextChanged(object sender, TextChangedEventArgs e)
    {
        foreach (MusicFile musicFile in deletedMusicFiles) musicFiles.Insert(musicFile.FileId - 1, musicFile);
        deletedMusicFiles = musicFiles.ToList().Where(e => !e.FileName.ToLower().Contains((sender as TextBox).Text.ToLower())).ToList();
        foreach (MusicFile musicFile in deletedMusicFiles) musicFiles.RemoveAt(musicFiles.IndexOf(musicFile));
    }

    public void SkipStart(object sender, RoutedEventArgs e) => ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : !audioPlayer.IsJustStarted() ? currentId : --currentId < 0 ? (musicFiles.Count - 1) : currentId);
    public void Skip10Start(object sender, RoutedEventArgs e) => audioPlayer.SkipTrack(currentId == -1 ? 0 : -10);

    public void StartPauseMusic(object sender, RoutedEventArgs e)
    {
        if (currentId == -1) ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : 0);
        else
        {
            isPlaying = !isPlaying;
            if (isPlaying)
            {
                audioPlayer.Play();
                StartStopImage.Source = stopImage;
            }
            else
            {
                audioPlayer.Pause();
                StartStopImage.Source = startImage;
            }
        }
    }

    public void Skip10End(object sender, RoutedEventArgs e) => audioPlayer.SkipTrack(currentId == -1 ? 0 : 10);
    public void SkipEnd(object sender, RoutedEventArgs e) => ChangeMusic(sender, e, musicFiles.Count == 0 ? -1 : ++currentId >= musicFiles.Count ? 0 : currentId);
    private void DurationChangedStart(object sender, DragStartedEventArgs e)
    {
        isDragging = true;
        if (isPlaying) audioPlayer.Pause();
    }

    private void DurationChangedEnd(object sender, DragCompletedEventArgs e)
    {
        isDragging = false;
        if (isPlaying) audioPlayer.Play();
    }

    private void DurationChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (isDragging)
        {
            audioPlayer.CurrentTrackDuration = DurationSlider.Value;
            CurrentDurationInfo.Text = audioPlayer.CurrentTrackDurationString != string.Empty ? audioPlayer.CurrentTrackDurationString : "00:00";
        }
    }

    private void MuteUnmuteVolume(object sender, RoutedEventArgs e)
    {
        isMute = !isMute;
        audioPlayer.Mute(isMute);
        if (isMute) VolumeImage.Source = muteVolume;
        else VolumeImage.Source = unmuteVolume;
    }

    public void MuteVolume(object sender, RoutedEventArgs e)
    {
        if (!isMute) MuteUnmuteVolume(sender, e);
    }

    public void UnmuteVolume(object sender, RoutedEventArgs e)
    {
        if (isMute) MuteUnmuteVolume(sender, e);
    }

    private void VolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => audioPlayer.ChangeVolume(Convert.ToInt32((sender as Slider).Value));
    private void ControlMouseEnter(object sender, MouseEventArgs e) => (sender as Button).BeginAnimation(Button.OpacityProperty, new DoubleAnimation(1.0d, 0.65d, TimeSpan.FromSeconds(0.2d)));
    private void ControlMouseLeave(object sender, MouseEventArgs e) => (sender as Button).BeginAnimation(Button.OpacityProperty, new DoubleAnimation(0.65d, 1d, TimeSpan.FromSeconds(0.2d)));

    private void StartStopButton_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
            Focus();
        }
    }

    public void ChangeFocus(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Space) StartStopButton.Focus();
    }
}