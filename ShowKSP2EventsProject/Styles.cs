using SpaceWarp.API.UI;
using UnityEngine;

namespace ShowKSP2Events
{
    public static class Styles
    {
        public static GUISkin SpaceWarpUISkin;
        public static GUIStyle LabelBase;
        public static GUIStyle MessageBase;
        public static GUIStyle MessageNormalColor;
        public static GUIStyle MessageStickyColor;
        public static GUIStyle MessageJustHitColor;
        public static GUIStyle Hits;
        public static GUIStyle PermaSticky;
        public static GUIStyle ExportButton;
        public static GUIStyle SettingsButton;
        public static Color NormalTextColor = Color.grey;
        public static Color StickyTextColor = Color.yellow;
        public static Color JustHitTextColor = Color.white;
        public static int SpacingAfterEntry = -12;

        public static int SettingsWidth = 300;

        public static void Initialize()
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
                fixedHeight = 20,
                fixedWidth = 20,
                padding = new RectOffset(-2, -2, -2, -2)
            };

            ExportButton = new GUIStyle(SpaceWarpUISkin.button)
            {
                fixedHeight = 20,
                fixedWidth = 20,
                padding = new RectOffset(-2, -2, -2, -2)
            };
        }
    }
}