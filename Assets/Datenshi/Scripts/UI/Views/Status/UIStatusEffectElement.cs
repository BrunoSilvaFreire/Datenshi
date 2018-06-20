using Datenshi.Scripts.Combat.Status;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Volatiles;
using UnityEngine;
using UnityEngine.UI;
using ColorUtility = Datenshi.Scripts.Util.ColorUtility;

namespace Datenshi.Scripts.UI.Views.Status {
    public class UIStatusEffectView : UIDefaultColoredView {
        public StatusEffect Effect;
        public VolatilePropertyModifier Modifier;
        public Image Background;
        public Image Foreground;
        public Text Label;
        public Text Magnitude;

        protected override bool HasColorAvailable() {
            return Effect != null;
        }

        protected override Color GetAvailableColor() {
            return Effect.GetColor();
        }

        public void Init(StatusEffect effect, VolatilePropertyModifier modifer) {
            Effect = effect;
            Modifier = modifer;
            float fillAmount = 1;
            if (Modifier != null && !Modifier.IsInfinite) {
                fillAmount = Modifier.TimePercentage;
            }

            Foreground.fillAmount = fillAmount;
            Label.text = effect.GetAlias();
            Magnitude.text = $"x{modifer.Multiplier}"; 
            UpdateColors();
        }

        private void Update() {
            if (Modifier == null || Modifier.IsInfinite) {
                return;
            }

            Foreground.fillAmount = Modifier.TimePercentage;
        }

        protected override void UpdateColors(Color color) {
            ColorUtility.SetSaturation(ref color, StatusEffect.ColorSaturation);
            Debug.Log($"Setting color to {color}");
            Background.color = color.SetBrightness(StatusEffect.ColorInactiveBrightness);
            Foreground.color = color.SetBrightness(StatusEffect.ColorActiveBrightness);
        }
    }
}