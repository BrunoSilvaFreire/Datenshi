using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States.Terrestrial {
    public class AerialMovement : MovementState<TerrestrialConfig> {
        public TerrestrialMovements.TerrestrialMovementData MovementData;
        public GroundedMovement GroundedMovement;
        public float JumpSlowdownSpeed = 0.25F;

        protected override void Move(
            ref Vector2 velocity,
            MovableEntity entity,
            TerrestrialConfig config,
            StateMotor motor
        ) {
            TerrestrialMovements.Move(ref velocity, entity, config, MovementData);
            if (!entity.InputProvider.GetJumping() && velocity.y > 0) {
                velocity.y = Mathf.Lerp(velocity.y, 0, JumpSlowdownSpeed);
            }

            if (entity.CollisionStatus.Down) {
                motor.SetState(entity, GroundedMovement);
            }
        }
    }
}