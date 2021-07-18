using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Demo.ScriptableObejct
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;
        public UnityEvent<string> ResponseWithString;
        public UnityEvent<int> ResponseWithInt;

        private void OnEnable()
        { Event.RegisterListener(this); }

        private void OnDisable()
        { Event.UnregisterListener(this); }

        public void OnEventRaised()
        { Response.Invoke(); }

        public void OnEventRaised(string data)
        {
            ResponseWithString.Invoke(data);
        }

        public void OnEventRaised(int data)
        {
            ResponseWithInt.Invoke(data);
        }
    }
}

