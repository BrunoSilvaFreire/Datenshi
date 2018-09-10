using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Volatiles;
using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement {
    public partial class RigidEntity : LivingEntity {
        public Rigidbody2D Rigidbody;
        public GameObject RigidStateHolder;
        public float SpeedMultiplier;
        public float DirectionChangeThreshold = .2F;

        public RigidEntityConfig Config {
            get;
            private set;
        }

        private void Start() {
            foreach (var movement in AnimatorUpdater.Animator.GetBehaviours<AnimatorMovement>()) {
                movement.Initialize(this);
            }
        }

        protected override void Update() {
            base.Update();
            UpdateCollisionStatus();
        }

        private void LateUpdate() {
            UpdateDirection();
        }

        private void UpdateDirection() {
            if (Defending && DefendingFor > DirectionChangeThreshold) {
                return;
            }

            var newDirection = Direction.FromVector(Rigidbody.velocity);
            var xDir = newDirection.X;
            var yDir = newDirection.Y;
            var dir = CurrentDirection;
            if (xDir != 0 && dir.X != xDir) {
                dir.X = xDir;
            }

            if (yDir != 0 && dir.Y != yDir) {
                dir.Y = yDir;
            }

            CurrentDirection = dir;
        }

        public T GetConfig<T>() where T : RigidEntityConfig {
            if (Config != null) {
                var candidate = Config as T;
                if (candidate != null) {
                    return candidate;
                }

                Destroy(Config);
            }

            return (T) (Config = RigidStateHolder.GetOrAddComponent<T>());
        }

        protected override void OnDrawGizmos() {
            var b = Hitbox.bounds;
            b.center += (Vector3) Rigidbody.velocity * Time.deltaTime;
            Gizmos.color = HitboxColor;
            Gizmos.DrawCube(b.center, b.size);
        }
    }

    public static class RigidEntityExtensions {
        public static float GetSpeedPercentage(this RigidEntity entity) {
            return entity.Rigidbody.velocity.magnitude / entity.Config.MaxSpeed;
        }
    }
}