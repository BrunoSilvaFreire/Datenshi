using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.UI.Misc {
    public class UIDefenseElement : UIMaxedCharacterBarElement {
        public bool UseCustomColor;
        public Color CustomColor;
        protected override bool HasColorAvailable() {
            return UseCustomColor || base.HasColorAvailable();
        }

        protected override Color GetAvailableColor() {
            return UseCustomColor ? CustomColor : base.GetAvailableColor();
        }

        protected override float GetPercentage(Entity entity) {
            var e = entity as LivingEntity;
            return e == null ? 0 : e.DefendTimePercent;
        }
    }
}