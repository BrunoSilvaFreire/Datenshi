using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor {
    [CreateAssetMenu(menuName = "Datenshi/Motors/AerialMotor")]
    public class AerialMotor : Motor {
        public override void Move(MovableEntity entity) {
            var input = entity.InputProvider.GetInputVector();
            var vel = entity.Velocity;
            var config = entity.MovementConfig;
            if (input.magnitude > 0) {
                vel += input * config.VerticalForce;
            } else {
                vel -= Vector2.ClampMagnitude(vel.normalized * config.VerticalForce, vel.magnitude);
            }

            vel = Vector2.ClampMagnitude(vel, config.MaxSpeed);
            entity.Velocity = vel;
        }
    }
}