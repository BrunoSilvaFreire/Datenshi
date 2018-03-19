using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public class Door : InteractableElement {
        public bool Locked {
            get {
                return !Collider.isTrigger;
            }
            set {
                Collider.isTrigger = !value;
                foreach (var r in renderers) {
                    r.enabled = value;
                }
            }
        }

        private void Start() {
            renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public Collider2D Collider;
        private SpriteRenderer[] renderers;

        public override bool CanInteract(MovableEntity e) {
            return true;
        }

        protected override void Execute(MovableEntity e) {
            Locked = !Locked;
        }
    }
}