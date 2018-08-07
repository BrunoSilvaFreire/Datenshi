using System;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using UnityEngine.Events;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

namespace Datenshi.Scripts.Behaviours.Tasks {
    [Serializable]
    public class CombatTargetChangedEvent : UnityEvent<LivingEntity, ICombatant> {
        public static readonly CombatTargetChangedEvent Instance = new CombatTargetChangedEvent();
        private CombatTargetChangedEvent() { }
    }

    public class UpdateCombatTarget : Action {
        public LivingEntity Combatant;
        public SharedCombatant Target;

        public override TaskStatus OnUpdate() {
            var t = Target.Value;
            CombatTargetChangedEvent.Instance.Invoke(Combatant, t);
            Combatant.SetVariable(CombatVariables.AttackTarget, t);
            return TaskStatus.Success;
        }
    }
}