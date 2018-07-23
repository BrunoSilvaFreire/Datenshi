using System.Collections.Generic;
using UnityEngine;

namespace Datenshi.Scripts.FX {
    [CreateAssetMenu(menuName = "Datenshi/Effects/CompositeEffect")]
    public class CompositeEffect : Effect {
        public List<Effect> SubEffects = new List<Effect>();

        public override void Execute(Vector3 location) {
            foreach (var subEffect in SubEffects) {
                subEffect.Execute(location);
            }
        }
    }
}