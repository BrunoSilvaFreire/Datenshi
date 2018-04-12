using System;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Following")]
    public class FollowingState : BehaviourState {
        public Vector2 Offset;

        public override void Execute(AIStateInputProvider provider, Entity e) {
            var entity = e as MovableEntity;
            if (entity == null) {
                return;
            }
            var target = entity.GetVariable(AttackingState.EntityTarget);
            if (target == null) {
                return;
            }
            var agent = entity.AIAgent;
            var pos = entity.transform.position;
            var targetPos = target.transform.position;
            var x = targetPos.x + Math.Sign(pos.x - targetPos.x) * Offset.x;
            var y = targetPos.y + Offset.y;
            var finalPos = new Vector2(x, y);
            agent.Target = finalPos;
            if (Vector2.Distance(finalPos, pos) > finalPos.magnitude) {
                provider.Reset();
            } else {
                agent.Execute(entity, provider);
            }
        }
    }
}