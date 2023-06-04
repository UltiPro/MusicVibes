#pragma warning disable CS0168

using MusicVibes.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMPLib;

namespace MusicVibes.Pages
{
    public partial class MainPage : Page
    {
        public ObservableCollection<MusicFile> musicFiles { get; set; } = new ObservableCollection<MusicFile>();
        private string[] extensions = new string[] { "mp3", "wav" };
        public MainPage()
        {
            InitializeComponent();
        }
        public bool LoadFiles()
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                try
                {
                    folderBrowserDialog.ShowDialog();
                    DirectoryInfo directoryInfo = new DirectoryInfo(folderBrowserDialog.SelectedPath);
                    var tempMusicFiles = directoryInfo.GetFiles("*.*").Where(file => extensions.Contains(System.IO.Path.GetExtension(file.FullName).TrimStart('.').ToLowerInvariant()));
                    musicFiles.Clear();
                    WindowsMediaPlayerClass wmp_temp = new WindowsMediaPlayerClass();
                    foreach (var musicFile in tempMusicFiles)
                    {
                        musicFiles.Add(new MusicFile(musicFile.FullName, musicFile.Name.Remove(musicFile.Name.Length - 4), musicFile.Extension, wmp_temp.newMedia(musicFile.FullName).durationString));
                    }
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}
