using UnityEngine;

namespace IagoUnityScriptsRepo
{
    public abstract class ASingletonDestroyedOnLoad<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        /// <summary>
        /// Access singleton instance through this property.
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
        }
    }
}