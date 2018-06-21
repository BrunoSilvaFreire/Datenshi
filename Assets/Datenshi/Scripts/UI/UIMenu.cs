using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.UI.Misc;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI {
    public class UIMenu : UICanvasGroupView {
        [ShowInInspector, ReadOnly]
        private UIElement[] elements;

        public bool PauseOnOpen;
        public IEnumerable<UIElement> Elements => elements;
        public bool SelectFirstOnOpen = true;

        private void Awake() {
            elements = GetComponentsInChildren<UIElement>();
        }

        protected override void OnShow() {
            base.OnShow();
            if (SelectFirstOnOpen) {
                elements.FirstOrDefault()?.Select();
            }
            SetElementsActive(true);
            if (PauseOnOpen) {
                Time.timeScale = 0;
            }
        }

        private void SetElementsActive(bool b) {
            foreach (var s in GetComponentsInChildren<Selectable>()) {
                s.interactable = b;
            }
        }

        protected override void OnHide() {
            base.OnHide();
            SetElementsActive(false);
            UIElement.Deselect();
            if (PauseOnOpen) {
                Time.timeScale = 1;
            }
        }
    }
}