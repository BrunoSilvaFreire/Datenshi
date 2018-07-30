using System;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Movement.Config;
using UnityEngine;
using UPM;
using UPM.Motors;
using UPM.Motors.Config;
using UPM.Motors.States;
using UPM.Physics;

namespace Datenshi.Scripts.Movement.States {
    public class WallClimbState : State {
        private static readonly Variable<float> SecondsSinceLeaveWall =
            new Variable<float>("user.motor.wall.leftWallFor", 0);

        public GroundedState NormalState;
        public float SlideSpeed;
        public static readonly VerticalPhysicsCheck VerticalVelocityCheck = new VerticalPhysicsCheck();
        public static readonly HorizontalPhysicsCheck HorizontalVelocityCheck = new HorizontalPhysicsCheck();

        public static readonly PhysicsBehaviour WallBehaviour = new PhysicsBehaviour(
            HorizontalVelocityCheck,
            VerticalVelocityCheck
        );

        public void DoWallClimb(ref Vector2 vel, DatenshiGroundConfig config, int inputDir) {
            vel.y = config.JumpForce;
            vel.x = -inputDir * config.WallClimbCounterForce;
        }

        public override void Move(
            IMovable user,
            ref Vector2 vel,
            ref CollisionStatus collStatus,
            StateMotorMachine machine,
            StateMotorConfig c,
            LayerMask collisionMask) {
            var config = c as DatenshiGroundConfig;
            var e = user as MovableEntity;

            var provider = user.InputProvider as DatenshiInputProvider;
            if (provider == null || e == null || config == null) {
                machine.State = NormalState;
                Debug.LogErrorFormat("Need these 3 to not be null in wall climb state: {0} || {1} || {2}", provider, e,
                    config);
                return;
            }

            var xInput = provider.GetHorizontal();

            var inputDir = Math.Sign(xInput);
            var wallDir = -collStatus.HorizontalCollisionDir;
            //Check is is leaving wall
            if (inputDir != wallDir) {
                var sinceLeft = e.GetVariable(SecondsSinceLeaveWall);
                var margin = config.OffWallTimeMargin;
                if (sinceLeft >= margin) {
                    e.SetVariable(SecondsSinceLeaveWall, 0);
                    machine.State = NormalState;
                    return;
                }

                if (provider.GetJumpDown()) {
                    vel.y = config.JumpForce;
                    vel.x = -wallDir * config.WallClimbCounterForce;
                    user.Velocity = vel;
                    e.SetVariable(SecondsSinceLeaveWall, 0);
                    machine.State = NormalState;
                    ;
                } else {
                    sinceLeft += Time.unscaledDeltaTime;
                    e.SetVariable(SecondsSinceLeaveWall, sinceLeft);
                    var gravity = GameResources.Instance.Gravity;
                    NormalState.ExecuteState(user, machine, ref vel, ref collStatus, collisionMask, gravity, this);
                }

                return;
            }

            var velDir = Math.Sign(vel.x);
            var rawAcceleration = user.AccelerationCurve.Evaluate(user.SpeedPercent);
            var acceleration = rawAcceleration * inputDir;
            var speed = Mathf.Abs(vel.x);
            vel.x += acceleration;
            var deacceleration = user.AccelerationCurve.Evaluate(1 - user.SpeedPercent);
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

            WallBehaviour.Check(user, ref vel, ref collStatus, collisionMask);
            if (!collStatus.Down && (collStatus.Left || collStatus.Right) && wallDir == inputDir) {
                //Sliding down
                if (provider.GetJumpDown()) {
                    DoWallClimb(ref vel, config, inputDir);
                    machine.State = NormalState;
                } else {
                    vel.y = SlideSpeed * e.DeltaTime;
                }

                user.Velocity = vel;
            } else {
                machine.State = NormalState;
            }
        }
    }
}