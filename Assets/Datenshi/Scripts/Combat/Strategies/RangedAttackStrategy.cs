using System;
using Datenshi.Scripts.AI;
using Datenshi.Scripts.Data;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/RangedAttackStrategy")]
    public class RangedAttackStrategy : AttackStrategy {
        public float MinDistance = 5F;
        public float Threshold = 1F;
        public float MinDelayBetweenAttacks = 2;
        public string Attack = "Attack";


        public override void Execute(AIStateInputProvider provider, ICombatant e, ICombatant target) {
            var targetEntityPos = target.Center;
            var entityPos = e.Center;
            var xDir = Math.Sign(entityPos.x - targetEntityPos.x);
            var mEntity = e as IMovableCombatant;
            var agent = mEntity == null ? null : mEntity.AINavigator;
            var targetPos = agent == null ? target.Center : agent.GetFavourablePosition(mEntity);
            var distance = Vector2.Distance(entityPos, targetPos);

            if (distance > MinDistance + Threshold) {
                provider.Attack = false;
                if (agent != null) {
                    agent.Target = targetPos;
                    agent.Execute(mEntity, provider);
                }

                return;
            }

            var lastAttack = e.GetVariable(CombatVariables.LastAttack);
            var time = Time.time;
            var delay = time - lastAttack;
            if (delay < MinDelayBetweenAttacks) {
                provider.Attack = false;
#if UNITY_EDITOR
                Debug.DrawLine(entityPos, targetPos, Color.yellow);
#endif
                return;
            }

            e.SetVariable(CombatVariables.LastAttack, time);
            var dir = e.CurrentDirection;
            dir.X = -xDir;
            e.CurrentDirection = dir;
            provider.Horizontal = 0;
            provider.Walk = true;
            provider.Jump = false;
            e.AnimatorUpdater.TriggerAttack(Attack);
        }

        private string GetStateInfo(float distance) {
            if (distance > MinDistance + Threshold) {
                return "Too far";
            }

            if (distance > MinDistance) {
                return "Within threshold";
            }

            return "Close";
        }

        public override float GetMinimumDistance(ICombatant entity, ICombatant target) {
            return MinDistance;
        }

        public override float GetCost(ICombatant entity, ICombatant target) {
            var dist = Vector2.Distance(entity.Center, target.Center);
            if (dist < MinDistance) {
                return 0;
            }

            return dist;
        }

        public override float GetEffectiveness(ICombatant entity, ICombatant target) {
            return Vector2.Distance(entity.Center, target.Center) - Threshold;
        }

        public override string GetTitle() {
            return "Ranged Strategy";
        }
    }
}