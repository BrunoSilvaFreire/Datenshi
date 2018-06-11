namespace Datenshi.Scripts.Combat {
    public interface IDefendable {
        bool CanAutoDefend(ICombatant combatant);
        float DoAutoDefend(ICombatant combatant, ref DamageInfo info);
        bool CanAgressiveDefend(ICombatant combatant);
        float DoAgressiveDefend(ICombatant combatant, ref DamageInfo info);
        bool CanEvasiveDefend(ICombatant combatant);
        float DoEvasiveDefend(ICombatant combatant, ref DamageInfo info);
    }

    public static class DefendableExtensions {
        public static void AutoDefend(this IDefendable defendable, ICombatant combatant, ref DamageInfo info) {
            var focusToConsume = defendable.DoAutoDefend(combatant, ref info);
            combatant.DefendTimeLeft -= focusToConsume;
        }

        public static void AgressiveDefend(this IDefendable defendable, ICombatant combatant, ref DamageInfo info) {
            var focusToConsume = defendable.DoAgressiveDefend(combatant, ref info);
            combatant.DefendTimeLeft -= focusToConsume;
        }

        public static void EvasiveDefend(this IDefendable defendable, ICombatant combatant, ref DamageInfo info) {
            var focusToConsume = defendable.DoEvasiveDefend(combatant, ref info);
            combatant.DefendTimeLeft -= focusToConsume;
        }
    }
}