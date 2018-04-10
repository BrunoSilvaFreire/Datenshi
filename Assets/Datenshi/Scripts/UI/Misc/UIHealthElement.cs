using System.Collections;
using Datenshi.Scripts.Entities;
using DG.Tweening;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
        private LivingEntity entity;

        [ShowInInspector]
        public LivingEntity Entity {
            get {
                return entity;
            }
            set {
                if (entity != null
#if UNITY_EDITOR
                    && EditorApplication.isPlaying
#endif
                ) {
                    entity.OnDamaged.AddListener(OnDamaged);
                }

                entity = value;
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying) {
                    return;
                }
#endif
                SetValue();
                if (entity != null) {
                    entity.OnDamaged.AddListener(OnDamaged);
                }
            }
        }

        private void SetValue() {
            SetValue(entity == null ? 1 : entity.HealthPercentage);
        }

        private void SetValue(float f) {
            if (currentRoutine != null) {
                StopCoroutine(currentRoutine);
            }

            currentRoutine = StartCoroutine(DamageEffect(f));
        }

        private void Start() {
            if (entity != null) {
                entity.OnDamaged.AddListener(OnDamaged);
            }

            SnapHide();
        }

        private Coroutine currentRoutine;

        private void OnDamaged(LivingEntity arg0, uint arg1) {
            SetValue();
        }

        private IEnumerator DamageEffect() {
            return DamageEffect(Entity.HealthPercentage);
        }

        private IEnumerator DamageEffect(float amount) {
            SnapShow();
            SubHealthBar.DOComplete();
            CanvasGroup.DOComplete();
            var currentFill = HealthBar.fillAmount;
            if (amount < currentFill) {
                HealthBar.fillAmount = amount;
                yield return new WaitForSeconds(SubHealthDelay);
                SubHealthBar.DOFillAmount(amount, SubHealthAnimationDuration);
                yield return new WaitForSeconds(SubHealthAnimationDuration);
            } else {
                SubHealthBar.fillAmount = amount;
                yield return new WaitForSeconds(SubHealthDelay);
                HealthBar.DOFillAmount(amount, SubHealthAnimationDuration);
                yield return new WaitForSeconds(SubHealthAnimationDuration);
            }

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