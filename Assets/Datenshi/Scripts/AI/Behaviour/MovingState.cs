using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Moving")]
    public class MovingState : BehaviourState {
        public override void Execute(AIStateInputProvider provider, Entity e, DebugInfo info) {
            var entity = e as MovableEntity;
            if (entity == null) {
                return;
            }
            var agent = entity.AINavigator;
            if (agent != null) {
                agent.Execute(entity, provider);
            }
        }

        public override void DrawGizmos(AIStateInputProvider provider, Entity entity, DebugInfo info) {
            
        }

        public override string GetTitle() {
            return "Moving State";
        }
    }
}