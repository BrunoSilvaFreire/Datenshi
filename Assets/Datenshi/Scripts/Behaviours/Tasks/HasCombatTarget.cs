using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class HasCombatTarget : Conditional {
        public SharedCombatant Target;

        public override TaskStatus OnUpdate() {
            return Target.Value == null ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}