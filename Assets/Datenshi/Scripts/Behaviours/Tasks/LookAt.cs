using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Movement;
using Datenshi.Scripts.Util;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class LookAt : Action {
        public MovableEntity Entity;
        public SharedCombatant Target;
        public override TaskStatus OnUpdate() {
            var t = Target.Value;
            if (t == null) {
                return TaskStatus.Failure;
            }

            Entity.CurrentDirection = Direction.FromX(Entity.XDirectionTo(t.Center));
            return TaskStatus.Success;
        }
    }
}