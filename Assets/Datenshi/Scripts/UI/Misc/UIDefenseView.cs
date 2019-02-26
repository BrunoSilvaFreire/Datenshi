using Datenshi.Scripts.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public class UIDefenseView : UIMaxedCharacterBarView {
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
            return e == null ? 0 : e.StaminaPercentage;
        }

        protected override void UpdateBar(Image defenseBar, Entity entity) {
            base.UpdateBar(defenseBar, entity);
            var l = entity as LivingEntity;
            if (l == null) {
                return;
            }

            if (!ShowAlways) {
                Showing = l.Defending;
            }
        }
    }
}