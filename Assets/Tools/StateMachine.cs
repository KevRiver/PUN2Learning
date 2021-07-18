using System;
using System.Collections.Generic;
using Demo.ScriptableObejct;

namespace Tools
{
    public class StateMachine<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        public T currentState;
        public T previousState;
        
        private List<StateEvent> _events = new List<StateEvent>();

        public void ChangeState(T newState)
        {
            if (newState.Equals(currentState))
                return;

            previousState = currentState;
            currentState = newState;
        }

        public void ChangeState(T newState, bool triggerEvent)
        {
            if (newState.Equals(currentState))
                return;

            previousState = currentState;
            currentState = newState;
            if (triggerEvent)
            {
                if (_events.Count > 0)
                {
                    for (int i = 0; i < _events.Count; i++)
                    {
                        _events[i].Raise();
                    }
                }
            }
        }
        
        public void AddStateEvent(StateEvent e)
        {
            _events.Add(e);
        }
        
        public void RemoveStateEvent(StateEvent e)
        {
            int whichToRemove = _events.FindIndex(x => x == e);
            if (whichToRemove > -1)
            {
                _events.RemoveAt(whichToRemove);
            }
        }
        
        public void ClearStateEvents()
        {
            _events.Clear();
        }
        
    }
}