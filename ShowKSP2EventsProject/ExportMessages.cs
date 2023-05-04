using BepInEx.Logging;
using Newtonsoft.Json;
using System.Reflection;

namespace ShowKSP2Events
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ExportMessages
    {
        [JsonProperty]
        internal string DateTimeCreated;
        [JsonProperty]
        internal List<MessageInfo> Messages;

        private static ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("ShowKSP2Events.ExportMessages");
        private static int _fileNumber = 0;
        private static string _path => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"ShowKSP2Events_export({_fileNumber}).json");

        internal ExportMessages(List<MessageInfo> messages)
        {
            Messages = messages;
            DateTimeCreated = DateTime.Now.ToString();
        }

        internal void Export()
        {
            try
            {
                while (File.Exists(_path))
                    _fileNumber++;

                var data = new SettingsData();
                File.WriteAllText(_path, JsonConvert.SerializeObject(this));
                _logger.LogInfo("Export successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error trying to export data. Error description: " + ex);
            }
        }
    }
}
