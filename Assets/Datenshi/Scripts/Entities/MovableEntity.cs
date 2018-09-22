using Datenshi.Scripts.AI;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class MovableEntity : LivingEntity, INavigable {
        public const string MovementGroup = "Movement";
        public const string AIGroup = "AI";

        [TitleGroup(GeneralGroup)]
        public Rigidbody2D Rigidbody;

        [TitleGroup(MovementGroup)]
        public GameObject RigidStateHolder;

        [TitleGroup(MovementGroup)]
        public float DirectionChangeThreshold = .2F;

        protected override void Start() {
            base.Start();
            foreach (var movement in AnimatorUpdater.Animator.GetBehaviours<AnimatorMovement>()) {
                movement.Initialize(this);
            }
        }

        protected override void Update() {
            base.Update();
            if (AnimatorUpdater != null) {
                AnimatorUpdater.UpdateAnimator();
            }
        }

        private void FixedUpdate() {
            UpdateCollisionStatus();
            if (Motor != null) {
                Motor.Move(this);
            }
        }

        private void LateUpdate() {
            UpdateDirection();
        }


        protected override void OnDrawGizmos() {
            var b = Hitbox.bounds;
            b.center += (Vector3) Rigidbody.velocity * Time.deltaTime;
            Gizmos.color = HitboxColor;
            Gizmos.DrawCube(b.center, b.size);
        }

        [SerializeField, TitleGroup(AIGroup)]
        private AINavigator aiNavigator;

        public AINavigator AINavigator => aiNavigator;
    }

    public static class RigidEntityExtensions {
        public static float GetSpeedPercentage(this MovableEntity entity) {
            return entity.Rigidbody.velocity.magnitude / entity.MovementConfig.MaxSpeed;
        }
    }
}