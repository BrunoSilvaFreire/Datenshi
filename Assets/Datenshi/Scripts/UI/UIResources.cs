using Datenshi.Scripts.UI.Dialogue;
using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    [CreateAssetMenu(menuName = "Datenshi/Resources/UIResources")]
    public class UIResources : SingletonScriptableObject<UIResources> {
        public UIPlayerView PlayerViewPrefab;
        public UIDialogueBox DialogueBoxPrefab;
    }
}