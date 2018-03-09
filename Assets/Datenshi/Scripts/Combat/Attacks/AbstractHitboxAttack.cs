using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    public abstract class AbstractHitboxAttack : Attack {
        public Bounds2D Hitbox;

        protected void DoAttack(LivingEntity entity, uint damage) {
            var hb = Hitbox;
            var xDir = entity.CurrentDirection.X;
            if (xDir != 0) {
                hb.Center.x *= xDir;
            }

            hb.Center += (Vector2) entity.transform.position;
            DebugUtil.DrawBounds2D(hb, Color.red);
            foreach (var hit in Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask)) {
                var e = hit.GetComponentInParent<LivingEntity>();
                if (e == null || e == entity || e.Relationship == entity.Relationship) {
                    continue;
                }

                e.Damage(entity, damage);
            }
        }
    }
}