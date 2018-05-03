using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Entities;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class TriggerAttack : Action {
        public string AttackTrigger;
        public LivingEntity Entity;

        public override TaskStatus OnUpdate() {
            Entity.AnimatorUpdater.TriggerAttack(AttackTrigger);
            return TaskStatus.Success;
        }
    }
}