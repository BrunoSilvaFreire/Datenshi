using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using UnityEngine;

namespace Datenshi.Scripts.Combat.FX {
    public class CombatantFXPlayer : MonoBehaviour {
        public SerializableCombatant Combatant;
        public Effect Effect;

        private void Start() {
            Combatant.Value.OnDamaged.AddListener(OnDamaged);
        }

        private void OnDamaged(DamageInfo arg0) {
            Effect.Play(new EffectContext(Combatant.Value as MonoBehaviour, new PositionFeature(transform.position)));
        }
    }
}