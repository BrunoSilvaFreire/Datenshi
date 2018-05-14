using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Util {
    public static class DOTweenExtensions {
        public static void DOFrequency(this AudioLowPassFilter filter, float frequency, float duration) {
            DOTween.To(
                () => filter.cutoffFrequency,
                value => filter.cutoffFrequency = value,
                frequency,
                duration);
        }

        public static void DOFrequency(this AudioHighPassFilter filter, float frequency, float duration) {
            DOTween.To(
                () => filter.cutoffFrequency,
                value => filter.cutoffFrequency = value,
                frequency,
                duration);
        }

        public static Tween DOFontSize(this Text text, int newSize, float duration) {
            return DOTween.To(() => text.fontSize, value => text.fontSize = value, newSize, duration);
        }
    }
}