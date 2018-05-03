using BehaviorDesigner.Runtime.Tasks;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class PatrolTask : Action {
        public override TaskStatus OnUpdate() {
            return TaskStatus.Running;
        }
    }
}