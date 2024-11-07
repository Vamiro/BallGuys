using System;
using UnityEngine;

namespace Game.Scripts.Utilities
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            if (_instance == null) return;
            Destroy(gameObject);
            return;
        }
    }
}