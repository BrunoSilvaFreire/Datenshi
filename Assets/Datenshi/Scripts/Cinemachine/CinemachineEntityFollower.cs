using Cinemachine;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine {
    public class CinemachineEntityFollower : MonoBehaviour {
        public CinemachineVirtualCamera CameraBase;

        private void Awake() {
            PlayerController.Instance.OnEntityChanged.AddListener(OnChanged);
        }

        private void OnChanged(Entity arg0, Entity arg1) {
            CameraBase.Follow = arg1.Transform;
        }
    }
}