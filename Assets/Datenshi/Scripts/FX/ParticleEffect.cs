using UnityEngine;

namespace Datenshi.Scripts.FX {
    [CreateAssetMenu(menuName = "Datenshi/Effects/ParticleEffect")]
    public class ParticleEffect : Effect {
        public ParticleSystem EffectPrefab;

        public override void Execute(Vector3 location) {
            var i = Instantiate(EffectPrefab, location, Quaternion.identity);
            if (!i.main.playOnAwake) {
                i.Play();
            }
        }
    }
}