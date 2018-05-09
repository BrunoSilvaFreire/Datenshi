using UnityEngine;

namespace Datenshi.Scripts.Combat {
    public static class CombatExtensions {
        public static bool ShouldAttack(this ICombatant combatant, ICombatant target) {
            if (combatant == null || target == null) {
                return false;
            }

            var cr = combatant.Relationship;
            var tr = target.Relationship;
            if (cr == CombatRelationship.Neutral || tr == CombatRelationship.Neutral) {
                return false;
            }

            return !target.Dead && !target.Ignored && !Equals(cr, tr);
        }
    }
}