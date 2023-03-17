using System;
using System.IO;

namespace WordBucket
{
    public static class AppConfig
    {
        public static string AppName => "WordBucket";

        public static string DefaultUserDataDbPath
        {
            get
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Join(appDataFolder, AppName);
            }
        }

        public static string[] AutoCreateFolders => new[] { DefaultUserDataDbPath };
    }
}
