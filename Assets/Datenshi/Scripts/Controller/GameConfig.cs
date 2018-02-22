using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.Controller {
    [CreateAssetMenu(menuName = "Datenshi/Resources/GameConfig")]
    public class GameConfig : SingletonScriptableObject<GameConfig> {
        public LayerMask WorldMask;
    }
}