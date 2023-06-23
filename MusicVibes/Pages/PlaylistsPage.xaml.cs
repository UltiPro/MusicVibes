﻿using System;
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

        public delegate void OnPlaylistChange(object sender, RoutedEventArgs e, string path);
        public event OnPlaylistChange onPlaylistChange;
        ResourceDictionary resources = new ResourceDictionary();
        
        public PlaylistsPage()
        {
            InitializeComponent();
            resources.Source = new Uri("../Themes/PlaylistButtonTheme.xaml", UriKind.RelativeOrAbsolute);
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
                        DockPanel playlistPanel = CreatePlaylistPanel(playlistPath);
                        PlaylistsStackPanel.Children.Add(playlistPanel);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas wczytywania playlist: {ex.Message}");
            }
        }

        private DockPanel CreatePlaylistPanel(string playlistPath)
        {
            DockPanel playlistPanel = new DockPanel();

            Button playlistButton = CreatePlaylistButton(playlistPath);
            Button deleteButton = CreateDeleteButton(playlistPath);

            DockPanel.SetDock(playlistButton, Dock.Left);
            DockPanel.SetDock(deleteButton, Dock.Right);

            playlistPanel.Children.Add(playlistButton);
            playlistPanel.Children.Add(deleteButton);

            return playlistPanel;
        }

        private Button CreatePlaylistButton(string playlistPath)
        {
            Button playlistButton = new Button();
            playlistButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#231942"));
            playlistButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0b1cb"));
            playlistButton.FontSize = 20;
            playlistButton.Padding = new Thickness(20, 10, 20, 10);
            playlistButton.BorderBrush = Brushes.Transparent;
            playlistButton.HorizontalAlignment = HorizontalAlignment.Left;
            playlistButton.Style = (Style)resources["PlaylistButtonTheme"];

            StackPanel playlistButtonContent = new StackPanel();
            playlistButtonContent.Orientation = Orientation.Horizontal;

            Image playlistIcon = new Image();
            playlistIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Images/PlaylistImages/Folder.png", UriKind.RelativeOrAbsolute));
            playlistIcon.Width = 24;
            playlistIcon.Height = 24;
            playlistIcon.Margin = new Thickness(5, 2, 5, 2);

            TextBlock playlistName = new TextBlock();
            playlistName.Text = Path.GetFileName(playlistPath).ToUpper();

            playlistButtonContent.Children.Add(playlistIcon);
            playlistButtonContent.Children.Add(playlistName);

            playlistButton.Content = playlistButtonContent;
            playlistButton.Tag = playlistPath;
            playlistButton.Click += PlaylistButton_Click;

            return playlistButton;
        }

        private Button CreateDeleteButton(string playlistPath)
        {
            Button deleteButton = new Button();
            deleteButton.Tag = playlistPath;
            deleteButton.Click += DeleteButton_Click;
            deleteButton.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#231942"));
            deleteButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0b1cb"));
            deleteButton.Padding = new Thickness(20, 10, 20, 10);
            deleteButton.BorderBrush = Brushes.Transparent;
            deleteButton.HorizontalAlignment = HorizontalAlignment.Right;
            deleteButton.Style = (Style)resources["PlaylistButtonTheme"];

            Image deleteIcon = new Image();
            deleteIcon.Source = new BitmapImage(new Uri("pack://application:,,,/Images/PlaylistImages/Delete.png", UriKind.RelativeOrAbsolute));
            deleteIcon.Width = 24;
            deleteIcon.Height = 24;
            deleteIcon.Margin = new Thickness(1, 2, 5, 2);

            deleteButton.Content = deleteIcon;

            return deleteButton;
        }
        private void PlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            Button playlistButton = (Button)sender;
            string playlistPath = playlistButton.Tag.ToString();
            // Wykonaj odpowiednie działania na podstawie wybranej playlisty
            onPlaylistChange(sender, e, playlistPath);
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = (Button)sender;
            DockPanel playlistPanel = (DockPanel)deleteButton.Parent;
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
