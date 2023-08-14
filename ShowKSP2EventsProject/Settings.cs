using BepInEx.Logging;
using Newtonsoft.Json;
using System.Reflection;

namespace ShowKSP2Events
{
    public class Settings
    {
        public static float JustHit = 1.0f;
        public static float StickyDuration = 20.0f;
        public static float DurationTillPruned = 60.0f;
        public static bool LogAll;

        private static ManualLogSource _logger = Logger.CreateLogSource("ShowKSP2Events.Settings");

        private static string _path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Settings.json");

        public static void Save()
        {
            try
            {
                var data = new SettingsData();
                File.WriteAllText(_path, JsonConvert.SerializeObject(data));
                _logger.LogInfo("Settings saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error trying to save settings. Error description: " + ex);
            }
        }

        public static void Load()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<SettingsData>(File.ReadAllText(_path));

                Settings.StickyDuration = data.StickyDuration;
                Settings.DurationTillPruned = data.DurationTillPruned;
                Settings.JustHit = data.JustHit;

                // Load data for messages that have been modified (ignored, logging or stickied)
                if (data.SavedMessages.Count() > 0)
                {
                    var savedMessages = MessageListener.Instance.Messages
                        .FindAll(m => data.SavedMessages.Select(s => s.TypeName).Contains(m.TypeName));

                    foreach (var message in savedMessages)
                    {
                        var savedData = data.SavedMessages.Find(s => s.TypeName == message.TypeName);
                        message.IsIgnored = savedData.IsIgnored;
                        message.IsLogging = savedData.IsLogging;
                        if(savedData.IsPermaSticky)
                        {
                            message.IsPermaSticky = true;
                            MessageListener.Instance.MoveToBelowLastPermaSticky(message);
                        }
                    }
                }

                _logger.LogInfo("Settings loaded successfully.");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning($"Settings file was not found at the expected location. Mod will continue with default settings. Full description:\n" + ex);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error trying to load settings. Full error description:\n" + ex);
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    internal class SettingsData
    {
        [JsonProperty]
        internal float StickyDuration;
        [JsonProperty]
        internal float DurationTillPruned;
        [JsonProperty]
        internal float JustHit;
        [JsonProperty]
        internal bool LogAll;
        [JsonProperty]
        internal List<SettingsMessageData> SavedMessages;

        internal SettingsData()
        {
            StickyDuration = Settings.StickyDuration;
            DurationTillPruned = Settings.DurationTillPruned;
            JustHit = Settings.JustHit;
            LogAll = Settings.LogAll;

            var savedMessages = MessageListener.Instance.Messages
                .Where(m => m.IsIgnored || m.IsLogging || m.IsPermaSticky);

            if (savedMessages.Count() > 0)
            {
                SavedMessages = new();

                foreach (var m in savedMessages)
                    SavedMessages.Add(new() {
                        TypeName = m.TypeName,
                        IsIgnored = m.IsIgnored,
                        IsLogging = m.IsLogging,
                        IsPermaSticky = m.IsPermaSticky
                    });
            }
        }
    }

    internal class SettingsMessageData
    {
        [JsonProperty]
        internal string TypeName;
        [JsonProperty]
        internal bool IsIgnored;
        [JsonProperty]
        internal bool IsLogging;
        [JsonProperty]
        internal bool IsPermaSticky;
    }
}