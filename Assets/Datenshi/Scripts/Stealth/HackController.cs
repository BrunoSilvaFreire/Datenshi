using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Stealth {
    public class HackController : MonoBehaviour {
        [ShowInInspector]
        private readonly List<InfiltrableElement> elementsInRange = new List<InfiltrableElement>();

        public List<InfiltrableElement> ElementsInRange {
            get {
                return elementsInRange;
            }
        }

        public bool IsHacking {
            get {
                return isHacking;
            }
            set {
                isHacking = value;
                foreach (var infiltrableElement in elementsInRange) {
                    var e = infiltrableElement.UIElement;
                    if (e != null) {
                        e.Button.interactable = value;
                    }
                }
            }
        }

        private bool isHacking;

        private void OnTriggerEnter2D(Collider2D other) {
            var e = other.GetComponentInParent<InfiltrableElement>();
            if (e == null) {
                return;
            }

            elementsInRange.Add(e);
            if (!IsHacking) {
                return;
            }

            var ui = e.UIElement;
            if (ui != null) {
                ui.Button.interactable = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var e = other.GetComponentInParent<InfiltrableElement>();
            if (e == null) {
                return;
            }

            elementsInRange.Remove(e);
            if (!IsHacking) {
                return;
            }

            var ui = e.UIElement;
            if (ui != null) {
                ui.Button.interactable = false;
            }
        }
    }
}