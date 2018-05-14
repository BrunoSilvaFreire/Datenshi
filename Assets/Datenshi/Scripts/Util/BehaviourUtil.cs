using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Datenshi.Scripts.Util {
    public static class BehaviourUtil {
        public static T Clone<T>(this T obj) where T : Object => Object.Instantiate(obj);

        public static T Clone<T>(this T obj, Vector3 transform) where T : Object {
            return Clone(obj, transform, Quaternion.identity);
        }

        public static T Clone<T>(this T obj, Vector3 transform, Quaternion rotation) where T : Object {
            return Object.Instantiate(obj, transform, rotation);
        }

        public static T Clone<T>(this T obj, Transform transform) where T : Object {
            return Object.Instantiate(obj, transform);
        }

        public static T GetOrAddComponent<T>(this Component obj) where T : Component {
            return obj.gameObject.GetOrAddComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component {
            var f = obj.GetComponent<T>();
            return f ? f : obj.AddComponent<T>();
        }

        public static TweenerCore<Vector2, Vector2, VectorOptions> DOPosition(
            this Transform transform,
            Vector2 target,
            float duration) {
            return DOTween.To(() => (Vector2) transform.position, v => transform.position = v, target, duration);
        }
    }
}