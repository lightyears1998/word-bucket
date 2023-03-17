using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Diagnostics;
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
            InitializeServices();
            desktop.MainWindow.Closed += (_, _) => StopServices();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };

            ThrowHelper.ThrowPlatformNotSupportedException();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void InitializeServices()
    {
        Task.Run(CollectorService.Instance.InitializeAsync);
    }

    private static void StopServices()
    {
        CollectorService.Instance.Stop();
    }

    private static void CreateFolders()
    {
        var directories = AppConfig.AutoCreateDirectories;
        foreach (var directory in directories)
        {
            System.IO.Directory.CreateDirectory(directory);
        }
    }
}
