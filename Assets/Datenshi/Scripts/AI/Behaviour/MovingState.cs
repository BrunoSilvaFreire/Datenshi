using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Origame/AI/States/Moving")]
    public class MovingState : BehaviourState {
        public override void Execute(AIStateInputProvider provider, MovableEntity entity) {
            var agent = entity.aiAgent;
            if (agent != null) {
                agent.Execute(entity, provider);
            }
        }
    }
}