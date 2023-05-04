using BepInEx.Logging;
using SpaceWarp.API.UI;
using UnityEngine;

namespace ShowKSP2Events
{
    internal static class Styles
    {
        internal static GUISkin SpaceWarpUISkin;
        internal static GUIStyle LabelBase;
        internal static GUIStyle MessageBase;
        internal static GUIStyle MessageNormalColor;
        internal static GUIStyle MessageStickyColor;
        internal static GUIStyle MessageJustHitColor;
        internal static GUIStyle Hits;
        internal static GUIStyle PermaSticky;
        internal static GUIStyle SaveButton;
        internal static GUIStyle SettingsButton;
        internal static Color NormalTextColor = Color.grey;
        internal static Color StickyTextColor = Color.yellow;
        internal static Color JustHitTextColor = Color.white;
        internal static int SpacingAfterEntry = -12;

        internal static void Initialize()
        {
            SpaceWarpUISkin = Skins.ConsoleSkin;

            LabelBase = new GUIStyle(SpaceWarpUISkin.label)
            {
                contentOffset = new Vector2(0, -7)
            };

            MessageBase = new GUIStyle(LabelBase)
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

            Hits = new GUIStyle(LabelBase)
            {
                alignment = TextAnchor.MiddleRight,
                fixedWidth = 50
            };

            PermaSticky = new GUIStyle(SpaceWarpUISkin.button)
            {
                fixedHeight = 15,
                fixedWidth = 15,
                padding = new RectOffset(-2, -2, -2, -2)
            };
            PermaSticky.normal.background = null;

            SettingsButton = new GUIStyle(SpaceWarpUISkin.button)
            {
                fixedHeight = 28,
                fixedWidth = 28,
                padding = new RectOffset(-2, -2, -2, -2)
            };

            SaveButton = new GUIStyle(SpaceWarpUISkin.button)
            {
                fixedHeight = 28,
                fixedWidth = 28,
                padding = new RectOffset(-2, -2, -2, -2)
            };
        }
    }
}