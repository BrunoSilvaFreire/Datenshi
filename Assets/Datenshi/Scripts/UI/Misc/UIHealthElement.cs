using System.Collections;
using Datenshi.Scripts.Entities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public class UIHealthElement : UIElement {
        public Image HealthBar;
        public Image SubHealthBar;
        public CanvasGroup CanvasGroup;
        public float FadeDuration = 3;
        public float SubHealthDelay = 1;
        public float SubHealthAnimationDuration = 1;
        public LivingEntity Entity;

        private void Start() {
            Entity.OnDamaged.AddListener(OnDamaged);
            SnapHide();
        }

        private Coroutine currentRoutine;

        private void OnDamaged(LivingEntity arg0, uint arg1) {
            if (currentRoutine != null) {
                StopCoroutine(currentRoutine);
            }

            currentRoutine = StartCoroutine(DamageEffect());
        }

        private IEnumerator DamageEffect() {
            SnapShow();
            SubHealthBar.DOComplete();
            CanvasGroup.DOComplete();
            var percent = Entity.HealthPercentage;
            HealthBar.fillAmount = percent;
            yield return new WaitForSeconds(SubHealthDelay);
            SubHealthBar.DOFillAmount(percent, SubHealthAnimationDuration);
            yield return new WaitForSeconds(SubHealthAnimationDuration);
            Hide();
            currentRoutine = null;
        }

        protected override void SnapShow() {
            CanvasGroup.DOKill();
            CanvasGroup.alpha = 1;
        }

        protected override void SnapHide() {
            CanvasGroup.DOKill();
            CanvasGroup.alpha = 0;
        }

        protected override void OnShow() {
            CanvasGroup.DOKill();
            CanvasGroup.DOFade(1, FadeDuration);
        }

        protected override void OnHide() {
            CanvasGroup.DOKill();
            CanvasGroup.DOFade(0, FadeDuration);
        }
    }
}