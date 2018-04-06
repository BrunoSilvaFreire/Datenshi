﻿using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Game;

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
        }

        private void Update() {
            if (CurrentState == null) {
                return;
            }
            CurrentState.Execute(this, Entity);
        }
    }
}