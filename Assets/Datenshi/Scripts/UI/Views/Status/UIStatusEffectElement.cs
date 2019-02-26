using Datenshi.Scripts.Combat.Status;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Buffs;
using UnityEngine;
using UnityEngine.UI;
using ColorUtility = Datenshi.Scripts.Util.ColorUtility;

namespace Datenshi.Scripts.UI.Views.Status {
    public class UIStatusEffectView : UIDefaultColoredView {
        
        public StatusEffect Effect;
        public PropertyModifier Modifier;
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

        public void Init(StatusEffect effect, PropertyModifier modifer) {
            Effect = effect;
            Modifier = modifer;
            float fillAmount = 1;
            var p = modifer as PeriodicPropertyModifier;
            if (p != null) {
                fillAmount = p.PercentCompleted;
            }

            Foreground.fillAmount = fillAmount;
            Label.text = effect.GetAlias();
            Magnitude.text = $"x{modifer.Multiplier}";
            UpdateColors();
        }

        private void Update() {
            var p = Modifier as PeriodicPropertyModifier;
            if (p == null) {
                return;
            }

            Foreground.fillAmount = p.PercentCompleted;
        }

        protected override void UpdateColors(Color color) {
            ColorUtility.SetSaturation(ref color, StatusEffect.ColorSaturation);
            Debug.Log($"Setting color to {color}");
            Background.color = color.SetBrightness(StatusEffect.ColorInactiveBrightness);
            Foreground.color = color.SetBrightness(StatusEffect.ColorActiveBrightness);
        }
    }
}