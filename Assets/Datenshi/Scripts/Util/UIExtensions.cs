using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Datenshi.Scripts.Util {
    public static class UIExtensions {
        public static void AddListener(this GameObject selectable, EventTriggerType type, UnityAction<BaseEventData> action) {
            var trigger = selectable.GetOrAddComponent<EventTrigger>();
            trigger.triggers.Add(CreateEntry(type, action));
        }

        public static void AddOnSelectListener(this Selectable selectable, UnityAction<BaseEventData> action) {
            selectable.gameObject.AddListener(EventTriggerType.Select, action);
        }

        public static void AddOnDeselectListener(this Selectable selectable, UnityAction<BaseEventData> action) {
            selectable.gameObject.AddListener(EventTriggerType.Deselect, action);
        }

        private static EventTrigger.Entry CreateEntry(EventTriggerType type, UnityAction<BaseEventData> action) {
            var cb = new EventTrigger.TriggerEvent();
            cb.AddListener(action);
            return new EventTrigger.Entry {
                callback = cb,
                eventID = type
            };
        }
    }
}