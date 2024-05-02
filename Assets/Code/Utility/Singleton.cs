using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FormulaBoy.Utility
{
    /// <summary>
    /// A Singleton is a class that only has one instance in the game at any given time.
    /// This is a generic Singleton class that can be used to create singletons of any type.
    /// </summary>
    /// <typeparam name="T">The type of the Singleton</typeparam>
    public class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                return _instance;
            }
        }

        protected virtual bool IsPersistent => false;
        protected virtual bool SpawnOnGameStart => false;

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                if (IsPersistent)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (_instance != this)
            {
                
                Debug.LogWarning($"Singleton of type {typeof(T)} already exists in gameobject {_instance.name}. Destroying this instance.");
                // Destroy(gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            _instance = null;
            if (IsPersistent)
            {
                Destroy(gameObject);
            }
        }
    }

    public class PersistentSingleton<T> : Singleton<T>
        where T : MonoBehaviour
    {
        protected override bool IsPersistent => true;
    }
}
