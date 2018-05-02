using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Datenshi.Scripts.Character {
    public class CharacterPortrait : ScriptableObject {
        public Image Image;
        public float VerticalOffset;
        public AppearanceMode Last;
        public bool SpriteFacesLeft;
    }

    [Serializable]
    public struct AppearanceMode {
        public float Duration;
        public float Offset;
        public bool Left;
        public AppearanceMode(float duration, float offset, bool left) {
            Duration = duration;
            Offset = offset;
            Left = left;
        }
#if UNITY_EDITOR

        [ShowInInspector]
        public bool Right {
            get {
                return !Left;
            }
            set {
                Left = !value;
            }
        }
#endif
    }
}