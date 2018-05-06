using System;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using Rewired;

namespace Datenshi.Scripts.AI {
    [Serializable]
    public class SerializableInputReceiver : SerializableInterface<IInputReceiver> { }

    public class DummyInputProvider : DatenshiInputProvider {
        public SerializableInputReceiver Entity;
        public float Horizontal;
        public float Vertical;
        public bool Jump;
        public bool Attack;
        public bool Walk;
        public bool Dash;
        public bool Submit;
        public bool Defend;

        private void Start() {
            var e = Entity.Value;
            e.RevokeOwnership();
            e.RequestOwnership(this);
        }

        private bool jumpDown;
        private bool lastJump;

        private void Update() {
            jumpDown = lastJump != Jump && Jump && !jumpDown;
            lastJump = Jump;
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
            return jumpDown;
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

        public void Reset() {
            Jump = false;
            Attack = false;
            Dash = false;
            Defend = false;
            Walk = false;
            Submit = false;
            Horizontal = 0;
            Vertical = 0;
        }
    }
}