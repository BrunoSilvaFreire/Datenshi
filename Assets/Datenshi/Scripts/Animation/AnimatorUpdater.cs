using UnityEngine;
using UnityEngine.Profiling;

namespace Datenshi.Scripts.Animation {
    public abstract class EntityAnimatorUpdater : AnimatorUpdater {
        public abstract void TriggerAttack();
        public abstract void TriggerAttack(string attack);
        public abstract void SetDefend(bool defend);

        public abstract void TriggerDeflect();

        public abstract void TriggerCounter();
    }

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