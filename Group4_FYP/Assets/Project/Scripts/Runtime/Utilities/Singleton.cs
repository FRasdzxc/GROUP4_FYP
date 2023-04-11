using UnityEngine;

namespace PathOfHero.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Debug.Log($"[Singleton] {typeof(T)} initialized");
                Instance = this as T;
            }
            else
                Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Debug.Log($"[Singleton] {typeof(T)} released");
                Instance = null;
            }
        }
    }

    public class SingletonPersistent<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Debug.Log($"[Singleton] {typeof(T)} initialized");
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Debug.Log($"[Singleton] {typeof(T)} released");
                Instance = null;
            }
        }
    }

}