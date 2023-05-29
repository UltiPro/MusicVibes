using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;

namespace MusicVibes
{
    public partial class MainWindow : Window
    {
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Media Files|*.wav;*.mp3;*.WAV;*.MP3;";
            openFileDialog.Multiselect = true;
            if(openFileDialog.ShowDialog() == true)
            {

            }
        }
    }
}
