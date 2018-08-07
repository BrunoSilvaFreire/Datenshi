using Datenshi.Scripts.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc {
    public class CombatStateTracker : MonoBehaviour {
        public LivingEntity Combatant;

        private void Start() {
            GlobalEntityDamagedEvent.Instance.AddListener(OnDamaged);
        }
        [ShowInInspector]
        public IDamageable LastDamaged {
            get;
            private set;
        }

        [ShowInInspector]
        public IDamageDealer LastDealer {
            get;
            private set;
        }

        [ShowInInspector]
        public IDamageSource LastSource {
            get;
            private set;
        }

        [ShowInInspector]
        public float LastDamageTime {
            get;
            private set;
        }

        private void OnDamaged(LivingEntity arg0, IDamageDealer damageDealer, IDamageSource damageSource, uint arg3) {
            if (arg0 == Combatant) {
                LastDealer = damageDealer;
                LastSource = damageSource;
                LastDamageTime = Time.time;
            } else if (Combatant == damageDealer) {
                LastDamaged = arg0;
            }
        }
    }
}