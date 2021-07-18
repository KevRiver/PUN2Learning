using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.ScriptableObejct
{
    public class StateEvent : ScriptableObject
    {
        private List<StateEventListener> listeners = new List<StateEventListener>();

        public void Raise()
        {
            for(int i = listeners.Count -1; i >= 0; i--)
                listeners[i].OnEventRaised();
        }

        public void RegisterListener(StateEventListener listener) { listeners.Add(listener); }

        public void UnregisterListener(StateEventListener listener) { listeners.Remove(listener); }
    }
}

