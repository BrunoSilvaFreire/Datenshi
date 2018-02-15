using UnityEngine;

namespace Datenshi.Scripts.UI {
    public abstract class UIElement : MonoBehaviour {
        public bool Showing {
            get {
                throw new System.NotImplementedException();
            }
            set {
                if (value == showing) {
                    return;
                }
                if (value) {
                    OnShow();
                } else {
                    OnHide();
                }
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

        protected abstract void OnShow();
        protected abstract void OnHide();
    }

    public abstract class UIView : UIElement {
        public abstract void Select();
    }
}