using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Movement.Config;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States {
    public class StateMovementConfig : MovementConfig {
        public MovementState CurrentState;
    }

    [CreateAssetMenu(menuName = "Datenshi/Motors/StateMotor")]
    public class StateMotor : Motor {
        public List<MovementState> States = new List<MovementState>();

        public MovementState GetActiveState(MovableEntity entity, bool force = true) {
            var activeState = entity.GetMovementConfigAs<StateMovementConfig>().CurrentState;
            if (activeState == null && force) {
                SetState(entity, activeState = States.First());
            }

            return activeState;
        }

        public void SetState(MovableEntity entity, MovementState newState) {
            var config = entity.GetMovementConfigAs<StateMovementConfig>();
            var activeState = config.CurrentState;
            if (activeState != null && activeState == newState) {
                return;
            }

            if (activeState != null) {
                activeState.Exit(entity, this);
            }

            config.CurrentState = newState;
            if (newState != null) {
                newState.Enter(entity, this);
            }
        }

        public override void Move(MovableEntity entity) {
            var state = GetActiveState(entity);
            if (state != null) {
                state.Move(entity, this);
            }
        }
    }

    public abstract class MovementListener : ScriptableObject {
        public abstract void Tick(MovableEntity entity, StateMotor motor);
    }

    public abstract class MovementState : ScriptableObject {
        [SerializeField, HideInInspector]
        public List<MovementListener> Listeners;

        public virtual void Enter(MovableEntity entity, StateMotor motor) { }
        public virtual void Exit(MovableEntity entity, StateMotor motor) { }
        protected abstract void Tick(MovableEntity entity, StateMotor motor);

        public void Move(MovableEntity entity, StateMotor motor) {
            Tick(entity, motor);
            foreach (var listener in Listeners) {
                listener.Tick(entity, motor);
            }
        }
    }

    public abstract class MovementState<T> : MovementState where T : MovementConfig {
        public sealed override void Enter(MovableEntity entity, StateMotor motor) {
            Enter(entity, motor, entity.GetMovementConfigAs<T>());
        }

        public sealed override void Exit(MovableEntity entity, StateMotor motor) {
            Exit(entity, motor, entity.GetMovementConfigAs<T>());
        }

        protected virtual void Enter(MovableEntity entity, StateMotor motor, T config) { }
        protected virtual void Exit(MovableEntity entity, StateMotor motor, T config) { }

        protected override void Tick(MovableEntity entity, StateMotor motor) {
            var vel = entity.Velocity;
            Move(ref vel, entity, entity.GetMovementConfigAs<T>(), motor);
            entity.Velocity = vel;
        }

        protected abstract void Move(ref Vector2 velocity, MovableEntity entity, T config, StateMotor motor);
    }
}