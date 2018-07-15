using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class LessThanFloat : Conditional {
        public SharedFloat Value;
        public SharedFloat CompareTo;

        public override TaskStatus OnUpdate() {
            return Value.Value > CompareTo.Value ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}