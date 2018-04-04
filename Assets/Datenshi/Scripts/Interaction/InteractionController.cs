using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public class InteractionController : MonoBehaviour {
        [ShowInInspector]
        protected readonly List<InteractableElement> elementsInRange = new List<InteractableElement>();

        public Collider2D Hitbox;

        public IEnumerable<InteractableElement> ElementsInRange {
            get {
                return elementsInRange;
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
                ui.Button.interactable = true;
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
                ui.Button.interactable = false;
            }
        }
    }
}