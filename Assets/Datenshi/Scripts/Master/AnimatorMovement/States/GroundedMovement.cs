using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement.States {
    public class GroundedMovement : AnimatorMovement<GroundedAnimatorConfig> {
        public float GravityScale = 1;
        public float OposingForceMultiplier = 2;
        protected override void OnEnter(RigidEntity entity, GroundedAnimatorConfig config) {
            entity.Rigidbody.gravityScale = GravityScale;
        }

        protected override void Move(ref Vector2 velocity, RigidEntity entity, GroundedAnimatorConfig config,
            Animator animator1) {
            var collisionStatus = entity.CollisionStatus;
            var animator = entity.AnimatorUpdater;
            var provider = entity.InputProvider;
            var hasProvider = provider != null;
            var xInput = hasProvider ? provider.GetHorizontal() : 0;
            var inputDir = System.Math.Sign(xInput);
            entity.Defending = hasProvider && provider.GetDefend();
            if (hasProvider && provider.GetAttack()) {
                animator.TriggerAttack();
            }

            var shouldJump = hasProvider && provider.GetJumpDown() && !entity.Defending;
            if (shouldJump && entity.JumpEllegible()) {
                //Grounded and should jump
                velocity.y = config.JumpForce;
            }

            var velDir = System.Math.Sign(velocity.x);
            var curve = config.Acceleration;
            var maxSpeed = config.MaxSpeed * entity.SpeedMultiplier;
            var speedPercent = entity.GetSpeedPercentage();
            var rawAcceleration = curve.Evaluate(speedPercent) * entity.SpeedMultiplier;
            var acceleration = rawAcceleration * inputDir;
            var speed = Mathf.Abs(velocity.x);
            //Check acceleration if (is stopped or (input is not empty and input is same dir as vel))
            if (velDir == 0 || velDir == inputDir && inputDir != 0) {
                velocity.x += acceleration;
            } else {
                var deacceleration = curve.Evaluate(1 - speedPercent);
                if (Mathf.Abs(xInput) > 0) {
                    //Changing direction, Double deacceleration
                    velocity.x += deacceleration * inputDir * OposingForceMultiplier;
                } else {
                    //Not inputting
                    if (speed < rawAcceleration) {
                        velocity.x = 0;
                    } else {
                        velocity.x += deacceleration * -velDir;
                    }
                }
            }
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        }
    }
}