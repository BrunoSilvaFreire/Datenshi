using System;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/RangedAttackStrategy")]
    public class RangedAttackStrategy : AttackStrategy {
        public float MinDistance = 5F;
        public float Threshold = 1F;

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
            var targetPos = entity.AIAgent.GetFavourablePosition(this, target);
            if (Vector2.Distance(entityPos, targetPos) > Threshold) {
                provider.Attack = false;
                var agent = entity.AIAgent;
                agent.Target = targetPos;
                agent.Execute(entity, provider);
                return;
            }
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