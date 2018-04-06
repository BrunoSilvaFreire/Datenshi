using Datenshi.Scripts.Util.Singleton;
using UnityEngine;

namespace Datenshi.Scripts.World.Parallax {
    [CreateAssetMenu(menuName = "Datenshi/Resources/ParallaxOption")]
    public class ParallaxOption : SingletonScriptableObject<ParallaxOption> {
        public bool MoveParallax;
    }
}