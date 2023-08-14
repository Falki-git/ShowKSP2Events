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
        public static GUIStyle SaveButton;
        public static GUIStyle SettingsButton;
        public static GUIStyle IgnoreButton;
        public static GUIStyle LogButtonDisabledButton;
        public static GUIStyle LogButtonEnabledButton;
        public static GUIStyle LogAllButton;
        public static GUIStyle MessageLabel;
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

            SaveButton = new GUIStyle(SpaceWarpUISkin.button)
            {
                fixedHeight = 20,
                fixedWidth = 20,
                padding = new RectOffset(-2, -2, -2, -2)
            };

            IgnoreButton = new GUIStyle(SpaceWarpUISkin.button)
            {
                fixedHeight = 15,
                fixedWidth = 15,
                padding = new RectOffset(0, 0, 0, 0)
            };
            IgnoreButton.normal.background = null;

            LogButtonDisabledButton = new GUIStyle(SpaceWarpUISkin.button)
            {
                fixedHeight = 15,
                fixedWidth = 20,
                padding = new RectOffset(-2, -2, -2, -2)
            };
            LogButtonDisabledButton.normal.background = null;
            LogButtonDisabledButton.normal.textColor = new Color32(80, 80, 80, 255); // Dark gray

            LogButtonEnabledButton = new GUIStyle(LogButtonDisabledButton);
            LogButtonEnabledButton.normal.textColor = Color.white;

            LogAllButton = new GUIStyle(SpaceWarpUISkin.button)
            {
                fixedHeight = 20,
                fixedWidth = 50,
                padding = new RectOffset(-2, -2, -2, -2)
            };
            LogAllButton.normal.background = null;

            MessageLabel = new GUIStyle(SpaceWarpUISkin.label);
            MessageLabel.normal.textColor = Color.green;
        }
    }
}