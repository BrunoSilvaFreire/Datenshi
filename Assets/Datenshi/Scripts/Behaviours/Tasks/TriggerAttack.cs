using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class TriggerAttack : Action {
        public string AttackTrigger;
        public LivingEntity Entity;
        public SharedCombatant Target;
        public float Delay = 3;
        private float lastUsed;

        public override TaskStatus OnUpdate() {
            var now = Time.time;
            if (now - lastUsed < Delay) {
                return TaskStatus.Success;
            }

            lastUsed = now;
            var t = Target.Value;
            if (t != null) {
                Entity.SetVariable(CombatVariables.AttackTarget, t);
            }

            Entity.AnimatorUpdater.TriggerAttack(AttackTrigger);
            return TaskStatus.Success;
        }
    }
}