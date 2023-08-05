using BepInEx.Logging;
using KSP.Game;
using KSP.Messages;
using System.Reflection;
using UnityEngine;

namespace ShowKSP2Events
{
    public class MessageListener
    {        
        private ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("ShowKSP2Events.MessageListener");
        private static MessageListener _instance;

        public List<MessageInfo> Messages = new();

        public MessageListener ()
        { }

        public static MessageListener Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MessageListener();
                return _instance;
            }
        }

        internal void InitializeSubscriptions()
        {
            var messageCenterType = typeof(MessageCenter);
            var messageTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(MessageCenterMessage)));

            foreach (var messageType in messageTypes)
            {
                try
                {
                    var method = typeof(MessageListener).GetMethod(nameof(MessageListener.MessageReceived), BindingFlags.Instance | BindingFlags.NonPublic);
                    var action = (Action<MessageCenterMessage>)Delegate.CreateDelegate(typeof(Action<MessageCenterMessage>), this, method);

                    var specificMethod = messageCenterType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(m => m.Name == "Subscribe")
                        .Select(m => new { Method = m, Parameters = m.GetParameters() })
                        .Where(x => x.Parameters.Length == 1 && x.Parameters[0].ParameterType.IsGenericType)
                        .Where(x => x.Parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(Action<>))
                        .Where(x => x.Parameters[0].ParameterType.GetGenericArguments()[0].IsAssignableFrom(messageType))
                        .Select(x => x.Method)
                        .FirstOrDefault();

                    specificMethod.MakeGenericMethod(messageType).Invoke(GameManager.Instance.Game.Messages, new object[] { action });

                    this.AddSubscription(messageType);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex);
                }
            }

            _logger.LogInfo($"Subscriptions initialized. No. of subscriptions: {Messages.Count}");
        }

        private void AddSubscription(Type messageType)
        {
            Messages.Add(new MessageInfo { Type = messageType });
        }

        private void MessageReceived(MessageCenterMessage messageReceived)
        {
            var messageInfo = Messages.Find(m => m.Type == messageReceived.GetType());
            messageInfo.Hits++;
            messageInfo.TimeOfLastHit = Time.time;
            messageInfo.DateTimeOfLastHit = DateTime.Now.ToString();
            messageInfo.IsSticky = true;
            messageInfo.IsStale = false;

            if (messageInfo.IsLogging)
                _logger.LogInfo($"Message {messageInfo.TypeName} triggered at {messageInfo.DateTimeOfLastHit}. Hit number: {messageInfo.Hits}.");

            if (!messageInfo.IsPermaSticky && !messageInfo.IsSticky)
                MoveToBelowLastSticky(messageInfo);
        }

        private void MoveToBelowLastSticky(MessageInfo message)
        {
            Messages.Remove(message);
            int lastStickyIndex = Messages.FindLastIndex(m => m.IsSticky || m.IsPermaSticky);
            Messages.Insert(lastStickyIndex == -1 ? 0 : lastStickyIndex + 1, message);
        }

        internal void MoveToBelowLastPermaSticky(MessageInfo message)
        {
            Messages.Remove(message);
            int lastPermaStickyIndex = Messages.FindLastIndex(m => m.IsPermaSticky);
            Messages.Insert(lastPermaStickyIndex == -1 ? 0 : lastPermaStickyIndex + 1, message);
        }

        public void UnSticky(MessageInfo message)
        {
            var messageInfo = Messages.Find(m => m.Type == message.Type);
            messageInfo.IsSticky = false;

            if (!message.IsPermaSticky)
                this.MoveToBelowLastSticky(messageInfo);
        }

        internal void CheckStickies()
        {
            foreach (var message in Messages)
            {
                if (message.IsSticky && Time.time - message.TimeOfLastHit > Settings.StickyDuration)
                {
                    UnSticky(message);
                }
            }
        }

        internal void CheckStales()
        {
            foreach (var message in Messages.Where(m => !m.IsStale && !m.IsPermaSticky))
            {
                if (Time.time - message.TimeOfLastHit > Settings.DurationTillPruned)
                {
                    message.IsStale = true;
                }
            }
        }

        public void OnPermaStickyClicked(Type messageType)
        {
            var message = Messages.Find(m => m.Type == messageType);
            message.IsPermaSticky = !message.IsPermaSticky;
            MoveToBelowLastPermaSticky(message);            
            Settings.Save();
            _logger.LogInfo($"Toggled pinning for {message.TypeName}.");
        }

        public void OnClearClicked()
        {
            foreach (var message in Messages)
            {
                message.Hits = 0;
                message.TimeOfLastHit = 0;
                message.IsPermaSticky = false;
                message.IsSticky = false;
                message.IsStale = true;
            }
        }

        public void OnExportClicked()
        {
            var x = new ExportMessages(Messages.FindAll(m => m.Hits > 0));
            x.Export();
        }
    }    
}
