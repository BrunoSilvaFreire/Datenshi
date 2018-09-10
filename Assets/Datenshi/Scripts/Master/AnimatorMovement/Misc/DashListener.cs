using Datenshi.Scripts.Master.AnimatorMovement.States;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement.Misc {
    public class DashListener : StateMachineBehaviour {
        [ShowInInspector, ReadOnly]
        private RigidEntity entity;

        public string DashingKey = "Dashing";

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            if (entity == null) {
                entity = animator.GetComponentInParent<RigidEntity>();
            }

            if (entity.GetConfig<GroundedAnimatorConfig>().DashEllegible && entity.InputProvider.GetDash()) {
                animator.SetBool(DashingKey, true);
            }
        }
    }
}