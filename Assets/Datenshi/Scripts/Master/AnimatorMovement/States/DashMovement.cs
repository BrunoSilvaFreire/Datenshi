using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Misc.Ghosting;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement.States {
    public class DashMovement : AnimatorMovement<GroundedAnimatorConfig> {
        public float DashDuration = 1;
        public float DashDistance = 8;
        public float DashGravityScale = .25F;

        [ShowInInspector]
        public float DashSpeed {
            get {
                return DashDistance / DashDuration;
            }
            set {
                DashDistance = value / DashDuration;
            }
        }

        public string DashingKey = "Dashing";

        [ShowInInspector, ReadOnly]
        private float currentDuration;

        private bool dashing;

        protected override void OnEnter(RigidEntity entity, GroundedAnimatorConfig config) {
            currentDuration = 0;
            entity.Rigidbody.gravityScale = DashGravityScale;
            SetGhosting(entity, true);
        }

        protected override void OnExit(RigidEntity entity, GroundedAnimatorConfig config, Animator animator) {
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

        protected override void Move(ref Vector2 velocity, RigidEntity entity, GroundedAnimatorConfig config,
            Animator animator) {
            if (!config.DashEllegible || (currentDuration += Time.deltaTime) >= DashDuration) {
                animator.SetBool(DashingKey, false);
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