using System.Linq;
using System.Reflection;
using UnityEngine;

namespace IagoUnityScriptsRepo
{
    public class AScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        public const string DefaultFilename = "DefaultFilename";

        public static T GetInstance()
        {
            var instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

            if (instance != null) return instance;

            //if no instance was found, tries to load a default file
            Debug.Log(DefaultFilename);
            instance = Resources.Load<T>(DefaultFilename);

            if (instance != null) return instance;

            //if still no instance was found, makes a new one
            if (instance == null)
            {
                instance = CreateInstance<T>();
                Debug.LogWarning("Instance of " + typeof(T) + " had to be created manually.");
                /*AssetDatabase.CreateAsset(instance, "Assets/Resources/" + DefaultFilename);
                AssetDatabase.SaveAssets();*/
            }

            return instance;
        }
    }
}
