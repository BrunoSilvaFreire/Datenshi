using DG.Tweening;
using UnityEngine;

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
    }
}