using System.Collections.Generic;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public class InteractionController : MonoBehaviour {
        [ShowInInspector]
        protected readonly List<InteractableElement> elementsInRange = new List<InteractableElement>();

        public MovableEntity Entity;
        public IEnumerable<InteractableElement> ElementsInRange => elementsInRange;

        private void Update() {
            if (Entity.InputProvider.GetButtonDown((int) Actions.Submit)) {
                Interact();
            }
        }

        private void Interact() {
            foreach (var element in elementsInRange) {
                element.Interact(Entity);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var e = other.GetComponentInParent<InteractableElement>();
            if (e == null) {
                return;
            }

            elementsInRange.Add(e);
            var ui = e.UIElement;
            if (ui != null) {
                ui.Showing = true;
                var b = ui.Button;
                if (b != null) {
                    b.interactable = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var e = other.GetComponentInParent<InteractableElement>();
            if (e == null) {
                return;
            }

            elementsInRange.Remove(e);
            var ui = e.UIElement;
            if (ui != null) {
                ui.Showing = false;
                var b = ui.Button;
                if (b != null) {
                    b.interactable = false;
                }
            }
        }
    }
}