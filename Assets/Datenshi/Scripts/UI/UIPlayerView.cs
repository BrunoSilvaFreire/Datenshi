using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

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

        public UICircle Circle;
        public Text XPLabel;
        public Text LevelLabel;
        public Text CharacterNameLabel;
        public Image HealthBar;
        public bool Showing = true;
        public PlayerInputProvider Player;
        private Entity currentEntity;

        private void Update() {
            Showing = Player.GetPlanningMenu();
        }

        [ShowInInspector]
        public byte CurrentLevel {
            get {
                return currentLevel;
            }
            set {
                currentLevel = value;
                LevelLabel.text = string.Format("Level {0}", value);
                UpdateXP();
            }
        }

        private void UpdateXP() {
            var next = XPUtil.GetRequiredXPForLevel((byte) (currentLevel + 1));
            Circle.SetProgress((float) currentXP / next);
            XPLabel.text = string.Format("{0} / {1}", currentXP, next);
        }

        [ShowInInspector]
        public byte DefaultColor {
            get {
                return defaultColor;
            }
            set {
                defaultColor = value;
                if (!currentEntity.Character) {
                    UpdateColors();
                }
            }
        }

        [ShowInInspector]
        public uint CurrentXP {
            get {
                return currentXP;
            }
            set {
                currentXP = value;
                var next = XPUtil.GetRequiredXPForLevel((byte) (currentLevel + 1));
                if (currentXP >= next) {
                    currentXP -= next;
                    CurrentLevel++;
                    return;
                }

                if (currentLevel >= byte.MaxValue) {
                    return;
                }

                UpdateXP();
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
            LevelLabel.color = color;
            Circle.SetProgressColor(color);
            CharacterNameLabel.color = color;
            HealthBar.color = color;
        }
    }
}