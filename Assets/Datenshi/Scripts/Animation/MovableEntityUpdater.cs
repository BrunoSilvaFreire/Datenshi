using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Animation {
    public class MovableEntityUpdater : AnimatorUpdater {
        public string SpeedPercentKey = "SpeedPercent";
        public string SpeedRawKey = "SpeedRaw";
        public string GroundedKey = "Grounded";
        public string YSpeedKey = "YSpeed";
        public string AttackKey = "Attack";
        public string DamagedKey = "Damaged";
        public string LastDamageKey = "LastDamage";
        public MovableEntity Entity;
        public SpriteRenderer     Renderer;

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
            anim.SetFloat(YSpeedKey, vel.y);
            anim.SetFloat(SpeedRawKey, speed);
            anim.SetFloat(SpeedPercentKey, percentSpeed);
            anim.SetBool(GroundedKey, Entity.CollisionStatus.Down);
            if (Entity.GetVariable(LivingEntity.Attacking)) {
                Entity.SetVariable(LivingEntity.Attacking, false);
                anim.SetTrigger(AttackKey);
            }

            Renderer.flipX = Entity.CurrentDirection.X == -1;
        }
    }
}