using System;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.UI.Stealth;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Interaction {
    [Serializable]
    public class EntityInteractionEvent : UnityEvent<MovableEntity> { }

    public abstract class InteractableElement : MonoBehaviour {
        public UIInteractableElementView UIElement;
        public EntityInteractionEvent OnInteract;
        public abstract bool CanInteract(MovableEntity e);

        private void Awake() {
            var e = UIElement;
            if (e != null) {
                e.SnapShowing(false);
            }
        }

        public void Interact(MovableEntity e) {
            Execute(e);
            OnInteract.Invoke(e);
        }

        protected abstract void Execute(MovableEntity e);
    }
}