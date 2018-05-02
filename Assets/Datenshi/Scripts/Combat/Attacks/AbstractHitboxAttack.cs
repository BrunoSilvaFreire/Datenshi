using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEngine;
using UPM.Util;

namespace Datenshi.Scripts.Combat.Attacks {
    public abstract class AbstractHitboxAttack : Attack {
        public static readonly Variable<bool> Blocked = new Variable<bool>("entity.combat.cqb.blocked", true);
        private static readonly Color HitboxColor = Color.green;
        public Bounds2D Hitbox;

        protected void DoAttack(ICombatant entity, uint damage) {
            var hb = Hitbox;
            var xDir = entity.CurrentDirection.X;
            if (xDir != 0) {
                hb.Center.x *= xDir;
            }

            hb.Center += (Vector2) entity.Center;
            DebugUtil.DrawBox2DWire(hb.Center, hb.Size, HitboxColor);
            var window = entity.Transform.gameObject.GetComponentInChildren<HitboxAttackCounterWindow>();
            if (window != null) {
                window.Available = true;
            }

            foreach (var hit in Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask)) {
                var d = hit.GetComponentInParent<IDefendable>();
                if (d != null && d.CanPoorlyDefend(entity)) {
                    var w = d as HitboxAttackCounterWindow;
                    if (w != null) {
                        w.AttackDamage = damage;
                    }

                    d.PoorlyDefend(entity);
                }

                if (entity.GetVariable(Blocked)) {
                    entity.SetVariable(Blocked, false);
                    return;
                }

                var e = hit.GetComponentInParent<ICombatant>();
                if (e != null && e.Ignored) {
                    continue;
                }

                if (e == null || e == entity || e.Relationship == entity.Relationship) {
                    continue;
                }

                e.Damage(entity, damage);
            }

            if (window != null) {
                window.Available = false;
            }
        }
    }
}