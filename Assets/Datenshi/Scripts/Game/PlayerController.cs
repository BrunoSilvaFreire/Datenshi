using Datenshi.Assets.Graphics.Shaders;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Misc;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class PlayerController : MonoBehaviour {
        public PlayerInputProvider Player;

        [SerializeField, HideInInspector]
        private Entity currentEntity;

        [ShowInInspector]
        private Tracker<Entity> tracker;

        public BlackAndWhiteFX Fx;
        public float LowCutoff = 440;
        public float DefendOverrideAmount = 1;
        public float DefendOverrideDuration = 0.5F;

        [ShowInInspector]
        public Entity CurrentEntity {
            get {
                return currentEntity;
            }
            set {
                if (currentEntity != null) {
                    currentEntity.RevokeOwnership();
                }

                currentEntity = value;
                currentEntity.RequestOwnership(Player);
            }
        }

        private void Start() {
            if (currentEntity.InputProvider == Player) {
                return;
            }

            currentEntity.RevokeOwnership();
            currentEntity.RequestOwnership(Player);
            tracker = new Tracker<Entity>(Entity.EntityEnabledEvent, Entity.EntityDisabledEvent);
        }

        [ShowInInspector]
        private bool defending;

        private void Update() {
            var p = Player;
            if (p == null) {
                return;
            }

            var currentDefending = p.GetDefend();
            if (currentDefending != defending) {
                Debug.Log("Defending = " + currentDefending + " - " + defending);
                defending = currentDefending;
                if (defending) {
                    ShowDefend();
                } else {
                    HideDefend();
                }
            }
        }

        private void HideDefend() {
            SetFilter(22000);
            SetColorOverride(0);
            SetFX(0);
        }

        private void ShowDefend() {
            SetFilter(440);
            SetColorOverride(DefendOverrideAmount);
            SetFX(1);
        }

        private void SetFilter(float f) {
            var filter = Singletons.Instance.LowPassFilter;
            filter.DOKill();
            DOTween.To(
                () => filter.cutoffFrequency,
                value => filter.cutoffFrequency = value,
                f,
                DefendOverrideDuration);
        }

        private void SetFX(float x) {
            Fx.DOKill();
            DOTween.To(() => Fx.Amount, value => Fx.Amount = value, x, DefendOverrideDuration);
        }

        private void SetColorOverride(float i) {
            foreach (var obj in tracker.Objects) {
                if (obj == null) {
                    continue;
                }
                obj.DOKill();
                var eRenderer = obj.Renderer;
                if (eRenderer == null) {
                    return;
                }

                SetColorOverride(eRenderer, i);
            }
        }

        private void SetColorOverride(EntityRenderer eRenderer, float i) {
            DOTween.To(
                () => eRenderer.ColorOverrideAmount,
                value => eRenderer.ColorOverrideAmount = value,
                i,
                DefendOverrideDuration);
        }
    }
}