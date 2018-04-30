using System;
using System.Collections;
using Datenshi.Scripts.Debugging;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
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
        public bool TeleportIfClose;

        public static readonly Variable<bool> Teleporting =
            new Variable<bool>("entity.combat.strategy.teleport.teleporting", false);

        public override void Execute(AIStateInputProvider provider, LivingEntity entity, LivingEntity target, DebugInfo info) {
            if (entity.GetVariable(Teleporting)) {
                provider.Reset();
                return;
            }

            var entityPos = entity.Center;
            var movable = entity as MovableEntity;
            var navigator = movable == null ? null : movable.AINavigator;
            var targetCenter = target.Center;
            var targetPos = navigator == null ? targetCenter : navigator.GetFavourablePosition(this, target);
#if UNITY_EDITOR
            var thresholdPoint = targetCenter - entityPos;
            thresholdPoint.Normalize();
            thresholdPoint *= TeleportThreshold;
            thresholdPoint += entityPos;
#endif
            var xDir = Math.Sign(entityPos.x - targetPos.x);
            var distance = Vector2.Distance(entityPos, targetCenter);
            if (TeleportIfClose && distance < TeleportThreshold) {
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
#if UNITY_EDITOR
                Debug.DrawLine(entityPos, targetPos, Color.yellow);
                Debug.DrawLine(entityPos, thresholdPoint, Color.magenta);
#endif
                provider.Attack = false;
                if (navigator == null) {
                    return;
                }

                navigator.Target = targetPos;
                navigator.Execute(movable, provider);

                return;
            }

            Debug.DrawLine(entityPos, targetPos, Color.green);
            Debug.DrawLine(entityPos, thresholdPoint, Color.magenta);
            entity.CurrentDirection.X = -xDir;
            provider.Horizontal = 0;
            provider.Walk = true;
            provider.Jump = false;
            provider.Attack = true;
        }

        private IEnumerator Teleport(LivingEntity entity, Vector2 pos) {
            entity.SetVariable(Teleporting, true);
            entity.Invulnerable = true;
            var updater = entity.AnimatorUpdater.Animator;
            updater.SetTrigger(StartTeleportKey);
            yield return new WaitForSeconds(TeleportDuration);
            entity.transform.position = pos;
            updater.SetTrigger(EndTeleportKey);
            entity.Invulnerable = false;
            entity.SetVariable(Teleporting, false);
        }

        public override float GetMinimumDistance(LivingEntity entity, LivingEntity target) {
            return MinDistance;
        }

        public override float GetCost(LivingEntity entity, LivingEntity target) {
            return 0;
        }

        public override float GetEffectiveness(LivingEntity entity, LivingEntity target) {
            return MinDistance - Vector2.Distance(entity.Center, target.Center);
        }

        public override string GetTitle() {
            return "Teleport Strategy";
        }
    }
}