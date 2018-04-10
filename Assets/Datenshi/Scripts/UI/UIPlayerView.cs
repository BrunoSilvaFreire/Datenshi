using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.UI.Misc;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.UI {
    public class UIPlayerView : MonoBehaviour {
        [SerializeField, HideInInspector]
        private byte defaultColor;

        [SerializeField, HideInInspector]
        private byte currentLevel;

        [SerializeField, HideInInspector]
        private uint currentXP;

        [SerializeField, HideInInspector]
        private byte saturation;

        public Text CharacterNameLabel;
        public bool Showing = true;
        public PlayerController Player;
        public UIHealthElement HealthBar;

        private void Awake() {
            Player.OnEntityChanged.AddListener(OnChanged);
            OnChanged();
        }

        private void OnChanged() {
            HealthBar.Entity = Player.CurrentEntity as LivingEntity;
            UpdateColors();
        }

        [ShowInInspector]
        public byte DefaultColor {
            get {
                return defaultColor;
            }
            set {
                defaultColor = value;
                var currentEntity = Player.CurrentEntity;
                if (!currentEntity.Character) {
                    UpdateColors();
                }
            }
        }

        [ShowInInspector]
        public byte Saturation {
            get {
                return saturation;
            }
            set {
                saturation = value;
                UpdateColors();
            }
        }

        private void UpdateColors() {
            float hue;
            var currentEntity = Player.CurrentEntity;
            var character = currentEntity.Character;
            if (character) {
                float s, v;
                Color.RGBToHSV(character.SignatureColor, out hue, out s, out v);
            } else {
                hue = (float) defaultColor / byte.MaxValue;
            }

            UpdateColors(hue, character);
        }

        private void UpdateColors(float hue, Character.Character character) {
            CharacterNameLabel.text = character ? character.Alias : "No character selected :c";
            var color = Color.HSVToRGB(hue, (float) saturation / byte.MaxValue, 1, true);
            CharacterNameLabel.color = color;
            HealthBar.HealthBar.color = color;
        }
    }
}