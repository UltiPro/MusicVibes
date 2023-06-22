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
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void LanguageButton_Pl(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "pl-PL";
            Properties.Settings.Default.Save();
        }

        private void LanguageButton_En(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.languageCode = "en-US";
            Properties.Settings.Default.Save();
        }

        private void ThemeButton_Light(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Resources["BackgroundColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e0b1cb"));
            System.Windows.Application.Current.Resources["BackgroundColor2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#be95c4"));
            System.Windows.Application.Current.Resources["BackgroundColor3"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9f86c0"));
            System.Windows.Application.Current.Resources["WhiteSpecial"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ecf8f8"));

            System.Windows.Application.Current.Resources["FontColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#231942"));
            System.Windows.Application.Current.Resources["FontColor2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f72585"));
        }

        private void ThemeButton_Dark(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Resources["BackgroundColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CF6679"));
            System.Windows.Application.Current.Resources["BackgroundColor2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#252525"));
            System.Windows.Application.Current.Resources["BackgroundColor3"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03dac6"));
            System.Windows.Application.Current.Resources["WhiteSpecial"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d3d3d3"));

            System.Windows.Application.Current.Resources["FontColor1"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#bb86fc"));
            System.Windows.Application.Current.Resources["FontColor2"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6200ee"));
        }
    }
}

