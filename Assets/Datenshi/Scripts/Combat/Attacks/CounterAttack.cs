using System;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Interaction;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Datenshi/Combat/CounterAttack")]
    public class CounterAttack : Attack {
        public float DefenseRegain = 1.5F;

        public override void Execute(LivingEntity entity) {
            var updater = entity.AnimatorUpdater;
            var hb = entity.DefaultAttackHitbox;
            hb.Center.x *= entity.CurrentDirection.X;
            hb.Center += (Vector2) entity.transform.position;
            var hit = Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask);
            DebugUtil.DrawBounds2D(hb, Color.cyan);
            DebugUtil.DrawBounds2D(new Bounds(entity.transform.position, Vector3.one), Color.cyan);
            var success = false;
            foreach (var coll in hit) {
                var d = coll.GetComponent<IDefendable>();
                if (d == null || !d.CanDefend(entity)) {
                    continue;
                }
                success = true;
                d.Defend(entity);
                if (updater == null) {
                    continue;
                }

                switch (d.GetDefenseType()) {
                    case DefenseType.Deflect:
                        updater.TriggerDeflect();
                        break;
                    case DefenseType.Counter:
                        updater.TriggerCounter();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            if (success) {
                entity.FocusTimeLeft += DefenseRegain;
            }
        }
    }
}