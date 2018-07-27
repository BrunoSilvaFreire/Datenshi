using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial.Trigger {
    public class TutorialColliderTrigger : MonoBehaviour {
        public Tutorial Tutorial;

        public void HideAndDestroy() {
            Tutorial.StopTutorial();
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.isTrigger) {
                return;
            }

            var e = other.GetComponentInParent<Entity>();
            if (e != PlayerController.Instance.CurrentEntity) {
                return;
            }

            Tutorial.StartTutorial();
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.isTrigger) {
                return;
            }

            var e = other.GetComponentInParent<Entity>();
            if (e != PlayerController.Instance.CurrentEntity) {
                return;
            }

            Tutorial.StopTutorial();
        }
    }
}