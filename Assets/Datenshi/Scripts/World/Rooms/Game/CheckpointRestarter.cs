using Datenshi.Scripts.Game;
using Datenshi.Scripts.Game.Restart;
using UnityEngine;

namespace Datenshi.Scripts.World.Rooms.Game {
    public class CheckpointRestarter : MonoBehaviour, IRestartable {
        public Transform Transform;

        private void Start() {
            if (Transform == null) {
                Transform = transform;
            }
        }

        public void Restart() {
            Transform.position = GetRespawnPoint();
        }

        private static Vector3 GetRespawnPoint() {
            var checkpoint = GameController.Instance.LastCheckpoint;
            return checkpoint == null ? World.Instance.SpawnPoint.position : checkpoint.Spawnpoint.position;
        }
    }
}