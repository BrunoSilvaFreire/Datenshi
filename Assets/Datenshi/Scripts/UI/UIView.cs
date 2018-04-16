using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Datenshi.Scripts.UI {
    public abstract class UIElement : MonoBehaviour {
        [ShowInInspector]
        public bool Showing {
            get {
                return overrideShowing ? overrideShowingValue : showing;
            }
            set {
                if (value == showing) {
                    return;
                }
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    SnapShowing(value);
                    return;
                }
#endif
                showing = value;
                if (!overrideShowing) {
                    UpdateState();
                }
            }
        }

        private bool overrideShowing;
        private bool overrideShowingValue;

        public void Override(bool value) {
            if (!overrideShowing) {
                overrideShowing = true;
            }

            overrideShowingValue = value;
            if (overrideShowingValue != showing) {
                UpdateState();
            }
        }

        public void ReleaseOverride() {
            overrideShowing = false;
            if (overrideShowingValue == Showing) {
                return;
            }

            UpdateState();
        }

        private void UpdateState() {
            if (Showing) {
                Show();
            } else {
                Hide();
            }
        }

        public void SnapShowing(bool show) {
            showing = show;
            if (show) {
                SnapShow();
            } else {
                SnapHide();
            }
        }

        private bool showing;

        public void Show() {
            showing = true;
            if (!overrideShowing) {
                OnShow();
            }
        }

        public void Hide() {
            showing = false;
            if (!overrideShowing) {
                OnHide();
            }
        }

        protected abstract void SnapShow();
        protected abstract void SnapHide();
        protected abstract void OnShow();
        protected abstract void OnHide();
    }

    public abstract class UIView : UIElement {
        public abstract void Select();
        public abstract void Deselect();
    }
}