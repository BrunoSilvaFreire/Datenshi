using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.AI.Traits;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Sirenix.OdinInspector;

namespace Datenshi.Input {
    public class AIStateInputProvider : InputProvider {
        public BehaviourState CurrentState;

        public Entity Entity;

        [ShowInInspector]
        public Trait Trait {
            get;
            private set;
        }

        public float Horizontal;
        public float Vertical;
        public bool Jump;
        public bool Attack;
        public bool Walk;
        public bool Dash;
        public bool Submit;
        public bool Defend;

        public override float GetHorizontal() {
            return Fetch(Horizontal);
        }

        private static T Fetch<T>(T horizontal) {
            return !RuntimeVariables.Instance.AllowPlayerInput ? default(T) : horizontal;
        }

        public override float GetVertical() {
            return Fetch(Vertical);
        }

        public override bool GetJump() {
            return Fetch(Jump);
        }

        public override bool GetJumpDown() {
            return Fetch(Jump);
        }

        public override bool GetAttack() {
            return Fetch(Attack);
        }

        public override bool GetWalk() {
            return Fetch(Walk);
        }

        public override bool GetDash() {
            return Fetch(Dash);
        }

        public override bool GetDefend() {
            return Fetch(Defend);
        }

        public override bool GetSubmit() {
            return Fetch(Submit);
        }

        private void Start() {
            Entity.RevokeOwnership();
            Entity.RequestOwnership(this);
            Trait = GetComponentInParent<Trait>();
        }

        private void Update() {
            if (CurrentState == null) {
                return;
            }

            CurrentState.Execute(this, Entity);
            if (Trait != null) {
                Trait.Execute();
            }
        }

        public void Reset() {
            Vertical = 0;
            Horizontal = 0;
            Walk = false;
            Attack = false;
            Defend = false;
            Submit = false;
            Jump = false;
        }
    }
}