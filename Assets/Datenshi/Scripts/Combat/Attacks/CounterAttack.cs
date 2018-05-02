using System;
using Datenshi.Scripts.Data;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Datenshi/Combat/CounterAttack")]
    public class CounterAttack : Attack {
        public float DefenseRegain = 1.5F;

        public override void Execute(ICombatant entity) {
            var updater = entity.AnimatorUpdater;
            var hb = entity.DefenseHitbox;
            hb.Center.x *= entity.CurrentDirection.X;
            hb.Center += (Vector2) entity.Center;
            var hit = Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask);
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