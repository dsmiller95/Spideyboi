using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets
{
    public class Listener
    {
        public EVENT_TYPE type;
        public Action<object> onEvent;
        public Listener(EVENT_TYPE type, Action<object> onEvent)
        {
            this.type = type;
            this.onEvent = onEvent;
        }
    }

    [Serializable]
    public class ObjectEvent : UnityEvent<object> { }

    public class CustomEventSystem : MonoBehaviour
    {
        public IList<Listener> listeners;
        public static CustomEventSystem instance;

        private void Awake()
        {
            instance = this;
            listeners = new List<Listener>();
        }

        public void Dispatch(EVENT_TYPE eventName, object data)
        {
            foreach (var listener in listeners.Where(x => x.type == eventName))
            {
                listener.onEvent(data);
            }
        }

        public void RegisterListener(Listener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(Listener listener)
        {
            listeners.Remove(listener);
        }

    }

    public enum EVENT_TYPE
    {
        WIN,
        RESET,
    }
}
