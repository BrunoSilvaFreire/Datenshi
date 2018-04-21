using Datenshi.Input;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/CQBAttackStrategy")]
    public class CQBAttackStategy : AttackStrategy {
        public static readonly Variable<float> LastAttack =
            new Variable<float>("entity.ai.combat.strategy.lastAttack", 0);

        public float MinDistance = 1F;
        public float MinDelayBetweenAttacks = 1F;

        public override void Execute(AIStateInputProvider provider, LivingEntity e, LivingEntity target) {
            var entity = e as MovableEntity;
            if (entity == null) {
                return;
            }
            Vector2 targetPos;
            var movableEntity = target as MovableEntity;
            if (movableEntity != null) {
                targetPos = movableEntity.GroundPosition;
            } else {
                targetPos = target.transform.position;
            }

            if (Vector2.Distance(entity.transform.position, targetPos) > MinDistance) {
                provider.Attack = false;
                var agent = entity.AINavigator;
                agent.Target = targetPos;
                agent.Execute(entity, provider);
                return;
            }

            var lastAttack = target.GetVariable(LastAttack);
            var time = Time.time;
            if (time - lastAttack < MinDelayBetweenAttacks) {
                return;
            }

            target.SetVariable(LastAttack, time);
            provider.Horizontal = 0;
            provider.Vertical = 0;
            provider.Jump = false;
            provider.Attack = true;
        }

        public override float GetMinimumDistance(LivingEntity entity, LivingEntity target) {
            return MinDistance;
        }
    }
}