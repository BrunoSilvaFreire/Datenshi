using UnityEngine;

namespace Datenshi.Scripts.Tutorials.Popup {
    [CreateAssetMenu(menuName = "Datenshi/Tutorial/PopUpContent")]
    public class PopUpTutorialConfig : ScriptableObject {
        public PopUpTutorialContent Prefab;
    }
}