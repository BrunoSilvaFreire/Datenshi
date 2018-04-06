using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Moving")]
    public class MovingState : BehaviourState {
        public override void Execute(AIStateInputProvider provider, MovableEntity entity) {
            var agent = entity.AIAgent;
            if (agent != null) {
                agent.Execute(entity, provider);
            }
        }
    }
}