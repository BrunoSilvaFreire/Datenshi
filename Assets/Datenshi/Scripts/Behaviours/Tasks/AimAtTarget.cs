using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class AimAtTarget : Action {
        public SharedCombatant Target;
        public Entity Entity;

        public override TaskStatus OnUpdate() {
            var c = Target.Value;
            if (c == null) {
                return TaskStatus.Failure;
            }

            var p = Entity.InputProvider as DummyInputProvider;
            if (p == null) {
                return TaskStatus.Failure;
            }

            var value = c.Center - Entity.Center;
            value.Normalize();
            p.Horizontal = value.x;
            p.Vertical = value.y;
            return TaskStatus.Success;
        }
    }
}