using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.UI.Misc {
    public class UICanvasGroupView : UIView {
        public CanvasGroup Group;
        public float ShowAlpha = 1;
        public float HideAlpha = 0;
        public float GroupTransitionDuration = .5F;

        protected override void SnapShow() {
            Group.alpha = ShowAlpha;
        }

        protected override void SnapHide() {
            Group.alpha = HideAlpha;
        }

        protected override void OnShow() {
            SetGroupAlpha(ShowAlpha);
        }

        protected override void OnHide() {
            SetGroupAlpha(HideAlpha);
        }

        private void SetGroupAlpha(float alpha) {
            Group.DOKill();
            Group.DOFade(alpha, GroupTransitionDuration).SetUpdate(true);
        }
    }
}