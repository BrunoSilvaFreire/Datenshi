using System;
using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game.Rank;
using Datenshi.Scripts.Game.Time;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Input;
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

    [Serializable]
    public class PlayerRankXPGainedEvent : UnityEvent<float> { }


    public class PlayerController : Singleton<PlayerController> {
        public PlayerInputProvider Player;

        [SerializeField, HideInInspector]
        private Entity currentEntity;

        [ShowInInspector]
        private Tracker<ColorizableRenderer> tracker;

        public Rank.Rank Rank;

        public float DamageDarkenAmount = 1;
        public float DamageDarkenDuration = 1;

        public float DamageGlitchAmount = 1;
        public float DamageColorDriftAmount = 1;
        public float DamageHorizontalShakeAmount = 1;


        public float DamageGlitchDuration = .25F;

        public float DefendOverrideAmount = 1;
        public float DefendOverrideDuration = 0.5F;
        public PlayerEntityChangedEvent OnEntityChanged;
        public PlayerRankXPGainedEvent PlayerRankXPGainedEvent;
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

        private void Start() {
            ResetGlitch();
            if (currentEntity.InputProvider != Player) {
                currentEntity.RevokeOwnership();
                currentEntity.RequestOwnership(Player);
                OnEntityChanged.Invoke(null, currentEntity);
            }

            GameState.RestartState();
            GlobalEntityDamagedEvent.Instance.AddListener(OnEntityDamaged);

            tracker = new Tracker<ColorizableRenderer>(
                ColorizableRenderer.ColorizableRendererEnabledEvent,
                ColorizableRenderer.ColorizableRendererDisabledEvent);
        }

        private void OnEntityDamaged(ICombatant damaged, ICombatant damager, Attack attack, uint damage) {
            if (currentEntity != null && (Entity) damager == currentEntity) {
                HandleRankAttack(attack);
            }

            if (!Equals(damager, currentEntity) && !Equals(damaged, currentEntity)) {
                return;
            }

            if (Equals(damaged, currentEntity)) {
                var graphics = GraphicsSingleton.Instance;
                var damageGlitch = graphics.Glitch;
                var bnw = graphics.BlackAndWhite;
                ResetGlitch();
                AudioManager.Instance.PlayFX(DamageAudio.RandomElement());
                damageGlitch.enabled = true;
                damageGlitch.ScanLineJitter = DamageGlitchAmount;
                damageGlitch.ColorDrift = DamageColorDriftAmount;
                damageGlitch.HorizontalShake = DamageHorizontalShakeAmount;


                glitchTweener = damageGlitch.DOScanLineJitter(0, DamageGlitchDuration);
                colorTweener = damageGlitch.DOColorDrift(0, DamageGlitchDuration);
                shakeTweener = damageGlitch.DOHorizontalShake(0, DamageGlitchDuration);
                bnw.DoDarkenImpact(DamageDarkenAmount, DamageDarkenDuration);

                glitchTweener.OnComplete(ResetGlitch);
                colorTweener.OnComplete(ResetGlitch);
                shakeTweener.OnComplete(ResetGlitch);
            }

            TimeController.Instance.Slowdown();
        }

        private Attack lastAttack;
        private uint timesReused;
        public float RankXPGainedWaitDuration = 2;
        public float RankXPDropSpeed = .1F;
        private float xpStopDurationLeft;


        private void HandleRankAttack(Attack attack) {
            if (attack == lastAttack) {
                timesReused++;
            } else {
                timesReused = 0;
                lastAttack = attack;
            }

            var xpToWin = GameResources.Instance.RankXPGraph.Evaluate(timesReused);
            if (xpToWin <= 0) {
                return;
            }

            xpStopDurationLeft = RankXPGainedWaitDuration;
            Rank.XP += xpToWin;
            PlayerRankXPGainedEvent.Invoke(xpToWin);
        }

        // oi
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

        //private bool defending;


        private void Update() {
            UpdateRank();
            UpdatePause();
        }

        private void UpdatePause() {
            if (Player.GetButtonDown((int) Actions.Cancel)) {
                RuntimeResources.Instance.TogglePaused();
            }
        }


        private void UpdateRank() {
            var delta = UnityEngine.Time.deltaTime;
            if (xpStopDurationLeft > 0) {
                xpStopDurationLeft -= delta;
                return;
            }

            var toDrop = RankXPDropSpeed * delta;
            if (Rank.CurrentLevel > RankLevel.F || Rank.XP > toDrop) {
                Rank.XP -= toDrop;
            } else {
                Rank.XP = 0;
            }
        }

        private void HideDefend() {
            SetColorOverride(0);
        }

        private void ShowDefend() {
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