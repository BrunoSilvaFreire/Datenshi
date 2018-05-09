using Datenshi.Scripts.UI.Misc;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.UI.Input {
    public class SpriteInputIcon : InputIcon {
        public Sprite Sprite;

        public float Rotation;
        public float InvertedRotationAmount = 90;
        public float InvertedRotationMultiplier = 2;

        public override void Setup(UIInputDisplay display, bool inverted) {
            display.Sprite = Sprite;
            var image = display.Image;
            var z = Rotation;
            if (inverted) {
                z += InvertedRotationAmount * InvertedRotationMultiplier;
            }

            image.transform.rotation = Quaternion.Euler(0, 0, z);
        }
    }
}