using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;

namespace Datenshi.Scripts.Behaviours {
    public class DummyInputProvider : DatenshiInputProvider {
        public Entity Entity;
        public float Horizontal;
        public float Vertical;
        public bool Jump;
        public bool Attack;
        public bool Walk;
        public bool Dash;
        public bool Submit;
        public bool Defend;

        private void Start() {
            Entity.RevokeOwnership();
            Entity.RequestOwnership(this);
        }

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
    }
}