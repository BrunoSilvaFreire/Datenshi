using System.Collections;
using Datenshi.Scripts.UI.Misc;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    public static class UIElementExtensions {
        public static void FadeIn(this UIView view) {
            view.SnapShowing(false);
            view.Showing = true;
        }

        public static void FadeOut(this UIView view) {
            view.SnapShowing(true);
            view.Showing = false;
        }

        public static void FadeDelete(this UICanvasGroupView view) {
            view.FadeOut();
            Object.Destroy(view.gameObject, view.GroupTransitionDuration);
        }
    }
}