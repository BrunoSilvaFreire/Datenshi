using Datenshi.Scripts.AI;
using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Sentry")]
    public class SentryState : BehaviourState {
        public float SightRadius = 10;

        public override void Execute(AIStateInputProvider provider, INavigable entity) {
            var e = entity as ICombatant;
            if (e == null) {
                return;
            }

            var pos = entity.Center;
            DebugUtil.DrawWireCircle2D(pos, SightRadius, Color.green);

            foreach (var hit in Physics2D.OverlapCircleAll(pos, SightRadius, GameResources.Instance.EntitiesMask)) {
                var en = hit.GetComponentInParent<ICombatant>();
                if (!e.ShouldAttack(en)) {
                    continue;
                }

                e.AttackStrategy.Execute(provider, e, en);
                return;
            }
        }

        public override void DrawGizmos(AIStateInputProvider provider, INavigable entity) { }

        public override string GetTitle() {
            return "Sentry State";
        }
    }
}