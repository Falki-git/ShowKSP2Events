using Newtonsoft.Json;
using UnityEngine;

namespace ShowKSP2Events
{
    internal class MessageInfo
    {
        internal Type Type;
        [JsonProperty]
        internal string TypeName => Type.Name;
        [JsonProperty]
        internal int Hits;
        internal double TimeOfLastHit;
        [JsonProperty]
        internal string DateTimeOfLastHit;
        internal bool IsSticky;
        internal bool IsPermaSticky;
        internal bool IsStale;
        internal bool JustHit => Time.time - TimeOfLastHit < Settings.JustHit;
    }
}
