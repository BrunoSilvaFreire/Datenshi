using System;
using Datenshi.Input;
using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Entities;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/RangedAttackStrategy")]
    public class RangedAttackStrategy : AttackStrategy {
        public float MinDistance = 5F;
        public float Threshold = 1F;
        public float MinDelayBetweenAttacks = 2;
        public string Attack = "attack";


        public override void Execute(AIStateInputProvider provider, LivingEntity e, LivingEntity target) {
            var targetEntityPos = target.Center;
            var entityPos = e.Center;
            var xDir = Math.Sign(entityPos.x - targetEntityPos.x);
            var mEntity = e as MovableEntity;
            var agent = mEntity == null ? null : mEntity.AINavigator;
            var targetPos = agent == null ? target.Center : agent.GetFavourablePosition(this, target);
            if (Vector2.Distance(entityPos, targetPos) > MinDistance + Threshold) {
                provider.Attack = false;
#if UNITY_EDITOR
                Debug.DrawLine(entityPos, targetPos, Color.red);
#endif
                if (agent == null) {
                    return;
                }

                agent.Target = targetPos;
                agent.Execute(mEntity, provider);
                return;
            }

            var lastAttack = e.GetVariable(CombatVariables.LastAttack);
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
            e.SetVariable(CombatVariables.LastAttack, time);
            e.SetVariable(CombatVariables.EntityTarget, target);
            e.CurrentDirection.X = -xDir;
            provider.Horizontal = 0;
            provider.Walk = true;
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
            return Vector2.Distance(entity.Center, target.Center) - Threshold;
        }
    }
}