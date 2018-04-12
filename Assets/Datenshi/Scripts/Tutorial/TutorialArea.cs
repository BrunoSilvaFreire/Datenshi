using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Tutorial {
    public class TutorialArea : MonoBehaviour {
        public PlayerController Controller;
        public UITutorial UITutorialPrefab;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.GetComponentInParent<Entity>() != Controller.CurrentEntity) {
                return;
            }
            UITutorialBox.Instance.Show(UITutorialPrefab);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.GetComponentInParent<Entity>() != Controller.CurrentEntity) {
                return;
            }
            UITutorialBox.Instance.Deregister(UITutorialPrefab);
        }
    }
}