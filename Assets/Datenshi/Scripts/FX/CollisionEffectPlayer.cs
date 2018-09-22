using Shiroi.FX.Effects;
using UnityEngine;

namespace Datenshi.Scripts.FX {
    public class CollisionEffectPlayer : MonoBehaviour {
        public Effect Effect;
        public float MaxEffectsPerSecond = 1;
        private float lastEffect;

        private void OnCollisionEnter2D(Collision2D other) {
            if (Time.time - lastEffect < 1 / MaxEffectsPerSecond) {
               return; 
            }
            lastEffect = Time.time;
            Effect.Play(new EffectContext(this));
        }
    }
}