using DG.Tweening;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public class UIBlackBarView : UIView {
        private static UIBlackBarView instance;
        public Image Up;
        public Image Down;
        public float ObscurePercent = 0.15F;
        public float ShowDelay = 0.5F;
        public float DialogueObscurePercent = 0.3F;

        public static UIBlackBarView Instance => instance;

        private void OnEnable() {
            instance = this;
        }

        private void OnDisable() {
            instance = null;
        }

        [SerializeField]
        private bool showDialogue;

        public bool ShowDialogue {
            get {
                return showDialogue;
            }
            set {
                showDialogue = value;
                UpdateState();
            }
        }
#if UNITY_EDITOR
        private void OnValidate() {
            if (EditorApplication.isPlaying) {
                return;
            }

            if (Showing) {
                SnapShow();
            } else {
                SnapHide();
            }
        }
#endif
        protected override void SnapShow() {
            SetMin(1 - ObscurePercent);
            SetMax(ShowDialogue ? DialogueObscurePercent : ObscurePercent);
        }

        private void SetMax(float f) {
            var a = Down.rectTransform;
            var min = a.anchorMax;
            min.y = f;
            a.anchorMax = min;
        }

        private void SetMin(float f) {
            var a = Up.rectTransform;
            var min = a.anchorMin;
            min.y = f;
            a.anchorMin = min;
        }

        protected override void SnapHide() {
            SetMin(1);
            SetMax(0);
        }

        protected override void OnShow() {
            DOSetMin(1 - ObscurePercent);
            DOSetMax(ShowDialogue ? DialogueObscurePercent : ObscurePercent);
        }

        private void DOSetMax(float obscurePercent) {
            Down.rectTransform.DOKill();
            var t = Down.rectTransform;
            var min = t.anchorMax;
            min.y = obscurePercent;
            t.DOAnchorMax(min, ShowDelay);
        }

        private void DOSetMin(float f) {
            Up.rectTransform.DOKill();
            var t = Up.rectTransform;
            var min = t.anchorMin;
            min.y = f;
            t.DOAnchorMin(min, ShowDelay);
        }

        protected override void OnHide() {
            DOSetMin(1);
            DOSetMax(0);
        }
    }
}