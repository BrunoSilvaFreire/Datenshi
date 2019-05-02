using Datenshi.Scripts.Data;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States.Terrestrial {
    public class GroundedMovement : MovementState<TerrestrialConfig> {
        public float GravityScale = 1;
        public TerrestrialMovements.TerrestrialMovementData MovementData;
        public AerialMovement AerialMovement;

        protected override void OnEnter(MovableEntity entity, StateMotor motor, TerrestrialConfig config) {
            entity.Rigidbody.gravityScale = GravityScale;
        }

        protected override void Move(ref Vector2 velocity, MovableEntity entity, TerrestrialConfig config,
            StateMotor motor) {
            TerrestrialMovements.Move(ref velocity, entity, config, MovementData, (ref Vector2 refVel) => {
                var provider = entity.InputProvider;
                var shouldJump = provider != null && provider.GetJump().Consume();
                if (shouldJump && entity.JumpEllegible()) {
                    //Grounded and should jump
                    refVel.y = config.VerticalForce;
                }
            });
            if (!entity.CollisionStatus.Down) {
                Debug.Log("New state is Aerial");
                motor.SetState(entity, AerialMovement);
            }
        }
    }
}