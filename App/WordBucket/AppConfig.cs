using System.IO;

namespace WordBucket
{
    public static class AppConfig
    {
        public static string AppName => "WordBucket";

        public static int MainWindowHeight => 450;

        public static int MainWindowWidth => 600;

        public static string DefaultUserDataDbPath
        {
            get
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Join(appDataFolder, AppName);
            }
        }

        public static string[] AutoCreateFolders => new[] { DefaultUserDataDbPath };

        public static int HttpListenerPort => 51238;
    }
}
