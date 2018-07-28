using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Misc {
    public class KillZone : AbstractDamageDealer, IDamageSource {
        public uint Damage;

        public uint GetDamage(IDamageable damageable) {
            return Damage;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            
            var d = other.GetComponentInParent<ICombatant>();
            if ((Object) d == null || d.Relationship != CombatRelationship.Ally) {
                return;
            }

            d.Kill();
        }
    }
}