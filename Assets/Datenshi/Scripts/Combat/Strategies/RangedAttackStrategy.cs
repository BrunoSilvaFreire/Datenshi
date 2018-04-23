using System;
using Datenshi.Input;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/RangedAttackStrategy")]
    public class RangedAttackStrategy : AttackStrategy {
        public float MinDistance = 5F;
        public float Threshold = 1F;
        public float MinDelayBetweenAttacks = 2;

        public static readonly Variable<float> LastAttack =
            new Variable<float>("entity.ai.combat.strategy.ranged.lastAttack", 0);

        public override void Execute(AIStateInputProvider provider, LivingEntity e, LivingEntity target) {
            var entity = e as MovableEntity;
            if (entity == null) {
                return;
            }
            Vector2 targetEntityPos;
            var movableEntity = target as MovableEntity;
            if (movableEntity != null) {
                targetEntityPos = movableEntity.GroundPosition;
            } else {
                targetEntityPos = target.transform.position;
            }
            var entityPos = entity.GroundPosition;
            var xDir = Math.Sign(entityPos.x - targetEntityPos.x);
            var agent = entity.AINavigator;
            var targetPos = agent.GetFavourablePosition(this, target);
            if (Vector2.Distance(entityPos, targetPos) > MinDistance + Threshold) {
                agent.Target = targetPos;
                agent.Execute(entity, provider);
                provider.Attack = false;
                Debug.Log("Too distant");
                return;
            }
            var lastAttack = target.GetVariable(LastAttack);
            var time = Time.time;
            if (time - lastAttack < MinDelayBetweenAttacks) {
                provider.Attack = false;
                Debug.Log("Too early for new attack" + (time - lastAttack) + " / " + MinDelayBetweenAttacks);
                return;
            }

            target.SetVariable(LastAttack, time);
            Debug.Log("Attacking");
            entity.CurrentDirection.X = -xDir;
            provider.Horizontal = 0;
            provider.Walk = true;
            provider.Jump = false;
            provider.Attack = true;
        }

        public override float GetMinimumDistance(LivingEntity entity, LivingEntity target) {
            return MinDistance;
        }
    }
}