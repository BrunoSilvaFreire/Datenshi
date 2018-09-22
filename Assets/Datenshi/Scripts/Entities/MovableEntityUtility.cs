using Datenshi.Scripts.Data;
using Datenshi.Scripts.Movement.Config;
using UnityEngine;
using UnityUtilities;

namespace Datenshi.Scripts.Entities {
    public static class MovableEntityUtility {
        public static bool JumpEllegible(this MovableEntity entity) {
            if (entity.CollisionStatus.Down) {
                return true;
            }

            var bounds = entity.Hitbox.bounds;
            var c = entity.GetMovementConfigAs<TerrestrialConfig>();
            if (c == null) {
                return false;
            }

            var min = bounds.min;
            var maxX = bounds.max.x;
            min.y += entity.Rigidbody.velocity.y * Time.deltaTime;
            var max = new Vector2(maxX, min.y);
            min.y -= c.RejumpLength;
            //DebugUtil.DrawArea(min, max, Color.red);
            return Physics2D.OverlapArea(min, max, GameResources.Instance.WorldMask);
        }
    }
}