using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Microsoft.Win32;
using FBD = System.Windows.Forms;

namespace MusicVibes.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy PlaylistsPage.xaml
    /// </summary>
    public partial class PlaylistsPage : Page
    {
        private const string PlaylistsFilePath = "playlistsPath.txt";
        private List<string> playlistPaths;

        public PlaylistsPage()
        {
            InitializeComponent();
            LoadPlaylists();
        }
        private void AddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFolderPath = folderBrowserDialog.SelectedPath;
                SavePlaylistPath(selectedFolderPath);
                LoadPlaylists();
            }
        }

        private void SavePlaylistPath(string playlistPath)
        {
            try
            {
                // Tworzenie pliku playlistsPath.txt, jeśli nie istnieje
                if (!File.Exists(PlaylistsFilePath))
                {
                    File.Create(PlaylistsFilePath).Close();
                }
                
                List<string> existingPaths = new List<string>(File.ReadAllLines(PlaylistsFilePath));

                // Sprawdzanie, czy ścieżka jest już zapisana w pliku
                if (existingPaths.Contains(playlistPath))
                {
                    MessageBox.Show("Ta ścieżka już istnieje w pliku.");
                    return;
                }
                // Zapisywanie ścieżki folderu do pliku playlistsPath.txt
                existingPaths.Add(playlistPath);
                File.WriteAllLines(PlaylistsFilePath, existingPaths);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisywania ścieżki playlisty: {ex.Message}");
            }
        }

        private void LoadPlaylists()
        {
            try
            {
                PlaylistsStackPanel.Children.Clear();

                if (File.Exists(PlaylistsFilePath))
                {
                    string[] playlistPaths = File.ReadAllLines(PlaylistsFilePath);

                    foreach (string playlistPath in playlistPaths)
                    {
                        StackPanel playlistPanel = new StackPanel();
                        playlistPanel.Orientation = Orientation.Horizontal;

                        Button playlistButton = new Button();
                        playlistButton.Content = Path.GetFileName(playlistPath).ToUpper();
                        playlistButton.Tag = playlistPath;
                        playlistButton.Click += PlaylistButton_Click;

                        Button deleteButton = new Button();
                        deleteButton.Content = "Usuń";
                        deleteButton.Tag = playlistPath;
                        deleteButton.Click += DeleteButton_Click;

                        playlistPanel.Children.Add(playlistButton);
                        playlistPanel.Children.Add(deleteButton);

                        PlaylistsStackPanel.Children.Add(playlistPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas wczytywania playlist: {ex.Message}");
            }
        }
        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Button playlistButton = (Button)sender;
            string playlistPath = playlistButton.Content.ToString();
            // Wykonaj odpowiednie działania na podstawie wybranej playlisty
            MessageBox.Show($"Wybrano playlistę: {playlistPath}");
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            StackPanel playlistPanel = (StackPanel)deleteButton.Parent;
            Button playlistButton = (Button)playlistPanel.Children[0];
            string playlistPath = playlistButton.Tag.ToString();

            try
            {
                if (File.Exists(PlaylistsFilePath))
                {
                    List<string> existingPaths = new List<string>(File.ReadAllLines(PlaylistsFilePath));

                    if (existingPaths.Contains(playlistPath))
                    {
                        existingPaths.Remove(playlistPath);
                        File.WriteAllLines(PlaylistsFilePath, existingPaths);
                        PlaylistsStackPanel.Children.Remove(playlistPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania playlisty: {ex.Message}");
            }
        }
    }
}
