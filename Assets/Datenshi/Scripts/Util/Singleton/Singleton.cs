using UnityEngine;

namespace Datenshi.Scripts.Util.Singleton {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
        private static T instance;

        public static T Instance => instance != null ? instance : (instance = FindObjectOfType<T>());
        public static T SilentInstance => instance;

        private void OnEnable() {
            if (instance != null) {
                Destroy(gameObject);
                return;
            }

            instance = (T) this;
        }

        private void OnDisable() {
            if (instance == this) {
                instance = null;
            }
        }
    }
}