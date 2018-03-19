using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Interaction;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Datenshi.Scripts.UI.Stealth {
    public class UIInteractableElementView : UIView {
        public Button Button;
        public Image Icon;
        public UICircle SelectionCircle;
        public CanvasGroup CanvasGroup;
        public float AppearenceDelay = 0.1F;

        private void Start() {
            if (Button != null) {
                Button.interactable = false;
            }
        }


        public void ResetColor() {
            SetColor(Color.white);
        }

        public void SetColor(Color color) {
            SelectionCircle.DOColor(color, AppearenceDelay);
            Icon.DOColor(color, AppearenceDelay);
        }

        protected override void SnapShow() {
            CanvasGroup.alpha = 1;
        }

        protected override void SnapHide() {
            CanvasGroup.alpha = 0;
        }

        protected override void OnShow() {
            CanvasGroup.DOFade(1, AppearenceDelay);
        }

        protected override void OnHide() {
            CanvasGroup.DOFade(0, AppearenceDelay);
        }

        public override void Select() {
            Button.Select();
        }

        public override void Deselect() {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}