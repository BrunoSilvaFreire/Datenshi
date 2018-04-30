using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;

namespace Datenshi.Scripts.Combat.Strategies {
    public class EmptyStrategy : AttackStrategy {
        public static readonly EmptyStrategy Instance = CreateInstance<EmptyStrategy>();
        public override void Execute(AIStateInputProvider provider, LivingEntity entity, LivingEntity target, DebugInfo info) { }

        public override float GetMinimumDistance(LivingEntity entity, LivingEntity target) {
            return 0;
        }

        public override float GetCost(LivingEntity entity, LivingEntity target) {
            return 0;
        }

        public override float GetEffectiveness(LivingEntity entity, LivingEntity target) {
            return 0;
        }

        public override string GetTitle() {
            return "Empty Strategy";
        }
    }
}