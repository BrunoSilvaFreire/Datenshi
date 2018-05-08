namespace Datenshi.Scripts.Combat {
    public interface IDefendable {
        bool CanDefend(ICombatant combatant);
        void Defend(ICombatant combatant, ref DamageInfo info);
        bool CanPoorlyDefend(ICombatant combatant);
        void PoorlyDefend(ICombatant combatant, ref DamageInfo info);
        DefenseType GetDefenseType();
    }

    public enum DefenseType {
        Deflect,
        Counter
    }
}