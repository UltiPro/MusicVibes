﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace MusicVibes.Pages;

public partial class SettingsPage : Page
{
    public ObservableCollection<string> themes { get; } = new ObservableCollection<string>();
    public ObservableCollection<string> languages { get; } = new ObservableCollection<string>();
    public string selectedTheme { get; private set; } = "Dark";
    public string selectedLanguage { get; private set; } = "English";

    public SettingsPage(string theme, string language)
    {
        InitializeComponent();
        themes.Add("Dark");
        themes.Add("Light");
        languages.Add("English");
        languages.Add("French");
        languages.Add("German");
        languages.Add("Italian");
        languages.Add("Japanese");
        languages.Add("Polish");
        languages.Add("Spanish");
        if (themes.Contains(theme)) selectedTheme = theme;
        if (languages.Contains(language)) selectedLanguage = language;
        Application.Current.Resources.MergedDictionaries.First().Source =
            new Uri($"pack://application:,,,/Themes/Colors/{selectedTheme}.xaml");
        Application.Current.Resources.MergedDictionaries.Last().Source =
            new Uri($"pack://application:,,,/Languages/{selectedLanguage}.xaml");
        ListViewThemes.SelectedIndex = themes.IndexOf(selectedTheme);
        ListViewLanguages.SelectedIndex = languages.IndexOf(selectedLanguage);
    }

    private void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Application.Current.Resources.MergedDictionaries.First().Source =
            new Uri($"pack://application:,,,/Themes/Colors/{themes[ListViewThemes.SelectedIndex]}.xaml");
        selectedTheme = themes[ListViewThemes.SelectedIndex];
    }

    private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Application.Current.Resources.MergedDictionaries.Last().Source =
            new Uri($"pack://application:,,,/Languages/{languages[ListViewLanguages.SelectedIndex]}.xaml");
        selectedLanguage = languages[ListViewLanguages.SelectedIndex];
    }
}