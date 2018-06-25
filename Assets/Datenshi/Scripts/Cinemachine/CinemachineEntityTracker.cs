using Cinemachine;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using UnityEngine;

namespace Datenshi.Scripts.Cinemachine {
    public class CinemachineEntityTracker : MonoBehaviour {
        public CinemachineVirtualCamera Camera;

        private void Awake() {
            PlayerController.Instance.OnEntityChanged.AddListener(OnChanged);
        }

        private void OnChanged(Entity arg0, Entity arg1) {
            Debug.Log("Changing to " + arg1);
            Camera.Follow = arg1 != null ? arg1.transform : null;
        }
    }
}