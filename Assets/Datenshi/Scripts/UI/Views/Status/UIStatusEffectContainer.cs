using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Status;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.UI.Misc;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Volatiles;
using UnityEngine;

namespace Datenshi.Scripts.UI.Views.Status {
    public class UIStatusEffectContainer : UICanvasGroupElement {
        public Transform ContentTransform;

        private void Start() {
            PlayerController.Instance.OnEntityChanged.AddListener(OnChanged);
            StatusEffectAppliedEvent.Instance.AddListener(OnStatusEffectApplied);
            StatusEffectRemovedEvent.Instance.AddListener(OnStatusEffectRemoved);
            SnapShow();
        }

        private void OnStatusEffectRemoved(StatusEffect arg0, ICombatant arg1) {
            if (!IsCurrent(arg1)) {
                return;
            }

            foreach (var element in GetComponentsInChildren<UIStatusEffectElement>()) {
                if (element.Effect == arg0) {
                    element.FadeDelete();
                }
            }
        }

        private void OnStatusEffectApplied(StatusEffect effect, ICombatant combatant, VolatilePropertyModifier modifier) {
            if (!IsCurrent(combatant)) {
                return;
            }

            InitNewElement(effect, modifier);
        }

        private static bool IsCurrent(ICombatant combatant) {
            var e = combatant as LivingEntity;
            return e == PlayerController.Instance.CurrentEntity;
        }

        private void OnChanged(Entity old, Entity newEntity) {
            ReloadForEntity(newEntity);
        }

        private void ReloadForEntity(Entity newEntity) {
            foreach (var e in GetComponentsInChildren<UIStatusEffectElement>()) {
                Destroy(e.gameObject);
            }

            var le = newEntity as LivingEntity;
            if (le == null) {
                return;
            }

            foreach (var effect in StatusEffect.GetEffects(le)) {
                InitNewElement(effect.Item1, effect.Item2);
            }
        }

        private void InitNewElement(StatusEffect effect, VolatilePropertyModifier modifier) {
            var element = UIResources.Instance.StatusEffectElementPrefab.Clone(ContentTransform);
            element.Init(effect, modifier);
            element.FadeIn();
        }
    }
}