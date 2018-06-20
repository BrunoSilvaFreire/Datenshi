using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Elements {
    public class UIButton : UIElement {
        public Button Button;
        public string ShowTrigger = "Show";
        public string HideTrigger = "Hide";

        private Animator Animator => Button.animator;

        protected override void SnapShow() {
            OnShow();
        }

        protected override void SnapHide() {
            OnHide();
        }

        protected override void OnShow() {
            Animator.SetTrigger(ShowTrigger);
        }

        protected override void OnHide() {
            Animator.SetTrigger(HideTrigger);
        }

        public override void Select() {
            Button.Select();
        }

        public override void Deselect() {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}