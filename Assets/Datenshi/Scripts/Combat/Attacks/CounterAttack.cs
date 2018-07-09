using System;
using System.Collections;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Attacks {
    [CreateAssetMenu(menuName = "Datenshi/Combat/CounterAttack")]
    public class CounterAttack : Attack {
        public enum CounterType {
            Agressive,
            Evasive
        }

        public float DefenseRegain = 1.5F;
        public ParticleSystem Particle;
        public byte Frames = 5;
        public CounterType Type;

        public override void Execute(ICombatant entity) {
            entity.AnimatorUpdater.StartCoroutine(DoAttack(entity));
        }

        public override uint GetDamage(ICombatant livingEntity) {
            return 0;
        }

        private IEnumerator DoAttack(ICombatant entity) {
            for (byte i = 0; i < Frames; i++) {
                if (ExecuteAttack(entity)) {
                    yield break;
                }

                yield return null;
            }
        }

        private bool ExecuteAttack(ICombatant entity) {
            var updater = entity.AnimatorUpdater;
            var hb = entity.DefenseHitbox;
            var hit = Physics2D.OverlapBoxAll(hb.Center, hb.Size, 0, GameResources.Instance.EntitiesMask);
            DebugUtil.DrawBox2DWire(hb.Center, hb.Size, Color.green);
            var success = false;
            foreach (var coll in hit) {
                var info = new DamageInfo(this, 1, null, entity);
                var d = coll.GetComponent<IDefendable>();
                if (d == null  
                    // || !CanDefend(d, entity)
                    ) {
                    continue;
                }

                success = true;
                DebugUtil.DrawBox2DWire(((MonoBehaviour) d).gameObject.transform.position, Vector2.one, Color.green);
                //Defend(d, entity, ref info);
                if (Particle != null) {
                    Particle.Clone(coll.transform.position);
                }

                if (updater == null) {
                    continue;
                }
            }

            if (success) {
                entity.FocusTimeLeft += DefenseRegain;
            }

            return success;
        }

        /*private void Defend(IDefendable defendable, ICombatant entity, ref DamageInfo info) {
            switch (Type) {
                case CounterType.Agressive:
                    defendable.AgressiveDefend(entity, ref info);
                    break;
                case CounterType.Evasive:
                    defendable.EvasiveDefend(entity, ref info);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CanDefend(IDefendable defendable, ICombatant entity) {
            switch (Type) {
                case CounterType.Agressive:
                    return defendable.CanAgressiveDefend(entity);
                case CounterType.Evasive:
                    return defendable.CanEvasiveDefend(entity);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }*/
    }
}