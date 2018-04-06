using Datenshi.Scripts.Entities.Motors.State.Ground;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors {
    [CreateAssetMenu(menuName = "Datenshi/Motor/GroundMotor")]
    public class GroundMotor : Motor<GroundMotorConfig> {
        public override void Initialize(MovableEntity entity, GroundMotorConfig config) {
            entity.MovementStateMachine = entity.GetOrAddComponent<GroundMotorStateMachine>();
        }

        public override void Execute(MovableEntity entity, GroundMotorConfig config, ref CollisionStatus collStatus) {
            entity.MovementStateMachine.Execute(entity, ref collStatus);
        }

        public override void Terminate(MovableEntity entity, GroundMotorConfig config) {
            entity.MovementStateMachine = null;
        }
    }
}