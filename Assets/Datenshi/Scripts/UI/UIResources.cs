using Datenshi.Scripts.UI.Menus;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    [CreateAssetMenu(menuName = "Datenshi/Resources/UIResources")]
    public class UIResources : SingletonScriptableObject<UIResources> {
        public UICharacterView CharacterViewPrefab;
        public UICharacterSelectionMenu CharacterSelectionMenuPrefab;
        public UIPlayerView PlayerViewPrefab;
    }
}