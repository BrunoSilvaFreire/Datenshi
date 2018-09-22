using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Movement;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using Shiroi.FX.Utilities;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc {
    public class ContactDamager : MonoBehaviour, IDamageSource, IDefendable {
        public LivingEntity Owner;
        public uint Damage;
        public float DefenseFocusComsuption = 5;
        public Vector2 ThrowbackForce;
        public Effect DefenseEffect;

        private void OnCollisionEnter2D(Collision2D other) {
            var c = other.collider.GetComponentInParent<LivingEntity>();
            if (Owner.Dead || !Owner.ShouldAttack(c)) {
                return;
            }

            var info = new DamageInfo(this, 1, c, Owner);
            c.Damage(ref info, this);
        }

        public uint GetDamage(IDamageable damageable) {
            return Damage;
        }

        public bool CanDefend(ICombatant combatant) {
            return true;
        }

        public float Defend(ICombatant combatant, ref DamageInfo info) {
            var m = Owner as IMovable;
            if (m != null) {
                var vel = ThrowbackForce;
                vel.x *= Owner.XDirectionTo(combatant.Center);
                m.ExternalForces = vel;
            }

            if (DefenseEffect != null) {
                DefenseEffect.PlayIfPresent(this);
            }

            return DefenseFocusComsuption;
        }
    }
}