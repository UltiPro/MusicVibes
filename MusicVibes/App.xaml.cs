using System.Threading;
using System.Windows;

namespace MusicVibes
{
    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var langCode = MusicVibes.Properties.Settings.Default.languageCode;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(langCode);
            base.OnStartup(e);
        }
    }
}
