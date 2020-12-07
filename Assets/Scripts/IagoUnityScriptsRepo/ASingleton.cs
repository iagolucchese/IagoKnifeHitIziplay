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