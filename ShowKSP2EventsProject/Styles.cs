using SpaceWarp.API.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShowKSP2Events
{
    internal static class Styles
    {
        internal static GUISkin SpaceWarpUISkin;
        internal static GUIStyle MessageBase;
        internal static GUIStyle MessageNormalColor;
        internal static GUIStyle MessageStickyColor;
        internal static GUIStyle MessageJustHitColor;
        internal static GUIStyle Hits;
        internal static Color NormalTextColor = Color.grey;
        internal static Color StickyTextColor = Color.yellow;
        internal static Color JustHitTextColor = Color.white;
        internal static int SpacingAfterEntry = -12;

        internal static void InitializeStyles()
        {
            SpaceWarpUISkin = Skins.ConsoleSkin;

            MessageBase = new GUIStyle(SpaceWarpUISkin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fixedWidth = 400
            };

            MessageNormalColor = new GUIStyle(MessageBase);
            MessageNormalColor.normal.textColor = NormalTextColor;

            MessageStickyColor = new GUIStyle(MessageBase);
            MessageStickyColor.normal.textColor = StickyTextColor;

            MessageJustHitColor = new GUIStyle(MessageBase);
            MessageJustHitColor.normal.textColor = JustHitTextColor;
            MessageJustHitColor.fontStyle = FontStyle.Bold;

            Hits = new GUIStyle(SpaceWarpUISkin.label)
            {
                alignment = TextAnchor.MiddleRight,
                fixedWidth = 50
            };
        }
    }
}
