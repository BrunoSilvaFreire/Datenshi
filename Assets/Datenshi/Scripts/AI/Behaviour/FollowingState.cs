using System;
using Datenshi.Input;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Following")]
    public class FollowingState : BehaviourState {
        public Vector2 Offset;
        public static readonly Variable<Entity> EntityTarget = new Variable<Entity>("entity.state.following.target", null);

        public override void Execute(AIStateInputProvider provider, Entity e) {
            var entity = e as MovableEntity;
            if (entity == null) {
                return;
            }

            var target = entity.GetVariable(EntityTarget);
            if (target == null) {
                return;
            }

            var agent = entity.AINavigator;
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