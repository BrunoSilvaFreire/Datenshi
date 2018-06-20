using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public abstract class UICharacterBarView : UICharacterColoredView {
        public Image DefenseBar;

        private void Update() {
            var entity = PlayerController.Instance.CurrentEntity;
            if (entity == null) {
                return;
            }

            DefenseBar.fillAmount = GetPercentage(entity);
            UpdateBar(DefenseBar, entity);
        }

        protected abstract void UpdateBar(Image defenseBar, Entity entity);

        protected abstract float GetPercentage(Entity entity);

        protected override void UpdateColors(Color color) {
            DefenseBar.color = color;
        }
    }
}