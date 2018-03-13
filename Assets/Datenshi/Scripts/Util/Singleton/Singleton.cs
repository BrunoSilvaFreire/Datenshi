using UnityEngine;

namespace Datenshi.Scripts.Util.Singleton {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
        private static T instance;

        public static T Instance {
            get {
                return instance ?? (instance = Load());
            }
        }

        private static T Load() {
            return FindObjectOfType<T>();
        }
    }
}