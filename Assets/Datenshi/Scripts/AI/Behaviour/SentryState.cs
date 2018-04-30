using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Sentry")]
    public class SentryState : BehaviourState {
        public float SightRadius = 10;

        public override void Execute(AIStateInputProvider provider, Entity entity, DebugInfo info) {
            var e = entity as LivingEntity;
            if (e == null) {
                return;
            }

            var pos = entity.transform.position;
            DebugUtil.DrawWireCircle2D(pos, SightRadius, Color.green);

            foreach (var hit in Physics2D.OverlapCircleAll(pos, SightRadius, GameResources.Instance.EntitiesMask)) {
                var en = hit.GetComponentInParent<LivingEntity>();
                if (!e.ShouldAttack(en)) {
                    continue;
                }

                e.DefaultAttackStrategy.Execute(provider, e, en, info);
                return;
            }
        }

        public override void DrawGizmos(AIStateInputProvider provider, Entity entity, DebugInfo info) {
            CombatDebug.DrawCombatInfo(entity, info);
        }

        public override string GetTitle() {
            return "Sentry State";
        }
    }
}