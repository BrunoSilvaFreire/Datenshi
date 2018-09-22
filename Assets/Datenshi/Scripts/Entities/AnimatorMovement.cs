using Datenshi.Scripts.Movement.Config;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public abstract class AnimatorMovement : StateMachineBehaviour {
        public abstract void Initialize(MovableEntity owner);
    }

    public abstract class AnimatorMovement<T> : AnimatorMovement where T : MovementConfig {
        [ShowInInspector, ReadOnly, SerializeField]
        private MovableEntity entity;

        public override void Initialize(MovableEntity owner) {
            entity = owner;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (entity == null) {
                return;
            }

            Move(animator);
        }

        private void Move(Animator animator) {
            var vel = entity.Velocity;
            Move(ref vel, entity, entity.GetMovementConfigAs<T>(), animator);
            entity.Velocity = vel;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            if (entity == null) {
                return;
            }

            OnEnter(entity, entity.GetMovementConfigAs<T>());
            Move(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            if (entity == null) {
                return;
            }

            OnExit(entity, entity.GetMovementConfigAs<T>(), animator);
        }

        protected virtual void OnEnter(MovableEntity entity, T config) { }
        protected virtual void OnExit(MovableEntity entity, T config, Animator animator) { }
        protected abstract void Move(ref Vector2 velocity, MovableEntity entity, T config, Animator animator);
    }
}