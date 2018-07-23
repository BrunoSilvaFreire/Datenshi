using UnityEngine;

namespace Datenshi.Scripts.FX {
    public class EffectExecutor : MonoBehaviour {
        public Effect Effect;

        public void Spawn() {
            Effect.Execute(transform.position);
        }
    }
}