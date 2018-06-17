using Datenshi.Scripts.UI.Misc;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    public abstract class UIDefaultColoredElement : UICanvasGroupElement {
        [SerializeField, HideInInspector]
        private byte defaultColor;

        [SerializeField, HideInInspector]
        private byte currentLevel;

        [SerializeField, HideInInspector]
        private uint currentXP;

        [SerializeField, HideInInspector]
        private byte saturation;


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

    }
}