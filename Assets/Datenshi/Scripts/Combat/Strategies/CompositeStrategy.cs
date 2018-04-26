using System.Linq;
using Datenshi.Input;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/CompositeStrategy")]
    public class CompositeStrategy : AttackStrategy {
        public AttackStrategy[] Strategies;

        public override void Execute(AIStateInputProvider provider, LivingEntity entity, LivingEntity target) {
            var s = Strategies.MinBy(strategy => strategy.GetCost(entity, target) - strategy.GetEffectiveness(entity, target));

            Debug.Log("Current Strategy = " + s);
            s.Execute(provider, entity, target);
        }

        public override float GetMinimumDistance(LivingEntity entity, LivingEntity target) {
            return Strategies.Min(strategy => strategy.GetMinimumDistance(entity, target));
        }

        public override float GetCost(LivingEntity entity, LivingEntity target) {
            return Strategies.Min(strategy => strategy.GetCost(entity, target));
        }

        public override float GetEffectiveness(LivingEntity entity, LivingEntity target) {
            return Strategies.Max(strategy => strategy.GetEffectiveness(entity, target));
        }
    }
}