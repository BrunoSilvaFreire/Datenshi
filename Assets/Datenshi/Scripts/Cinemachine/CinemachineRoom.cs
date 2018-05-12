using Cinemachine;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.World;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine {
    public class CinemachineRoom : AbstractRoomMember {
        public CinemachineVirtualCameraBase Camera;
        public int EnterPriority = 15;
        public int ExitPriority;

        private void Start() {
            Room.OnObjectEnter.AddListener(OnEnter);
            Room.OnObjectExit.AddListener(OnExit);
        }

        private void OnEnter(Collider2D other) {
            if (other.GetComponentInParent<Entity>() == PlayerController.Instance.CurrentEntity) {
                Camera.Priority = EnterPriority;
            }
        }

        private void OnExit(Collider2D other) {
            if (other.GetComponentInParent<Entity>() == PlayerController.Instance.CurrentEntity) {
                Camera.Priority = ExitPriority;
            }
        }
    }
}