using Datenshi.Scripts.AI;
using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Data;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Attacking")]
    public class AttackingState : BehaviourState {
        public BehaviourState OnTargetLost;
        public BehaviourState OnINavigableKilled;

        public override void Execute(AIStateInputProvider provider, INavigable e) {
            var entity = e as ICombatant;
            if (entity == null) {
                Debug.LogWarning("Need a living entity to use attacking state");
                return;
            }

            var target = entity.GetVariable(CombatVariables.AttackTarget);
            if (target == null) {
                if (OnTargetLost != null) {
                    provider.CurrentState = OnTargetLost;
                }

                return;
            }

            var strategy = entity.AttackStrategy;
            strategy.Execute(provider, entity, target);
        }

        public override void DrawGizmos(AIStateInputProvider provider, INavigable entity) {
        }

        public override string GetTitle() {
            return "Attacking State";
        }
    }
}