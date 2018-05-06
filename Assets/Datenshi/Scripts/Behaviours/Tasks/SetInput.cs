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
            Provider.Attack = Attack;
            Provider.Jump = Jump;
            Provider.Horizontal = Horizontal;
            Provider.Vertical = Vertical;
            Provider.Walk = Walk;
            Provider.Dash = Dash;
            Provider.Submit = Submit;
            Provider.Defend = Defend;
            return TaskStatus.Success;
        }
    }
}