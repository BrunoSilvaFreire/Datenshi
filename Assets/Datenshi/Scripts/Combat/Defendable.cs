namespace Datenshi.Scripts.Combat {
    public interface IDefendable {
        bool CanDefend(ICombatant combatant);
        float Defend(ICombatant combatant, ref DamageInfo info);
    }
}