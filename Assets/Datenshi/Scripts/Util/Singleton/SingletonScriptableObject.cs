#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Datenshi.Scripts.Util.Singleton {
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject {
        private static readonly string Path = "Datenshi/" + typeof(T).Name;
        private static T instance;

        public static T Instance {
            get {
                return instance ?? (instance = Load());
            }
        }

        private static T Load() {
#if UNITY_EDITOR
            var resourcePath = string.Format("Assets/Resources/{0}.asset", Path);
            if (!AssetDatabase.LoadAssetAtPath<T>(resourcePath)) {
                Debug.LogFormat("Creating new singleton @ {0}", resourcePath);
                var asset = CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, resourcePath);
                return asset;
            }

#endif
            return Resources.Load<T>(Path);
        }
    }
}