using System;
using System.Collections.Generic;

namespace CrimsonCompass.Core
{
    public class EventBus
    {
        private readonly Dictionary<GameEventType, Action<object>> _handlers = new();

        public void Subscribe(GameEventType type, Action<object> handler)
        {
            if (!_handlers.ContainsKey(type)) _handlers[type] = _ => { };
            _handlers[type] += handler;
        }

        public void Publish(GameEventType type, object payload = null)
        {
            UnityEngine.Debug.Log("Event published: " + type + (payload != null ? " with payload" : ""));
            if (_handlers.TryGetValue(type, out var h)) h?.Invoke(payload);
        }
    }
}
