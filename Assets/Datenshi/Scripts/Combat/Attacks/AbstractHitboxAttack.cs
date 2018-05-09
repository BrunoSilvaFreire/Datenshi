using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UPM.Util;

namespace Datenshi.Scripts.Combat.Attacks {
    public abstract class AbstractHitboxAttack : Attack {
        public static readonly Variable<bool> Blocked = new Variable<bool>("entity.combat.cqb.blocked", true);
        private static readonly Color HitboxColor = Color.green;
        public Bounds2D Hitbox;
#if UNITY_EDITOR

        [ShowInInspector]
        public Collider2D CopyFrom {
            get {
                return null;
            }
            set {
                Hitbox.Center = value.offset;
                Hitbox.Size = value.bounds.size;
            }
        }
#endif

        protected bool DoAttack(ICombatant entity, uint damage) {
            var hb = Hitbox;
            var xDir = entity.CurrentDirection.X;
            if (xDir != 0) {
                hb.Center.x *= xDir;
            }

            hb.Center += (Vector2) entity.Transform.position;
            DebugUtil.DrawBox2DWire(hb.Center, hb.Size, HitboxColor);
            var success = false;
            foreach (var hit in Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask)) {
                var info = new DamageInfo(entity, damage);
                var d = hit.GetComponentInParent<IDefendable>();
                if (d != null && d.CanPoorlyDefend(entity)) {
                    success = true;
                    d.PoorlyDefend(entity, ref info);
                }

                if (entity.GetVariable(Blocked)) {
                    entity.SetVariable(Blocked, false);
                    continue;
                }

                var e = hit.GetComponentInParent<ICombatant>();
                if (e != null && e.Ignored) {
                    continue;
                }

                if (e == null || e == entity || e.Relationship == entity.Relationship) {
                    continue;
                }

                if (!info.Canceled) {
                    success = true;
                    e.Damage(entity, damage);
                }
            }

            return success;
        }
    }
}