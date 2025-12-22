using ColorPicker.Wpf.App1.ViewModels;
using System.Windows;

namespace ColorPicker.Wpf.App1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            mainWindow.ShowDialog();
        }
    }
}