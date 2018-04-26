using Datenshi.Input;
using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Misc;
using Datenshi.Scripts.Util;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/CQBAttackStrategy")]
    public class CQBAttackStategy : AttackStrategy {
        public float MinDistance = 1F;
        public float MinDelayBetweenAttacks = 1F;
        public string Attack = "attack";

        public override void Execute(AIStateInputProvider provider, LivingEntity e, LivingEntity target) {
            var entityPos = e.Center;
            var targetPos = target.Center;
            DebugUtil.DrawWireCircle2D(entityPos, MinDistance, Color.magenta);
            if (Vector2.Distance(entityPos, targetPos) > MinDistance) {
                provider.Attack = false;
                var movableEntity = e as MovableEntity;
#if UNITY_EDITOR
                Debug.DrawLine(entityPos, targetPos, Color.red);
#endif
                if (movableEntity == null) {
                    return;
                }

                var agent = movableEntity.AINavigator;
                agent.Target = targetPos;
                agent.Execute(movableEntity, provider);

                return;
            }

            var lastAttack = target.GetVariable(CombatVariables.LastAttack);
            var time = Time.time;
            if (time - lastAttack < MinDelayBetweenAttacks) {
                provider.Attack = false;
#if UNITY_EDITOR
                Debug.DrawLine(entityPos, targetPos, Color.yellow);
#endif
                return;
            }
#if UNITY_EDITOR
            Debug.DrawLine(entityPos, targetPos, Color.green);
#endif

            target.SetVariable(CombatVariables.LastAttack, time);
            e.SetVariable(CombatVariables.EntityTarget, target);
            provider.Horizontal = 0;
            provider.Vertical = 0;
            provider.Jump = false;
            e.AnimatorUpdater.TriggerAttack(Attack);
        }

        public override float GetMinimumDistance(LivingEntity entity, LivingEntity target) {
            return MinDistance;
        }

        public override float GetCost(LivingEntity entity, LivingEntity target) {
            var dist = Vector2.Distance(entity.Center, target.Center);
            if (dist < MinDistance) {
                return 0;
            }

            return dist;
        }

        public override float GetEffectiveness(LivingEntity entity, LivingEntity target) {
            return MinDistance - Vector2.Distance(entity.Center, target.Center);
        }
    }
}