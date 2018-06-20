using Datenshi.Scripts.Entities;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public abstract class UIMaxedCharacterBarView : UICharacterBarView {
        public float ShowPercentage = 0.975F;
        public bool ShowAlways;

        protected virtual bool CancelBarUpdate() {
            return false;
        }
        protected override void UpdateBar(Image defenseBar, Entity entity) {
            if (ShowAlways || CancelBarUpdate()) {
                return;
            }

            var percent = defenseBar.fillAmount;
            if (percent <= ShowPercentage) {
                if (!Showing) {
                    Show();
                }
            } else {
                if (percent >= ShowPercentage) {
                    Hide();
                }
            }
        }
    }
}