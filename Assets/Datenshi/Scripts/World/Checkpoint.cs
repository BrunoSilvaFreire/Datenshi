using System;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.World {
    [Serializable]
    public class CheckpointCollidedEvent : UnityEvent<Checkpoint, Collision2D> {
        public static readonly CheckpointCollidedEvent Instance = new CheckpointCollidedEvent();
        private CheckpointCollidedEvent() { }
    }

    public class Checkpoint : MonoBehaviour {
        [SerializeField]
        private byte priority;

        public byte Priority => priority;
        public Transform Spawnpoint;

        private void OnCollisionEnter2D(Collision2D other) {
            CheckpointCollidedEvent.Instance.Invoke(this, other);
        }
    }
}