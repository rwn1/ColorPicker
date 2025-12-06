using ColorPicker.App3.ViewModels;
using System.Windows;

namespace ColorPicker.App3
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