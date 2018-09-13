using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement {
    public abstract class AnimatorMovement : StateMachineBehaviour {
        public abstract void Initialize(RigidEntity owner);
    }

    public abstract class AnimatorMovement<T> : AnimatorMovement where T : RigidEntityConfig {
        [ShowInInspector, ReadOnly, SerializeField]
        private RigidEntity entity;

        public override void Initialize(RigidEntity owner) {
            entity = owner;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (entity == null) {
                return;
            }

            Move(animator);
        }

        private void Move(Animator animator) {
            var vel = entity.Rigidbody.velocity;
            Move(ref vel, entity, entity.GetConfig<T>(), animator);
            entity.Rigidbody.velocity = vel;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            if (entity == null) {
                return;
            }

            OnEnter(entity, entity.GetConfig<T>());
            Move(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            if (entity == null) {
                return;
            }

            OnExit(entity, entity.GetConfig<T>(), animator);
        }

        protected virtual void OnEnter(RigidEntity entity, T config) { }
        protected virtual void OnExit(RigidEntity entity, T config, Animator animator) { }
        protected abstract void Move(ref Vector2 velocity, RigidEntity entity, T config, Animator animator);
    }
}