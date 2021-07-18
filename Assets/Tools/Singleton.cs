using UnityEngine;

namespace Tools
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                _instance = FindObjectOfType<T>();
                if (!_instance)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    return _instance;
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
                return;
            _instance = this as T;
        }
    }
}
