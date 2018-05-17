using Datenshi.Scripts.Entities;
using Datenshi.Scripts.World;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class GameState {
        public static GameState CurrentState {
            get;
            private set;
        }

        public static void RestartState() {
            CurrentState = new GameState();
        }

        public Checkpoint LastCheckpoint {
            get;
            private set;
        }

        public void AttempSetCheckpoint(Checkpoint cp) {
            if (LastCheckpoint != null && cp.Priority < LastCheckpoint.Priority) {
                return;
            }

            LastCheckpoint = cp;
        }


        private GameState() {
            CheckpointCollidedEvent.Instance.AddListener(OnCheckpointReached);
        }

        private void OnCheckpointReached(Checkpoint arg0, Collision2D collision2D) {
            var e = collision2D.collider.GetComponentInParent<Entity>();
            var ce = PlayerController.Instance.CurrentEntity;
            if (ce == null || ce != e) {
                return;
            }

            AttempSetCheckpoint(arg0);
        }
    }
}