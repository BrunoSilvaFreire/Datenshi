using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.AI;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class SetInput : Action {
        public DummyInputProvider Provider;
        public float Horizontal;
        public float Vertical;
        public bool Jump;
        public bool Attack;
        public bool Walk;
        public bool Dash;
        public bool Submit;
        public bool Defend;

        public override TaskStatus OnUpdate() {
            if (Attack) {
                Provider.Attack.Set();
            } else {
                Provider.Attack.Consume();
            }

            if (Jump) {
                Provider.Jump.Set();
            } else {
                Provider.Jump.Consume();
            }

            Provider.Horizontal = Horizontal;
            Provider.Vertical = Vertical;
            if (Dash) {
                Provider.Dash.Set();
            } else {
                Provider.Dash.Consume();
            }

            Provider.Submit = Submit;
            Provider.Focus = Defend;
            return TaskStatus.Success;
        }
    }
}