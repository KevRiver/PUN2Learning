using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Demo.ScriptableObejct
{
    public class NetworkEventListener : MonoBehaviour
    {
        public NetworkEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnNetworkEventRaise()
        {
            Response.Invoke();
        }
    }
}

