using Datenshi.Scripts.Behaviours.Tasks;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial {
    public class AssistantTrait : MonoBehaviour {
        public Follow FollowTask;

        private void Start() {
            var b = UITutorialBox.Instance;
            b.OnShowTutorial.AddListener(OnShow);
            b.OnHideTutorial.AddListener(OnHide);
        }

        private void OnHide(TutorialTrigger arg0) {
            FollowTask.Override = null;
        }

        private void OnShow(TutorialTrigger arg0) {
            if (arg0.HasCustomLocation) {
                FollowTask.Override = arg0.CustomLocation;
            }
        }
    }
}