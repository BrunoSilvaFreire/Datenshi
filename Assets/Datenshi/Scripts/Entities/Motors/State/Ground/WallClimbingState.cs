using System;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors.State.Ground
{
    public class WallClimbingState : GroundMotorState
    {
        public static readonly WallClimbingState Instance = new WallClimbingState();

        private WallClimbingState()
        {
        }

        public override void Execute(MovableEntity entity, MotorStateMachine<GroundMotorState> machine,
            ref CollisionStatus collStatus)
        {
            var vel = entity.Velocity;
            var provider = entity.InputProvider;
            if (provider == null)
            {
                machine.SetState(entity, ref collStatus, NormalGroundMotorState.Instance);
                return;
            }
            var xInput = provider.GetHorizontal();


            var inputDir = Math.Sign(xInput);
            if (inputDir != collStatus.HorizontalCollisionDir)
            {
                machine.SetState(entity, ref collStatus, NormalGroundMotorState.Instance);

                return;
            }
            var velDir = Math.Sign(vel.x);
            var rawAcceleration = entity.AccelerationCurve.Evaluate(entity.SpeedPercent);
            var acceleration = rawAcceleration * inputDir;
            var maxSpeed = entity.MaxSpeed * Mathf.Abs(xInput);
            var speed = Mathf.Abs(vel.x);
            vel.x += acceleration;
           
            var deacceleration = entity.AccelerationCurve.Evaluate(1 - entity.SpeedPercent);
            if (Mathf.Abs(xInput) > 0)
            {
                //Changing direction, Double deacceleration
                vel.x += deacceleration * inputDir * 2;
                return;
            }

            //Not inputting
            if (speed < rawAcceleration)
            {
                vel.x = 0;
            }
            else
            {
                vel.x += deacceleration * -velDir;
            }
            RaycastHit2D? hit;
            PhysicsUtil.DoPhysics(entity, ref vel, ref collStatus, out hit);
            if (!collStatus.Down && (collStatus.Left || collStatus.Right) &&
                collStatus.HorizontalCollisionDir == inputDir)
            {
                //Sliding down
                if (provider.GetJump())
                {
                    vel.y = entity.YForce;
                }
                else
                {
                    vel.y = GameResources.Instance.Gravity * entity.GravityScale * Time.deltaTime * ((GroundMotorConfig) entity.Config).WallClimbGravityScale;

                }
                entity.Velocity = vel;
            }
            else
            {
                machine.SetState(entity, ref collStatus, NormalGroundMotorState.Instance);
            }
        }
    }
}