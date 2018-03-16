using Datenshi.Scripts.Character;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Interaction;
using Datenshi.Scripts.UI;
using Datenshi.Scripts.UI.Stealth;
using UnityEngine;

namespace Datenshi.Scripts.Stealth {
    public abstract class InfiltrableElement : InteractableElement {
        public UIInteractableElementView UIElement;
    }

    public abstract class HackableElement : InfiltrableElement {
        public override bool CanInteract(MovableEntity e) {
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