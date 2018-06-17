using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class GetCurrentEntity : Action {
        public SharedLivingEntity Target;

        public override TaskStatus OnUpdate() {
            Target.Value = PlayerController.Instance.CurrentEntity as LivingEntity;
            return Target.Value == null ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}