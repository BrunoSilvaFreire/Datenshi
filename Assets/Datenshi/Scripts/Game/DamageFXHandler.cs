using Datenshi.Scripts.Audio;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.FX;
using Datenshi.Scripts.Graphics;
using Datenshi.Scripts.Util.Time;
using DG.Tweening;
using UnityEngine;

namespace Datenshi.Scripts.Game {
    public class DamageFXHandler : MonoBehaviour {
        public Effect DamageAudio;

        public AnimationCurve DamageLineJitterAmount;
        public AnimationCurve DamageColorDriftAmount;
        public AnimationCurve DamageHorizontalShakeAmount;
        public AnimationCurve DamageVerticalJumpAmount;

        public float DamageGlitchDuration = .25F;

        public float SlowdownDuration;
        public AnimationCurve SlowdownScale;
        public AnimationCurve DesaturateCurve;
        public AnimationCurve DarkenCurve;
        public float BNWDuration = 1;

        private void Start() {
            GlobalEntityDamagedEvent.Instance.AddListener(OnEntityDamaged);
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
                DamageAudio.Execute(currentEntity.Center);
                var meta = new GlitchMeta(
                    DamageLineJitterAmount,
                    DamageVerticalJumpAmount,
                    DamageHorizontalShakeAmount,
                    DamageColorDriftAmount
                );
                damageGlitch.RequesTimedService(DamageGlitchDuration, meta);
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