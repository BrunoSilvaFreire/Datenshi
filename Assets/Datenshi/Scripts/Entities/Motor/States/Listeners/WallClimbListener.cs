using System;

namespace Datenshi.Scripts.Entities.Motor.States.Listeners {
    public class WallClimbListener : MovementListener {
        public MovementState Destination;

        public override void OnTick(MovableEntity entity, StateMotor motor) {
            var s = entity.CollisionStatus;
            /*if (entity.Velocity.y < 0) {
                return;
            }*/

            if (s.HasHorizontal && s.HorizontalCollisionDir == Math.Sign(entity.InputProvider.GetHorizontal()) &&
                !s.Down) {
                motor.SetState(entity, Destination);
            }
        }
    }
}