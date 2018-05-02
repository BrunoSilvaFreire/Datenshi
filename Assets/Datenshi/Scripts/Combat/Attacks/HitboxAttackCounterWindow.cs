using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    public class HitboxAttackCounterWindow : MonoBehaviour, IDefendable {
        public ICombatant Entity;
        public bool Available;
        public uint AttackDamage;
        public float StunDuration;

        public bool CanDefend(ICombatant entity) {
            return Available;
        }

        public void Defend(ICombatant entity) {
            Entity.Damage(entity, AttackDamage);
            Entity.Stun(StunDuration);
            PoorlyDefend(entity);
        }

        public bool CanPoorlyDefend(ICombatant entity) {
            return Available;
        }

        public void PoorlyDefend(ICombatant entity) {
            entity.SetVariable(AbstractHitboxAttack.Blocked, true);
        }

        public DefenseType GetDefenseType() {
            return DefenseType.Counter;
        }
    }
}