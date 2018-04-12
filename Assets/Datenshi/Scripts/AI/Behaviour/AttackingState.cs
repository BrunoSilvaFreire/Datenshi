using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Attacking")]
    public class AttackingState : BehaviourState {
        public static readonly Variable<LivingEntity> EntityTarget = new Variable<LivingEntity>("entity.ai.target", null);
        public BehaviourState OnTargetLost;
        public BehaviourState OnEntityKilled;

        public override void Execute(AIStateInputProvider provider, Entity e) {
            var entity = e as LivingEntity;
            if (entity == null) {
                return;
            }
            var target = entity.GetVariable(EntityTarget);
            if (target == null && OnTargetLost != null) {
                provider.CurrentState = OnTargetLost;
                return;
            }
            entity.DefaultAttackStrategy.Execute(provider, entity, target);
        }
    }
}