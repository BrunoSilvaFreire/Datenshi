using UnityEngine;

namespace Datenshi.Scripts.UI {
    public abstract class UIMenu : UIElement {
        [SerializeField, HideInInspector]
        private UIView[] views;

        public UIView[] Views => views;

        private void Awake() {
            views = GetComponentsInChildren<UIView>();
        }
    }
}