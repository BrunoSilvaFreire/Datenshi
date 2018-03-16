using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public class Door : InteractableElement {
        public bool Locked;
        public Collider2D Collider;
        public override bool CanInteract(MovableEntity e) {
            return true;
        }

        protected override void Execute(MovableEntity e) {
            var obj = Collider.gameObject;
            obj.SetActive(!obj.activeSelf);
        }
    }
}