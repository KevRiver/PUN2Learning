using UnityEngine;
using System.Collections.Generic;

namespace Demo.ScriptableObejct
{
    [CreateAssetMenu]
    public class NetworkEvent : ScriptableObject
    {
       private List<NetworkEventListener> _listeners = new List<NetworkEventListener>();

       public void Raise()
       {
           for (int i = 0; i < _listeners.Count; i++)
           {
               _listeners[i].OnNetworkEventRaise();
           }
       }

       public void RegisterListener(NetworkEventListener listener)
       {
           _listeners.Add(listener);
       }

       public void UnregisterListener(NetworkEventListener listener)
       {
           _listeners.Remove(listener);
       }
    }
}
