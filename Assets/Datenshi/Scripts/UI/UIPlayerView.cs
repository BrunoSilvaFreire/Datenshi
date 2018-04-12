using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI {
    public class UIPlayerView : UIDefaultColoredElement {
        public Text CharacterNameLabel;
        public PlayerController Player;

        private void Awake() {
            Player.OnEntityChanged.AddListener(OnChanged);
        }

        private void OnChanged(Entity arg0, Entity entity) {
            UpdateColors();
        }


        protected override bool HasColorAvailable() {
            var e = Player.CurrentEntity;
            var character = e != null ? e.Character : null;
            return character != null;
        }

        protected override Color GetAvailableColor() {
            return Player.CurrentEntity.Character.SignatureColor;
        }

        protected override void UpdateColors(Color color) {
            var e = Player.CurrentEntity;
            var character = e != null ? e.Character : null;
            CharacterNameLabel.text = character != null ? character.Alias : "No character selected :c";
            CharacterNameLabel.color = color;
        }
    }
}