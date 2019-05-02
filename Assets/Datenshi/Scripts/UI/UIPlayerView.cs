using Datenshi.Scripts.Entities;
using UnityEngine;
using UnityEngine.UI;
using Datenshi.Scripts.Game;
namespace Datenshi.Scripts.UI {
    public class UIPlayerView : UIDefaultColoredView {
        public Text CharacterNameLabel;

        private void Awake() {
            PlayerController.Instance.OnEntityChanged.AddListener(OnChanged);
        }

        private void OnChanged(Entity arg1, Entity entity1) {
            UpdateColors();
        }


        protected override bool HasColorAvailable() {
            var e = PlayerController.Instance.CurrentEntity;
            var character = e != null ? e.Character : null;
            return character != null;
        }

        protected override Color GetAvailableColor() {
            return PlayerController.Instance.CurrentEntity.Character.SignatureColor;
        }

        protected override void UpdateColors(Color color) {
            var e = PlayerController.Instance.CurrentEntity;
            var character = e != null ? e.Character : null;
            CharacterNameLabel.text = character != null ? character.Alias : "No character selected :c";
            CharacterNameLabel.color = color;
        }
    }
}