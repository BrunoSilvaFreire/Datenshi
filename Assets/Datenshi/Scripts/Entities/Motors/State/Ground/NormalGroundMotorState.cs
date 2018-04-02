using System;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Misc;
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
            var config = (GroundMotorConfig) entity.Config;

            var vel = entity.Velocity;
            int xDir;
            ProcessInputs(ref vel, entity, machine, collStatus, out xDir);
            vel.y += GameResources.Instance.Gravity * entity.GravityScale * Time.deltaTime;

            var maxSpeed = entity.MaxSpeed;
            if (collStatus.Down) {
                vel.x = Mathf.Clamp(vel.x, -maxSpeed, maxSpeed);
            }

            RaycastHit2D? vertical;
            RaycastHit2D? horizontal;
            var bounds = (Bounds2D) entity.Hitbox.bounds;
            //bounds.Center += vel * Time.deltaTime;
            var skinBounds = bounds;
            var skinWidth = entity.SkinWidth;
            skinBounds.Expand(-2 * skinWidth);
            var layerMask = GameResources.Instance.WorldMask;
            PhysicsUtil.DoPhysics(entity, ref vel, ref collStatus, out vertical, out horizontal, bounds, skinBounds,
                layerMask);
            var gravity = new Vector2(0, -config.SlopeGroundCheckLength);
            var down =
                PhysicsUtil.RaycastEntityVertical(ref gravity, entity, ref collStatus, bounds, skinBounds, layerMask);
            if (vel.y < 0 && IsRunningTowardsWall(down, collStatus, xDir)) {
                machine.SetState(entity, ref collStatus, WallClimbingState.Instance);
                return;
            }

            if (CheckSlope(entity, ref collStatus, ref vel, layerMask, horizontal, down, machine,
                skinWidth)) {
                return;
            }


            vel.x *= entity.SpeedMultiplier;
            entity.Velocity = vel;
        }

        private bool IsRunningTowardsWall(RaycastHit2D? down, CollisionStatus collStatus, int xDir) {
            return (!down.HasValue || !down.Value) && (collStatus.Left || collStatus.Right) &&
                   collStatus.HorizontalCollisionDir == xDir;
        }

        public bool CheckSlope(
            MovableEntity entity,
            ref CollisionStatus collStatus,
            ref Vector2 vel,
            LayerMask layerMask,
            RaycastHit2D? horizontal,
            RaycastHit2D? vertical,
            MotorStateMachine<GroundMotorState> machine,
            float skinWidth) {
            var provider = entity.InputProvider;
            var hasInput = provider != null && Mathf.Abs(provider.GetHorizontal()) > 0;

            if (vertical.HasValue && hasInput) {
                var val = vertical.Value;
                var angle = Mathf.Abs(Vector2.Angle(val.normal, Vector2.up));
                var max = ((GroundMotorConfig) entity.Config).MaxAngle;
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
            MotorStateMachine<GroundMotorState> machine,
            CollisionStatus collisionStatus) {
            int x;
            ProcessInputs(ref vel, entity, machine, collisionStatus, out x);
        }

        public static void ProcessInputs(ref Vector2 vel, MovableEntity entity,
            MotorStateMachine<GroundMotorState> machine,
            CollisionStatus collisionStatus,
            out int inputDir) {
            var provider = entity.InputProvider;
            var hasProvider = provider != null;
            var xInput = hasProvider ? provider.GetHorizontal() : 0;
            var config = ((GroundMotorConfig) entity.Config);
            inputDir = Math.Sign(xInput);
            if (hasProvider) {
                if (provider.GetDash()) {
                    var lastTimeDash = entity.GetVariable(DashGroundMotorState.DashStart);
                    if (Time.time - lastTimeDash > config.DashCooldown) {
                        machine.CurrentState = DashGroundMotorState.Instance;
                        return;
                    }
                }

                if (provider.GetAttack()) {
                    entity.SetVariable(LivingEntity.Attacking, true);
                }

                if (entity.CollisionStatus.Down) {
                    if (provider.GetSubmit()) {
                        entity.Interact();
                    }

                    if (provider.GetJumpDown()) {
                        vel.y = entity.YForce;
                    }
                } else {
                    if (vel.y > 0 && !provider.GetJump()) {
                        vel.y += GameResources.Instance.Gravity * Time.deltaTime * entity.GravityScale * config.JumpCutGravityModifier;
                    }
                }

                if (provider.GetWalk()) {
                    xInput /= 2;
                }
            }

            var velDir = Math.Sign(vel.x);
            var curve = collisionStatus.Down
                ? entity.AccelerationCurve
                : config.AirControlSlope;

            var rawAcceleration = curve.Evaluate(entity.SpeedPercent);
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

            var deacceleration = curve.Evaluate(1 - entity.SpeedPercent);
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