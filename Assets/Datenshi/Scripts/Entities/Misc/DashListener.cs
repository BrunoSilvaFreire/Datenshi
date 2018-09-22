using Datenshi.Scripts.Movement.Config;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Misc {
    public class DashListener : StateMachineBehaviour {
        [ShowInInspector, ReadOnly]
        private MovableEntity entity;

        public string DashingKey = "Dashing";

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            if (entity == null) {
                entity = animator.GetComponentInParent<MovableEntity>();
            }

            if (entity.GetMovementConfigAs<TerrestrialConfig>().DashEllegible && entity.InputProvider.GetDash()) {
                animator.SetBool(DashingKey, true);
            }
        }
    }
}