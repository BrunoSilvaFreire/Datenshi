using BehaviorDesigner.Runtime.Tasks;
using Datenshi.Scripts.Behaviours.Variables;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Data;
using Datenshi.Scripts.Entities;
using Lunari.Tsuki;
using UnityEngine;

namespace Datenshi.Scripts.Behaviours.Tasks {
    public class SentryTask : Action {
        public LivingEntity Entity;
        public float SightRadius;
        public SharedCombatant Target;

        public override TaskStatus OnUpdate() {
            var e = Entity as ICombatant;
            if (e == null) {
                return TaskStatus.Failure;
            }

            foreach (var hit in Physics2D.OverlapCircleAll(Entity.Center, SightRadius, GameResources.Instance.EntitiesMask)) {
                var en = hit.GetComponentInParent<ICombatant>();
                if (!e.ShouldAttack(en)) {
                    continue;
                }

                Target.Value = en;
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        public override void OnDrawGizmos() {
            DebugUtil.DrawWireCircle2D(Entity.Center, SightRadius, Color.green);
        }
    }
}