using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.FX;
using Datenshi.Scripts.Game.Time;
using Datenshi.Scripts.Graphics;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class DamageFXHandler : MonoBehaviour {
        public Effect DamageAudio;

        public float DamageGlitchAmount = 1;
        public float DamageColorDriftAmount = 1;
        public float DamageHorizontalShakeAmount = 1;

        public float DamageGlitchDuration = .25F;

        public float SlowdownDuration;
        public AnimationCurve SlowdownScale;
        public AnimationCurve DesaturateCurve;
        public AnimationCurve DarkenCurve;
        public float BNWDuration = 1;

        private void Start() {
            ResetGlitch();
            GlobalEntityDamagedEvent.Instance.AddListener(OnEntityDamaged);
        }

        private static void ResetGlitch() {
            var damageGlitch = GraphicsSingleton.Instance.Glitch;
            damageGlitch.ScanLineJitter = 0;
            damageGlitch.ColorDrift = 0;
            damageGlitch.HorizontalShake = 0;
            damageGlitch.enabled = false;
        }

        private void OnEntityDamaged(ICombatant damaged, IDamageDealer damager, IDamageSource attack, uint damage) {
            Vibrate();
            var currentEntity = PlayerController.Instance.CurrentEntity;
            if (!Equals(damager, currentEntity) && !Equals(damaged, currentEntity)) {
                return;
            }

            if (Equals(damaged, currentEntity) && !damaged.Dead) {
                var graphics = GraphicsSingleton.Instance;
                var damageGlitch = graphics.Glitch;
                var bnw = graphics.BlackAndWhite;
                ResetGlitch();
                DamageAudio.Execute(currentEntity.Center);
                damageGlitch.DOKill();
                damageGlitch.enabled = true;
                damageGlitch.ScanLineJitter = DamageGlitchAmount;
                damageGlitch.ColorDrift = DamageColorDriftAmount;
                damageGlitch.HorizontalShake = DamageHorizontalShakeAmount;


                damageGlitch.DOScanLineJitter(0, DamageGlitchDuration);
                damageGlitch.DOColorDrift(0, DamageGlitchDuration).OnComplete(ResetGlitch);
                damageGlitch.DOHorizontalShake(0, DamageGlitchDuration);
                bnw.RequestService(BNWDuration, DesaturateCurve, DarkenCurve);
            }

            TimeController.Instance.RequestAnimatedSlowdown(SlowdownScale, SlowdownDuration, 2);
        }

        private Coroutine vibrateCoroutine;
        public float VibrationDuration = .1F;
        public float VibrationLevel = .25F;

        private void Vibrate() {
            PlayerController.Instance.Player.CurrentPlayer.SetVibration(0, VibrationLevel, VibrationDuration);
        }
    }
}