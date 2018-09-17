using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Entities;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class UpdateAINavigator : Action {
        public AINavigator Navigator;
        public SharedVector2 Location;
        public MovableEntity Entity;
        
        public override TaskStatus OnUpdate() {
            var p = Entity.InputProvider as DummyInputProvider;
            if (p == null) {
                return TaskStatus.Failure;
            }
            Navigator.SetTarget(Location.Value);
            Navigator.Execute(Entity, p);
            return TaskStatus.Success;
        }
    }
}