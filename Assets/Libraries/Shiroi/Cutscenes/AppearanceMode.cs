using System;
using Sirenix.OdinInspector;

namespace Shiroi.Cutscenes {
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