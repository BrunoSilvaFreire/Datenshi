using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Character {
    [CreateAssetMenu(menuName = "Datenshi/Characters/Portrait")]
    public class CharacterPortrait : ScriptableObject {
        public Sprite Image;
        public Vector2 Scale;
        public AudioClip SpeechClip;
        public float VerticalOffset;
        public bool SpriteFacesLeft;
#if UNITY_EDITOR
        public RectTransform CopyFrom {
            get {
                return null;
            }
            set {
                if (value == null) {
                    return;
                }

                Scale = value.localScale;
            }
        }
#endif
    }
}