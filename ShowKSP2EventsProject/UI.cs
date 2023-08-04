using UnityEngine;

namespace ShowKSP2Events
{
    internal class UI
    {
        private static UI _instance;
        private Rect _windowRect = new Rect(650, 140, 500, 100);
        private Rect _settingsRect;

        private bool _showSettings;
        private float _justHitTemp;
        private float _stickyTemp;
        private float _tillPrunedTemp;

        internal UI()
        { }

        internal static UI Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UI();
                return _instance;
            }
        }

        internal void DrawMessageListener()
        {
            _windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                _windowRect,
                FillMessageListener,
                "// ShowKSP2Events",
                GUILayout.Height(0)
                );

            if (_showSettings)
                _settingsRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    _settingsRect,
                    FillSettings,
                    "// SETTINGS",
                    GUILayout.Height(0)
                    );

        }

        private void FillMessageListener(int windowID)
        {
            GUILayout.Space(-28);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Textures.Settings, Styles.SettingsButton))
            {
                _settingsRect = new Rect(_windowRect.x - Styles.SettingsWidth, _windowRect.y, Styles.SettingsWidth, 0);
                _justHitTemp = Settings.JustHit;
                _stickyTemp = Settings.StickyDuration;
                _tillPrunedTemp = Settings.DurationTillPruned;
                _showSettings = !_showSettings;
            }
            if (GUILayout.Button(Textures.Export, Styles.ExportButton))
                MessageListener.Instance.OnExportClicked();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clear"))
                MessageListener.Instance.OnClearClicked();
            GUILayout.EndHorizontal();

            // Draw each message
            foreach (var message in MessageListener.Instance.Messages.Where(m => !m.IsIgnored && m.Hits > 0 && !m.IsStale))
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(Textures.Cross, Styles.IgnoreButton))
                    message.IsIgnored = true;
                GUILayout.Space(5);                
                
                if (GUILayout.Button("LOG", message.IsLogging ? Styles.LogButtonEnabled : Styles.LogButtonDisabled))
                    message.IsLogging = !message.IsLogging;
                GUILayout.Space(5);

                if (GUILayout.Button(message.IsPermaSticky ? Textures.PermaStickyActive : Textures.PermaStickyInactive, Styles.PermaSticky))
                    MessageListener.Instance.OnPermaStickyClicked(message.Type);
                GUILayout.Space(5);

                if (message.JustHit)
                    GUILayout.Label($"{message.Type.Name}: ", Styles.MessageJustHitColor);
                else if (message.IsSticky)
                    GUILayout.Label($"{message.Type.Name}: ", Styles.MessageStickyColor);
                else
                    GUILayout.Label($"{message.Type.Name}: ", Styles.MessageNormalColor);

                GUILayout.FlexibleSpace();
                GUILayout.Label(message.Hits.ToString(), Styles.Hits);
                GUILayout.EndHorizontal();
                GUILayout.Space(Styles.SpacingAfterEntry);
            }

            GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
        }

        bool _logTest = false;

        private void FillSettings(int windowID)
        {
            GUILayout.Label($"JustHit (white) duration: {_justHitTemp} sec");
            _justHitTemp = Mathf.Clamp((float)Math.Round(GUILayout.HorizontalSlider(_justHitTemp, 0, 60f), 0), 0, _stickyTemp);

            GUILayout.Label($"Sticky (yellow) duration: {_stickyTemp} sec");
            _stickyTemp = Mathf.Clamp((float)Math.Round(GUILayout.HorizontalSlider(_stickyTemp, 0, 120f), 0), _justHitTemp, _tillPrunedTemp);

            GUILayout.Label($"TillPruned (gray) duration: {_tillPrunedTemp} sec");
            _tillPrunedTemp = Mathf.Clamp((float)Math.Round(GUILayout.HorizontalSlider(_tillPrunedTemp, 0, 120f), 0), _stickyTemp, 120f);

            GUILayout.Space(10);
            if (GUILayout.Button("Save settings"))
            {
                Settings.JustHit = _justHitTemp;
                Settings.StickyDuration = _stickyTemp;
                Settings.DurationTillPruned = _tillPrunedTemp;
                Settings.Save();
                _showSettings = false;
            }

            // Draw ignored messages
            var ignoredMessages = MessageListener.Instance.Messages.Where(m => m.IsIgnored);

            if (ignoredMessages.Count() > 0)
            {
                GUILayout.Space(5);
                GUILayout.Label("--");
                GUILayout.Space(Styles.SpacingAfterEntry);
                GUILayout.Label("Ignored events:");
                GUILayout.Space(5);

                foreach (var message in ignoredMessages)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(Textures.Plus, Styles.IgnoreButton))
                        message.IsIgnored = false;
                    GUILayout.Label(message.TypeName, Styles.LabelBase);
                    GUILayout.EndHorizontal();
                    GUILayout.Space(Styles.SpacingAfterEntry);
                }
            }
        }
    }
}
