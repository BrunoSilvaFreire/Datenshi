using Datenshi.Scripts.Util;
using DG.Tweening;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public class UICurtainsView : UIView {
        private static UICurtainsView instance;

        public static UICurtainsView Instance
            => instance == null ? (instance = FindObjectOfType<UICurtainsView>()) : instance;

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