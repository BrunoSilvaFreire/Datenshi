using System.Collections;
using Datenshi.Scripts.UI.Misc;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    public static class UIElementExtensions {
        public static void FadeIn(this UIElement element) {
            element.SnapShowing(false);
            element.Showing = true;
        }

        public static void FadeOut(this UIElement element) {
            element.SnapShowing(true);
            element.Showing = false;
        }

        public static void FadeDelete(this UICanvasGroupElement element) {
            element.FadeOut();
            Object.Destroy(element.gameObject, element.GroupTransitionDuration);
        }
    }
}