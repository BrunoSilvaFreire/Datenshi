using Datenshi.Scripts.Movement.Config;
using Datenshi.Scripts.Util;
using Datenshi.Scripts.Util.Buffs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities {
    public partial class MovableEntity {
        [SerializeField, BoxGroup(MovementGroup)]
        private FloatProperty speedMultiplier;

        public FloatProperty SpeedMultiplier {
            get {
                return speedMultiplier;
            }
            set {
                speedMultiplier = value;
            }
        }

        [ShowInInspector, BoxGroup(MovementGroup)]
        public Vector2 ExternalForces {
            get;
            set;
        }

        [SerializeField, BoxGroup(MovementGroup)]
        private bool applyVelocity;

        public bool ApplyVelocity {
            get {
                return applyVelocity;
            }
            set {
                applyVelocity = value;
            }
        }

        public Vector2 Velocity {
            get {
                return Rigidbody.velocity;
            }
            set {
                Rigidbody.velocity = value;
            }
        }

        [SerializeField, BoxGroup(MovementGroup), InlineEditor]
        private MovementConfig config;

        [BoxGroup(MovementGroup), InlineEditor()]
        public Motor.Motor Motor;

        public MovementConfig MovementConfig {
            get {
                return config;
            }
            set {
                config = value;
            }
        }

        public T GetMovementConfigAs<T>() where T : MovementConfig {
            if (config != null) {
                var candidate = config as T;
                if (candidate != null) {
                    return candidate;
                }

                Destroy(config);
            }

            return (T) (config = RigidStateHolder.GetOrAddComponent<T>());
        }

        private void UpdateRigidBody() {
            Debug.Log($"Updating rigid body to {Velocity}");
            Rigidbody.velocity = Velocity;
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
    }
}