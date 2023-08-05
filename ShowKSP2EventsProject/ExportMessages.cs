using BepInEx.Logging;
using Newtonsoft.Json;
using System.Reflection;

namespace ShowKSP2Events
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExportMessages
    {
        [JsonProperty]
        public string DateTimeCreated;
        [JsonProperty]
        public List<MessageInfo> Messages;

        private static ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("ShowKSP2Events.ExportMessages");
        private static int _fileNumber = 0;
        private static string _path => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"ShowKSP2Events_export({_fileNumber}).json");

        public ExportMessages(List<MessageInfo> messages)
        {
            Messages = messages;
            DateTimeCreated = DateTime.Now.ToString();
        }

        public void Export()
        {
            try
            {
                while (File.Exists(_path))
                    _fileNumber++;

                File.WriteAllText(_path, JsonConvert.SerializeObject(this));
                _logger.LogInfo("Export successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error trying to export data. Error description: " + ex);
            }
        }

        public void WriteAllToLog()
        {
            _logger.LogInfo($"Writing all {Messages.Count} messages to log...");

            foreach (var message in Messages)
                _logger.LogInfo($"{message.TypeName}");
        }
    }
}
