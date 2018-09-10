using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

namespace Datenshi.Scripts.UI {
    public abstract class UIView : MonoBehaviour {
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

        public bool AllowOverride = true;

        [ShowInInspector]
        private bool overrideShowing;

        [ShowInInspector]
        private bool overrideShowingValue;

        private void Start() {
            UpdateState();
        }

        public void Override(bool value) {
            if (!AllowOverride) {
                return;
            }

            if (!overrideShowing) {
                overrideShowing = true;
            }

            overrideShowingValue = value;
            UpdateState();
        }

        public void ReleaseOverride() {
            overrideShowing = false;
            UpdateState();
        }

        protected void UpdateState() {
            if (Showing) {
                OnShow();
            } else {
                OnHide();
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

        [SerializeField, HideInInspector]
        private bool showing;

        public void Show() {
            showing = true;
            OnShow();
        }

        public void Hide() {
            showing = false;
            OnHide();
        }

        protected abstract void SnapShow();
        protected abstract void SnapHide();
        protected abstract void OnShow();
        protected abstract void OnHide();
    }

    public abstract class UIElement : UIView {
        public abstract void Select();

        public static void Deselect() {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}