using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class BehaviourUtil {
        public static T Clone<T>(this T obj) where T : Object {
            return Object.Instantiate(obj);
        }

        public static T Clone<T>(this T obj, Transform transform) where T : Object {
            return Object.Instantiate(obj, transform);
        }

        public static T GetOrAddComponent<T>(this Component obj) where T : Component {
            return obj.gameObject.GetOrAddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component {
            var f = obj.GetComponent<T>();
            return f ?? (f = obj.AddComponent<T>());
        }
    }
}