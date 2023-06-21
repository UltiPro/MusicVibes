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

namespace MusicVibes.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy PlaylistsPage.xaml
    /// </summary>
    public partial class PlaylistsPage : Page
    {
        private readonly string playlistsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Playlists");

        public PlaylistsPage()
        {

            InitializeComponent();
            LoadFolders();
        }
        private void LoadFolders()
        {
            if (!Directory.Exists(playlistsFolderPath))
            {
                MessageBox.Show("Folder 'Playlists' nie istnieje na pulpicie.");
                return;
            }

            string[] folders = Directory.GetDirectories(playlistsFolderPath);
            foreach (string folderPath in folders)
            {
                string folderName = Path.GetFileName(folderPath);
                Button folderButton = new Button
                {
                    Content = folderName,
                    Margin = new Thickness(5),
                    Tag = folderPath
                };
                folderButton.Click += FolderButton_Click;
                foldersPanel.Children.Add(folderButton);
            }
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            Button folderButton = (Button)sender;
            string folderPath = (string)folderButton.Tag;
            LoadSongs(folderPath);
        }

        private void LoadSongs(string folderPath)
        {
            string[] songs = Directory.GetFiles(folderPath, "*.mp3");

            List<string> songNames = new List<string>();
            foreach (string songPath in songs)
            {
                string songName = Path.GetFileName(songPath);
                songNames.Add(songName);
            }

            songsListBox.ItemsSource = songNames;
        }
    }
}
