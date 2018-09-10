using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement.States {
    public class WallClimbingMovement : AnimatorMovement<GroundedAnimatorConfig> {
        private float enteredAt;
        public AnimationCurve Gravity = AnimationCurve.Linear(0, 0, 3, 39.24F);
        public float WallCounterForce = 5;
        public float WallJumpVerticalForce = 10;

        protected override void OnEnter(RigidEntity entity, GroundedAnimatorConfig config) {
            enteredAt = 0;
            entity.Rigidbody.gravityScale = 0;
        }

        protected override void OnExit(RigidEntity entity, GroundedAnimatorConfig config, Animator animator) { }

        protected override void Move(ref Vector2 velocity, RigidEntity entity, GroundedAnimatorConfig config,
            Animator animator) {
            var coll = entity.CollisionStatus;
            if (coll.HorizontalCollisionDir == 0) {
                return;
            }

            Debug.Log("Velocity set to 0 @ "+coll.HorizontalCollisionDir);
            enteredAt += Time.deltaTime;
            velocity.y = -Gravity.Evaluate(enteredAt);
            velocity.x = 0;
            if (entity.InputProvider.GetJumpDown()) {
                velocity.x = WallCounterForce * coll.HorizontalCollisionDir;
                velocity.y = WallJumpVerticalForce;
            }
        }
    }
}