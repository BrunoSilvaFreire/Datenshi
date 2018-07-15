using Datenshi.Scripts.Combat;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class ContactDamager : MonoBehaviour, IDamageSource {
        public LivingEntity Owner;
        public uint Damage;

        private void OnCollisionEnter2D(Collision2D other) {
            var c = other.collider.GetComponentInParent<LivingEntity>();
            if (Owner.Dead || !Owner.ShouldAttack(c)) {
                return;
            }

            var info = new DamageInfo(this, 1, c, Owner);
            c.Damage(ref info);
        }

        public uint GetDamage(IDamageable damageable) {
            return Damage;
        }
    }
}