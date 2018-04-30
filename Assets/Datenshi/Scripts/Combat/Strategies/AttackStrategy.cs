using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    public abstract class AttackStrategy : ScriptableObject, IDebugabble {
        public abstract void Execute(AIStateInputProvider provider, LivingEntity entity, LivingEntity target, DebugInfo info);
        public abstract float GetMinimumDistance(LivingEntity entity, LivingEntity target);
        public abstract float GetCost(LivingEntity entity, LivingEntity target);
        public abstract float GetEffectiveness(LivingEntity entity, LivingEntity target);
        public abstract string GetTitle();
    }
}