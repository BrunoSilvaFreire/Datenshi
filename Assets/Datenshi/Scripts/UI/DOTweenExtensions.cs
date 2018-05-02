using DG.Tweening;
using UnityEngine.UI.Extensions;

namespace Datenshi.Scripts.UI {
    public static class DOTweenExtensions {
        public static void DOProgress(this UICircle circle, float endValue, float duration) {
            DOTween.To(() => circle.Progress, value => circle.Progress = value, endValue, duration);
        }

        public static void DOPadding(this UICircle circle, int endValue, float duration) {
            DOTween.To(() => circle.Padding, circle.SetPadding, endValue, duration);
        }

        public static void DORadius(this RadialLayout layout, float radius, float duration) {
            DOTween.To(() => layout.Radius, value => layout.Radius = value, radius, duration);
        }
    }
}