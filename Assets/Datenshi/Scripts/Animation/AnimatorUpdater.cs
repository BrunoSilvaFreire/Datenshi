using UnityEngine;
using UnityEngine.Profiling;

namespace Datenshi.Scripts.Animation {
    public abstract class AnimatorUpdater : MonoBehaviour {
        [SerializeField]
        public Animator Animator;

        public void UpdateAnimator() {
            UpdateAnimator(Animator);
        }

        protected abstract void UpdateAnimator(Animator anim);
    }
}