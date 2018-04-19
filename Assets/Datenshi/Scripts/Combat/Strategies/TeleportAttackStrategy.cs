using System;
using System.Collections;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Entities.Input;
using Datenshi.Scripts.Misc;
using UnityEngine;

namespace Datenshi.Scripts.Combat.Strategies {
    [CreateAssetMenu(menuName = "Datenshi/AI/Combat/Strategy/TeleportAttackStrategy")]
    public class TeleportAttackStrategy : AttackStrategy {
        public float MinDistance = 5F;
        public float TeleportDistance = 7;
        public float TeleportThreshold = 2F;
        public float TeleportDuration = 5;
        public string StartTeleportKey = "TeleportBegin";
        public string EndTeleportKey = "TeleportEnd";

        public static readonly Variable<bool> Teleporting =
            new Variable<bool>("entity.combat.strategy.teleport.teleporting", false);

        public override void Execute(AIStateInputProvider provider, LivingEntity e, LivingEntity target) {
            var entity = e as MovableEntity;
            if (entity == null) {
                return;
            }

            if (entity.GetVariable(Teleporting)) {
                provider.Reset();
                return;
            }

            Vector2 targetEntityPos = target.transform.position;

            var entityPos = entity.transform.position;
            var xDir = Math.Sign(entityPos.x - targetEntityPos.x);
            var targetPos = entity.AIAgent.GetFavourablePosition(this, target);
            var distance = Vector2.Distance(entityPos, targetPos);
            Debug.LogFormat("Distance from {0} {1} to {2} {3} = {4} / {5}", entity, entityPos, target, targetEntityPos,
                distance, TeleportThreshold);
            if (distance < TeleportThreshold) {
                var newPos = entityPos;
                var teleportDir = xDir * TeleportDistance;
                newPos.x += teleportDir;
                var room = entity.EntityRoom;
                if (room != null && room.IsOutInBounds(newPos)) {
                    newPos.x -= 2 * teleportDir;
                }

                entity.StartCoroutine(Teleport(entity, newPos));
                return;
            }

            if (distance > MinDistance) {
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

        private IEnumerator Teleport(MovableEntity entity, Vector2 pos) {
            entity.SetVariable(Teleporting, true);
            entity.Invulnerable = true;
            Debug.Log("Teleport begin");
            var updater = entity.AnimatorUpdater.Animator;
            updater.SetTrigger(StartTeleportKey);
            yield return new WaitForSeconds(TeleportDuration);
            entity.transform.position = pos;
            updater.SetTrigger(EndTeleportKey);
            Debug.Log("Teleport end");
            entity.Invulnerable = false;
            entity.SetVariable(Teleporting, false);
        }

        public override float GetMinimumDistance(LivingEntity entity, LivingEntity target) {
            return MinDistance;
        }
    }
}