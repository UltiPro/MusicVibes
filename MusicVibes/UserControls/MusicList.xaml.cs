#pragma warning disable CS8602, CS8618

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicVibes.UserControls;

public partial class MusicList : UserControl
{
    public int? selectedMusic { get; private set; } = null;
    public delegate void OnMusicChange(object sender, RoutedEventArgs e, int id);
    public event OnMusicChange onMusicChange;
    public delegate void OnFocusChange(object sender, KeyEventArgs e);
    public event OnFocusChange onFocusChange;

    public MusicList() => InitializeComponent();

    private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (selectedMusic == null) return;
        onMusicChange(sender, e, (int)selectedMusic);
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListView? listView = sender as ListView;
        if (listView != null) selectedMusic = listView.SelectedIndex;
    }

    private void ListViewItem_SpacePreviewKeyDown(object sender, KeyEventArgs e) => onFocusChange(sender, e);

    public void SetItemFocus(int id) => List.SelectedIndex = id;
}