using System;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using UnityEngine;
using UPM;
using UPM.Motors;
using UPM.Motors.Config;
using UPM.Motors.States;
using UPM.Physics;
using UPM.Util;

namespace Datenshi.Scripts.Movement.States {
    public class GroundedState : State {
        public State OnHitWallAir;
        public static readonly VerticalPhysicsCheck VerticalVelocityCheck = new VerticalPhysicsCheck();
        public static readonly HorizontalPhysicsCheck HorizontalVelocityCheck = new HorizontalPhysicsCheck();
        public static readonly VerticalPhysicsCheck SlopeCheck = new VerticalPhysicsCheck(SlopeCheckProvider);

        public static readonly PhysicsBehaviour GroundedBehaviour = new PhysicsBehaviour(
            VerticalVelocityCheck,
            HorizontalVelocityCheck,
            SlopeCheck
        );

        private static Vector2 SlopeCheckProvider(IMovable arg) {
            return Vector2.down * Time.fixedDeltaTime;
        }


        public override void Move(IMovable user, ref Vector2 velocity, ref CollisionStatus collisionStatus,
            StateMotorMachine machine, StateMotorConfig config1, LayerMask collisionMask) {
            var gravity = GameResources.Instance.Gravity;
            ExecuteState(user, machine, ref velocity, ref collisionStatus, collisionMask, gravity, OnHitWallAir);
        }

        public static void ExecuteState(IMovable user, StateMotorMachine machine, ref Vector2 velocity,
            ref CollisionStatus collisionStatus, LayerMask collisionMask, float gravity, State onHitAir) {
            var config = user.GetMotorConfig<GroundMotorConfig>();
            if (config == null) {
                UPMDebug.LogWarning("Expected GroundMotorConfig for GroundedState @ " + user);
                return;
            }

            int dir;
            ProcessInputs(user, config, ref velocity, ref collisionStatus, gravity, out dir);
            var max = user.MaxSpeed;
            var u = user as IDatenshiMovable;
            if (u != null) {
                max *= u.SpeedMultiplier;
            }

            Debug.Log("Max = " + max);
            velocity.x = Mathf.Clamp(velocity.x, -max, max);
            GroundedBehaviour.Check(user, ref velocity, ref collisionStatus, collisionMask);
            if (velocity.y < 0 && IsRunningTowardsWall(SlopeCheck.LastHit, collisionStatus, dir)) {
                machine.State = onHitAir;
            }
        }

        private static bool IsRunningTowardsWall(RaycastHit2D? down, CollisionStatus collStatus, int xDir) {
            return (!down.HasValue || !down.Value) && (collStatus.Left || collStatus.Right) &&
                   collStatus.HorizontalCollisionDir == xDir;
        }

        private static void ProcessInputs(IMovable user, GroundMotorConfig config, ref Vector2 velocity,
            ref CollisionStatus collisionStatus, float gravity, out int dir) {
            var provider = user.InputProvider as DatenshiInputProvider;
            var hasProvider = provider != null;
            var xInput = hasProvider ? provider.GetHorizontal() : 0;
            dir = Math.Sign(xInput);
            var inputDir = Math.Sign(xInput);
            var jump = hasProvider && provider.GetJump();
            var combatant = user as ICombatant;
            if (combatant != null) {
                combatant.Focusing = hasProvider && provider.GetDefend();
                if (hasProvider && provider.GetAttack()) {
                    combatant.AnimatorUpdater.TriggerAttack();
                }
            }

            if (collisionStatus.Down) {
                if (jump) {
                    velocity.y = config.JumpForce;
                }
            } else {
                var g = gravity * config.GravityScale * Time.fixedDeltaTime;
                if (velocity.y > 0 && !jump) {
                    g *= config.JumpCutGravityModifier;
                }

                velocity.y += g;
            }

            var velDir = Math.Sign(velocity.x);
            var curve = user.AccelerationCurve;
            var speedPercent = user.SpeedPercent;
            var rawAcceleration = curve.Evaluate(speedPercent);
            var acceleration = rawAcceleration * inputDir;
            var maxSpeed = user.MaxSpeed * Mathf.Abs(xInput);
            var speed = Mathf.Abs(velocity.x);
            if (velDir == 0 || velDir == inputDir && inputDir != 0) {
                //Accelerating
                if (speed < maxSpeed) {
                    velocity.x += acceleration;
                }

                return;
            }

            var deacceleration = curve.Evaluate(1 - speedPercent);
            if (Mathf.Abs(xInput) > 0) {
                //Changing direction, Double deacceleration
                velocity.x += deacceleration * inputDir * 2;
                return;
            }

            //Not inputting
            if (speed < rawAcceleration) {
                velocity.x = 0;
            } else {
                velocity.x += deacceleration * -velDir;
            }
        }
    }
}