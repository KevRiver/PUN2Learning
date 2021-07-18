using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Demo.ScriptableObejct
{
    public class StateEventListener : MonoBehaviour
    {
        public StateEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        { Event.RegisterListener(this); }

        private void OnDisable()
        { Event.UnregisterListener(this); }

        public void OnEventRaised()
        { Response.Invoke(); }
    }
}

