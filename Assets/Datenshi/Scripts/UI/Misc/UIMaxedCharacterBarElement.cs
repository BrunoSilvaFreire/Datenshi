using Datenshi.Scripts.Entities;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public abstract class UIMaxedCharacterBarElement : UICharacterBarElement {
        protected override void UpdateBar(Image defenseBar, Entity entity) {
            var percent = defenseBar.fillAmount;
            if (percent <= 1) {
                if (!Showing) {
                    Show();
                }
            } else {
                if (percent >= 1) {
                    Hide();
                }
            }
        }

    }
}