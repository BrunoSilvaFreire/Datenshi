using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI.Misc {
    public class UIDefenseElement : UIDefaultColoredElement {
        public PlayerController PlayerController;
        public Image DefenseBar;

        private void Update() {
            var entity = PlayerController.CurrentEntity as LivingEntity;
            if (entity == null) {
                return;
            }
            DefenseBar.fillAmount = entity.FocusTimePercent;
            if (entity.Defending) {
                if (!Showing) {
                    Show();
                }
            } else {
                if (entity.FocusTimeLeft >= entity.FocusMaxTime && Showing) {
                    Hide();
                }
            }
        }

        protected override bool HasColorAvailable() {
            var e = PlayerController.CurrentEntity as LivingEntity;
            var character = e != null ? e.Character : null;
            return character != null;
        }

        protected override Color GetAvailableColor() {
            return PlayerController.CurrentEntity.Character.SignatureColor;
        }

        protected override void UpdateColors(Color color) {
            DefenseBar.color = color;
        }
    }
}