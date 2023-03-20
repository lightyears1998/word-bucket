using Microsoft.EntityFrameworkCore;
using System.IO;
using WordBucket.Contexts;

namespace WordBucket
{
    public static class AppConfig
    {
        public static string AppName => "WordBucket";

        public static string AssemblyLocation => System.Reflection.Assembly.GetExecutingAssembly().Location;

        public static string WorkingDirectory => Directory.GetCurrentDirectory();

        public static int MainWindowHeight => 450;

        public static int MainWindowWidth => 600;

        public static string ApplicationDataDirectory
        {
            get
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#if DEBUG
                return Path.Join(appDataFolder, $"{AppName} (Dev)");
#else
                return Path.Join(appDataFolder, AppName);
#endif
            }
        }

        public static string DefaultUserSettingsPath => Path.Join(ApplicationDataDirectory, "./UserSettings.JSON");

        public static string DefaultUserDataDbPath => Path.Join(ApplicationDataDirectory, "./UserData.SQLite3");

        public static string DefaultDictionaryDataDbPath => Path.Join(ApplicationDataDirectory, "./Dictionary.SQLite3");

        public static string[] AutoCreateDirectories => new[] { ApplicationDataDirectory };

        public static int HttpListenerPort => 9123;

        public static void MigrateDatabases()
        {
            using var user = new UserContext();
            using var dictionary = new DictionaryContext();

            user.Database.Migrate();
            dictionary.Database.Migrate();
        }
    }
}
