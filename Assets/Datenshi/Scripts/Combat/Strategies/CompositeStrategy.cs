using System.Linq;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/CompositeStrategy")]
    public class CompositeStrategy : AttackStrategy {
        public AttackStrategy[] Strategies;

        public override void Execute(AIStateInputProvider provider, ICombatant entity, ICombatant target) {
            var s = Strategies.MinBy(strategy => strategy.GetCost(entity, target) - strategy.GetEffectiveness(entity, target));
            s.Execute(provider, entity, target);
        }

        public override float GetMinimumDistance(ICombatant entity, ICombatant target) {
            return Strategies.Min(strategy => strategy.GetMinimumDistance(entity, target));
        }

        public override float GetCost(ICombatant entity, ICombatant target) {
            return Strategies.Min(strategy => strategy.GetCost(entity, target));
        }

        public override float GetEffectiveness(ICombatant entity, ICombatant target) {
            return Strategies.Max(strategy => strategy.GetEffectiveness(entity, target));
        }

        public override string GetTitle() {
            return "Composite Strategy";
        }
    }
}