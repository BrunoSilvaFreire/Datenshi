using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Util {
    public class CollisionListener : MonoBehaviour {
        public event UnityAction Collide;
        public event UnityAction Trigger;

        protected virtual void OnCollide() {
            var handler = Collide;
            handler?.Invoke();
        }

        protected virtual void OnTrigger() {
            var handler = Trigger;
            handler?.Invoke();
        }
    }

    public static class ColliderUtil {
        public static void AddTriggerListener(this Collider2D collider2D, UnityAction action) {
            collider2D.GetOrAddComponent<CollisionListener>().Trigger += action;
        }

        public static void RemoveTriggerListener(this Collider2D collider2D, UnityAction action) {
            collider2D.GetOrAddComponent<CollisionListener>().Trigger -= action;
        }
    }
}