using System.IO;
using System.Text.Json;

namespace WordBucket
{
    public record class UserSettings
    {
        public bool MainWindowAlwaysOnTop { set; get; } = true;

        public string? UserDataDbPath { set; get; } = null;

        public string? DictionaryDataDbPath { set; get; } = null;

        public UserSettings()
        { }

        private static UserSettings? _current;

        public static UserSettings Current
        {
            get
            {
                _current ??= Load();
                return _current;
            }
        }

        private UserSettings? _previousSettings;

        public bool DetectChanges()
        {
            if (_previousSettings == null)
            {
                return true;
            }

            return _previousSettings != this;
        }

        private static UserSettings Load(string? settingsPath = null)
        {
            settingsPath ??= AppConfig.DefaultUserSettingsPath;

            if (!File.Exists(settingsPath))
            {
                return new UserSettings();
            }
            var settingsText = File.ReadAllText(settingsPath);

            var settings = JsonSerializer.Deserialize<UserSettings>(settingsText)!;
            settings._previousSettings = settings with { };
            return settings;
        }

        public void Save(string? settingsPath = null)
        {
            var settingsText = JsonSerializer.Serialize(this);
            settingsPath ??= AppConfig.DefaultUserSettingsPath;
            File.WriteAllText(settingsPath, settingsText);
        }
    }
}
