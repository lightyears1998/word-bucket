using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData.Binding;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace WordBucket.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private bool _alwaysOnTop = false;

        public bool AlwaysOnTop
        {
            get => _alwaysOnTop;
            set => this.RaiseAndSetIfChanged(ref _alwaysOnTop, value);
        }

        public ICommand OpenDataDirectoryCommand { get; }

        public SettingsViewModel()
        {
            OpenDataDirectoryCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    ProcessStartInfo info = new()
                    {
                        FileName = "explorer.exe",
                        Arguments = AppConfig.ApplicationDataDirectory
                    };

                    await Process.Start(info)!.WaitForExitAsync();
                }
            });

            this.WhenPropertyChanged(x => x.AlwaysOnTop)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => SetAlwaysOnTop());
        }

        public void SetAlwaysOnTop()
        {
            if (Application.Current!.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow != null)
                {
                    desktop.MainWindow.Topmost = AlwaysOnTop;
                }
            }
        }
    }
}
