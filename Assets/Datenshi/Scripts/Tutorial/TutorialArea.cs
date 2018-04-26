using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial {
    public class TutorialTrigger : MonoBehaviour {
        public UITutorial TutorialPrefab;
        public bool HasCustomLocation;
        public Vector2 CustomLocation;

        public void Show() {
            UITutorialBox.Instance.Show(this);
        }

        public void Hide() {
            UITutorialBox.Instance.Deregister(this);
        }
    }

    public class TutorialArea : TutorialTrigger {
        public bool DestroyOnLeave = true;

        private void OnTriggerEnter2D(Collider2D other) {
            var controller = PlayerController.Instance;
            if (other.isTrigger || other.GetComponentInParent<Entity>() != controller.CurrentEntity) {
                return;
            }

            Show();
        }

        private void OnTriggerExit2D(Collider2D other) {
            var controller = PlayerController.Instance;
            if (other.isTrigger || other.GetComponentInParent<Entity>() != controller.CurrentEntity) {
                return;
            }

            Hide();
        }
    }
}