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
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Forms;

namespace MusicVibes.Pages
{

    
    /// <summary>
    /// Logika interakcji dla klasy SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AudioOutputComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AudioQualityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.ComboBox comboBox = (System.Windows.Controls.ComboBox)sender;
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)comboBox.SelectedItem;

            string selectedQuality = selectedComboBoxItem.Content.ToString();

            switch (selectedQuality)
            {
                case "Low":
                    
                    break;
                case "Medium":
                    // Logika dla opcji "Medium"
                    break;
                case "High":
                    // Logika dla opcji "High"
                    break;
                default:
                    // Logika dla innych przypadków (jeśli potrzebna)
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmb_lg.SelectedIndex== 0)
                 Properties.Settings.Default.languageCode = "en-US";
            else
                Properties.Settings.Default.languageCode = "pl-PL";
            Properties.Settings.Default.Save();

            //TODO wyświetlnie ostrzeżenia o potrzebie restartu aplikacji i zapytanie czy zrobić to teraz 
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
                System.Windows.Application.Current.Shutdown();
            });

        }
    }
}

