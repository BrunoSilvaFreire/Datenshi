using Datenshi.Scripts.Animation;
using UnityEngine;

namespace Datenshi.Scripts.UI {
    public class UIPlayerViewUpdater : AnimatorUpdater {
        public string ShowingKey = "Showing";
        public UIPlayerView View;

        protected override void UpdateAnimator(Animator animator, AnimatorControllerParameter parameter) {
            if (parameter.name == ShowingKey) {
                animator.SetBool(ShowingKey, View.Showing);
            }
        }
    }
}