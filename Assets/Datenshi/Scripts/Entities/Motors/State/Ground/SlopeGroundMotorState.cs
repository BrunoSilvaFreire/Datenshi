using System;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors.State.Ground {
    public class SlopeGroundMotorState : GroundMotorState {
        public static readonly Variable<bool> GroundedLastFrame =
            new Variable<bool>("entity.motor.slope.groundedLast", false);

        public static readonly SlopeGroundMotorState Instance = new SlopeGroundMotorState();
        private SlopeGroundMotorState() { }

        public override void Execute(
            MovableEntity entity,
            MotorStateMachine<GroundMotorState> machine,
            ref CollisionStatus collStatus) {
            var vel = entity.Velocity;
            var provider = entity.InputProvider;
            NormalGroundMotorState.ProcessInputs(ref vel, provider, entity, machine, collStatus);
            vel.y += GameResources.Instance.Gravity * entity.GravityScale * Time.deltaTime;
            var maxSpeed = entity.MaxSpeed;
            vel.x = Mathf.Clamp(vel.x, -maxSpeed, maxSpeed);
            RaycastHit2D? horizontal;
            RaycastHit2D? tempVer;
            var bounds = (Bounds2D) entity.Hitbox.bounds;
            //bounds.Center += vel * Time.deltaTime;
            DebugUtil.DrawBounds2D(bounds, Color.red);
            var skinBounds = bounds;
            skinBounds.Expand(-2 * entity.SkinWidth);
            var layerMask = GameResources.Instance.WorldMask;
            PhysicsUtil.DoPhysics(
                entity,
                ref vel,
                ref collStatus,
                out tempVer,
                out horizontal,
                bounds,
                skinBounds,
                layerMask);
            var config = (GroundMotorConfig) entity.Config;
            var groundedLastFrame = entity.GetVariable(GroundedLastFrame);
            var gravityY = -config.SlopeGroundCheckLength;
            if (groundedLastFrame) {
                gravityY *= 2;
            }

            var gravity = new Vector2(0, gravityY);
            var downRaycast =
                PhysicsUtil.RaycastEntityVertical(ref gravity, entity, ref collStatus, bounds, skinBounds, layerMask);
            var down = downRaycast.HasValue;
            collStatus.Down = down;
            if (horizontal.HasValue) {
                var raycast = horizontal.Value;
                var angle = Mathf.Abs(Vector2.Angle(raycast.normal, Vector2.up));
                var slopeLimit = config.MaxAngle;
                //var skinWidth = entity.SkinWidth;
                if (angle < slopeLimit && down) {
                    var slopeModifier = config.SlopeSpeedMultiplier.Evaluate(angle);
                    // apply the slopeModifier to slow our movement up the slope
                    if (vel.x < 0) {
                        //if was stopped, apply extra vel
                        var hasProvider = provider != null;
                        var xInput = hasProvider ? provider.GetHorizontal() : 0;
                        if (hasProvider && provider.GetWalk()) {
                            xInput /= 2;
                        }

                        vel.x += entity.AccelerationCurve.Evaluate(entity.SpeedPercent) * xInput;
                    }

                    var angleRad = angle * Mathf.Deg2Rad;
                    vel.x *= slopeModifier;
                    var max = maxSpeed * Mathf.Abs(Mathf.Cos(angleRad));
                    vel.x = Mathf.Clamp(vel.x, -max, max);
                    vel.y = Mathf.Abs(Mathf.Tan(angleRad) * vel.x);
                } else {
                    vel.x = 0;
                    machine.CurrentState = NormalGroundMotorState.Instance;
                }
            } else {
                HandleSlopeNoHorizontal(
                    entity,
                    bounds,
                    skinBounds,
                    layerMask,
                    groundedLastFrame,
                    downRaycast,
                    machine,
                    ref vel);
            }

            vel.x *= entity.SpeedMultiplier;
            entity.Velocity = vel;
        }

        private void HandleSlopeNoHorizontal(
            MovableEntity entity,
            Bounds2D bounds,
            Bounds2D skinBounds,
            LayerMask layerMask,
            bool groundedLastFrame,
            RaycastHit2D? down,
            MotorStateMachine<GroundMotorState> machine,
            ref Vector2 vel
        ) {
            Debug.Log("No horizontal");
            var slopeDescend = CheckVerticalSlope(entity, bounds, skinBounds, vel, layerMask);
            if (slopeDescend) {
                Debug.Log("Has vertical");
                var angle = Vector2.Angle(slopeDescend.normal, Vector2.up);
                if (Mathf.RoundToInt(Mathf.Abs(angle)) != 0) {
                    var slopeDir = Math.Sign(entity.GroundPosition.x - slopeDescend.point.x);
                    var entityDir = Math.Sign(vel.x);
                    Debug.Log("Angle = " + angle);
                    Debug.Log("Slope dir = " + slopeDir + " vs " + entityDir);
                    if (slopeDir == entityDir) {
                        vel.y = -Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * vel.x);
                        Debug.Log("Vel y = " + vel.y);
                        return;
                    }
                }
            }

            if (groundedLastFrame) {
                if (down.HasValue) {
                    var yDifference = down.Value.point.y - bounds.Min.y;
                    var newPos = entity.transform.position;
                    newPos.y += yDifference;
                    entity.transform.position = newPos;
                }

                vel.y = 0;
                entity.SetVariable(GroundedLastFrame, false);
            } else {
                entity.SetVariable(GroundedLastFrame, true);
            }

            machine.CurrentState = NormalGroundMotorState.Instance;
        }

        private RaycastHit2D CheckVerticalSlope(
            MovableEntity entity,
            Bounds2D bounds,
            Bounds2D skinBounds,
            Vector2 vel,
            LayerMask layerMask) {
            var dir = Math.Sign(vel.x);


            var xOrigin = skinBounds.Min.x;
            if (dir == -1) {
                var width = skinBounds.Size.x;
                var total = (entity.Motor.VerticalRays - 1);
                var spacing = width / total;
                xOrigin += total * spacing;
            }

            var gravity = ((GroundMotorConfig) entity.Config).SlopeGroundCheckLength * Time.deltaTime;
            var origin = new Vector2(xOrigin, bounds.Min.y);
            var dirVec = new Vector2(0, -gravity);
            var hit = Physics2D.Raycast(
                origin,
                dirVec,
                gravity,
                layerMask
            );
            Debug.DrawRay(origin, dirVec, hit ? Color.magenta : Color.cyan);
            return hit;
        }
    }
}