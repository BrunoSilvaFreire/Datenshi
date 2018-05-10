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

        public BlackAndWhiteFX Fx;
        public AnalogGlitch DamageGlitch;

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
                ResetDarken();
                ResetGlitch();
                AudioManager.Instance.ImpactLowFilter(DamageLowfilterDefault, DamageCutoff, DamageDarkenDuration);
                AudioManager.Instance.PlayFX(DamageAudio.RandomElement());
                DamageGlitch.enabled = true;
                DamageGlitch.ScanLineJitter = DamageGlitchAmount;
                DamageGlitch.ColorDrift = DamageColorDriftAmount;
                DamageGlitch.HorizontalShake = DamageHorizontalShakeAmount;
                Fx.Amount = DamageDarkenAmount;


                glitchTweener = DamageGlitch.DOScanLineJitter(0, DamageGlitchDuration);
                colorTweener = DamageGlitch.DOColorDrift(0, DamageGlitchDuration);
                shakeTweener = DamageGlitch.DOHorizontalShake(0, DamageGlitchDuration);
                darkenTweener = Fx.DOAmount(0, DamageDarkenDuration);

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
            DamageGlitch.ScanLineJitter = 0;
            DamageGlitch.ColorDrift = 0;
            DamageGlitch.HorizontalShake = 0;
            DamageGlitch.enabled = false;
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
            //SetFX(0);
        }

        private void ShowDefend() {
            SetFilter(LowCutoff);
            SetColorOverride(DefendOverrideAmount);
            //SetFX(1);
        }

        private void SetFilter(float f) {
            var filter = AudioManager.Instance.LowPassFilter;
            filter.DOKill();
            filter.DOFrequency(f, DefendOverrideDuration);
        }

        private void SetFX(float x) {
            Fx.DOKill();
            Fx.DOAmount(x, DefendOverrideDuration);
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