using System;
using Datenshi.Scripts.Controller;
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
            PhysicsUtil.DoPhysics(entity, ref vel, ref collStatus);
            vel.x *= entity.SpeedMultiplier;
            entity.Velocity = vel;
        }

        private static void ProcessInputs(ref Vector2 vel, MovableEntity entity, MotorStateMachine<GroundMotorState> machine) {
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
                if (provider.GetJump()) {
                    vel.y += entity.YForce;
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