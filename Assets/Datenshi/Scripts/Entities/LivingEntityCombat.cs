using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;

namespace Datenshi.Scripts.Entities {
    public partial class LivingEntity : ICombatant {
        private CombatRelationship relationship;
        public CombatRelationship Relationship => relationship;


        public bool IsEnemy(LivingEntity entity) {
            if (Relationship == CombatRelationship.Neutral || entity.Relationship == CombatRelationship.Neutral) {
                return false;
            }

            return Relationship != entity.Relationship;
        }

        public bool IsNeutral => Relationship == CombatRelationship.Neutral;

        public bool IsAlly => Relationship == CombatRelationship.Ally;

        public void ExecuteAttack(Attack attack) {
            if (Stunned || attack == null) {
                return;
            }

            OnAttack.Invoke(attack);
            attack.Execute(this);
        }
    }
}