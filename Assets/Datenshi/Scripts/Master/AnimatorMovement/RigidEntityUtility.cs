using Datenshi.Scripts.Data;
using Datenshi.Scripts.Master.AnimatorMovement.States;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Master.AnimatorMovement {
    public static class RigidEntityUtility {
        public static bool JumpEllegible(this RigidEntity entity) {
            if (entity.CollisionStatus.Down) {
                return true;
            }

            var bounds = entity.Hitbox.bounds;
            var c = entity.GetConfig<GroundedAnimatorConfig>();
            if (c == null) {
                return false;
            }

            var min = bounds.min;
            var maxX = bounds.max.x;
            min.y += entity.Rigidbody.velocity.y * Time.deltaTime;
            var max = new Vector2(maxX, min.y);
            min.y -= c.RaycastLength;
            DebugUtil.DrawArea(min, max, Color.red);
            return Physics2D.OverlapArea(min, max, GameResources.Instance.WorldMask);
        }
    }
}