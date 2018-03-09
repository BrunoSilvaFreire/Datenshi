using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motors.State {
    public abstract class MotorStateMachine : MonoBehaviour {
        public abstract void Execute(MovableEntity entity, ref CollisionStatus collStatus);
    }

    public class MotorStateMachine<S> : MotorStateMachine where S : MotorState<S> {
        [ShowInInspector, ReadOnly]
        public S CurrentState {
            get;
            set;
        }

        public override void Execute(MovableEntity entity, ref CollisionStatus collStatus) {
            CurrentState.Execute(entity, this, ref collStatus);
        }
    }

    public abstract class MotorState<S> where S : MotorState<S> {
        public abstract void Execute(MovableEntity entity, MotorStateMachine<S> machine, ref CollisionStatus collStatus);
    }
}