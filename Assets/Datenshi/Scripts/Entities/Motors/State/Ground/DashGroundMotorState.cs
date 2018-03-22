using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors.State.Ground {
    public class DashGroundMotorState : GroundMotorState {
        public static readonly Variable<bool> Dashing = new Variable<bool>("entity.motor.ground.dashing", false);
        public static readonly Variable<Vector2> DashDirection = new Variable<Vector2>("entity.motor.ground.dashDirection", Vector2.zero);
        public static readonly Variable<float> DashStart = new Variable<float>("entity.motor.ground.dashStart", 0);

        public static readonly DashGroundMotorState Instance = new DashGroundMotorState();
        private DashGroundMotorState() { }

        public override void Execute(MovableEntity entity, MotorStateMachine<GroundMotorState> machine, ref CollisionStatus collStatus) {
            var dashing = entity.GetVariable(Dashing);
            var config = entity.Config as GroundMotorConfig;
            float dashDuration, dashDistance;
            if (config == null) {
                dashDuration = GroundMotorConfig.DefaultDuration;
                dashDistance = GroundMotorConfig.DefaultDistance;
            } else {
                dashDuration = config.DashDuration;
                dashDistance = config.DashDistance;
            }

            var time = Time.time;
            Vector2 dir;
            if (!dashing) {
                entity.Invulnerable = true;
                dir = Direction.FromVector(entity.InputProvider.GetInputVector());
                entity.SetVariable(DashStart, time);
                entity.SetVariable(DashDirection, dir);
                entity.SetVariable(Dashing, true);
            } else {
                var start = entity.GetVariable(DashStart);
                dir = entity.GetVariable(DashDirection);
                if (time - start > dashDuration) {
                    entity.Invulnerable = false;
                    entity.SetVariable(Dashing, false);
                    entity.Velocity = dir * entity.MaxSpeed;
                    machine.CurrentState = NormalGroundMotorState.Instance;
                    return;
                }
            }
            dir *= dashDistance / dashDuration;
            RaycastHit2D? hit;
            PhysicsUtil.DoPhysics(entity, ref dir, ref collStatus, out hit);
            entity.Velocity = dir;
        }
    }
}