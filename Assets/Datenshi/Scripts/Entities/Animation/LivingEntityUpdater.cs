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
    public class LivingEntityUpdater : CombatantAnimatorUpdater {
        public string AttackKey = "Attack";
        public string DamagedKey = "Damaged";
        public string AbsInputVerticalKey = "AbsInputVertical";
        public string AbsInputHorizontalKey = "AbsInputHorizontal";
        public string InputVerticalKey = "InputVertical";
        public string InputHorizontalKey = "InputHorizontal";
        public string LastDamageKey = "LastDamage";
        public string DefendKey = "Defend";
        public string DeflectKey = "Deflect";
        public string CounterKey = "Counter";
        public string StunKey = "Stunned";
        public string DeadKey = "Dead";
        public string SpawnKey = "Spawn";
        public LivingEntity Entity;
        public SpriteRenderer Renderer;
#if UNITY_EDITOR

        [ShowInInspector, UsedImplicitly, Button]
        public void CreateParameters() {
            AddParameter(AttackKey, AnimatorControllerParameterType.Trigger);
            AddParameter(DamagedKey, AnimatorControllerParameterType.Trigger);
            AddParameter(AbsInputVerticalKey, AnimatorControllerParameterType.Float);
            AddParameter(AbsInputHorizontalKey, AnimatorControllerParameterType.Float);
            AddParameter(InputVerticalKey, AnimatorControllerParameterType.Float);
            AddParameter(InputHorizontalKey, AnimatorControllerParameterType.Float);
            AddParameter(LastDamageKey, AnimatorControllerParameterType.Int);
            AddParameter(DefendKey, AnimatorControllerParameterType.Bool);
            AddParameter(DeflectKey, AnimatorControllerParameterType.Trigger);
            AddParameter(CounterKey, AnimatorControllerParameterType.Trigger);
            AddParameter(StunKey, AnimatorControllerParameterType.Bool);
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

        private void Awake() {
            Entity.OnDamaged.AddListener(OnDamaged);
        }

        private void OnDamaged(ICombatant combatant, uint arg1) {
            Debug.Log("Damaged found");
            Animator.SetTrigger(DamagedKey);
            Animator.SetInteger(LastDamageKey, (int) arg1);
        }

        protected override void UpdateAnimator(Animator anim) {
            var provider = Entity.InputProvider;
            if (provider != null) {
                var inputDir = Math.Sign(provider.GetHorizontal());
                var v = provider.GetVertical();
                var h = provider.GetHorizontal();
                anim.SetFloat(InputVerticalKey, v);
                anim.SetFloat(InputHorizontalKey, h);
                anim.SetFloat(AbsInputVerticalKey, Mathf.Abs(v));
                anim.SetFloat(AbsInputHorizontalKey, Mathf.Abs(h));
            }

            anim.SetBool(StunKey, Entity.Stunned);
            Renderer.flipX = Entity.CurrentDirection.X == -1;
        }

        public override void TriggerAttack() {
            Animator.SetTrigger(AttackKey);
        }

        public override void TriggerAttack(string attack) {
            Animator.SetTrigger(attack);
        }

        public override void SetDefend(bool defend) {
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

        public override void SetBool(string key, bool value) {
            Animator.SetBool(key, value);
        }
    }
}