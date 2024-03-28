using System.IO;
using System.Security.AccessControl;
using System;
using System.Windows;
using System.Windows.Input;
using MusicVibes.Pages;
using System.Collections.Generic;

namespace MusicVibes;

public partial class MainWindow : Window
{
    private const string settingsFilePath = "settings";
    public MainPage mainPage;
    public PlaylistsPage playlistsPage;
    public SettingsPage settingsPage;

    public MainWindow()
    {
        List<string?> settings = new List<string?>();
        if (File.Exists(settingsFilePath))
            foreach (string setting in File.ReadAllLines(settingsFilePath))
                settings.Add(setting);
        while (settings.Count < 4) settings.Add(null);
        mainPage = new MainPage(int.TryParse(settings[0], out int vol) ? vol : 50, settings[1] ?? "");
        playlistsPage = new PlaylistsPage();
        settingsPage = new SettingsPage(settings[2] ?? "", settings[3] ?? "");
        InitializeComponent();
        MainFrame.Content = mainPage;
        playlistsPage.onPlaylistChange += OpenFolderFromPlaylists;
    }

    private void App_MouseDown(object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) DragMove(); }
    private void MainButton_Click(object sender, RoutedEventArgs e) => MainFrame.Content = mainPage;
    private void OpenFolderButton_Click(object sender, RoutedEventArgs e) { if (mainPage.LoadFilesFromDialog()) MainFrame.Content = mainPage; }
    private void PlaylistsButton_Click(object sender, RoutedEventArgs e) => MainFrame.Content = playlistsPage;
    private void SettingsButton_Click(object sender, RoutedEventArgs e) => MainFrame.Content = settingsPage;

    private void QuitButton_Click(object sender, RoutedEventArgs e)
    {
        mainPage.stopApp = true;
        if (!File.Exists(settingsFilePath))
        {
            using (FileStream settingsFile = File.Create(settingsFilePath))
            {
                settingsFile.Close();
                new FileInfo(settingsFilePath).SetAccessControl(
                    new FileSecurity(settingsFilePath, AccessControlSections.Access));
            }
        }
        try
        {
            File.WriteAllLines(settingsFilePath, new List<string>
            {
                mainPage.currentVolume.ToString(),
                mainPage.currentPath ?? "",
                settingsPage.selectedTheme,
                settingsPage.selectedLanguage
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while saving settings: {ex.Message}");
        }
        Close();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    public void OpenFolderFromPlaylists(object sender, RoutedEventArgs e, string path) { if (mainPage.LoadFilesFromPlaylists(path)) MainFrame.Content = mainPage; }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            mainPage.ChangeMusic(sender, e, 0);
        else if (e.Key == Key.Space)
            mainPage.StartPauseMusic(sender, e);
    }
}