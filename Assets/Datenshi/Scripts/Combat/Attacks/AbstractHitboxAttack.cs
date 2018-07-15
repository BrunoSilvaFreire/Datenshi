using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UPM.Util;

namespace Datenshi.Scripts.Combat.Attacks {
    public class HitboxAttackExecutedEvent : UnityEvent<ICombatant, uint, Collider2D[]> {
        public static readonly HitboxAttackExecutedEvent Instance = new HitboxAttackExecutedEvent();
        private HitboxAttackExecutedEvent() { }
    }

    public abstract class AbstractHitboxAttack : Attack, IDefendable {
        private static readonly Color HitboxColor = Color.green;
        public ParticleSystem Particle;

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
            DebugUtil.DrawBox2DWire(hb.Center, hb.Size, HitboxColor);
            var success = false;
            var found = Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask);
            HitboxAttackExecutedEvent.Instance.Invoke(entity, damage, found);
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
                success = true;
                d.Damage(ref info, this);
                if (Particle != null) {
                    Particle.Clone(d.Center);
                }
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