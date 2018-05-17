using Datenshi.Scripts.Util;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public class UICurtainsElement : UIElement {
        private static UICurtainsElement instance;

        public static UICurtainsElement Instance
            => instance == null ? (instance = FindObjectOfType<UICurtainsElement>()) : instance;

        public Image Curtain;
        public float FadeDuration = 2F;

        public void Reveal() {
            SnapHide();
            Show();
        }

        public void Conceal() {
            SnapShow();
            Hide();
        }

        protected override void SnapShow() {
            Curtain.SetAlpha(1);
        }

        protected override void SnapHide() {
            Curtain.SetAlpha(0);
        }

        protected override void OnShow() {
            Curtain.DOKill();
            Curtain.DOFade(1, FadeDuration);
        }

        protected override void OnHide() {
            Curtain.DOKill();
            Curtain.DOFade(0, FadeDuration);
        }
    }
}