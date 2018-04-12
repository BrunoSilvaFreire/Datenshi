using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    public abstract class AttackStrategy : ScriptableObject {
        public abstract void Execute(AIStateInputProvider provider, LivingEntity entity, LivingEntity target);
        public abstract float GetMinimumDistance(LivingEntity entity, LivingEntity target);
    }
}