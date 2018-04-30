using System;
using System.Collections;
using Datenshi.Scripts.Combat;
using Datenshi.Scripts.Combat.Attacks.Ranged;
using Datenshi.Scripts.Entities;
using Datenshi.Scripts.Input;
using UnityEngine;

namespace Datenshi.Scripts.AI.Traits {
    public class TeleportTrait : Trait {
        private bool teleporting;
        public AIStateInputProvider provider;
        public LivingEntity Entity;
        public float MinDistance = 5F;
        public float TeleportDistance = 7;
        public float TeleportThreshold = 2F;
        public float TeleportCloseDuration = 2;
        public float TeleportShotDuration = 5;
        public bool TeleportIfClose;
        public bool TeleportIfWasShot;

        public string StartTeleportKey = "TeleportBegin";
        public string EndTeleportKey = "TeleportEnd";

        private void Start() {
            if (TeleportIfWasShot) {
                ProjectileShotEvent.Instance.AddListener(OnShot);
            }
        }

        private void OnShot(Projectile arg0, LivingEntity arg1, LivingEntity arg2) {
            if (arg2 == Entity) {
                StartCoroutine(Teleport(Entity.transform.position, TeleportShotDuration));
            }
        }

        private IEnumerator Teleport(Vector2 pos, float duration) {
            teleporting = true;
            Entity.Invulnerable = true;
            var updater = Entity.AnimatorUpdater.Animator;
            updater.SetTrigger(StartTeleportKey);
            yield return new WaitForSeconds(duration);
            Entity.transform.position = pos;
            updater.SetTrigger(EndTeleportKey);
            Entity.Invulnerable = false;
            teleporting = false;
        }

        public override void Execute() {
            provider.ExecuteState = !teleporting;
            if (teleporting) {
                provider.Reset();
                return;
            }

            var entityPos = Entity.Center;
            var movable = Entity as MovableEntity;
            var navigator = movable == null ? null : movable.AINavigator;
            var target = Entity.GetVariable(CombatVariables.EntityTarget);
            if (target == null) {
                return;
            }

            var targetCenter = target.Center;
            var targetPos = navigator == null ? targetCenter : navigator.GetFavourablePosition(Entity.DefaultAttackStrategy, target);
#if UNITY_EDITOR
            var thresholdPoint = targetCenter - entityPos;
            thresholdPoint.Normalize();
            thresholdPoint *= TeleportThreshold;
            thresholdPoint += entityPos;
            Debug.DrawLine(entityPos, targetCenter, Color.green);
            Debug.DrawLine(entityPos, thresholdPoint, Color.magenta);
#endif
            var xDir = Math.Sign(entityPos.x - targetPos.x);
            var distance = Vector2.Distance(entityPos, targetCenter);
            if (TeleportIfClose && distance < TeleportThreshold) {
                var newPos = entityPos;
                var teleportDir = xDir * TeleportDistance;
                newPos.x += teleportDir;
                var room = Entity.EntityRoom;
                if (room != null && room.IsOutInBounds(newPos)) {
                    newPos.x -= 2 * teleportDir;
                }

                Entity.StartCoroutine(Teleport(newPos, TeleportCloseDuration));
                return;
            }
        }
    }
}