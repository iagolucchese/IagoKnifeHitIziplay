using UnityEngine;

namespace IagoUnityScriptsRepo
{
    public abstract class ASingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                // Search for existing instance.
                _instance = (T) FindObjectOfType(typeof(T));

                if (_instance != null) return _instance;

                // Create new instance if one doesn't already exist.
                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject();
                _instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T) + " (Singleton)";

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}