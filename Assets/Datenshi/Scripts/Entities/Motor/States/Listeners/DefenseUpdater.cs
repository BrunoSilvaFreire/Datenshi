using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States.Listeners {
    public class DefenseUpdater : MovementListener {
        public override void OnTick(MovableEntity entity, StateMotor motor) {
            var provider = entity.InputProvider;
            var hasProvider = provider != null;
            entity.Defending = hasProvider && provider.GetFocus();
        }
    }
}