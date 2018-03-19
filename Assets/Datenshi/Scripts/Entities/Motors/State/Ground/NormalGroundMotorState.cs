using System;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors.State.Ground {
    public class NormalGroundMotorState : GroundMotorState {
        public static readonly NormalGroundMotorState Instance = new NormalGroundMotorState();

        private NormalGroundMotorState() { }

        public override void Execute(
            MovableEntity entity,
            MotorStateMachine<GroundMotorState> machine,
            ref CollisionStatus collStatus) {
            var vel = entity.Velocity;
            ProcessInputs(ref vel, entity, machine);
            vel.y += GameResources.Instance.Gravity * entity.GravityScale * Time.deltaTime;
            var maxSpeed = entity.MaxSpeed;
            vel.x = Mathf.Clamp(vel.x, -maxSpeed, maxSpeed);
            RaycastHit2D? vertical;
            RaycastHit2D? horizontal;
            var bounds = (Bounds2D) entity.Hitbox.bounds;
            //bounds.Center += vel * Time.deltaTime;
            DebugUtil.DrawBounds2D(bounds, Color.red);
            var skinBounds = bounds;
            var skinWidth = entity.SkinWidth;
            skinBounds.Expand(-2 * skinWidth);
            var layerMask = GameResources.Instance.WorldMask;
            PhysicsUtil.DoPhysics(entity, ref vel, ref collStatus, out vertical, out horizontal, bounds, skinBounds,
                layerMask);
            if (CheckSlope(entity, ref collStatus, ref vel, bounds, skinBounds, layerMask, horizontal, machine,
                skinWidth)) {
                return;
            }

            vel.x *= entity.SpeedMultiplier;
            entity.Velocity = vel;
        }

        public bool CheckSlope(
            MovableEntity entity,
            ref CollisionStatus collStatus,
            ref Vector2 vel,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask,
            RaycastHit2D? horizontal,
            MotorStateMachine<GroundMotorState> machine,
            float skinWidth) {
            var provider = entity.InputProvider;
            var hasInput = provider != null && Mathf.Abs(provider.GetHorizontal()) > 0;
            var config = (GroundMotorConfig) entity.Config;
            var gravity = new Vector2(0, -config.SlopeGroundCheckLength);
            var vertical =
                PhysicsUtil.RaycastEntityVertical(ref gravity, entity, ref collStatus, bounds, skinBounds, layerMask);
            if (vertical.HasValue && hasInput) {
                var val = vertical.Value;
                var angle = Mathf.Abs(Vector2.Angle(val.normal, Vector2.up));
                var max = ((GroundMotorConfig) entity.Config).MaxAngle;
                Debug.Log((Mathf.RoundToInt(Mathf.Abs(angle)) != 0) + "  && " + (angle < max));
                if (Mathf.RoundToInt(Mathf.Abs(angle)) != 0 && angle < max) {
                    machine.SetState(entity, ref collStatus, SlopeGroundMotorState.Instance);
                    return true;
                }
            }

            if (horizontal.HasValue && hasInput) {
                //Check for slope
                var val = horizontal.Value;
                var angle = Mathf.Abs(Vector2.Angle(val.normal, Vector2.up));
                var max = ((GroundMotorConfig) entity.Config).MaxAngle;
                if (angle < max) {
                    machine.SetState(entity, ref collStatus, SlopeGroundMotorState.Instance);
                    return true;
                }

                //On this point, we may be "bugged inside"
                var origin = new Vector2(val.point.x, vel.y + skinWidth);
                var secondTry = Physics2D.Raycast(origin, vel, vel.magnitude, layerMask);
                Debug.DrawRay(origin, vel, secondTry ? Color.green : Color.red);
                if (!secondTry) {
                    var pos = entity.transform.position;
                    pos.y += skinWidth;
                    entity.transform.position = pos;
                    machine.SetState(entity, ref collStatus, SlopeGroundMotorState.Instance);
                    return true;
                }
            }

            return false;
        }

        public static void ProcessInputs(ref Vector2 vel, MovableEntity entity,
            MotorStateMachine<GroundMotorState> machine) {
            var provider = entity.InputProvider;
            var hasProvider = provider != null;
            if (hasProvider && provider.GetDash()) {
                var lastTimeDash = entity.GetVariable(DashGroundMotorState.DashStart);
                if (Time.time - lastTimeDash > ((GroundMotorConfig) entity.Config).DashCooldown) {
                    machine.CurrentState = DashGroundMotorState.Instance;
                    return;
                }
            }

            if (hasProvider && entity.CollisionStatus.Down) {
                if (provider.GetSubmit()) {
                    entity.Interact();
                }
                if (provider.GetJump()) {
                    vel.y = entity.YForce;
                } else if (provider.GetAttack()) {
                    entity.SetVariable(LivingEntity.Attacking, true);
                }
            }

            var xInput = hasProvider ? provider.GetHorizontal() : 0;
            if (hasProvider && provider.GetWalk()) {
                xInput /= 2;
            }

            var inputDir = Math.Sign(xInput);
            var velDir = Math.Sign(vel.x);
            var rawAcceleration = entity.AccelerationCurve.Evaluate(entity.SpeedPercent);
            var acceleration = rawAcceleration * inputDir;
            var maxSpeed = entity.MaxSpeed * Mathf.Abs(xInput);
            var speed = Mathf.Abs(vel.x);
            if (velDir == 0 || velDir == inputDir && inputDir != 0) {
                //Accelerating
                if (speed < maxSpeed) {
                    vel.x += acceleration;
                }

                return;
            }

            var deacceleration = entity.AccelerationCurve.Evaluate(1 - entity.SpeedPercent);
            if (Mathf.Abs(xInput) > 0) {
                //Changing direction, Double deacceleration
                vel.x += deacceleration * inputDir * 2;
                return;
            }

            //Not inputting
            if (speed < rawAcceleration) {
                vel.x = 0;
            } else {
                vel.x += deacceleration * -velDir;
            }
        }
    }
}