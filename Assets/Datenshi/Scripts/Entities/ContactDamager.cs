using Datenshi.Scripts.Combat;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public class ContactDamager : MonoBehaviour, IDamageSource {
        public LivingEntity Owner;
        public uint Damage;

        private void OnTriggerEnter2D(Collider2D other) {
            var c = other.GetComponentInParent<LivingEntity>();
            Debug.Log("Collided with " + c);
            if (Owner.Dead || !Owner.ShouldAttack(c)) {
                return;
            }

            var info = new DamageInfo(this, 1, c, Owner);
            c.Damage(c, ref info);
        }

        public uint GetDamage(IDamageable damageable) {
            return Damage;
        }
    }
}