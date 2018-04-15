using System;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors.State.Ground {
    public class WallClimbingState : GroundMotorState {
        public static readonly WallClimbingState Instance = new WallClimbingState();

        private static readonly Variable<float> SecondsSinceLeaveWall =
            new Variable<float>("entity.motor.wall.leftWallFor", 0);

        private WallClimbingState() { }

        public override void Execute(MovableEntity entity, MotorStateMachine<GroundMotorState> machine,
            ref CollisionStatus collStatus) {
            var vel = entity.Velocity;
            var provider = entity.InputProvider;
            if (provider == null) {
                machine.SetState(entity, ref collStatus, NormalGroundMotorState.Instance);
                return;
            }

            var xInput = provider.GetHorizontal();


            var inputDir = Math.Sign(xInput);
            var config = (GroundMotorConfig) entity.Config;
            var wallDir = collStatus.HorizontalCollisionDir;
            if (inputDir != wallDir) {
                var sinceLeft = entity.GetVariable(SecondsSinceLeaveWall);
                var margin = config.OffWallTimeMargin;
                Debug.Log("Since left = " + sinceLeft + " / " + margin);
                if (sinceLeft >= margin) {
                    entity.SetVariable(SecondsSinceLeaveWall, 0);
                    machine.SetState(entity, ref collStatus, NormalGroundMotorState.Instance);
                    return;
                }

                if (provider.GetJumpDown()) {
                    vel.y = entity.YForce;
                    vel.x = -wallDir * config.WallClimbCounterForce;
                    Debug.Log("Jumping @ " + vel);
                    entity.Velocity = vel;
                    entity.SetVariable(SecondsSinceLeaveWall, 0);
                    machine.SetState(entity, ref collStatus, NormalGroundMotorState.Instance);
                    ;
                } else {
                    Debug.Log("Not jumping");
                    sinceLeft += Time.deltaTime;
                    entity.SetVariable(SecondsSinceLeaveWall, sinceLeft);
                    NormalGroundMotorState.ExecuteState(entity, machine, ref collStatus);
                }

                return;
            }

            var velDir = Math.Sign(vel.x);
            var rawAcceleration = entity.AccelerationCurve.Evaluate(entity.SpeedPercent);
            var acceleration = rawAcceleration * inputDir;
            var speed = Mathf.Abs(vel.x);
            vel.x += acceleration;

            var deacceleration = entity.AccelerationCurve.Evaluate(1 - entity.SpeedPercent);
            if (Mathf.Abs(xInput) > 0) {
                //Changing direction, Double deacceleration
                vel.x += deacceleration * inputDir * 2;
            } else {
                //Not inputting
                if (speed < rawAcceleration) {
                    vel.x = 0;
                } else {
                    vel.x += deacceleration * -velDir;
                }
            }

            RaycastHit2D? hit;
            PhysicsUtil.DoPhysics(entity, ref vel, ref collStatus, out hit);
            if (!collStatus.Down && (collStatus.Left || collStatus.Right) &&
                collStatus.HorizontalCollisionDir == inputDir) {
                //Sliding down
                if (provider.GetJumpDown()) {
                    vel.y = entity.YForce;
                    vel.x = -inputDir * config.WallClimbCounterForce;
                    machine.CurrentState = NormalGroundMotorState.Instance;
                } else {
                    vel.y = GameResources.Instance.Gravity * entity.GravityScale * Time.deltaTime *
                            config.WallClimbGravityScale;
                }

                entity.Velocity = vel;
            } else {
                machine.SetState(entity, ref collStatus, NormalGroundMotorState.Instance);
            }
        }
    }
}