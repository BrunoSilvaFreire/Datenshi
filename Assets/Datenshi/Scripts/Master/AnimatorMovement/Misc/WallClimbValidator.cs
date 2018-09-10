using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement.Misc {
    public class WallClimbValidator : StateMachineBehaviour {
        public string WallClimbValidKey = "WallClimbValid";

        [ShowInInspector, ReadOnly]
        private RigidEntity entity;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (entity == null) {
                entity = animator.GetComponentInParent<RigidEntity>();
            }

            UpdateState(animator);
        }

        private void UpdateState(Animator animator) {
            var b = -entity.CollisionStatus.HorizontalCollisionDir ==
                    System.Math.Sign(entity.InputProvider.GetHorizontal());
            Debug.Log(
                $"Checking collision for wall climb @ {-entity.CollisionStatus.HorizontalCollisionDir} =={System.Math.Sign(entity.InputProvider.GetHorizontal())}");
            animator.SetBool(WallClimbValidKey, b);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            UpdateState(animator);
        }
    }
}