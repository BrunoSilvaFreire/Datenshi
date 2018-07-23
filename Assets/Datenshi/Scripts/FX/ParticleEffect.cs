using UnityEngine;

namespace Datenshi.Scripts.FX {
    [CreateAssetMenu(menuName = "Datenshi/Effects/ParticleEffect")]
    public class ParticleEffect : Effect {
        public ParticleSystem EffectPrefab;

        public override void Execute(Vector3 location) {
            Instantiate(EffectPrefab, location, Quaternion.identity);
        }
    }
}