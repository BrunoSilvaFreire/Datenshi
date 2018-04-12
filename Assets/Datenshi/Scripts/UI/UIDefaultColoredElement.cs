using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    public abstract class UIDefaultColoredElement : UIElement {
        [SerializeField, HideInInspector]
        private byte defaultColor;

        [SerializeField, HideInInspector]
        private byte currentLevel;

        [SerializeField, HideInInspector]
        private uint currentXP;

        [SerializeField, HideInInspector]
        private byte saturation;

        public CanvasGroup CanvasGroup;
        public float CanvasGroupShowAmount = 1;
        public float CanvasGroupHideAmount = 0;
        public float CanvasGroupFadeDuration = 1;

        [ShowInInspector]
        public byte DefaultColor {
            get {
                return defaultColor;
            }
            set {
                defaultColor = value;
                UpdateColors();
            }
        }

        protected abstract bool HasColorAvailable();
        protected abstract Color GetAvailableColor();
        protected abstract void UpdateColors(Color color);

        [ShowInInspector]
        public byte Saturation {
            get {
                return saturation;
            }
            set {
                saturation = value;
                UpdateColors();
            }
        }

        protected void UpdateColors() {
            float hue;
            if (HasColorAvailable()) {
                float s, v;
                Color.RGBToHSV(GetAvailableColor(), out hue, out s, out v);
            } else {
                hue = (float) defaultColor / byte.MaxValue;
            }

            UpdateColors(Color.HSVToRGB(hue, (float) saturation / byte.MaxValue, 1, true));
        }

        protected override void SnapShow() {
            CanvasGroup.DOKill();
            CanvasGroup.alpha = CanvasGroupShowAmount;
        }

        protected override void SnapHide() {
            CanvasGroup.DOKill();
            CanvasGroup.alpha = CanvasGroupHideAmount;
        }

        protected override void OnShow() {
            CanvasGroup.DOKill();
            CanvasGroup.DOFade(CanvasGroupShowAmount, CanvasGroupFadeDuration);
        }

        protected override void OnHide() {
            CanvasGroup.DOKill();
            CanvasGroup.DOFade(CanvasGroupHideAmount, CanvasGroupFadeDuration);
        }
    }
}