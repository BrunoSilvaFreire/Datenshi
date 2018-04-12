using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Interaction;

namespace Datenshi.Scripts.Combat.Attacks {
    public class HitboxAttackCounterWindow : Defendable {
        public LivingEntity Entity;
        public bool Available;
        public uint AttackDamage;
        public float StunDuration;
        public override bool CanDefend(LivingEntity entity) {
            return Available;
        }

        public override void Defend(LivingEntity entity) {
            Entity.Damage(entity, AttackDamage);
            Entity.Stun(StunDuration);
            PoorlyDefend(entity);
        }

        public override bool CanPoorlyDefend(LivingEntity entity) {
            return Available;
        }

        public override void PoorlyDefend(LivingEntity entity) {
            entity.SetVariable(AbstractHitboxAttack.Blocked, true);
        }

        public override DefenseType GetDefenseType() {
            return DefenseType.Counter;
        }
    }
}