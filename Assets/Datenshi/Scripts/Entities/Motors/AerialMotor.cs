using System;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors {
    [CreateAssetMenu(menuName = "Datenshi/Motor/AerialMotor")]
    public class AerialMotor : Motor {
        public override void Execute(MovableEntity entity, ref CollisionStatus collStatus) {
            var vel = entity.Velocity;

            ProcessInputs(ref vel, entity);

            var maxSpeed = entity.MaxSpeed;
            vel = Vector2.ClampMagnitude(vel, maxSpeed);
            RaycastHit2D? hit;
            PhysicsUtil.DoPhysics(entity, ref vel, ref collStatus, out hit);
            vel *= entity.SpeedMultiplier;
            entity.Velocity = vel;
        }

        private static void ProcessInputs(ref Vector2 vel, MovableEntity entity) {
            var provider = entity.InputProvider;
            if (provider == null) {
                return;
            }

            if (provider.GetAttack()) {
                entity.AnimatorUpdater.TriggerAttack();
            }

            var xInput = provider.GetHorizontal();
            var yInput = provider.GetVertical();
            var xInputDir = Math.Sign(xInput);
            var yInputDir = Math.Sign(yInput);
            var maxSpeed = entity.MaxSpeed;
            var xPercent = vel.x / maxSpeed;
            var yPercent = vel.y / maxSpeed;
            var xAcceleration = entity.AccelerationCurve.Evaluate(xPercent) * xInputDir;
            var yAcceleration = entity.AccelerationCurve.Evaluate(yPercent) * yInputDir;
            vel.x += xAcceleration;
            vel.y += yAcceleration;
            var deacceleration = entity.AccelerationCurve.Evaluate(1 - entity.SpeedPercent);
            if (Mathf.Abs(xInput) > 0 || Mathf.Abs(yInput) > 0) {
                return;
            }

            var de = deacceleration / 2;
            if (vel.magnitude <= de) {
                vel = Vector2.zero;
            } else {
                vel += de * -vel.normalized;
            }
        }

        public override void Initialize(MovableEntity entity) { }

        public override void Terminate(MovableEntity entity) { }
    }
}