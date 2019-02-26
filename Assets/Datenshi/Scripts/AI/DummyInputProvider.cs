using System;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;

namespace Datenshi.Scripts.AI {
    [Serializable]
    public class SerializableInputReceiver : SerializableInterface<IInputReceiver> { }

    public class DummyInputProvider : DatenshiInputProvider {
        public SerializableInputReceiver Entity;
        public float Horizontal;
        public float Vertical;
        public ConsumableInput Attack, Jump;
        public ConsumableInput Dash;
        public bool Submit;
        public bool Focus;


        private void Start() {
            var e = Entity.Value;
            if (e == null) {
                return;
            }

            e.RevokeOwnership();
            e.RequestOwnership(this);
        }


        public override float GetHorizontal() {
            return Fetch(Horizontal);
        }

        private static T Fetch<T>(T horizontal) {
            var i = RuntimeResources.Instance;
            if (i == null) {
                return default(T);
            }

            return !i.AllowPlayerInput ? default(T) : horizontal;
        }

        public override float GetVertical() {
            return Fetch(Vertical);
        }

        public override ConsumableInput GetJump() {
            return Jump;
        }

        public override ConsumableInput GetAttack() {
            return Attack;
        }

        public override ConsumableInput GetDash() {
            return Dash;
        }

        public override bool GetDashing() {
            return Dash.Peek();
        }

        public override bool GetJumping() {
            return Jump.Peek();
        }

        public override bool GetSubmit() {
            return Fetch(Submit);
        }

        public override bool GetFocus() {
            return Focus;
        }

        public void Reset() {
            Jump.Consume();
            Attack.Consume();
            Dash.Consume();
            Submit = false;
            Horizontal = 0;
            Vertical = 0;
        }
    }
}