﻿using System.Collections;
using Datenshi.Scripts.Entities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public abstract class UIHealthElement : UIDefaultColoredElement {
        public Image HealthBar;
        public Image SubHealthBar;
        public float SubHealthDelay = 1;
        public float SubHealthAnimationDuration = 1;

        protected virtual void Start() {
            SnapHide();
        }


        private Coroutine currentRoutine;

        private void OnDamaged() {
            UpdateBar();
        }

        protected abstract LivingEntity GetEntity();

        protected void UpdateBar() {
            var entity = GetEntity();
            UpdateBar(entity == null ? 1 : entity.HealthPercentage);
        }

        protected void UpdateBar(float f) {
            if (currentRoutine != null) {
                StopCoroutine(currentRoutine);
            }
            UpdateColors();
            currentRoutine = StartCoroutine(DamageEffect(f));
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


        protected override bool HasColorAvailable() {
            var e = GetEntity();
            var character = e != null ? e.Character : null;
            return character != null;
        }

        protected override Color GetAvailableColor() {
            return GetEntity().Character.SignatureColor;
        }

        protected override void UpdateColors(Color color) {
            HealthBar.color = color;
        }
    }
}