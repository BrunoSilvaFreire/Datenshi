using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Movement;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class CloseTo : Conditional {
        public float Threshold=1;
        public SharedVector2 Position;
        public Entity Entity;
        public override TaskStatus OnUpdate() {
            return Entity.DistanceTo(Position.Value) > Threshold ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}