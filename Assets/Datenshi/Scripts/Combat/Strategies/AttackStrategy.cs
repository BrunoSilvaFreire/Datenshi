using Datenshi.Scripts.AI;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    public abstract class AttackStrategy : ScriptableObject {
        public abstract void Execute(AIStateInputProvider provider, ICombatant entity, ICombatant target);
        public abstract float GetMinimumDistance(ICombatant entity, ICombatant target);
        public abstract float GetCost(ICombatant entity, ICombatant target);
        public abstract float GetEffectiveness(ICombatant entity, ICombatant target);
        public abstract string GetTitle();
    }
}