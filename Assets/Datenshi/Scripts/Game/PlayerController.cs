using System;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game.Time;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Singleton;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Game {
    [Serializable]
    public class PlayerEntityChangedEvent : UnityEvent<Entity, Entity> { }

    public class PlayerController : Singleton<PlayerController> {
        public PlayerInputProvider Player;

        [SerializeField, HideInInspector]
        private Entity currentEntity;

        [ShowInInspector]
        private Tracker<ColorizableRenderer> tracker;


        public float DamageDarkenAmount = 1;
        public float DamageDarkenDuration = 1;

        public float DamageGlitchAmount = 1;
        public float DamageColorDriftAmount = 1;
        public float DamageHorizontalShakeAmount = 1;


        public float DamageGlitchDuration = .25F;
        public float DamageCutoff = 5000;
        public float DamageLowfilterDefault = 0;
        public float LowCutoff = 440;
        public float DefendOverrideAmount = 1;
        public float DefendOverrideDuration = 0.5F;
        public PlayerEntityChangedEvent OnEntityChanged;
        public AudioFX[] DamageAudio;

        [ShowInInspector]
        public Entity CurrentEntity {
            get {
                return currentEntity;
            }
            set {
                if (currentEntity != null) {
                    currentEntity.RevokeOwnership();
                }

                OnEntityChanged.Invoke(currentEntity, value);
                currentEntity = value;
                currentEntity.RequestOwnership(Player);
            }
        }

        private TweenerCore<float, float, FloatOptions> glitchTweener;
        private TweenerCore<float, float, FloatOptions> colorTweener;
        private TweenerCore<float, float, FloatOptions> shakeTweener;
        private TweenerCore<float, float, FloatOptions> darkenTweener;

        private void Start() {
            ResetGlitch();
            if (currentEntity.InputProvider != Player) {
                currentEntity.RevokeOwnership();
                currentEntity.RequestOwnership(Player);
                OnEntityChanged.Invoke(null, currentEntity);
            }

            GlobalEntityDamagedEvent.Instance.AddListener(OnEntityDamaged);
            tracker = new Tracker<ColorizableRenderer>(
                ColorizableRenderer.ColorizableRendererEnabledEvent,
                ColorizableRenderer.ColorizableRendererDisabledEvent);
        }

        private void OnEntityDamaged(ICombatant damaged, ICombatant damager, uint arg2) {
            if (!Equals(damager, currentEntity) && !Equals(damaged, currentEntity)) {
                return;
            }

            if (Equals(damaged, currentEntity)) {
                var graphics = GraphicsSingleton.Instance;
                var damageGlitch = graphics.Glitch;
                var bnw = graphics.BlackAndWhite;
                ResetDarken();
                ResetGlitch();
                AudioManager.Instance.ImpactLowFilter(DamageLowfilterDefault, DamageCutoff, DamageDarkenDuration);
                AudioManager.Instance.PlayFX(DamageAudio.RandomElement());
                damageGlitch.enabled = true;
                damageGlitch.ScanLineJitter = DamageGlitchAmount;
                damageGlitch.ColorDrift = DamageColorDriftAmount;
                damageGlitch.HorizontalShake = DamageHorizontalShakeAmount;
                bnw.Amount = DamageDarkenAmount;


                glitchTweener = damageGlitch.DOScanLineJitter(0, DamageGlitchDuration);
                colorTweener = damageGlitch.DOColorDrift(0, DamageGlitchDuration);
                shakeTweener = damageGlitch.DOHorizontalShake(0, DamageGlitchDuration);
                darkenTweener = bnw.DOAmount(0, DamageDarkenDuration);

                glitchTweener.OnComplete(ResetGlitch);
                colorTweener.OnComplete(ResetGlitch);
                shakeTweener.OnComplete(ResetGlitch);
                darkenTweener.OnComplete(ResetDarken);
            }

            TimeController.Instance.ImpactFrame();
        }

        private void ResetGlitch() {
            glitchTweener?.Kill();
            colorTweener?.Kill();
            shakeTweener?.Kill();
            glitchTweener = null;
            colorTweener = null;
            shakeTweener = null;
            var damageGlitch = GraphicsSingleton.Instance.Glitch;
            damageGlitch.ScanLineJitter = 0;
            damageGlitch.ColorDrift = 0;
            damageGlitch.HorizontalShake = 0;
            damageGlitch.enabled = false;
        }

        private void ResetDarken() {
            darkenTweener?.Kill();
            darkenTweener = null;
        }

        [ShowInInspector]
        private bool defending;


        private void Update() {
            var p = Player;
            if (p == null) {
                return;
            }

            var l = currentEntity as LivingEntity;
            var currentDefending = l != null && l.Focusing;
            if (currentDefending == defending) {
                return;
            }

            defending = currentDefending;
            if (defending) {
                ShowDefend();
            } else {
                HideDefend();
            }
        }

        private void HideDefend() {
            SetFilter(22000);
            SetColorOverride(0);
        }

        private void ShowDefend() {
            SetFilter(LowCutoff);
            SetColorOverride(DefendOverrideAmount);
        }

        private void SetFilter(float f) {
            var filter = AudioManager.Instance.LowPassFilter;
            filter.DOKill();
            filter.DOFrequency(f, DefendOverrideDuration);
        }


        private void SetColorOverride(float i) {
            foreach (var obj in tracker.Objects) {
                if (obj == null) {
                    continue;
                }

                SetColorOverride(obj, i);
            }
        }

        private void SetColorOverride(ColorizableRenderer r, float i) {
            r.DOKill();
            DOTween.To(
                () => r.ColorOverrideAmount,
                value => r.ColorOverrideAmount = value,
                i,
                DefendOverrideDuration);
        }
    }
}