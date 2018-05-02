using System;
using System.Collections;
using UnityEngine;

namespace Datenshi.Scripts.AI.Traits {
    //TODO fix
/*    public class TeleportTrait : Trait {
        private bool teleporting;
        public AIStateInputProvider provider;
        public LivingINavigable INavigable;
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

        private void OnShot(Projectile arg0, LivingINavigable arg1, LivingINavigable arg2) {
            if (arg2 == INavigable) {
                StartCoroutine(Teleport(INavigable.transform.position, TeleportShotDuration));
            }
        }

        private IEnumerator Teleport(Vector2 pos, float duration) {
            teleporting = true;
            INavigable.Invulnerable = true;
            var updater = INavigable.AnimatorUpdater.Animator;
            updater.SetTrigger(StartTeleportKey);
            yield return new WaitForSeconds(duration);
            INavigable.transform.position = pos;
            updater.SetTrigger(EndTeleportKey);
            INavigable.Invulnerable = false;
            teleporting = false;
        }

        public override void Execute() {
            provider.ExecuteState = !teleporting;
            if (teleporting) {
                provider.Reset();
                return;
            }

            var entityPos = INavigable.Center;
            var movable = INavigable as MovableINavigable;
            var navigator = movable == null ? null : movable.AINavigator;
            var target = INavigable.GetVariable(CombatVariables.INavigableTarget);
            if (target == null) {
                return;
            }

            var targetCenter = target.Center;
            var targetPos = navigator == null ? targetCenter : navigator.GetFavourablePosition(INavigable.DefaultAttackStrategy, target);
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
                var room = INavigable.INavigableRoom;
                if (room != null && room.IsOutInBounds(newPos)) {
                    newPos.x -= 2 * teleportDir;
                }

                INavigable.StartCoroutine(Teleport(newPos, TeleportCloseDuration));
                return;
            }
        }
    }*/
}