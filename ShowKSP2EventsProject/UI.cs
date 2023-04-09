using UnityEngine;

namespace ShowKSP2Events
{
    internal class UI
    {
        private Rect _windowRect = new Rect(650, 140, 500, 100);
        private MessageListener _listener;

        internal void DrawMessageListener(MessageListener listener)
        {
            _listener = listener;

            _windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                _windowRect,
                FillMessageListener,
                "ShowKSP2Events",
                GUILayout.Height(0)
                );
        }

        private void FillMessageListener(int windowID)
        {
            foreach (var message in _listener.Messages.Where(m => m.Hits > 0 && !m.IsStale))
            {
                GUILayout.BeginHorizontal();
                if (message.JustHit)
                    GUILayout.Label($"{message.Type.Name}: ", Styles.MessageJustHitColor);
                else if (message.IsSticky || message.IsPermaSticky)
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
    }
}
