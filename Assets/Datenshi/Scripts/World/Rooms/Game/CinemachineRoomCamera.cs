using Cinemachine;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine {
    public class CinemachineRoom : MonoBehaviour {
        public CinemachineVirtualCameraBase Camera;
        public int EnterPriority = 15;
        public int ExitPriority;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.GetComponentInParent<Entity>() == PlayerController.Instance.CurrentEntity) {
                Camera.Priority = EnterPriority;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.GetComponentInParent<Entity>() == PlayerController.Instance.CurrentEntity) {
                Camera.Priority = ExitPriority;
            }
        }
    }
}