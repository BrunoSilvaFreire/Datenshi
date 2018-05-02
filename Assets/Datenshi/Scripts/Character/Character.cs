using UnityEngine;

namespace Datenshi.Scripts.Character {
    [CreateAssetMenu(menuName = "Datenshi/Characters/Character")]
    public class Character : ScriptableObject {
        public const float AlternativeBrightnessDifference = 0.3F;
        public string Alias;
        public Color SignatureColor;
        public CharacterPortrait Portrait;

        public Color AlternativeSignatureColor {
            get {
                float h, s, v;
                Color.RGBToHSV(SignatureColor, out h, out s, out v);
                v -= AlternativeBrightnessDifference;
                return Color.HSVToRGB(h, s, v);
            }
        }
    }
}