using System;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Animation {
    public class MovableEntityUpdater : EntityAnimatorUpdater {
        public string SpeedPercentKey = "SpeedPercent";
        public string SpeedRawKey = "SpeedRaw";
        public string GroundedKey = "Grounded";
        public string YSpeedKey = "YSpeed";
        public string AttackKey = "Attack";
        public string DamagedKey = "Damaged";
        public string StoppingKey = "Stopping";
        public string AbsInputVerticalKey = "AbsInputVertical";
        public string AbsInputHorizontalKey = "AbsInputHorizontal";
        public string InputVerticalKey = "InputVertical";
        public string InputHorizontalKey = "InputHorizontal";
        public string LastDamageKey = "LastDamage";
        public string BecameGroundedKey = "BecameGrounded";
        public string DefendKey = "Defend";
        public string DeflectKey = "Deflect";
        public string CounterKey = "Counter";
        public string BecameAiredKey = "BecameAired";
        public string StunKey = "Stunned";
        public MovableEntity Entity;
        public SpriteRenderer Renderer;

        private void Awake() {
            Entity.OnDamaged.AddListener(OnDamaged);
        }

        private void OnDamaged(LivingEntity arg0, uint arg1) {
            Animator.SetTrigger(DamagedKey);
            Animator.SetInteger(LastDamageKey, (int) arg1);
        }

        protected override void UpdateAnimator(Animator anim) {
            var vel = Entity.Velocity;
            var speed = vel.magnitude;
            var percentSpeed = speed / Entity.MaxSpeed;
            var velDir = Math.Sign(vel.x);
            var provider = Entity.InputProvider;
            if (provider != null) {
                var inputDir = Math.Sign(provider.GetHorizontal());
                anim.AttemptSetBool(StoppingKey, inputDir == -velDir);
                var v = provider.GetVertical();
                var h = provider.GetHorizontal();
                anim.AttemptSetFloat(InputVerticalKey, v);
                anim.AttemptSetFloat(InputHorizontalKey, h);
                anim.AttemptSetFloat(AbsInputVerticalKey, Mathf.Abs(v));
                anim.AttemptSetFloat(AbsInputHorizontalKey, Mathf.Abs(h));
            }

            anim.AttemptSetBool(StunKey, Entity.Stunned);
            anim.AttemptSetFloat(YSpeedKey, vel.y);
            anim.AttemptSetFloat(SpeedRawKey, speed);
            anim.AttemptSetFloat(SpeedPercentKey, percentSpeed);
            var grounded = Entity.CollisionStatus.Down;
            var wasGrounded = anim.GetBool(GroundedKey);
            if (wasGrounded != grounded) {
                anim.AttemptSetTrigger(grounded ? BecameGroundedKey : BecameAiredKey);
            }

            anim.AttemptSetBool(GroundedKey, grounded);


            Renderer.flipX = Entity.CurrentDirection.X == -1;
        }

        public override void TriggerAttack() {
            Animator.AttemptSetTrigger(AttackKey);
        }

        public override void SetDefend(bool defend) {
            Animator.AttemptSetBool(DefendKey, defend);
        }

        public override void TriggerDeflect() {
            Animator.AttemptSetTrigger(DeflectKey);
        }

        public override void TriggerCounter() {
            Animator.AttemptSetTrigger(CounterKey);
        }
    }
}