#pragma warning disable CS8618, CS8600, CS8602

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SWC = System.Windows.Forms;
using System.Security.AccessControl;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace MusicVibes.Pages;

public partial class PlaylistsPage : Page
{
    private const string playlistsFilePath = "playlistsPaths.txt";
    public ObservableCollection<string> playlistsPaths { get; } = new ObservableCollection<string>();
    public delegate void OnPlaylistChange(object sender, RoutedEventArgs e, string path);
    public event OnPlaylistChange onPlaylistChange;

    public PlaylistsPage()
    {
        InitializeComponent();
        LoadPlaylists();
    }

    private void SavePlaylists()
    {
        if (!File.Exists(playlistsFilePath))
        {
            using (FileStream fileStream = File.Create(playlistsFilePath))
            {
                fileStream.Close();
                FileSecurity fileSecurity = new FileSecurity(playlistsFilePath, AccessControlSections.Access);
                FileInfo fileInfo = new FileInfo(playlistsFilePath);
                fileInfo.SetAccessControl(fileSecurity);
            }
        }

        try
        {
            File.WriteAllLines(playlistsFilePath, playlistsPaths);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd podczas zapisywania ścieżki playlisty: {ex.Message}");
        }
    }

    private void LoadPlaylists()
    {
        if (File.Exists(playlistsFilePath))
            foreach (string playlistPath in File.ReadAllLines(playlistsFilePath))
                playlistsPaths.Add(playlistPath);
    }

    private void AddPlaylist(object sender, RoutedEventArgs e)
    {
        using (SWC.FolderBrowserDialog folderBrowserDialog = new SWC.FolderBrowserDialog())
        {
            if (folderBrowserDialog.ShowDialog() == SWC.DialogResult.OK)
            {
                playlistsPaths.Add(folderBrowserDialog.SelectedPath);
                SavePlaylists();
            }
        }
    }

    private void DeletePlaylist(object sender, RoutedEventArgs e)
    {
        int index = playlistsPaths.IndexOf(((sender as Button).Parent as Grid).Children.OfType<TextBlock>().FirstOrDefault().Text);

        if(index == -1) return;

        playlistsPaths.RemoveAt(index);

        SavePlaylists();
    }

    private void OpenPlaylist(object sender, RoutedEventArgs e) => onPlaylistChange(sender, e, ((sender as Button).Parent as Grid).Children.OfType<TextBlock>().First().Text);
}