using Datenshi.Scripts.Util;
using UnityEngine;
using UnityEngine.Profiling;

namespace Datenshi.Scripts.Animation {


    public abstract class AnimatorUpdater : MonoBehaviour {
        [SerializeField]
        public Animator Animator;

        private void LateUpdate() {
            Profiler.BeginSample("Updating " + name + " @ " + GetType().Name);
            UpdateAnimator(Animator);
            Profiler.EndSample();
        }

        protected abstract void UpdateAnimator(Animator anim);
    }
}