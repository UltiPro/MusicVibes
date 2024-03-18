#pragma warning disable CS8618, CS8602

using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SWF = System.Windows.Forms;
using System.Security.AccessControl;
using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace MusicVibes.Pages;

public partial class PlaylistsPage : Page
{
    private const string playlistsFilePath = "playlistsPaths";
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
            using (FileStream playlistsFile = File.Create(playlistsFilePath))
            {
                playlistsFile.Close();
                new FileInfo(playlistsFilePath).SetAccessControl(
                    new FileSecurity(playlistsFilePath, AccessControlSections.Access));
            }
        }

        try
        {
            File.WriteAllLines(playlistsFilePath, playlistsPaths);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while saving paths to playlists: {ex.Message}");
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
        using (SWF.FolderBrowserDialog folderBrowserDialog = new SWF.FolderBrowserDialog())
        {
            if (folderBrowserDialog.ShowDialog() == SWF.DialogResult.OK)
            {
                playlistsPaths.Add(folderBrowserDialog.SelectedPath);
                SavePlaylists();
            }
        }
    }

    private void DeletePlaylist(object sender, RoutedEventArgs e)
    {
        int index = playlistsPaths.IndexOf(((sender as Button).Parent as Grid).Children.OfType<TextBlock>().First().Text);

        if (index == -1) return;

        playlistsPaths.RemoveAt(index);

        SavePlaylists();
    }

    private void OpenPlaylist(object sender, RoutedEventArgs e) => onPlaylistChange(sender, e, ((sender as Button).Parent as Grid).Children.OfType<TextBlock>().First().Text);
}