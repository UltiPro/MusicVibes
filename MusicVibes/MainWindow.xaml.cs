using Microsoft.Win32;
using System.IO;
using System.Runtime;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MusicVibes
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<FileInfo>? _files;
        private string[] extensions = new string[] { "mp3", "wav"};
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OpenFiles_Click(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.ShowDialog();
                _files = new ObservableCollection<FileInfo>();
                DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                var tempMusic = directoryInfo.GetFiles("*.*").Where(file => extensions.Contains(Path.GetExtension(file.FullName).TrimStart('.').ToLowerInvariant()));
                foreach(var file in tempMusic) _files.Add(file);
            }
        }
    }
}
