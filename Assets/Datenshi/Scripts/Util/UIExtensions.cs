using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Datenshi.Scripts.Util {
    public static class UIExtensions {
        public static void DOProgress(this UICircle circle, float endValue, float duration) {
            DOTween.To(() => circle.Progress, value => circle.Progress = value, endValue, duration);
        }

        public static void DOPadding(this UICircle circle, int endValue, float duration) {
            DOTween.To(() => circle.Padding, circle.SetPadding, endValue, duration);
        }

        public static void DORadius(this RadialLayout layout, float radius, float duration) {
            DOTween.To(() => layout.Radius, value => layout.Radius = value, radius, duration);
        }

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