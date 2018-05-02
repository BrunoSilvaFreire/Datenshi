using Datenshi.Scripts.AI;
namespace Datenshi.Scripts.Combat.Strategies {
    public class EmptyStrategy : AttackStrategy {
        public static readonly EmptyStrategy Instance = CreateInstance<EmptyStrategy>();
        public override void Execute(AIStateInputProvider provider, ICombatant entity, ICombatant target) { }

        public override float GetMinimumDistance(ICombatant entity, ICombatant target) {
            return 0;
        }

        public override float GetCost(ICombatant entity, ICombatant target) {
            return 0;
        }

        public override float GetEffectiveness(ICombatant entity, ICombatant target) {
            return 0;
        }

        public override string GetTitle() {
            return "Empty Strategy";
        }
    }
}