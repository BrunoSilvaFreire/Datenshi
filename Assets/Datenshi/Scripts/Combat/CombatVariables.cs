using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Misc;

namespace Datenshi.Scripts.Combat {
    public static class CombatVariables {
        public static readonly Variable<float> LastAttack =
            new Variable<float>("entity.ai.combat.strategy.lastAttack", 0);

        public static readonly Variable<LivingEntity> EntityTarget = new Variable<LivingEntity>("entity.ai.target", null);
    }
}