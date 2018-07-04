using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial.Popup {
    public class PopUpTutorialContent : MonoBehaviour {
        public CanvasGroup group;
        public SpriteRenderer[] Renderers;
        public RectTransform RectTransform => transform as RectTransform;

        public Vector2 Size => RectTransform.sizeDelta;

        private void Update() {
            foreach (var spriteRenderer in Renderers) {
                var c = spriteRenderer.color;
                c.a = group.alpha;
                spriteRenderer.color = c;
            }
        }

        public IEnumerator Fade(float alpha, float duration) {
            group.DOKill();
            var main = group.DOFade(alpha, duration);
            while (main.IsActive() && main.IsPlaying()) {
                yield return null;
            }
        }

        public IEnumerator FadeOut(float duration) {
            Destroy(gameObject, duration);
            yield return Fade(0, duration);
        }

        public IEnumerator FadeIn(float duration) {
            yield return Fade(1, duration);
        }
    }
}