using System.Collections.Generic;
using UnityEngine;

namespace Datenshi.Scripts.Stealth {
    public class HackController : MonoBehaviour {
        private List<HackableElement> elementsInRange = new List<HackableElement>();

        private void OnTriggerEnter2D(Collider2D other) {
            var e = other.GetComponentInParent<HackableElement>();
            if (e != null) {
                elementsInRange.Add(e);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var e = other.GetComponentInParent<HackableElement>();
            if (e != null) {
                elementsInRange.Remove(e);
            }
        }
    }
}