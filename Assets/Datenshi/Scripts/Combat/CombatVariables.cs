using Datenshi.Scripts.Data;

namespace Datenshi.Scripts.Combat {
    public static class CombatVariables {
        public static readonly Variable<float> LastAttack =
            new Variable<float>("entity.ai.combat.strategy.lastAttack", 0);

        public static readonly Variable<ICombatant> AttackTarget = new Variable<ICombatant>("entity.ai.combat.target", null);
    }
}