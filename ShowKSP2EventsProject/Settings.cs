using BepInEx.Logging;
using Newtonsoft.Json;
using System.Reflection;

namespace ShowKSP2Events
{
    public class Settings
    {
        public static float StickyDuration = 20.0f;
        public static float DurationTillPruned = 30.0f; //60.0f
        public static float JustHit = 1.0f;

        private static ManualLogSource _logger = Logger.CreateLogSource("ShowKSP2Events.Settings");

        private static string _path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Settings.json");

        public static void Save()
        {
            try
            {
                var data = new SettingsData();

                var temp = JsonConvert.SerializeObject(data);
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

                _logger.LogInfo("Settings loaded successfully.");
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning($"Error loading settings. File was not found at the expected location. Full error description:\n" + ex);

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

        internal SettingsData()
        {
            StickyDuration = Settings.StickyDuration;
            DurationTillPruned = Settings.DurationTillPruned;
            JustHit = Settings.JustHit;
        }
    }
}