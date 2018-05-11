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
        public string CollectedKey = "Collected";
        public Animator Animator;
        private bool collected;

        private void OnCollisionEnter2D(Collision2D other) {
            if (collected) {
                return;
            }

            var e = other.collider.GetComponentInParent<MovableEntity>();
            if (e == null) {
                return;
            }
            collected = true;

            Collect(e);
            if (Animator != null) {
                Animator.SetTrigger(CollectedKey);
                return;
            }

            Destroy(gameObject);
        }

        protected abstract void Collect(MovableEntity movableEntity);
    }
}