using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.ScriptableObejct
{
    [CreateAssetMenu]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise()
        {
            for(int i = listeners.Count -1; i >= 0; i--)
                listeners[i].OnEventRaised();
        }

        public void Raise(string data)
        {
            for(int i = listeners.Count -1; i >= 0; i--)
                listeners[i].OnEventRaised(data);
        }

        public void Raise(int data)
        {
            for(int i = listeners.Count -1; i >= 0; i--)
                listeners[i].OnEventRaised(data);
        }

        public void RegisterListener(GameEventListener listener) { listeners.Add(listener); }

        public void UnregisterListener(GameEventListener listener) { listeners.Remove(listener); }
    }
}

