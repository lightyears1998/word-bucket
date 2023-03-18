using System.IO;
using System.Text.Json;

namespace WordBucket
{
    public class UserSettings
    {
        private UserSettings()
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

        private static UserSettings Load(string? settingsPath = null)
        {
            settingsPath ??= AppConfig.DefaultUserSettingsPath;
            var settingsText = File.ReadAllText(settingsPath);
            return JsonSerializer.Deserialize<UserSettings>(settingsText)!;
        }

        public void Save(string? settingsPath = null)
        {
            var settingsText = JsonSerializer.Serialize(this);
            settingsPath ??= AppConfig.DefaultUserSettingsPath;
            File.WriteAllText(settingsPath, settingsText);
        }
    }
}
