using System.Collections.Generic;
using System.Linq;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.UI.Stealth;
using Datenshi.Scripts.Util;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Stealth {
    public class HackController : AbilityController<HackableElement> {
        public Entity Entity;
        public float LowFilterFrenquencyNormal = 20000;

        public float LowFilterFrenquency = 400;
        public float LowFilterDuration = 0.1F;

        protected override void OnElementAdded(HackableElement e) {
            if (!IsActive || lineRenderers.ContainsKey(e)) {
                return;
            }
            var obj = GameResources.Instance.HackLineRendererPrefab.Clone(transform);
            obj.name = string.Format("line({0}-{1})", e.name, Entity.name);
            lineRenderers[e] = obj;
        }

        protected override void OnElementRemoved(HackableElement e) {
            if (!IsActive || !lineRenderers.ContainsKey(e)) {
                return;
            }
            var r = lineRenderers[e];
            Destroy(r.gameObject);
            lineRenderers.Remove(e);
        }


        [ShowInInspector]
        private readonly Dictionary<InfiltrableElement, LineRenderer> lineRenderers = new Dictionary<InfiltrableElement, LineRenderer>();

        private void Update() {
            foreach (var pair in lineRenderers) {
                var r = pair.Value;
                r.SetPositions(
                    new[] {
                        Entity.transform.position,
                        pair.Key.UIElement.transform.position
                    }
                );
            }
        }

        protected override void OnActiveChanged() {
            Reselect();
            var filter = Singletons.Instance.LowPassFilter;
            filter.DOKill();
            DOTween.To(
                    () => filter.cutoffFrequency,
                    f => filter.cutoffFrequency = f,
                    IsActive ? LowFilterFrenquency : LowFilterFrenquencyNormal,
                    LowFilterDuration
                )
                .Play();
            AbilityFocus.Instance.Showing = IsActive;
            if (!IsActive) {
                foreach (var pair in lineRenderers) {
                    Destroy(pair.Value.gameObject);
                }
                lineRenderers.Clear();
            }
            foreach (var infiltrableElement in elementsInRange) {
                var e = infiltrableElement.UIElement;
                if (IsActive) {
                    var obj = GameResources.Instance.HackLineRendererPrefab.Clone(transform);
                    obj.name = string.Format("line({0}-{1})", infiltrableElement.name, Entity.name);
                    lineRenderers[infiltrableElement] = obj;
                }
                e.Button.interactable = IsActive;
            }
        }

        private void Reselect() {
            var provider = Entity.InputProvider;
            if (provider == null) {
                return;
            }
            var dir = provider.GetInputVector();
            var origin = Entity.transform.position;
            var minDistance = float.MaxValue;

            UIInteractableElementView current = null;
            foreach (var element in elementsInRange) {
                var view = element.UIElement;
                if (view == null) {
                    continue;
                }
                var pos = view.transform.position;
                var distance = CalculateDistance(origin, dir, pos);
                if (distance < minDistance) {
                    minDistance = distance;
                    current = view;
                }
            }
            if (current != null) {
                current.Select();
            }
        }

        private static float CalculateDistance(Vector2 origin, Vector2 dir, Vector3 pos) {
            return Vector2.Distance(origin, dir);
        }
    }
}