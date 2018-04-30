using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.AI.Behaviour {
    [CreateAssetMenu(menuName = "Datenshi/AI/States/Attacking")]
    public class AttackingState : BehaviourState {
        public BehaviourState OnTargetLost;
        public BehaviourState OnEntityKilled;

        public override void Execute(AIStateInputProvider provider, Entity e, DebugInfo info) {
            var entity = e as LivingEntity;
            if (entity == null) {
                Debug.LogWarning("Need a living entity to use attacking state");
                return;
            }

            var target = entity.GetVariable(CombatVariables.EntityTarget);
            if (target == null) {
#if UNITY_EDITOR
                info.AddInfo("No target found");
#endif
                if (OnTargetLost != null) {
                    provider.CurrentState = OnTargetLost;
#if UNITY_EDITOR
                    info.AddInfo("No fallback state");
#endif
                }

                return;
            }

            var strategy = entity.DefaultAttackStrategy;
            info.CurrentDebugabble = strategy;
            strategy.Execute(provider, entity, target, info);
        }

        public override void DrawGizmos(AIStateInputProvider provider, Entity entity, DebugInfo info) {
            CombatDebug.DrawCombatInfo(entity, info);
        }

        public override string GetTitle() {
            return "Attacking State";
        }
    }
}