using System;
using System.Linq;
using Datenshi.Scripts.Combat;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;

#endif

namespace Datenshi.Scripts.Entities.Animation {
    public class MovableEntityUpdater : CombatantAnimatorUpdater {
        public string SpeedPercentKey = "SpeedPercent";
        public string SpeedRawKey = "SpeedRaw";
        public string GroundedKey = "Grounded";
        public string YSpeedKey = "YSpeed";
        public string DamagedKey = "Damaged";
        public string StoppingKey = "Stopping";
        public string AbsInputVerticalKey = "AbsInputVertical";
        public string AbsInputHorizontalKey = "AbsInputHorizontal";
        public string InputVerticalKey = "InputVertical";
        public string InputHorizontalKey = "InputHorizontal";
        public string DefendKey = "Defend";
        public string StunKey = "Stunned";
        public string DeadKey = "Dead";
        public string DefendedKey = "Defended";
        public string SpawnKey = "Spawn";
        public MovableEntity Entity;
        public SpriteRenderer Renderer;
        public bool InvertFlip;
#if UNITY_EDITOR
        [ShowInInspector, UsedImplicitly, Button]
        public void CreateParameters() {
            AddParameter(SpeedPercentKey, AnimatorControllerParameterType.Float);
            AddParameter(SpeedRawKey, AnimatorControllerParameterType.Float);
            AddParameter(GroundedKey, AnimatorControllerParameterType.Bool);
            AddParameter(YSpeedKey, AnimatorControllerParameterType.Float);
            AddParameter(DamagedKey, AnimatorControllerParameterType.Trigger);
            AddParameter(StoppingKey, AnimatorControllerParameterType.Bool);
            AddParameter(AbsInputVerticalKey, AnimatorControllerParameterType.Float);
            AddParameter(AbsInputHorizontalKey, AnimatorControllerParameterType.Float);
            AddParameter(InputVerticalKey, AnimatorControllerParameterType.Float);
            AddParameter(InputHorizontalKey, AnimatorControllerParameterType.Float);
            AddParameter(DefendKey, AnimatorControllerParameterType.Bool);
            AddParameter(StunKey, AnimatorControllerParameterType.Bool);
            AddParameter(DeadKey, AnimatorControllerParameterType.Trigger);
            AddParameter(SpawnKey, AnimatorControllerParameterType.Trigger);
        }

        public void AddParameter(string parameter, AnimatorControllerParameterType type) {
            var c = Animator.runtimeAnimatorController as AnimatorController;
            if (c == null) {
                return;
            }

            if (c.parameters.Any(controllerParameter => controllerParameter.name == parameter)) {
                return;
            }

            c.AddParameter(
                new AnimatorControllerParameter {
                    name = parameter,
                    type = type
                });
        }
#endif
        private void Start() {
            Entity.OnDamaged.AddListener(OnDamaged);
        }

        private void OnDamaged(DamageInfo info) {
            Animator.SetTrigger(DamagedKey);
        }

        protected override void UpdateAnimator(Animator anim) {
            var vel = Entity.Velocity;
            var speed = vel.magnitude;
            var percentSpeed = speed / Entity.MaxSpeed;
            var velDir = Math.Sign(vel.x);
            var provider = Entity.InputProvider;
            if (provider != null) {
                var inputDir = Math.Sign(provider.GetHorizontal());
                anim.SetBool(StoppingKey, inputDir == -velDir);
                var v = provider.GetVertical();
                var h = provider.GetHorizontal();
                anim.SetFloat(InputVerticalKey, v);
                anim.SetFloat(InputHorizontalKey, h);
                anim.SetFloat(AbsInputVerticalKey, Mathf.Abs(v));
                anim.SetFloat(AbsInputHorizontalKey, Mathf.Abs(h));
            }

            anim.SetBool(StunKey, Entity.Stunned);
            anim.SetFloat(YSpeedKey, vel.y);
            anim.SetFloat(SpeedRawKey, speed);
            anim.SetFloat(SpeedPercentKey, percentSpeed);
            var grounded = Entity.CollisionStatus.Down;
            anim.SetBool(GroundedKey, grounded);
            Renderer.flipX = Entity.CurrentDirection.X == (InvertFlip ? 1 : -1);
        }

        public override void TriggerAttack(string attack = DefaultAttackName) {
            Animator.SetTrigger(attack);
        }

        public override void SetDefending(bool defend) {
            Animator.SetBool(DefendKey, defend);
        }

        public override void SetTrigger(string key) {
            Animator.SetTrigger(key);
        }

        public override void TriggerDeath() {
            Animator.SetTrigger(DeadKey);
        }

        public override void TriggerSpawn() {
            Animator.SetTrigger(SpawnKey);
        }

        public override void SetBool(string key, bool p1) {
            Animator.SetBool(key, p1);
        }

        public override void TriggerDefend() {
            Animator.SetTrigger(DefendedKey);
        }
    }
}