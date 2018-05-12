using Datenshi.Scripts.UI;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Cutscenes.Dialogue.UI {
    public class UIDialogueBox : UIElement {
        public CanvasGroup CanvasGroup;
        public float FadeDuration;
        public Scripts.UI.Narrator.Narrator Narrator;

        public void Type(string text) {
            Narrator.TypeText(text);
        }

        protected override void SnapShow() {
            CanvasGroup.alpha = 1;
        }

        protected override void SnapHide() {
            CanvasGroup.alpha = 0;
        }

        protected override void OnShow() {
            CanvasGroup.DOFade(1, FadeDuration);
        }

        protected override void OnHide() {
            CanvasGroup.DOFade(0, FadeDuration);
        }
    }
}