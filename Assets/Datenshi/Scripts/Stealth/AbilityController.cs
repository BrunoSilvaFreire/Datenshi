using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Interaction;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Stealth {
    public abstract class AbilityController : MonoBehaviour {
        private static readonly List<AbilityController> controllers = new List<AbilityController>();

        public static IEnumerable<AbilityController> AbilityControllers {
            get {
                return controllers;
            }
        }

        private bool isActive;

        public bool IsActive {
            get {
                return isActive;
            }
            set {
                isActive = value;
                OnActiveChanged();
            }
        }

        protected abstract void OnActiveChanged();
    }

    public abstract class AbilityController<T> : AbilityController where T : InfiltrableElement {
        [ShowInInspector]
        protected readonly List<T> elementsInRange = new List<T>();


        public IEnumerable<T> ElementsInRange {
            get {
                return elementsInRange.Cast<T>();
            }
        }

        protected abstract void OnElementAdded(T e);
        protected abstract void OnElementRemoved(T e);

        protected virtual bool Allows(T e) {
            return true;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var e = other.GetComponentInParent<T>();
            if (e == null) {
                return;
            }
            if (!Allows(e)) {
                return;
            }
            elementsInRange.Add(e);
            if (!IsActive) {
                return;
            }

            var ui = e.UIElement;
            if (ui != null) {
                ui.Button.interactable = true;
            }

            OnElementAdded(e);
        }

        private void OnTriggerExit2D(Collider2D other) {
            var e = other.GetComponentInParent<T>();
            if (e == null) {
                return;
            }
            if (!Allows(e)) {
                return;
            }
            elementsInRange.Remove(e);
            if (!IsActive) {
                return;
            }

            var ui = e.UIElement;
            if (ui != null) {
                ui.Button.interactable = false;
            }
            OnElementRemoved(e);
        }
    }
}