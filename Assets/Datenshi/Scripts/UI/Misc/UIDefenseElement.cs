using Datenshi.Scripts.Entities;

namespace Datenshi.Scripts.UI.Misc {
    public class UIDefenseElement : UIMaxedCharacterBarElement {
/*        protected override void UpdateBar(Image defenseBar, Entity e) {
            var entity = e as LivingEntity;
            if (entity == null) {
                return;
            }

            if (entity.Defending) {
                if (!Showing) {
                    Show();
                }
            } else {
                if (entity.FocusTimeLeft >= entity.FocusMaxTime && Showing) {
                    Hide();
                }
            }
        }*/

        protected override float GetPercentage(Entity entity) {
            var e = entity as LivingEntity;
            return e == null ? 0 : e.FocusTimePercent;
        }
    }
}