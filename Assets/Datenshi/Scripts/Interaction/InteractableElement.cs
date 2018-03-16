using System;
using Datenshi.Scripts.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Interaction {
    [Serializable]
    public class EntityInteractionEvent : UnityEvent<MovableEntity> { }

    public abstract class InteractableElement : MonoBehaviour {
        public EntityInteractionEvent OnInteract;
        public abstract bool CanInteract(MovableEntity e);

        public void Interact(MovableEntity e) {
            Execute(e);
            OnInteract.Invoke(e);
        }
        protected abstract void Execute(MovableEntity e);
    }
}