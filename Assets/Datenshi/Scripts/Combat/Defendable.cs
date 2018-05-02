namespace Datenshi.Scripts.Combat {
    public interface IDefendable {
        bool CanDefend(ICombatant combatant);
        void Defend(ICombatant combatant);
        bool CanPoorlyDefend(ICombatant combatant);
        void PoorlyDefend(ICombatant combatant);
        DefenseType GetDefenseType();
    }

    public enum DefenseType {
        Deflect,
        Counter
    }
}