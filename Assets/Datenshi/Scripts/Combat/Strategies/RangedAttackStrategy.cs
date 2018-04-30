using System;
using Datenshi.Scripts.AI.Behaviour;
using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/RangedAttackStrategy")]
    public class RangedAttackStrategy : AttackStrategy {
        public float MinDistance = 5F;
        public float Threshold = 1F;
        public float MinDelayBetweenAttacks = 2;
        public string Attack = "Attack";


        public override void Execute(AIStateInputProvider provider, LivingEntity e, LivingEntity target, DebugInfo info) {
            var targetEntityPos = target.Center;
            var entityPos = e.Center;
            var xDir = Math.Sign(entityPos.x - targetEntityPos.x);
            var mEntity = e as MovableEntity;
            var agent = mEntity == null ? null : mEntity.AINavigator;
            var targetPos = agent == null ? target.Center : agent.GetFavourablePosition(this, target);
            var distance = Vector2.Distance(entityPos, targetPos);
#if UNITY_EDITOR
            var msg = "Distance to target: " + distance + " (" + GetStateInfo(distance) + ")";
            Debug.Log(msg);
            info.AddInfo(msg);
#endif
            if (distance > MinDistance + Threshold) {
                provider.Attack = false;
#if UNITY_EDITOR
                Debug.DrawLine(entityPos, targetPos, Color.red);
                var targetMsg = "target: " + targetPos;
#endif
                if (agent != null) {
                    agent.Target = targetPos;
                    agent.Execute(mEntity, provider);
#if UNITY_EDITOR
                    targetMsg += string.Format(" (Navigator: {0})", agent);
#endif
                }
#if UNITY_EDITOR
                info.AddInfo(targetMsg);
#endif
                return;
            }

            var lastAttack = e.GetVariable(CombatVariables.LastAttack);
            var time = Time.time;
            var delay = time - lastAttack;
#if UNITY_EDITOR
            info.AddInfo("Delay: " + delay + "/" + MinDelayBetweenAttacks);
#endif
            if (delay < MinDelayBetweenAttacks) {
                provider.Attack = false;
#if UNITY_EDITOR
                Debug.DrawLine(entityPos, targetPos, Color.yellow);
#endif
                return;
            }
#if UNITY_EDITOR
            Debug.DrawLine(entityPos, targetPos, Color.green);
#endif
            info.AddInfo("Attack: " + Attack);
            e.SetVariable(CombatVariables.LastAttack, time);
            e.CurrentDirection.X = -xDir;
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

        public override string GetTitle() {
            return "Ranged Strategy";
        }
    }
}