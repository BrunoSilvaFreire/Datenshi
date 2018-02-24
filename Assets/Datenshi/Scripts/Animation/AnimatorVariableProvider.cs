using UnityEngine;

namespace Datenshi.Scripts.Animation {
    public abstract class AnimatorUpdater : MonoBehaviour {
        public Animator Animator;

        private void Update() {
            foreach (var parameter in Animator.parameters) {
                UpdateAnimator(Animator, parameter);
            }
        }

        protected abstract void UpdateAnimator(Animator animator, AnimatorControllerParameter parameter);
    }
}