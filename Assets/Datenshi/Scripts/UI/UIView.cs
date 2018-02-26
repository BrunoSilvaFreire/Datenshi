using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    public abstract class UIElement : MonoBehaviour {
        [ShowInInspector]
        public bool Showing {
            get {
                return showing;
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
                if (value) {
                    Show();
                } else {
                    Hide();
                }
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

    public abstract class UIView : UIElement {
        public abstract void Select();
    }
}