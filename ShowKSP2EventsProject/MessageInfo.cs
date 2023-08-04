using Newtonsoft.Json;
using UnityEngine;

namespace ShowKSP2Events
{
    public class MessageInfo
    {
        public Type Type;
        [JsonProperty]
        public string TypeName => Type.Name;
        [JsonProperty]
        public int Hits;
        public double TimeOfLastHit;
        [JsonProperty]
        public string DateTimeOfLastHit;
        public bool IsSticky;
        public bool IsPermaSticky;
        public bool IsStale;
        public bool JustHit => Time.time - TimeOfLastHit < Settings.JustHit;
    }
}
