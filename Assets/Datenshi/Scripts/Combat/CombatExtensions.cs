namespace Datenshi.Scripts.Combat {
    public static class CombatExtensions {
        public static bool ShouldAttack(this ICombatant combatant, ICombatant target) {
            if (combatant == null || target == null) {
                return false;
            }

            return !target.Ignored && combatant.Relationship != target.Relationship;
        }
    }
}