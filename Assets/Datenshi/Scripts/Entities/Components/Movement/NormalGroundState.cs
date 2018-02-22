using Datenshi.Scripts.Util.StateMachine;
using Datenshi_Input_Constants;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components.Movement {
    public class NormalGroundState : GroundState {
        public override void OnExecute(StateMachine<GroundState, GameEntity> stateMachine) {
            var entity = stateMachine.Owner;

            //Components
            var groundMovement = entity.groundMovement;
            var provider = groundMovement.Provider;
            var controller = groundMovement.Controller;
            //Variables
            var vel = entity.velocity.Velocity;
            var speed = vel.magnitude;
            var maxSpeed = groundMovement.MaxSpeed;
            var currentPos = speed / maxSpeed;
            var grounded = controller.Collisions.Below;
            //Inputs
            float x;
            float y;
            bool jump;
            if (provider == null) {
                //No provider
                x = 0;
                y = 0;
                jump = false;
            } else {
                x = provider.GetXInput();
                y = provider.GetYInput();
                jump = provider.GetButtonDown(Action.Jump);
            }
            x *= groundMovement.SpeedMultiplier;
            y *= groundMovement.SpeedMultiplier;
            if (Mathf.Abs(x) < Constants.DefaultInputThreshold && !jump) {
                //Not inputing, should deaccelerate if grounded
                if (grounded) {
                    var deacceleration = groundMovement.DeaccelerationCurve.Evaluate(currentPos);
                    vel.x = Mathf.Lerp(vel.x, 0, deacceleration);
                }
            } else {
                //Has input, do movement
                if (IsUnderLimit(x, vel.x, groundMovement)) {
                    var acceleration = groundMovement.AccelerationCurve.Evaluate(currentPos);
                    vel.x += x * acceleration;
                }
                vel.x = Mathf.Clamp(vel.x, -maxSpeed, maxSpeed);
            }
            if (jump && grounded) {
                vel.y += groundMovement.MaxJumpHeight;
            }

            var finalVel = vel * Time.deltaTime;
            controller.Move(ref finalVel, new Vector2(x, y));
            Debug.Log("");
            if (!controller.Collisions.Below) {
                vel.y += Constants.Gravity * Time.deltaTime;
            }
            entity.ReplaceVelocity(vel);
        }

        public override bool AllowInteraction() {
            return true;
        }

        private static bool IsUnderLimit(float x, float velX, GroundMovement entity) {
            var limit = entity.MaxSpeed * x;
            return x < 0 ? velX > limit : velX < limit;
        }
    }
}