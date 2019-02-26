using System;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Entities.Motor.States.Terrestrial {
    public static class TerrestrialMovements {
        [Serializable]
        public struct TerrestrialMovementData {
            public float OposingForceMultiplier;
        }

        public delegate void Mover(ref Vector2 velocity);

        public static void Move(
            ref Vector2 velocity, MovableEntity entity, TerrestrialConfig config,
            TerrestrialMovementData movementData, Mover beforeVel = null
        ) {
            var animator = entity.AnimatorUpdater;
            var provider = entity.InputProvider;
            var hasProvider = provider != null;
            var xInput = hasProvider ? provider.GetHorizontal() : 0;

            var inputDir = Math.Sign(xInput);
            if (hasProvider && provider.GetAttack().Consume()) {
                animator.TriggerAttack();
            }

            beforeVel?.Invoke(ref velocity);
            var velDir = Math.Sign(velocity.x);
            var curve = config.Acceleration;
            var maxSpeed = config.MaxSpeed * entity.SpeedMultiplier.Value;
            var speedPercent = entity.GetSpeedPercentage();
            var rawAcceleration = curve.Evaluate(speedPercent) * entity.SpeedMultiplier.Value;
            var acceleration = rawAcceleration * inputDir;

            var speed = Mathf.Abs(velocity.x);
            //Check acceleration if (is stopped or (input is not empty and input is same dir as vel))
            if (velDir == 0 || velDir == inputDir && inputDir != 0) {
                velocity.x += acceleration;
            } else {
                var deceleration = curve.Evaluate(1 - speedPercent);
                if (Mathf.Abs(xInput) > 0) {
                    //Changing direction, Double deacceleration
                    var d = deceleration * inputDir;
                    if (entity.CollisionStatus.Down) {
                        d *= movementData.OposingForceMultiplier;
                    }

                    velocity.x += d;
                } else {
                    //Not inputting
                    if (speed < rawAcceleration) {
                        velocity.x = 0;
                    } else {
                        velocity.x += deceleration * -velDir;
                    }
                }
            }

            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        }
    }
}