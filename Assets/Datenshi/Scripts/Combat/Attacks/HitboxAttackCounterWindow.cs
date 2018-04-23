using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Interaction;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    public class HitboxAttackCounterWindow : MonoBehaviour, IDefendable {
        public LivingEntity Entity;
        public bool Available;
        public uint AttackDamage;
        public float StunDuration;

        public bool CanDefend(LivingEntity entity) {
            return Available;
        }

        public void Defend(LivingEntity entity) {
            Entity.Damage(entity, AttackDamage);
            Entity.Stun(StunDuration);
            PoorlyDefend(entity);
        }

        public bool CanPoorlyDefend(LivingEntity entity) {
            return Available;
        }

        public void PoorlyDefend(LivingEntity entity) {
            entity.SetVariable(AbstractHitboxAttack.Blocked, true);
        }

        public DefenseType GetDefenseType() {
            return DefenseType.Counter;
        }
    }
}