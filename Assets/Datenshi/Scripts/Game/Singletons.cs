using Datenshi.Scripts.Util.Singleton;
using UnityEngine;
using UnityEngine.Playables;

namespace Datenshi.Scripts.Game {
    [CreateAssetMenu(menuName = "Datenshi/Singletons")]
    public class Singletons : SingletonScriptableObject<Singletons> {
        public PlayerController PlayerControllerPrefab;
    }
}