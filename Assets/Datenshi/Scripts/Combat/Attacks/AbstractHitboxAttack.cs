using Datenshi.Scripts.Data;
using Lunari.Tsuki;
using Lunari.Tsuki.Misc;
using Shiroi.FX.Effects;
using Shiroi.FX.Features;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Datenshi.Scripts.Combat.Attacks {
    public class HitboxAttackExecutedEvent : UnityEvent<ICombatant, uint, Collider2D, DamageInfo> {
        public static readonly HitboxAttackExecutedEvent Instance = new HitboxAttackExecutedEvent();
        private HitboxAttackExecutedEvent() { }
    }

    public abstract class AbstractHitboxAttack : Attack, IDefendable {
        private static readonly Color HitboxColor = Color.green;
        public Effect OnHit, OnHitEnemyEntity;

        public Bounds2D Hitbox;
        public float FocusConsumption = 0.1f;
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
            DebugUtil.DrawWireBox2D(hb.Center, hb.Size, HitboxColor);
            var success = false;
            var found = Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask);
            foreach (var hit in found) {
                var d = hit.GetComponentInParent<IDamageable>();
                if (d == null || d.Ignored) {
                    continue;
                }

                var e = d as ICombatant;
                if (e != null && !entity.ShouldAttack(e)) {
                    continue;
                }

                var info = new DamageInfo(this, 1, d, entity);
                HitboxAttackExecutedEvent.Instance.Invoke(entity, damage, hit, info);
                success = true;
                d.Damage(ref info, this);
                OnHit.PlayIfPresent(new EffectContext(entity as MonoBehaviour,
                    new PositionFeature(hit.transform.position)
                ));
            }

            return success;
        }

/*

        public bool CanAutoDefend(ICombatant combatant) {
            return true;
        }

        public float DoAutoDefend(ICombatant combatant, ref DamageInfo info) {
            info.Canceled = true;
            return FocusConsumption;
        }

        public bool CanAgressiveDefend(ICombatant combatant) {
            return true;
        }

        public float DoAgressiveDefend(ICombatant combatant, ref DamageInfo info) {
            info.Canceled = true;
            return FocusConsumption;
        }

        public bool CanEvasiveDefend(ICombatant combatant) {
            return true;
        }

        public float DoEvasiveDefend(ICombatant combatant, ref DamageInfo info) {
            info.Canceled = true;
            return FocusConsumption;
        }*/
        public bool CanDefend(ICombatant combatant) {
            return true;
        }

        public float Defend(ICombatant combatant, ref DamageInfo info) {
            info.Canceled = true;
            return FocusConsumption;
        }
    }
}