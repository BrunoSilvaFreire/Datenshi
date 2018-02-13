using Datenshi.Scripts.Util.StateMachine;
using Datenshi_Input_Constants;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Components.Movement {
    public class NormalGroundState : GroundState {
        private GameEntity entity;

        public static float CalculateSpeedMultiplier(GameEntity e) {
            return Evaluate(e, e.groundMovement.AccelerationCurve);
        }

        private static float Evaluate(GameEntity gameEntity, AnimationCurve curve) {
            var speed = gameEntity.velocity.Velocity.magnitude;
            var currentPos = speed / gameEntity.groundMovement.MaxSpeed;
            return curve.Evaluate(currentPos);
        }

        public override void OnExecute(StateMachine<GroundState> stateMachine) {
            var vel = entity.velocity;
            var groundMovement = entity.groundMovement;
            var controller = groundMovement.Provider;
            float x;
            float y;
            bool jump;
            if (controller == null) {
                x = 0;
                y = 0;
                jump = false;
            } else {
                x = controller.GetXInput();
                y = controller.GetYInput();
                jump = controller.GetButtonDown(Action.Jump);
            }
            //x *= entity.SpeedMultiplier;
            //y *= entity.SpeedMultiplier;
/*            if (Mathf.Abs(x) < Constants.DefaultInputThreshold && !jump) {
                //Not inputing, should deaccelerate if grounded
                if (entity.Grounded) {
                    vel.x = Mathf.Lerp(vel.x, 0, entity.CurrentDeacceleration);
                }
            } else {
                //Has input, do movement
                if (IsUnderLimit(x, vel.x, entity)) {
                    vel.x += x * entity.CurrentAcceleration;
                }
                vel.x = Mathf.Clamp(vel.x, -entity.MaxSpeed, entity.MaxSpeed);
            }
            if (jump && entity.Grounded) {
                vel.y += entity.JumpHeight;
                EntityJumpEvent.Instance.Invoke(entity);
            }
            var p = entity.Controller;
            p.Move(vel * Time.deltaTime, new Vector2(x, y));
            if (!p.Collisions.Below) {
                vel.y += entity.Gravity * Time.deltaTime;
            } else if (!jump) {
                vel.y = entity.Gravity * Time.deltaTime;
            }
            entity.Velocity = vel;*/
        }

        public override bool AllowInteraction() {
            return true;
        }
    }
}