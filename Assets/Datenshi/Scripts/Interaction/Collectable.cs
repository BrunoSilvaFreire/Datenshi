using System;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public abstract class Collectable : MonoBehaviour {
        [Flags]
        public enum CollectTarget {
            Player = 1 << 0,
            Enemy = 1 << 1
        }

        public CollectTarget Target = CollectTarget.Player;

        private void OnCollisionEnter2D(Collision2D other) {
            var e = other.collider.GetComponentInParent<MovableEntity>();
            if (e == null) {
                return;
            }

            Collect(e);
            Destroy(gameObject);
        }

        protected abstract void Collect(MovableEntity movableEntity);
    }
}