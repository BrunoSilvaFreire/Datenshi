using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Interaction {
    public class InteractionManager : MonoBehaviour {
        [ShowInInspector]
        private readonly List<InteractableElement> elements = new List<InteractableElement>();

        public IEnumerable<InteractableElement> Elements {
            get {
                return elements;
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var element = other.GetComponentInParent<InteractableElement>();
            if (element != null) {
                elements.Add(element);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var element = other.GetComponentInParent<InteractableElement>();
            if (element != null) {
                elements.Remove(element);
            }
        }
    }
}