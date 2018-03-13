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

        public override void Execute(MovableEntity entity, MotorStateMachine<GroundMotorState> machine,
            ref CollisionStatus collStatus) {
            var vel = entity.Velocity;
            NormalGroundMotorState.ProcessInputs(ref vel, entity, machine);
            if (entity.InputProvider.GetJump()) {
                machine.CurrentState = NormalGroundMotorState.Instance;
                return;
            }

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
            PhysicsUtil.DoPhysics(entity, ref vel, ref collStatus, out tempVer, out horizontal, bounds, skinBounds,
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
            var vertical = CheckVerticalSlope(entity, skinBounds, vel, layerMask);
            if (!horizontal.HasValue) {
                Debug.Log("No horizontal");
                if (vertical) {
                    Debug.Log("Has vertical");
                    var angle = Vector2.Angle(vertical.normal, Vector2.up);
                    if (Mathf.RoundToInt(Mathf.Abs(angle)) != 0) {
                        var slopeDir = Math.Sign(vertical.point.x - entity.GroundPosition.x);
                        var entityDir = Math.Sign(vel.x);
                        Debug.Log("Angle = " + angle);
                        Debug.Log("Slope dir = " + slopeDir + " vs " + entityDir);
                        if (slopeDir == entityDir) {
                            vel.y = -Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * vel.x);
                            Debug.Log("Vel y = " + vel.y);
                        }
                    }
                }

                if (groundedLastFrame) {
                    if (down) {
                        var yDifference = downRaycast.Value.point.y - bounds.Min.y;
                        var newPos = entity.transform.position;
                        newPos.y += yDifference;
                        entity.transform.position = newPos;
                    }

                    entity.SetVariable(GroundedLastFrame, false);
                    machine.CurrentState = NormalGroundMotorState.Instance;
                    vel.y = 0;
                } else {
                    entity.SetVariable(GroundedLastFrame, true);
                }
            } else {
                var raycast = horizontal.Value;
                var angle = Mathf.Abs(Vector2.Angle(raycast.normal, Vector2.up));
                var slopeLimit = config.MaxAngle;
                //var skinWidth = entity.SkinWidth;
                if (angle < slopeLimit && down) {
                    var slopeModifier = config.SlopeSpeedMultiplier.Evaluate(angle);
                    // apply the slopeModifier to slow our movement up the slope
                    if (vel.x < 0) {
                        //if was stopped, apply extra vel
                        var provider = entity.InputProvider;
                        var hasProvider = provider != null;
                        var xInput = hasProvider ? provider.GetHorizontal() : 0;
                        if (hasProvider && provider.GetWalk()) {
                            xInput /= 2;
                        }

                        vel.x += entity.AccelerationCurve.Evaluate(entity.SpeedPercent) * xInput;
                    }

                    vel.x *= slopeModifier;
                    vel.y = Mathf.Abs(Mathf.Tan(angle * Mathf.Deg2Rad) * vel.x);
                } else {
                    vel.x = 0;
                    machine.CurrentState = NormalGroundMotorState.Instance;
                }
            }

            vel.x *= entity.SpeedMultiplier;
            entity.Velocity = vel;
        }

        private RaycastHit2D CheckVerticalSlope(MovableEntity entity, Bounds2D skinBounds, Vector2 vel,
            LayerMask layerMask) {
            var dir = Math.Sign(vel.x);


            var min = skinBounds.Min;
            var xOrigin = min.x;
            if (dir == -1) {
                var width = skinBounds.Size.x;
                var total = (entity.Motor.VerticalRays - 1);
                var spacing = width / total;
                xOrigin += total * spacing;
            }

            var gravity = ((GroundMotorConfig) entity.Config).SlopeGroundCheckLength;
            return Physics2D.Raycast(
                new Vector2(xOrigin, min.y + entity.SkinWidth),
                new Vector2(0, -gravity),
                gravity,
                layerMask
            );
        }
    }
}