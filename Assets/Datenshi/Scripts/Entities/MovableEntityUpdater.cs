using System.Linq;
using Datenshi.Scripts.Combat;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEditor.Animations;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace Datenshi.Scripts.Entities {
    public class MovableEntityUpdater : CombatantAnimatorUpdater {
        public MovableEntity Entity;
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
        public string HorizontalCollisionKey = "HorizontalCollision";
        public string StunKey = "Stunned";
        public string DeadKey = "Dead";
        public string DefendedKey = "Defended";
        public string SpawnKey = "Spawn";
        public bool InvertFlip;
        public bool RestartGameOnDeath;

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
            if (Entity == null) {
                return;
            }

            Entity.OnDamaged.AddListener(OnDamaged);
        }

        private void OnDamaged(DamageInfo info) {
            Animator.SetTrigger(DamagedKey);
        }

        protected override void UpdateAnimator(Animator anim) {
            if (Entity == null || Entity.MovementConfig == null) {
                return;
            }

            var vel = Entity.Rigidbody.velocity;
            var speed = vel.magnitude;
            var percentSpeed = speed / Entity.MovementConfig.MaxSpeed;
            var velDir = System.Math.Sign(vel.x);
            var provider = Entity.InputProvider;
            anim.SetBool(HorizontalCollisionKey, Entity.CollisionStatus.HorizontalCollisionDir != 0);
            if (provider != null) {
                var inputDir = System.Math.Sign(provider.GetHorizontal());
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
            Entity.ColorizableRenderer.FlipX = Entity.CurrentDirection.X == (InvertFlip ? 1 : -1);
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
            /*if (RestartGameOnDeath) {
                GameController.Instance.RestartGame();
                return;
            }*/

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