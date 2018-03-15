using UnityEngine;
using UnityEngine.Tilemaps;

namespace Datenshi.Scripts.Animation.Behaviour {
    public class ChargeTimeDelayer : StateMachineBehaviour {
        public string TriggerKey = "Fire";
        public float Duration;
        private float startTime;

        private void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            startTime = Time.time;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (Time.time - startTime >= Duration) {
                animator.SetTrigger(TriggerKey);
            }
        }
    }
}