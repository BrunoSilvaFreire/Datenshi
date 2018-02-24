﻿using DG.Tweening;
using UnityEngine.UI.Extensions;

namespace Datenshi.Scripts.Util {
    public static class UIExtensions {
        public static void DOProgress(this UICircle circle, float endValue, float duration) {
            DOTween.To(() => circle.Progress, value => circle.Progress = value, endValue, duration);
        }

        public static void DORadius(this RadialLayout layout, float radius, float duration) {
            DOTween.To(() => layout.fDistance, value => layout.fDistance = value, radius, duration);
        }
    }
}