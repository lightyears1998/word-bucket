using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using WordBucket.Services;
using WordBucket.ViewModels;
using WordBucket.Views;

namespace WordBucket;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(),
                Topmost = true,
                Height = AppConfig.MainWindowHeight,
                Width = AppConfig.MainWindowWidth,
                WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterScreen
            };

            CreateFolders();
            Task.Run(CollectorService.Instance.InitializeAsync);
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void CreateFolders()
    {
        var folders = AppConfig.AutoCreateFolders;
        foreach (var folder in folders)
        {
            System.IO.Directory.CreateDirectory(folder);
        }
    }
}
