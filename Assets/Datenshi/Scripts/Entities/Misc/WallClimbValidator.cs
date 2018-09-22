using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc {
    public class WallClimbValidator : StateMachineBehaviour {
        public string WallClimbValidKey = "WallClimbValid";

        [ShowInInspector, ReadOnly]
        private MovableEntity entity;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (entity == null) {
                entity = animator.GetComponentInParent<MovableEntity>();
            }

            UpdateState(animator);
        }

        private void UpdateState(Animator animator) {
            var b = -entity.CollisionStatus.HorizontalCollisionDir ==
                    System.Math.Sign(entity.InputProvider.GetHorizontal());
            animator.SetBool(WallClimbValidKey, b);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            UpdateState(animator);
        }
    }
}