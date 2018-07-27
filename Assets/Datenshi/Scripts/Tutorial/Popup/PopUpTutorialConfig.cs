using UnityEngine;

namespace Datenshi.Scripts.Tutorial.Popup {
    [CreateAssetMenu(menuName = "Datenshi/Tutorial/PopUpContent")]
    public class PopUpTutorialConfig : ScriptableObject {
        public PopUpTutorialContent Prefab;
    }
}