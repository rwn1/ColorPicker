using ColorPicker.Maui.App1.ViewModels;

namespace ColorPicker.Maiu.App1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell()
        {
            BindingContext = new MainViewModel()
        };
    }
}