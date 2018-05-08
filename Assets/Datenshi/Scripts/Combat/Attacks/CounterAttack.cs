using System;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Datenshi/Combat/CounterAttack")]
    public class CounterAttack : Attack {
        public float DefenseRegain = 1.5F;
        public ParticleSystem Particle;

        public override void Execute(ICombatant entity) {
            var updater = entity.AnimatorUpdater;
            var hb = entity.DefenseHitbox;
            var hit = Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask);
            DebugUtil.DrawBox2DWire(hb.Center, hb.Size, Color.green);
            var success = false;
            foreach (var coll in hit) {
                var info = new DamageInfo(entity, 0);
                var d = coll.GetComponent<IDefendable>();
                if (d == null || !d.CanDefend(entity)) {
                    continue;
                }

                success = true;
                DebugUtil.DrawBox2DWire(((MonoBehaviour) d).gameObject.transform.position, Vector2.one, Color.green);
                d.Defend(entity, ref info);
                if (Particle != null) {
                    Particle.Clone(coll.transform.position);
                }

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