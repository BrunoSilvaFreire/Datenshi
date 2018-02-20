using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class BehaviourUtil {
        public static T Clone<T>(this T obj) where T : Object {
            return Object.Instantiate(obj);
        }
    }
}