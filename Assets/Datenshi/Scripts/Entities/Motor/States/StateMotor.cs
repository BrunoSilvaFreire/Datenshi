using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Movement.Config;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States {
    [CreateAssetMenu(menuName = "Datenshi/Motors/StateMotor")]
    public class StateMotor : Motor {
        public List<MovementState> States = new List<MovementState>();
        private MovementState activeState;

        public MovementState ActiveState {
            get {
                if (activeState == null) {
                    return activeState = States.First();
                }

                return activeState;
            }
        }

        public void SetState(MovementState defaultState) { }

        public override void Move(MovableEntity entity) {
            var state = ActiveState;
            if (state != null) {
                state.Move(entity, this);
            }
        }
    }

    public abstract class MovementState : ScriptableObject {
        public abstract void Move(MovableEntity entity, StateMotor motor);
    }

    public abstract class MovementState<T> : MovementState where T : MovementConfig {
        public override void Move(MovableEntity entity, StateMotor motor) {
            var vel = entity.Velocity;
            Move(ref vel, entity, entity.GetMovementConfigAs<T>(), motor);
            entity.Velocity = vel;
        }

        protected virtual void OnEnter(MovableEntity entity, T config) { }
        protected virtual void OnExit(MovableEntity entity, T config, Animator animator) { }

        protected abstract void Move(ref Vector2 velocity, MovableEntity entity, T config, StateMotor motor);
    }
}