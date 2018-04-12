using Datenshi.Scripts.Combat.Attacks;
using Datenshi.Scripts.Combat.Strategies;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Game;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Sentry")]
    public class SentryState : BehaviourState {
        public float SightRadius = 10;
        public float AttackCooldown = 3;

        public override void Execute(AIStateInputProvider provider, Entity entity) {
            var e = entity as LivingEntity;
            if (e == null) {
                return;
            }
            var lastAttack = e.GetVariable(CQBAttackStategy.LastAttack);
            var time = Time.time;
            if (time - lastAttack < AttackCooldown) {
                return;
            }
            var pos = entity.transform.position;
            DebugUtil.DrawBox2DWire(pos, new Vector2(SightRadius, SightRadius), Color.green);

            foreach (var hit in Physics2D.OverlapCircleAll(pos, SightRadius, GameResources.Instance.EntitiesMask)) {
                var en = hit.GetComponentInParent<LivingEntity>();
                if (en == null || en == entity || e.Relationship == en.Relationship) {
                    continue;
                }
                entity.SetVariable(AttackingState.EntityTarget, en);
                var targetPos = en.transform.position;
                var dir = targetPos - pos;
                dir.Normalize();
                provider.Horizontal = dir.x;
                provider.Vertical = dir.y;
                provider.Attack = true;
                e.SetVariable(CQBAttackStategy.LastAttack, time);
                entity.AnimatorUpdater.TriggerAttack();
                return;
            }
        }
    }
}