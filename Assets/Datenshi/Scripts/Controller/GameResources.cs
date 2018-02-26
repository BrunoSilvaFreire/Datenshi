using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.Controller {
    [CreateAssetMenu(menuName = "Datenshi/Resources/GameResources")]
    public class GameResources : SingletonScriptableObject<GameResources> {
        public LayerMask WorldMask;

        public Camera CharacterCameraPrefab;
    }
}