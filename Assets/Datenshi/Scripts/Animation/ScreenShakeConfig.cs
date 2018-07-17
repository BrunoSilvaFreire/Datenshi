using UnityEngine;

namespace Datenshi.Scripts.Animation {
    [CreateAssetMenu(menuName = "Datenshi/Misc/ScreenShakeConfig")]
    public class ScreenShakeConfig : ScriptableObject {
        public float Duration = 3;

        public float Strength = 3;
    }
}