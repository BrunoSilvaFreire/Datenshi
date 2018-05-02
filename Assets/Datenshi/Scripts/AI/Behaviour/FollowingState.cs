using System;
using Datenshi.Scripts.Data;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Following")]
    public class FollowingState : BehaviourState {
        public Vector2 Offset;
        public static readonly Variable<INavigable> FollowTarget = new Variable<INavigable>("entity.state.following.target", null);

        public override void Execute(AIStateInputProvider provider, INavigable entity) {
            if (entity == null) {
                return;
            }

            var target = entity.GetVariable(FollowTarget);
            if (target == null) {
                return;
            }

            var agent = entity.AINavigator;
            var pos = entity.Center;
            var targetPos = target.Center;
            var x = targetPos.x + Math.Sign(pos.x - targetPos.x) * Offset.x;
            var y = targetPos.y + Offset.y;
            var finalPos = new Vector2(x, y);
            agent.Target = finalPos;
            agent.Execute(entity, provider);
        }

        public override void DrawGizmos(AIStateInputProvider provider, INavigable entity) {
        
        }

        public override string GetTitle() {
            return "Following State";
        }
    }
}