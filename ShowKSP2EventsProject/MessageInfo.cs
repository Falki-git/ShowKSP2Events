using UnityEngine;

namespace ShowKSP2Events
{
    internal class MessageInfo
    {
        internal Type Type;
        internal int Hits;
        internal double TimeOfLastHit;
        internal bool IsSticky;
        internal bool IsPermaSticky; // TODO: implement
        internal bool IsStale;
        internal bool JustHit => Time.time - TimeOfLastHit < Settings.JustHit;
    }
}
