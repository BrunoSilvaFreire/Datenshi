using System;
using Datenshi.Scripts.Data;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States.Terrestrial {
    public class WallClimbingMovement : MovementState<TerrestrialConfig> {
        public AnimationCurve Gravity = AnimationCurve.Linear(0, 0, 3, 39.24F);
        public float WallCounterForce = 5;
        public float WallJumpMultiplier = 2;
        public MovementState DefaultState;
        private static readonly Variable<float> EnteredAt = "movement.wallClimb.enteredAt";

        protected override void OnEnter(MovableEntity entity, StateMotor motor, TerrestrialConfig config) {

            entity.SetVariable(EnteredAt, 0);
            entity.Rigidbody.gravityScale = 0;
        }


        protected override void Move(ref Vector2 velocity, MovableEntity entity, TerrestrialConfig config,
            StateMotor motor) {
            var coll = entity.CollisionStatus;
            var collDir = coll.HorizontalCollisionDir;
            if (coll.Down || collDir == 0) {
                motor.SetState(entity, DefaultState);
                return;
            }

            var enteredAt = entity.GetVariable(EnteredAt);
            enteredAt += Time.deltaTime;
            entity.SetVariable(EnteredAt, enteredAt);
            velocity.y = -Gravity.Evaluate(enteredAt);
            velocity.x = 0;
            var exit = false;
            if (entity.InputProvider.GetJump().Consume()) {
                velocity.x = WallCounterForce * -collDir;
                velocity.y = WallJumpMultiplier * config.VerticalForce;
                exit = true;
            }

            var iDir = Math.Sign(entity.InputProvider.GetHorizontal());
            if (iDir != collDir) {
                exit = true;
            }


            if (exit) {
                coll.Left = false;
                coll.Right = false;
                entity.CollisionStatus = coll;
                motor.SetState(entity, DefaultState);
            }
        }
    }
}