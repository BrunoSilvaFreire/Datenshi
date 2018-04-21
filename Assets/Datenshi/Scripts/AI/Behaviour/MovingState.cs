using Datenshi.Input;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Moving")]
    public class MovingState : BehaviourState {
        public override void Execute(AIStateInputProvider provider, Entity e) {
            var entity = e as MovableEntity;
            if (entity == null) {
                return;
            }
            var agent = entity.AINavigator;
            if (agent != null) {
                agent.Execute(entity, provider);
            }
        }
    }
}