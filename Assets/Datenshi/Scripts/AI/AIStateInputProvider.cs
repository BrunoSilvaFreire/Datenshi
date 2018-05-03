using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.AI.Traits;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI {
    public class AIStateInputProvider : DatenshiInputProvider {
        public BehaviourState CurrentState;

        public SerializableNavigable Navigable;

        [SerializeField]
        private Trait trait;

        public Trait Trait {
            get {
                return trait;
            }
        }

        public float Horizontal;
        public float Vertical;
        public bool Jump;
        public bool Attack;
        public bool Walk;
        public bool Dash;
        public bool Submit;
        public bool Defend;
        public bool ExecuteState = true;


        public override float GetHorizontal() {
            return Fetch(Horizontal);
        }

        private static T Fetch<T>(T horizontal) {
            return !RuntimeResources.Instance.AllowPlayerInput ? default(T) : horizontal;
        }

        public override float GetVertical() {
            return Fetch(Vertical);
        }

        public override float GetAxis(string key) {
            return 0;
        }

        public override float GetAxis(int id) {
            return 0;
        }

        public override bool GetButtonDown(string key) {
            return false;
        }

        public override bool GetButtonDown(int id) {
            return false;
        }

        public override bool GetButton(string key) {
            return false;
        }

        public override bool GetButton(int id) {
            return false;
        }

        public override bool GetButtonUp(string key) {
            return false;
        }

        public override bool GetButtonUp(int id) {
            return false;
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
            Navigable.Value.RevokeOwnership();
            Navigable.Value.RequestOwnership(this);
            if (trait == null) {
                trait = GetComponentInChildren<Trait>();
            }
        }

        private void Update() {
            if (CurrentState == null) {
                return;
            }

            if (ExecuteState) {
                CurrentState.Execute(this, Navigable.Value);
            }

            if (Trait != null) {
                Trait.Execute();
            }
        }

        private void OnDrawGizmos() {
            if (ExecuteState) {
                CurrentState.DrawGizmos(this, Navigable.Value);
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