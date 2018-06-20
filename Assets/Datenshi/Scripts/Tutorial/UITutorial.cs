using Datenshi.Scripts.UI;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial {
    public class UITutorial : UIView {
        public Vector2 Size;
        public float FadeDuration;
        public SpriteRenderer[] Renderers;
        public CanvasGroup CanvasGroup;

        protected override void SnapShow() {
            Set(1);
        }

        protected override void SnapHide() {
            Set(0);
        }

        protected override void OnShow() {
            DoLerp(1);
        }

        protected override void OnHide() {
            DoLerp(0);
        }

        private void DoLerp(float alpha) {
            CanvasGroup.DOFade(alpha, FadeDuration);
            foreach (var spriteRenderer in Renderers) {
                var color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.DOBlendableColor(color, FadeDuration);
            }
        }

        private void Set(float alpha) {
            CanvasGroup.alpha = alpha;
            foreach (var spriteRenderer in Renderers) {
                var color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
        }
    }
}