using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    [CreateAssetMenu(menuName = "Datenshi/Resources/UIResources")]
    public class UIResources : SingletonScriptableObject<UIResources> {
        public UICharacterView CharacterViewPrefab;
    }
}