using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Graphics;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using Shiroi.FX.Services.BuiltIn;
using Shiroi.FX.Utilities;
using UnityEngine;

namespace Datenshi.Scripts.Master {
    public class DamageFXHandler : MonoBehaviour {
        public Effect DamageAudio;
        public DamagePopUpPool DamagePopUpPool;
        public AnimationCurve DamageLineJitterAmount;
        public AnimationCurve DamageColorDriftAmount;
        public AnimationCurve DamageHorizontalShakeAmount;
        public AnimationCurve DamageVerticalJumpAmount;
        public Color DefaultDamageColor;
        public float DamageGlitchDuration = .25F;
        public float DamagePopUpHeight = 2;
        public float SlowdownDuration;
        public AnimationCurve SlowdownScale;
        public AnimationCurve DesaturateCurve;
        public AnimationCurve DarkenCurve;
        public float BNWDuration = 1;

        private void Start() {
            GlobalEntityDamagedEvent.Instance.AddListener(OnEntityDamaged);
            GlobalDamageEvent.Instance.AddListener(OnDamaged);
            GlobalDefenseEvent.Instance.AddListener(OnDefended);
        }

        private void AttemptVibrate(DamageInfo info) {
            AttemptVibrate(info.Damager, info.Damaged);
        }

        private void AttemptVibrate(IDamageDealer damager, IDamageable damaged) {
            var currentEntity = PlayerController.Instance.CurrentEntity;
            if (!Equals(damager, currentEntity) && !Equals(damaged, currentEntity)) {
                return;
            }

            Vibrate();
        }

        private void OnDefended(ICombatant ignored, DamageInfo arg0) {
            AttemptVibrate(arg0);
        }

        private void OnDamaged(DamageInfo arg0) {
            AttemptVibrate(arg0);
        }

        private void OnEntityDamaged(ICombatant damaged, IDamageDealer damager, IDamageSource attack, uint damage) {
            var currentEntity = PlayerController.Instance.CurrentEntity;
            var color = GetColor(damager);
            DamagePopUpPool.Get().Play(damaged.Transform.position + Vector3.up * DamagePopUpHeight, damage, color, popUp
                => {
                popUp.OnFinished.RemoveAllListeners();
                DamagePopUpPool.Return(popUp);
            });
            if (!Equals(damager, currentEntity) && !Equals(damaged, currentEntity)) {
                return;
            }

            Vibrate();

            if (Equals(damaged, currentEntity) && !damaged.Dead) {
                var graphics = GraphicsSingleton.Instance;
                var damageGlitch = graphics.Glitch;
                var bnw = graphics.BlackAndWhite;
                DamageAudio.Play(new EffectContext(this, new PositionFeature(currentEntity.Center)));
                var meta = new GlitchMeta(
                    DamageLineJitterAmount,
                    DamageVerticalJumpAmount,
                    DamageHorizontalShakeAmount,
                    DamageColorDriftAmount
                );
                damageGlitch.RegisterTimedService(DamageGlitchDuration, meta);
                bnw.RegisterTimedService(BNWDuration, new BlackAndWhiteMeta(DesaturateCurve, DarkenCurve));
            }

            TimeController.Instance.RegisterTimedService(SlowdownDuration, new AnimatedTimeMeta(SlowdownScale),
                priority: 200);
        }

        private Color GetColor(IDamageDealer damager) {
            var entity = damager as Entity;
            if (entity == null) {
                return DefaultDamageColor;
            }

            var character = entity.Character;
            return character == null ? DefaultDamageColor : character.SignatureColor;
        }

        private Coroutine vibrateCoroutine;
        public float VibrationDuration = .1F;
        public float VibrationLevel = .25F;

        private void Vibrate() {
            PlayerController.Instance.Player.CurrentPlayer.SetVibration(0, VibrationLevel, VibrationDuration);
        }
    }
}