using System.Diagnostics.CodeAnalysis;
using Datenshi.Scripts.Entities.Misc.Ghosting;
using Datenshi.Scripts.Movement.Config;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Entities.Motor.States {
    public class DashMovement : MovementState<TerrestrialConfig> {
        public float DashDuration = 1;
        public float DashDistance = 8;
        public float DashGravityScale = .25F;
        public MovementState DefaultState;

        [ShowInInspector]
        public float DashSpeed {
            get {
                return DashDistance / DashDuration;
            }
            set {
                DashDistance = value / DashDuration;
            }
        }

        [ShowInInspector, ReadOnly]
        private float currentDuration;

        private bool dashing;

        protected override void OnEnter(MovableEntity entity, TerrestrialConfig config) {
            currentDuration = 0;
            entity.Rigidbody.gravityScale = DashGravityScale;
            SetGhosting(entity, true);
        }

        protected override void OnExit(MovableEntity entity, TerrestrialConfig config, Animator animator) {
            dashing = false;
            SetGhosting(entity, false);
        }


        private static void SetGhosting(Entity entity, bool b) {
            GhostingContainer g;
            if (TryGetGhosting(entity, out g)) {
                g.Spawning = b;
            }
        }


        private static bool TryGetGhosting(Entity entity, out GhostingContainer ghostingContainer) {
            var m = entity.MiscController;
            ghostingContainer = m == null ? null : m.GhostingContainer;
            return ghostingContainer != null;
        }

        [SuppressMessage("ReSharper", "RedundantAssignment")]
        protected override void Move(ref Vector2 velocity, MovableEntity entity, TerrestrialConfig config,
            StateMotor motor) {
            if (!config.DashEllegible || (currentDuration += Time.deltaTime) >= DashDuration) {
                motor.SetState(DefaultState);
            }

            int dir;
            if (dashing) {
                dir = entity.CurrentDirection.X;
            } else {
                dir = System.Math.Sign(entity.InputProvider.GetHorizontal());
            }

            dashing = true;

            velocity = new Vector2(DashSpeed * dir, 0);
        }
    }
}