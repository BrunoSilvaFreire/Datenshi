using UnityEngine;

namespace Datenshi.Scripts.Animation {
    public abstract class AnimatorUpdater : MonoBehaviour {
        [SerializeField]
        protected Animator Animator;

        private void LateUpdate() {
            UpdateAnimator(Animator);
        }

        protected abstract void UpdateAnimator(Animator anim);
    }
}