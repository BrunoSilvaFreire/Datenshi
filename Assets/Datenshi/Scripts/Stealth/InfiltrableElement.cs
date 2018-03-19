using Datenshi.Scripts.Character;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Interaction;
using Datenshi.Scripts.UI;
using Datenshi.Scripts.UI.Stealth;
using UnityEngine;

namespace Datenshi.Scripts.Stealth {
    public abstract class InfiltrableElement : MonoBehaviour {
        public UIInteractableElementView UIElement;

        public void Infiltrate(MovableEntity e) {
            Execute(e);
        }

        public abstract bool CanInfiltrate(MovableEntity e);
        protected abstract void Execute(MovableEntity e);
    }

    public abstract class HackableElement : InfiltrableElement {
        public override bool CanInfiltrate(MovableEntity e) {
            var character = e.Character;
            if (character == null) {
                return false;
            }

            var c = character as PlayableCharacter;
            if (c == null) {
                return false;
            }

            return c.Trait == Trait.Hacker;
        }
    }
}