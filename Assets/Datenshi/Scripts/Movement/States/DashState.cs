using System;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Movement.Config;
using Sirenix.OdinInspector;
using UnityEngine;
using UPM;
using UPM.Motors;
using UPM.Motors.Config;
using UPM.Motors.States;
using UPM.Physics;

namespace Datenshi.Scripts.Movement.States {
    public class DashState : State {
        public static readonly Variable<bool> Dashing = new Variable<bool>("entity.motors.states.dash.dashing", false);

        public static readonly Variable<float> Duration =
            new Variable<float>("entity.motors.states.dash.dashDuration", 0);

        public static readonly VerticalPhysicsCheck VerticalVelocityCheck = new VerticalPhysicsCheck();
        public static readonly HorizontalPhysicsCheck HorizontalVelocityCheck = new HorizontalPhysicsCheck();


        public static readonly PhysicsBehaviour DashBehaviour = new PhysicsBehaviour(
            VerticalVelocityCheck,
            HorizontalVelocityCheck
        );

        public float DashDistance = 10;
        public float DashDuration = 1;

#if UNITY_EDITOR

        [ShowInInspector]
        public float MetersPerSecond {
            get {
                return DashDistance / DashDuration;
            }
            set {
                DashDistance = DashDuration / value;
            }
        }

        [ShowInInspector]
        public float SecondPerMeter {
            get {
                return DashDuration / DashDistance;
            }
            set {
                DashDuration = DashDistance / value;
            }
        }
#endif
        public State DefaultState;
        public string DashAnimatorKey = "Dashing";

        public override void Move(IMovable user, ref Vector2 velocity, ref CollisionStatus collisionStatus,
            StateMotorMachine machine, StateMotorConfig config, LayerMask collisionMask) {
            var entity = user as MovableEntity;
            if (entity == null) {
                return;
            }

            var c = entity.GetMotorConfig<DatenshiGroundConfig>();
            if (c == null) {
                return;
            }

            var m = entity.MiscController;
            var g = m == null ? null : m.GhostingContainer;
            c.DashEllegible = false;
            var speed = DashDistance / DashDuration;
            var provider = user.InputProvider;
            if (!entity.GetVariable(Dashing)) {
                if (g != null) {
                    g.Spawning = true;
                }

                entity.AnimatorUpdater.SetBool(DashAnimatorKey, true);
                entity.SetVariable(Dashing, true);
                var dir = Math.Sign(provider.GetHorizontal());
                if (dir == 0) {
                    dir = entity.CurrentDirection.X;
                }

                velocity = new Vector2(dir * speed, 0);
            }

            c.AddDashDuration();
            var duration = c.CurrentDashDuration;
            DashBehaviour.Check(user, ref velocity, ref collisionStatus, collisionMask);
            if (!HorizontalVelocityCheck.LastHit.HasValue && provider.GetButton((int) Actions.Dash) &&
                duration < DashDuration) {
                return;
            }

            if (g != null) {
                g.Spawning = false;
            }

            entity.AnimatorUpdater.SetBool(DashAnimatorKey, false);
            c.ResetDash();
            entity.SetVariable(Dashing, false);
            machine.State = DefaultState;
        }
    }
}