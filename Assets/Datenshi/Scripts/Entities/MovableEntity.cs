using Datenshi.Scripts.AI;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Buffs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class MovableEntity : LivingEntity, INavigable {
        public const string MovementGroup = "Movement";
        public const string AIGroup = "AI";

        [BoxGroup(GeneralGroup)]
        public Rigidbody2D Rigidbody;

        [BoxGroup(MovementGroup)]
        public GameObject RigidStateHolder;

        [BoxGroup(MovementGroup)]
        public float DirectionChangeThreshold = .2F;

        protected override void Start() {
            base.Start();
        }

        protected override void Update() {
            base.Update();
            UpdateAI();
            speedMultiplier.Tick();
        }

        private void FixedUpdate() {
            UpdateCollisionStatus();

            if (Motor != null) {
                Motor.Move(this);
            }
        }

        private void LateUpdate() {
            UpdateDirection();
            if (AnimatorUpdater != null) {
                AnimatorUpdater.UpdateAnimator();
            }
        }


        protected override void OnDrawGizmos() {
            var b = Hitbox.bounds;
            b.center += (Vector3) Rigidbody.velocity * Time.deltaTime;
            Gizmos.color = HitboxColor;
            Gizmos.DrawCube(b.center, b.size);
        }
    }

    public static class RigidEntityExtensions {
        public static float GetSpeedPercentage(this MovableEntity entity) {
            return entity.Rigidbody.velocity.magnitude / entity.MovementConfig.MaxSpeed;
        }
    }
}