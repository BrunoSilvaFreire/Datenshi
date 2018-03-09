using Datenshi.Scripts.AI.Behaviour;

namespace Datenshi.Scripts.Entities.Input {
    public class AIStateInputProvider : InputProvider {
        public BehaviourState CurrentState;

        public MovableEntity Entity;

        public float Horizontal;
        public float Vertical;
        public bool Jump;
        public bool Attack;
        public bool Walk;
        public bool Dash;

        public override float GetHorizontal() {
            return Horizontal;
        }

        public override float GetVertical() {
            return Vertical;
        }

        public override bool GetJump() {
            return Jump;
        }

        public override bool GetAttack() {
            return Attack;
        }

        public override bool GetWalk() {
            return Walk;
        }

        public override bool GetDash() {
            return Dash;
        }

        private void Update() {
            if (CurrentState == null) {
                return;
            }

            CurrentState.Execute(this, Entity);
        }
    }
}