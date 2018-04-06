using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Following")]
    public class FollowingState : BehaviourState {
        public Vector2 Offset;

        public override void Execute(AIStateInputProvider provider, MovableEntity entity) {
            var target = entity.GetVariable(AttackingState.EntityTarget);
            if (target == null) {
                return;
            }
            var agent = entity.AIAgent;
            agent.Target = (Vector2) target.transform.position + Offset;
            agent.Execute(entity, provider);
        }
    }
}