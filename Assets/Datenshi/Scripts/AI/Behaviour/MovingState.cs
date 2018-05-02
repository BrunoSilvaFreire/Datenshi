using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Moving")]
    public class MovingState : BehaviourState {
        public override void Execute(AIStateInputProvider provider, INavigable entity) {
            var agent = entity.AINavigator;
            if (agent != null) {
                agent.Execute(entity, provider);
            }
        }

        public override void DrawGizmos(AIStateInputProvider provider, INavigable entity) {
            
        }

        public override string GetTitle() {
            return "Moving State";
        }
    }
}