using Datenshi.Scripts.UI.Dialogue;
using Datenshi.Scripts.UI.Menus;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    [CreateAssetMenu(menuName = "Datenshi/Resources/UIResources")]
    public class UIResources : SingletonScriptableObject<UIResources> {
        public UICharacterView CharacterViewPrefab;
        public UICharacterSelectionMenu CharacterSelectionMenuPrefab;
        public UIPlayerView PlayerViewPrefab;
        public UIDialogueBox DialogueBoxPrefab;
    }
}