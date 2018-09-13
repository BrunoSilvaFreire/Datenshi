#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Datenshi.Scripts.Util.Singleton {
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject {
        private static readonly string Path = typeof(T).Name;
        private static T instance;

        public static T Instance => instance == null ? (instance = Load()) : instance;

        private static T Load() {
#if UNITY_EDITOR
            var resourcePath = $"Assets/Datenshi/Resources/{Path}.asset";
            if (!AssetDatabase.LoadAssetAtPath<T>(resourcePath)) {
                Debug.LogFormat("Creating new singleton @ {0}", resourcePath);
                var asset = CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, resourcePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return asset;
            }

#endif
            return Resources.Load<T>(Path);
        }
    }
}