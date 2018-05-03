using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class CloseToCombatTarget : Conditional {
        public LivingEntity Entity;
        public SharedLivingEntity Target;
        public float Distance;

        public override TaskStatus OnUpdate() {
            var target = Target.Value;
            if (target == null) {
                return TaskStatus.Failure;
            }

            var d = Vector2.Distance(Entity.transform.position, target.Center);
            return d > Distance ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}