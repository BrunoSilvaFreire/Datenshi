using System;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Movement.Config;
using Datenshi.Scripts.Util;
using UnityEngine;
using UPM;
using UPM.Motors;
using UPM.Motors.Config;
using UPM.Motors.States;
using UPM.Physics;
using UPM.Util;

namespace Datenshi.Scripts.Movement.States {
    public class GroundedState : State {
        public WallClimbState WallClimbState;
        public float ExtraWallClimbLengthCheck = .5F;

        public static readonly VerticalPhysicsCheck VerticalVelocityCheck = new VerticalPhysicsCheck();
        public static readonly HorizontalPhysicsCheck HorizontalVelocityCheck = new HorizontalPhysicsCheck();
        public static readonly VerticalPhysicsCheck SlopeCheck = new VerticalPhysicsCheck(SlopeCheckProvider);


        public static readonly PhysicsBehaviour GroundedBehaviour = new PhysicsBehaviour(
            VerticalVelocityCheck,
            HorizontalVelocityCheck,
            SlopeCheck
        );

        // Wall check
        private int wallClimbExtraCheckDir = 0;
        private readonly HorizontalPhysicsCheck wallClimbExtraCheck;
        public State DashState;

        public GroundedState() {
            wallClimbExtraCheck = new HorizontalPhysicsCheck(WallClimbExtraChecker);
        }

        private Vector2 WallClimbExtraChecker(IMovable movable) {
            return Vector2.right * wallClimbExtraCheckDir * ExtraWallClimbLengthCheck;
        }

        private static Vector2 SlopeCheckProvider(IMovable arg) {
            return Vector2.down * Time.deltaTime;
        }


        public override void Move(
            IMovable user,
            ref Vector2 velocity,
            ref CollisionStatus collisionStatus,
            StateMotorMachine machine,
            StateMotorConfig config1,
            LayerMask collisionMask) {
            var gravity = GameResources.Instance.Gravity;
            ExecuteState(user, machine, ref velocity, ref collisionStatus, collisionMask, gravity, WallClimbState);
        }

        public void ExecuteState(
            IMovable u,
            StateMotorMachine machine,
            ref Vector2 velocity,
            ref CollisionStatus collisionStatus,
            LayerMask collisionMask,
            float gravity,
            WallClimbState wallClimbState) {
            var user = u as IDatenshiMovable;
            if (user == null) {
                return;
            }

            var config = user.GetMotorConfig<DatenshiGroundConfig>();
            if (config == null) {
                UPMDebug.LogWarning("Expected DatenshiGroundConfig for GroundedState @ " + user);
                return;
            }

            if (CheckStateChange(user, config, machine)) {
                return;
            }

            int dir;
            var bounds = (Bounds2D) user.Hitbox.bounds;
            var shrinkedBounds = bounds;
            shrinkedBounds.Expand(user.Inset * -2);
            ProcessInputs(
                user,
                config,
                ref velocity,
                ref collisionStatus,
                gravity,
                out dir,
                collisionMask,
                bounds,
                shrinkedBounds,
                wallClimbState);

            var max = user.MaxSpeed;
            if (collisionStatus.Down) {
                max *= user.SpeedMultiplier;                
            }
            velocity.x = Mathf.Clamp(velocity.x, -max, max);

            GroundedBehaviour.Check(user, ref velocity, ref collisionStatus, collisionMask);
            if (velocity.y < 0 && IsRunningTowardsWall(SlopeCheck.LastHit, collisionStatus, dir)) {
                machine.State = wallClimbState;
            }
        }

        private bool CheckStateChange(IDatenshiMovable user, DatenshiGroundConfig c, StateMotorMachine machine) {
            if (user.CollisionStatus.HasAny()) {
                if (!c.DashEllegible) {
                    c.DashEllegible = true;
                }
            }

            if (c.DashEllegible && user.InputProvider.GetButtonDown((int) Actions.Dash)) {
                machine.State = DashState;
                return true;
            }

            return false;
        }

        private static bool IsRunningTowardsWall(RaycastHit2D? down, CollisionStatus collStatus, int xDir) {
            return (!down.HasValue || !down.Value) && (collStatus.Left || collStatus.Right) &&
                   collStatus.HorizontalCollisionDir == xDir;
        }

        private void ProcessInputs(
            IDatenshiMovable user,
            DatenshiGroundConfig config,
            ref Vector2 velocity,
            ref CollisionStatus collisionStatus,
            float gravity,
            out int dir,
            LayerMask collisionMask,
            Bounds2D bounds,
            Bounds2D shrinkedBounds,
            WallClimbState wallClimbState) {
            var provider = user.InputProvider as DatenshiInputProvider;
            var hasProvider = provider != null;
            var xInput = hasProvider ? provider.GetHorizontal() : 0;
            /*
            // Let's leave this away for now
            var d = user.Direction;
            var newX = Direction.DirectionValue.FromVector(xInput);
            if (newX != Direction.DirectionValue.Zero) {
                d.X = newX;
            }

            user.Direction = d;*/
            dir = Math.Sign(xInput);
            var jump = hasProvider && provider.GetJump();
            var combatant = user as ICombatant;
            if (combatant != null) {
                combatant.Focusing = hasProvider && provider.GetDefend();
                if (hasProvider && provider.GetAttack()) {
                    combatant.AnimatorUpdater.TriggerAttack();
                }
            }

            if (jump) {
                if (collisionStatus.Down) {
                    velocity.y = config.JumpForce;
                } else {
                    var jumpDown = provider.GetJumpDown();
                    if (jumpDown) {
                        var a = user.GroundPosition;
                        var b = a;
                        b.y -= config.RejumpLength;
                        var c = Physics2D.Linecast(a, b, collisionMask);
#if UNITY_EDITOR
                        Debug.DrawLine(a, b, c ? Color.green : Color.red);
#endif
                        if (c) {
                            //Succeded jump call
                            velocity.y = config.JumpForce;
                        } else if (dir != 0) {
                            // Check for wall climb call
                            var valid = false;
                            var vel = velocity;
                            DoExtraWallClimbCheck(
                                1,
                                user,
                                ref vel,
                                ref collisionStatus,
                                collisionMask,
                                bounds,
                                shrinkedBounds,
                                config,
                                wallClimbState);
                            valid = wallClimbExtraCheck.LastHit.HasValue;
                            if (!valid) {
                                DoExtraWallClimbCheck(
                                    -1,
                                    user,
                                    ref vel,
                                    ref collisionStatus,
                                    collisionMask,
                                    bounds,
                                    shrinkedBounds,
                                    config,
                                    wallClimbState);
                                valid = wallClimbExtraCheck.LastHit.HasValue;
                            }

                            if (valid) {
                                velocity = vel;
                            }
                        }
                    }
                }
            }

            if (!collisionStatus.Down) {
                var g = gravity * config.GravityScale * Time.deltaTime;
                if (velocity.y > 0 && !jump) {
                    g *= config.JumpCutGravityModifier;
                }

                velocity.y += g;
            }

            var velDir = Math.Sign(velocity.x);
            var curve = user.AccelerationCurve;
            var speedPercent = user.SpeedPercent;
            var rawAcceleration = curve.Evaluate(speedPercent);
            var acceleration = rawAcceleration * dir;
            var maxSpeed = user.MaxSpeed * Mathf.Abs(xInput);
            var speed = Mathf.Abs(velocity.x);
            if (velDir == 0 || velDir == dir && dir != 0) {
                //Accelerating
                if (speed < maxSpeed) {
                    velocity.x += acceleration;
                }

                return;
            }

            var deacceleration = curve.Evaluate(1 - speedPercent);
            if (Mathf.Abs(xInput) > 0) {
                //Changing direction, Double deacceleration
                velocity.x += deacceleration * dir * 2;
                return;
            }

            //Not inputting
            if (speed < rawAcceleration) {
                velocity.x = 0;
            } else {
                velocity.x += deacceleration * -velDir;
            }
        }

        private void DoExtraWallClimbCheck(
            int dir,
            IMovable user,
            ref Vector2 velocity,
            ref CollisionStatus collisionStatus,
            LayerMask collisionMask,
            Bounds2D bounds,
            Bounds2D shrinkedBounds,
            DatenshiGroundConfig config,
            WallClimbState wallClimbState) {
            var csCopy = collisionStatus;
            wallClimbExtraCheckDir = dir;
            wallClimbExtraCheck.Check(user, ref velocity, ref csCopy, collisionMask, bounds, shrinkedBounds);
            if (!wallClimbExtraCheck.LastHit.HasValue) {
                return;
            }

            collisionStatus = csCopy;
            wallClimbState.DoWallClimb(ref velocity, config, dir);
        }
    }
}